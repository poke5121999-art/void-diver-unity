using System.Collections.Generic;
using System;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GamePingManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameChatManager _gameChatManager;

	private const int PingLimitCount = 10;

	private const float PingLimitSeconds = 5f;

	private Queue<float> _pingTimestamps = new Queue<float>();

	private float _pingBlockEndTime;

	public readonly Subject<(EPingType pingType, Vector2 inputPosition)> OnPingTriggered = new Subject<(EPingType pingType, Vector2 inputPosition)>();

	private bool IsPingBlocked => Time.time < _pingBlockEndTime;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Subscribe(delegate
		{
			OnStageStarted();
		}).AddTo(this);
		_gameChannel.Receive<NtfPing>().Subscribe(OnPing).AddTo(this);
	}

	private void OnStageStarted()
	{
	}

	public void TriggerPing(EPingType pingType, Vector2 inputPosition)
	{
		OnPingTriggered.OnNext((pingType, inputPosition));
	}

	public EPingResult TrySendPingByScreenPoint(EPingType pingType, Vector2 screenPoint)
	{
		EPingTargetType targetType = EPingTargetType.None;
		int value1 = 0;
		int value2 = 0;
		if (!TryScreenPointToWorldPosition(screenPoint, out var position, out var go))
		{
			return EPingResult.RestrictedArea;
		}
		if (pingType == EPingType.Notice || pingType == EPingType.Alert)
		{
			if (go.TryGetComponent<MonsterModel>(out var monster))
			{
				targetType = EPingTargetType.Monster;
				value1 = monster.TableId;
			}
			else if (go.TryGetComponent<RewardBox>(out var rewardBox))
			{
				targetType = EPingTargetType.RewardBox;
				value1 = rewardBox.RewardBoxId;
			}
			else if (go.TryGetComponent<MimicObject>(out var mimic))
			{
				targetType = EPingTargetType.Mimic;
				value1 = mimic.MimicId;
			}
			else if (go.TryGetComponent<DropGoods>(out var dropGoods))
			{
				targetType = EPingTargetType.DropGoods;
				value1 = (int)dropGoods.GoodsType;
				value2 = dropGoods.GoodsId;
			}
			else if (go.TryGetComponent<TrapObject>(out var trap))
			{
				targetType = EPingTargetType.Trap;
				value1 = trap.TrapId;
			}
			else if (go.TryGetComponent<Entrance>(out var entrance))
			{
				targetType = EPingTargetType.Entrance;
				value1 = entrance.EntranceId;
			}
			else if (go.TryGetComponent<NpcObject>(out var npc))
			{
				targetType = EPingTargetType.Npc;
				value1 = npc.NpcId;
			}
		}
		return TrySendPing(pingType, position, targetType, value1, value2);
	}

	public EPingResult TrySendPingByPosition(EPingType pingType, Vector3 position)
	{
		GameObject go = null;
		if (PhysicsUtility.Raycast(position, out go, Const.LayerMask.PLANE))
		{
			return TrySendPing(pingType, position);
		}
		return EPingResult.RestrictedArea;
	}

	public EPingResult TrySendPingByItem(int itemId)
	{
		return TrySendPing(EPingType.Item, default(Vector3), EPingTargetType.None, itemId);
	}

	public EPingResult TrySendPingBySkill(int skillId)
	{
		return TrySendPing(EPingType.Skill, default(Vector3), EPingTargetType.None, skillId);
	}

	public EPingResult TrySendPingByStatus()
	{
		return TrySendPing(EPingType.Status);
	}

	private EPingResult TrySendPing(EPingType pingType, Vector3 position = default(Vector3), EPingTargetType targetType = EPingTargetType.None, int value1 = 0, int value2 = 0)
	{
		if (Time.time < _pingBlockEndTime)
		{
			return EPingResult.Blocked;
		}
		_gameChannel.SendToHost(new ReqPing
		{
			PingType = pingType,
			Position = position.ToProtoVector3(),
			Value1 = value1,
			Value2 = value2,
			TargetType = targetType
		});
		float time = Time.time;
		while (_pingTimestamps.Count > 0 && time - _pingTimestamps.Peek() > 5f)
		{
			_pingTimestamps.Dequeue();
		}
		_pingTimestamps.Enqueue(time);
		if (_pingTimestamps.Count >= 10)
		{
			_pingBlockEndTime = time + 5f;
			_pingTimestamps.Clear();
		}
		return EPingResult.Ok;
	}

	private void OnPing(NtfPing message)
	{
		CharacterModel characterModel = _gameManager.FindCharacterModel(message.ClientId);
		if (characterModel == null)
		{
			return;
		}
		PlayPing(characterModel.PlayerName, message.PingType, message.Position.ToVector3(), message.TargetType, characterModel, message.Value1, message.Value2);
	}

	public void PlayPing(string senderName, EPingType pingType, Vector3 position, EPingTargetType targetType = EPingTargetType.None, CharacterModel characterModel = null, int value1 = 0, int value2 = 0)
	{
		if (pingType.IsPointPing())
		{
			_objectManager.SpawnPingObject(pingType, position);
		}
		string pingNoticeText = GetPingNoticeText(senderName, characterModel, pingType, targetType, value1, value2);
		_gameChatManager.AddChat(new ChatInfo
		{
			Type = EChatType.Ping,
			Text = pingNoticeText,
			UtcTime = DateTime.UtcNow
		});
		_soundManager.PlaySfx(GetPingSoundName(pingType));
	}

	private bool TryScreenPointToWorldPosition(Vector3 point, out Vector3 position, out GameObject go)
	{
		position = Vector3.zero;
		go = null;
		Camera mainCamera = _cameraManager.MainCamera;
		if (Physics.Raycast(mainCamera.ScreenPointToRay(point), out var hitInfo, 1000f, Const.LayerMask.Ping | Const.LayerMask.PLANE))
		{
			GameObject gameObject = hitInfo.collider.gameObject;
			go = ((gameObject.layer == Const.Layer.PLANE) ? gameObject : gameObject.transform.parent.gameObject);
			position = ((gameObject.layer == Const.Layer.PLANE) ? hitInfo.point : go.transform.position);
			return true;
		}
		return false;
	}

	private static string GetPingNoticeText(string senderName, CharacterModel characterModel, EPingType pingType, EPingTargetType targetType, int value1, int value2)
	{
		string text;
		switch (targetType)
		{
		case EPingTargetType.Monster:
			text = TMonster.Table[value1].Name ?? "";
			break;
		case EPingTargetType.RewardBox:
			text = TRewardBox.Table[value1].Name ?? "";
			break;
		case EPingTargetType.DropGoods:
		{
			EGoodsCategory eGoodsCategory = ((EGoodsType)value1).GoodsCategory();
			switch (eGoodsCategory)
			{
			case EGoodsCategory.Item:
				text = TItem.Table[value2].Name ?? "";
				break;
			case EGoodsCategory.Equipment:
				text = TEquipment.Table[value2].Name ?? "";
				break;
			case EGoodsCategory.Bag:
				text = TBag.Table[value2].Name ?? "";
				break;
			default:
				text = "";
				break;
			}
			break;
		}
		case EPingTargetType.Trap:
			text = TTrap.Table[value1].Name ?? "";
			break;
		case EPingTargetType.Entrance:
			text = TEntrance.Table[value1].Name ?? "";
			break;
		case EPingTargetType.Npc:
			text = TNpc.Table[value1].Name ?? "";
			break;
		case EPingTargetType.Mimic:
			text = TMimic.Table[value1].Name ?? "";
			break;
		default:
			text = "";
			break;
		}
		switch (pingType)
		{
		case EPingType.Notice:
			if (!string.IsNullOrEmpty(text))
			{
				return string.Format(TLocalizedText.PingNoticeObjectFormat.LocalText, senderName, text);
			}
			return string.Format(TLocalizedText.PingNoticeFormat.LocalText, senderName);
		case EPingType.Alert:
			if (!string.IsNullOrEmpty(text))
			{
				return string.Format(TLocalizedText.PingAlertObjectFormat.LocalText, senderName, text);
			}
			return string.Format(TLocalizedText.PingAlertFormat.LocalText, senderName);
		case EPingType.Item:
		{
			TItem tItem = TItem.Table[value1];
			long itemCount = characterModel.GetComponent<Inventory>().GetItemCount(value1);
			float remainConsumableTypeCoolTime = characterModel.GetRemainConsumableTypeCoolTime(tItem.ConsumableType);
			return string.Format(TLocalizedText.PingItemFormat.LocalText, senderName, tItem.Name, itemCount, string.Format("{0:F1}", remainConsumableTypeCoolTime));
		}
		case EPingType.Skill:
		{
			float remainCoolTime = characterModel.GetRemainCoolTime(value1);
			return string.Format(TLocalizedText.PingSkillFormat.LocalText, senderName, TSkill.Table[value1].Name, string.Format("{0:F1}", remainCoolTime));
		}
		case EPingType.Status:
			return string.Format(TLocalizedText.PingStatusFormat.LocalText, senderName, string.Format("{0}/{1}", characterModel.Hp, characterModel.HpMax), string.Format("{0}/{1}", characterModel.Stress, TConst.StressMax.IntValue));
		default:
			return string.Format(TLocalizedText.PingTypeFormat.LocalText, senderName, pingType.ToLocalText());
		}
	}

	private string GetPingSoundName(EPingType pingType)
	{
		return string.Format("{0}{1}", "EPingType", pingType);
	}
}

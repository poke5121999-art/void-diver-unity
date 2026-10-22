using System.Collections.Generic;
using GameCommon.Enum;
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

	private Queue<float> _pingTimestamps;

	private float _pingBlockEndTime;

	public readonly Subject<(EPingType pingType, Vector2 inputPosition)> OnPingTriggered;

	private bool IsPingBlocked => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageStarted()
	{
	}

	public void TriggerPing(EPingType pingType, Vector2 inputPosition)
	{
	}

	public EPingResult TrySendPingByScreenPoint(EPingType pingType, Vector2 screenPoint)
	{
		return EPingResult.Ok;
	}

	public EPingResult TrySendPingByPosition(EPingType pingType, Vector3 position)
	{
		return EPingResult.Ok;
	}

	public EPingResult TrySendPingByItem(int itemId)
	{
		return EPingResult.Ok;
	}

	public EPingResult TrySendPingBySkill(int skillId)
	{
		return EPingResult.Ok;
	}

	public EPingResult TrySendPingByStatus()
	{
		return EPingResult.Ok;
	}

	private EPingResult TrySendPing(EPingType pingType, Vector3 position = default(Vector3), EPingTargetType targetType = EPingTargetType.None, int value1 = 0, int value2 = 0)
	{
		return EPingResult.Ok;
	}

	private void OnPing(NtfPing message)
	{
	}

	public void PlayPing(string senderName, EPingType pingType, Vector3 position, EPingTargetType targetType = EPingTargetType.None, CharacterModel characterModel = null, int value1 = 0, int value2 = 0)
	{
	}

	private bool TryScreenPointToWorldPosition(Vector3 point, out Vector3 position, out GameObject go)
	{
		position = default;
		go = null;
		return false;
	}

	private static string GetPingNoticeText(string senderName, CharacterModel characterModel, EPingType pingType, EPingTargetType targetType, int value1, int value2)
	{
		return null;
	}

	private string GetPingSoundName(EPingType pingType)
	{
		return null;
	}
}

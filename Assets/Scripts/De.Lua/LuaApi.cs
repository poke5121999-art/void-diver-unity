using System;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using MoonSharp.Interpreter;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

[MoonSharpUserData]
public class LuaApi : NativeBase
{
	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameSoundManager _gameSoundManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GamePingManager _gamePingManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameCutsceneManager _cutsceneManager;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	private int CampaignId => _gameContext.CampaignId;

	private Inventory MyInventory => _gameManager.MyInventory;

	public void LogDebug(string message)
	{
		_logger.Debug(string.Format("[{0:D5}] {1}", _gameContext.CampaignId, message));
	}

	public void LogInfo(string message)
	{
		_logger.Info(string.Format("[{0:D5}] {1}", _gameContext.CampaignId, message));
	}

	public void LogWarning(string message)
	{
		_logger.Warning(string.Format("[{0:D5}] {1}", _gameContext.CampaignId, message));
	}

	public void LogError(string message)
	{
		_logger.Error(string.Format("[{0:D5}] {1}", _gameContext.CampaignId, message));
	}

	public bool IsHost()
	{
		return _gameManager.IsHost;
	}

	public bool IsTutorial()
	{
		return _gameContext.IsTutorial;
	}

	public void SetIsTutorial(bool isTutorial)
	{
		_gameContext.IsTutorial = isTutorial;
	}

	public void SendTutorialEvent(string eventName)
	{
		_eventSystem.Trigger(new TutorialEvent(eventName));
	}

	public bool IsTaskAchieved(int campaignTaskId)
	{
		return _campaignManager.IsTaskAchieved(campaignTaskId);
	}

	public bool IsAllTaskAchieved()
	{
		return _campaignManager.IsAllTaskAchieved();
	}

	public int GetStep()
	{
		return _campaignManager.StepId;
	}

	public void SetStep(int stepId)
	{
		_campaignManager.SetQuestStepId(stepId);
	}

	public int GetCharacterId()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return 0;
		}
		return myCharacterModel.CharacterId;
	}

	public int GetCampaignId()
	{
		return _campaignManager.CampaignId;
	}

	public Vector3 GetCharacterPosition()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			_logger.Warning("MyCharacterModel not found.");
			return Vector3.zero;
		}
		return myCharacterModel.Position;
	}

	public Vector3 GetCharacterForward()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			_logger.Warning("MyCharacterModel not found.");
			return Vector3.zero;
		}
		return myCharacterModel.Forward;
	}

	public void SetCharacterPosition(DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqSetCharacterPosition
		{
			Position = position.ToProtoVector3()
		});
	}

	public void SetCharacterForward(DynValue dynForward)
	{
		Vector3 forward = new Vector3((float)dynForward.Table.Get("x").Number, (float)dynForward.Table.Get("y").Number, (float)dynForward.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqSetCharacterForward
		{
			Forward = forward.ToProtoVector3()
		});
	}

	public void AddCharacterBuff(int buffId)
	{
		if (_gameManager.MyCharacterController == null)
		{
			_logger.Warning("MyCharacterController not found.");
			return;
		}
		_gameChannel.SendToHost(new ReqAddLuaBuff
		{
			BuffId = buffId
		});
	}

	public Vector3 GetZoneSpawnPosition(int zoneSpawnId)
	{
		return _objectManager.GetZoneSpawnPosition(zoneSpawnId);
	}

	public bool IsClearedCampaign(int campaignId)
	{
		return _playerManager.IsClearedCampaign(campaignId);
	}

	public int GetQuestState()
	{
		return (int)_campaignManager.State;
	}

	public void SetQuestState(ECampaignState state)
	{
		switch (state)
		{
		case ECampaignState.NotStarted:
		case ECampaignState.InProgress:
		case ECampaignState.NotCompleted:
			_campaignManager.SetCampaignState(state);
			break;
		case ECampaignState.Completed:
			_campaignManager.CompleteQuest();
			break;
		case ECampaignState.Failed:
			_campaignManager.FailQuest();
			break;
		}
	}

	public EMissionState GetMissionState(int missionId)
	{
		return _missionManager.GetMissionState(missionId);
	}

	public void SetMissionState(int missionId, EMissionState state)
	{
		switch (state)
		{
		case EMissionState.NotStarted:
		case EMissionState.InProgress:
		case EMissionState.NotCompleted:
		case EMissionState.Failed:
			_missionManager.SetMissionState(missionId, state);
			break;
		case EMissionState.Completed:
			_missionManager.CompleteMission(missionId);
			break;
		}
	}

	public void SetProgress(int key, int progress)
	{
		_eventSystem.Trigger(new LuaProgressEvent(key, progress));
	}

	public int GetMissionStep(int missionId)
	{
		return _missionManager.GetMissionStep(missionId);
	}

	public void SetMissionStep(int missionId, int stepId)
	{
		_missionManager.SetMissionStep(missionId, stepId);
	}

	public ELoungeQuestState GetLoungeQuestState(int loungeQuestId)
	{
		return _loungeQuestManager.GetQuestState(loungeQuestId);
	}

	public void SetLoungeQuestState(int loungeQuestId, ELoungeQuestState state)
	{
		switch (state)
		{
		case ELoungeQuestState.NotStarted:
			_loungeQuestManager.AddQuest(loungeQuestId);
			break;
		case ELoungeQuestState.InProgress:
		case ELoungeQuestState.NotCompleted:
			_loungeQuestManager.SetQuestState(loungeQuestId, state);
			break;
		case ELoungeQuestState.Completed:
			_loungeQuestManager.CompleteQuest(loungeQuestId);
			break;
		}
	}

	public bool IsAllLoungeQuestTaskCompleted(int loungeQuestId)
	{
		return _loungeQuestManager.IsAllQuestTaskCompleted(loungeQuestId);
	}

	public void ShowBubbleText(ESpeaker speakerType, int tableId, string text)
	{
		string value = ConvertToLocalText(text);
		Subject<string> subject;
		switch (speakerType)
		{
		case ESpeaker.Player:
		{
			CharacterController myCharacterController = _gameManager.MyCharacterController;
			if (myCharacterController == null)
			{
				_logger.Warning("MyCharacterController not found.");
				return;
			}
			subject = _gameManager.MyCharacterController.OnShowBubbleText;
			break;
		}
		case ESpeaker.Npc:
		{
			NpcObject npcObject = _objectManager.FindNpc(tableId);
			if (npcObject == null)
			{
				_logger.Warning(string.Format("Npc ID ({0}) not found.", tableId));
				return;
			}
			subject = npcObject.OnShowBubbleText;
			break;
		}
		case ESpeaker.Monster:
		{
			MonsterController monsterController = _objectManager.FindMonsterControllerByMonsterId(tableId);
			if (monsterController == null)
			{
				_logger.Warning(string.Format("Monster ID ({0}) not found.", tableId));
				return;
			}
			subject = monsterController.OnShowBubbleText;
			break;
		}
		default:
			_logger.Warning(string.Format("Unknown speaker type: {0}", speakerType));
			return;
		}
		subject.OnNext(value);
	}

	public void ShowMonologueText(ESpeaker speakerType, int tableId, string text)
	{
		string value = ConvertToLocalText(text);
		Subject<string> subject;
		switch (speakerType)
		{
		case ESpeaker.Player:
		{
			CharacterController myCharacterController = _gameManager.MyCharacterController;
			if (myCharacterController == null)
			{
				_logger.Warning("MyCharacterController not found.");
				return;
			}
			subject = _gameManager.MyCharacterController.OnShowMonologueText;
			break;
		}
		case ESpeaker.Npc:
		{
			NpcObject npcObject = _objectManager.FindNpc(tableId);
			if (npcObject == null)
			{
				_logger.Warning(string.Format("Npc ID ({0}) not found.", tableId));
				return;
			}
			subject = npcObject.OnShowMonologueText;
			break;
		}
		case ESpeaker.Monster:
		{
			MonsterController monsterController = _objectManager.FindMonsterControllerByMonsterId(tableId);
			if (monsterController == null)
			{
				_logger.Warning(string.Format("Monster ID ({0}) not found.", tableId));
				return;
			}
			subject = monsterController.OnShowMonologueText;
			break;
		}
		default:
			_logger.Warning(string.Format("Unknown speaker type: {0}", speakerType));
			return;
		}
		subject.OnNext(value);
	}

	public void ShowRadioText(ESpeaker speakerType, int tableId, string imageName, string text)
	{
		string message = ConvertToLocalText(text);
		if (speakerType == ESpeaker.Player)
		{
			if (_gameManager.MyCharacterModel == null)
			{
				return;
			}
			tableId = _gameManager.MyCharacterModel.CharacterId;
		}
		_toastManager.Show(new RadioToastModel
		{
			Speaker = speakerType,
			TableId = tableId,
			ImageName = imageName,
			Message = message
		});
	}

	public TaskWrapper OpenDialogAsync(float backgroundAlpha = 0.5f)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		_popupManager.OpenAsync<DialogPopupPresenter>(new DialogPopupModel()).ContinueWith(delegate(DialogPopupPresenter presenter)
		{
			presenter.SetBackgroundAlpha(backgroundAlpha);
			taskWrapper.IsCompleted = true;
		}).Forget();
		return taskWrapper;
	}

	public TaskWrapper AppendDialogAsync(DialogSpeakerEntry[] speakers, int speakerIndex, string message, string[] selectableMessages)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			taskWrapper.IsCompleted = true;
			return taskWrapper;
		}
		DialogEntry dialogEntry = new DialogEntry();
		dialogEntry.Speakers = speakers;
		dialogEntry.SpeakerIndex = speakerIndex;
		dialogEntry.Message = ConvertToLocalText(message);
		dialogEntry.SelectableMessageDatas = selectableMessages?.Select((string m) => new SelectableMessageData
		{
			Message = ConvertToLocalText(m)
		}).ToArray();
		dialogEntry.OnCompleted.AsObservable().Subscribe(delegate(int result)
		{
			taskWrapper.IsCompleted = true;
			taskWrapper.Result = result;
			_dialogReplayService.SetDialogReplayRecord(message, result);
		});
		dialogPopupPresenter.AppendDialogText(dialogEntry);
		return taskWrapper;
	}

	public TaskWrapper SetDialogCustomImageAsync(string image, bool isFront, int index)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("SetDialogCustomBackgroundAsync: DialogPopupPresenter 없음");
			taskWrapper.IsCompleted = true;
			return taskWrapper;
		}
		dialogPopupPresenter.SetCustomImageAsync(image, isFront, index).ContinueWith(delegate
		{
			taskWrapper.IsCompleted = true;
		}).Forget();
		return taskWrapper;
	}

	public TaskWrapper ClearDialogCustomImageAsync(bool isFront, int index)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("ClearDialogCustomBackgroundAsync: DialogPopupPresenter 없음");
			taskWrapper.IsCompleted = true;
			return taskWrapper;
		}
		dialogPopupPresenter.ClearCustomImageAsync(isFront, index).ContinueWith(delegate
		{
			taskWrapper.IsCompleted = true;
		}).Forget();
		return taskWrapper;
	}

	public TaskWrapper ClearAllDialogCustomImagesAsync()
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("ClearDialogCustomBackgroundAsync: DialogPopupPresenter 없음");
			taskWrapper.IsCompleted = true;
			return taskWrapper;
		}
		dialogPopupPresenter.ClearAllCustomImagesAsync().ContinueWith(delegate
		{
			taskWrapper.IsCompleted = true;
		}).Forget();
		return taskWrapper;
	}

	public void FadeInDialog()
	{
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("FadeInDialogue: DialogPopupPresenter 없음");
		}
		else
		{
			dialogPopupPresenter.FadeInFrame();
		}
	}

	public void FadeOutDialog()
	{
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("FadeOutDialogue: DialogPopupPresenter 없음");
		}
		else
		{
			dialogPopupPresenter.FadeOutFrame();
		}
	}

	public void SetDialogImmediate(DialogSpeakerEntry[] speakers, int speakerIndex, string message)
	{
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			_logger.Warning("SetDialogImmediate: DialogPopupPresenter 없음");
			return;
		}
		dialogPopupPresenter.SetDialogImmediate(new DialogEntry
		{
			Speakers = speakers,
			SpeakerIndex = speakerIndex,
			Message = ConvertToLocalText(message)
		});
	}

	public TaskWrapper CloseDialogAsync()
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
		if (dialogPopupPresenter == null)
		{
			taskWrapper.IsCompleted = true;
		}
		else
		{
			dialogPopupPresenter.SetClosable();
			dialogPopupPresenter.OnClose.AsObservable().Subscribe(delegate
			{
				taskWrapper.IsCompleted = true;
			});
		}
		return taskWrapper;
	}

	public void SetNpcState(int npcId, ENpcState npcState)
	{
		NpcObject npcObject = _objectManager.FindNpc(npcId);
		if (npcObject == null)
		{
			_logger.Warning(string.Format("Npc ID ({0}) not found.", npcId));
		}
		else
		{
			npcObject.RxNpcState.Value = npcState;
		}
	}

	public void AddDialog(int npcId, DynValue dynDialogInfo)
	{
		string text = dynDialogInfo.Table.Get("Type").String ?? "None";
		if (!Enum.TryParse<EDialogType>(text, true, out var result))
		{
			_logger.Warning(string.Concat("AddDialog: Invalid Type '", text, "'. Using None."));
			result = EDialogType.None;
		}
		string text2 = dynDialogInfo.Table.Get("Purpose").String ?? "None";
		if (!Enum.TryParse<EDialogPurpose>(text2, true, out var result2))
		{
			_logger.Warning(string.Concat("AddDialog: Invalid Purpose '", text2, "'. Using None."));
			result2 = EDialogPurpose.None;
		}
		string text3 = dynDialogInfo.Table.Get("Title").String ?? "";
		string luaKey = dynDialogInfo.Table.Get("LuaKey").String ?? "";
		string functionName = dynDialogInfo.Table.Get("FunctionName").String ?? "";
		string key = string.Concat(luaKey, "/", functionName);
		DialogInfo dialogInfo = new DialogInfo();
		dialogInfo.Key = key;
		if (!string.IsNullOrEmpty(text3))
		{
			text3 = ConvertToLocalText(text3);
		}
		dialogInfo.Title = text3;
		dialogInfo.Type = result;
		dialogInfo.Priority = (int)dynDialogInfo.Table.Get("Priority").Number;
		dialogInfo.Purpose = result2;
		dialogInfo.OnClicked = delegate
		{
			_eventSystem.Trigger(new LuaDialogEvent(luaKey, functionName));
		};
		_npcDialogManager.AddDialog(npcId, dialogInfo);
	}

	public void RemoveDialog(int npcId, string luaKey, string functionName)
	{
		_npcDialogManager.RemoveDialog(npcId, string.Concat(luaKey, "/", functionName));
	}

	public void ClearDialogs(int npcId)
	{
		_npcDialogManager.ClearDialogs(npcId);
	}

	public void SpawnNpc(int npcId, DynValue dynPosition)
	{
		if (_gameManager.IsHost)
		{
			Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
			_objectManager.SpawnNpc(npcId, position, true);
		}
	}

	public void DespawnNpc(int npcId)
	{
		if (_gameManager.IsHost)
		{
			_objectManager.DespawnNpc(npcId);
		}
	}

	public bool HasNpc(int npcId)
	{
		return _objectManager.FindNpc(npcId) != null;
	}

	public Vector3 GetNpcPosition(int npcId)
	{
		NpcObject npcObject = _objectManager.FindNpc(npcId);
		if (npcObject == null)
		{
			_logger.Warning(string.Format("Npc ID ({0}) not found.", npcId));
			return Vector3.zero;
		}
		return npcObject.transform.position;
	}

	public void SpawnMonster(int monsterId, DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqSpawnMonster
		{
			MonsterId = monsterId,
			Position = position.ToProtoVector3()
		});
	}

	public void SpawnWaveExecutor(int waveExecutorId, DynValue dynPosition, bool executeImmediately)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqSpawnWaveExecutor
		{
			WaveExecutorId = waveExecutorId,
			Position = position.ToProtoVector3(),
			ExecuteImmediately = executeImmediately
		});
	}

	public int GetMonsterCount(int monsterId)
	{
		if (!_gameManager.IsHost)
		{
			_logger.Warning("GetMonsterCount can be called only by the host.");
			return 0;
		}
		return _objectManager.GetMonsterCount(monsterId);
	}

	public void SpawnExit(int exitId, DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqSpawnExit
		{
			ExitId = exitId,
			Position = position.ToProtoVector3()
		});
	}

	public void SpawnDropItem(int itemId, int count, DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		long gainCount = 0L;
		long baseCount = count;
		if (!_gameManager.IsInLounge)
		{
			baseCount = 0L;
			gainCount = count;
		}
		Goods goods = GoodsCreator.MakeItemGoods(itemId, baseCount, gainCount);
		_gameChannel.SendToHost(new ReqSpawnDropGoods
		{
			Goods = new Goods[1] { goods },
			Position = position.ToProtoVector3()
		});
	}

	public void SpawnRewardBox(int rewardBoxId, DynValue dynPosition, DynValue dynForward)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		Vector3 forward = new Vector3((float)dynForward.Table.Get("x").Number, (float)dynForward.Table.Get("y").Number, (float)dynForward.Table.Get("z").Number);
		_gameChannel.SendToHost(new ReqRewardBox
		{
			RewardBoxId = rewardBoxId,
			Position = position.ToProtoVector3(),
			Forward = forward.ToProtoVector3()
		});
	}

	public void SpawnSequentialTimeline(string prefabName, DynValue dynPosition)
	{
		if (_gameManager.IsHost)
		{
			Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
			_gameHost.SendToAll(new NtfSpawnSequentialTimeline
			{
				PrefabName = prefabName,
				Position = position.ToProtoVector3()
			});
		}
	}

	public void SpawnPointerArrow(int id, DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_objectManager.SpawnPointerArrow(id, position);
	}

	public void DespawnPointerArrow(int id)
	{
		_objectManager.DespawnPointerArrow(id);
	}

	public void SpawnMiniMapMarker(int customId, string markerName, DynValue dynPosition)
	{
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_mapManager.SpawnCustomMarker(new CustomMarkerData
		{
			CustomId = customId,
			MarkerName = markerName,
			Position = position
		});
	}

	public void PlayBgm(string bgmName)
	{
		if (string.IsNullOrEmpty(bgmName))
		{
			_logger.Warning("PlayBgm: bgmName 비어있음");
		}
		else
		{
			_gameSoundManager.PlayOverrideBgm(bgmName);
		}
	}

	public void StopBgm(float seconds = 0f)
	{
		_gameSoundManager.StopOverrideBgm(seconds);
	}

	public void PlaySfx(string sfxName)
	{
		_soundManager.PlaySfx(sfxName);
	}

	public void StopSfx(string sfxName)
	{
		_soundManager.StopSfxAll(sfxName);
	}

	public void DespawnMiniMapMarker(int id)
	{
		_mapManager.DespawnCustomMarker(id);
	}

	public void SetTriggerable(ETriggerableObjectType objectType, int id, bool canTriggered)
	{
		if (objectType == ETriggerableObjectType.Triggerable)
		{
			_objectManager.FindTriggerableObject(id)?.SetTriggerable(canTriggered);
		}
		else if (objectType == ETriggerableObjectType.WaveExecutor)
		{
			_objectManager.FindWaveExecutor(id)?.SetTriggerable(canTriggered);
		}
	}

	public void TriggerEventToAll(int value)
	{
		_gameChannel.SendToHost(new ReqTriggerLuaEvent
		{
			Value = value
		});
	}

	public Vector3 GetTriggerableObjectPosition(int id)
	{
		ITriggerable triggerable = _objectManager.FindTriggerableObject(id);
		if (triggerable == null)
		{
			_logger.Warning(string.Format("Triggerable Object ID ({0}) not found.", id));
			return Vector3.zero;
		}
		return triggerable.Position;
	}

	public void ShowSystemToastText(string message)
	{
		_toastManager.Notice(ConvertToLocalText(message));
	}

	public TaskWrapper OpenNoteSystemPopup(string text, ENoteType noteType, float backGroundAlpha = 0.5f)
	{
		return OpenNoteSystemPopup(new string[1] { text }, noteType, backGroundAlpha);
	}

	public TaskWrapper OpenNoteSystemPopup(string[] texts, ENoteType noteType, float backGroundAlpha = 0.5f)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		if (texts == null || texts.Length == 0)
		{
			_logger.Warning("OpenNoteSystemPopup: texts 비어있음");
			taskWrapper.IsCompleted = true;
			return taskWrapper;
		}
		NoteSystemPopupModel model = new NoteSystemPopupModel
		{
			Texts = texts.Select(ConvertToLocalText).ToArray(),
			BackGroundAlpha = backGroundAlpha,
			NoteType = noteType
		};
		_popupManager.OpenAsync<NoteSystemPopupPresenter>(model).ContinueWith(delegate(NoteSystemPopupPresenter presenter)
		{
			if (presenter == null)
			{
				taskWrapper.IsCompleted = true;
			}
			else
			{
				presenter.OnClose.AsObservable().Take(1).Subscribe(delegate
				{
					taskWrapper.IsCompleted = true;
				}).AddTo(presenter);
			}
		}).Forget();
		return taskWrapper;
	}

	public void GiveItem(int itemId, int count)
	{
		long gainCount = 0L;
		long baseCount = count;
		if (!_gameManager.IsInLounge)
		{
			baseCount = 0L;
			gainCount = count;
		}
		Goods goods = GoodsCreator.MakeItemGoods(itemId, baseCount, gainCount);
		_gameChannel.SendToHost(new ReqGainGoods
		{
			Goods = new Goods[1] { goods }
		});
	}

	public void RemoveItem(int itemId, int count)
	{
		long num = count;
		if (_gameManager.IsInLounge)
		{
			PlayerInventory myInventory = _gameManager.MyInventory;
			if (myInventory != null)
			{
				long itemCount = myInventory.GetItemCount(itemId);
				if (itemCount > 0)
				{
					long baseCount = Math.Min(itemCount, num);
					Goods goods = GoodsCreator.MakeItemGoods(itemId, baseCount, 0L);
					_gameChannel.SendToHost(new ReqConsumeGoods
					{
						Goods = new Goods[1] { goods }
					});
				}
				if (itemCount < num)
				{
					Goods consumeGoods = GoodsCreator.MakeItemGoods(itemId, num - itemCount, 0L);
					_playerManager.ConsumeStashGoods(consumeGoods);
				}
				return;
			}
		}
		Goods goods2 = GoodsCreator.MakeItemGoods(itemId, num, 0L);
		_gameChannel.SendToHost(new ReqConsumeGoods
		{
			Goods = new Goods[1] { goods2 }
		});
	}

	public bool HasItem(int itemId)
	{
		if (_gameManager.MyInventory == null)
		{
			_logger.Error(string.Format("MyInventory null. ItemId:{0}", itemId));
			return false;
		}
		Goods goods = GoodsCreator.MakeItemGoods(itemId, 0L, 0L);
		return _playerManager.GetTotalGoodsCount(goods) > 0;
	}

	public bool HasItem(int itemId, int count)
	{
		if (_gameManager.MyInventory == null)
		{
			_logger.Error(string.Format("MyInventory null. ItemId:{0}, count:{1}", itemId, count));
			return false;
		}
		Goods goods = GoodsCreator.MakeItemGoods(itemId, 0L, 0L);
		return count <= _playerManager.GetTotalGoodsCount(goods);
	}

	public long GetItemCount(int itemId)
	{
		return _playerManager.GetTotalGoodsCount(GoodsCreator.MakeItemGoods(itemId, 0L, 0L));
	}

	public int GetEmptyInventorySlotCount()
	{
		return _gameManager.MyInventory.GetEmptySlotCount();
	}

	public void GiveItemToStorage(int itemId, int count)
	{
		_playerManager.AddGoodsToStorage(GoodsCreator.MakeItemGoods(itemId, count, 0L));
	}

	public void GiveEquipment(int equipmentId, int count)
	{
		TEquipment data = TEquipment.Table[equipmentId];
		int[] prefixes = data.PickPrefixes();
		long gainCount = count;
		long baseCount;
		if (_gameManager.IsInLounge)
		{
			baseCount = gainCount;
			gainCount = 0L;
		}
		else
		{
			baseCount = 0L;
		}
		Goods goods = GoodsCreator.MakeEquipmentGoods(equipmentId, baseCount, gainCount, prefixes, null, null, 1);
		_gameChannel.SendToHost(new ReqGainGoods
		{
			Goods = new Goods[1] { goods }
		});
	}

	public void RemoveEquipment(int equipmentId, int count)
	{
		Goods goods = GoodsCreator.MakeEquipmentGoods(equipmentId, count, 0L, null, null, null, 1);
		_gameChannel.SendToHost(new ReqConsumeGoods
		{
			Goods = new Goods[1] { goods }
		});
	}

	public bool HasEquipment(int equipmentId)
	{
		return _gameManager.MyInventory.GetEquipmentCount(equipmentId) > 0;
	}

	public bool HasEquipment(int equipmentId, int count)
	{
		return count <= _gameManager.MyInventory.GetEquipmentCount(equipmentId);
	}

	public long GetEquipmentCount(int equipmentId)
	{
		return _gameManager.MyInventory.GetEquipmentCount(equipmentId);
	}

	public void GiveEquipmentToStorage(int equipmentId, int count)
	{
		TEquipment data = TEquipment.Table[equipmentId];
		int[] prefixes = data.PickPrefixes();
		_playerManager.AddGoodsToStorage(GoodsCreator.MakeEquipmentGoods(equipmentId, count, 0L, prefixes, null, null, 1));
	}

	public void GiveCoin(int count)
	{
		_gameChannel.SendToHost(new ReqGainGoods
		{
			Goods = new Goods[1]
			{
				new CoinGoods(0L, count)
			}
		});
	}

	public void RemoveCoin(int count)
	{
		_gameChannel.SendToHost(new ReqConsumeGoods
		{
			Goods = new Goods[1]
			{
				new CoinGoods(0L, count)
			}
		});
	}

	public bool HasCoin(int count)
	{
		return _playerManager.HasEnoughCoin(count);
	}

	public void GiveGold(int count)
	{
		_gameChannel.SendToHost(new ReqGainGoods
		{
			Goods = new Goods[1]
			{
				new GoldGoods(0L, count)
			}
		});
	}

	public void RemoveGold(int count)
	{
		_gameChannel.SendToHost(new ReqConsumeGoods
		{
			Goods = new Goods[1]
			{
				new GoldGoods(0L, count)
			}
		});
	}

	public bool HasGold(int count)
	{
		return _playerManager.HasEnoughGold(count);
	}

	public void PlayPing(string senderName, EPingType pingType, DynValue dynPosition)
	{
		if (pingType == EPingType.Notice || pingType == EPingType.Alert)
		{
			Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
			_gamePingManager.PlayPing(ConvertToLocalText(senderName), pingType, position, EPingTargetType.None, null, 0, 0);
		}
	}

	public TaskWrapper WaitDelayAsync(int milliseconds)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		UniTask.Delay(milliseconds).ContinueWith(delegate
		{
			taskWrapper.IsCompleted = true;
			return true;
		});
		return taskWrapper;
	}

	public string GetString(int id, string key)
	{
		return _playerManager.GetPlayerStoreString(id, key);
	}

	public int GetInt(int id, string key)
	{
		return _playerManager.GetPlayerStoreInt(id, key);
	}

	public float GetFloat(int id, string key)
	{
		return _playerManager.GetPlayerStoreFloat(id, key);
	}

	public bool GetBool(int id, string key)
	{
		return _playerManager.GetPlayerStoreBool(id, key);
	}

	public void SetString(int id, string key, string value)
	{
		_playerManager.SetPlayerStoreString(id, key, value);
	}

	public void SetInt(int id, string key, int value)
	{
		_playerManager.SetPlayerStoreInt(id, key, value);
	}

	public void SetFloat(int id, string key, float value)
	{
		_playerManager.SetPlayerStoreFloat(id, key, value);
	}

	public void SetBool(int id, string key, bool value)
	{
		_playerManager.SetPlayerStoreBool(id, key, value);
	}

	public void SetTempString(int id, string key, string value)
	{
		_playerManager.SetPlayerStoreTempString(id, key, value);
	}

	public void SetTempInt(int id, string key, int value)
	{
		_playerManager.SetPlayerStoreTempInt(id, key, value);
	}

	public void SetTempFloat(int id, string key, float value)
	{
		_playerManager.SetPlayerStoreTempFloat(id, key, value);
	}

	public void SetTempBool(int id, string key, bool value)
	{
		_playerManager.SetPlayerStoreTempBool(id, key, value);
	}

	public void RemoveKey(int id)
	{
		_playerManager.RemovePlayerStore(id);
	}

	public void RemoveValue(int id, string key)
	{
		_playerManager.RemovePlayerStoreValue(id, key);
	}

	public int GetCharacterHp()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return 0;
		}
		return myCharacterModel.Hp;
	}

	public int GetCharacterMaxHp()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return 0;
		}
		return (int)myCharacterModel.HpMax;
	}

	public int GetCharacterStress()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return 0;
		}
		return (int)myCharacterModel.Stress;
	}

	public int GetCharacterLightFuel()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return 0;
		}
		return myCharacterModel.LightFuel;
	}

	public int[] GetParadoxIds()
	{
		return _gameManager.MyPlayer?.GetParadoxIds() ?? Array.Empty<int>();
	}

	public void SetCharacterHp(float hp)
	{
		_gameChannel.SendToHost(new ReqSetCharacterHp
		{
			Hp = hp
		});
	}

	public void SetCharacterStress(float stress)
	{
		_gameChannel.SendToHost(new ReqSetCharacterStress
		{
			Stress = stress
		});
	}

	public void SetCharacterLightFuel(int lightFuel)
	{
		_gameChannel.SendToHost(new ReqSetCharacterLightFuel
		{
			LightFuel = lightFuel
		});
	}

	public void ForceStartStage()
	{
		if (!_gameManager.IsHost)
		{
			_logger.Warning("ForceStartStage: 호스트만 호출 가능");
		}
		else if (_gameManager == null)
		{
			_logger.Warning("ForceStartStage: GameManager null");
		}
		else
		{
			ForceStartStageAsync().Forget();
		}
	}

	private async UniTask ForceStartStageAsync()
	{
		await UniTask.DelayFrame(1);
		if (_gameContext.CurrentScene == ESceneType.Lounge)
		{
			_gameManager.StartStage();
		}
		else
		{
			_logger.Warning("ForceStartStage: 현재 라운지 상태가 아님");
		}
	}

	public void ForceEscapeStage()
	{
		_gameChannel.SendToHost(new ReqForceEscape());
	}

	public TaskWrapper DelayDialogAsync(int milliseconds)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		if (milliseconds > 0)
		{
			DialogPopupPresenter dialogPopupPresenter = _popupManager.FindPopup<DialogPopupPresenter>();
			if (dialogPopupPresenter != null)
			{
				dialogPopupPresenter.DelayDialogAsync(milliseconds).ContinueWith(delegate
				{
					taskWrapper.IsCompleted = true;
				}).Forget();
				return taskWrapper;
			}
			_logger.Warning("DelayDialogAsync: DialogPopupPresenter 없음");
		}
		taskWrapper.IsCompleted = true;
		return taskWrapper;
	}

	public void SetInGameHudActive(bool active)
	{
		_gameManager.RxActiveInGameHud.Value = active;
	}

	private string ConvertToLocalText(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		Match match = new Regex("<Key:([A-Za-z0-9_]+)>(.*)").Match(text);
		if (!match.Success)
		{
			_logger.Error(string.Concat("\"", text, "\" (expected format: <Key:####>)"));
			return text;
		}
		return LocalizationUtility.GetLocalizedText(match.Groups[1].Value);
	}

	public void SetNpcNavigationActive(int npcId, bool isActive)
	{
		NpcObject npcObject = _objectManager.FindNpc(npcId);
		if (npcObject == null)
		{
			_logger.Warning(string.Format("SetNpcNavigationActive: Npc ID ({0}) not found.", npcId));
		}
		else
		{
			npcObject.SetNavigationActive(isActive);
		}
	}

	public void AcquireParadox(int paradoxId, bool includePositive, bool includeNegative)
	{
		_gameChannel.SendToHost(new ReqAcquireParadox
		{
			ParadoxId = paradoxId,
			IncludePositive = includePositive,
			IncludeNegative = includeNegative
		});
	}

	public void PlayCutscene(string cutscenePrefab, DynValue dynDelta)
	{
		Vector3 position = new Vector3((float)dynDelta.Table.Get("x").Number, (float)dynDelta.Table.Get("y").Number, (float)dynDelta.Table.Get("z").Number);
		_cutsceneManager.RequestPlayCutscene(cutscenePrefab, position);
	}

	public TaskWrapper PlayCutsceneAsync(string cutscenePrefab, DynValue dynPosition)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		Vector3 position = new Vector3((float)dynPosition.Table.Get("x").Number, (float)dynPosition.Table.Get("y").Number, (float)dynPosition.Table.Get("z").Number);
		_cutsceneManager.RxIsPlaying.Skip(1).Where((bool isPlaying) => !isPlaying).Take(1)
			.Subscribe(delegate
			{
				taskWrapper.IsCompleted = true;
			});
		_cutsceneManager.RequestPlayCutscene(cutscenePrefab, position);
		return taskWrapper;
	}

	public TaskWrapper WaitCutsceneSignalAsync(int signalId)
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		if (_cutsceneManager.RxIsPlaying.Value && !_cutsceneManager.IsLuaSignalFired(signalId))
		{
			_cutsceneManager.GetLuaSignalSubject(signalId).Take(1).Subscribe(delegate
			{
				taskWrapper.IsCompleted = true;
			});
			return taskWrapper;
		}
		taskWrapper.IsCompleted = true;
		return taskWrapper;
	}

	public TaskWrapper WaitCutsceneEndAsync()
	{
		TaskWrapper taskWrapper = new TaskWrapper();
		if (_cutsceneManager.RxIsPlaying.Value)
		{
			_cutsceneManager.RxIsPlaying.Where((bool isPlaying) => !isPlaying).Take(1).Subscribe(delegate
			{
				taskWrapper.IsCompleted = true;
			});
			return taskWrapper;
		}
		taskWrapper.IsCompleted = true;
		return taskWrapper;
	}

	public void RemoveParadox(int paradoxId, bool includePositive, bool includeNegative)
	{
		_gameChannel.SendToHost(new ReqRemoveParadox
		{
			ParadoxId = paradoxId,
			IncludePositive = includePositive,
			IncludeNegative = includeNegative
		});
	}
}

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using MoonSharp.Interpreter;
using NemoLib.Log;
using NemoLib.Popup;
using UnityEngine;
using VContainer;

[MoonSharpUserData]
public class LuaApi : NativeBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CForceStartStageAsync_003Ed__140 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LuaApi _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

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

	private int CampaignId => 0;

	private Inventory MyInventory => null;

	public void LogDebug(string message)
	{
	}

	public void LogInfo(string message)
	{
	}

	public void LogWarning(string message)
	{
	}

	public void LogError(string message)
	{
	}

	public bool IsHost()
	{
		return false;
	}

	public bool IsTutorial()
	{
		return false;
	}

	public void SetIsTutorial(bool isTutorial)
	{
	}

	public void SendTutorialEvent(string eventName)
	{
	}

	public bool IsTaskAchieved(int campaignTaskId)
	{
		return false;
	}

	public bool IsAllTaskAchieved()
	{
		return false;
	}

	public int GetStep()
	{
		return 0;
	}

	public void SetStep(int stepId)
	{
	}

	public int GetCharacterId()
	{
		return 0;
	}

	public int GetCampaignId()
	{
		return 0;
	}

	public Vector3 GetCharacterPosition()
	{
		return default;
	}

	public Vector3 GetCharacterForward()
	{
		return default;
	}

	public void SetCharacterPosition(DynValue dynPosition)
	{
	}

	public void SetCharacterForward(DynValue dynForward)
	{
	}

	public void AddCharacterBuff(int buffId)
	{
	}

	public Vector3 GetZoneSpawnPosition(int zoneSpawnId)
	{
		return default;
	}

	public bool IsClearedCampaign(int campaignId)
	{
		return false;
	}

	public int GetQuestState()
	{
		return 0;
	}

	public void SetQuestState(ECampaignState state)
	{
	}

	public EMissionState GetMissionState(int missionId)
	{
		return EMissionState.None;
	}

	public void SetMissionState(int missionId, EMissionState state)
	{
	}

	public void SetProgress(int key, int progress)
	{
	}

	public int GetMissionStep(int missionId)
	{
		return 0;
	}

	public void SetMissionStep(int missionId, int stepId)
	{
	}

	public ELoungeQuestState GetLoungeQuestState(int loungeQuestId)
	{
		return ELoungeQuestState.None;
	}

	public void SetLoungeQuestState(int loungeQuestId, ELoungeQuestState state)
	{
	}

	public bool IsAllLoungeQuestTaskCompleted(int loungeQuestId)
	{
		return false;
	}

	public void ShowBubbleText(ESpeaker speakerType, int tableId, string text)
	{
	}

	public void ShowMonologueText(ESpeaker speakerType, int tableId, string text)
	{
	}

	public void ShowRadioText(ESpeaker speakerType, int tableId, string imageName, string text)
	{
	}

	public TaskWrapper OpenDialogAsync(float backgroundAlpha = 0.5f)
	{
		return null;
	}

	public TaskWrapper AppendDialogAsync(DialogSpeakerEntry[] speakers, int speakerIndex, string message, string[] selectableMessages)
	{
		return null;
	}

	public TaskWrapper SetDialogCustomImageAsync(string image, bool isFront, int index)
	{
		return null;
	}

	public TaskWrapper ClearDialogCustomImageAsync(bool isFront, int index)
	{
		return null;
	}

	public TaskWrapper ClearAllDialogCustomImagesAsync()
	{
		return null;
	}

	public void FadeInDialog()
	{
	}

	public void FadeOutDialog()
	{
	}

	public void SetDialogImmediate(DialogSpeakerEntry[] speakers, int speakerIndex, string message)
	{
	}

	public TaskWrapper CloseDialogAsync()
	{
		return null;
	}

	public void SetNpcState(int npcId, ENpcState npcState)
	{
	}

	public void AddDialog(int npcId, DynValue dynDialogInfo)
	{
	}

	public void RemoveDialog(int npcId, string luaKey, string functionName)
	{
	}

	public void ClearDialogs(int npcId)
	{
	}

	public void SpawnNpc(int npcId, DynValue dynPosition)
	{
	}

	public void DespawnNpc(int npcId)
	{
	}

	public bool HasNpc(int npcId)
	{
		return false;
	}

	public Vector3 GetNpcPosition(int npcId)
	{
		return default;
	}

	public void SpawnMonster(int monsterId, DynValue dynPosition)
	{
	}

	public void SpawnWaveExecutor(int waveExecutorId, DynValue dynPosition, bool executeImmediately)
	{
	}

	public int GetMonsterCount(int monsterId)
	{
		return 0;
	}

	public void SpawnExit(int exitId, DynValue dynPosition)
	{
	}

	public void SpawnDropItem(int itemId, int count, DynValue dynPosition)
	{
	}

	public void SpawnRewardBox(int rewardBoxId, DynValue dynPosition, DynValue dynForward)
	{
	}

	public void SpawnSequentialTimeline(string prefabName, DynValue dynPosition)
	{
	}

	public void SpawnPointerArrow(int id, DynValue dynPosition)
	{
	}

	public void DespawnPointerArrow(int id)
	{
	}

	public void SpawnMiniMapMarker(int customId, string markerName, DynValue dynPosition)
	{
	}

	public void PlayBgm(string bgmName)
	{
	}

	public void StopBgm(float seconds = 0f)
	{
	}

	public void PlaySfx(string sfxName)
	{
	}

	public void StopSfx(string sfxName)
	{
	}

	public void DespawnMiniMapMarker(int id)
	{
	}

	public void SetTriggerable(ETriggerableObjectType objectType, int id, bool canTriggered)
	{
	}

	public void TriggerEventToAll(int value)
	{
	}

	public Vector3 GetTriggerableObjectPosition(int id)
	{
		return default;
	}

	public void ShowSystemToastText(string message)
	{
	}

	public TaskWrapper OpenNoteSystemPopup(string text, ENoteType noteType, float backGroundAlpha = 0.5f)
	{
		return null;
	}

	public TaskWrapper OpenNoteSystemPopup(string[] texts, ENoteType noteType, float backGroundAlpha = 0.5f)
	{
		return null;
	}

	public void GiveItem(int itemId, int count)
	{
	}

	public void RemoveItem(int itemId, int count)
	{
	}

	public bool HasItem(int itemId)
	{
		return false;
	}

	public bool HasItem(int itemId, int count)
	{
		return false;
	}

	public long GetItemCount(int itemId)
	{
		return 0L;
	}

	public int GetEmptyInventorySlotCount()
	{
		return 0;
	}

	public void GiveItemToStorage(int itemId, int count)
	{
	}

	public void GiveEquipment(int equipmentId, int count)
	{
	}

	public void RemoveEquipment(int equipmentId, int count)
	{
	}

	public bool HasEquipment(int equipmentId)
	{
		return false;
	}

	public bool HasEquipment(int equipmentId, int count)
	{
		return false;
	}

	public long GetEquipmentCount(int equipmentId)
	{
		return 0L;
	}

	public void GiveEquipmentToStorage(int equipmentId, int count)
	{
	}

	public void GiveCoin(int count)
	{
	}

	public void RemoveCoin(int count)
	{
	}

	public bool HasCoin(int count)
	{
		return false;
	}

	public void GiveGold(int count)
	{
	}

	public void RemoveGold(int count)
	{
	}

	public bool HasGold(int count)
	{
		return false;
	}

	public void PlayPing(string senderName, EPingType pingType, DynValue dynPosition)
	{
	}

	public TaskWrapper WaitDelayAsync(int milliseconds)
	{
		return null;
	}

	public string GetString(int id, string key)
	{
		return null;
	}

	public int GetInt(int id, string key)
	{
		return 0;
	}

	public float GetFloat(int id, string key)
	{
		return 0f;
	}

	public bool GetBool(int id, string key)
	{
		return false;
	}

	public void SetString(int id, string key, string value)
	{
	}

	public void SetInt(int id, string key, int value)
	{
	}

	public void SetFloat(int id, string key, float value)
	{
	}

	public void SetBool(int id, string key, bool value)
	{
	}

	public void SetTempString(int id, string key, string value)
	{
	}

	public void SetTempInt(int id, string key, int value)
	{
	}

	public void SetTempFloat(int id, string key, float value)
	{
	}

	public void SetTempBool(int id, string key, bool value)
	{
	}

	public void RemoveKey(int id)
	{
	}

	public void RemoveValue(int id, string key)
	{
	}

	public int GetCharacterHp()
	{
		return 0;
	}

	public int GetCharacterMaxHp()
	{
		return 0;
	}

	public int GetCharacterStress()
	{
		return 0;
	}

	public int GetCharacterLightFuel()
	{
		return 0;
	}

	public int[] GetParadoxIds()
	{
		return null;
	}

	public void SetCharacterHp(float hp)
	{
	}

	public void SetCharacterStress(float stress)
	{
	}

	public void SetCharacterLightFuel(int lightFuel)
	{
	}

	public void ForceStartStage()
	{
	}

	[AsyncStateMachine(typeof(_003CForceStartStageAsync_003Ed__140))]
	private UniTask ForceStartStageAsync()
	{
		return default;
	}

	public void ForceEscapeStage()
	{
	}

	public TaskWrapper DelayDialogAsync(int milliseconds)
	{
		return null;
	}

	public void SetInGameHudActive(bool active)
	{
	}

	private string ConvertToLocalText(string text)
	{
		return null;
	}

	public void SetNpcNavigationActive(int npcId, bool isActive)
	{
	}

	public void AcquireParadox(int paradoxId, bool includePositive, bool includeNegative)
	{
	}

	public void PlayCutscene(string cutscenePrefab, DynValue dynDelta)
	{
	}

	public TaskWrapper PlayCutsceneAsync(string cutscenePrefab, DynValue dynPosition)
	{
		return null;
	}

	public TaskWrapper WaitCutsceneSignalAsync(int signalId)
	{
		return null;
	}

	public TaskWrapper WaitCutsceneEndAsync()
	{
		return null;
	}

	public void RemoveParadox(int paradoxId, bool includePositive, bool includeNegative)
	{
	}
}

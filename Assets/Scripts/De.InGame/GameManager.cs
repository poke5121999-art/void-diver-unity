using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class GameManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CBackToSinglePlay_003Ed__176 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameManager _003C_003E4__this;

		public bool isUnexpectedBackToSinglePlay;

		public ERoomDifficulty difficulty;

		public GamePlayerData playerData;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

		private UniTask<int>.Awaiter _003C_003Eu__3;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnSceneLoadCompleteAsync_003Ed__146 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameManager _003C_003E4__this;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private SeriLogger _logger;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private NetworkTimer _networkTimer;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private BlockManager _blockManager;

	[Inject]
	private EnvironmentSettings _settings;

	private CompositeDisposable _sceneDisposable;

	private CancellationTokenSource _sceneJoinCts;

	private readonly Dictionary<ulong, CharacterController> _characterControllers;

	private readonly List<CharacterModel> _characterModels;

	public readonly ReactiveProperty<bool> RxIsJoined;

	public readonly ReactiveProperty<bool> RxIsRevealingMap;

	private bool _waitingStageResultLocalInfos;

	private CancellationTokenSource _waitLocalInfoCts;

	private bool _loungeMoveRequested;

	private const float WAIT_LOCALINFO_TIMEOUT = 3f;

	private const float WAIT_LOUNGE_MOVE_DELAY = 2f;

	private HashSet<ulong> _introFinishedClientIds;

	private bool _isUnexpectedBackToSinglePlay;

	private bool _stateInputBlocked;

	private CancellationTokenSource _stageStageCancellationToken;

	private readonly List<int> _sessionUnlockedTalents;

	private CompositeDisposable _analyticsDisposable;

	private bool _stageEnded;

	private bool _ignoreTutorial;

	private bool _ignoreLoungeTutorial;

	public ulong LocalClientId => 0uL;

	public bool IsHost => false;

	public IReadOnlyList<CharacterModel> CharacterModels => null;

	public Subject<CharacterController> OnCharacterSpawned { get; }

	public Subject<CharacterController> OnCharacterDespawned { get; }

	public Subject<CharacterModel> OnMyCharacterSpawned { get; }

	public GamePlayer HostPlayer => null;

	public GamePlayer MyPlayer => null;

	public CharacterController MyCharacterController => null;

	public CharacterModel MyCharacterModel => null;

	public CharacterView MyCharacterView => null;

	public PlayerInventory MyInventory => null;

	public Subject<GamePlayer> OnClientJoined { get; }

	public Subject<GamePlayer> OnPlayerEntered { get; }

	public Subject<GamePlayer> OnPlayerLeft { get; }

	public Subject<Unit> OnGameStarted { get; }

	public Subject<Unit> OnGameClosed { get; }

	public ReactiveProperty<EStageState> RxStageState { get; }

	public EStageState StageState => EStageState.None;

	public ReactiveProperty<bool> RxActiveInGameHud { get; }

	public ReactiveProperty<ulong> RxViewTargetClientId { get; }

	public bool IsParticipant => false;

	public int PlayerCount => 0;

	private int CharacterCount => 0;

	public bool IsInLounge => false;

	public bool IsInStage => false;

	public bool IsTutorial => false;

	public bool IsPlayingGame => false;

	public bool IsRevealingMapMap => false;

	public StageResultInfo StageResultInfo { get; private set; }

	public Subject<PlayerInventory> OnPlayerInventoryInitialized { get; }

	public string CachedAddress { get; private set; }

	public ushort CachedPort { get; private set; }

	public int CachedConnectionResultCode { get; set; }

	public bool IsUnexpectedBackToSinglePlay => false;

	internal bool IsMyMissionClear { get; private set; }

	public bool IgnoreTutorial => false;

	public bool IgnoreLoungeTutorial => false;

	public int[] MissionIds { get; private set; }

	public float TimeNow => 0f;

	public bool IsClientJoined(ulong clientId)
	{
		return false;
	}

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void AssertIfNotHost()
	{
	}

	public void LoadGame()
	{
	}

	private void MoveToLounge()
	{
	}

	private bool IsSceneLoadInProgress()
	{
		return false;
	}

	public void OnReadyStage()
	{
	}

	public void OnCancelStartStage()
	{
	}

	public void StartStage()
	{
	}

	public void FailStage()
	{
	}

	private void EndStage()
	{
	}

	private void InitializeStageResultInfo()
	{
	}

	private void RefreshStageResultPlayerStates()
	{
	}

	private void OnStageResultPlayerInfo(NtfStageResultPlayerInfo message)
	{
	}

	private void OnStageResultLocalInfo(NtfStageResultLocalInfo message)
	{
	}

	public CharacterController FindCharacterController(ulong clientId)
	{
		return null;
	}

	public CharacterModel FindCharacterModel(ulong clientId)
	{
		return null;
	}

	public bool ContainCharacter(ulong clientId)
	{
		return false;
	}

	public void StartVoteCampaign(int campaignId, EDifficulty difficulty, EAnomalyType anomalyType)
	{
	}

	public void StartCampaign(int campaignId, EDifficulty difficulty, EAnomalyType anomalyType)
	{
	}

	public void CloseCampaign(ECampaignCloseReason reason)
	{
	}

	private void OnClientCaughtUp(ulong clientId)
	{
	}

	private void OnSceneEvent(SceneEvent sceneEvent)
	{
	}

	private void OnSceneUnload(string sceneName)
	{
	}

	private void OnSceneLoad(string sceneName)
	{
	}

	private void OnSceneLoadComplete(string sceneName)
	{
	}

	[AsyncStateMachine(typeof(_003COnSceneLoadCompleteAsync_003Ed__146))]
	private UniTask OnSceneLoadCompleteAsync(CancellationToken ct)
	{
		return default;
	}

	private void OnStageStateChange(EStageState stageState)
	{
	}

	private void BlockInput()
	{
	}

	private void UnblockInput()
	{
	}

	private void OnResourceLoadCompleted()
	{
	}

	private void OnUpdateSec()
	{
	}

	private void OnGameReady(NtfGameReady message)
	{
	}

	private void OnGameLeft()
	{
	}

	private void OnConnectionDataSet((string address, ushort port) connectionData)
	{
	}

	private void OnPlayersReady(NtfPlayersReady message)
	{
	}

	private void OnRoomPlayersAdded(GamePlayer player)
	{
	}

	private void OnRoomPlayersRemoved(GamePlayer player)
	{
	}

	private void TryEndStageOnPlayerLeft()
	{
	}

	private void OnDeadOrEscaped(CharacterModel characterModel)
	{
	}

	private void OnMyCharacterRevive()
	{
	}

	private void CloseScene()
	{
	}

	public bool IsAllDeadOrEscaped()
	{
		return false;
	}

	public bool IsEscaped()
	{
		return false;
	}

	public void NotifyConsumeItemVfx(EGoodsType itemType, float duration, Vector3 position, Vector3 direction = default(Vector3), ulong targetNetworkObjectId = 0uL, Vector3 targetPositionOffset = default(Vector3))
	{
	}

	public void ChangeViewTarget()
	{
	}

	public void ClearStageResult()
	{
	}

	private void BeginWaitStageResultLocalInfos()
	{
	}

	private void TryCompleteLocalInfoCollection()
	{
	}

	private void CompleteWaitAndMoveToLounge()
	{
	}

	private void OnReqStageResultPlayerLocalInfo(ReqStageResultPlayerLocalInfo req)
	{
	}

	private void OnRequestUserChat(ReqUserChat message)
	{
	}

	private void OnReqClientIntroFinished(ReqClientIntroFinished req)
	{
	}

	private void TryCompleteIntroFinish()
	{
	}

	private void OnAllClientsIntroFinished(NtfAllClientsIntroFinished message)
	{
	}

	public void StartVoteGiveUpCampaign()
	{
	}

	[AsyncStateMachine(typeof(_003CBackToSinglePlay_003Ed__176))]
	public UniTask BackToSinglePlay(GamePlayerData playerData, ERoomDifficulty difficulty, bool isUnexpectedBackToSinglePlay = false)
	{
		return default;
	}

	public void ClearUnexpectedBackToSinglePlay()
	{
	}

	internal void SetMissionClear(bool value)
	{
	}

	private void TriggerSessionStart()
	{
	}

	private void TriggerSessionEnd()
	{
	}

	internal void TriggerStageStart()
	{
	}

	internal void TriggerStageEnd(bool isEscaped)
	{
	}

	private void TriggerStageAbort()
	{
	}

	private int GetMyCharacterId()
	{
		return 0;
	}

	private int? GetAnomalyId()
	{
		return null;
	}

	private string BuildUseSkillString()
	{
		return null;
	}

	private string BuildUseEquipmentString()
	{
		return null;
	}

	private int? GetArtifactCount()
	{
		return null;
	}

	private (int?, int?) CalculateGainCurrency()
	{
		return default;
	}

	private static void AccumulateGainWorth(Goods goods, ref int soft, ref int hard)
	{
	}

	internal static int DifficultyToRound(EDifficulty difficulty)
	{
		return 0;
	}

	private void LoadCheatPreferences()
	{
	}

	public void SetIgnoreTutorial(bool value)
	{
	}

	public void SetIgnoreLoungeTutorial(bool value)
	{
	}

	public void SetMissionIds(int[] value)
	{
	}

	private void OnClientStarted()
	{
	}

	private void OnClientStopped(bool _)
	{
	}
}

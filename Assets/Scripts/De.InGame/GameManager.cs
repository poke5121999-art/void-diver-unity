using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using Unity.Netcode;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

public class GameManager : MonoBase
{
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

	private readonly Dictionary<ulong, CharacterController> _characterControllers = new Dictionary<ulong, CharacterController>();

	private readonly List<CharacterModel> _characterModels = new List<CharacterModel>();

	public readonly ReactiveProperty<bool> RxIsJoined = new ReactiveProperty<bool>(initialValue: false);

	public readonly ReactiveProperty<bool> RxIsRevealingMap = new ReactiveProperty<bool>();

	private bool _waitingStageResultLocalInfos;

	private CancellationTokenSource _waitLocalInfoCts;

	private bool _loungeMoveRequested;

	private const float WAIT_LOCALINFO_TIMEOUT = 3f;

	private const float WAIT_LOUNGE_MOVE_DELAY = 2f;

	private HashSet<ulong> _introFinishedClientIds = new HashSet<ulong>();

	private bool _isUnexpectedBackToSinglePlay;

	private bool _stateInputBlocked;

	private CancellationTokenSource _stageStageCancellationToken;

	private readonly List<int> _sessionUnlockedTalents = new List<int>();

	private CompositeDisposable _analyticsDisposable;

	private bool _stageEnded;

	private bool _ignoreTutorial;

	private bool _ignoreLoungeTutorial;

	public ulong LocalClientId => _networkManager.LocalClientId;

	public bool IsHost => _networkManager.IsHost;

	public IReadOnlyList<CharacterModel> CharacterModels => _characterModels;

	public Subject<CharacterController> OnCharacterSpawned { get; } = new Subject<CharacterController>();

	public Subject<CharacterController> OnCharacterDespawned { get; } = new Subject<CharacterController>();

	public Subject<CharacterModel> OnMyCharacterSpawned { get; } = new Subject<CharacterModel>();

	public GamePlayer HostPlayer => _gameContext.Players.Values.FirstOrDefault((GamePlayer p) => p.IsHostPlayer);

	public GamePlayer MyPlayer => _gameContext.Players[_networkManager.LocalClientId];

	public CharacterController MyCharacterController => _characterControllers.GetValueOrDefault(_networkManager.LocalClientId);

	public CharacterModel MyCharacterModel => _characterControllers.GetValueOrDefault(_networkManager.LocalClientId)?.GetComponent<CharacterModel>();

	public CharacterView MyCharacterView => _characterControllers.GetValueOrDefault(_networkManager.LocalClientId)?.GetComponent<CharacterView>();

	public PlayerInventory MyInventory => _characterControllers.GetValueOrDefault(_networkManager.LocalClientId)?.GetComponent<PlayerInventory>();

	public Subject<GamePlayer> OnClientJoined { get; } = new Subject<GamePlayer>();

	public Subject<GamePlayer> OnPlayerEntered { get; } = new Subject<GamePlayer>();

	public Subject<GamePlayer> OnPlayerLeft { get; } = new Subject<GamePlayer>();

	public Subject<Unit> OnGameStarted { get; } = new Subject<Unit>();

	public Subject<Unit> OnGameClosed { get; } = new Subject<Unit>();

	public ReactiveProperty<EStageState> RxStageState { get; } = new ReactiveProperty<EStageState>();

	public EStageState StageState => RxStageState.Value;

	public ReactiveProperty<bool> RxActiveInGameHud { get; } = new ReactiveProperty<bool>(initialValue: true);

	public ReactiveProperty<ulong> RxViewTargetClientId { get; } = new ReactiveProperty<ulong>(ulong.MaxValue);

	public bool IsParticipant
	{
		get
		{
			if (RxViewTargetClientId.Value != _networkManager.LocalClientId)
			{
				return false;
			}
			if (MyCharacterModel == null)
			{
				return false;
			}
			if (MyCharacterModel.State == EActionState.Dead)
			{
				return false;
			}
			return MyCharacterModel.State != EActionState.Escaped;
		}
	}

	public int PlayerCount => _gameContext.Players.Count;

	private int CharacterCount => _characterControllers.Count;

	public bool IsInLounge => _gameContext.IsInLounge;

	public bool IsInStage => _gameContext.IsInStage;

	public bool IsTutorial => _gameContext.IsTutorial;

	public bool IsPlayingGame => _characterControllers.GetValueOrDefault(_networkManager.LocalClientId) != null;

	public bool IsRevealingMapMap => RxIsRevealingMap.Value;

	public StageResultInfo StageResultInfo { get; private set; }

	public Subject<PlayerInventory> OnPlayerInventoryInitialized { get; } = new Subject<PlayerInventory>();

	public string CachedAddress { get; private set; }

	public ushort CachedPort { get; private set; }

	public int CachedConnectionResultCode { get; set; }

	public bool IsUnexpectedBackToSinglePlay => _isUnexpectedBackToSinglePlay;

	internal bool IsMyMissionClear { get; private set; }

	public bool IgnoreTutorial => _ignoreTutorial && _settings.Environment != EEnvironment.Live;

	public bool IgnoreLoungeTutorial => _ignoreLoungeTutorial && _settings.Environment != EEnvironment.Live;

	public int[] MissionIds { get; private set; }

	public float TimeNow => _networkTimer.GameTime;

	public bool IsClientJoined(ulong clientId)
	{
		return _gameContext.Players.ContainsKey(clientId);
	}

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
		LoadCheatPreferences();
	}

	private void Start()
	{
		_networkManager.OnClientStarted += OnClientStarted;
		_networkManager.OnClientStopped += OnClientStopped;
		_gameChannel.Receive<NtfGameReady>().Subscribe(OnGameReady).AddTo(this);
		_gameChannel.Receive<NtfPlayersReady>().Subscribe(OnPlayersReady).AddTo(this);
		_gameChannel.Receive<NtfStageResultPlayerInfo>().Subscribe(OnStageResultPlayerInfo).AddTo(this);
		_gameChannel.Receive<NtfStageResultLocalInfo>().Subscribe(OnStageResultLocalInfo).AddTo(this);
		_gameChannel.Receive<NtfAllClientsIntroFinished>().Subscribe(OnAllClientsIntroFinished).AddTo(this);
		_gameHost.Receive<ReqStageResultPlayerLocalInfo>().Subscribe(OnReqStageResultPlayerLocalInfo).AddTo(this);
		_gameHost.Receive<ReqUserChat>().Subscribe(OnRequestUserChat).AddTo(this);
		_gameHost.Receive<ReqClientIntroFinished>().Subscribe(OnReqClientIntroFinished).AddTo(this);
		_gameChannel.OnStopped.Subscribe(delegate
		{
			OnGameLeft();
		}).AddTo(this);
		_gameChannel.OnJoined.Subscribe(delegate
		{
			RxIsJoined.Value = true;
		}).AddTo(this);
		_gameContext.RxPlayers.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<ulong, GamePlayer> e)
		{
			OnClientJoined.OnNext(e.Value);
		}).AddTo(this);
		_gameHost.OnConnectionDataSet.Subscribe(OnConnectionDataSet).AddTo(this);
		_gameHost.OnClientCaughtUp.Subscribe(OnClientCaughtUp).AddTo(this);
		OnCharacterSpawned.Subscribe(delegate(CharacterController controller)
		{
			CharacterModel component = controller.GetComponent<CharacterModel>();
			_characterControllers.Add(controller.ClientId, controller);
			_characterModels.Add(component);
			if (component.ClientId == _networkManager.LocalClientId)
			{
				OnMyCharacterSpawned.OnNext(component);
			}
		}).AddTo(this);
		OnCharacterDespawned.Subscribe(delegate(CharacterController controller)
		{
			CharacterModel component2 = controller.GetComponent<CharacterModel>();
			_characterControllers.Remove(controller.ClientId);
			_characterModels.Remove(component2);
		}).AddTo(this);
		RxStageState.CombineLatest(OnCharacterSpawned, (EStageState stageState, CharacterController _) => stageState).CombineLatest(_gameContext.RxIsCutscenePreloaded, (EStageState stageState, bool _) => stageState).DelayFrame(1)
			.Where((EStageState _) => RxStageState.Value == EStageState.SceneLoaded && _gameContext.Players.Count > 0 && _gameContext.Players.Count == _characterControllers.Count && _gameContext.RxIsCutscenePreloaded.Value)
			.Subscribe(delegate
			{
				OnResourceLoadCompleted();
			})
			.AddTo(this);
		RxStageState.Subscribe(OnStageStateChange).AddTo(this);
	}

	public void AssertIfNotHost()
	{
		_networkManager.AssertIfNotHost();
	}

	public void LoadGame()
	{
		if (_networkManager.IsHost)
		{
			_logger.Info("", Color.green);
			MoveToLounge();
		}
		else
		{
			_logger.Warning("Player is not a host.");
		}
	}

	private void MoveToLounge()
	{
		if (!_networkManager.IsHost)
		{
			_logger.Warning("Player is not a host.");
		}
		else if (IsSceneLoadInProgress())
		{
			_logger.Warning(string.Format("[Scene] Duplicate load ignored: Lounge requested while a load is in progress (state={0})", RxStageState.Value));
		}
		else
		{
			_gameContext.CurrentScene = ESceneType.Lounge;
			RxStageState.Value = EStageState.PrepareLoad;
			_gameHost.LoadScene("LoungeScene");
		}
	}

	private bool IsSceneLoadInProgress()
	{
		if (RxStageState.Value > EStageState.None)
		{
			return RxStageState.Value < EStageState.Intro;
		}
		return false;
	}

	public void OnReadyStage()
	{
		if (_characterModels.All((CharacterModel m) => m.State == EActionState.StageReady))
		{
			_stageStageCancellationToken = new CancellationTokenSource();
			float enterTime = _networkTimer.GameTime + TConst.StageEnterDelay.FloatValue;
			_gameHost.SendToAll(new NtfAllPlayerReadyToEnterStage
			{
				EnterTime = enterTime
			});
			UniTask.Delay(System.TimeSpan.FromSeconds(TConst.StageEnterDelay.FloatValue), ignoreTimeScale: false, PlayerLoopTiming.Update, _stageStageCancellationToken.Token).ContinueWith(StartStage).Forget();
		}
	}

	public void OnCancelStartStage()
	{
		if (_stageStageCancellationToken != null)
		{
			_stageStageCancellationToken.Cancel();
		}
		if (_stageStageCancellationToken != null)
		{
			_stageStageCancellationToken.Dispose();
		}
		_stageStageCancellationToken = null;
		_gameHost.SendToAll(new NtfCanceledEnterStage());
	}

	public void StartStage()
	{
		_networkManager.AssertIfNotHost();
		_gameContext.CurrentScene = ESceneType.Stage;
		RxStageState.Value = EStageState.PrepareLoad;
		_gameHost.LoadScene("StageScene");
	}

	public void FailStage()
	{
		_networkManager.AssertIfNotHost();
		_characterModels.Where((CharacterModel m) => m.State != EActionState.Escaped && m.State != EActionState.Dead && m.State != EActionState.Dying).ForEach(delegate(CharacterModel m)
		{
			m.GetComponent<UnitController>().HandleUnitEvent(new DieEvent());
		});
	}

	private void EndStage()
	{
		if (RxStageState.Value == EStageState.Ended)
		{
			return;
		}
		bool isEscaped = MyCharacterModel.State == EActionState.Escaped;
		TriggerStageEnd(isEscaped);
		_eventSystem.Trigger(new StageEndEvent(_gameContext.CampaignId, _gameContext.Difficulty, _gameContext.Players.Count, isEscaped));
		RefreshStageResultPlayerStates();
		RxStageState.Value = EStageState.Ended;
		_logger.Info("Stage mission ended.", Color.green);
		if (!_networkManager.IsHost)
		{
			return;
		}
		if (!_gameContext.IsTutorial)
		{
			_gameContext.Players.Values.ForEach(delegate(GamePlayer p)
			{
				p.AcquireRandomParadox(includePositive: true, includeNegative: false);
				p.AcquireRandomParadox(includePositive: false, includeNegative: true);
				p.ResetRestCount();
				Profile profile = p.Profile.Clone();
				profile.LostItemRerollCount = 0;
				p.Profile = profile;
			});
		}
		BeginWaitStageResultLocalInfos();
	}

	private void InitializeStageResultInfo()
	{
		if (!_gameContext.IsInStage)
		{
			return;
		}
		StageResultInfo = new StageResultInfo
		{
			PlayerInfos = _characterControllers.Select(delegate(KeyValuePair<ulong, CharacterController> kv)
			{
				UnitModel component = kv.Value.GetComponent<UnitModel>();
				_gameContext.Players.TryGetValue(kv.Key, out var value);
				int[] initialParadoxIds = value?.GetParadoxIds() ?? System.Array.Empty<int>();
				return new StageResultPlayerInfo
				{
					ClientId = kv.Key,
					State = component.State,
					CharacterId = (value?.Character?.Id ?? 0),
					UserName = value?.Name,
					InitialParadoxIds = initialParadoxIds,
					GainedParadoxIds = System.Array.Empty<int>(),
					RemovedParadoxIds = System.Array.Empty<int>()
				};
			}).ToArray(),
			LocalInfos = _characterControllers.Select((KeyValuePair<ulong, CharacterController> kv) => new StageResultLocalInfo
			{
				ClientId = kv.Key
			}).ToArray(),
			CampaignId = _gameContext.CampaignId
		};
	}

	private void RefreshStageResultPlayerStates()
	{
		if (StageResultInfo == null || StageResultInfo.PlayerInfos == null)
		{
			return;
		}
		StageResultPlayerInfo[] playerInfos = StageResultInfo.PlayerInfos;
		foreach (StageResultPlayerInfo stageResultPlayerInfo in playerInfos)
		{
			if (_characterControllers.TryGetValue(stageResultPlayerInfo.ClientId, out var value))
			{
				stageResultPlayerInfo.State = value.GetComponent<UnitModel>().State;
			}
		}
	}

	private void OnStageResultPlayerInfo(NtfStageResultPlayerInfo message)
	{
		if (message == null || message.PlayerInfo == null || StageResultInfo == null || StageResultInfo.PlayerInfos == null)
		{
			return;
		}
		StageResultPlayerInfo incoming = message.PlayerInfo;
		StageResultInfo.PlayerInfos = StageResultInfo.PlayerInfos.Select(delegate(StageResultPlayerInfo playerInfo)
		{
			if (playerInfo.ClientId == incoming.ClientId)
			{
				incoming.InitialParadoxIds = playerInfo.InitialParadoxIds;
				return incoming;
			}
			return playerInfo;
		}).ToArray();
	}

	private void OnStageResultLocalInfo(NtfStageResultLocalInfo message)
	{
		if (message == null || message.LocalInfo == null || StageResultInfo == null || StageResultInfo.LocalInfos == null)
		{
			return;
		}
		List<StageResultLocalInfo> list = StageResultInfo.LocalInfos.ToList();
		int num = list.FindIndex((StageResultLocalInfo li) => li.ClientId == message.LocalInfo.ClientId);
		if (num < 0)
		{
			list.Add(message.LocalInfo);
		}
		else
		{
			list[num] = message.LocalInfo;
		}
		StageResultInfo.LocalInfos = list.OrderBy((StageResultLocalInfo li) => li.ClientId).ToArray();
	}

	public CharacterController FindCharacterController(ulong clientId)
	{
		return _characterControllers.GetValueOrDefault(clientId);
	}

	public CharacterModel FindCharacterModel(ulong clientId)
	{
		return _characterControllers.GetValueOrDefault(clientId)?.GetComponent<CharacterModel>();
	}

	public bool ContainCharacter(ulong clientId)
	{
		return _characterControllers.ContainsKey(clientId);
	}

	public void StartVoteCampaign(int campaignId, EDifficulty difficulty, EAnomalyType anomalyType)
	{
		if (_roomContext.RoomState == ERoomState.Ready && _gameHost.IsAllPlayersReady)
		{
			_gameHost.SendToAll(new NtfVoteCampaign
			{
				ClientId = _networkManager.LocalClientId,
				CampaignId = campaignId,
				Difficulty = difficulty,
				IsAgreed = true,
				AnomalyType = anomalyType,
				IsVoteStart = true
			});
			_logger.Info("", Color.green);
		}
	}

	public void StartCampaign(int campaignId, EDifficulty difficulty, EAnomalyType anomalyType)
	{
		_networkManager.AssertIfNotHost();
		if (_roomContext.RoomState == ERoomState.Ready && _gameHost.IsAllPlayersReady)
		{
			if (_networkManager.IsHost)
			{
				_roomMatchmaker.UpdateGameStarted(isStarted: true);
			}
			_gameHost.SetState(ERoomState.Playing);
			_gameHost.SendToAll(new NtfStartCampaign
			{
				CampaignId = campaignId,
				Difficulty = difficulty,
				AnomalyType = anomalyType
			});
			_logger.Info(string.Format("CampaignId: {0}", campaignId), Color.green);
		}
	}

	public void CloseCampaign(ECampaignCloseReason reason)
	{
		_networkManager.AssertIfNotHost();
		if (_networkManager.IsHost)
		{
			_roomMatchmaker.UpdateGameStarted(isStarted: false);
		}
		_gameHost.SetState(ERoomState.Ready);
		_gameHost.SendToAll(new NtfCloseCampaign
		{
			Reason = reason
		});
		_logger.Info("", Color.green);
	}

	private void OnClientCaughtUp(ulong clientId)
	{
		_networkManager.AssertIfNotHost();
		if (_networkManager.IsHost)
		{
			_roomMatchmaker.UpdateGameStarted(isStarted: false);
		}
		_gameHost.SetState(ERoomState.Ready);
		_gameHost.SendToAll(new NtfCloseCampaign
		{
			Reason = ECampaignCloseReason.None
		});
		_logger.Info("", Color.green);
	}

	private void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent.ClientId == _networkManager.LocalClientId)
		{
			switch (sceneEvent.SceneEventType)
			{
			case SceneEventType.Load:
				OnSceneLoad(sceneEvent.SceneName);
				break;
			case SceneEventType.Unload:
				if (_roomContext.RoomState != ERoomState.Creating)
				{
					OnSceneUnload(sceneEvent.SceneName);
				}
				break;
			case SceneEventType.LoadComplete:
				OnSceneLoadComplete(sceneEvent.SceneName);
				break;
			}
		}
	}

	private void OnSceneUnload(string sceneName)
	{
		_logger.Info(sceneName ?? "", Color.green);
		CloseScene();
	}

	private void OnSceneLoad(string sceneName)
	{
		if (!_networkManager.IsHost)
		{
			_gameContext.CurrentScene = ((sceneName == "LoungeScene") ? ESceneType.Lounge : ((sceneName == "StageScene") ? ESceneType.Stage : ESceneType.None));
		}
		RxIsRevealingMap.Value = _gameContext.IsInLounge;
		RxStageState.Value = EStageState.SceneLoad;
	}

	private void OnSceneLoadComplete(string sceneName)
	{
		_sceneJoinCts = new CancellationTokenSource();
		OnSceneLoadCompleteAsync(_sceneJoinCts.Token).Forget();
	}

	private async UniTask OnSceneLoadCompleteAsync(CancellationToken ct)
	{
		if (_networkManager.IsHost)
		{
			RxIsJoined.Value = true;
		}
		else
		{
			await RxIsJoined.Where((bool v) => v).Take(1).ToUniTask(useFirstValue: false, ct);
		}
		RxStageState.Value = EStageState.SceneLoaded;
	}

	private void OnStageStateChange(EStageState stageState)
	{
		_logger.Info(string.Format("State : {0}", stageState), new Color(0.4980392f, 1f, 0.8313726f, 1f));
		if (stageState == EStageState.SceneLoad)
		{
			BlockInput();
		}
		else if (stageState == EStageState.Playing || stageState == EStageState.Closed)
		{
			UnblockInput();
		}
	}

	private void BlockInput()
	{
		if (!_stateInputBlocked)
		{
			_blockManager.Block(calling: false);
			_stateInputBlocked = true;
		}
	}

	private void UnblockInput()
	{
		if (_stateInputBlocked)
		{
			_blockManager.Release(calling: false);
			_stateInputBlocked = false;
		}
	}

	private void OnResourceLoadCompleted()
	{
		_logger.Info("", Color.green);
		if (_sceneDisposable != null)
		{
			_sceneDisposable.Dispose();
		}
		_sceneDisposable = new CompositeDisposable();
		_gameContext.RxPlayers.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<ulong, GamePlayer> e)
		{
			OnRoomPlayersAdded(e.Value);
		}).AddTo(_sceneDisposable);
		_gameContext.RxPlayers.ObserveRemove().Subscribe(delegate(DictionaryRemoveEvent<ulong, GamePlayer> e)
		{
			OnRoomPlayersRemoved(e.Value);
		}).AddTo(_sceneDisposable);
		if (_gameContext.IsInStage)
		{
			InitializeStageResultInfo();
			_characterModels.Select((CharacterModel m) => m.GetComponent<CharacterModel>()).ForEach(delegate(CharacterModel model)
			{
				model.RxState.Where((EActionState s) => s == EActionState.Dead || s == EActionState.Escaped).Subscribe(delegate
				{
					OnDeadOrEscaped(model);
				}).AddTo(_sceneDisposable);
			});
			MyCharacterModel.RxState.Where((EActionState s) => s == EActionState.Revive).Subscribe(delegate
			{
				OnMyCharacterRevive();
			}).AddTo(_sceneDisposable);
			_eventSystem.Trigger(new StageStartEvent(_gameContext.CampaignId, _gameContext.Difficulty, _gameContext.Players.Count));
			TriggerStageStart();
		}
		RxStageState.Value = EStageState.LoadCompleted;
		CharacterModel characterModel = FindCharacterModel(_networkManager.LocalClientId);
		if (characterModel == null)
		{
			characterModel = _characterModels.FirstOrDefault();
		}
		if (characterModel != null)
		{
			RxViewTargetClientId.Value = characterModel.ClientId;
		}
		_gameChannel.SendToHost(new ReqUpdateClientState
		{
			State = EClientState.SceneLoaded
		});
		Observable.Interval(System.TimeSpan.FromSeconds(1.0)).Subscribe(delegate
		{
			OnUpdateSec();
		}).AddTo(_sceneDisposable);
	}

	private void OnUpdateSec()
	{
		NetworkTime serverTime = _networkManager.ServerTime;
		NetworkTime localTime = _networkManager.LocalTime;
		string text = string.Format("RTT: {0} /ms\n", (int)((serverTime.Time - localTime.Time) * 1000.0));
		string text2 = string.Format("FixedTime: {0} | {1} Diff: {2}\n", serverTime.FixedTime, localTime.FixedTime, serverTime.FixedTime - localTime.FixedTime);
		string text3 = string.Format("Tick: {0} | {1} Diff: {2}\n", serverTime.Tick, localTime.Tick, serverTime.Tick - localTime.Tick);
		string text4 = string.Format("TickOffset: {0} | {1} Diff: {2}\n", serverTime.TickOffset, localTime.TickOffset, serverTime.TickOffset - localTime.TickOffset);
		_logger.Trace(text + text2 + text3 + text4);
	}

	private void OnGameReady(NtfGameReady message)
	{
		_logger.Info("", Color.green);
		OnGameStarted.OnNext(Unit.Default);
		TriggerSessionStart();
	}

	private void OnGameLeft()
	{
		_popupManager.CloseAllWithoutSfx(System.Array.Empty<System.Type>());
		CloseScene();
		if (!_roomContext.IsSingleMode && _gameChannel.RxConnectState.Value != ERoomConnectState.ConnectingToHost)
		{
			_roomMatchmaker.LeaveLobby();
		}
		TriggerSessionEnd();
		_gameContext.Clear();
		StageResultInfo = null;
		RxIsJoined.Value = false;
		OnGameClosed.OnNext(Unit.Default);
	}

	private void OnConnectionDataSet((string address, ushort port) connectionData)
	{
		CachedAddress = connectionData.address;
		CachedPort = connectionData.port;
	}

	private void OnPlayersReady(NtfPlayersReady message)
	{
		_logger.Info("", Color.green);
		_introFinishedClientIds.Clear();
		RxStageState.Value = ((_gameContext.IsInStage && !_gameContext.IsTutorial) ? EStageState.Intro : EStageState.Playing);
		_ = _gameContext.IsInStage;
		_ = _gameContext.IsInLounge;
	}

	private void OnRoomPlayersAdded(GamePlayer player)
	{
		OnPlayerEntered.OnNext(player);
	}

	private void OnRoomPlayersRemoved(GamePlayer player)
	{
		OnPlayerLeft.OnNext(player);
		if (RxStageState.Value == EStageState.Intro)
		{
			_introFinishedClientIds.Remove(player.ClientId);
			TryCompleteIntroFinish();
		}
		if (_gameContext.IsInStage)
		{
			TryEndStageOnPlayerLeft();
		}
	}

	private void TryEndStageOnPlayerLeft()
	{
		if (!_gameContext.IsInStage || RxStageState.Value != EStageState.Playing || MyCharacterModel == null)
		{
			return;
		}
		List<EActionState> list = _characterModels.Where((CharacterModel m) => _gameContext.Players.ContainsKey(m.ClientId)).Select((CharacterModel m) => m.State).ToList();
		if (list.Count > 0 && list.All((EActionState s) => s == EActionState.Dead || s == EActionState.Escaped))
		{
			EndStage();
		}
	}

	private void OnDeadOrEscaped(CharacterModel characterModel)
	{
		if (IsAllDeadOrEscaped())
		{
			EndStage();
		}
		else if (characterModel.ClientId == RxViewTargetClientId.Value)
		{
			ChangeViewTarget();
		}
	}

	private void OnMyCharacterRevive()
	{
		RxViewTargetClientId.Value = MyCharacterModel.ClientId;
	}

	private void CloseScene()
	{
		if (_sceneJoinCts != null)
		{
			_sceneJoinCts.Cancel();
			_sceneJoinCts.Dispose();
			_sceneJoinCts = null;
		}
		if (_sceneDisposable != null)
		{
			_sceneDisposable.Dispose();
			_sceneDisposable = null;
		}
		RxViewTargetClientId.Value = ulong.MaxValue;
		RxStageState.Value = EStageState.Closed;
		RxIsRevealingMap.Value = false;
		_introFinishedClientIds.Clear();
	}

	public bool IsAllDeadOrEscaped()
	{
		return _characterModels.Select((CharacterModel m) => m.State).All((EActionState s) => s == EActionState.Dead || s == EActionState.Escaped);
	}

	public bool IsEscaped()
	{
		return _characterModels.Select((CharacterModel m) => m.State).Any((EActionState s) => s == EActionState.Escaped);
	}

	public void NotifyConsumeItemVfx(EGoodsType itemType, float duration, Vector3 position, Vector3 direction = default(Vector3), ulong targetNetworkObjectId = 0uL, Vector3 targetPositionOffset = default(Vector3))
	{
		string text = itemType.ToString();
		string sfx = System.StringExtensions.ToPascalCase(itemType.ToString());
		_gameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = "ConsumeItem/" + text,
			VfxDuration = duration,
			Sfx = sfx,
			Position = position.ToProtoVector3(),
			Direction = direction.ToProtoVector3(),
			TargetNetworkObjectId = targetNetworkObjectId,
			TargetPositionOffset = targetPositionOffset.ToProtoVector3()
		});
	}

	public void ChangeViewTarget()
	{
		List<ulong> list = (from m in _characterModels
			where m.State != EActionState.Escaped
			select m.ClientId into id
			orderby id
			select id).ToList();
		if (list.Count >= 1)
		{
			int num = list.IndexOf(RxViewTargetClientId.Value);
			RxViewTargetClientId.Value = ((num < 0) ? list.First() : list[(num + 1) % list.Count]);
		}
	}

	public void ClearStageResult()
	{
		StageResultInfo = null;
	}

	private void BeginWaitStageResultLocalInfos()
	{
		_waitingStageResultLocalInfos = true;
		_loungeMoveRequested = false;
		if (_waitLocalInfoCts != null)
		{
			_waitLocalInfoCts.Cancel();
		}
		if (_waitLocalInfoCts != null)
		{
			_waitLocalInfoCts.Dispose();
		}
		_waitLocalInfoCts = new CancellationTokenSource();
		UniTask.Delay(System.TimeSpan.FromSeconds(WAIT_LOCALINFO_TIMEOUT), ignoreTimeScale: false, PlayerLoopTiming.Update, _waitLocalInfoCts.Token).ContinueWith(delegate
		{
			if (_waitingStageResultLocalInfos)
			{
				CompleteWaitAndMoveToLounge();
			}
		}).Forget();
	}

	private void TryCompleteLocalInfoCollection()
	{
		if (_waitingStageResultLocalInfos && !_loungeMoveRequested)
		{
			int num = StageResultInfo?.LocalInfos?.Select((StageResultLocalInfo li) => li.ClientId).Distinct().Count() ?? 0;
			if (num >= _gameContext.Players.Count)
			{
				CompleteWaitAndMoveToLounge();
			}
		}
	}

	private void CompleteWaitAndMoveToLounge()
	{
		if (!_loungeMoveRequested)
		{
			_waitingStageResultLocalInfos = false;
			_loungeMoveRequested = true;
			if (_waitLocalInfoCts != null)
			{
				_waitLocalInfoCts.Cancel();
				_waitLocalInfoCts.Dispose();
				_waitLocalInfoCts = null;
			}
			UniTask.Delay(System.TimeSpan.FromSeconds(WAIT_LOUNGE_MOVE_DELAY)).ContinueWith(MoveToLounge).Forget();
		}
	}

	private void OnReqStageResultPlayerLocalInfo(ReqStageResultPlayerLocalInfo req)
	{
		if (req == null || req.LocalInfo == null)
		{
			return;
		}
		List<StageResultLocalInfo> list = StageResultInfo.LocalInfos.ToList();
		int num = list.FindIndex((StageResultLocalInfo li) => li.ClientId == req.LocalInfo.ClientId);
		if (num < 0)
		{
			list.Add(req.LocalInfo);
		}
		else
		{
			list[num] = req.LocalInfo;
		}
		StageResultInfo.LocalInfos = list.OrderBy((StageResultLocalInfo li) => li.ClientId).ToArray();
		_gameHost.SendToAll(new NtfStageResultLocalInfo
		{
			LocalInfo = req.LocalInfo
		});
		TryCompleteLocalInfoCollection();
	}

	private void OnRequestUserChat(ReqUserChat message)
	{
		CharacterModel characterModel = _characterModels.FirstOrDefault((CharacterModel m) => m.ClientId == message.SenderId);
		if (characterModel == null)
		{
			_logger.Error(string.Format("[Chat] Character not found for ClientId: {0}", message.SenderId));
			return;
		}
		_gameHost.SendToAll(new NtfUserChat
		{
			ClientId = message.SenderId,
			Text = "[" + characterModel.PlayerName + "] " + message.Text
		});
	}

	private void OnReqClientIntroFinished(ReqClientIntroFinished req)
	{
		ulong num = ((req.SenderId == 0) ? req.ClientId : req.SenderId);
		if (_gameContext.Players.ContainsKey(num) && _introFinishedClientIds.Add(num))
		{
			TryCompleteIntroFinish();
		}
	}

	private void TryCompleteIntroFinish()
	{
		if (_introFinishedClientIds.Count >= _gameContext.Players.Count && _gameContext.Players.Count > 0)
		{
			_gameHost.SendToAll(new NtfAllClientsIntroFinished());
		}
	}

	private void OnAllClientsIntroFinished(NtfAllClientsIntroFinished message)
	{
		if (RxStageState.Value == EStageState.Intro)
		{
			RxStageState.Value = EStageState.Playing;
		}
	}

	public void StartVoteGiveUpCampaign()
	{
		_gameHost.SendToAll(new NtfVoteGiveUpCampaign
		{
			ClientId = _networkManager.LocalClientId,
			IsAgreed = true
		});
		_logger.Info("", Color.green);
	}

	public async UniTask BackToSinglePlay(GamePlayerData playerData, ERoomDifficulty difficulty, bool isUnexpectedBackToSinglePlay = false)
	{
		_isUnexpectedBackToSinglePlay = isUnexpectedBackToSinglePlay;
		StageResultInfo = null;
		_gameChannel.RxConnectState.Value = ERoomConnectState.BackToLocalRoom;
		_popupManager.CloseAllWithoutSfx(System.Array.Empty<System.Type>());
		await _sceneManager.LoadSceneAsync("Scene/LoadingScene.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
		await _gameChannel.ShutdownAsync();
		await _gameChannel.RecreateTransportAsync();
		if (_settings.Platform == EPlatform.Local)
		{
			_gameHost.SetUp(CachedAddress, CachedPort);
		}
		_gameChannel.RxConnectState.Value = ERoomConnectState.None;
		_gameHost.CreateRoom(difficulty);
		await _gameChannel.TryJoinGame(playerData);
		if (_networkManager.IsHost)
		{
			_logger.Info("", Color.green);
			MoveToLounge();
		}
		else
		{
			_logger.Warning("Player is not a host.");
		}
	}

	public void ClearUnexpectedBackToSinglePlay()
	{
		_isUnexpectedBackToSinglePlay = false;
	}

	internal void SetMissionClear(bool value)
	{
		IsMyMissionClear = value;
	}

	private void TriggerSessionStart()
	{
		_sessionUnlockedTalents.Clear();
		if (_analyticsDisposable != null)
		{
			_analyticsDisposable.Dispose();
		}
		_analyticsDisposable = new CompositeDisposable();
		_gameChannel.Receive<ResUnlockTalent>().Where((ResUnlockTalent r) => r.IsSuccess).Subscribe(delegate(ResUnlockTalent r)
		{
			_sessionUnlockedTalents.Add(r.TalentId);
		}).AddTo(_analyticsDisposable);
		int myCharacterId = GetMyCharacterId();
		EDifficulty difficulty = _gameContext.Difficulty;
		_eventSystem.Trigger(new PlaySessionStartEvent(DifficultyToRound(difficulty), myCharacterId, _gameContext.Players.Count));
	}

	private void TriggerSessionEnd()
	{
		bool flag = _gameContext.IsInStage && !_stageEnded;
		if (flag)
		{
			TriggerStageAbort();
		}
		if (_analyticsDisposable != null)
		{
			_analyticsDisposable.Dispose();
		}
		_analyticsDisposable = null;
		int myCharacterId = GetMyCharacterId();
		EDifficulty difficulty = _gameContext.Difficulty;
		string unlockTalent = ((_sessionUnlockedTalents.Count < 1) ? null : string.Join(",", _sessionUnlockedTalents));
		_eventSystem.Trigger(new PlaySessionEndEvent(DifficultyToRound(difficulty), myCharacterId, _gameContext.Players.Count, unlockTalent, flag));
	}

	internal void TriggerStageStart()
	{
		_stageEnded = false;
		IsMyMissionClear = false;
		int campaignId = _gameContext.CampaignId;
		EDifficulty difficulty = _gameContext.Difficulty;
		CharacterModel myCharacterModel = MyCharacterModel;
		int characterId = ((myCharacterModel == null) ? 0 : myCharacterModel.CharacterId);
		_eventSystem.Trigger(new PlayStageStartEvent(campaignId, DifficultyToRound(difficulty), characterId, _gameContext.Players.Count, GetAnomalyId(), BuildUseSkillString(), BuildUseEquipmentString()));
	}

	internal void TriggerStageEnd(bool isEscaped)
	{
		_stageEnded = true;
		int campaignId = _gameContext.CampaignId;
		EDifficulty difficulty = _gameContext.Difficulty;
		CharacterModel myCharacterModel = MyCharacterModel;
		int characterId = ((myCharacterModel == null) ? 0 : myCharacterModel.CharacterId);
		(int? soft, int? hard) tuple = CalculateGainCurrency();
		_eventSystem.Trigger(new PlayStageEndEvent(campaignId, DifficultyToRound(difficulty), characterId, isEscaped, IsMyMissionClear, isAborted: false, _gameContext.Players.Count, GetAnomalyId(), GetArtifactCount(), tuple.soft, tuple.hard));
	}

	private void TriggerStageAbort()
	{
		_stageEnded = true;
		int campaignId = _gameContext.CampaignId;
		EDifficulty difficulty = _gameContext.Difficulty;
		CharacterModel myCharacterModel = MyCharacterModel;
		int characterId = ((myCharacterModel == null) ? 0 : myCharacterModel.CharacterId);
		_eventSystem.Trigger(new PlayStageEndEvent(campaignId, DifficultyToRound(difficulty), characterId, isEscaped: false, isMissionClear: false, isAborted: true, _gameContext.Players.Count, GetAnomalyId(), GetArtifactCount()));
	}

	private int GetMyCharacterId()
	{
		if (MyCharacterModel != null)
		{
			return MyCharacterModel.CharacterId;
		}
		_gameContext.Players.TryGetValue(_networkManager.LocalClientId, out var value);
		return (value?.Profile?.SelectedCharacterId).GetValueOrDefault();
	}

	private int? GetAnomalyId()
	{
		EAnomalyType anomalyType = _gameContext.AnomalyType;
		if (anomalyType == EAnomalyType.None)
		{
			return null;
		}
		return (int)anomalyType;
	}

	private string BuildUseSkillString()
	{
		if (_gameContext.Players.TryGetValue(_networkManager.LocalClientId, out var value))
		{
			IEnumerable<int> enumerable = value.Character?.SelectedActiveSkillIds?.Where((int id) => id > 0);
			if (enumerable != null && enumerable.Any())
			{
				return string.Join(",", enumerable);
			}
		}
		return null;
	}

	private string BuildUseEquipmentString()
	{
		if (MyCharacterModel == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		if (MyCharacterModel.WeaponId > 0)
		{
			list.Add(MyCharacterModel.WeaponId);
		}
		list.AddRange(MyCharacterModel.AccessoryIds);
		if (list.Count < 1)
		{
			return null;
		}
		return string.Join(",", list);
	}

	private int? GetArtifactCount()
	{
		CharacterModel myCharacterModel = MyCharacterModel;
		if (myCharacterModel != null)
		{
			int num = myCharacterModel.Artifacts.Count();
			if (0 < num)
			{
				return num;
			}
		}
		return null;
	}

	private (int?, int?) CalculateGainCurrency()
	{
		if (!_gameContext.Players.TryGetValue(_networkManager.LocalClientId, out var value))
		{
			return (null, null);
		}
		Character character = value.Character;
		if (character == null)
		{
			return (null, null);
		}
		IEnumerable<GoodsSlot> enumerable = Enumerable.Empty<GoodsSlot>().Concat(character.WeaponSlots ?? Enumerable.Empty<GoodsSlot>()).Concat(character.AccessorySlots ?? Enumerable.Empty<GoodsSlot>()).Concat(character.ArtifactSlots ?? Enumerable.Empty<GoodsSlot>())
			.Concat(value.InventorySlots?.Values ?? Enumerable.Empty<GoodsSlot>())
			.Concat(value.SafeInventorySlots?.Values ?? Enumerable.Empty<GoodsSlot>());
		int soft = 0;
		int hard = 0;
		foreach (GoodsSlot item in enumerable)
		{
			AccumulateGainWorth(item?.Goods, ref soft, ref hard);
		}
		int num = ((hard < 1) ? 0 : (hard / TConst.CoinToGoldRate.IntValue));
		int? item2 = null;
		if (soft >= 1)
		{
			item2 = soft;
		}
		int? item3 = null;
		if (num > 0)
		{
			item3 = num;
		}
		return (item2, item3);
	}

	private static void AccumulateGainWorth(Goods goods, ref int soft, ref int hard)
	{
		if (goods == null)
		{
			return;
		}
		if (goods is BagGoods bagGoods)
		{
			if (bagGoods.GainCount > 0)
			{
				soft += bagGoods.GainWorth;
			}
			if (bagGoods.GoodsSlots != null)
			{
				GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
				foreach (GoodsSlot goodsSlot in goodsSlots)
				{
					AccumulateGainWorth(goodsSlot?.Goods, ref soft, ref hard);
				}
			}
		}
		else if (goods.GainCount > 0)
		{
			if (goods.Type == EGoodsType.Artifact)
			{
				hard += goods.GainWorth;
			}
			else
			{
				soft += goods.GainWorth;
			}
		}
	}

	internal static int DifficultyToRound(EDifficulty difficulty)
	{
		return (int)(difficulty + 1);
	}

	private void LoadCheatPreferences()
	{
		bool flag = _settings.Platform == EPlatform.Local;
		_ignoreTutorial = PlayerPrefs.GetInt("Cheat_IgnoreTutorial", flag ? 1 : 0) == 1;
		_ignoreLoungeTutorial = PlayerPrefs.GetInt("Cheat_IgnoreLoungeTutorial", flag ? 1 : 0) == 1;
	}

	public void SetIgnoreTutorial(bool value)
	{
		_ignoreTutorial = value;
		PlayerPrefs.SetInt("Cheat_IgnoreTutorial", value ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetIgnoreLoungeTutorial(bool value)
	{
		_ignoreLoungeTutorial = value;
		PlayerPrefs.SetInt("Cheat_IgnoreLoungeTutorial", value ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetMissionIds(int[] value)
	{
		MissionIds = value;
	}

	private void OnClientStarted()
	{
		if (!(_networkManager == null) && _networkManager.SceneManager != null)
		{
			_networkManager.SceneManager.OnSceneEvent -= OnSceneEvent;
			_networkManager.SceneManager.OnSceneEvent += OnSceneEvent;
		}
	}

	private void OnClientStopped(bool _)
	{
		if (!(_networkManager == null) && _networkManager.SceneManager != null)
		{
			_networkManager.SceneManager.OnSceneEvent -= OnSceneEvent;
		}
	}
}

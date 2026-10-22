using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.LcAnalytics;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using VContainer;

public class AnalyticsController : MonoBase
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private LcAnalyticsClient _client;

	private const string _liveEndpoint = "https://cdp.platform.loadcomplete.com";

	private const string _liveWriteKey = "3CkVu3uhsaTrm7ma6ZAOwd8s0HT";

	private const string _devEndpoint = "https://cdp.dev.platform.loadcomplete.com/";

	private const string _devWriteKey = "3CkXPe1gf8rWNdCe4baXql5VLzu";

	private long _sessionId;

	private bool _isFirstEvent = true;

	private string _sessionPlayId;

	private string _campaignPlayId;

	private string _stagePlayId;

	private float _sessionPlayStartTime;

	private float _campaignPlayStartTime;

	private float _stagePlayStartTime;

	private readonly Dictionary<(int, int), int> _stageRepCounts = new Dictionary<(int, int), int>();

	private readonly Dictionary<(int, int), int> _campaignRepCounts = new Dictionary<(int, int), int>();

	private int _fpsFrameCount;

	private float _fpsTimeElapsed;

	private float _appCheckAccumulated;

	private const float APP_CHECK_INTERVAL = 300f;

	private bool _hasFocus = true;

	private readonly Dictionary<string, float> _errorCooldown = new Dictionary<string, float>();

	private const float ERROR_COOLDOWN_SEC = 30f;

	internal string ULoungeId { get; private set; }

	internal string MultiHostId { get; private set; }

	protected override void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		_sessionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		if (_environmentSettings.UseGameAnalytics)
		{
			_client = ((_environmentSettings.Environment == EEnvironment.Live) ? new LcAnalyticsClient("https://cdp.platform.loadcomplete.com", "3CkVu3uhsaTrm7ma6ZAOwd8s0HT", 5) : new LcAnalyticsClient("https://cdp.dev.platform.loadcomplete.com/", "3CkXPe1gf8rWNdCe4baXql5VLzu", 5));
		}
	}

	private void Start()
	{
		_eventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
		Application.logMessageReceived += OnLogMessageReceived;
		Application.quitting += OnApplicationQuitting;
		MainThreadDispatcher.RegisterUnhandledExceptionCallback(OnUnhandledException);
		_roomMatchmaker.OnCreateLobby.Subscribe(delegate
		{
			OnLoungeCreated();
		}).AddTo(this);
		_roomMatchmaker.OnJoinLobby.Subscribe(OnLoungeJoined).AddTo(this);
		_roomMatchmaker.OnLeaveLobby.Subscribe(delegate
		{
			OnLoungeLeft();
		}).AddTo(this);
	}

	private void Update()
	{
		_fpsFrameCount++;
		_fpsTimeElapsed += Time.unscaledDeltaTime;
		_appCheckAccumulated += Time.unscaledDeltaTime;
		if (_appCheckAccumulated >= 300f)
		{
			_appCheckAccumulated -= 300f;
			int avgFps = ((_fpsTimeElapsed > 0f) ? ((int)((float)_fpsFrameCount / _fpsTimeElapsed)) : 0);
			_fpsFrameCount = 0;
			_fpsTimeElapsed = 0f;
			SendAppCheck(avgFps);
		}
	}

	private void OnDestroy()
	{
		Application.logMessageReceived -= OnLogMessageReceived;
		Application.quitting -= OnApplicationQuitting;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (_hasFocus != hasFocus)
		{
			_hasFocus = hasFocus;
			SendLoginStep(new LoginStepEvent(hasFocus ? ELoginStep.RESUME : ELoginStep.BACKGROUND, ELoginDtl.SUCCESS, ELoginRsn.AUTO, ELoginType.LC, 0L));
		}
	}

	private void OnApplicationQuitting()
	{
		SendLoginStep(new LoginStepEvent(ELoginStep.SESSION_OUT, ELoginDtl.SUCCESS, ELoginRsn.AUTO, ELoginType.LC, 0L));
	}

	public void SendAppCheckNow()
	{
		int avgFps = ((_fpsTimeElapsed > 0f) ? ((int)((float)_fpsFrameCount / _fpsTimeElapsed)) : 0);
		_fpsFrameCount = 0;
		_fpsTimeElapsed = 0f;
		_appCheckAccumulated = 0f;
		SendAppCheck(avgFps);
	}

	private void OnLogMessageReceived(string message, string stackTrace, LogType type)
	{
		if ((type == LogType.Error || type == LogType.Assert || type == LogType.Exception) && !_logger.IsClosed)
		{
			if (string.IsNullOrEmpty(stackTrace))
			{
				stackTrace = new System.Diagnostics.StackTrace(1).ToString();
			}
			SendErrorDeferred(message, stackTrace, type).Forget();
		}
	}

	private async UniTaskVoid SendErrorDeferred(string message, string stackTrace, LogType type)
	{
		await UniTask.Yield();
		if (!_logger.IsClosed)
		{
			SendError(message, stackTrace, type);
		}
	}

	private void OnUnhandledException(Exception ex)
	{
		if (!_logger.IsClosed)
		{
			// TODO(decomp): Type virtual getter at vtable slot 0x1b8 read as Name; could be FullName @0x18289fe30
			SendError(ex.GetType().Name + ": " + ex.Message, ex.ToString(), LogType.Exception);
			UnityEngine.Debug.LogException(ex);
		}
	}

	private void OnLoungeCreated()
	{
		MatchingLobby lobby = _roomMatchmaker.JoinedLobby;
		if (lobby != null)
		{
			ULoungeId = lobby.LobbyId.ToString();
			MultiHostId = _appManager.AccountId;
			EMultiMod mod = DifficultyToMultiMod(lobby.RoomDifficulty);
			EMultiOpen open = ((lobby.PublishType == ERoomPublishType.Private) ? EMultiOpen.Private : EMultiOpen.Public);
			SendMulti(EMultiTag.Create, ULoungeId, mod, open, lobby.Title, 1);
		}
	}

	private void OnLoungeJoined(bool isOwner)
	{
		MatchingLobby lobby = _roomMatchmaker.JoinedLobby;
		if (lobby != null)
		{
			ULoungeId = lobby.LobbyId.ToString();
			MultiHostId = (isOwner ? _appManager.AccountId : lobby.HostId.ToString());
			EMultiMod mod = DifficultyToMultiMod(lobby.RoomDifficulty);
			EMultiOpen open = ((lobby.PublishType == ERoomPublishType.Private) ? EMultiOpen.Private : EMultiOpen.Public);
			SendMulti(isOwner ? EMultiTag.Create : EMultiTag.Join, ULoungeId, mod, open, lobby.Title, lobby.MemberCount);
		}
	}

	private void OnLoungeLeft()
	{
		MatchingLobby lobby = _roomMatchmaker.JoinedLobby;
		string uLoungeId = ULoungeId ?? lobby?.LobbyId.ToString() ?? "";
		EMultiMod mod = ((lobby != null) ? DifficultyToMultiMod(lobby.RoomDifficulty) : EMultiMod.Normal);
		EMultiOpen open = ((lobby != null && lobby.PublishType == ERoomPublishType.Private) ? EMultiOpen.Private : EMultiOpen.Public);
		string name = lobby?.Title ?? "";
		int rslt = ((lobby != null) ? Math.Max(0, lobby.MemberCount - 1) : 0);
		SendMulti(EMultiTag.Leave, uLoungeId, mod, open, name, rslt);
		ULoungeId = null;
	}

	private static EMultiMod DifficultyToMultiMod(ERoomDifficulty difficulty)
	{
		switch (difficulty)
		{
		case ERoomDifficulty.Story:
			return EMultiMod.Story;
		case ERoomDifficulty.Junior:
			return EMultiMod.Normal;
		case ERoomDifficulty.Classic:
			return EMultiMod.Hard;
		case ERoomDifficulty.Veteran:
			return EMultiMod.Extreme;
		default:
			return EMultiMod.Normal;
		}
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
		if (baseEvent is TutorialEvent tutorialEvent)
		{
			SendTutorialStep(tutorialEvent.EventName);
		}
		else if (baseEvent is LevelEvent levelEvent)
		{
			SendUserLevel(levelEvent.Level);
		}
		else if (baseEvent is GainGoldEvent gainGoldEvent)
		{
			SendGainGold(gainGoldEvent.GainType, gainGoldEvent.GainTypeValue, gainGoldEvent.Amount);
		}
		else if (baseEvent is ConsumeGoldEvent consumeGoldEvent)
		{
			SendConsumeGold(consumeGoldEvent.ConsumeType, consumeGoldEvent.ConsumeTypeValue, consumeGoldEvent.Amount);
		}
		else if (baseEvent is CampaignStartEvent campaignStartEvent)
		{
			SendCampaignStart(campaignStartEvent.CampaignId, campaignStartEvent.Difficulty, campaignStartEvent.Players);
		}
		else if (baseEvent is CampaignEndEvent campaignEndEvent)
		{
			SendCampaignEnd(campaignEndEvent.CampaignId, campaignEndEvent.Difficulty, campaignEndEvent.Players, campaignEndEvent.Reason);
		}
		else if (baseEvent is StageStartEvent stageStartEvent)
		{
			SendStageStart(stageStartEvent.CampaignId, stageStartEvent.Difficulty, stageStartEvent.Players);
		}
		else if (baseEvent is StageEndEvent stageEndEvent)
		{
			SendStageEnd(stageEndEvent.CampaignId, stageEndEvent.Difficulty, stageEndEvent.Players, stageEndEvent.IsEscaped);
		}
		else if (baseEvent is LoginStepEvent loginStepEvent)
		{
			SendLoginStep(loginStepEvent);
		}
		else if (baseEvent is CampaignClearEvent campaignClearEvent)
		{
			SendUserinflowCampaignClear(campaignClearEvent);
		}
		else if (baseEvent is LoungeQuestClearEvent loungeQuestClearEvent)
		{
			SendUserinflowLoungeQuestClear(loungeQuestClearEvent);
		}
		else if (baseEvent is AchievementEvent achievementEvent)
		{
			SendAchievement(achievementEvent);
		}
		else if (baseEvent is TycoonFinishEvent tycoonFinishEvent)
		{
			SendTycoonFinish(tycoonFinishEvent);
		}
		else if (baseEvent is ArtifactDealEvent artifactDealEvent)
		{
			SendArtifactDeal(artifactDealEvent);
		}
		else if (baseEvent is PlaySessionStartEvent playSessionStartEvent)
		{
			SendSessionStart(playSessionStartEvent);
		}
		else if (baseEvent is PlaySessionEndEvent playSessionEndEvent)
		{
			SendSessionEnd(playSessionEndEvent);
		}
		else if (baseEvent is PlayCampaignStartEvent playCampaignStartEvent)
		{
			SendCampaignPlayStart(playCampaignStartEvent);
		}
		else if (baseEvent is PlayCampaignEndEvent playCampaignEndEvent)
		{
			SendCampaignPlayEnd(playCampaignEndEvent);
		}
		else if (baseEvent is PlayStageStartEvent playStageStartEvent)
		{
			SendStagePlayStart(playStageStartEvent);
		}
		else if (baseEvent is PlayStageEndEvent playStageEndEvent)
		{
			SendStagePlayEnd(playStageEndEvent);
		}
		else if (baseEvent is PlayingCrisisEvent playingCrisisEvent)
		{
			SendPlayingCrisis(playingCrisisEvent);
		}
		else if (baseEvent is AppCheckCheatEvent)
		{
			SendAppCheckNow();
		}
	}

	private T MakeEvent<T>(T e) where T : GameAnalyticsEvent
	{
		e.UserId = _appManager.AccountId;
		e.Context.Os.Name = SystemInfo.operatingSystem;
		e.Context.Os.Version = SystemInfo.operatingSystem;
		DateTime installTime = new DateTime(_playerManager.CreateTimeTicks, DateTimeKind.Utc);
		e.Context.Traits.InstallDatetime = installTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
		e.Context.Traits.InstallDate = installTime.ToString("yyyy-MM-dd");
		e.Context.Traits.InstallVersion = Application.version;
		// TODO(decomp): RegionInfo virtual getter at vtable slot 0x238 read as TwoLetterISORegionName @0x180d93c80
		e.Context.Traits.Country = RegionInfo.CurrentRegion.TwoLetterISORegionName;
		e.Context.Traits.DevBuild = _environmentSettings.Environment != EEnvironment.Live;
		e.Context.SessionId = _sessionId;
		e.Context.SessionStart = _isFirstEvent;
		e.Context.Timezone = TimeZoneInfo.Local.Id;
		// TODO(decomp): CultureInfo virtual getter at vtable slot 0x1a8 read as Name @0x180d93c80
		e.Context.Locale = CultureInfo.CurrentCulture.Name;
		_isFirstEvent = false;
		e.Properties.UserState.Playtime = _playerManager.PlayTimeSeconds;
		e.Properties.UserState.BestStage = _playerManager.BestStage;
		e.Properties.UserState.AccountPowerScore = _playerManager.AccountPowerScore;
		e.Properties.UserState.ULoungeId = ULoungeId;
		e.Properties.UserState.MultiHostId = MultiHostId;
		return e;
	}

	private void SendTutorialStep(string tutorialEvent)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug("tutorialEvent:" + tutorialEvent, Color.magenta);
		}
	}

	private void SendUserLevel(int level)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("level:{0}", level), Color.magenta);
			return;
		}
		LevelAnalyticsEvent levelAnalyticsEvent = MakeEvent(new LevelAnalyticsEvent());
		levelAnalyticsEvent.Properties.Level = level;
		_client.Send(levelAnalyticsEvent);
	}

	private void SendGainGold(EGoldGainType gainType, string value, long amount)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("{0}, amount:{1}, {2}", gainType, amount, value), Color.magenta);
		}
	}

	private void SendConsumeGold(EGoldConsumeType consumeType, string value, long amount)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("{0}, amount:{1}, {2}", consumeType, amount, value), Color.magenta);
		}
	}

	private void SendCampaignStart(int campaignId, EDifficulty difficulty, int playerCount)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("campaignId:{0}, difficulty:{1}, playerCount:{2}", campaignId, difficulty, playerCount), Color.magenta);
		}
	}

	private void SendCampaignEnd(int campaignId, EDifficulty difficulty, int playerCount, ECampaignCloseReason reason)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("campaignId:{0}, difficulty:{1}, playerCount:{2}, reason:{3}", campaignId, difficulty, playerCount, reason), Color.magenta);
		}
	}

	private void SendStageStart(int campaignId, EDifficulty difficulty, int playerCount)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("campaignId:{0}, difficulty:{1}, playerCount:{2}", campaignId, difficulty, playerCount), Color.magenta);
		}
	}

	private void SendStageEnd(int campaignId, EDifficulty difficulty, int playerCount, bool isEscaped)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("campaignId:{0}, difficulty:{1}, playerCount:{2}, isEscaped:{3}", campaignId, difficulty, playerCount, isEscaped), Color.magenta);
		}
	}

	private void SendAppCheck(int avgFps)
	{
		int memoryUsageMb = (int)(Profiler.GetTotalAllocatedMemoryLong() / 1048576);
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[app_check] fps:{0} memory_usage_mb:{1}", avgFps, memoryUsageMb), Color.magenta);
			return;
		}
		AppCheckAnalyticsEvent appCheckAnalyticsEvent = MakeEvent(new AppCheckAnalyticsEvent());
		appCheckAnalyticsEvent.Properties.Fps = avgFps;
		appCheckAnalyticsEvent.Properties.MemoryUsageMb = memoryUsageMb;
		_client.Send(appCheckAnalyticsEvent);
	}

	private void SendLoginStep(LoginStepEvent gameEvent)
	{
		if (string.IsNullOrEmpty(_appManager?.AccountId))
		{
			return;
		}
		string text = LoginStepToString(gameEvent.LoginStep);
		DeviceType deviceType = SystemInfo.deviceType;
		if (_environmentSettings.UseGameAnalytics)
		{
			LoginAnalyticsEvent loginAnalyticsEvent = MakeEvent(new LoginAnalyticsEvent());
			loginAnalyticsEvent.Properties.LoginCat = ((deviceType == DeviceType.Handheld) ? ELoginCat.MOBILE : ELoginCat.PC).ToString();
			loginAnalyticsEvent.Properties.LoginType = gameEvent.LoginType.ToString();
			loginAnalyticsEvent.Properties.LoginStep = text;
			loginAnalyticsEvent.Properties.Dtl = gameEvent.Dtl.ToString();
			loginAnalyticsEvent.Properties.Rsn = gameEvent.Rsn.ToString();
			loginAnalyticsEvent.Properties.EngagementTimeMsec = gameEvent.EngagementTimeMsec;
			_client.Send(loginAnalyticsEvent);
		}
		else
		{
			_logger.Debug(string.Format("[login] step:{0} dtl:{1} rsn:{2}", text, gameEvent.Dtl, gameEvent.Rsn), Color.magenta);
		}
	}

	private static string LoginStepToString(ELoginStep step)
	{
		switch (step)
		{
		case ELoginStep.SESSION_IN:
			return "100_SESSION_IN";
		case ELoginStep.SPLASH:
			return "200_SPLASH";
		case ELoginStep.LOGIN:
			return "300_LOGIN";
		case ELoginStep.RESUME:
			return "400_RESUME";
		case ELoginStep.BACKGROUND:
			return "500_BACKGROUND";
		case ELoginStep.SESSION_OUT:
			return "600_SESSION_OUT";
		default:
			return step.ToString();
		}
	}

	private void SendUserinflowCampaignClear(CampaignClearEvent gameEvent)
	{
		if (!gameEvent.IsFirstClear)
		{
			return;
		}
		string text = (TCampaign.Table[gameEvent.CampaignId].BitFlagIndex + 20000).ToString("D5");
		string text2 = string.Format("campaign_{0}_clear", gameEvent.CampaignId);
		if (_environmentSettings.UseGameAnalytics)
		{
			UserinflowAnalyticsEvent userinflowAnalyticsEvent = MakeEvent(new UserinflowAnalyticsEvent());
			userinflowAnalyticsEvent.Properties.InflowIndex = text;
			userinflowAnalyticsEvent.Properties.InflowCat = EInflowCat.INGAME.ToString();
			userinflowAnalyticsEvent.Properties.InflowValue = EInflowValue.COMPLETE.ToString();
			userinflowAnalyticsEvent.Properties.InflowDtl = text2;
			_client.Send(userinflowAnalyticsEvent);
		}
		else
		{
			_logger.Debug("[userinflow] INGAME/COMPLETE index:" + text + " " + text2, Color.magenta);
		}
	}

	private void SendUserinflowLoungeQuestClear(LoungeQuestClearEvent gameEvent)
	{
		string text = (TLoungeQuest.Table[gameEvent.LoungeQuestId].BitFlagIndex + 70000).ToString("D5");
		string text2 = string.Format("lounge_quest_{0}_clear", gameEvent.LoungeQuestId);
		if (_environmentSettings.UseGameAnalytics)
		{
			UserinflowAnalyticsEvent userinflowAnalyticsEvent = MakeEvent(new UserinflowAnalyticsEvent());
			userinflowAnalyticsEvent.Properties.InflowIndex = text;
			userinflowAnalyticsEvent.Properties.InflowCat = EInflowCat.INGAME.ToString();
			userinflowAnalyticsEvent.Properties.InflowValue = EInflowValue.COMPLETE.ToString();
			userinflowAnalyticsEvent.Properties.InflowDtl = text2;
			_client.Send(userinflowAnalyticsEvent);
		}
		else
		{
			_logger.Debug("[userinflow] INGAME/COMPLETE index:" + text + " " + text2, Color.magenta);
		}
	}

	private void SendSessionStart(PlaySessionStartEvent gameEvent)
	{
		_sessionPlayId = Guid.NewGuid().ToString("N");
		_sessionPlayStartTime = Time.realtimeSinceStartup;
		_stageRepCounts.Clear();
		_campaignRepCounts.Clear();
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYSTART cat:Session round:{0} players:{1}", gameEvent.Round, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYSTART.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Session.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _sessionPlayId;
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendSessionEnd(PlaySessionEndEvent gameEvent)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float sessionPlayStartTime = _sessionPlayStartTime;
		EPlayWin ePlayWin = (gameEvent.WasInterrupted ? EPlayWin.DRAW : EPlayWin.EXIT);
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYEND cat:Session round:{0} pwin:{1} players:{2}", gameEvent.Round, ePlayWin, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYEND.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Session.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _sessionPlayId;
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Pwin = ePlayWin.ToString();
		playendAnalyticsEvent.Properties.PwinDtl = (gameEvent.WasInterrupted ? EPlayWinDtl.UNKNOWN_INTERRUPT : EPlayWinDtl.BACK_TO_TITLE).ToString();
		playendAnalyticsEvent.Properties.Ptm = (int)(realtimeSinceStartup - sessionPlayStartTime);
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.UnlockTalent = gameEvent.UnlockTalent;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendCampaignPlayStart(PlayCampaignStartEvent gameEvent)
	{
		_campaignPlayId = Guid.NewGuid().ToString("N");
		_campaignPlayStartTime = Time.realtimeSinceStartup;
		(int, int) key = (gameEvent.CampaignId, gameEvent.Round);
		_campaignRepCounts.TryGetValue(key, out var value);
		value++;
		_campaignRepCounts[key] = value;
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYSTART cat:Campaign stg:{0} round:{1} players:{2}", gameEvent.CampaignId, gameEvent.Round, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYSTART.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Campaign.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _campaignPlayId;
		playendAnalyticsEvent.Properties.Stg = gameEvent.CampaignId.ToString();
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Rep = value.ToString();
		playendAnalyticsEvent.Properties.UseChar = gameEvent.CharacterId.ToString();
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.AnomalyId = gameEvent.AnomalyId;
		playendAnalyticsEvent.Properties.UseEquipment = gameEvent.UseEquipment;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendCampaignPlayEnd(PlayCampaignEndEvent gameEvent)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float campaignPlayStartTime = _campaignPlayStartTime;
		EPlayWin ePlayWin;
		EPlayWinDtl? ePlayWinDtl;
		switch (gameEvent.Reason)
		{
		case ECampaignCloseReason.Clear:
			ePlayWin = EPlayWin.WIN;
			ePlayWinDtl = EPlayWinDtl.CAMPAIGN_CLEAR;
			break;
		case ECampaignCloseReason.ClearFail:
			ePlayWin = EPlayWin.LOSE;
			ePlayWinDtl = EPlayWinDtl.MOB_KILL;
			break;
		case ECampaignCloseReason.Left:
			ePlayWin = EPlayWin.EXIT;
			ePlayWinDtl = EPlayWinDtl.USER_QUIT;
			break;
		case ECampaignCloseReason.GiveUp:
			ePlayWin = EPlayWin.EXIT;
			ePlayWinDtl = EPlayWinDtl.CAMPAIGN_QUIT;
			break;
		default:
			ePlayWin = EPlayWin.DRAW;
			ePlayWinDtl = EPlayWinDtl.UNKNOWN_INTERRUPT;
			break;
		}
		_campaignRepCounts.TryGetValue((gameEvent.CampaignId, gameEvent.Round), out var value);
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYEND cat:Campaign stg:{0} pwin:{1} players:{2}", gameEvent.CampaignId, ePlayWin, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYEND.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Campaign.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _campaignPlayId;
		playendAnalyticsEvent.Properties.Stg = gameEvent.CampaignId.ToString();
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Pwin = ePlayWin.ToString();
		playendAnalyticsEvent.Properties.PwinDtl = ePlayWinDtl?.ToString();
		playendAnalyticsEvent.Properties.Ptm = (int)(realtimeSinceStartup - campaignPlayStartTime);
		playendAnalyticsEvent.Properties.UseChar = gameEvent.CharacterId.ToString();
		playendAnalyticsEvent.Properties.Rep = value.ToString();
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.AnomalyId = gameEvent.AnomalyId;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendStagePlayStart(PlayStageStartEvent gameEvent)
	{
		_stagePlayId = Guid.NewGuid().ToString("N");
		_stagePlayStartTime = Time.realtimeSinceStartup;
		(int, int) key = (gameEvent.CampaignId, gameEvent.Round);
		_stageRepCounts.TryGetValue(key, out var value);
		value++;
		_stageRepCounts[key] = value;
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYSTART cat:Stage stg:{0} round:{1} players:{2}", gameEvent.CampaignId, gameEvent.Round, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYSTART.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Stage.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _stagePlayId;
		playendAnalyticsEvent.Properties.Stg = gameEvent.CampaignId.ToString();
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Rep = value.ToString();
		playendAnalyticsEvent.Properties.UseChar = gameEvent.CharacterId.ToString();
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.AnomalyId = gameEvent.AnomalyId;
		playendAnalyticsEvent.Properties.UseSkill = gameEvent.UseSkill;
		playendAnalyticsEvent.Properties.UseEquipment = gameEvent.UseEquipment;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendStagePlayEnd(PlayStageEndEvent gameEvent)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float stagePlayStartTime = _stagePlayStartTime;
		EPlayWin ePlayWin;
		EPlayWinDtl? ePlayWinDtl;
		if (gameEvent.IsAborted)
		{
			ePlayWin = EPlayWin.DRAW;
			ePlayWinDtl = EPlayWinDtl.UNKNOWN_INTERRUPT;
		}
		else if (gameEvent.IsEscaped && gameEvent.IsMissionClear)
		{
			ePlayWin = EPlayWin.WIN;
			ePlayWinDtl = EPlayWinDtl.CLEAR;
		}
		else if (gameEvent.IsEscaped)
		{
			ePlayWin = EPlayWin.EXIT;
			ePlayWinDtl = EPlayWinDtl.ESCAPE_ALIVE;
		}
		else
		{
			ePlayWin = EPlayWin.LOSE;
			ePlayWinDtl = EPlayWinDtl.MOB_KILL;
		}
		_stageRepCounts.TryGetValue((gameEvent.CampaignId, gameEvent.Round), out var value);
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYEND cat:Stage stg:{0} pwin:{1} players:{2}", gameEvent.CampaignId, ePlayWin, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYEND.ToString();
		playendAnalyticsEvent.Properties.PlayCat = EPlayCat.Stage.ToString();
		playendAnalyticsEvent.Properties.UPlayId = _stagePlayId;
		playendAnalyticsEvent.Properties.Stg = gameEvent.CampaignId.ToString();
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Pwin = ePlayWin.ToString();
		playendAnalyticsEvent.Properties.PwinDtl = ePlayWinDtl?.ToString();
		playendAnalyticsEvent.Properties.Ptm = (int)(realtimeSinceStartup - stagePlayStartTime);
		playendAnalyticsEvent.Properties.UseChar = gameEvent.CharacterId.ToString();
		playendAnalyticsEvent.Properties.Rep = value.ToString();
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.AnomalyId = gameEvent.AnomalyId;
		playendAnalyticsEvent.Properties.ArtifactCount = gameEvent.ArtifactCount;
		playendAnalyticsEvent.Properties.GetSoftCurrency = gameEvent.GetSoftCurrency;
		playendAnalyticsEvent.Properties.GetHardCurrency = gameEvent.GetHardCurrency;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendPlayingCrisis(PlayingCrisisEvent gameEvent)
	{
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[playend] PLAYING cat:{0} pwin:{1} players:{2}", gameEvent.PlayCat, gameEvent.Pwin, gameEvent.PlayerCount), Color.magenta);
			return;
		}
		PlayendAnalyticsEvent playendAnalyticsEvent = MakeEvent(new PlayendAnalyticsEvent());
		playendAnalyticsEvent.Properties.Tag = EPlayTag.PLAYING.ToString();
		playendAnalyticsEvent.Properties.PlayCat = gameEvent.PlayCat.ToString();
		playendAnalyticsEvent.Properties.UPlayId = gameEvent.UPlayId;
		playendAnalyticsEvent.Properties.Stg = gameEvent.Stg;
		playendAnalyticsEvent.Properties.Round = gameEvent.Round;
		playendAnalyticsEvent.Properties.Pwin = gameEvent.Pwin.ToString();
		playendAnalyticsEvent.Properties.Ptm = gameEvent.Ptm;
		playendAnalyticsEvent.Properties.UseChar = gameEvent.UseChar;
		playendAnalyticsEvent.Properties.PlayerCount = gameEvent.PlayerCount;
		playendAnalyticsEvent.Properties.AnomalyId = gameEvent.AnomalyId;
		playendAnalyticsEvent.Properties.UseSkill = gameEvent.UseSkill;
		playendAnalyticsEvent.Properties.UseEquipment = gameEvent.UseEquipment;
		_client.Send(playendAnalyticsEvent);
	}

	private void SendAchievement(AchievementEvent gameEvent)
	{
		if (_environmentSettings.UseGameAnalytics)
		{
			AchievementAnalyticsEvent achievementAnalyticsEvent = MakeEvent(new AchievementAnalyticsEvent());
			achievementAnalyticsEvent.Properties.Tag = gameEvent.Tag.ToString();
			achievementAnalyticsEvent.Properties.AchievementCat = gameEvent.AchievementCat.ToString();
			achievementAnalyticsEvent.Properties.UAchvId = gameEvent.UAchvId;
			achievementAnalyticsEvent.Properties.AchievementName = gameEvent.AchievementName;
			achievementAnalyticsEvent.Properties.AchievementId = gameEvent.AchievementId;
			achievementAnalyticsEvent.Properties.AchievementLv = gameEvent.AchievementLv;
			achievementAnalyticsEvent.Properties.AchievementRslt = gameEvent.AchievementRslt.ToString();
			achievementAnalyticsEvent.Properties.AchievementRsltDtl = gameEvent.AchievementRsltDtl;
			_client.Send(achievementAnalyticsEvent);
		}
		else
		{
			_logger.Debug(string.Format("[achievement] tag:{0} cat:{1} id:{2} rslt:{3}", gameEvent.Tag, gameEvent.AchievementCat, gameEvent.AchievementId, gameEvent.AchievementRslt), Color.magenta);
		}
	}

	private void SendTycoonFinish(TycoonFinishEvent gameEvent)
	{
		string text = gameEvent.Artifact.Id.ToString();
		EAchievementRslt eAchievementRslt = (gameEvent.IsPanic ? EAchievementRslt.FAIL : EAchievementRslt.SUCCESS);
		if (_environmentSettings.UseGameAnalytics)
		{
			AchievementAnalyticsEvent achievementAnalyticsEvent = MakeEvent(new AchievementAnalyticsEvent());
			achievementAnalyticsEvent.Properties.Tag = EAchievementTag.ACHIEVE.ToString();
			achievementAnalyticsEvent.Properties.AchievementCat = EAchievementCat.NEGOTIATION_TYCOON.ToString();
			achievementAnalyticsEvent.Properties.UAchvId = Guid.NewGuid().ToString("N");
			achievementAnalyticsEvent.Properties.AchievementName = text;
			achievementAnalyticsEvent.Properties.AchievementId = text;
			achievementAnalyticsEvent.Properties.AchievementRslt = eAchievementRslt.ToString();
			achievementAnalyticsEvent.Properties.AchievementRsltDtl = string.Format("{0:D7},{1}", gameEvent.EmployeeId, (int)gameEvent.ResultCoin);
			_client.Send(achievementAnalyticsEvent);
		}
		else
		{
			_logger.Debug(string.Format("[achievement] NEGOTIATION_TYCOON artifact:{0} rslt:{1} coin:{2}", text, eAchievementRslt, (int)gameEvent.ResultCoin), Color.magenta);
		}
	}

	private void SendArtifactDeal(ArtifactDealEvent gameEvent)
	{
		string text = gameEvent.ArtifactItemId.ToString();
		EAchievementRslt eAchievementRslt = EAchievementRslt.SUCCESS;
		int num;
		if (!gameEvent.IsAccepted)
		{
			eAchievementRslt = EAchievementRslt.GIVE_UP;
			num = 0;
		}
		else
		{
			num = gameEvent.Price;
		}
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Format("[achievement] NEGOTIATION_VIP npc:{0} artifact:{1} rslt:{2}", gameEvent.NpcId, text, eAchievementRslt), Color.magenta);
			return;
		}
		AchievementAnalyticsEvent achievementAnalyticsEvent = MakeEvent(new AchievementAnalyticsEvent());
		achievementAnalyticsEvent.Properties.Tag = EAchievementTag.ACHIEVE.ToString();
		achievementAnalyticsEvent.Properties.AchievementCat = EAchievementCat.NEGOTIATION_VIP.ToString();
		achievementAnalyticsEvent.Properties.UAchvId = Guid.NewGuid().ToString("N");
		achievementAnalyticsEvent.Properties.AchievementName = text;
		achievementAnalyticsEvent.Properties.AchievementId = text;
		achievementAnalyticsEvent.Properties.AchievementRslt = eAchievementRslt.ToString();
		achievementAnalyticsEvent.Properties.AchievementRsltDtl = string.Format("{0:D7},{1}", gameEvent.NpcId, num);
		_client.Send(achievementAnalyticsEvent);
	}

	private void SendMulti(EMultiTag tag, string uLoungeId, EMultiMod mod, EMultiOpen open, string name, int rslt)
	{
		if (_environmentSettings.UseGameAnalytics)
		{
			MultiAnalyticsEvent multiAnalyticsEvent = MakeEvent(new MultiAnalyticsEvent());
			multiAnalyticsEvent.Properties.Tag = tag.ToString();
			multiAnalyticsEvent.Properties.ULoungeId = uLoungeId;
			multiAnalyticsEvent.Properties.MultiMod = mod.ToString();
			multiAnalyticsEvent.Properties.MultiOpen = open.ToString();
			multiAnalyticsEvent.Properties.MultiName = name;
			multiAnalyticsEvent.Properties.MultiRslt = rslt;
			_client.Send(multiAnalyticsEvent);
		}
		else
		{
			_logger.Debug(string.Format("[multi] tag:{0} lounge:{1} mod:{2} rslt:{3}", tag, uLoungeId, mod, rslt), Color.magenta);
		}
	}

	private void SendError(string message, string stackTrace, LogType logType)
	{
		if (string.IsNullOrEmpty(message))
		{
			return;
		}
		string text = ComputeIssueId(message, stackTrace);
		if (_errorCooldown.TryGetValue(text, out var value) && Time.realtimeSinceStartup - value < 30f)
		{
			return;
		}
		_errorCooldown[text] = Time.realtimeSinceStartup;
		if (!_environmentSettings.UseGameAnalytics)
		{
			_logger.Debug(string.Concat("[error_report] severity:", ClassifySeverity(logType), " issue_id:", text, " ", message), Color.magenta);
			return;
		}
		ErrorReportAnalyticsEvent errorReportAnalyticsEvent = MakeEvent(new ErrorReportAnalyticsEvent());
		errorReportAnalyticsEvent.Properties.IssueId = text;
		errorReportAnalyticsEvent.Properties.VariantId = ComputeVariantId(text);
		errorReportAnalyticsEvent.Properties.IssueTitle = ExtractIssueTitle(message);
		errorReportAnalyticsEvent.Properties.IssueSubtitle = ExtractIssueSubtitle(stackTrace);
		errorReportAnalyticsEvent.Properties.Severity = ClassifySeverity(logType);
		errorReportAnalyticsEvent.Properties.ErrorType = ClassifyErrorType(message, stackTrace);
		errorReportAnalyticsEvent.Properties.LibraryName = ExtractLibraryName(stackTrace);
		errorReportAnalyticsEvent.Properties.FreeRamMb = GetFreeRamMb();
		errorReportAnalyticsEvent.Properties.FreeDiskMb = GetFreeDiskMb();
		errorReportAnalyticsEvent.Properties.LocationContext = SceneManager.GetActiveScene().name;
		_client.Send(errorReportAnalyticsEvent);
	}

	private static string ComputeIssueId(string message, string stackTrace)
	{
		string text = ExtractIssueTitle(message);
		string text2 = ExtractIssueSubtitle(stackTrace) ?? "";
		return ComputeMd5Short(text + ":" + text2);
	}

	private static string ComputeVariantId(string issueId)
	{
		return ComputeMd5Short(string.Concat(issueId, ":", SystemInfo.operatingSystem, ":", SystemInfo.deviceModel));
	}

	private static string ComputeMd5Short(string input)
	{
		using (MD5 mD = MD5.Create())
		{
			byte[] hash = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
			return BitConverter.ToString(hash).Replace("-", "").Substring(0, 16).ToLower();
		}
	}

	private static string ExtractIssueTitle(string message)
	{
		string text = message.Split('\n')[0].Trim();
		return (text.Length > 200) ? text.Substring(0, 200) : text;
	}

	private static string ExtractIssueSubtitle(string stackTrace)
	{
		if (string.IsNullOrEmpty(stackTrace))
		{
			return null;
		}
		string text = string.Join('\n', stackTrace.Split('\n').Select(ExtractFrame).Where((string f) => f != null)
			.Take(10));
		return (text.Length > 0) ? text : null;
	}

	private static string ExtractFrame(string line)
	{
		string text = line.Trim();
		string text2;
		int num = text.IndexOf("(at ", StringComparison.Ordinal);
		if (num < 0)
		{
			if (!text.StartsWith("at ", StringComparison.Ordinal))
			{
				return null;
			}
			int num2 = text.IndexOf(" [0x", StringComparison.Ordinal);
			if (num2 < 3)
			{
				return text;
			}
			text2 = text.Substring(3, num2 - 3);
			int num3 = text.LastIndexOf("] in ", StringComparison.Ordinal);
			if (num3 >= 0)
			{
				string text3 = text.Substring(num3 + 5);
				if (!text3.StartsWith("<") && text3.Contains(":"))
				{
					return string.Concat("at ", text2, " (", text3, ")");
				}
			}
		}
		else
		{
			int num4 = num + 4;
			int num5 = text.LastIndexOf(')');
			if (num5 <= num4)
			{
				return null;
			}
			text2 = text.Substring(num4, num5 - num4);
			if (text2.StartsWith("<"))
			{
				text2 = text.Substring(0, num).Trim();
				if (text2.Length < 1)
				{
					return null;
				}
			}
		}
		return "at " + text2;
	}

	private static string ExtractLibraryName(string stackTrace)
	{
		if (string.IsNullOrEmpty(stackTrace))
		{
			return null;
		}
		string[] array = stackTrace.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.StartsWith("at "))
			{
				int num = text.IndexOf('.', 3);
				if (num >= 0)
				{
					string text2 = text.Substring(3, num - 3);
					if (text2 != "System" && text2 != "Unity" && text2 != "UnityEngine")
					{
						return text2;
					}
				}
			}
		}
		return null;
	}

	private static string ClassifySeverity(LogType logType)
	{
		return (logType == LogType.Assert || logType == LogType.Exception) ? "FATAL" : "NON_FATAL";
	}

	private static string ClassifyErrorType(string message, string stackTrace)
	{
		string text = message + "\n" + stackTrace;
		if (ContainsAny(text, "AssetBundle", "Addressable", "Resources.Load"))
		{
			return "ASSET_LOAD_FAIL";
		}
		if (ContainsAny(text, "Network", "Http", "Timeout", "Socket", "WebRequest", "UnityWebRequest"))
		{
			return "NETWORK_TIMEOUT";
		}
		if (ContainsAny(text, "UnityAds", "BillingSDK", "Firebase", "Steamworks"))
		{
			return "PLUGIN_ERROR";
		}
		return "SCRIPT_ERROR";
	}

	private static bool ContainsAny(string text, params string[] keywords)
	{
		for (int i = 0; i < keywords.Length; i++)
		{
			if (text.IndexOf(keywords[i], StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	private static int GetFreeRamMb()
	{
		return Math.Max(0, SystemInfo.systemMemorySize - (int)(Profiler.GetTotalAllocatedMemoryLong() / 1048576));
	}

	private static int GetFreeDiskMb()
	{
		string pathRoot = Path.GetPathRoot(Path.GetFullPath(Application.persistentDataPath));
		return (int)(new DriveInfo(pathRoot).AvailableFreeSpace / 1048576);
	}
}

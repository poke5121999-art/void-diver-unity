using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.LcAnalytics;
using NemoLib.Log;
using UnityEngine;
using VContainer;

public class AnalyticsController : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSendErrorDeferred_003Ed__42 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public AnalyticsController _003C_003E4__this;

		public string message;

		public string stackTrace;

		public LogType type;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__1;

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

	private bool _isFirstEvent;

	private string _sessionPlayId;

	private string _campaignPlayId;

	private string _stagePlayId;

	private float _sessionPlayStartTime;

	private float _campaignPlayStartTime;

	private float _stagePlayStartTime;

	private readonly Dictionary<(int, int), int> _stageRepCounts;

	private readonly Dictionary<(int, int), int> _campaignRepCounts;

	private int _fpsFrameCount;

	private float _fpsTimeElapsed;

	private float _appCheckAccumulated;

	private const float APP_CHECK_INTERVAL = 300f;

	private bool _hasFocus;

	private readonly Dictionary<string, float> _errorCooldown;

	private const float ERROR_COOLDOWN_SEC = 30f;

	internal string ULoungeId { get; private set; }

	internal string MultiHostId { get; private set; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnApplicationFocus(bool hasFocus)
	{
	}

	private void OnApplicationQuitting()
	{
	}

	public void SendAppCheckNow()
	{
	}

	private void OnLogMessageReceived(string message, string stackTrace, LogType type)
	{
	}

	[AsyncStateMachine(typeof(_003CSendErrorDeferred_003Ed__42))]
	private UniTaskVoid SendErrorDeferred(string message, string stackTrace, LogType type)
	{
		return default;
	}

	private void OnUnhandledException(Exception ex)
	{
	}

	private void OnLoungeCreated()
	{
	}

	private void OnLoungeJoined(bool isOwner)
	{
	}

	private void OnLoungeLeft()
	{
	}

	private static EMultiMod DifficultyToMultiMod(ERoomDifficulty difficulty)
	{
		return EMultiMod.Story;
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
	}

	private T MakeEvent<T>(T e) where T : GameAnalyticsEvent
	{
		return null;
	}

	private void SendTutorialStep(string tutorialEvent)
	{
	}

	private void SendUserLevel(int level)
	{
	}

	private void SendGainGold(EGoldGainType gainType, string value, long amount)
	{
	}

	private void SendConsumeGold(EGoldConsumeType consumeType, string value, long amount)
	{
	}

	private void SendCampaignStart(int campaignId, EDifficulty difficulty, int playerCount)
	{
	}

	private void SendCampaignEnd(int campaignId, EDifficulty difficulty, int playerCount, ECampaignCloseReason reason)
	{
	}

	private void SendStageStart(int campaignId, EDifficulty difficulty, int playerCount)
	{
	}

	private void SendStageEnd(int campaignId, EDifficulty difficulty, int playerCount, bool isEscaped)
	{
	}

	private void SendAppCheck(int avgFps)
	{
	}

	private void SendLoginStep(LoginStepEvent gameEvent)
	{
	}

	private static string LoginStepToString(ELoginStep step)
	{
		return null;
	}

	private void SendUserinflowCampaignClear(CampaignClearEvent gameEvent)
	{
	}

	private void SendUserinflowLoungeQuestClear(LoungeQuestClearEvent gameEvent)
	{
	}

	private void SendSessionStart(PlaySessionStartEvent gameEvent)
	{
	}

	private void SendSessionEnd(PlaySessionEndEvent gameEvent)
	{
	}

	private void SendCampaignPlayStart(PlayCampaignStartEvent gameEvent)
	{
	}

	private void SendCampaignPlayEnd(PlayCampaignEndEvent gameEvent)
	{
	}

	private void SendStagePlayStart(PlayStageStartEvent gameEvent)
	{
	}

	private void SendStagePlayEnd(PlayStageEndEvent gameEvent)
	{
	}

	private void SendPlayingCrisis(PlayingCrisisEvent gameEvent)
	{
	}

	private void SendAchievement(AchievementEvent gameEvent)
	{
	}

	private void SendTycoonFinish(TycoonFinishEvent gameEvent)
	{
	}

	private void SendArtifactDeal(ArtifactDealEvent gameEvent)
	{
	}

	private void SendMulti(EMultiTag tag, string uLoungeId, EMultiMod mod, EMultiOpen open, string name, int rslt)
	{
	}

	private void SendError(string message, string stackTrace, LogType logType)
	{
	}

	private static string ComputeIssueId(string message, string stackTrace)
	{
		return null;
	}

	private static string ComputeVariantId(string issueId)
	{
		return null;
	}

	private static string ComputeMd5Short(string input)
	{
		return null;
	}

	private static string ExtractIssueTitle(string message)
	{
		return null;
	}

	private static string ExtractIssueSubtitle(string stackTrace)
	{
		return null;
	}

	private static string ExtractFrame(string line)
	{
		return null;
	}

	private static string ExtractLibraryName(string stackTrace)
	{
		return null;
	}

	private static string ClassifySeverity(LogType logType)
	{
		return null;
	}

	private static string ClassifyErrorType(string message, string stackTrace)
	{
		return null;
	}

	private static bool ContainsAny(string text, params string[] keywords)
	{
		return false;
	}

	private static int GetFreeRamMb()
	{
		return 0;
	}

	private static int GetFreeDiskMb()
	{
		return 0;
	}
}

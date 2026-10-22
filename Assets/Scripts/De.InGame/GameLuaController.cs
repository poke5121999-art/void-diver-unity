using GameCommon.Enum;
using NemoLib.Log;
using VContainer;

public class GameLuaController : MonoBase
{
	private const string STEP = "Step";

	private const string START_LOUNGE_FUNC = "OnLounge";

	private const string START_STAGE_FUNC = "OnStage";

	private const string EVENT_FUNC = "OnEvent";

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ILuaRunner _luaRunner;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameNpcTalkManager _npcTalkManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	private string CampaignLuaKey => null;

	private ulong MyClientId => 0uL;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private string GetMissionLuaKey(int missionId)
	{
		return null;
	}

	public void RunLua(string functionName, params object[] args)
	{
	}

	public void RunLua(string luaKey, string functionName, params object[] args)
	{
	}

	private void RunCampaignLua(string functionName, params object[] args)
	{
	}

	private void RunMissionLua(int missionId, string functionName, params object[] args)
	{
	}

	private void RunMissionLua(string functionName, params object[] args)
	{
	}

	private void RunLoungeQuestLua(string functionName, params object[] args)
	{
	}

	private void RunLoungeQuestLua(int loungeQuestId, string functionName, params object[] args)
	{
	}

	private void OnStageStarted()
	{
	}

	private void InitLoungeQuestLuaStates()
	{
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
	}

	private void OnCollisionTriggerEnter(NtfCollisionTriggerEnter message)
	{
	}

	private void OnWaveStarted(NtfWaveStarted message)
	{
	}

	private void OnWaveFinished(NtfWaveFinished message)
	{
	}

	private void OnTriggerLuaEvent(NtfTriggerLuaEvent message)
	{
	}

	private void OnEventOccured(IBaseEvent gameEvent)
	{
	}

	private void OnQuestStepChanged(int stepId)
	{
	}

	private void OnCampaignStateChanged(ECampaignState state)
	{
	}

	private void OnCampaignTaskAchieved(int taskId)
	{
	}

	private void OnMissionStepChanged(int missionId, int stepId)
	{
	}

	private void OnMissionStateChanged(int missionId, EMissionState state)
	{
	}

	private void OnMissionAchieved(int missionId)
	{
	}

	private void OnLoungeQuestAdded(LoungeQuest quest)
	{
	}

	private void OnLoungeQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) data)
	{
	}
}

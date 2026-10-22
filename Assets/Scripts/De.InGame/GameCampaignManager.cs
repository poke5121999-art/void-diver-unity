using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GameCampaignManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	public readonly Subject<(int id, bool isFirstClear)> OnCampaignCompleted;

	public readonly Subject<Unit> OnCampaignUpdated;

	public readonly Subject<int> OnTaskAchieved;

	private readonly ReactiveProperty<ECampaignState> _rxState;

	private readonly ReactiveProperty<int> _rxCampaignStepId;

	private readonly Dictionary<ulong, ECampaignState> _clientStates;

	public readonly ReactiveCollection<ulong> RxCampaignAgreeingClientIds;

	private int _votingCampaignId;

	private EDifficulty _votingDifficulty;

	private EAnomalyType _votingAnomalyType;

	public readonly Subject<Unit> OnGiveUpCampaignVoteStarted;

	public readonly Subject<bool> OnHostWaitOtherPlayerCampaignCompleted;

	public ReactiveCollection<ulong> RxCampaignGiveUpAgreeingClientIds;

	private int[] _playableNormalCampaignIds;

	private readonly Dictionary<int, EAnomalyType> _campaignAnomalyTypes;

	public int CampaignId => 0;

	public Dictionary<int, CampaignTask> CampaignTasks { get; private set; }

	public Subject<(int, EDifficulty, EAnomalyType)> OnCampaignVoteStarted { get; }

	public Subject<Unit> OnCampaignVoteCancelled { get; }

	public Subject<(int, EDifficulty)> OnCampaignStarted { get; }

	public Subject<Unit> OnCampaignClosed { get; }

	public IReadOnlyDictionary<ulong, ECampaignState> ClientStates => null;

	public IReadOnlyReactiveProperty<ECampaignState> RxState => null;

	public ECampaignState State => ECampaignState.None;

	public IReadOnlyReactiveProperty<int> RxCampaignStepId => null;

	public int StepId => 0;

	public int CampaignAgreeingCount => 0;

	public bool IsVotingCampaign => false;

	private int CampaignGiveUpAgreeingCount => 0;

	public bool IsVotingCampaignGiveUp => false;

	public long CurrentDialogReplayId { get; private set; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnGameStarted(Unit _)
	{
	}

	private void OnVoteCampaign(NtfVoteCampaign message)
	{
	}

	private void OnStartCampaign(NtfStartCampaign message)
	{
	}

	private void OnCloseCampaign(NtfCloseCampaign message)
	{
	}

	private void OnUpdateCampaignState(NtfUpdateCampaignState message)
	{
	}

	private void CheckOtherPlayerCampaignCompleted()
	{
	}

	private void CheckAllPlayerCampaignEnd()
	{
	}

	private void RefreshPlayableNormalCampaignIds()
	{
	}

	private void RefreshCampaignAnomalies()
	{
	}

	public bool OverrideAnomalyType(int campaignId, EAnomalyType anomalyType)
	{
		return false;
	}

	public EAnomalyType GetCampaignAnomalyType(int campaignId)
	{
		return EAnomalyType.None;
	}

	public int[] GetPlayableCampaignIds(ECampaignCategory category)
	{
		return null;
	}

	public bool HasNewStoryCampaign()
	{
		return false;
	}

	public ECampaignState GetPlayerCampaignState(ulong clientId)
	{
		return ECampaignState.None;
	}

	private void OnPlayerLeft(GamePlayer player)
	{
	}

	private void OnRoomClosed()
	{
	}

	private void Clear()
	{
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
	}

	public void SetCampaignState(ECampaignState questState)
	{
	}

	public void SetQuestStepId(int stepId)
	{
	}

	public bool IsAllTaskAchieved()
	{
		return false;
	}

	public bool IsTaskAchieved(int campaignTaskId)
	{
		return false;
	}

	public void CompleteQuest()
	{
	}

	public void FailQuest()
	{
	}

	public bool HasVoted(ulong clientId)
	{
		return false;
	}

	private void ClearVote()
	{
	}

	private void OnStageFinished()
	{
	}

	private void ClearVoteGiveUp()
	{
	}

	private void OnVoteGiveUpCampaign(NtfVoteGiveUpCampaign message)
	{
	}

	public bool HasVotedGiveUp(ulong clientId)
	{
		return false;
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
	}

	private void TriggerCampaignStart()
	{
	}

	private void TriggerCampaignEnd(ECampaignCloseReason reason)
	{
	}

	private int? GetCampaignAnomalyId()
	{
		return null;
	}

	private string BuildCampaignUseEquipment()
	{
		return null;
	}
}

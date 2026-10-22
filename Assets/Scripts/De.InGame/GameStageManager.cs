using VContainer;

public class GameStageManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameContext _gameContext;

	private long _accumulatedExp;

	private long _baseExpFromRewardBox;

	private long _baseExpFromMonsterKill;

	private long _baseExpFromEscape;

	private long _difficultyBonusExp;

	private long _talentBonusExp;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEventOccured(IBaseEvent gameEvent)
	{
	}

	private void AccumulateExp(long value)
	{
	}

	private void FlushAccumulatedExp()
	{
	}

	private void SendStageLocalInfo()
	{
	}

	private void ResetBuckets()
	{
	}
}

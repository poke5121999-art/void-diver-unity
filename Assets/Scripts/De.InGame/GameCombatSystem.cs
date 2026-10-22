using System.Collections.Generic;
using UniRx;
using VContainer;

public class GameCombatSystem : MonoBase
{
	private const float _aggroRange = 10f;

	private const float _combatCooldown = 3f;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameEventSystem _gameEventSystem;

	private readonly Dictionary<ulong, MonsterModel> _monsterModels;

	private readonly Dictionary<ulong, MonsterModel> _aggroedMonsterModels;

	private readonly Dictionary<ulong, MonsterModel> _aggroedBossModels;

	public ReactiveProperty<bool> RxIsInCombat;

	private float _combatEmptyTime;

	private readonly List<ulong> _monsterRemoveBuffer;

	private readonly List<ulong> _aggroedRemoveBuffer;

	private readonly List<ulong> _bossRemoveBuffer;

	public bool IsBossEncountered => false;

	public IEnumerable<MonsterModel> AggroedBossModels => null;

	public Subject<MonsterModel> OnBossEncountered { get; }

	public Subject<MonsterModel> OnBossDeparted { get; }

	public bool IsInCombat => false;

	private CharacterModel MyCharacterModel => null;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void OnMonsterTargetChanged(NtfMonsterTargetChanged message)
	{
	}

	private void OnHitCollision(NtfHitCollision message)
	{
	}

	private void OnHpDamage(NtfHpDamage message)
	{
	}

	private void UpdateMonsterModels()
	{
	}

	private void UpdateAggroedSets()
	{
	}

	private void UpdateAggroedMonster(ulong key, MonsterModel model, bool inRange)
	{
	}

	private void UpdateAggroedBoss(ulong key, MonsterModel model, bool inRange)
	{
	}

	private void TriggerAggroedEvent(MonsterModel monster)
	{
	}

	private void RemoveStaleAggroedMonsters()
	{
	}

	private void RemoveStaleAggroedBosses()
	{
	}

	private void UpdateCombatState()
	{
	}

	private void OnStageClosed()
	{
	}
}

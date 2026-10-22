using System.Collections.Generic;
using GameCommon.Enum;
using UniRx;
using UnityEngine;
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

	private readonly Dictionary<ulong, MonsterModel> _monsterModels = new Dictionary<ulong, MonsterModel>();

	private readonly Dictionary<ulong, MonsterModel> _aggroedMonsterModels = new Dictionary<ulong, MonsterModel>();

	private readonly Dictionary<ulong, MonsterModel> _aggroedBossModels = new Dictionary<ulong, MonsterModel>();

	public ReactiveProperty<bool> RxIsInCombat = new ReactiveProperty<bool>();

	private float _combatEmptyTime;

	private readonly List<ulong> _monsterRemoveBuffer = new List<ulong>();

	private readonly List<ulong> _aggroedRemoveBuffer = new List<ulong>();

	private readonly List<ulong> _bossRemoveBuffer = new List<ulong>();

	public bool IsBossEncountered => _aggroedBossModels.Count > 0;

	public IEnumerable<MonsterModel> AggroedBossModels => _aggroedBossModels.Values;

	public Subject<MonsterModel> OnBossEncountered { get; } = new Subject<MonsterModel>();

	public Subject<MonsterModel> OnBossDeparted { get; } = new Subject<MonsterModel>();

	public bool IsInCombat => RxIsInCombat.Value;

	private CharacterModel MyCharacterModel => _gameManager.MyCharacterModel;

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<NtfHitCollision>().Subscribe(OnHitCollision).AddTo(this);
		_gameChannel.Receive<NtfHpDamage>().Subscribe(OnHpDamage).AddTo(this);
		_gameChannel.Receive<NtfMonsterTargetChanged>().Subscribe(OnMonsterTargetChanged).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
	}

	private void FixedUpdate()
	{
		UpdateMonsterModels();
		UpdateAggroedSets();
		UpdateCombatState();
	}

	private void OnMonsterTargetChanged(NtfMonsterTargetChanged message)
	{
		if (MyCharacterModel == null)
		{
			return;
		}
		if (_objectManager.FindUnitModel(message.NetworkObjectId) is MonsterModel monsterModel && !monsterModel.IsInactiveInStage && monsterModel.Target is CharacterModel characterModel && characterModel.ClientId != ulong.MaxValue)
		{
			_monsterModels.TryAdd(message.NetworkObjectId, monsterModel);
		}
	}

	private void OnHitCollision(NtfHitCollision message)
	{
		if (_objectManager.FindUnitModel(message.SourceNetworkObjectId) is CharacterModel characterModel && characterModel.ClientId == _gameManager.RxViewTargetClientId.Value && _objectManager.FindUnitModel(message.TargetNetworkObjectId) is MonsterModel monsterModel && !monsterModel.IsInactiveInStage)
		{
			_monsterModels.TryAdd(monsterModel.NetworkObjectId, monsterModel);
		}
	}

	private void OnHpDamage(NtfHpDamage message)
	{
		if (MyCharacterModel == null)
		{
			return;
		}
		if (_gameManager.MyCharacterModel.NetworkObjectId == message.SourceNetworkObjectId && _objectManager.FindUnitModel(message.NetworkObjectId) is MonsterModel monsterModel)
		{
			bool isPenalty = message.ElementalDamagedType.IsPenalty();
			_gameEventSystem.Trigger(new LocalPlayerAttackEvent(monsterModel.MonsterId, isPenalty));
		}
	}

	private void UpdateMonsterModels()
	{
		_monsterRemoveBuffer.Clear();
		foreach (KeyValuePair<ulong, MonsterModel> monsterModel in _monsterModels)
		{
			if (monsterModel.Value == null || monsterModel.Value.IsInactiveInStage)
			{
				_monsterRemoveBuffer.Add(monsterModel.Key);
			}
		}
		for (int i = 0; i < _monsterRemoveBuffer.Count; i++)
		{
			_monsterModels.Remove(_monsterRemoveBuffer[i]);
		}
	}

	private void UpdateAggroedSets()
	{
		if (MyCharacterModel == null)
		{
			return;
		}
		Vector3 position = _gameManager.MyCharacterModel.Position;
		foreach (KeyValuePair<ulong, MonsterModel> monsterModel in _monsterModels)
		{
			ulong key = monsterModel.Key;
			MonsterModel value = monsterModel.Value;
			float sqrMagnitude = (position - value.Position).sqrMagnitude;
			if (value.IsBoss)
			{
				float bossDetectionRange = value.MonsterData.BossDetectionRange;
				UpdateAggroedBoss(key, value, sqrMagnitude < bossDetectionRange * bossDetectionRange);
			}
			else
			{
				UpdateAggroedMonster(key, value, sqrMagnitude < _aggroRange * _aggroRange);
			}
		}
		RemoveStaleAggroedMonsters();
		RemoveStaleAggroedBosses();
	}

	private void UpdateAggroedMonster(ulong key, MonsterModel model, bool inRange)
	{
		if (inRange)
		{
			if (_aggroedMonsterModels.TryAdd(key, model))
			{
				TriggerAggroedEvent(model);
			}
		}
		else
		{
			_aggroedMonsterModels.Remove(key);
		}
	}

	private void UpdateAggroedBoss(ulong key, MonsterModel model, bool inRange)
	{
		if (inRange)
		{
			if (_aggroedBossModels.ContainsKey(key))
			{
				return;
			}
			ITarget target = model.Target;
			if (target == null || (!(target is CharacterModel) && !(target is ExtraUnitModel)))
			{
				return;
			}
			_aggroedBossModels.Add(key, model);
			TriggerAggroedEvent(model);
			OnBossEncountered.OnNext(model);
		}
		else if (_aggroedBossModels.Remove(key))
		{
			OnBossDeparted.OnNext(model);
		}
	}

	private void TriggerAggroedEvent(MonsterModel monster)
	{
		ulong targetClientId = ((monster.Target is CharacterModel characterModel) ? characterModel.ClientId : ulong.MaxValue);
		_gameEventSystem.Trigger(new MonsterAggroedEvent(targetClientId, monster.MonsterId));
	}

	private void RemoveStaleAggroedMonsters()
	{
		_aggroedRemoveBuffer.Clear();
		foreach (ulong key in _aggroedMonsterModels.Keys)
		{
			if (!_monsterModels.ContainsKey(key))
			{
				_aggroedRemoveBuffer.Add(key);
			}
		}
		for (int i = 0; i < _aggroedRemoveBuffer.Count; i++)
		{
			_aggroedMonsterModels.Remove(_aggroedRemoveBuffer[i]);
		}
	}

	private void RemoveStaleAggroedBosses()
	{
		_bossRemoveBuffer.Clear();
		foreach (ulong key in _aggroedBossModels.Keys)
		{
			if (!_monsterModels.ContainsKey(key))
			{
				_bossRemoveBuffer.Add(key);
			}
		}
		for (int i = 0; i < _bossRemoveBuffer.Count; i++)
		{
			if (_aggroedBossModels.Remove(_bossRemoveBuffer[i], out var value) && (object)value != null)
			{
				OnBossDeparted.OnNext(value);
			}
		}
	}

	private void UpdateCombatState()
	{
		if (_aggroedMonsterModels.Count > 0 || _aggroedBossModels.Count > 0)
		{
			_combatEmptyTime = 0f;
			if (!RxIsInCombat.Value)
			{
				RxIsInCombat.Value = true;
			}
		}
		else if (RxIsInCombat.Value)
		{
			_combatEmptyTime += Time.fixedDeltaTime;
			if (!(_combatEmptyTime < _combatCooldown))
			{
				RxIsInCombat.Value = false;
				_combatEmptyTime = 0f;
			}
		}
	}

	private void OnStageClosed()
	{
		foreach (MonsterModel value in _aggroedBossModels.Values)
		{
			if (value != null)
			{
				OnBossDeparted.OnNext(value);
			}
		}
		_monsterModels.Clear();
		_aggroedMonsterModels.Clear();
		_aggroedBossModels.Clear();
		RxIsInCombat.Value = false;
		_combatEmptyTime = 0f;
	}
}

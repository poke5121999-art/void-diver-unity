using System;
using System.Collections.Generic;
using System.Linq;
using SomeNet.Collections;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Unity.Netcode;
using UnityEngine;

public class MonsterModel : UnitModel
{
	private bool _isInitialized;

	private float _globalSkillCoolTime;

	public readonly NetworkVariable<int> SyncHitLayer = new NetworkVariable<int>();

	public NetworkVariable<int> SyncMonsterSpawnId = new NetworkVariable<int>();

	public NetworkVariable<bool> SyncIsLifeTimeFinished = new NetworkVariable<bool>();

	private ESpawnType _spawnType;

	public bool IsPendingFleeState;

	public bool IsPendingAggroState;

	public int AggroChainDepth;

	public float CombatExitTime;

	public bool IsWanderSpecificPosition;

	public Vector3 WanderSpecificPosition;

	public Vector3[] PatrolWanderPositions;

	public bool UseFixedAimPos;

	public Vector3 FixedAimPos;

	public float LastWeakPointDamageTime;

	public float LastWeakPointFullStackDamageTime;

	private ITarget _target;

	private NetworkVariable<float> _syncWeakPointStackDamage = new NetworkVariable<float>();

	private NetworkVariable<EElementalType> _syncWeakPointType = new NetworkVariable<EElementalType>();

	private NetworkVariable<ulong> _syncTargetNetworkObjectId = new NetworkVariable<ulong>();

	private NetworkVariable<float> _syncSuppressionGauge = new NetworkVariable<float>();

	private NetworkVariable<float> _syncSuppressionImmunityEndTime = new NetworkVariable<float>();

	private NetworkVariable<int> _syncGroggyCount = new NetworkVariable<int>();

	private NetworkVariable<bool> _syncIsInCombat = new NetworkVariable<bool>();

	private NetworkVariable<int> _syncMonsterId = new NetworkVariable<int>();

	private readonly ReactiveProperty<float> _rxWeakPointStackDamage = new ReactiveProperty<float>();

	private readonly ReactiveProperty<EElementalType> _rxWeakPointType = new ReactiveProperty<EElementalType>();

	private readonly ReactiveProperty<ulong> _rxTargetNetworkObjectId = new ReactiveProperty<ulong>();

	private readonly ReactiveProperty<float> _rxSuppressionGauge = new ReactiveProperty<float>();

	private readonly ReactiveProperty<float> _rxSuppressionImmunityEndTime = new ReactiveProperty<float>();

	private readonly ReactiveProperty<int> _rxGroggyCount = new ReactiveProperty<int>();

	private readonly ReactiveProperty<bool> _rxIsInCombat = new ReactiveProperty<bool>();

	private readonly ReactiveProperty<int> _rxMonsterId = new ReactiveProperty<int>();

	public override int HitLayer => SyncHitLayer.Value;

	public override int TargetHitLayer
	{
		get
		{
			if (Target is IHitable hitable)
			{
				return hitable.HitLayer;
			}
			return 0;
		}
	}

	public override EUnitType UnitType => EUnitType.Monster;

	public override EElementalType ElementalType => EElementalType.None;

	public override EMonsterCategory[] MonsterCategories => TMonster.Table[TableId].MonsterCategories;

	public override EStressState StressState => EStressState.Alert;

	public override int WeaponId => 0;

	public EMonsterType MonsterType => TMonster.Table[TableId].MonsterType;

	public override int TableId => MonsterId;

	public override bool IsInitialized => _isInitialized;

	public override float GlobalSkillCoolTime => _globalSkillCoolTime;

	public TMonster MonsterData => TMonster.Table[TableId];

	public int FactionId
	{
		get
		{
			if (!TMonster.Table.TryGetValue(_syncMonsterId.Value, out var value))
			{
				return -1;
			}
			return value.FactionId;
		}
	}

	public int MonsterSpawnId => SyncMonsterSpawnId.Value;

	private int VariantDifficulty => GameContext.VariantDifficulty;

	public int BasePhaseMonsterId => MonsterData.BasePhaseMonsterId;

	public int Phase => MonsterData.Phase;

	public bool IsPhaseMonster => MonsterData.BasePhaseMonsterId > 0;

	public int NextPhaseMonsterId
	{
		get
		{
			if (MonsterData.BasePhaseMonsterId < 1)
			{
				return 0;
			}
			return TMonster.Table.Values.Where((TMonster _) => _.BasePhaseMonsterId == MonsterData.BasePhaseMonsterId && MonsterData.Phase < _.Phase).Select((TMonster _) => _.Id).DefaultIfEmpty(0)
				.Min();
		}
	}

	public bool IsLifeTimeFinished => SyncIsLifeTimeFinished.Value;

	public override float WeaponAtkMultiplier => 1f;

	public bool IsSuppressionImmune => GameManager.TimeNow < _rxSuppressionImmunityEndTime.Value;

	public float MaxWeakPointStackDamage => GetStat(EStatType.HpMax) * MonsterData.WeakpointGaugeHpPercent / 100f;

	public float MaxSuppressionGauge => TMonster.Table[TableId].SuppressionGauge;

	public bool HasSuppressionSystem
	{
		get
		{
			if (TMonster.Table[_syncMonsterId.Value].MonsterType.IsBoss())
			{
				return MaxSuppressionGauge > 0f;
			}
			return false;
		}
	}

	public float EffectiveMaxSuppressionGauge => MaxSuppressionGauge * GetSuppressionDifficultyFactor() * Mathf.Pow(1f + TConst.SuppressionResistanceGrowthFactor.FloatValue, GroggyCount);

	public bool HasTarget => _rxTargetNetworkObjectId.Value != 0;

	public ITarget Target
	{
		get
		{
			ulong num = ((_target != null) ? _target.NetworkObjectId : 0);
			if (num != _rxTargetNetworkObjectId.Value)
			{
				ITarget target = null;
				if (_rxTargetNetworkObjectId.Value != 0)
				{
					target = ObjectManager.FindTarget(_rxTargetNetworkObjectId.Value);
				}
				_target = target;
			}
			return _target;
		}
	}

	public ulong OriginalTargetNetworkObjectId { get; set; }

	public ReactiveProperty<UnitModel> RxSummoner { get; } = new ReactiveProperty<UnitModel>();

	public UnitModel Summoner
	{
		get
		{
			return RxSummoner.Value;
		}
		set
		{
			RxSummoner.Value = value;
		}
	}

	public bool IsShadow => TMonster.Table[_syncMonsterId.Value].MonsterType == EMonsterType.Shadow;

	public bool IsBoss => TMonster.Table[_syncMonsterId.Value].MonsterType.IsBoss();

	public IReadOnlyReactiveProperty<float> RxWeakPointStackDamage => _rxWeakPointStackDamage;

	public IReadOnlyReactiveProperty<EElementalType> RxWeakPointType => _rxWeakPointType;

	public ReactiveProperty<ulong> RxTargetNetworkObjectId => _rxTargetNetworkObjectId;

	public IReadOnlyReactiveProperty<float> RxSuppressionGauge => _rxSuppressionGauge;

	public IReadOnlyReactiveProperty<float> RxSuppressionImmunityEndTime => _rxSuppressionImmunityEndTime;

	public IReadOnlyReactiveProperty<int> RxGroggyCount => _rxGroggyCount;

	public IReadOnlyReactiveProperty<bool> RxIsInCombat => _rxIsInCombat;

	public IReadOnlyReactiveProperty<int> RxMonsterId => _rxMonsterId;

	public int MonsterId
	{
		get
		{
			return _syncMonsterId.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncMonsterId, value);
			_rxMonsterId.Value = value;
		}
	}

	public float WeakPointStackDamage
	{
		get
		{
			return _rxWeakPointStackDamage.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncWeakPointStackDamage, value);
		}
	}

	public EElementalType WeakPointElementalType
	{
		get
		{
			return _rxWeakPointType.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncWeakPointType, value);
		}
	}

	public ulong TargetNetworkObjectId
	{
		get
		{
			return _rxTargetNetworkObjectId.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncTargetNetworkObjectId, value);
		}
	}

	public float SuppressionGaugeValue
	{
		get
		{
			return _rxSuppressionGauge.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncSuppressionGauge, value);
		}
	}

	public float SuppressionImmunityEndTime
	{
		get
		{
			return _rxSuppressionImmunityEndTime.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncSuppressionImmunityEndTime, value);
		}
	}

	public int GroggyCount
	{
		get
		{
			return _rxGroggyCount.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncGroggyCount, value);
		}
	}

	public bool IsInCombat
	{
		get
		{
			return _rxIsInCombat.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncIsInCombat, value);
		}
	}

	public void ResetSuppressionGauge()
	{
		SetNetworkVariable(ref _syncSuppressionGauge, EffectiveMaxSuppressionGauge);
	}

	private float GetSuppressionDifficultyFactor()
	{
		return GameContext.Difficulty switch
		{
			EDifficulty.Easy => TConst.SuppressionGaugeFactorEasy.FloatValue,
			EDifficulty.Normal => TConst.SuppressionGaugeFactorNormal.FloatValue,
			EDifficulty.Hard => TConst.SuppressionGaugeFactorHard.FloatValue,
			EDifficulty.Insane => TConst.SuppressionGaugeFactorInsane.FloatValue,
			_ => 1f,
		};
	}

	public bool IsSuppressionBuffTag(EStatusEffectTag tag)
	{
		if (!TMonster.Table[_syncMonsterId.Value].MonsterType.IsBoss() || !(MaxSuppressionGauge > 0f))
		{
			return false;
		}
		return TStatusEffectSuppression.Table.ContainsKey(tag);
	}

	public override void SetCalculatedDeltaAimPos(Vector3 value)
	{
		if (!UseFixedAimPos)
		{
			CalculatedDeltaAimPos = value;
		}
	}

	public void Init(Vector3 forward, ESpawnType spawnType)
	{
		TMonster tMonster = TMonster.Table[_syncMonsterId.Value];
		_spawnType = spawnType;
		Init(tMonster.GetBaseStats(GameContext.Difficulty, GameContext.VariantDifficulty), tMonster.ActiveSkillIds, tMonster.PassiveSkillIds, forward);
		if (base.NetworkManager.IsHost)
		{
			SyncIsLifeTimeFinished.Value = false;
			CalculatedDeltaAimPos = Vector3.right;
			LastWeakPointFullStackDamageTime = float.NegativeInfinity;
			InitNetDataVariable();
			SetNetworkVariable(ref _syncSuppressionGauge, EffectiveMaxSuppressionGauge);
			OnStatChanged.Where(((EStatType statType, UnitStat oldValue, UnitStat newValue) _) => _.statType == EStatType.HpMax).Subscribe(delegate((EStatType statType, UnitStat oldValue, UnitStat newValue) _)
			{
				int num = (int)(_.oldValue?.Value ?? 0f);
				int num2 = (int)(_.newValue?.Value ?? 0f);
				if (num < num2)
				{
					Hp = Math.Min(Hp + (num2 - num), num2);
				}
				else
				{
					Hp = Math.Min(Hp, num2);
				}
			}).AddTo(this);
		}
		_globalSkillCoolTime = TMonster.Table[TableId].GlobalSkillCoolTime;
		_globalSkillCoolTime = TDifficulty.Table[GameContext.Difficulty].GlobalSkillCoolTimePercent / 100f * _globalSkillCoolTime;
		float num3 = ((GameContext.VariantDifficulty < 1) ? 100f : TVariantDifficulty.Table[GameContext.VariantDifficulty].GlobalSkillCoolTimePercent);
		_globalSkillCoolTime = num3 / 100f * _globalSkillCoolTime;
		_isInitialized = true;
	}

	public void SetNextPhase(int phaseMonsterId)
	{
		AssertIfNotHost();
		SetNetworkVariable(ref _syncMonsterId, phaseMonsterId);
		_rxMonsterId.Value = phaseMonsterId;
		SetNetworkVariable(ref _syncSuppressionImmunityEndTime, 0f);
		SetNetworkVariable(ref _syncGroggyCount, 0);
		SetNetworkVariable(ref _syncSuppressionGauge, EffectiveMaxSuppressionGauge);
		TMonster tMonster = TMonster.Table[phaseMonsterId];
		BaseStats = tMonster.GetBaseStats(GameContext.Difficulty, GameContext.VariantDifficulty);
		ClearBuffs();
		ApplyActiveSkills(tMonster.ActiveSkillIds);
		ApplyPassiveSkills(tMonster.PassiveSkillIds);
		ClearConsumableTypeCoolTimes();
		BaseStats.ForEach(delegate(KeyValuePair<EStatType, float> _)
		{
			SetStat(_.Key, _.Value);
		});
		Hp = (int)GetStat(EStatType.HpMax);
	}

	public void SetTarget(ITarget target)
	{
		AssertIfNotHost();
		_target = target;
		SetNetworkVariable(ref _syncTargetNetworkObjectId, target?.NetworkObjectId ?? 0);
	}

	private void OnMaxHpChanged(UnitStat oldValue, UnitStat newValue)
	{
		int num = (int)(oldValue?.Value ?? 0f);
		int num2 = (int)(newValue?.Value ?? 0f);
		if (num < num2)
		{
			Hp = Math.Min(Hp + (num2 - num), num2);
		}
		else
		{
			Hp = Math.Min(Hp, num2);
		}
	}

	protected override void RegisterVariableEvents()
	{
		base.RegisterVariableEvents();
		RegisterVariableEvent(_syncWeakPointStackDamage, _rxWeakPointStackDamage);
		RegisterVariableEvent(_syncWeakPointType, _rxWeakPointType);
		RegisterVariableEvent(_syncTargetNetworkObjectId, _rxTargetNetworkObjectId);
		RegisterVariableEvent(_syncSuppressionGauge, _rxSuppressionGauge);
		RegisterVariableEvent(_syncSuppressionImmunityEndTime, _rxSuppressionImmunityEndTime);
		RegisterVariableEvent(_syncGroggyCount, _rxGroggyCount);
		RegisterVariableEvent(_syncIsInCombat, _rxIsInCombat);
		RegisterVariableEvent(_syncMonsterId, _rxMonsterId);
	}

	protected override void UnregisterVariableEvents()
	{
		base.UnregisterVariableEvents();
		UnregisterVariableEvent(_syncWeakPointStackDamage);
		UnregisterVariableEvent(_syncWeakPointType);
		UnregisterVariableEvent(_syncTargetNetworkObjectId);
		UnregisterVariableEvent(_syncSuppressionGauge);
		UnregisterVariableEvent(_syncSuppressionImmunityEndTime);
		UnregisterVariableEvent(_syncGroggyCount);
		UnregisterVariableEvent(_syncIsInCombat);
		UnregisterVariableEvent(_syncMonsterId);
	}

	private void InitNetDataVariable()
	{
		_rxWeakPointStackDamage.Value = 0f;
		_syncWeakPointStackDamage.Value = 0f;
		_rxWeakPointType.Value = EElementalType.None;
		_syncWeakPointType.Value = EElementalType.None;
		_syncTargetNetworkObjectId.Value = 0uL;
		_rxSuppressionGauge.Value = 0f;
		_syncSuppressionGauge.Value = 0f;
		_rxSuppressionImmunityEndTime.Value = 0f;
		_syncSuppressionImmunityEndTime.Value = 0f;
		_rxGroggyCount.Value = 0;
		_syncGroggyCount.Value = 0;
		_rxIsInCombat.Value = false;
		_syncIsInCombat.Value = false;
	}
}

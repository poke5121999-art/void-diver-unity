using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Unity.Netcode;
using UnityEngine;

public class MonsterModel : UnitModel
{
	private bool _isInitialized;

	private float _globalSkillCoolTime;

	public readonly NetworkVariable<int> SyncHitLayer;

	public NetworkVariable<int> SyncMonsterSpawnId;

	public NetworkVariable<bool> SyncIsLifeTimeFinished;

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

	private NetworkVariable<float> _syncWeakPointStackDamage;

	private NetworkVariable<EElementalType> _syncWeakPointType;

	private NetworkVariable<ulong> _syncTargetNetworkObjectId;

	private NetworkVariable<float> _syncSuppressionGauge;

	private NetworkVariable<float> _syncSuppressionImmunityEndTime;

	private NetworkVariable<int> _syncGroggyCount;

	private NetworkVariable<bool> _syncIsInCombat;

	private NetworkVariable<int> _syncMonsterId;

	private readonly ReactiveProperty<float> _rxWeakPointStackDamage;

	private readonly ReactiveProperty<EElementalType> _rxWeakPointType;

	private readonly ReactiveProperty<ulong> _rxTargetNetworkObjectId;

	private readonly ReactiveProperty<float> _rxSuppressionGauge;

	private readonly ReactiveProperty<float> _rxSuppressionImmunityEndTime;

	private readonly ReactiveProperty<int> _rxGroggyCount;

	private readonly ReactiveProperty<bool> _rxIsInCombat;

	private readonly ReactiveProperty<int> _rxMonsterId;

	public override int HitLayer => 0;

	public override int TargetHitLayer => 0;

	public override EUnitType UnitType => EUnitType.None;

	public override EElementalType ElementalType => EElementalType.None;

	public override EMonsterCategory[] MonsterCategories => null;

	public override EStressState StressState => EStressState.Alert;

	public override int WeaponId => 0;

	public EMonsterType MonsterType => EMonsterType.None;

	public override int TableId => 0;

	public override bool IsInitialized => false;

	public override float GlobalSkillCoolTime => 0f;

	public TMonster MonsterData => null;

	public int FactionId => 0;

	public int MonsterSpawnId => 0;

	private int VariantDifficulty => 0;

	public int BasePhaseMonsterId => 0;

	public int Phase => 0;

	public bool IsPhaseMonster => false;

	public int NextPhaseMonsterId => 0;

	public bool IsLifeTimeFinished => false;

	public override float WeaponAtkMultiplier => 0f;

	public bool IsSuppressionImmune => false;

	public float MaxWeakPointStackDamage => 0f;

	public float MaxSuppressionGauge => 0f;

	public bool HasSuppressionSystem => false;

	public float EffectiveMaxSuppressionGauge => 0f;

	public bool HasTarget => false;

	public ITarget Target => null;

	public ulong OriginalTargetNetworkObjectId { get; set; }

	public ReactiveProperty<UnitModel> RxSummoner { get; }

	public UnitModel Summoner
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsShadow => false;

	public bool IsBoss => false;

	public IReadOnlyReactiveProperty<float> RxWeakPointStackDamage => null;

	public IReadOnlyReactiveProperty<EElementalType> RxWeakPointType => null;

	public ReactiveProperty<ulong> RxTargetNetworkObjectId => null;

	public IReadOnlyReactiveProperty<float> RxSuppressionGauge => null;

	public IReadOnlyReactiveProperty<float> RxSuppressionImmunityEndTime => null;

	public IReadOnlyReactiveProperty<int> RxGroggyCount => null;

	public IReadOnlyReactiveProperty<bool> RxIsInCombat => null;

	public IReadOnlyReactiveProperty<int> RxMonsterId => null;

	public int MonsterId
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public float WeakPointStackDamage
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public EElementalType WeakPointElementalType
	{
		get
		{
			return EElementalType.None;
		}
		set
		{
		}
	}

	public ulong TargetNetworkObjectId
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public float SuppressionGaugeValue
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SuppressionImmunityEndTime
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public int GroggyCount
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool IsInCombat
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public void ResetSuppressionGauge()
	{
	}

	private float GetSuppressionDifficultyFactor()
	{
		return 0f;
	}

	public bool IsSuppressionBuffTag(EStatusEffectTag tag)
	{
		return false;
	}

	public override void SetCalculatedDeltaAimPos(Vector3 value)
	{
	}

	public void Init(Vector3 forward, ESpawnType spawnType)
	{
	}

	public void SetNextPhase(int phaseMonsterId)
	{
	}

	public void SetTarget(ITarget target)
	{
	}

	private void OnMaxHpChanged(UnitStat oldValue, UnitStat newValue)
	{
	}

	protected override void RegisterVariableEvents()
	{
	}

	protected override void UnregisterVariableEvents()
	{
	}

	private void InitNetDataVariable()
	{
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}

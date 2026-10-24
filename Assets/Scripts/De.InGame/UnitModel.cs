using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using SomeNet.Collections;
using Tracker;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public abstract class UnitModel : NetworkBase, IHitable, ITarget, IActionStats
{
	[SerializeField]
	public GameObject BodyObject;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected SeriLogger Logger;

	private readonly Dictionary<object, Delegate> _listEventHandlers = new Dictionary<object, Delegate>();

	private readonly Dictionary<(int buffId, ulong casterId), IDisposable> _realtimeStatSubs = new Dictionary<(int buffId, ulong casterId), IDisposable>();

	private readonly HashSet<EStatType> _dirtyRealtimeStats = new HashSet<EStatType>();

	private bool _isConverging;

	protected bool IsShieldDirty;

	private NetworkVariable<EActionState> syncState = new NetworkVariable<EActionState>();

	private NetworkVariable<PackedDirMag> syncMoveDir = new NetworkVariable<PackedDirMag>();

	private NetworkVariable<QuantAimDelta> syncCalculatedDeltaAimPos = new NetworkVariable<QuantAimDelta>();

	private NetworkVariable<QuantAimDelta> syncDeltaAimPos = new NetworkVariable<QuantAimDelta>();

	private NetworkVariable<int> syncHp = new NetworkVariable<int>();

	private NetworkVariable<PackedDir> syncForward = new NetworkVariable<PackedDir>();

	private NetworkVariable<float> syncStress = new NetworkVariable<float>();

	private NetworkVariable<int> syncCorruption = new NetworkVariable<int>();

	private NetworkVariable<float> syncStamina = new NetworkVariable<float>();

	private NetworkVariable<int> syncSoul = new NetworkVariable<int>();

	private NetworkVariable<int> syncUsingSkillId = new NetworkVariable<int>();

	private NetworkVariable<PlanarPos> syncPosition = new NetworkVariable<PlanarPos>();

	private NetworkVariable<int> syncShield = new NetworkVariable<int>();

	private NetworkVariable<QuantSpeed> syncInterpolationSpeed = new NetworkVariable<QuantSpeed>();

	private ReactiveProperty<EActionState> rxState = new ReactiveProperty<EActionState>();

	private ReactiveProperty<Vector3> rxMoveDir = new ReactiveProperty<Vector3>();

	private ReactiveProperty<Vector3> rxCalculatedDeltaAimPos = new ReactiveProperty<Vector3>();

	private ReactiveProperty<Vector3> rxDeltaAimPos = new ReactiveProperty<Vector3>();

	private ReactiveProperty<Vector3> rxForward = new ReactiveProperty<Vector3>();

	private ReactiveProperty<int> rxHp = new ReactiveProperty<int>();

	private ReactiveProperty<float> rxStress = new ReactiveProperty<float>();

	private ReactiveProperty<int> rxCorruption = new ReactiveProperty<int>();

	private ReactiveProperty<float> rxStamina = new ReactiveProperty<float>();

	private ReactiveProperty<int> rxSoul = new ReactiveProperty<int>();

	private ReactiveProperty<int> rxUsingSkillId = new ReactiveProperty<int>();

	private ReactiveProperty<Vector3> rxPosition = new ReactiveProperty<Vector3>();

	private ReactiveProperty<int> rxShield = new ReactiveProperty<int>();

	private ReactiveProperty<float> rxInterpolationSpeed = new ReactiveProperty<float>();

	private readonly Dictionary<int, int> _changeSkillOverrides = new Dictionary<int, int>();

	private readonly Dictionary<ulong, SpecialField> _enteredSpecialFields = new Dictionary<ulong, SpecialField>();

	private readonly Dictionary<ulong, float> _stackBuffNextFireTimes = new Dictionary<ulong, float>();

	private readonly Dictionary<int, int> _expirePreventedStackBuffRefCounts = new Dictionary<int, int>();

	protected TrackableDictionary<int, UnitSkill> trackableSkills = new TrackableDictionary<int, UnitSkill>();

	protected TrackableDictionary<int, UnitBuff> trackableBuffs = new TrackableDictionary<int, UnitBuff>();

	private TrackableDictionary<EConsumableType, UnitItemCoolTime> trackableConsumableTypeCoolTimes = new TrackableDictionary<EConsumableType, UnitItemCoolTime>();

	private TrackableDictionary<EStatType, UnitStat> trackableStats = new TrackableDictionary<EStatType, UnitStat>();

	private TrackableDictionary<ECrowdControlType, UnitCc> trackableCrowdControls = new TrackableDictionary<ECrowdControlType, UnitCc>();

	private int _syncTick;

	protected GameHost GameHost => NetworkHost as GameHost;

	protected GameChannel GameChannel => NetworkChannel as GameChannel;

	public bool IsInTrainingRoom => HasBuffEffect(EBuffEffectType.TrainingRoom);

	public bool IsNonCombat
	{
		get
		{
			if (GameManager.IsInLounge)
			{
				return !HasBuffEffect(EBuffEffectType.TrainingRoom);
			}
			return false;
		}
	}

	public abstract EUnitType UnitType { get; }

	public abstract EElementalType ElementalType { get; }

	public abstract EMonsterCategory[] MonsterCategories { get; }

	public abstract EStressState StressState { get; }

	public abstract int WeaponId { get; }

	public abstract int TableId { get; }

	public abstract bool IsInitialized { get; }

	public abstract float GlobalSkillCoolTime { get; }

	public float HitRadius { get; private set; }

	public ICollection<int> TrackableSkillIds => trackableSkills.Keys;

	public Transform Transform => base.transform;

	public EActionState SyncState => syncState.Value;

	public new bool IsHost => base.NetworkManager.IsHost;

	public abstract int HitLayer { get; }

	public abstract int TargetHitLayer { get; }

	public bool IsHitable
	{
		get
		{
			if (rxState.Value == EActionState.Dead || rxState.Value == EActionState.Revive || rxState.Value == EActionState.PhaseUp || rxState.Value == EActionState.Cutscene || rxState.Value == EActionState.ReservedDespawn || rxState.Value == EActionState.Escaped || rxState.Value == EActionState.Captured || rxState.Value == EActionState.StageReady)
			{
				return false;
			}
			return rxState.Value != EActionState.Spawn;
		}
	}

	public bool IsInactiveInStage => rxState.Value == EActionState.Dead || rxState.Value == EActionState.ReservedDespawn || rxState.Value == EActionState.Escaped || rxState.Value == EActionState.Captured;

	public bool IsActivateInStage => !IsInactiveInStage;

	public List<int> ActiveSkillIds { get; private set; }

	public List<int> PassiveSkillIds { get; private set; }

	public Subject<int[]> OnActiveSkillsChanged { get; } = new Subject<int[]>();

	public Subject<int[]> OnPassiveSkillsChanged { get; } = new Subject<int[]>();

	public Dictionary<int, int> SkillLevels { get; protected set; }

	public Dictionary<(ulong casterId, int shieldId), UnitShield> Shields { get; } = new Dictionary<(ulong casterId, int shieldId), UnitShield>();

	public IReadOnlyReactiveProperty<EActionState> RxState => rxState;

	public IReadOnlyReactiveProperty<Vector3> RxMoveDir => rxMoveDir;

	public IReadOnlyReactiveProperty<Vector3> RxCalculatedDeltaAimPos => rxCalculatedDeltaAimPos;

	public IReadOnlyReactiveProperty<Vector3> RxDeltaAimPos => rxDeltaAimPos;

	public IReadOnlyReactiveProperty<Vector3> RxForward => rxForward;

	public IReadOnlyReactiveProperty<int> RxHp => rxHp;

	public IReadOnlyReactiveProperty<float> RxStress => rxStress;

	public IReadOnlyReactiveProperty<int> RxCorruption => rxCorruption;

	public IReadOnlyReactiveProperty<float> RxStamina => rxStamina;

	public IReadOnlyReactiveProperty<int> RxSoul => rxSoul;

	public IReadOnlyReactiveProperty<int> RxUsingSkillId => rxUsingSkillId;

	public IReadOnlyReactiveProperty<Vector3> RxPosition => rxPosition;

	public IReadOnlyReactiveProperty<int> RxShield => rxShield;

	public IReadOnlyReactiveProperty<float> RxInterpolationSpeed => rxInterpolationSpeed;

	public EActionState State
	{
		get
		{
			return rxState.Value;
		}
		set
		{
			SetNetworkVariable(ref syncState, value);
		}
	}

	public Vector3 MoveDir
	{
		get
		{
			return rxMoveDir.Value;
		}
		set
		{
			SetNetworkVariable(ref syncMoveDir, new PackedDirMag(value));
		}
	}

	public Vector3 CalculatedDeltaAimPos
	{
		get
		{
			return rxCalculatedDeltaAimPos.Value;
		}
		set
		{
			SetNetworkVariable(ref syncCalculatedDeltaAimPos, new QuantAimDelta(value));
		}
	}

	public Vector3 DeltaAimPos
	{
		get
		{
			return rxDeltaAimPos.Value;
		}
		set
		{
			SetNetworkVariable(ref syncDeltaAimPos, new QuantAimDelta(value));
		}
	}

	public Vector3 Forward
	{
		get
		{
			return rxForward.Value;
		}
		set
		{
			SetNetworkVariable(ref syncForward, new PackedDir(value));
		}
	}

	public int Hp
	{
		get
		{
			return rxHp.Value;
		}
		set
		{
			SetNetworkVariable(ref syncHp, value);
		}
	}

	public float Stress
	{
		get
		{
			return rxStress.Value;
		}
		set
		{
			SetNetworkVariable(ref syncStress, value);
		}
	}

	public int Corruption
	{
		get
		{
			return rxCorruption.Value;
		}
		set
		{
			SetNetworkVariable(ref syncCorruption, value);
		}
	}

	public float Stamina
	{
		get
		{
			return rxStamina.Value;
		}
		set
		{
			SetNetworkVariable(ref syncStamina, value);
		}
	}

	public int Soul
	{
		get
		{
			return rxSoul.Value;
		}
		set
		{
			SetNetworkVariable(ref syncSoul, value);
		}
	}

	public int UsingSkillId
	{
		get
		{
			return rxUsingSkillId.Value;
		}
		set
		{
			SetNetworkVariable(ref syncUsingSkillId, value);
		}
	}

	public Vector3 Position
	{
		get
		{
			return rxPosition.Value;
		}
		set
		{
			SetNetworkVariable(ref syncPosition, new PlanarPos(value));
		}
	}

	public int Shield
	{
		get
		{
			return rxShield.Value;
		}
		set
		{
			SetNetworkVariable(ref syncShield, value);
		}
	}

	public float InterpolationSpeed
	{
		get
		{
			return rxInterpolationSpeed.Value;
		}
		set
		{
			SetNetworkVariable(ref syncInterpolationSpeed, new QuantSpeed(value));
		}
	}

	public bool IsBasicAttackHolding { get; set; }

	public abstract float WeaponAtkMultiplier { get; }

	public IDictionary<EStatType, float> BaseStats { get; protected set; }

	public float BaseAtk => GetBaseStat(EStatType.Atk);

	public float BaseDef => GetBaseStat(EStatType.Def);

	public float BaseMoveSpeed => GetBaseStat(EStatType.MoveSpeed);

	public float BaseAtkSpeed => GetBaseStat(EStatType.AtkSpeed);

	public float BaseHpMax => GetBaseStat(EStatType.HpMax);

	public float BaseHpRegen => GetBaseStat(EStatType.HpRegen);

	public float BaseStaminaMax => GetBaseStat(EStatType.StaminaMax);

	public float BaseStaminaRegen => GetBaseStat(EStatType.StaminaRegen);

	public float BaseRunStaminaCost => GetBaseStat(EStatType.RunStaminaCost);

	public float BaseRunIncreaseSpeed => GetBaseStat(EStatType.RunIncreaseSpeed);

	public float BaseCorruptionMax => GetBaseStat(EStatType.CorruptionMax);

	public float BaseAtkPercent => GetBaseStat(EStatType.AtkPercent);

	public float BaseDefPercent => GetBaseStat(EStatType.DefPercent);

	public float BaseMoveSpeedPercent => GetBaseStat(EStatType.MoveSpeedPercent);

	public float BaseAtkSpeedPercent => GetBaseStat(EStatType.AtkSpeedPercent);

	public float BaseHpMaxPercent => GetBaseStat(EStatType.HpMaxPercent);

	public float BaseHpRegenPercent => GetBaseStat(EStatType.HpRegenPercent);

	public float BaseStressIntervalPercent => GetBaseStat(EStatType.StressIntervalPercent);

	public float BasePenetration => GetBaseStat(EStatType.Penetration);

	public float BasePenetrationPercent => GetBaseStat(EStatType.PenetrationPercent);

	public float BaseCriticalChancePercent => GetBaseStat(EStatType.CriticalChancePercent);

	public float BaseCriticalTakenChancePercent => GetBaseStat(EStatType.CriticalTakenChancePercent);

	public float BaseCriticalDamagePercent => GetBaseStat(EStatType.CriticalDamagePercent);

	public float BaseWeaknessDamagePercent => GetBaseStat(EStatType.WeaknessDamagePercent);

	public float BaseCooldownReductionPercent => GetBaseStat(EStatType.CooldownReductionPercent);

	public float BaseAdditionalAttackDamagePercent => GetBaseStat(EStatType.AdditionalAttackDamagePercent);

	public float BaseReductionDamagePercent => GetBaseStat(EStatType.ReductionDamagePercent);

	public float BaseBackAttackDamagePercent => GetBaseStat(EStatType.BackAttackDamagePercent);

	public float BaseDashDistancePercent => GetBaseStat(EStatType.DashDistancePercent);

	public float BaseFireResistancePercent => GetBaseStat(EStatType.FireResistancePercent);

	public float BaseWaterResistancePercent => GetBaseStat(EStatType.WaterResistancePercent);

	public float BaseWindResistancePercent => GetBaseStat(EStatType.WindResistancePercent);

	public float BaseFireDamagePercent => GetBaseStat(EStatType.FireDamagePercent);

	public float BaseWaterDamagePercent => GetBaseStat(EStatType.WaterDamagePercent);

	public float BaseWindDamagePercent => GetBaseStat(EStatType.WindDamagePercent);

	public float BaseRunStaminaCostPercent => GetBaseStat(EStatType.RunStaminaCostPercent);

	public float BaseRunIncreaseSpeedPercent => GetBaseStat(EStatType.RunIncreaseSpeedPercent);

	public float BaseMax => GetBaseStat(EStatType.Max);

	public float Atk => GetStat(EStatType.Atk);

	public float Def => GetStat(EStatType.Def);

	public float MoveSpeed => GetStat(EStatType.MoveSpeed);

	public float AtkSpeed => GetStat(EStatType.AtkSpeed);

	public float HpMax => GetStat(EStatType.HpMax);

	public float HpRegen => GetStat(EStatType.HpRegen);

	public float StaminaMax => GetStat(EStatType.StaminaMax);

	public float StaminaRegen => GetStat(EStatType.StaminaRegen);

	public float RunStaminaCost => GetStat(EStatType.RunStaminaCost);

	public float RunIncreaseSpeed => GetStat(EStatType.RunIncreaseSpeed);

	public float CorruptionMax => GetStat(EStatType.CorruptionMax);

	public float AtkPercent => GetStat(EStatType.AtkPercent);

	public float DefPercent => GetStat(EStatType.DefPercent);

	public float MoveSpeedPercent => GetStat(EStatType.MoveSpeedPercent);

	public float AtkSpeedPercent => GetStat(EStatType.AtkSpeedPercent);

	public float HpMaxPercent => GetStat(EStatType.HpMaxPercent);

	public float HpRegenPercent => GetStat(EStatType.HpRegenPercent);

	public float StressIntervalPercent => GetStat(EStatType.StressIntervalPercent);

	public float Penetration => GetStat(EStatType.Penetration);

	public float PenetrationPercent => GetStat(EStatType.PenetrationPercent);

	public float CriticalChancePercent => GetStat(EStatType.CriticalChancePercent);

	public float CriticalTakenChancePercent => GetStat(EStatType.CriticalTakenChancePercent);

	public float CriticalDamagePercent => GetStat(EStatType.CriticalDamagePercent);

	public float WeaknessDamagePercent => GetStat(EStatType.WeaknessDamagePercent);

	public float CooldownReductionPercent => GetStat(EStatType.CooldownReductionPercent);

	public float AdditionalAttackDamagePercent => GetStat(EStatType.AdditionalAttackDamagePercent);

	public float ReductionDamagePercent => GetStat(EStatType.ReductionDamagePercent);

	public float BackAttackDamagePercent => GetStat(EStatType.BackAttackDamagePercent);

	public float DashDistancePercent => GetStat(EStatType.DashDistancePercent);

	public float FireResistancePercent => GetStat(EStatType.FireResistancePercent);

	public float WaterResistancePercent => GetStat(EStatType.WaterResistancePercent);

	public float WindResistancePercent => GetStat(EStatType.WindResistancePercent);

	public float FireDamagePercent => GetStat(EStatType.FireDamagePercent);

	public float WaterDamagePercent => GetStat(EStatType.WaterDamagePercent);

	public float WindDamagePercent => GetStat(EStatType.WindDamagePercent);

	public float RunStaminaCostPercent => GetStat(EStatType.RunStaminaCostPercent);

	public float RunIncreaseSpeedPercent => GetStat(EStatType.RunIncreaseSpeedPercent);

	public float Max => GetStat(EStatType.Max);

	public IObservable<EStatType> RxAtk => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.Atk);

	public IObservable<EStatType> RxDef => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.Def);

	public IObservable<EStatType> RxMoveSpeed => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.MoveSpeed);

	public IObservable<EStatType> RxAtkSpeed => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.AtkSpeed);

	public IObservable<EStatType> RxHpMax => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.HpMax);

	public IObservable<EStatType> RxHpRegen => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.HpRegen);

	public IObservable<EStatType> RxStaminaMax => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.StaminaMax);

	public IObservable<EStatType> RxStaminaRegen => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.StaminaRegen);

	public IObservable<EStatType> RxRunStaminaCost => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.RunStaminaCost);

	public IObservable<EStatType> RxRunIncreaseSpeed => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.RunIncreaseSpeed);

	public IObservable<EStatType> RxCorruptionMax => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.CorruptionMax);

	public IObservable<EStatType> RxAtkPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.AtkPercent);

	public IObservable<EStatType> RxDefPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.DefPercent);

	public IObservable<EStatType> RxMoveSpeedPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.MoveSpeedPercent);

	public IObservable<EStatType> RxAtkSpeedPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.AtkSpeedPercent);

	public IObservable<EStatType> RxHpMaxPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.HpMaxPercent);

	public IObservable<EStatType> RxHpRegenPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.HpRegenPercent);

	public IObservable<EStatType> RxStressIntervalPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.StressIntervalPercent);

	public IObservable<EStatType> RxPenetration => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.Penetration);

	public IObservable<EStatType> RxPenetrationPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.PenetrationPercent);

	public IObservable<EStatType> RxCriticalChancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.CriticalChancePercent);

	public IObservable<EStatType> RxCriticalTakenChancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.CriticalTakenChancePercent);

	public IObservable<EStatType> RxCriticalDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.CriticalDamagePercent);

	public IObservable<EStatType> RxWeaknessDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.WeaknessDamagePercent);

	public IObservable<EStatType> RxCooldownReductionPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.CooldownReductionPercent);

	public IObservable<EStatType> RxAdditionalAttackDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.AdditionalAttackDamagePercent);

	public IObservable<EStatType> RxReductionDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.ReductionDamagePercent);

	public IObservable<EStatType> RxBackAttackDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.BackAttackDamagePercent);

	public IObservable<EStatType> RxDashDistancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.DashDistancePercent);

	public IObservable<EStatType> RxFireResistancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.FireResistancePercent);

	public IObservable<EStatType> RxWaterResistancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.WaterResistancePercent);

	public IObservable<EStatType> RxWindResistancePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.WindResistancePercent);

	public IObservable<EStatType> RxFireDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.FireDamagePercent);

	public IObservable<EStatType> RxWaterDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.WaterDamagePercent);

	public IObservable<EStatType> RxWindDamagePercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.WindDamagePercent);

	public IObservable<EStatType> RxRunStaminaCostPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.RunStaminaCostPercent);

	public IObservable<EStatType> RxRunIncreaseSpeedPercent => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.RunIncreaseSpeedPercent);

	public IObservable<EStatType> RxMax => RxStats.ObserveChange().Where((EStatType t) => t == EStatType.Max);

	public bool HasChange => trackableSkills.HasChange || trackableBuffs.HasChange || trackableConsumableTypeCoolTimes.HasChange || trackableStats.HasChange || trackableCrowdControls.HasChange;

	public ReactiveDictionary<int, UnitSkill> RxSkills { get; } = new ReactiveDictionary<int, UnitSkill>();

	public ReactiveDictionary<int, UnitBuff> RxBuffs { get; } = new ReactiveDictionary<int, UnitBuff>();

	public ReactiveDictionary<EConsumableType, UnitItemCoolTime> RxConsumableTypeCoolTimes { get; } = new ReactiveDictionary<EConsumableType, UnitItemCoolTime>();

	public ReactiveDictionary<EStatType, UnitStat> RxStats { get; } = new ReactiveDictionary<EStatType, UnitStat>();

	public ReactiveDictionary<ECrowdControlType, UnitCc> RxCrowdControls { get; } = new ReactiveDictionary<ECrowdControlType, UnitCc>();

	public IDictionary<int, UnitSkill> Skills => base.NetworkManager.IsHost ? trackableSkills : RxSkills;

	public IDictionary<int, UnitBuff> Buffs => base.NetworkManager.IsHost ? trackableBuffs : RxBuffs;

	public IDictionary<EConsumableType, UnitItemCoolTime> ConsumableTypeCoolTimes => base.NetworkManager.IsHost ? trackableConsumableTypeCoolTimes : RxConsumableTypeCoolTimes;

	public IDictionary<EStatType, UnitStat> Stats => base.NetworkManager.IsHost ? trackableStats : RxStats;

	public IDictionary<ECrowdControlType, UnitCc> CrowdControls => base.NetworkManager.IsHost ? trackableCrowdControls : RxCrowdControls;

	public Subject<(EStatType statType, UnitStat oldValue, UnitStat newValue)> OnStatChanged { get; } = new Subject<(EStatType statType, UnitStat oldValue, UnitStat newValue)>();

	ulong ITarget.NetworkObjectId => base.NetworkObjectId;

	public virtual void SetCalculatedDeltaAimPos(Vector3 value)
	{
		SetNetworkVariable(ref syncCalculatedDeltaAimPos, new QuantAimDelta(value));
	}

	protected void Init(IDictionary<EStatType, float> baseStats, IList<int> activeSkillIds, IList<int> passiveSkillIds, Vector3 forward)
	{
		BaseStats = baseStats;
		InitVariables(EActionState.Idle, Vector3.zero, Vector3.left, Vector3.left, forward, 0, 0f, 0, 0f, 0, 0, Vector3.zero, 0, -1f);
		InitLists();
		if (base.NetworkManager.IsHost)
		{
			AddTrackableConsumableTypeCoolTime(EConsumableType.HpRecovery, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.StressRecovery, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.BrightnessRecovery, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.StaminaRecovery, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.Buff, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.ReleaseDebuff, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.AttackSupport, 0f);
			AddTrackableConsumableTypeCoolTime(EConsumableType.AttackSupport2, 0f);
			BaseStats.ForEach(delegate(KeyValuePair<EStatType, float> kv)
			{
				SetStat(kv.Key, kv.Value);
			});
			Shields.Clear();
		}
		ApplyActiveSkills(activeSkillIds);
		ApplyPassiveSkills(passiveSkillIds);
		RegisterVariableEvents();
		RegisterListEvents();
		if (base.NetworkManager.IsHost)
		{
			FlushTrackableDatas(syncToClients: false);
		}
		void AddTrackableConsumableTypeCoolTime(EConsumableType type, float endTime)
		{
			trackableConsumableTypeCoolTimes.Add(type, new UnitItemCoolTime
			{
				EndTime = endTime,
				ConsumableType = type
			});
		}
	}

	public void ApplyActiveSkills(IEnumerable<int> skillIds)
	{
		List<int> list = ActiveSkillIds?.ToList() ?? new List<int>();
		ActiveSkillIds = skillIds?.ToList() ?? new List<int>();
		List<int> activeSkillIds = GetActiveSkillIds();
		if (base.NetworkManager.IsHost)
		{
			list.Where((int id) => id >= 1 && !ActiveSkillIds.Contains(id)).ForEach(delegate(int id)
			{
				SetSkillActive(id, isActive: false);
			});
			ActiveSkillIds.Where((int id) => 0 < id).ForEach(delegate(int id)
			{
				if (trackableSkills.ContainsKey(id))
				{
					SetSkillActive(id, isActive: true);
				}
				else
				{
					trackableSkills.Add(id, new UnitSkill(id));
				}
			});
		}
		OnActiveSkillsChanged.OnNext(activeSkillIds.ToArray());
	}

	private void SetSkillActive(int skillId, bool isActive)
	{
		if (trackableSkills.TryGetValue(skillId, out var value) && value.IsActiveNow != isActive)
		{
			UnitSkill unitSkill = value.Clone();
			unitSkill.IsActiveNow = isActive;
			trackableSkills[skillId] = unitSkill;
		}
	}

	public void ApplyPassiveSkills(IEnumerable<int> skillIds)
	{
		List<int> list = PassiveSkillIds?.ToList() ?? new List<int>();
		PassiveSkillIds = skillIds?.ToList() ?? new List<int>();
		if (base.NetworkManager.IsHost)
		{
			list.ForEach(delegate(int _)
			{
				trackableSkills.Remove(_);
			});
			list.Select((int _) => TSkill.Table[_].RootActionNode).Where((SkillActionNode _) => _?.skillAction?.actionEvents != null).Where((SkillActionNode _) => _?.skillAction?.SkillTrigger is AddPassiveSkillTrigger)
				.SelectMany((SkillActionNode _) => _?.skillAction?.actionEvents)
				.Select((ActionEvent _) => _ as BuffActionEvent)
				.Where((BuffActionEvent _) => _ != null)
				.Select((BuffActionEvent _) => _.Id)
				.ToList()
				.ForEach(delegate(int _)
				{
					RemoveBuff(_, 1, isExpired: false);
				});
		}
		OnPassiveSkillsChanged.OnNext(PassiveSkillIds.ToArray());
	}

	public List<int> ApplyPassiveSkillsDelta(IEnumerable<int> newSkillIds)
	{
		List<int> list = PassiveSkillIds ?? new List<int>();
		List<int> list2 = newSkillIds?.ToList() ?? new List<int>();
		List<int> list3 = list2.Except(list).ToList();
		List<int> list4 = list.Except(list2).ToList();
		if (list3.Count == 0 && list4.Count == 0)
		{
			return list3;
		}
		PassiveSkillIds = list2;
		if (base.NetworkManager.IsHost)
		{
			list4.ForEach(delegate(int _)
			{
				trackableSkills.Remove(_);
			});
			list4.Select((int _) => TSkill.Table[_].RootActionNode).Where((SkillActionNode _) => _?.skillAction?.actionEvents != null).Where((SkillActionNode _) => _?.skillAction?.SkillTrigger is AddPassiveSkillTrigger)
				.SelectMany((SkillActionNode _) => _?.skillAction?.actionEvents)
				.Select((ActionEvent _) => _ as BuffActionEvent)
				.Where((BuffActionEvent _) => _ != null)
				.Select((BuffActionEvent _) => _.Id)
				.ToList()
				.ForEach(delegate(int _)
				{
					RemoveBuff(_, 1, isExpired: false);
				});
		}
		OnPassiveSkillsChanged.OnNext(PassiveSkillIds.ToArray());
		return list3;
	}

	public void ClearConsumableTypeCoolTimes()
	{
		AssertIfNotHost();
		ConsumableTypeCoolTimes.Values.Select((UnitItemCoolTime _) => _.Clone()).ToList().ForEach(delegate(UnitItemCoolTime _)
		{
			_.EndTime = 0f;
			ConsumableTypeCoolTimes[_.ConsumableType] = _;
		});
	}

	public virtual void UpdateAim()
	{
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		SetNetworkVariable(ref syncPosition, new PlanarPos(base.transform.position));
		syncHp.Value = (int)GetStat(EStatType.HpMax);
		syncStamina.Value = GetStat(EStatType.StaminaMax);
		syncStress.Value = 0f;
		syncShield.Value = 0;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		HitRadius = GetComponent<CapsuleCollider>().radius;
		if (!base.NetworkManager.IsHost)
		{
			rxPosition.Value = syncPosition.Value.ToVector3();
			base.transform.position = syncPosition.Value.ToVector3();
			rxHp.Value = syncHp.Value;
			rxStress.Value = syncStress.Value;
			GameChannel.Receive<ResSyncUnitModel>().Where((ResSyncUnitModel _) => _.NetworkObjectId == base.NetworkObjectId).Subscribe(OnResSyncUnitModel)
				.AddTo(this);
			GameChannel.Receive<NtfUpdateUnitModel>().Where((NtfUpdateUnitModel _) => _.NetworkObjectId == base.NetworkObjectId).Subscribe(OnUpdateUnitModel)
				.AddTo(this);
			GameChannel.SendToHost(new ReqSyncUnitModel
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
		else
		{
			rxHp.Value = (int)GetStat(EStatType.HpMax);
			rxStamina.Value = GetStat(EStatType.StaminaMax);
			rxStress.Value = 0f;
			GameHost.Receive<ReqSyncUnitModel>().Where((ReqSyncUnitModel _) => _.NetworkObjectId == base.NetworkObjectId).Subscribe(OnReqSyncUnitModel)
				.AddTo(this);
		}
	}

	public override void OnNetworkDespawn()
	{
		UnregisterVariableEvents();
		UnregisterListEvents();
		base.OnNetworkDespawn();
	}

	protected void SetNetworkVariable<T>(ref NetworkVariable<T> networkVariable, T value) where T : struct
	{
		if (IsHost)
		{
			networkVariable.Value = value;
		}
		else
		{
			Logger.Warning("Player is not a host.");
		}
	}

	protected void RegisterVariableEvent<T>(NetworkVariable<T> syncVar, ReactiveProperty<T> rxVar) where T : struct
	{
		rxVar.Value = syncVar.Value;
		syncVar.OnValueChanged = (NetworkVariable<T>.OnValueChangedDelegate)Delegate.Combine(syncVar.OnValueChanged, (NetworkVariable<T>.OnValueChangedDelegate)delegate(T oldValue, T newValue)
		{
			rxVar.Value = newValue;
		});
	}

	protected void RegisterVariableEvent<TWire, TValue>(NetworkVariable<TWire> syncVar, ReactiveProperty<TValue> rxVar, Func<TWire, TValue> decode) where TWire : struct
	{
		rxVar.Value = decode(syncVar.Value);
		syncVar.OnValueChanged = (NetworkVariable<TWire>.OnValueChangedDelegate)Delegate.Combine(syncVar.OnValueChanged, (NetworkVariable<TWire>.OnValueChangedDelegate)delegate(TWire oldValue, TWire newValue)
		{
			rxVar.Value = decode(newValue);
		});
	}

	protected void UnregisterVariableEvent<T>(NetworkVariable<T> syncVar) where T : struct
	{
		if (syncVar != null)
		{
			syncVar.OnValueChanged = null;
		}
	}

	private void RegisterListEvent<T>(NetworkList<T> syncVar, IList<T> rxVar) where T : unmanaged, IEquatable<T>
	{
		NetworkList<T>.OnListChangedDelegate onListChangedDelegate = delegate(NetworkListEvent<T> changeEvent)
		{
			switch (changeEvent.Type)
			{
			case NetworkListEvent<T>.EventType.Add:
				rxVar.Add(changeEvent.Value);
				break;
			case NetworkListEvent<T>.EventType.Insert:
				rxVar.Insert(changeEvent.Index, changeEvent.Value);
				break;
			case NetworkListEvent<T>.EventType.Remove:
				rxVar.RemoveAt(changeEvent.Index);
				break;
			case NetworkListEvent<T>.EventType.Value:
				rxVar[changeEvent.Index] = changeEvent.Value;
				break;
			case NetworkListEvent<T>.EventType.Clear:
				rxVar.Clear();
				break;
			}
		};
		syncVar.OnListChanged += onListChangedDelegate;
		_listEventHandlers.Add(syncVar, onListChangedDelegate);
	}

	protected void UnregisterListEvent<T>(NetworkList<T> syncVar) where T : unmanaged, IEquatable<T>
	{
		if (_listEventHandlers.TryGetValue(syncVar, out var value))
		{
			syncVar.OnListChanged -= (NetworkList<T>.OnListChangedDelegate)value;
			_listEventHandlers.Remove(syncVar);
		}
	}

	public void AddBuff(int buffId, IActionStats actionStats, int stack = 1, ulong casterNetworkObjectId = 0uL, Vector3 point = default(Vector3))
	{
		AssertIfNotHost();
		if (BaseStats == null)
		{
			Logger.Error(string.Format("UnitModel.AddBuff called before Init. buffId: {0}", buffId));
			return;
		}
		if (IsInactiveInStage)
		{
			return;
		}
		if (buffId <= 0)
		{
			Logger.Error(string.Format("UnitModel.AddBuff called with invalid buffId: {0}", buffId));
			return;
		}
		TBuff tBuff = TBuff.Table[buffId];
		if (HasImmunityEffect(tBuff.EffectTag))
		{
			GameHost.SendToAll(new NtfOnImmuneStatusEffect
			{
				NetworkObjectId = base.NetworkObjectId,
				StatusEffectTag = tBuff.EffectTag,
				IsFullyImmune = HasFullyImmuneEffect()
			});
			return;
		}
		bool flag = false;
		UnitBuff value;
		bool flag2 = Buffs.TryGetValue(buffId, out value);
		if (flag2)
		{
			flag = value.IsActive && !value.IsSuppressionBuff;
		}
		UnitBuff unitBuff;
		if (value == null)
		{
			unitBuff = new UnitBuff
			{
				Id = buffId
			};
		}
		else
		{
			unitBuff = value.Clone();
		}
		if (unitBuff.CasterIds == null)
		{
			unitBuff.CasterIds = Array.Empty<ulong>();
		}
		if (casterNetworkObjectId != 0)
		{
			unitBuff.CasterIds = unitBuff.CasterIds.Concat(new ulong[1] { casterNetworkObjectId }).Distinct().ToArray();
		}
		unitBuff.AddStack(this, actionStats, GameManager.TimeNow, stack);
		unitBuff.IsActive = CheckBuffTrigger(buffId);
		unitBuff.IsSuppressionBuff = this is MonsterModel monsterModel && monsterModel.IsSuppressionBuffTag(tBuff.EffectTag);
		Vector3 vector = ((point == Vector3.zero) ? rxPosition.Value : point);
		unitBuff.Point = vector.ToProtoVector3();
		Buffs[buffId] = unitBuff;
		if (unitBuff.IsActive && !unitBuff.IsSuppressionBuff)
		{
			if (flag)
			{
				RefreshBuffStats(buffId);
			}
			else
			{
				OnBuffActivated(buffId);
			}
		}
	}

	public void RemoveBuff(int buffId, int stack = 1, bool isExpired = false)
	{
		UnitBuff buff = null;
		TBuff buffData = null;
		AssertIfNotHost();
		if (buffId < 1)
		{
			Logger.Error(string.Format("UnitModel.RemoveBuff called with invalid buffId: {0}", buffId));
			return;
		}
		if (!Buffs.TryGetValue(buffId, out var value))
		{
			return;
		}
		buffData = TBuff.Table[buffId];
		if (isExpired)
		{
			stack = ((!buffData.ExpirePerStack) ? value.StackCount : 1);
		}
		if (value == null)
		{
			return;
		}
		buff = value.Clone();
		bool isActive = value.IsActive;
		buff.RemoveStack(stack);
		if (buff.StackCount < 1)
		{
			if (!IsInactiveInStage)
			{
				CheckSpawnHitBoxOnBuffEnd();
			}
			Buffs.Remove(buffId);
			if (isActive)
			{
				RefreshBuffStats(buffId);
				OnBuffDeactivated(buffId);
			}
			GetComponent<UnitController>()?.HandleUnitEvent(new BuffRemovedEvent
			{
				BuffId = buffId,
				IsExpired = isExpired
			});
		}
		else
		{
			if (isExpired && buffData.ExpirePerStack)
			{
				buff.EndTime = GameManager.TimeNow + GetModifiedBuffDuration(buffData);
			}
			Buffs[buffId] = buff;
			if (isActive)
			{
				RefreshBuffStats(buffId);
			}
		}
		void CheckSpawnHitBoxOnBuffEnd()
		{
			IEnumerable<SpawnHitBoxOnEndEffect> enumerable = buffData.BuffEffects?.Where((BuffEffect e) => e is SpawnHitBoxOnEndEffect).Select((BuffEffect e) => e as SpawnHitBoxOnEndEffect) ?? Enumerable.Empty<SpawnHitBoxOnEndEffect>();
			foreach (SpawnHitBoxOnEndEffect item in enumerable)
			{
				if (item == null || item.HitBoxId <= 0 || !item.SpawnOnDie)
				{
					continue;
				}
				ulong[] casterIds = buff.CasterIds;
				for (int i = 0; i < casterIds.Length; i++)
				{
					UnitModel unitModel = ObjectManager.FindUnitModel(casterIds[i]);
					if (!(unitModel == null))
					{
						Vector3 value2 = rxPosition.Value;
						Vector3 deltaAimPos = ((unitModel.rxForward.Value == Vector3.zero) ? Vector3.forward : unitModel.rxForward.Value);
						ObjectManager.SpawnHitBoxAsync(item.HitBoxId, unitModel, unitModel.ToStatsData(), unitModel.GetHitLayerInfo(), deltaAimPos, value2).Forget();
					}
				}
			}
		}
	}

	public void ClearBuffs()
	{
		AssertIfNotHost();
		Buffs.Values.ToList().ForEach(delegate(UnitBuff _)
		{
			RemoveBuff(_.Id, _.StackCount);
		});
	}

	public void SetBuff(int buffId, IActionStats actionStats, int stack, ulong casterNetworkObjectId = 0uL, Vector3 point = default(Vector3))
	{
		AssertIfNotHost();
		if (Buffs.TryGetValue(buffId, out var value))
		{
			stack -= value.StackCount;
		}
		if (stack != 0)
		{
			if (stack < 1)
			{
				RemoveBuff(buffId, Math.Abs(stack));
			}
			else
			{
				AddBuff(buffId, actionStats, stack, casterNetworkObjectId, point);
			}
		}
	}

	public void UpdateBuffCasterId(int buffId, IList<ulong> casterIds)
	{
		AssertIfNotHost();
		if (casterIds != null && Buffs.TryGetValue(buffId, out var value) && !(value.CasterIds ?? Array.Empty<ulong>()).SequenceEqual(casterIds))
		{
			UnitBuff unitBuff = value.Clone();
			unitBuff.CasterIds = casterIds.ToArray();
			Buffs[buffId] = unitBuff;
		}
	}

	public int GetBuffStackCount(int buffId)
	{
		if (!Buffs.TryGetValue(buffId, out var value))
		{
			return 0;
		}
		return value.StackCount;
	}

	protected virtual void OnBuffActivated(int buffId)
	{
		RefreshBuffStats(buffId);
		SubscribeRealtimeCasterStats(buffId);
		TBuff tBuff = TBuff.Table[buffId];
		if (tBuff.BuffEffects == null)
		{
			return;
		}
		BuffEffect[] buffEffects = tBuff.BuffEffects;
		foreach (BuffEffect buffEffect in buffEffects)
		{
			if (buffEffect is ImmunityEffect immunityEffect)
			{
				ApplyImmunityEffect(immunityEffect);
			}
			else if (buffEffect is EscapeOnActiveEffect)
			{
				if (IsHitable)
				{
					GetComponent<UnitController>()?.HandleUnitEvent(new EscapeEvent());
				}
			}
			else if (buffEffect is ChangeSkillEffect changeSkillEffect)
			{
				RemoveConflictingChangeSkillBuff(buffId, changeSkillEffect.OriginalSkillId);
				OnChangeSkillEffectActivated(changeSkillEffect);
			}
		}
	}

	protected virtual void OnBuffDeactivated(int buffId)
	{
		UnsubscribeRealtimeCasterStats(buffId);
		RefreshBuffStats(buffId);
		TBuff tBuff = TBuff.Table[buffId];
		if (tBuff.BuffEffects == null)
		{
			return;
		}
		BuffEffect[] buffEffects = tBuff.BuffEffects;
		foreach (BuffEffect buffEffect in buffEffects)
		{
			if (buffEffect is ChangeSkillEffect effect)
			{
				OnChangeSkillEffectDeactivated(effect);
			}
		}
	}

	private void RemoveConflictingChangeSkillBuff(int newBuffId, int originalSkillId)
	{
		int num = 0;
		foreach (KeyValuePair<int, UnitBuff> item in Buffs)
		{
			int key = item.Key;
			UnitBuff value = item.Value;
			if (key == newBuffId || !value.IsEffectActive)
			{
				continue;
			}
			BuffEffect[] buffEffects = TBuff.Table[key].BuffEffects;
			if (buffEffects != null)
			{
				foreach (BuffEffect buffEffect in buffEffects)
				{
					if (buffEffect is ChangeSkillEffect changeSkillEffect && changeSkillEffect.OriginalSkillId == originalSkillId)
					{
						num = key;
						break;
					}
				}
				if (num > 0)
				{
					break;
				}
			}
		}
		if (num > 0)
		{
			RemoveBuff(num, int.MaxValue);
		}
	}

	protected virtual void OnChangeSkillEffectActivated(ChangeSkillEffect effect)
	{
	}

	protected virtual void OnChangeSkillEffectDeactivated(ChangeSkillEffect effect)
	{
	}

	private void RefreshBuffStats(int buffId)
	{
		TBuff tBuff = TBuff.Table[buffId];
		if (tBuff.BuffEffects == null)
		{
			return;
		}
		BuffEffect[] buffEffects = tBuff.BuffEffects;
		foreach (BuffEffect buffEffect in buffEffects)
		{
			if (buffEffect == null)
			{
				continue;
			}
			if (buffEffect is SlowEffect slowEffect)
			{
				if (slowEffect.SlowType == ESlowType.MoveSlow)
				{
					UpdateStat(EStatType.MoveSpeed);
				}
				else if (slowEffect.SlowType == ESlowType.AttackSlow)
				{
					UpdateStat(EStatType.AtkSpeed);
				}
			}
			else if (buffEffect is StatEffect statEffect)
			{
				UpdateStat(statEffect.statType);
			}
		}
	}

	public bool CheckBuffTrigger(int buffId)
	{
		if (!TBuff.Table.TryGetValue(buffId, out var value))
		{
			return false;
		}
		if (GameManager.IsInLounge && !HasBuffEffect(EBuffEffectType.TrainingRoom) && value.InactiveInLounge)
		{
			return false;
		}
		if (value.Triggers == null || value.Triggers.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < value.Triggers.Length; i++)
		{
			if (value.Triggers[i] != null && !EvaluateBuffTrigger(this, value.Triggers[i], value))
			{
				return false;
			}
		}
		return true;
	}

	private bool EvaluateBuffTrigger(UnitModel unit, BuffTriggerBase trigger, TBuff buffData)
	{
		if (!(trigger is FloatValueCompareBuffTrigger trigger2))
		{
			if (!(trigger is ActiveSkillTagBuffTrigger trigger3))
			{
				if (trigger is NearbyUnitCountBuffTrigger trigger4)
				{
					return EvaluateNearbyUnitCount(unit, trigger4, buffData);
				}
				return true;
			}
			return EvaluateActiveSkillTag(unit, trigger3);
		}
		return EvaluateFloatValueCompare(unit, trigger2, buffData);
	}

	private bool EvaluateFloatValueCompare(UnitModel u, FloatValueCompareBuffTrigger trigger, TBuff buffData)
	{
		float a;
		switch (trigger.TriggerType)
		{
		default:
			return true;
		case EBuffTrigger.HpPercent:
			a = (int)((float)u.Hp / u.GetStat(EStatType.HpMax) * 100f);
			break;
		case EBuffTrigger.Stress:
			a = u.Stress;
			break;
		case EBuffTrigger.LightFuel:
			if (u is CharacterModel characterModel)
			{
				a = ((!GameManager.IsInStage) ? 100f : ((float)characterModel.LightFuel));
				break;
			}
			a = 0f;
			break;
		case EBuffTrigger.Stack:
			a = u.GetBuffStackCount(buffData.Id);
			break;
		case EBuffTrigger.CorruptionPercent:
			if (u.GetStat(EStatType.CorruptionMax) > 0f)
			{
				a = (int)((float)u.Corruption / u.GetStat(EStatType.CorruptionMax) * 100f);
				break;
			}
			a = 0f;
			break;
		}
		return ValueComparer.Compare(a, trigger.CompareType, trigger.CompareValue);
	}

	private bool EvaluateActiveSkillTag(UnitModel u, ActiveSkillTagBuffTrigger trigger)
	{
		if (trigger.SkillTags == null || trigger.SkillTags.Length == 0)
		{
			return false;
		}
		int value = u.rxUsingSkillId.Value;
		if (value < 1)
		{
			return false;
		}
		TSkill skillData = TSkill.Table[value];
		if (!trigger.MatchAny)
		{
			return trigger.SkillTags.All((ESkillTagType tag) => skillData.SkillTags.Contains(tag));
		}
		return trigger.SkillTags.Any((ESkillTagType tag) => skillData.SkillTags.Contains(tag));
	}

	private bool EvaluateNearbyUnitCount(UnitModel self, NearbyUnitCountBuffTrigger trigger, TBuff buffData)
	{
		if (trigger.CompareValue <= 0f && trigger.CompareType == ECompareType.GreaterEqual)
		{
			return true;
		}
		IEnumerable<UnitModel> unitModels = ObjectManager.UnitModels;
		HitLayerInfo hitLayerInfo = self.GetHitLayerInfo();
		int num = 0;
		if (trigger.Relation == EHitTarget.None)
		{
			trigger.Relation = EHitTarget.All;
		}
		foreach (UnitModel other in unitModels)
		{
			if (other == null || !other.IsHitable)
			{
				continue;
			}
			if (trigger.UnitType != EUnitType.None && other.UnitType != trigger.UnitType)
			{
				continue;
			}
			float num2 = ((trigger.Range <= 0f) ? (-1f) : (trigger.Range * trigger.Range));
			if (num2 > 0f && (other.rxPosition.Value - self.rxPosition.Value).sqrMagnitude > num2)
			{
				continue;
			}
			HitLayerInfo hitLayerInfo2 = other.GetHitLayerInfo();
			if (trigger.Relation.IsHitable(hitLayerInfo, hitLayerInfo2, other.HitLayer))
			{
				if (trigger.NearUnitConditionTriggers != null && trigger.NearUnitConditionTriggers.Length != 0 && !trigger.NearUnitConditionTriggers.Where((BuffTriggerBase t) => t != null).All((BuffTriggerBase cond) => EvaluateBuffTrigger(other, cond, buffData)))
				{
					continue;
				}
				num++;
			}
		}
		return ValueComparer.Compare((float)num, trigger.CompareType, trigger.CompareValue);
	}

	public void UpdateBuffs()
	{
		float timeNow = GameManager.TimeNow;
		List<int> list = new List<int>();
		foreach (UnitBuff value in Buffs.Values)
		{
			TBuff tBuff = TBuff.Table[value.Id];
			if (tBuff.Duration >= 0f && !_expirePreventedStackBuffRefCounts.ContainsKey(value.Id) && !(timeNow < value.EndTime))
			{
				list.Add(value.Id);
			}
		}
		list.ForEach(delegate(int _)
		{
			RemoveBuff(_, 1, isExpired: true);
		});
		List<UnitBuff> list2 = new List<UnitBuff>();
		foreach (UnitBuff value2 in Buffs.Values)
		{
			bool flag = CheckBuffTrigger(value2.Id);
			if (value2.IsActive != flag)
			{
				UnitBuff unitBuff = value2.Clone();
				unitBuff.IsActive = flag;
				list2.Add(unitBuff);
			}
		}
		foreach (UnitBuff item in list2)
		{
			Buffs[item.Id] = item;
			if (!item.IsActive || item.IsSuppressionBuff)
			{
				OnBuffDeactivated(item.Id);
			}
			else
			{
				OnBuffActivated(item.Id);
			}
		}
	}

	private void ApplyImmunityEffect(ImmunityEffect immunityEffect)
	{
		if (immunityEffect != null && immunityEffect.DispelOnApply)
		{
			RemoveStatusEffects(immunityEffect.ImmunityEffectTags);
		}
	}

	public void RemoveStatusEffects(EStatusEffectTag effectTag)
	{
		Buffs.Values.ToList().ForEach(delegate(UnitBuff _)
		{
			TBuff tBuff = TBuff.Table[_.Id];
			if (tBuff.EffectTag != EStatusEffectTag.None && (effectTag & tBuff.EffectTag) == tBuff.EffectTag)
			{
				GameHost.SendToAll(new NtfOnImmuneStatusEffect
				{
					NetworkObjectId = base.NetworkObjectId,
					IsFullyImmune = (effectTag == (EStatusEffectTag)(-1))
				});
				RemoveBuff(_.Id, _.StackCount);
			}
		});
		CrowdControls.Keys.ToList().ForEach(delegate(ECrowdControlType _)
		{
			EStatusEffectTag effectTag2 = _.GetEffectTag();
			if ((effectTag2 & effectTag) == effectTag2)
			{
				GameHost.SendToAll(new NtfOnImmuneStatusEffect
				{
					NetworkObjectId = base.NetworkObjectId,
					IsFullyImmune = (effectTag == (EStatusEffectTag)(-1))
				});
				CrowdControls.Remove(_);
			}
		});
	}

	public bool HasFullyImmuneEffect()
	{
		return Buffs.Values.Where((UnitBuff _) => _.IsActive).Where((UnitBuff _) => TBuff.Table[_.Id].BuffEffects != null).SelectMany((UnitBuff _) => TBuff.Table[_.Id].BuffEffects)
			.Where((BuffEffect _) => _.EffectType == EBuffEffectType.Immunity)
			.Select((BuffEffect _) => _ as ImmunityEffect)
			.Any((ImmunityEffect _) => _.IsFullImmunity);
	}

	public bool HasImmunityEffect(EStatusEffectTag effectTag)
	{
		if (effectTag == EStatusEffectTag.None)
		{
			return false;
		}
		return Buffs.Values.Where((UnitBuff _) => _.IsActive).Where((UnitBuff _) => TBuff.Table[_.Id].BuffEffects != null).SelectMany((UnitBuff _) => TBuff.Table[_.Id].BuffEffects)
			.Where((BuffEffect _) => _.EffectType == EBuffEffectType.Immunity)
			.Select((BuffEffect _) => _ as ImmunityEffect)
			.Any((ImmunityEffect _) => (_.ImmunityEffectTags & effectTag) == effectTag);
	}

	public void UpdateStat(EStatType statType)
	{
		AssertIfNotHost();
		GetStat(statType);
		switch (statType)
		{
		case EStatType.Atk:
		{
			float num7 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat7 = GetAdditionalStat(statType);
			float num8 = GetBaseStat(statType) + num7 + additionalStat7;
			if (num8 <= 0f)
			{
				num8 = 0f;
			}
			num8 = (GetStat(EStatType.AtkPercent) / 100f + 1f) * num8;
			SetStat(statType, num8);
			break;
		}
		case EStatType.Def:
		case EStatType.HpMax:
		case EStatType.HpRegen:
		case EStatType.RunStaminaCost:
			UpdateOriginalStat();
			break;
		case EStatType.MoveSpeed:
		{
			float num9 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat8 = GetAdditionalStat(statType);
			float num10 = GetBaseStat(statType) + num9 + additionalStat8;
			if (num10 <= 0f)
			{
				num10 = 0f;
			}
			if (this is CharacterModel characterModel)
			{
				if (characterModel.TryRun && State != EActionState.Skill)
				{
					num10 = GetStat(EStatType.RunIncreaseSpeed) + num10;
					if (num10 <= 0f)
					{
						num10 = 0f;
					}
				}
				if (characterModel.RxIsMovingBackward.Value)
				{
					num10 *= TConst.BackwardMoveSpeedMultiplier.FloatValue;
				}
			}
			num10 = (GetStat(EStatType.MoveSpeedPercent) / 100f + 1f) * num10 * (GetSlowPercent(ESlowType.MoveSlow) / 100f + 1f);
			SetStat(statType, num10);
			break;
		}
		case EStatType.AtkSpeed:
		{
			float num5 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat6 = GetAdditionalStat(statType);
			float num6 = GetBaseStat(statType) + num5 + additionalStat6;
			if (num6 <= 0f)
			{
				num6 = 0f;
			}
			num6 = (GetStat(EStatType.AtkSpeedPercent) / 100f + 1f) * num6 * (GetSlowPercent(ESlowType.AttackSlow) / 100f + 1f);
			SetStat(statType, num6);
			break;
		}
		case EStatType.StaminaMax:
		case EStatType.StaminaRegen:
		case EStatType.Penetration:
		case EStatType.CriticalTakenChancePercent:
		case EStatType.CriticalDamagePercent:
		case EStatType.WeaknessDamagePercent:
		case EStatType.BackAttackDamagePercent:
			UpdateIndividualStat(allowNegative: false);
			break;
		case EStatType.RunIncreaseSpeed:
			UpdateOriginalStat();
			if (this is CharacterModel characterModel2 && characterModel2.TryRun)
			{
				UpdateStat(EStatType.MoveSpeed);
			}
			break;
		case EStatType.CorruptionMax:
		{
			float num3 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat4 = GetAdditionalStat(statType);
			float num4 = GetBaseStat(statType) + num3 + additionalStat4;
			if (num4 <= 0f)
			{
				num4 = 0f;
			}
			SetStat(statType, num4);
			break;
		}
		case EStatType.AtkPercent:
		case EStatType.DefPercent:
		case EStatType.MoveSpeedPercent:
		case EStatType.AtkSpeedPercent:
		case EStatType.HpMaxPercent:
		case EStatType.HpRegenPercent:
		case EStatType.RunStaminaCostPercent:
		case EStatType.RunIncreaseSpeedPercent:
			UpdatePercentageStat();
			break;
		case EStatType.PenetrationPercent:
		{
			List<StatEffect> list = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Where((UnitBuff _) => TBuff.Table[_.Id].BuffEffects != null).SelectMany((UnitBuff _) => TBuff.Table[_.Id].BuffEffects)
				.Where((BuffEffect _) => _.EffectType == EBuffEffectType.Stat)
				.Select((BuffEffect _) => _ as StatEffect)
				.Where((StatEffect _) => _.statType == statType)
				.ToList();
			float additionalStat2 = GetAdditionalStat(statType);
			float baseStat = GetBaseStat(statType);
			if (baseStat == 0f && additionalStat2 == 0f && list.Count < 1)
			{
				SetStat(statType, 0f);
				break;
			}
			float num = (100f - GetBaseStat(statType)) / 100f;
			if (additionalStat2 > 0f)
			{
				num *= (100f - additionalStat2) / 100f;
			}
			foreach (StatEffect item in list)
			{
				num *= (100f - item.statValue) / 100f;
			}
			SetStat(statType, (1f - num) * 100f);
			break;
		}
		case EStatType.CriticalChancePercent:
		{
			float num11 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat9 = GetAdditionalStat(statType);
			float num12 = GetBaseStat(statType) + num11 + additionalStat9;
			if (num12 <= 0f)
			{
				num12 = 0f;
			}
			SetStat(statType, num12);
			break;
		}
		case EStatType.CooldownReductionPercent:
		case EStatType.AdditionalAttackDamagePercent:
		case EStatType.ReductionDamagePercent:
		case EStatType.DashDistancePercent:
		case EStatType.FireResistancePercent:
		case EStatType.WaterResistancePercent:
		case EStatType.WindResistancePercent:
		case EStatType.FireDamagePercent:
		case EStatType.WaterDamagePercent:
		case EStatType.WindDamagePercent:
			UpdateIndividualStat(allowNegative: true);
			break;
		}
		void UpdateIndividualStat(bool allowNegative)
		{
			float num13 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat10 = GetAdditionalStat(statType);
			float num14;
			if (allowNegative)
			{
				num14 = GetBaseStat(statType) + num13 + additionalStat10;
			}
			else
			{
				num14 = GetBaseStat(statType) + num13 + additionalStat10;
				if (num14 <= 0f)
				{
					num14 = 0f;
				}
			}
			SetStat(statType, num14);
		}
		void UpdateOriginalStat()
		{
			float num15 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff x) => x.GetStat(statType));
			float additionalStat11 = GetAdditionalStat(statType);
			float num16 = GetBaseStat(statType) + num15 + additionalStat11;
			if (num16 <= 0f)
			{
				num16 = 0f;
			}
			if (!Enum.TryParse<EStatType>(string.Format("{0}Percent", statType), out var result))
			{
				Serilog.Log.Error(string.Format("Invalid stat type conversion from {0} to {1}", statType, result));
				return;
			}
			num16 = (GetStat(result) / 100f + 1f) * num16;
			if (num16 <= 1f)
			{
				num16 = 1f;
			}
			SetStat(statType, num16);
		}
		void UpdatePercentageStat()
		{
			float num17 = Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Sum((UnitBuff buff) => buff.GetStat(statType));
			float additionalStat12 = GetAdditionalStat(statType);
			SetStat(statType, GetBaseStat(statType) + num17 + additionalStat12);
			string text = statType.ToString().Replace("Percent", "");
			if (!Enum.TryParse<EStatType>(text, out var result2))
			{
				Serilog.Log.Error(string.Format("Invalid stat type conversion from {0} to {1}", statType, text));
			}
			else
			{
				UpdateStat(result2);
			}
		}
	}

	private float GetSlowPercent(ESlowType slowType)
	{
		return Buffs.Values.Where((UnitBuff b) => b.IsEffectActive).Where((UnitBuff _) => TBuff.Table[_.Id].BuffEffects != null).SelectMany((UnitBuff unitBuff) => TBuff.Table[unitBuff.Id].BuffEffects.Where((BuffEffect b) => b.EffectType == EBuffEffectType.Slow).Where((BuffEffect b) => ((SlowEffect)b).SlowType == slowType).Select(delegate(BuffEffect b)
		{
			SlowEffect slowEffect = (SlowEffect)b;
			float num = (float)unitBuff.StackCount * slowEffect.value;
			if (slowEffect.UseStatValue)
			{
				float num2 = ((!slowEffect.RealtimeStatValue) ? ((unitBuff.StackEntries != null) ? unitBuff.StackEntries.GetCasterStatSum(slowEffect.StatType) : 0f) : unitBuff.GetRealtimeCasterStatSum(slowEffect.StatType));
				num += num2 * slowEffect.StatPercent / 100f;
			}
			return num;
		})).DefaultIfEmpty(0f).Min();
	}

	private void SubscribeRealtimeCasterStats(int buffId)
	{
		if (!Buffs.TryGetValue(buffId, out var value))
		{
			return;
		}
		List<BuffEffect> list = TBuff.Table[buffId].BuffEffects?.Where((BuffEffect e) => e.UseStatValue && e.RealtimeStatValue).ToList();
		if (list == null || list.Count == 0)
		{
			return;
		}
		HashSet<EStatType> statTypes = list.Select((BuffEffect e) => e.StatType).Distinct().ToHashSet();
		ulong[] array = value.CasterIds;
		if (array == null)
		{
			array = Array.Empty<ulong>();
		}
		ulong[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ulong num = array2[i];
			UnitModel unitModel = ObjectManager.FindUnitModel(num);
			if (unitModel == null)
			{
				continue;
			}
			(int, ulong) key = (buffId, num);
			if (_realtimeStatSubs.TryGetValue(key, out var value2))
			{
				value2.Dispose();
			}
			if (value.RealtimeCasterStats == null)
			{
				value.RealtimeCasterStats = new Dictionary<(ulong, EStatType), float>();
			}
			foreach (EStatType item in statTypes)
			{
				value.RealtimeCasterStats[(num, item)] = unitModel.GetStat(item);
			}
			CompositeDisposable compositeDisposable = new CompositeDisposable();
			unitModel.RxStats.ObserveChange().Where(statTypes.Contains).Subscribe(delegate(EStatType statType)
			{
				MarkRealtimeStatDirty(buffId, statType);
			})
				.AddTo(compositeDisposable);
			_realtimeStatSubs[key] = compositeDisposable;
		}
	}

	private void UnsubscribeRealtimeCasterStats(int buffId)
	{
		foreach ((int buffId, ulong casterId) item in _realtimeStatSubs.Keys.Where(((int buffId, ulong casterId) k) => k.buffId == buffId).ToList())
		{
			_realtimeStatSubs[item].Dispose();
			_realtimeStatSubs.Remove(item);
		}
	}

	private void MarkRealtimeStatDirty(int buffId, EStatType changedCasterStatType)
	{
		TBuff tBuff = TBuff.Table[buffId];
		if (tBuff.BuffEffects == null)
		{
			return;
		}
		BuffEffect[] buffEffects = tBuff.BuffEffects;
		foreach (BuffEffect buffEffect in buffEffects)
		{
			if (buffEffect != null && buffEffect.UseStatValue && buffEffect.RealtimeStatValue && buffEffect.StatType == changedCasterStatType)
			{
				if (buffEffect is StatEffect statEffect)
				{
					_dirtyRealtimeStats.Add(statEffect.statType);
				}
				else if (buffEffect is SlowEffect slowEffect)
				{
					_dirtyRealtimeStats.Add((slowEffect.SlowType == ESlowType.MoveSlow) ? EStatType.MoveSpeed : EStatType.AtkSpeed);
				}
			}
		}
		if (!_isConverging)
		{
			ConvergeRealtimeStats();
		}
	}

	private void ConvergeRealtimeStats()
	{
		_isConverging = true;
		for (int i = 0; i < 20; i++)
		{
			if (_dirtyRealtimeStats.Count == 0)
			{
				break;
			}
			List<EStatType> list = _dirtyRealtimeStats.ToList();
			_dirtyRealtimeStats.Clear();
			Dictionary<EStatType, float> before = list.ToDictionary((EStatType s) => s, GetStat);
			RefreshAllRealtimeCasterValues();
			foreach (EStatType item in list)
			{
				UpdateStat(item);
			}
			if (list.Max((EStatType s) => Mathf.Abs(GetStat(s) - before[s])) < 0.01f)
			{
				break;
			}
		}
		_dirtyRealtimeStats.Clear();
		_isConverging = false;
	}

	private void RefreshAllRealtimeCasterValues()
	{
		foreach ((int, ulong) key in _realtimeStatSubs.Keys)
		{
			int item = key.Item1;
			ulong item2 = key.Item2;
			if (!Buffs.TryGetValue(item, out var value) || value.RealtimeCasterStats == null)
			{
				continue;
			}
			UnitModel unitModel = ObjectManager.FindUnitModel(item2);
			if (unitModel == null)
			{
				continue;
			}
			BuffEffect[] buffEffects = TBuff.Table[item].BuffEffects;
			if (buffEffects == null)
			{
				continue;
			}
			foreach (BuffEffect buffEffect in buffEffects)
			{
				if (buffEffect.UseStatValue && buffEffect.RealtimeStatValue)
				{
					value.RealtimeCasterStats[(item2, buffEffect.StatType)] = unitModel.GetStat(buffEffect.StatType);
				}
			}
		}
	}

	public float GetModifiedBuffDuration(TBuff buffData)
	{
		float num = buffData.Duration;
		if (buffData.EffectTag != EStatusEffectTag.None)
		{
			IList<BuffEffectInfo<StatusEffectDurationAmplifierEffect>> buffEffects = GetBuffEffects<StatusEffectDurationAmplifierEffect>();
			if (buffEffects.Count > 0)
			{
				float num2 = buffEffects.Where((BuffEffectInfo<StatusEffectDurationAmplifierEffect> e) => (e.BuffEffect.TargetStatusEffect & buffData.EffectTag) == buffData.EffectTag).Sum((BuffEffectInfo<StatusEffectDurationAmplifierEffect> e) => e.BuffEffect.Percent);
				if (Mathf.Abs(num2) > 0.001f)
				{
					num *= num2 / 100f + 1f;
				}
			}
		}
		return num;
	}

	public bool IsSkillTargetable(int skillId, UnitModel target)
	{
		if (!target.IsHitable)
		{
			return false;
		}
		return TSkill.Table[skillId].TargetType.IsHitable(this.GetHitLayerInfo(), target.GetHitLayerInfo(), TargetHitLayer);
	}

	public void AddCrowdControl(ECrowdControlType type, float duration)
	{
		float timeNow = GameManager.TimeNow;
		EStatusEffectTag effectTag = type.GetEffectTag();
		if (effectTag != EStatusEffectTag.None)
		{
			IList<BuffEffectInfo<StatusEffectDurationAmplifierEffect>> buffEffects = GetBuffEffects<StatusEffectDurationAmplifierEffect>();
			if (buffEffects.Count > 0)
			{
				float num = buffEffects.Where((BuffEffectInfo<StatusEffectDurationAmplifierEffect> e) => (e.BuffEffect.TargetStatusEffect & effectTag) == effectTag).Sum((BuffEffectInfo<StatusEffectDurationAmplifierEffect> e) => e.BuffEffect.Percent);
				if (Mathf.Abs(num) > 0.001f)
				{
					duration *= num / 100f + 1f;
					if (duration < 0f)
					{
						duration = 0f;
					}
				}
			}
		}
		UnitCc unitCc;
		if (!CrowdControls.TryGetValue(type, out var value))
		{
			unitCc = new UnitCc
			{
				CcType = type,
				EndTime = timeNow + duration
			};
		}
		else
		{
			unitCc = value.Clone();
			if (unitCc.EndTime <= timeNow + duration)
			{
				unitCc.EndTime = timeNow + duration;
			}
		}
		CrowdControls[type] = unitCc;
	}

	public void RemoveCrowdControl(ECrowdControlType type)
	{
		if (CrowdControls.ContainsKey(type))
		{
			CrowdControls.Remove(type);
		}
	}

	public void ReduceCrowdControlDuration(ECrowdControlType type, float percent)
	{
		if (percent <= 0f || !CrowdControls.TryGetValue(type, out var value))
		{
			return;
		}
		float timeNow = GameManager.TimeNow;
		float num = value.EndTime - timeNow;
		if (!(num <= 0f))
		{
			float num2 = Mathf.Max(0f, 1f - percent / 100f);
			UnitCc unitCc = value.Clone();
			unitCc.EndTime = num2 * num + timeNow;
			CrowdControls[type] = unitCc;
		}
	}

	public void UpdateCrowdControl()
	{
		CrowdControls.Keys.Where((ECrowdControlType ccType) => CrowdControls[ccType].EndTime <= GameManager.TimeNow).ToList().ForEach(delegate(ECrowdControlType ccType)
		{
			if (CrowdControls.ContainsKey(ccType))
			{
				CrowdControls.Remove(ccType);
			}
		});
	}

	public ECrowdControlType GetImmunizedCcType(ECrowdControlType ccType)
	{
		EStatusEffectTag effectTag = ccType.GetEffectTag();
		if (!HasImmunityEffect(effectTag))
		{
			return ccType;
		}
		return ECrowdControlType.None;
	}

	public bool IsCrowdControlled(ECrowdControlType type)
	{
		return CrowdControls.ContainsKey(type);
	}

	public float GetRemainConsumableTypeCoolTime(EConsumableType consumableType)
	{
		float timeNow = GameManager.TimeNow;
		if (ConsumableTypeCoolTimes.TryGetValue(consumableType, out var value))
		{
			float num = value.EndTime - timeNow;
			if (0f < num)
			{
				return num;
			}
		}
		return 0f;
	}

	public void ApplyConsumableTypeCoolTime(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		float timeNow = GameManager.TimeNow;
		foreach (UnitItemCoolTime item in ConsumableTypeCoolTimes.Values.ToList())
		{
			float num = item.EndTime;
			if (item.ConsumableType == tItem.ConsumableType)
			{
				float num2 = tItem.Cooltime;
				float floatValue = TConst.ItemCooltime.FloatValue;
				if (num2 <= floatValue)
				{
					num2 = floatValue;
				}
				if (num <= num2 + timeNow)
				{
					num = num2 + timeNow;
				}
			}
			else
			{
				float num3 = TConst.ItemCooltime.FloatValue + timeNow;
				if (num <= num3)
				{
					num = num3;
				}
			}
			if (Mathf.Abs(num - item.EndTime) > Mathf.Epsilon)
			{
				UnitItemCoolTime unitItemCoolTime = item.Clone();
				unitItemCoolTime.EndTime = num;
				ConsumableTypeCoolTimes[unitItemCoolTime.ConsumableType] = unitItemCoolTime;
			}
		}
	}

	public int GetSkillLevel(int skillId)
	{
		if (SkillLevels == null)
		{
			return 0;
		}
		return SkillLevels.GetValueOrDefault(skillId);
	}

	public virtual bool HasTalent(int talentId)
	{
		return false;
	}

	protected virtual void InitLists()
	{
	}

	protected virtual void RegisterListEvents()
	{
	}

	protected virtual void UnregisterListEvents()
	{
	}

	protected virtual void InitVariables(EActionState state, Vector3 moveDir, Vector3 calculatedDeltaAimPos, Vector3 deltaAimPos, Vector3 forward, int hp, float stress, int corruption, float stamina, int soul, int usingSkillId, Vector3 position, int shield, float interpolationSpeed)
	{
		rxState.Value = state;
		rxMoveDir.Value = moveDir;
		rxCalculatedDeltaAimPos.Value = calculatedDeltaAimPos;
		rxDeltaAimPos.Value = deltaAimPos;
		rxForward.Value = forward;
		rxHp.Value = hp;
		rxStress.Value = stress;
		rxCorruption.Value = corruption;
		rxStamina.Value = stamina;
		rxSoul.Value = soul;
		rxUsingSkillId.Value = usingSkillId;
		rxPosition.Value = position;
		rxShield.Value = shield;
		rxInterpolationSpeed.Value = interpolationSpeed;
		if (base.NetworkManager.IsHost)
		{
			syncState.Value = rxState.Value;
			syncMoveDir.Value = new PackedDirMag(rxMoveDir.Value);
			syncCalculatedDeltaAimPos.Value = new QuantAimDelta(rxCalculatedDeltaAimPos.Value);
			syncDeltaAimPos.Value = new QuantAimDelta(rxDeltaAimPos.Value);
			syncForward.Value = new PackedDir(rxForward.Value);
			syncHp.Value = rxHp.Value;
			syncStress.Value = rxStress.Value;
			syncCorruption.Value = rxCorruption.Value;
			syncStamina.Value = rxStamina.Value;
			syncSoul.Value = rxSoul.Value;
			syncUsingSkillId.Value = rxUsingSkillId.Value;
			syncPosition.Value = new PlanarPos(rxPosition.Value);
			syncShield.Value = rxShield.Value;
			syncInterpolationSpeed.Value = new QuantSpeed(rxInterpolationSpeed.Value);
		}
	}

	protected virtual void RegisterVariableEvents()
	{
		RegisterVariableEvent(syncState, rxState);
		RegisterVariableEvent(syncMoveDir, rxMoveDir, (PackedDirMag v) => v.ToVector3());
		RegisterVariableEvent(syncCalculatedDeltaAimPos, rxCalculatedDeltaAimPos, (QuantAimDelta v) => v.ToVector3());
		RegisterVariableEvent(syncDeltaAimPos, rxDeltaAimPos, (QuantAimDelta v) => v.ToVector3());
		RegisterVariableEvent(syncForward, rxForward, (PackedDir v) => v.ToVector3());
		RegisterVariableEvent(syncHp, rxHp);
		RegisterVariableEvent(syncStress, rxStress);
		RegisterVariableEvent(syncCorruption, rxCorruption);
		RegisterVariableEvent(syncStamina, rxStamina);
		RegisterVariableEvent(syncSoul, rxSoul);
		RegisterVariableEvent(syncUsingSkillId, rxUsingSkillId);
		RegisterVariableEvent(syncPosition, rxPosition, (PlanarPos v) => v.ToVector3());
		RegisterVariableEvent(syncShield, rxShield);
		RegisterVariableEvent(syncInterpolationSpeed, rxInterpolationSpeed, (QuantSpeed v) => v.ToFloat());
	}

	protected virtual void UnregisterVariableEvents()
	{
		UnregisterVariableEvent(syncState);
		UnregisterVariableEvent(syncMoveDir);
		UnregisterVariableEvent(syncCalculatedDeltaAimPos);
		UnregisterVariableEvent(syncDeltaAimPos);
		UnregisterVariableEvent(syncForward);
		UnregisterVariableEvent(syncHp);
		UnregisterVariableEvent(syncStress);
		UnregisterVariableEvent(syncCorruption);
		UnregisterVariableEvent(syncStamina);
		UnregisterVariableEvent(syncSoul);
		UnregisterVariableEvent(syncUsingSkillId);
		UnregisterVariableEvent(syncPosition);
		UnregisterVariableEvent(syncShield);
		UnregisterVariableEvent(syncInterpolationSpeed);
	}

	public int AbsorbDamageWithShields(int amount)
	{
		AssertIfNotHost();
		if (0 < amount && Shields.Count != 0)
		{
			List<((ulong casterId, int shieldId), UnitShield, TShield)> list = (from kvp in Shields
				select (kvp.Key, kvp.Value, TShield.Table[kvp.Value.Id])).OrderByDescending((((ulong casterId, int shieldId), UnitShield, TShield) _) => _.Item3.IsDecrease).ThenBy((((ulong casterId, int shieldId), UnitShield, TShield) _) => (!(_.Item2.EndTime <= 0f)) ? _.Item2.EndTime : float.MaxValue).ToList();
			foreach (var item in list)
			{
				if (amount < 1)
				{
					break;
				}
				UnitShield unitShield = item.Item2.Clone();
				if (0 < unitShield.Value)
				{
					int num = amount;
					if (unitShield.Value < amount)
					{
						num = unitShield.Value;
					}
					unitShield.Value -= num;
					amount -= num;
				}
				if (unitShield.Value < 1)
				{
					Shields.Remove(item.Item1);
				}
				else
				{
					Shields[item.Item1] = unitShield;
				}
			}
			IsShieldDirty = true;
		}
		return amount;
	}

	public void AddShield(int shieldId, int amount, ulong casterId = 0uL, float? overrideDuration = null)
	{
		AssertIfNotHost();
		if (IsInactiveInStage || shieldId < 1 || amount < 1)
		{
			return;
		}
		(ulong, int) key = (casterId, shieldId);
		TShield tShield = TShield.Table[shieldId];
		float timeNow = GameManager.TimeNow;
		float num = overrideDuration ?? tShield.Duration;
		float num2 = ((num > 0f) ? (num + timeNow) : (-1f));
		float num3 = ((tShield.MaxValueType != EShieldMaxValueType.PercentOfMaxHp) ? tShield.MaxValue : (GetStat(EStatType.HpMax) * tShield.MaxValue / 100f));
		float num4 = 0f;
		if (0f <= num3)
		{
			num4 = num3;
		}
		int num5 = Mathf.CeilToInt(num4);
		if (!Shields.TryGetValue(key, out var value))
		{
			if (amount < num5)
			{
				num5 = amount;
			}
			Shields[key] = new UnitShield
			{
				StartTime = timeNow,
				EndTime = num2,
				Id = shieldId,
				CasterId = casterId,
				Value = num5,
				OriginalValue = num5
			};
			IsShieldDirty = true;
			return;
		}
		UnitShield unitShield = value.Clone();
		if (num5 < unitShield.Value)
		{
			unitShield.Value = num5;
		}
		unitShield.StartTime = timeNow;
		if (!tShield.IsStackable)
		{
			if (amount < num5)
			{
				num5 = amount;
			}
		}
		else
		{
			int num6 = unitShield.Value + amount;
			if (num6 < num5)
			{
				num5 = num6;
			}
		}
		unitShield.Value = num5;
		unitShield.OriginalValue = unitShield.Value;
		float num7 = -1f;
		if (unitShield.EndTime >= 0f && num2 >= 0f)
		{
			num7 = unitShield.EndTime;
			if (num7 <= num2)
			{
				num7 = num2;
			}
		}
		unitShield.EndTime = num7;
		Shields[key] = unitShield;
		IsShieldDirty = true;
	}

	public void RemoveShield(int shieldId, ulong casterId = 0uL, int amount = int.MaxValue)
	{
		AssertIfNotHost();
		if (shieldId < 1)
		{
			return;
		}
		(ulong, int) key = (casterId, shieldId);
		if (!Shields.TryGetValue(key, out var value))
		{
			return;
		}
		if (0 < amount && amount < value.Value)
		{
			UnitShield unitShield = value.Clone();
			unitShield.Value = Math.Max(0, unitShield.Value - amount);
			Shields[key] = unitShield;
			IsShieldDirty = true;
			return;
		}
		Shields.Remove(key);
		IsShieldDirty = true;
	}

	public void UpdateShields()
	{
		AssertIfNotHost();
		float timeNow = GameManager.TimeNow;
		List<(ulong, int)> list = new List<(ulong, int)>();
		foreach (KeyValuePair<(ulong casterId, int shieldId), UnitShield> shield in Shields)
		{
			UnitShield value = shield.Value;
			if (!(value.EndTime >= 0f) || value.OriginalValue <= 0)
			{
				continue;
			}
			TShield tShield = TShield.Table[value.Id];
			float num = value.EndTime - value.StartTime;
			float num2 = value.EndTime - timeNow;
			if (num2 <= 0f)
			{
				list.Add(shield.Key);
				IsShieldDirty = true;
			}
			else if (tShield.IsDecrease)
			{
				if (num <= 0f || (num2 /= num) < 0f)
				{
					num2 = 0f;
				}
				else if (1f < num2)
				{
					num2 = 1f;
				}
				int num3 = Mathf.CeilToInt((float)value.OriginalValue * num2);
				if (num3 < value.Value)
				{
					value.Value = num3;
					IsShieldDirty = true;
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (var item in list)
			{
				Shields.Remove(item);
			}
		}
		if (IsShieldDirty)
		{
			syncShield.Value = Shields.Values.Sum((UnitShield s) => s.Value);
		}
	}

	public List<int> GetActiveSkillIds()
	{
		if (ActiveSkillIds == null)
		{
			return new List<int>();
		}
		if (_changeSkillOverrides.Count == 0)
		{
			return ActiveSkillIds;
		}
		List<int> list = ActiveSkillIds.ToList();
		foreach (KeyValuePair<int, int> changeSkillOverride in _changeSkillOverrides)
		{
			int num = list.IndexOf(changeSkillOverride.Key);
			if (num > -1)
			{
				list[num] = changeSkillOverride.Value;
			}
		}
		return list;
	}

	public float GetChangeSkillBuffEndTime(int skillId)
	{
		if (ActiveSkillIds != null && ActiveSkillIds.Contains(skillId))
		{
			return -1f;
		}
		foreach (UnitBuff value in Buffs.Values)
		{
			if (!value.IsActive)
			{
				continue;
			}
			TBuff tBuff = TBuff.Table[value.Id];
			if (!(tBuff.Duration >= 0f) || tBuff.BuffEffects == null)
			{
				continue;
			}
			BuffEffect[] buffEffects = tBuff.BuffEffects;
			foreach (BuffEffect buffEffect in buffEffects)
			{
				if (buffEffect is ChangeSkillEffect changeSkillEffect && changeSkillEffect.ReplacementSkillId == skillId)
				{
					return value.EndTime;
				}
			}
		}
		return -1f;
	}

	public void ApplyChangeSkillOverride(int originalSkillId, int replacementSkillId)
	{
		_changeSkillOverrides[originalSkillId] = replacementSkillId;
		OnChangeSkillOverridesChanged();
	}

	public void RemoveChangeSkillOverride(int originalSkillId, int replacementSkillId)
	{
		if (_changeSkillOverrides.TryGetValue(originalSkillId, out var value) && value == replacementSkillId)
		{
			_changeSkillOverrides.Remove(originalSkillId);
			OnChangeSkillOverridesChanged();
		}
	}

	private void OnChangeSkillOverridesChanged()
	{
		List<int> activeSkillIds = GetActiveSkillIds();
		if (base.NetworkManager.IsHost)
		{
			activeSkillIds.Where((int _) => _ >= 1 && !trackableSkills.ContainsKey(_)).ToList().ForEach(delegate(int _)
			{
				trackableSkills.Add(_, new UnitSkill(_));
			});
		}
		OnActiveSkillsChanged.OnNext(activeSkillIds.ToArray());
	}

	public void UpdateSkillStacks()
	{
		GetActiveSkillIds()?.ForEach(UpdateSkillStack);
		PassiveSkillIds?.ForEach(UpdateSkillStack);
		void UpdateSkillStack(int skillId)
		{
			if (skillId < 1)
			{
				return;
			}
			TSkill tSkill = TSkill.Table[skillId];
			float timeNow = GameManager.TimeNow;
			if (Skills.TryGetValue(skillId, out var value) && value.IsActiveNow && value.Stack < tSkill.StackCount && !(timeNow < value.StackChargeTime) && !(tSkill.StackChargeInterval <= 0f))
			{
				UnitSkill unitSkill = value.Clone();
				unitSkill.Stack++;
				unitSkill.StackChargeTime = ((tSkill.StackChargeInterval >= 0f || value.Stack < tSkill.StackCount) ? (GetReducedStackChargeInterval(tSkill) + timeNow) : (-1f));
				Skills[skillId] = unitSkill;
			}
		}
	}

	public void ConsumeSkillStack(int skillId)
	{
		UnitSkill unitSkill = ((!Skills.TryGetValue(skillId, out var value)) ? new UnitSkill(skillId) : value.Clone());
		if (!unitSkill.IsActiveNow)
		{
			return;
		}
		if (0 < unitSkill.Stack)
		{
			TSkill tSkill = TSkill.Table[skillId];
			if (0f <= unitSkill.StackChargeTime && tSkill.StackCount <= unitSkill.Stack)
			{
				unitSkill.StackChargeTime = GetReducedStackChargeInterval(tSkill) + GameManager.TimeNow;
			}
			unitSkill.Stack--;
			Skills[skillId] = unitSkill;
		}
		else
		{
			Logger.Error(string.Format("Skill Stack is not enough. SkillId: {0}", skillId));
		}
	}

	public void StartSkillCoolDown(int skillId)
	{
		TSkill tSkill = TSkill.Table[skillId];
		float timeNow = GameManager.TimeNow;
		float num = GetStat(EStatType.CooldownReductionPercent);
		float floatValue = TConst.MaxCooldownReductionPercent.FloatValue;
		if (floatValue <= num)
		{
			num = floatValue;
		}
		float num2 = (100f - num) / 100f;
		if (tSkill.IsBasicAttack)
		{
			num2 *= 1f / GetStat(EStatType.AtkSpeed);
		}
		float coolTime = tSkill.CoolTime;
		float floatValue2 = TConst.MinCoolTime.FloatValue;
		coolTime *= num2;
		if (coolTime <= floatValue2)
		{
			coolTime = floatValue2;
		}
		UnitSkill unitSkill = ((!Skills.TryGetValue(skillId, out var value)) ? new UnitSkill(skillId) : value.Clone());
		unitSkill.CoolEndTime = coolTime + timeNow;
		Skills[skillId] = unitSkill;
	}

	public void StartGlobalSkillCooldown()
	{
		if (GlobalSkillCoolTime <= 0f || !Skills.Any())
		{
			return;
		}
		float endTime = GlobalSkillCoolTime + GameManager.TimeNow;
		Skills.Values.Where((UnitSkill _) => _.CoolEndTime < endTime).Where((UnitSkill _) => PassiveSkillIds == null || !PassiveSkillIds.Contains(_.Id)).Select((UnitSkill _) => _.Clone())
			.ToList()
			.ForEach(delegate(UnitSkill _)
			{
				_.CoolEndTime = endTime;
				Skills[_.Id] = _;
			});
	}

	public float GetRemainCoolTime(int skillId)
	{
		float timeNow = GameManager.TimeNow;
		float num = ((!Skills.TryGetValue(skillId, out var value)) ? 0f : value.CoolEndTime);
		num -= timeNow;
		if (num <= 0f)
		{
			num = 0f;
		}
		return num;
	}

	public float GetSkillChargeAmount(int skillId)
	{
		if (TSkill.Table[skillId].ChargeCost < 1)
		{
			return -1f;
		}
		if (!Skills.TryGetValue(skillId, out var value))
		{
			return 0f;
		}
		return value.ChargeAmount;
	}

	public void AdjustSkillCosts(TSkill skill, ref int hpCost, ref int stressCost, ref int staminaCost)
	{
		IList<ESkillTagType> tags = skill.SkillTags;
		if (tags == null || skill.SkillTags.Length == 0)
		{
			return;
		}
		float num = 1f;
		float num2 = 1f;
		float num3 = 1f;
		IList<BuffEffectInfo<SkillHpCostAmplifierEffect>> buffEffects = GetBuffEffects<SkillHpCostAmplifierEffect>();
		if (buffEffects.Any())
		{
			num2 = buffEffects.Where((BuffEffectInfo<SkillHpCostAmplifierEffect> e) => Intersects(tags, e.BuffEffect.SkillTags)).Sum((BuffEffectInfo<SkillHpCostAmplifierEffect> e) => (float)e.Stack * e.BuffEffect.Percent) / 100f + 1f;
		}
		IList<BuffEffectInfo<SkillStressCostAmplifierEffect>> buffEffects2 = GetBuffEffects<SkillStressCostAmplifierEffect>();
		if (buffEffects2.Any())
		{
			num3 = buffEffects2.Where((BuffEffectInfo<SkillStressCostAmplifierEffect> e) => Intersects(tags, e.BuffEffect.SkillTags)).Sum((BuffEffectInfo<SkillStressCostAmplifierEffect> e) => (float)e.Stack * e.BuffEffect.Percent) / 100f + 1f;
		}
		IList<BuffEffectInfo<SkillStaminaCostAmplifierEffect>> buffEffects3 = GetBuffEffects<SkillStaminaCostAmplifierEffect>();
		if (buffEffects3.Any())
		{
			num = buffEffects3.Where((BuffEffectInfo<SkillStaminaCostAmplifierEffect> e) => Intersects(tags, e.BuffEffect.SkillTags)).Sum((BuffEffectInfo<SkillStaminaCostAmplifierEffect> e) => (float)e.Stack * e.BuffEffect.Percent) / 100f + 1f;
		}
		float num4 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
		float num5 = Mathf.Clamp(num3, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
		float num6 = Mathf.Clamp(num, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
		if (0 < hpCost)
		{
			int num7 = Mathf.RoundToInt((float)hpCost * num4);
			if (num7 < 0)
			{
				num7 = 0;
			}
			hpCost = num7;
		}
		if (0 < stressCost)
		{
			int num8 = Mathf.RoundToInt((float)stressCost * num5);
			if (num8 < 0)
			{
				num8 = 0;
			}
			stressCost = num8;
		}
		if (0 < staminaCost)
		{
			int num9 = Mathf.RoundToInt((float)staminaCost * num6);
			if (num9 < 0)
			{
				num9 = 0;
			}
			staminaCost = num9;
		}
		static bool Intersects(IList<ESkillTagType> a, ESkillTagType[] b)
		{
			if (b == null || b.Length == 0)
			{
				return false;
			}
			return b.Any((ESkillTagType tag) => a.Contains(tag));
		}
	}

	public virtual bool CheckSkill(int skillId)
	{
		TSkill tSkill = TSkill.Table[skillId];
		if (Skills.ContainsKey(skillId) && Skills[skillId].Stack < 1)
		{
			return false;
		}
		int hpCost = tSkill.HpCost;
		int stressCost = tSkill.StressCost;
		int staminaCost = tSkill.StaminaCost;
		AdjustSkillCosts(tSkill, ref hpCost, ref stressCost, ref staminaCost);
		if (0 < hpCost && rxHp.Value <= hpCost)
		{
			return false;
		}
		if (0 < staminaCost && rxStamina.Value <= (float)staminaCost)
		{
			return false;
		}
		float timeNow = GameManager.TimeNow;
		float num = ((!Skills.TryGetValue(skillId, out var value)) ? 0f : value.CoolEndTime);
		if (0f < num - timeNow)
		{
			return false;
		}
		if (tSkill.SkillType == ESkillType.Active && HasBuffEffect(EBuffEffectType.Silence) && !tSkill.IsBasicAttack)
		{
			return false;
		}
		if (HasBuffEffect(EBuffEffectType.Root) && tSkill.RootActionNode.skillAction.isDisabledWhenSnared)
		{
			return false;
		}
		if (0 < tSkill.ChargeCost)
		{
			if (!Skills.TryGetValue(skillId, out var value2))
			{
				return false;
			}
			if (value2.ChargeAmount < (float)tSkill.ChargeCost)
			{
				return false;
			}
		}
		if (tSkill.RequireWeapon && WeaponId < 1)
		{
			return false;
		}
		if (tSkill.DeactivateBuffCondition != null && 0 < tSkill.DeactivateBuffCondition.BuffId)
		{
			int buffStackCount = GetBuffStackCount(tSkill.DeactivateBuffCondition.BuffId);
			if (ValueComparer.Compare(buffStackCount, tSkill.DeactivateBuffCondition.CompareType, tSkill.DeactivateBuffCondition.StackCount))
			{
				return false;
			}
		}
		return true;
	}

	public void ConsumeSkillCharge(int skillId)
	{
		TSkill tSkill = TSkill.Table[skillId];
		if (tSkill.ChargeCost < 1)
		{
			return;
		}
		UnitSkill unitSkill = ((!Skills.TryGetValue(skillId, out var value)) ? new UnitSkill(skillId) : value.Clone());
		if (unitSkill.IsActiveNow && !(unitSkill.ChargeAmount < (float)tSkill.ChargeCost))
		{
			unitSkill.ChargeAmount -= tSkill.ChargeCost;
			if (unitSkill.ChargeAmount < 0f)
			{
				unitSkill.ChargeAmount = 0f;
			}
			Skills[skillId] = unitSkill;
		}
	}

	private float GetReducedStackChargeInterval(TSkill skillData)
	{
		float num = GetStat(EStatType.CooldownReductionPercent);
		float floatValue = TConst.MaxCooldownReductionPercent.FloatValue;
		if (floatValue <= num)
		{
			num = floatValue;
		}
		float num2 = (100f - num) / 100f * skillData.StackChargeInterval;
		float floatValue2 = TConst.MinCoolTime.FloatValue;
		if (num2 <= floatValue2)
		{
			num2 = floatValue2;
		}
		return num2;
	}

	public bool CheckSkillActionCost(SkillActionNode node, TSkill skill)
	{
		if (node.skillAction.IsUsingCost && !((float)skill.StressCost + rxStress.Value <= (float)TConst.StressMax.IntValue) && !(skill.HpCost < rxHp.Value) && !((float)skill.StaminaCost < rxStamina.Value))
		{
			return false;
		}
		if (node.skillAction.IsUsingStack && Skills.ContainsKey(skill.Id) && Skills[skill.Id].Stack < 1)
		{
			return false;
		}
		return true;
	}

	public bool IsInCoolTime(int skillId)
	{
		float timeNow = GameManager.TimeNow;
		float num = ((!Skills.TryGetValue(skillId, out var value)) ? 0f : value.CoolEndTime);
		return 0f < num - timeNow;
	}

	public void AddSkillCharge(int skillId, float addAmount)
	{
		TSkill tSkill = TSkill.Table[skillId];
		if (GetBuffEffects<BlockSkillChargeEffect>().Any((BuffEffectInfo<BlockSkillChargeEffect> be) => be.BuffEffect.SkillIds != null && be.BuffEffect.SkillIds.Contains(skillId)))
		{
			return;
		}
		UnitSkill unitSkill = ((!Skills.TryGetValue(skillId, out var value)) ? new UnitSkill(skillId) : value.Clone());
		if (unitSkill.IsActiveNow)
		{
			float num = addAmount + unitSkill.ChargeAmount;
			float num2 = tSkill.ChargeCost;
			if (num2 <= num)
			{
				num = num2;
			}
			unitSkill.ChargeAmount = num;
			Skills[skillId] = unitSkill;
		}
	}

	public void OnEnterSpecialField(ulong networkObjectId)
	{
		SpecialField specialField = ObjectManager.FindSpecialField(networkObjectId);
		if (specialField == null)
		{
			Logger.Error(string.Format("SpecialField not found. NetworkObjectId: {0}", networkObjectId));
			return;
		}
		if (!_enteredSpecialFields.TryAdd(networkObjectId, specialField))
		{
			return;
		}
		Logger.Debug(string.Format("{0}", _enteredSpecialFields.Count));
		int buffId = specialField.GetBuffId(this);
		if (0 < buffId)
		{
			AddBuff(buffId, specialField, 1, 0uL, specialField.Position);
		}
		float stackBuffInterval = specialField.GetStackBuffInterval(this);
		if (0f < stackBuffInterval)
		{
			_stackBuffNextFireTimes[networkObjectId] = GameManager.TimeNow + stackBuffInterval;
		}
		if (specialField.GetPreventStackBuffExpire(this))
		{
			int stackBuffId = specialField.GetStackBuffId(this);
			if (stackBuffId >= 1)
			{
				_expirePreventedStackBuffRefCounts.TryGetValue(stackBuffId, out var value);
				_expirePreventedStackBuffRefCounts[stackBuffId] = value + 1;
			}
		}
	}

	public void OnExitSpecialField(ulong networkObjectId)
	{
		if (!_enteredSpecialFields.Remove(networkObjectId, out var value))
		{
			Logger.Error(string.Format("SpecialField not found. NetworkObjectId: {0}", networkObjectId));
			return;
		}
		_stackBuffNextFireTimes.Remove(networkObjectId);
		int buffId = value.GetBuffId(this);
		if (0 < buffId)
		{
			int stack = _enteredSpecialFields.Values.Count((SpecialField _) => _.GetBuffId(this) == buffId);
			SetBuff(buffId, value, stack, 0uL, Vector3.zero);
		}
		if (value.GetPreventStackBuffExpire(this))
		{
			int stackBuffId = value.GetStackBuffId(this);
			if (_expirePreventedStackBuffRefCounts.TryGetValue(stackBuffId, out var value2))
			{
				if (value2 < 2)
				{
					_expirePreventedStackBuffRefCounts.Remove(stackBuffId);
				}
				else
				{
					_expirePreventedStackBuffRefCounts[stackBuffId] = value2 - 1;
				}
			}
		}
	}

	public void UpdateSpecialFieldsOnRevive()
	{
		foreach (BuffEffectInfo<EscapeOnActiveEffect> buffEffect in GetBuffEffects<EscapeOnActiveEffect>(checkIsActive: false))
		{
			RemoveBuff(buffEffect.BuffId, int.MaxValue);
		}
		UnitController component = GetComponent<UnitController>();
		foreach (SpecialField specialField in ObjectManager.SpecialFields)
		{
			specialField.RescanUnit(component);
		}
	}

	public void UpdateSpecialFields()
	{
		_enteredSpecialFields.Values.GroupBy((SpecialField _) => _.GetBuffId(this)).Where((IGrouping<int, SpecialField> _) => _.Key > 0).ForEach(delegate(IGrouping<int, SpecialField> _)
		{
			int key = _.Key;
			SpecialField actionStats = _.First();
			int stack = _.Count();
			SetBuff(key, actionStats, stack, 0uL, Vector3.zero);
		});
	}

	public void UpdateStackBuffTimers()
	{
		float timeNow = GameManager.TimeNow;
		foreach (ulong item in _stackBuffNextFireTimes.Keys.ToList())
		{
			if (!_enteredSpecialFields.TryGetValue(item, out var value))
			{
				continue;
			}
			float stackBuffInterval = value.GetStackBuffInterval(this);
			if (stackBuffInterval <= 0f)
			{
				continue;
			}
			float num = _stackBuffNextFireTimes[item];
			while (num <= timeNow)
			{
				num += stackBuffInterval;
				int stackBuffId = value.GetStackBuffId(this);
				if (0 < stackBuffId)
				{
					AddBuff(stackBuffId, value, 1, 0uL, value.Position);
				}
			}
			_stackBuffNextFireTimes[item] = num;
		}
	}

	public List<T> GetEnteredSpecialFields<T>() where T : SpecialField
	{
		if (_enteredSpecialFields.Any())
		{
			return _enteredSpecialFields.Values.OfType<T>().ToList();
		}
		return new List<T>();
	}

	public virtual float GetBaseStat(EStatType statType)
	{
		if (!BaseStats.TryGetValue(statType, out var value))
		{
			return 0f;
		}
		return value;
	}

	public virtual float GetAdditionalStat(EStatType statType)
	{
		return 0f;
	}

	public float GetStat(EStatType statType)
	{
		if (!Stats.TryGetValue(statType, out var value))
		{
			return 0f;
		}
		return value.Value;
	}

	public void SetStat(EStatType statType, float value)
	{
		AssertIfNotHost();
		Stats[statType] = new UnitStat
		{
			Value = value,
			StatType = statType
		};
	}

	public bool HasBuff(int buffId)
	{
		return Buffs.ContainsKey(buffId);
	}

	public EStatusEffectTag GetBuffStatusEffectTags()
	{
		return Buffs.Values.Where((UnitBuff _) => _.IsActive).Select((UnitBuff _) => TBuff.Table[_.Id].EffectTag).Where((EStatusEffectTag tag) => tag != EStatusEffectTag.None)
			.Aggregate(EStatusEffectTag.None, (EStatusEffectTag acc, EStatusEffectTag tag) => acc | tag);
	}

	public EStatusEffectTag GetCcStatusEffectTags()
	{
		EStatusEffectTag eStatusEffectTag = EStatusEffectTag.None;
		foreach (ECrowdControlType value in Enum.GetValues(typeof(ECrowdControlType)))
		{
			if (value != ECrowdControlType.None && IsCrowdControlled(value))
			{
				eStatusEffectTag |= value.GetEffectTag();
			}
		}
		return eStatusEffectTag;
	}

	public EStatusEffectTag GetStatusEffectTags()
	{
		return GetBuffStatusEffectTags() | GetCcStatusEffectTags();
	}

	public float GetStatusEffectTagEndTime(EStatusEffectTag tag)
	{
		ECrowdControlType ccType = tag.GetCcType();
		if (ccType != ECrowdControlType.None && IsCrowdControlled(ccType))
		{
			return CrowdControls[ccType].EndTime;
		}
		IEnumerable<UnitBuff> source = Buffs.Values.Where((UnitBuff _) => TBuff.Table[_.Id].EffectTag == tag);
		if (source.Any((UnitBuff _) => _.EndTime <= 0f))
		{
			return -1f;
		}
		return source.Select((UnitBuff _) => _.EndTime).DefaultIfEmpty(-1f).Max();
	}

	public bool HasStatusEffectTag(EStatusEffectTag effectTag)
	{
		return ((GetBuffStatusEffectTags() | GetCcStatusEffectTags()) & effectTag) == effectTag;
	}

	public bool HasBuffEffect(EBuffEffectType effectType)
	{
		return Buffs.Values.Where((UnitBuff _) => TBuff.Table[_.Id].BuffEffects != null).Any((UnitBuff _) => TBuff.Table[_.Id].BuffEffects.Where((BuffEffect effect, int index) => effect.EffectType == effectType && _.StackCount > 0).Any());
	}

	public IList<BuffEffectInfo<T>> GetBuffEffects<T>(bool checkIsActive = true) where T : BuffEffect
	{
		List<BuffEffectInfo<T>> list = new List<BuffEffectInfo<T>>();
		foreach (UnitBuff value in Buffs.Values)
		{
			if (TBuff.Table[value.Id].BuffEffects == null || (checkIsActive && !value.IsEffectActive))
			{
				continue;
			}
			for (int i = 0; i < TBuff.Table[value.Id].BuffEffects.Length; i++)
			{
				if (TBuff.Table[value.Id].BuffEffects[i] is T val && value.StackCount > 0)
				{
					list.Add(new BuffEffectInfo<T>
					{
						BuffEffect = val,
						BuffId = value.Id,
						Stack = value.StackCount,
						CasterIds = value.CasterIds,
						Point = value.Point.ToVector3()
					});
				}
			}
		}
		return list;
	}

	public bool IsDamageBlockedByInvincibility(bool isCritical = false, bool isBackAttack = false)
	{
		List<BuffEffectInfo<InvincibilityEffect>> list = GetBuffEffects<InvincibilityEffect>().Where((BuffEffectInfo<InvincibilityEffect> e) => e.BuffEffect.BlockDamage).ToList();
		if (!list.Any())
		{
			return false;
		}
		if (isCritical && list.All((BuffEffectInfo<InvincibilityEffect> e) => !e.BuffEffect.IgnoreCriticalHit))
		{
			return false;
		}
		if (isBackAttack && list.All((BuffEffectInfo<InvincibilityEffect> e) => !e.BuffEffect.IgnoreBackAttack))
		{
			return false;
		}
		return true;
	}

	public void Synchronize()
	{
		AssertIfNotHost();
		FlushTrackableDatas();
	}

	private void FlushTrackableDatas(bool syncToClients = true, HashSet<ulong> excludeIds = null)
	{
		AssertIfNotHost();
		if (!trackableSkills.HasChange && !trackableBuffs.HasChange && !trackableConsumableTypeCoolTimes.HasChange && !trackableStats.HasChange && !trackableCrowdControls.HasChange)
		{
			return;
		}
		UnitSkill[] array = trackableSkills.Flush();
		UnitBuff[] array2 = trackableBuffs.Flush();
		UnitItemCoolTime[] array3 = trackableConsumableTypeCoolTimes.Flush();
		UnitStat[] array4 = trackableStats.Flush();
		UnitCc[] array5 = trackableCrowdControls.Flush();
		ApplySkills(array);
		ApplyBuffs(array2);
		ApplyConsumableTypeCoolTimes(array3);
		ApplyStats(array4);
		ApplyCrowdControls(array5);
		if (syncToClients)
		{
			GameHost.SendToAllExceptHost(new NtfUpdateUnitModel
			{
				Tick = (int)base.NetworkManager.ServerTime.Tick,
				NetworkObjectId = base.NetworkObjectId,
				ChangeSkills = array,
				ChangeBuffs = array2,
				ChangeConsumableTypeCoolTimes = array3,
				ChangeStats = array4,
				ChangeCrowdControls = array5
			}, excludeIds);
		}
	}

	private void ApplySkills(UnitSkill[] skills)
	{
		skills?.ForEach(delegate(UnitSkill _)
		{
			if (_.Operation == EDataOperation.Remove)
			{
				RxSkills.Remove(_.Id);
			}
			if (_.Operation == EDataOperation.Add)
			{
				try
				{
					RxSkills.Add(_.Id, _);
				}
				catch (Exception arg)
				{
					RxSkills[_.Id] = _;
					Logger.Error(string.Format("Type:{0}, Id:{1} SkillId: {2}, Operation: {3}, Exception: {4}", UnitType, TableId, _.Id, _.Operation, arg));
				}
			}
			if (_.Operation == EDataOperation.Replace)
			{
				RxSkills[_.Id] = _;
			}
		});
	}

	private void ApplyBuffs(UnitBuff[] buffs)
	{
		buffs?.ForEach(delegate(UnitBuff _)
		{
			if (_.Operation == EDataOperation.Remove)
			{
				RxBuffs.Remove(_.Id);
			}
			if (_.Operation == EDataOperation.Add)
			{
				try
				{
					RxBuffs.Add(_.Id, _);
				}
				catch (Exception arg)
				{
					RxBuffs[_.Id] = _;
					Logger.Error(string.Format("Type:{0}, Id:{1} BuffId: {2}, Operation: {3}, Exception: {4}", UnitType, TableId, _.Id, _.Operation, arg));
				}
			}
			if (_.Operation == EDataOperation.Replace)
			{
				RxBuffs[_.Id] = _;
			}
		});
	}

	private void ApplyConsumableTypeCoolTimes(UnitItemCoolTime[] consumableTypeCoolTimes)
	{
		consumableTypeCoolTimes?.ForEach(delegate(UnitItemCoolTime _)
		{
			if (_.Operation == EDataOperation.Remove)
			{
				RxConsumableTypeCoolTimes.Remove(_.ConsumableType);
			}
			if (_.Operation == EDataOperation.Add)
			{
				try
				{
					RxConsumableTypeCoolTimes.Add(_.ConsumableType, _);
				}
				catch (Exception arg)
				{
					RxConsumableTypeCoolTimes[_.ConsumableType] = _;
					Logger.Error(string.Format("Type:{0}, Id:{1} ConsumableTypeCoolTime: {2}, Operation: {3}, Exception: {4}", UnitType, TableId, _.ConsumableType, _.Operation, arg));
				}
			}
			if (_.Operation == EDataOperation.Replace)
			{
				RxConsumableTypeCoolTimes[_.ConsumableType] = _;
			}
		});
	}

	private void ApplyStats(UnitStat[] stats)
	{
		stats?.ForEach(delegate(UnitStat _)
		{
			RxStats.TryGetValue(_.StatType, out var value);
			UnitStat newValue = new UnitStat
			{
				Operation = _.Operation,
				StatType = _.StatType,
				Value = _.Value
			};
			if (_.Operation == EDataOperation.Remove)
			{
				RxStats.Remove(_.StatType);
			}
			if (_.Operation == EDataOperation.Add)
			{
				try
				{
					RxStats.Add(_.StatType, _);
				}
				catch (Exception arg)
				{
					RxStats[_.StatType] = _;
					Logger.Error(string.Format("Type:{0}, Id:{1} Stat: {2}, Operation: {3}, Exception: {4}", UnitType, TableId, _.StatType, _.Operation, arg));
				}
			}
			if (_.Operation == EDataOperation.Replace)
			{
				RxStats[_.StatType] = _;
			}
			OnStatChanged.OnNext((_.StatType, value, newValue));
		});
	}

	private void ApplyCrowdControls(UnitCc[] crowdControls)
	{
		crowdControls?.ForEach(delegate(UnitCc _)
		{
			if (_.Operation == EDataOperation.Remove)
			{
				RxCrowdControls.Remove(_.CcType);
			}
			if (_.Operation == EDataOperation.Add)
			{
				try
				{
					RxCrowdControls.Add(_.CcType, _);
				}
				catch (Exception arg)
				{
					RxCrowdControls[_.CcType] = _;
					Logger.Error(string.Format("Type:{0}, Id:{1} cc: {2}, Operation: {3}, Exception: {4}", UnitType, TableId, _.CcType, _.Operation, arg));
				}
			}
			if (_.Operation == EDataOperation.Replace)
			{
				RxCrowdControls[_.CcType] = _;
			}
		});
	}

	private void OnReqSyncUnitModel(ReqSyncUnitModel message)
	{
		HashSet<ulong> hashSet = new HashSet<ulong>();
		hashSet.Add(message.SenderId);
		FlushTrackableDatas(syncToClients: true, hashSet);
		UnitSkill[] array = Skills.Values.Select((UnitSkill _) => _.Clone()).ToArray();
		UnitBuff[] array2 = Buffs.Values.Select((UnitBuff _) => _.Clone()).ToArray();
		UnitStat[] array3 = Stats.Values.Select((UnitStat _) => _.Clone()).ToArray();
		UnitItemCoolTime[] array4 = ConsumableTypeCoolTimes.Values.Select((UnitItemCoolTime _) => _.Clone()).ToArray();
		UnitCc[] array5 = CrowdControls.Values.Select((UnitCc _) => _.Clone()).ToArray();
		array.ForEach(delegate(UnitSkill _)
		{
			_.Operation = EDataOperation.Replace;
		});
		array2.ForEach(delegate(UnitBuff _)
		{
			_.Operation = EDataOperation.Replace;
		});
		array3.ForEach(delegate(UnitStat _)
		{
			_.Operation = EDataOperation.Replace;
		});
		array4.ForEach(delegate(UnitItemCoolTime _)
		{
			_.Operation = EDataOperation.Replace;
		});
		array5.ForEach(delegate(UnitCc _)
		{
			_.Operation = EDataOperation.Replace;
		});
		GameHost.SendToClient(message.SenderId, new ResSyncUnitModel
		{
			Tick = (int)base.NetworkManager.ServerTime.Tick,
			NetworkObjectId = message.NetworkObjectId,
			Buffs = array2,
			Skills = array,
			ItemTypeCoolTimes = array4,
			Stats = array3,
			Ccs = array5
		});
	}

	private void OnResSyncUnitModel(ResSyncUnitModel message)
	{
		RxSkills.Clear();
		RxBuffs.Clear();
		RxStats.Clear();
		RxConsumableTypeCoolTimes.Clear();
		RxCrowdControls.Clear();
		ApplySkills(message.Skills);
		ApplyBuffs(message.Buffs);
		ApplyStats(message.Stats);
		ApplyConsumableTypeCoolTimes(message.ItemTypeCoolTimes);
		ApplyCrowdControls(message.Ccs);
		_syncTick = message.Tick;
	}

	private void OnUpdateUnitModel(NtfUpdateUnitModel message)
	{
		if (0 < _syncTick && _syncTick < message.Tick)
		{
			ApplySkills(message.ChangeSkills);
			ApplyBuffs(message.ChangeBuffs);
			ApplyStats(message.ChangeStats);
			ApplyConsumableTypeCoolTimes(message.ChangeConsumableTypeCoolTimes);
			ApplyCrowdControls(message.ChangeCrowdControls);
		}
	}



}

using System;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
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

	private readonly Dictionary<object, Delegate> _listEventHandlers;

	private readonly Dictionary<(int buffId, ulong casterId), IDisposable> _realtimeStatSubs;

	private readonly HashSet<EStatType> _dirtyRealtimeStats;

	private bool _isConverging;

	protected bool IsShieldDirty;

	private NetworkVariable<EActionState> syncState;

	private NetworkVariable<PackedDirMag> syncMoveDir;

	private NetworkVariable<QuantAimDelta> syncCalculatedDeltaAimPos;

	private NetworkVariable<QuantAimDelta> syncDeltaAimPos;

	private NetworkVariable<int> syncHp;

	private NetworkVariable<PackedDir> syncForward;

	private NetworkVariable<float> syncStress;

	private NetworkVariable<int> syncCorruption;

	private NetworkVariable<float> syncStamina;

	private NetworkVariable<int> syncSoul;

	private NetworkVariable<int> syncUsingSkillId;

	private NetworkVariable<PlanarPos> syncPosition;

	private NetworkVariable<int> syncShield;

	private NetworkVariable<QuantSpeed> syncInterpolationSpeed;

	private ReactiveProperty<EActionState> rxState;

	private ReactiveProperty<Vector3> rxMoveDir;

	private ReactiveProperty<Vector3> rxCalculatedDeltaAimPos;

	private ReactiveProperty<Vector3> rxDeltaAimPos;

	private ReactiveProperty<Vector3> rxForward;

	private ReactiveProperty<int> rxHp;

	private ReactiveProperty<float> rxStress;

	private ReactiveProperty<int> rxCorruption;

	private ReactiveProperty<float> rxStamina;

	private ReactiveProperty<int> rxSoul;

	private ReactiveProperty<int> rxUsingSkillId;

	private ReactiveProperty<Vector3> rxPosition;

	private ReactiveProperty<int> rxShield;

	private ReactiveProperty<float> rxInterpolationSpeed;

	private readonly Dictionary<int, int> _changeSkillOverrides;

	private readonly Dictionary<ulong, SpecialField> _enteredSpecialFields;

	private readonly Dictionary<ulong, float> _stackBuffNextFireTimes;

	private readonly Dictionary<int, int> _expirePreventedStackBuffRefCounts;

	protected TrackableDictionary<int, UnitSkill> trackableSkills;

	protected TrackableDictionary<int, UnitBuff> trackableBuffs;

	private TrackableDictionary<EConsumableType, UnitItemCoolTime> trackableConsumableTypeCoolTimes;

	private TrackableDictionary<EStatType, UnitStat> trackableStats;

	private TrackableDictionary<ECrowdControlType, UnitCc> trackableCrowdControls;

	private int _syncTick;

	protected GameHost GameHost => null;

	protected GameChannel GameChannel => null;

	public bool IsInTrainingRoom => false;

	public bool IsNonCombat => false;

	public abstract EUnitType UnitType { get; }

	public abstract EElementalType ElementalType { get; }

	public abstract EMonsterCategory[] MonsterCategories { get; }

	public abstract EStressState StressState { get; }

	public abstract int WeaponId { get; }

	public abstract int TableId { get; }

	public abstract bool IsInitialized { get; }

	public abstract float GlobalSkillCoolTime { get; }

	public float HitRadius { get; private set; }

	public ICollection<int> TrackableSkillIds => null;

	public Transform Transform => null;

	public EActionState SyncState => EActionState.None;

	public new bool IsHost => false;

	public abstract int HitLayer { get; }

	public abstract int TargetHitLayer { get; }

	public bool IsHitable => false;

	public bool IsInactiveInStage => false;

	public bool IsActivateInStage => false;

	public List<int> ActiveSkillIds { get; private set; }

	public List<int> PassiveSkillIds { get; private set; }

	public Subject<int[]> OnActiveSkillsChanged { get; }

	public Subject<int[]> OnPassiveSkillsChanged { get; }

	public Dictionary<int, int> SkillLevels { get; protected set; }

	public Dictionary<(ulong casterId, int shieldId), UnitShield> Shields { get; }

	public IReadOnlyReactiveProperty<EActionState> RxState => null;

	public IReadOnlyReactiveProperty<Vector3> RxMoveDir => null;

	public IReadOnlyReactiveProperty<Vector3> RxCalculatedDeltaAimPos => null;

	public IReadOnlyReactiveProperty<Vector3> RxDeltaAimPos => null;

	public IReadOnlyReactiveProperty<Vector3> RxForward => null;

	public IReadOnlyReactiveProperty<int> RxHp => null;

	public IReadOnlyReactiveProperty<float> RxStress => null;

	public IReadOnlyReactiveProperty<int> RxCorruption => null;

	public IReadOnlyReactiveProperty<float> RxStamina => null;

	public IReadOnlyReactiveProperty<int> RxSoul => null;

	public IReadOnlyReactiveProperty<int> RxUsingSkillId => null;

	public IReadOnlyReactiveProperty<Vector3> RxPosition => null;

	public IReadOnlyReactiveProperty<int> RxShield => null;

	public IReadOnlyReactiveProperty<float> RxInterpolationSpeed => null;

	public EActionState State
	{
		get
		{
			return EActionState.None;
		}
		set
		{
		}
	}

	public Vector3 MoveDir
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public Vector3 CalculatedDeltaAimPos
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public Vector3 DeltaAimPos
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public Vector3 Forward
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public int Hp
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public float Stress
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public int Corruption
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public float Stamina
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public int Soul
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int UsingSkillId
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public Vector3 Position
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public int Shield
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public float InterpolationSpeed
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public bool IsBasicAttackHolding { get; set; }

	public abstract float WeaponAtkMultiplier { get; }

	public IDictionary<EStatType, float> BaseStats { get; protected set; }

	public float BaseAtk => 0f;

	public float BaseDef => 0f;

	public float BaseMoveSpeed => 0f;

	public float BaseAtkSpeed => 0f;

	public float BaseHpMax => 0f;

	public float BaseHpRegen => 0f;

	public float BaseStaminaMax => 0f;

	public float BaseStaminaRegen => 0f;

	public float BaseRunStaminaCost => 0f;

	public float BaseRunIncreaseSpeed => 0f;

	public float BaseCorruptionMax => 0f;

	public float BaseAtkPercent => 0f;

	public float BaseDefPercent => 0f;

	public float BaseMoveSpeedPercent => 0f;

	public float BaseAtkSpeedPercent => 0f;

	public float BaseHpMaxPercent => 0f;

	public float BaseHpRegenPercent => 0f;

	public float BaseStressIntervalPercent => 0f;

	public float BasePenetration => 0f;

	public float BasePenetrationPercent => 0f;

	public float BaseCriticalChancePercent => 0f;

	public float BaseCriticalTakenChancePercent => 0f;

	public float BaseCriticalDamagePercent => 0f;

	public float BaseWeaknessDamagePercent => 0f;

	public float BaseCooldownReductionPercent => 0f;

	public float BaseAdditionalAttackDamagePercent => 0f;

	public float BaseReductionDamagePercent => 0f;

	public float BaseBackAttackDamagePercent => 0f;

	public float BaseDashDistancePercent => 0f;

	public float BaseFireResistancePercent => 0f;

	public float BaseWaterResistancePercent => 0f;

	public float BaseWindResistancePercent => 0f;

	public float BaseFireDamagePercent => 0f;

	public float BaseWaterDamagePercent => 0f;

	public float BaseWindDamagePercent => 0f;

	public float BaseRunStaminaCostPercent => 0f;

	public float BaseRunIncreaseSpeedPercent => 0f;

	public float BaseMax => 0f;

	public float Atk => 0f;

	public float Def => 0f;

	public float MoveSpeed => 0f;

	public float AtkSpeed => 0f;

	public float HpMax => 0f;

	public float HpRegen => 0f;

	public float StaminaMax => 0f;

	public float StaminaRegen => 0f;

	public float RunStaminaCost => 0f;

	public float RunIncreaseSpeed => 0f;

	public float CorruptionMax => 0f;

	public float AtkPercent => 0f;

	public float DefPercent => 0f;

	public float MoveSpeedPercent => 0f;

	public float AtkSpeedPercent => 0f;

	public float HpMaxPercent => 0f;

	public float HpRegenPercent => 0f;

	public float StressIntervalPercent => 0f;

	public float Penetration => 0f;

	public float PenetrationPercent => 0f;

	public float CriticalChancePercent => 0f;

	public float CriticalTakenChancePercent => 0f;

	public float CriticalDamagePercent => 0f;

	public float WeaknessDamagePercent => 0f;

	public float CooldownReductionPercent => 0f;

	public float AdditionalAttackDamagePercent => 0f;

	public float ReductionDamagePercent => 0f;

	public float BackAttackDamagePercent => 0f;

	public float DashDistancePercent => 0f;

	public float FireResistancePercent => 0f;

	public float WaterResistancePercent => 0f;

	public float WindResistancePercent => 0f;

	public float FireDamagePercent => 0f;

	public float WaterDamagePercent => 0f;

	public float WindDamagePercent => 0f;

	public float RunStaminaCostPercent => 0f;

	public float RunIncreaseSpeedPercent => 0f;

	public float Max => 0f;

	public IObservable<EStatType> RxAtk => null;

	public IObservable<EStatType> RxDef => null;

	public IObservable<EStatType> RxMoveSpeed => null;

	public IObservable<EStatType> RxAtkSpeed => null;

	public IObservable<EStatType> RxHpMax => null;

	public IObservable<EStatType> RxHpRegen => null;

	public IObservable<EStatType> RxStaminaMax => null;

	public IObservable<EStatType> RxStaminaRegen => null;

	public IObservable<EStatType> RxRunStaminaCost => null;

	public IObservable<EStatType> RxRunIncreaseSpeed => null;

	public IObservable<EStatType> RxCorruptionMax => null;

	public IObservable<EStatType> RxAtkPercent => null;

	public IObservable<EStatType> RxDefPercent => null;

	public IObservable<EStatType> RxMoveSpeedPercent => null;

	public IObservable<EStatType> RxAtkSpeedPercent => null;

	public IObservable<EStatType> RxHpMaxPercent => null;

	public IObservable<EStatType> RxHpRegenPercent => null;

	public IObservable<EStatType> RxStressIntervalPercent => null;

	public IObservable<EStatType> RxPenetration => null;

	public IObservable<EStatType> RxPenetrationPercent => null;

	public IObservable<EStatType> RxCriticalChancePercent => null;

	public IObservable<EStatType> RxCriticalTakenChancePercent => null;

	public IObservable<EStatType> RxCriticalDamagePercent => null;

	public IObservable<EStatType> RxWeaknessDamagePercent => null;

	public IObservable<EStatType> RxCooldownReductionPercent => null;

	public IObservable<EStatType> RxAdditionalAttackDamagePercent => null;

	public IObservable<EStatType> RxReductionDamagePercent => null;

	public IObservable<EStatType> RxBackAttackDamagePercent => null;

	public IObservable<EStatType> RxDashDistancePercent => null;

	public IObservable<EStatType> RxFireResistancePercent => null;

	public IObservable<EStatType> RxWaterResistancePercent => null;

	public IObservable<EStatType> RxWindResistancePercent => null;

	public IObservable<EStatType> RxFireDamagePercent => null;

	public IObservable<EStatType> RxWaterDamagePercent => null;

	public IObservable<EStatType> RxWindDamagePercent => null;

	public IObservable<EStatType> RxRunStaminaCostPercent => null;

	public IObservable<EStatType> RxRunIncreaseSpeedPercent => null;

	public IObservable<EStatType> RxMax => null;

	public bool HasChange => false;

	public ReactiveDictionary<int, UnitSkill> RxSkills { get; }

	public ReactiveDictionary<int, UnitBuff> RxBuffs { get; }

	public ReactiveDictionary<EConsumableType, UnitItemCoolTime> RxConsumableTypeCoolTimes { get; }

	public ReactiveDictionary<EStatType, UnitStat> RxStats { get; }

	public ReactiveDictionary<ECrowdControlType, UnitCc> RxCrowdControls { get; }

	public IDictionary<int, UnitSkill> Skills => null;

	public IDictionary<int, UnitBuff> Buffs => null;

	public IDictionary<EConsumableType, UnitItemCoolTime> ConsumableTypeCoolTimes => null;

	public IDictionary<EStatType, UnitStat> Stats => null;

	public IDictionary<ECrowdControlType, UnitCc> CrowdControls => null;

	public Subject<(EStatType statType, UnitStat oldValue, UnitStat newValue)> OnStatChanged { get; }

	ulong ITarget.NetworkObjectId => 0uL;

	public virtual void SetCalculatedDeltaAimPos(Vector3 value)
	{
	}

	protected void Init(IDictionary<EStatType, float> baseStats, IList<int> activeSkillIds, IList<int> passiveSkillIds, Vector3 forward)
	{
	}

	public void ApplyActiveSkills(IEnumerable<int> skillIds)
	{
	}

	private void SetSkillActive(int skillId, bool isActive)
	{
	}

	public void ApplyPassiveSkills(IEnumerable<int> skillIds)
	{
	}

	public List<int> ApplyPassiveSkillsDelta(IEnumerable<int> newSkillIds)
	{
		return null;
	}

	public void ClearConsumableTypeCoolTimes()
	{
	}

	public virtual void UpdateAim()
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	protected void SetNetworkVariable<T>(ref NetworkVariable<T> networkVariable, T value) where T : struct
	{
	}

	protected void RegisterVariableEvent<T>(NetworkVariable<T> syncVar, ReactiveProperty<T> rxVar) where T : struct
	{
	}

	protected void RegisterVariableEvent<TWire, TValue>(NetworkVariable<TWire> syncVar, ReactiveProperty<TValue> rxVar, Func<TWire, TValue> decode) where TWire : struct
	{
	}

	protected void UnregisterVariableEvent<T>(NetworkVariable<T> syncVar) where T : struct
	{
	}

	private void RegisterListEvent<T>(NetworkList<T> syncVar, IList<T> rxVar) where T : struct, IEquatable<T>
	{
	}

	protected void UnregisterListEvent<T>(NetworkList<T> syncVar) where T : struct, IEquatable<T>
	{
	}

	public void AddBuff(int buffId, IActionStats actionStats, int stack = 1, ulong casterNetworkObjectId = 0uL, Vector3 point = default(Vector3))
	{
	}

	public void RemoveBuff(int buffId, int stack = 1, bool isExpired = false)
	{
	}

	public void ClearBuffs()
	{
	}

	public void SetBuff(int buffId, IActionStats actionStats, int stack, ulong casterNetworkObjectId = 0uL, Vector3 point = default(Vector3))
	{
	}

	public void UpdateBuffCasterId(int buffId, IList<ulong> casterIds)
	{
	}

	public int GetBuffStackCount(int buffId)
	{
		return 0;
	}

	protected virtual void OnBuffActivated(int buffId)
	{
	}

	protected virtual void OnBuffDeactivated(int buffId)
	{
	}

	private void RemoveConflictingChangeSkillBuff(int newBuffId, int originalSkillId)
	{
	}

	protected virtual void OnChangeSkillEffectActivated(ChangeSkillEffect effect)
	{
	}

	protected virtual void OnChangeSkillEffectDeactivated(ChangeSkillEffect effect)
	{
	}

	private void RefreshBuffStats(int buffId)
	{
	}

	public bool CheckBuffTrigger(int buffId)
	{
		return false;
	}

	private bool EvaluateBuffTrigger(UnitModel unit, BuffTriggerBase trigger, TBuff buffData)
	{
		return false;
	}

	private bool EvaluateFloatValueCompare(UnitModel u, FloatValueCompareBuffTrigger trigger, TBuff buffData)
	{
		return false;
	}

	private bool EvaluateActiveSkillTag(UnitModel u, ActiveSkillTagBuffTrigger trigger)
	{
		return false;
	}

	private bool EvaluateNearbyUnitCount(UnitModel self, NearbyUnitCountBuffTrigger trigger, TBuff buffData)
	{
		return false;
	}

	public void UpdateBuffs()
	{
	}

	private void ApplyImmunityEffect(ImmunityEffect immunityEffect)
	{
	}

	public void RemoveStatusEffects(EStatusEffectTag effectTag)
	{
	}

	public bool HasFullyImmuneEffect()
	{
		return false;
	}

	public bool HasImmunityEffect(EStatusEffectTag effectTag)
	{
		return false;
	}

	public void UpdateStat(EStatType statType)
	{
	}

	private float GetSlowPercent(ESlowType slowType)
	{
		return 0f;
	}

	private void SubscribeRealtimeCasterStats(int buffId)
	{
	}

	private void UnsubscribeRealtimeCasterStats(int buffId)
	{
	}

	private void MarkRealtimeStatDirty(int buffId, EStatType changedCasterStatType)
	{
	}

	private void ConvergeRealtimeStats()
	{
	}

	private void RefreshAllRealtimeCasterValues()
	{
	}

	public float GetModifiedBuffDuration(TBuff buffData)
	{
		return 0f;
	}

	public bool IsSkillTargetable(int skillId, UnitModel target)
	{
		return false;
	}

	public void AddCrowdControl(ECrowdControlType type, float duration)
	{
	}

	public void RemoveCrowdControl(ECrowdControlType type)
	{
	}

	public void ReduceCrowdControlDuration(ECrowdControlType type, float percent)
	{
	}

	public void UpdateCrowdControl()
	{
	}

	public ECrowdControlType GetImmunizedCcType(ECrowdControlType ccType)
	{
		return ECrowdControlType.None;
	}

	public bool IsCrowdControlled(ECrowdControlType type)
	{
		return false;
	}

	public float GetRemainConsumableTypeCoolTime(EConsumableType consumableType)
	{
		return 0f;
	}

	public void ApplyConsumableTypeCoolTime(int itemId)
	{
	}

	public int GetSkillLevel(int skillId)
	{
		return 0;
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
	}

	protected virtual void RegisterVariableEvents()
	{
	}

	protected virtual void UnregisterVariableEvents()
	{
	}

	public int AbsorbDamageWithShields(int amount)
	{
		return 0;
	}

	public void AddShield(int shieldId, int amount, ulong casterId = 0uL, float? overrideDuration = null)
	{
	}

	public void RemoveShield(int shieldId, ulong casterId = 0uL, int amount = int.MaxValue)
	{
	}

	public void UpdateShields()
	{
	}

	public List<int> GetActiveSkillIds()
	{
		return null;
	}

	public float GetChangeSkillBuffEndTime(int skillId)
	{
		return 0f;
	}

	public void ApplyChangeSkillOverride(int originalSkillId, int replacementSkillId)
	{
	}

	public void RemoveChangeSkillOverride(int originalSkillId, int replacementSkillId)
	{
	}

	private void OnChangeSkillOverridesChanged()
	{
	}

	public void UpdateSkillStacks()
	{
	}

	public void ConsumeSkillStack(int skillId)
	{
	}

	public void StartSkillCoolDown(int skillId)
	{
	}

	public void StartGlobalSkillCooldown()
	{
	}

	public float GetRemainCoolTime(int skillId)
	{
		return 0f;
	}

	public float GetSkillChargeAmount(int skillId)
	{
		return 0f;
	}

	public void AdjustSkillCosts(TSkill skill, ref int hpCost, ref int stressCost, ref int staminaCost)
	{
	}

	public virtual bool CheckSkill(int skillId)
	{
		return false;
	}

	public void ConsumeSkillCharge(int skillId)
	{
	}

	private float GetReducedStackChargeInterval(TSkill skillData)
	{
		return 0f;
	}

	public bool CheckSkillActionCost(SkillActionNode node, TSkill skill)
	{
		return false;
	}

	public bool IsInCoolTime(int skillId)
	{
		return false;
	}

	public void AddSkillCharge(int skillId, float addAmount)
	{
	}

	public void OnEnterSpecialField(ulong networkObjectId)
	{
	}

	public void OnExitSpecialField(ulong networkObjectId)
	{
	}

	public void UpdateSpecialFieldsOnRevive()
	{
	}

	public void UpdateSpecialFields()
	{
	}

	public void UpdateStackBuffTimers()
	{
	}

	public List<T> GetEnteredSpecialFields<T>() where T : SpecialField
	{
		return null;
	}

	public virtual float GetBaseStat(EStatType statType)
	{
		return 0f;
	}

	public virtual float GetAdditionalStat(EStatType statType)
	{
		return 0f;
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}

	public void SetStat(EStatType statType, float value)
	{
	}

	public bool HasBuff(int buffId)
	{
		return false;
	}

	public EStatusEffectTag GetBuffStatusEffectTags()
	{
		return EStatusEffectTag.None;
	}

	public EStatusEffectTag GetCcStatusEffectTags()
	{
		return EStatusEffectTag.None;
	}

	public EStatusEffectTag GetStatusEffectTags()
	{
		return EStatusEffectTag.None;
	}

	public float GetStatusEffectTagEndTime(EStatusEffectTag tag)
	{
		return 0f;
	}

	public bool HasStatusEffectTag(EStatusEffectTag effectTag)
	{
		return false;
	}

	public bool HasBuffEffect(EBuffEffectType effectType)
	{
		return false;
	}

	public IList<BuffEffectInfo<T>> GetBuffEffects<T>(bool checkIsActive = true) where T : BuffEffect
	{
		return null;
	}

	public bool IsDamageBlockedByInvincibility(bool isCritical = false, bool isBackAttack = false)
	{
		return false;
	}

	public void Synchronize()
	{
	}

	private void FlushTrackableDatas(bool syncToClients = true, HashSet<ulong> excludeIds = null)
	{
	}

	private void ApplySkills(UnitSkill[] skills)
	{
	}

	private void ApplyBuffs(UnitBuff[] buffs)
	{
	}

	private void ApplyConsumableTypeCoolTimes(UnitItemCoolTime[] consumableTypeCoolTimes)
	{
	}

	private void ApplyStats(UnitStat[] stats)
	{
	}

	private void ApplyCrowdControls(UnitCc[] crowdControls)
	{
	}

	private void OnReqSyncUnitModel(ReqSyncUnitModel message)
	{
	}

	private void OnResSyncUnitModel(ResSyncUnitModel message)
	{
	}

	private void OnUpdateUnitModel(NtfUpdateUnitModel message)
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

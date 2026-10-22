using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class HitBox : NetworkBase, ITarget, IActionExecutable, IActionStats
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected GameHost GameHost;

	[Inject]
	protected AssetManager AssetManager;

	[Inject]
	protected SeriLogger Logger;

	private Collider _hitBoxCollider;

	private Collider _traceCollider;

	private readonly HashSet<Collider> _triggeredSet;

	private readonly NetworkVariable<int> _syncHitBoxId;

	private readonly NetworkVariable<EHitBoxDespawnReason> _syncDespawnReason;

	public ReactiveProperty<EHitBoxDespawnReason> RxDespawnReason;

	private readonly NetworkVariable<bool> _syncIsFlipped;

	private int _hitBoxId;

	private HitLayerInfo _hitLayerInfo;

	private ActionStatsData _statsData;

	private bool _isFlipped;

	private Vector3 _executionPosition;

	private Vector3 _deltaAimPos;

	private Vector3 _normalizedDir;

	private Vector3 _rotatedStartOffset;

	private EHitBoxMoveType _moveType;

	private float _moveSpeedBase;

	private AnimationCurve _moveSpeedCurve;

	private float _parabolaMoveHeight;

	private float _minMoveDistance;

	private float _maxMoveDistance;

	private Vector3 _moveStartOffset;

	private GameObject _moveTarget;

	private TweenerCore<float, float, FloatOptions> _moveTween;

	private IHitable _traceTarget;

	private readonly List<CollisionEvent> _disabledCollisionEvents;

	private readonly Dictionary<CollisionDamageEvent, float> _skillChargeAccumulated;

	private int _skillId;

	private int _level;

	private float _elapsedTime;

	private bool _isFirstHit;

	private bool _isDurationExpired;

	private int _pierceRemain;

	private int _collisionScaleKeyIndex;

	private readonly NetworkVariable<ulong> _syncOwnerUnitNetworkObjectId;

	private UnitModel _ownerUnitModel;

	private UnitIdentity _ownerIdentity;

	private readonly HashSet<Collider> _currentHitColliders;

	private IActionExecutable _actionExecutableImplementation;

	private bool _isMadness;

	public Vector3 Position => default;

	public HitBoxInfo HitBoxInfo => null;

	public int HitBoxId => 0;

	private UnitModel AliveOwnerUnitModel => null;

	private UnitController OwnerUnitController => null;

	private ulong OwnerUnitNetworkObjectId => 0uL;

	private float OwnerUnitHitRadius => 0f;

	private bool HasOwnerUnit => false;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	public GameObjectManager ObjectManager => null;

	public GameObject GameObject => null;

	ulong ITarget.NetworkObjectId => 0uL;

	ulong IActionExecutable.NetworkObjectId => 0uL;

	public void Init(int hitBoxId, UnitModel ownerUnitModel, ActionStatsData statDatas, HitBoxInfo hitBoxInfo, HitLayerInfo hitLayerInfo, Vector3 deltaAimPos, Vector3 executionPosition, bool isFlipped, int skillId, GameObject moveTarget)
	{
	}

	private void OnMultiHitIntervalElapsed()
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

	private void OnDespawnReasonChanged(EHitBoxDespawnReason oldValue, EHitBoxDespawnReason newValue)
	{
	}

	private string GetEndSfx(EHitBoxDespawnReason reason)
	{
		return null;
	}

	private void SpawnVfx(string vfxName, Vector3 hitBoxScale, bool isLoopVfx, float vfxLoopDuration, float zOffset, IList<AnimationSpeed> vfxSpeed)
	{
	}

	private void FixedUpdate()
	{
	}

	private void UpdateCollisionScale()
	{
	}

	private void MoveTrace(Vector3 targetPosition)
	{
	}

	private void LateUpdate()
	{
	}

	private float GetOwnerStat(EStatType statType)
	{
		return 0f;
	}

	private void Move(Vector3 direction)
	{
	}

	private float GetCalculatedMoveSpeed()
	{
		return 0f;
	}

	public void MoveToPosition(Vector3 deltaMovePos)
	{
	}

	public void MoveToPositionBySpeed(Vector3 deltaMovePos)
	{
	}

	public void SetMoveData(EHitBoxMoveType moveType, float moveSpeedBase = 0f, AnimationCurve moveSpeedCurve = null, Vector3 deltaAimPos = default(Vector3), float minMoveDistance = -1f, float maxMoveDistance = -1f, float parabolaMoveHeight = 0f, Vector3 startOffset = default(Vector3), GameObject moveTarget = null)
	{
	}

	private void UpdateTracingRotate(MonoBehaviour traceTargetMono)
	{
	}

	private bool CheckHitable(Collider target, bool includeHighObstacle, EHitTarget targetType, out IHitable hitable, IEnumerable<HitableCondition> conditions = null)
	{
		hitable = null;
		return false;
	}

	private bool CheckHitableCondition(HitableCondition condition, IHitable hitable)
	{
		return false;
	}

	private bool MatchHitableCondition(IHitable hitable, IEnumerable<HitableCondition> conditions)
	{
		return false;
	}

	private void OnHitTriggerStay(Collider other)
	{
	}

	private void UpdateHitCollision()
	{
	}

	private NtfDisplayFx CreateNtfDisplayFx(Vector3 hitPosition)
	{
		return null;
	}

	private bool HandleCollisionEvent(CollisionEvent collisionEvent, Collider other, Vector3 hitPosition, bool isParrying, ref NtfDisplayFx ntfDisplayFx, ref bool parrySucceeded)
	{
		return false;
	}

	private void HandleFxEvent(CollisionFxEvent fxEvent, Vector3 hitPosition, GameObject target)
	{
	}

	private void HandleBuffEvent(CollisionBuffEvent buffEvent, GameObject target)
	{
	}

	private void HandleRemoveBuffEvent(CollisionRemoveStatusEffectEvent statusEffectEvent, GameObject target)
	{
	}

	private void GrantOwnerSkillChargeOnDamageStart(CollisionDamageEvent damageEvent)
	{
	}

	private void DecreaseOwnerWeaponDurabilityOnDamageStart(int calculatedDamage)
	{
	}

	private void HandleDamageEvent(CollisionDamageEvent damageEvent, bool madnessExtended, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx, bool isFreezeFireTouch = false)
	{
	}

	private void HandleHealEvent(CollisionHealEvent healEvent, GameObject target)
	{
	}

	private void HandleStressDamageEvent(CollisionStressDamageEvent StressDamageEvent, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx)
	{
	}

	private void HandleStressRecoveryEvent(CollisionStressRecoveryEvent recoveryEvent, GameObject target)
	{
	}

	private void HandleStressRecoveryOwnerEvent(CollisionStressRecoveryOwnerEvent recoveryEvent, GameObject target)
	{
	}

	private void HandleBrightnessDamageEvent(CollisionBrightnessDamageEvent damageEvent, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx)
	{
	}

	private void HandleBrightnessRecoveryEvent(CollisionBrightnessRecoveryEvent recoveryEvent, GameObject target)
	{
	}

	private void HandleSoulRecoveryOwnerEvent(CollisionSoulRecoveryOnHitMonsterEvent recoveryEvent, GameObject target)
	{
	}

	private void HandleForceMonsterSetTargetEvent(CollisionForceMonsterSetTargetEvent forceMonsterSetTargetEvent, GameObject target)
	{
	}

	private void HandleTryMonsterCuriousStateEvent(CollisionTryMonsterCuriousStateEvent tryCuriousStateEvent, GameObject target)
	{
	}

	private void HandleCrowdControlEvent(CollisionCrowdControlEvent crowdControlEvent, GameObject target, Vector3 hitPosition)
	{
	}

	private void HandleAcquireParadoxEvent(CollisionAcquireParadoxEvent acquireParadoxEvent, GameObject target)
	{
	}

	private void HandleRemoveParadoxEvent(CollisionRemoveParadoxEvent removeParadoxEvent, GameObject target)
	{
	}

	private void OnTraceTargetEnter(Collider other)
	{
	}

	private void SpawnHitBoxOnCollision(int id)
	{
	}

	private void HandleSpawnMonster(CollisionSummonEvent summonEvent, Vector3 hitPosition)
	{
	}

	private void SpawnExtraUnitOnDestroy(int extraUnitId)
	{
	}

	private Vector3 CorrectAimDir(Vector3 originalDir)
	{
		return default;
	}

	private Vector3 GetAttackDirectionFlat(Vector3 targetPosition)
	{
		return default;
	}

	private bool CheckFrontGuard(UnitModel target, out FrontGuardEffect frontGuardEffect)
	{
		frontGuardEffect = null;
		return false;
	}

	private bool CheckBackAttack(UnitModel target)
	{
		return false;
	}

	private void DestroyHitBox()
	{
	}

	private EHitTarget GetHitTargetType(EHitTarget baseHitTargetType)
	{
		return EHitTarget.None;
	}

	private EElementalType GetHitElementType(DamageInfo damageInfo, UnitModel targetUnit)
	{
		return EElementalType.None;
	}

	public bool HasElementalType(EElementalType elementalType)
	{
		return false;
	}

	private void HandleSkillChargeEvent(CollisionSkillChargeEvent chargeEvent, GameObject target)
	{
	}

	private void HandleSpecialFieldEvent(CollisionSpecialFieldEvent specialFieldEvent, Vector3 hitPosition)
	{
	}

	private void HandleShieldEvent(CollisionShieldEvent shieldEvent, GameObject target)
	{
	}

	private void HandleAddSuppressionEvent(CollisionAddSuppressionEvent addSuppressionEvent, GameObject target)
	{
	}

	private void HandleCaptureEvent(CollisionCaptureEvent captureEvent, GameObject target)
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}

	private bool IsAlly(UnitModel targetUnit)
	{
		return false;
	}

	private bool IsFreezeFireTouchCandidate(CollisionEvent collisionEvent, Collider other)
	{
		return false;
	}



}

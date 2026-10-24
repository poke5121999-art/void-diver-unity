using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using SomeNet.Utility;
using UniRx;
using UniRx.Triggers;
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

	private readonly HashSet<Collider> _triggeredSet = new HashSet<Collider>();

	private readonly NetworkVariable<int> _syncHitBoxId = new NetworkVariable<int>();

	private readonly NetworkVariable<EHitBoxDespawnReason> _syncDespawnReason = new NetworkVariable<EHitBoxDespawnReason>();

	public ReactiveProperty<EHitBoxDespawnReason> RxDespawnReason = new ReactiveProperty<EHitBoxDespawnReason>();

	private readonly NetworkVariable<bool> _syncIsFlipped = new NetworkVariable<bool>();

	private int _hitBoxId = -1;

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

	private readonly List<CollisionEvent> _disabledCollisionEvents = new List<CollisionEvent>();

	private readonly Dictionary<CollisionDamageEvent, float> _skillChargeAccumulated = new Dictionary<CollisionDamageEvent, float>();

	private int _skillId = -1;

	private int _level;

	private float _elapsedTime;

	private bool _isFirstHit = true;

	private bool _isDurationExpired;

	private int _pierceRemain;

	private int _collisionScaleKeyIndex;

	private readonly NetworkVariable<ulong> _syncOwnerUnitNetworkObjectId = new NetworkVariable<ulong>();

	private UnitModel _ownerUnitModel;

	private UnitIdentity _ownerIdentity;

	private readonly HashSet<Collider> _currentHitColliders = new HashSet<Collider>();

	private IActionExecutable _actionExecutableImplementation;

	private bool _isMadness;

	public Vector3 Position => base.transform.position;

	public HitBoxInfo HitBoxInfo => THitBox.Table[_gameManager.IsHost ? _hitBoxId : _syncHitBoxId.Value]?.HitBoxInfo;

	public int HitBoxId => _syncHitBoxId.Value;

	private UnitModel AliveOwnerUnitModel => (_ownerUnitModel != null && _ownerUnitModel.IsSpawned) ? _ownerUnitModel : null;

	private UnitController OwnerUnitController => _ownerUnitModel ? _ownerUnitModel.GetComponent<UnitController>() : null;

	private ulong OwnerUnitNetworkObjectId => _ownerUnitModel ? _ownerUnitModel.NetworkObjectId : ulong.MaxValue;

	private float OwnerUnitHitRadius => _ownerUnitModel ? _ownerUnitModel.HitRadius : 0f;

	private bool HasOwnerUnit => _ownerUnitModel != null;

	public EElementalType ElementalType => (_ownerUnitModel != null) ? _ownerUnitModel.ElementalType : EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	public GameObjectManager ObjectManager => _objectManager;

	public GameObject GameObject => base.gameObject;

	ulong ITarget.NetworkObjectId => base.NetworkObjectId;

	ulong IActionExecutable.NetworkObjectId => base.NetworkObjectId;

	public void Init(int hitBoxId, UnitModel ownerUnitModel, ActionStatsData statDatas, HitBoxInfo hitBoxInfo, HitLayerInfo hitLayerInfo, Vector3 deltaAimPos, Vector3 executionPosition, bool isFlipped, int skillId, GameObject moveTarget)
	{
		AssertIfNotHost();
		_hitBoxId = hitBoxId;
		_ownerUnitModel = ownerUnitModel;
		_ownerIdentity = UnitIdentity.From(ownerUnitModel);
		_hitLayerInfo = hitLayerInfo;
		_statsData = statDatas;
		_isFlipped = isFlipped;
		_skillId = skillId;
		_moveTarget = moveTarget;
		_executionPosition = executionPosition;
		Vector3 vector = new Vector3(deltaAimPos.x, 0f, deltaAimPos.z);
		if (hitBoxInfo.UseAimCorrection)
		{
			vector = CorrectAimDir(new Vector3(deltaAimPos.x, 0f, deltaAimPos.z));
		}
		SetMoveData(hitBoxInfo.MoveType, hitBoxInfo.moveSpeed, hitBoxInfo.MoveSpeedCurve, vector, hitBoxInfo.MinMoveDistance, hitBoxInfo.MaxMoveDistance, hitBoxInfo.ParabolaMoveHeight, hitBoxInfo.startOffset);
		_level = ((_ownerUnitModel != null) ? _ownerUnitModel.GetSkillLevel(skillId) : 0);
		_hitBoxCollider = GetComponentInChildren<Collider>();
		_hitBoxCollider.enabled = false;
		_isFirstHit = true;
		_elapsedTime = 0f;
		_pierceRemain = hitBoxInfo.pierceCount;
		_disabledCollisionEvents.Clear();
		ObservableFixed.FixedTimer(TimeSpan.FromSeconds(hitBoxInfo.collisionDelay)).Subscribe(delegate
		{
			_hitBoxCollider.enabled = true;
		}).AddTo(this);
		if (hitBoxInfo.collisionEndTime > 0f)
		{
			ObservableFixed.FixedTimer(TimeSpan.FromSeconds(hitBoxInfo.collisionEndTime)).Subscribe(delegate
			{
				_hitBoxCollider.enabled = false;
			}).AddTo(this);
		}
		_hitBoxCollider.OnTriggerStayAsObservable().Subscribe(OnHitTriggerStay).AddTo(this);
		_traceTarget = null;
		if (hitBoxInfo.tracingAngle > 0f && hitBoxInfo.tracingDetectingRange > 0f)
		{
			GameObject gameObject = new GameObject("TraceCollider");
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.layer = base.gameObject.layer;
			_traceCollider = gameObject.GetOrAddComponent<SphereCollider>();
			((SphereCollider)_traceCollider).radius = hitBoxInfo.tracingDetectingRange;
			_traceCollider.isTrigger = true;
			_traceCollider.OnTriggerEnterAsObservable().Subscribe(OnTraceTargetEnter).AddTo(this);
		}
		if (hitBoxInfo.multiHitInterval > 0f)
		{
			Observable.Timer(TimeSpan.FromSeconds(hitBoxInfo.collisionDelay)).Subscribe(delegate
			{
				Observable.Interval(TimeSpan.FromSeconds(hitBoxInfo.multiHitInterval)).Subscribe(delegate
				{
					_triggeredSet.Clear();
					_skillChargeAccumulated.Clear();
				}).AddTo(this);
			}).AddTo(this);
		}
		if (hitBoxInfo.ClampSpawnPositionToWall)
		{
			Vector3 vector2 = base.transform.position - _executionPosition;
			float magnitude = vector2.magnitude;
			if (magnitude > 0f && Physics.Raycast(_executionPosition, vector2.normalized, out var hitInfo, magnitude, Const.LayerMask.HIGH_OBSTACLE))
			{
				base.transform.position = hitInfo.point;
			}
		}
	}


	private void OnMultiHitIntervalElapsed()
	{
		_triggeredSet.Clear();
		_skillChargeAccumulated.Clear();
	}


	public override void OnHostSpawn()
	{
		_syncHitBoxId.Value = _hitBoxId;
		_syncOwnerUnitNetworkObjectId.Value = OwnerUnitNetworkObjectId;
		_syncIsFlipped.Value = _isFlipped;
	}


	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_syncDespawnReason.OnValueChanged += OnDespawnReasonChanged;
		if (!base.IsHost)
		{
			_ownerUnitModel = _objectManager.FindUnitModel(_syncOwnerUnitNetworkObjectId.Value);
			if (HitBoxInfo.vfx.HasValue())
			{
				SpawnVfx(HitBoxInfo.vfx, HitBoxInfo.collisionScale, HitBoxInfo.IsLoopVfx, HitBoxInfo.VfxLoopDuration, HitBoxInfo.VfxZOffset, HitBoxInfo.vfxSpeeds);
			}
		}
		else
		{
			EGroundType groundType = BackgroundTileTypeCore.GetGroundType(base.transform.position);
			string text = HitBoxInfo.ResolveSpawnSfx(groundType);
			if (text.HasValue())
			{
				GameHost.SendToAll(new NtfPlaySfx
				{
					Sfx = text,
					Position = base.transform.position.ToProtoVector3(),
					TargetNetworkObjectId = (HitBoxInfo.SfxFollowOwner ? OwnerUnitNetworkObjectId : 0)
				});
			}
			if (HitBoxInfo.vfx.HasValue())
			{
				SpawnVfx(HitBoxInfo.vfx, HitBoxInfo.collisionScale, HitBoxInfo.IsLoopVfx, HitBoxInfo.VfxLoopDuration, HitBoxInfo.VfxZOffset, HitBoxInfo.vfxSpeeds);
			}
			if (_ownerUnitModel != null)
			{
				if (HitBoxInfo.IsCancleOnSkillChange)
				{
					_ownerUnitModel.RxUsingSkillId.Skip(1).Subscribe(delegate
					{
						DestroyHitBox();
					}).AddTo(this);
				}
				if (HitBoxInfo.IsCancelOnOwnerDie)
				{
					_ownerUnitModel.RxState.Where((EActionState state) => state == EActionState.Dying || state == EActionState.Dead).Subscribe(delegate
					{
						DestroyHitBox();
					}).AddTo(this);
				}
			}
			ObservableFixed.FixedTimer(TimeSpan.FromSeconds(HitBoxInfo.duration)).Subscribe(delegate
			{
				_isDurationExpired = true;
			}).AddTo(this);
		}
		if (base.transform.lossyScale.x < 0f || base.transform.lossyScale.y < 0f || base.transform.lossyScale.z < 0f)
		{
			Logger.Warning(string.Format("HitBox negative scale : {0}", _syncHitBoxId.Value));
		}
		_objectManager.OnTargetSpawned.OnNext(this);
	}


	public override void OnNetworkDespawn()
	{
		_objectManager.OnTargetDespawned.OnNext(this);
		base.OnNetworkDespawn();
		_syncDespawnReason.OnValueChanged -= OnDespawnReasonChanged;
	}


	private void OnDespawnReasonChanged(EHitBoxDespawnReason oldValue, EHitBoxDespawnReason newValue)
	{
		RxDespawnReason.Value = newValue;
		string text;
		switch (newValue)
		{
		case EHitBoxDespawnReason.Timeout:
			text = HitBoxInfo.DestroyByTimeoutSfx;
			break;
		case EHitBoxDespawnReason.Destroy:
			text = HitBoxInfo.DestroyByHitSfx;
			break;
		default:
			text = null;
			break;
		}
		if (text.HasValue())
		{
			SoundManager.PlaySfx3D(text, base.transform.position);
		}
	}


	private string GetEndSfx(EHitBoxDespawnReason reason)
	{
		switch (reason)
		{
		case EHitBoxDespawnReason.Timeout:
			return HitBoxInfo?.DestroyByTimeoutSfx;
		case EHitBoxDespawnReason.Destroy:
			return HitBoxInfo?.DestroyByHitSfx;
		default:
			return null;
		}
	}


	private float GetOwnerStat(EStatType statType)
	{
		if (_statsData == null || _statsData.StatDic == null)
		{
			return 0f;
		}
		if (!_statsData.StatDic.TryGetValue(statType, out var value))
		{
			return 0f;
		}
		return value;
	}


	private float GetCalculatedMoveSpeed()
	{
		if (_moveSpeedCurve == null)
		{
			return _moveSpeedBase;
		}
		return _moveSpeedCurve.Evaluate(_elapsedTime / HitBoxInfo.duration) * _moveSpeedBase;
	}


	private EHitTarget GetHitTargetType(EHitTarget baseHitTargetType)
	{
		bool flag = false;
		if (_ownerUnitModel != null && _ownerUnitModel.HasBuffEffect(EBuffEffectType.Madness))
		{
			flag = baseHitTargetType == EHitTarget.Enemy;
		}
		_isMadness = flag;
		if (!flag)
		{
			return baseHitTargetType;
		}
		return EHitTarget.AllExceptOwner;
	}


	private EElementalType GetHitElementType(DamageInfo damageInfo, UnitModel targetUnit)
	{
		return InGameUtility.GetResolvedElementalType(damageInfo, AliveOwnerUnitModel, targetUnit);
	}


	public bool HasElementalType(EElementalType elementalType)
	{
		if (HitBoxInfo == null)
		{
			return false;
		}
		if (HitBoxInfo.collisionEvents == null)
		{
			return false;
		}
		HashSet<EElementalType> hashSet = HitBoxInfo.collisionEvents.OfType<CollisionDamageEvent>().Select((CollisionDamageEvent _) => _.DamageInfo.ElementalType).ToHashSet();
		if (_ownerUnitModel != null)
		{
			hashSet.Add(_ownerUnitModel.ElementalType);
		}
		return hashSet.Contains(elementalType);
	}


	public float GetStat(EStatType statType)
	{
		if (_statsData == null || _statsData.StatDic == null)
		{
			return 0f;
		}
		if (!_statsData.StatDic.TryGetValue(statType, out var value))
		{
			return 0f;
		}
		return value;
	}


	private bool IsAlly(UnitModel targetUnit)
	{
		if (_ownerUnitModel == null || targetUnit == null)
		{
			return false;
		}
		return _ownerUnitModel.TargetHitLayer == targetUnit.TargetHitLayer;
	}


	private bool IsFreezeFireTouchCandidate(CollisionEvent collisionEvent, Collider other)
	{
		if (collisionEvent is CollisionDamageEvent collisionDamageEvent)
		{
			UnitModel component = other.GetComponent<UnitModel>();
			if (component != null && component.IsCrowdControlled(ECrowdControlType.Freeze))
			{
				if (collisionDamageEvent.DamageInfo.ElementalType == EElementalType.Fire)
				{
					return true;
				}
				if (_ownerUnitModel != null && _ownerUnitModel.ElementalType == EElementalType.Fire)
				{
					return true;
				}
			}
		}
		return false;
	}


	private bool MatchHitableCondition(IHitable hitable, IEnumerable<HitableCondition> conditions)
	{
		if (conditions == null)
		{
			return true;
		}
		return conditions.All((HitableCondition _) => CheckHitableCondition(_, hitable));
	}


	private bool CheckHitableCondition(HitableCondition condition, IHitable hitable)
	{
		if (condition is HpPercentCondition hpPercentCondition)
		{
			if (hitable is UnitModel unitModel && unitModel.HpMax > 0f)
			{
				return (float)unitModel.Hp / unitModel.HpMax * 100f < hpPercentCondition.HpPercent;
			}
			return false;
		}
		if (condition is MonsterTypeCondition monsterTypeCondition)
		{
			if (hitable is MonsterModel monsterModel)
			{
				return monsterModel.MonsterType == monsterTypeCondition.MonsterType;
			}
			return false;
		}
		return true;
	}


	private Vector3 GetAttackDirectionFlat(Vector3 targetPosition)
	{
		Vector3 vector;
		if (_moveType == EHitBoxMoveType.FollowSelf)
		{
			Vector3 vector2 = ((_ownerUnitModel != null) ? _ownerUnitModel.Position : base.transform.position);
			vector = new Vector3(targetPosition.x - vector2.x, 0f, targetPosition.z - vector2.z);
			if (vector == Vector3.zero)
			{
				return Vector3.zero;
			}
		}
		else if (!HitBoxInfo.UsePositionBasedBackAttack)
		{
			vector = new Vector3(_normalizedDir.x, 0f, _normalizedDir.z);
			if (vector == Vector3.zero)
			{
				vector = new Vector3(targetPosition.x - _executionPosition.x, 0f, targetPosition.z - _executionPosition.z);
				if (vector == Vector3.zero)
				{
					return Vector3.zero;
				}
			}
		}
		else
		{
			vector = new Vector3(targetPosition.x - base.transform.position.x, 0f, targetPosition.z - base.transform.position.z);
			if (vector == Vector3.zero)
			{
				return Vector3.zero;
			}
		}
		return vector.normalized;
	}


	private bool CheckFrontGuard(UnitModel target, out FrontGuardEffect frontGuardEffect)
	{
		frontGuardEffect = null;
		if (!target.HasBuffEffect(EBuffEffectType.FrontGuard))
		{
			return false;
		}
		Vector3 vector = GetAttackDirectionFlat(target.transform.position);
		if (vector == Vector3.zero)
		{
			return false;
		}
		Vector3 incomingDir = -vector;
		Vector3 aimDir = target.CalculatedDeltaAimPos;
		if (aimDir == Vector3.zero)
		{
			aimDir = target.Forward;
		}
		if (aimDir == Vector3.zero)
		{
			return false;
		}
		aimDir = aimDir.normalized;
		FrontGuardEffect frontGuardEffect2 = target.GetBuffEffects<FrontGuardEffect>().Select((BuffEffectInfo<FrontGuardEffect> _) => _.BuffEffect).Where((FrontGuardEffect effect) => Mathf.Cos(Mathf.Clamp(effect.AngleDeg, 0f, 360f) * 0.5f * (MathF.PI / 180f)) <= Vector3.Dot(aimDir, incomingDir))
			.OrderByDescending((FrontGuardEffect e) => e.AngleDeg)
			.FirstOrDefault();
		if (frontGuardEffect2 != null)
		{
			frontGuardEffect = frontGuardEffect2;
			return true;
		}
		return false;
	}


	private bool CheckBackAttack(UnitModel target)
	{
		Vector3 lhs = GetAttackDirectionFlat(target.transform.position);
		if (lhs == Vector3.zero)
		{
			return false;
		}
		Vector3 vector = ((_moveType == EHitBoxMoveType.FollowSelf) ? ((!(_ownerUnitModel != null)) ? base.transform.position : _ownerUnitModel.transform.position) : _executionPosition);
		float num = vector.x - target.transform.position.x;
		float num2 = vector.z - target.transform.position.z;
		float floatValue = TConst.BackAttackMaxRange.FloatValue;
		if (num * num + 0f + num2 * num2 <= floatValue * floatValue)
		{
			Vector3 vector2 = new Vector3(target.Forward.x, 0f, target.Forward.z);
			if (vector2 != Vector3.zero)
			{
				vector2 = vector2.normalized;
				float angle = TConst.CharacterBackAttackAngle.FloatValue;
				if (target is MonsterModel monsterModel)
				{
					angle = TMonster.Table[monsterModel.MonsterId].BackAttackAngle;
				}
				else if (target is ExtraUnitModel)
				{
					angle = TExtraUnit.Table[target.TableId].BackAttackAngle;
				}
				return Mathf.Cos(angle * 0.5f * (MathF.PI / 180f)) < Vector3.Dot(lhs, vector2);
			}
			return false;
		}
		return false;
	}



	private void SpawnVfx(string vfxName, Vector3 hitBoxScale, bool isLoopVfx, float vfxLoopDuration, float zOffset, IList<AnimationSpeed> vfxSpeed)
	{
		GameObject gameObject = AssetManager.SpawnSkillVfx(vfxName);
		if (gameObject == null)
		{
			return;
		}
		float x = ((HitBoxInfo.InheritOwnerScaleX && _syncIsFlipped.Value) ? (-1f) : 1f);
		gameObject.transform.localScale = new Vector3(x, 1f, 1f);
		Vector3 vector = new Vector3(0f, 0f, zOffset);
		gameObject.transform.position = base.transform.TransformPoint(vector);
		gameObject.transform.rotation = base.transform.rotation;
		SkillVfx skillVfx = gameObject.GetOrAddComponent<SkillVfx>();
		if (skillVfx == null)
		{
			Logger.Error(string.Format("[HitBox] SkillVfx component missing/invalid on VFX prefab: '{0}'. HitBoxId={1}", vfxName, _hitBoxId));
			return;
		}
		GameObject ownerGo = ((_ownerUnitModel != null) ? _ownerUnitModel.gameObject : null);
		skillVfx.Init(ownerGo, this, vector);
		bool flag = vfxSpeed != null && vfxSpeed.Count > 0;
		if (isLoopVfx || flag)
		{
			gameObject.GetOrAddComponent<SpeedVfxObject>().SetSpeed(vfxSpeed, isLoopVfx, vfxLoopDuration).Forget();
		}
	}


	private void FixedUpdate()
	{
		if (!_gameManager.IsHost || !base.IsSpawned || _isDurationExpired)
		{
			return;
		}
		_elapsedTime += Time.fixedDeltaTime;
		if (_moveType == EHitBoxMoveType.Parabola || _moveType == EHitBoxMoveType.ParabolaToAim || _moveType == EHitBoxMoveType.LinearToAim || _moveType == EHitBoxMoveType.LinearToAimBySpeed)
		{
			return;
		}
		UpdateCollisionScale();
		if (_traceTarget is MonoBehaviour monoBehaviour && monoBehaviour != null)
		{
			UpdateTracingRotate(monoBehaviour);
		}
		if (_moveType == EHitBoxMoveType.FollowSelf && _ownerUnitModel != null)
		{
			base.transform.position = _ownerUnitModel.transform.position + _rotatedStartOffset;
			return;
		}
		if (_moveType == EHitBoxMoveType.TraceOwner && _ownerUnitModel != null)
		{
			MoveTrace(_ownerUnitModel.transform.position);
			return;
		}
		if (_moveType == EHitBoxMoveType.TraceMoveTarget && _moveTarget != null)
		{
			MoveTrace(_moveTarget.transform.position);
			return;
		}
		Move(_normalizedDir);
	}


	private void UpdateCollisionScale()
	{
		CollisionScaleKey[] collisionScaleKeys = HitBoxInfo.CollisionScaleKeys;
		if (collisionScaleKeys == null || collisionScaleKeys.Length == 0 || _collisionScaleKeyIndex >= collisionScaleKeys.Length)
		{
			return;
		}
		while (_collisionScaleKeyIndex < collisionScaleKeys.Length && !(_elapsedTime < collisionScaleKeys[_collisionScaleKeyIndex].Time))
		{
			_collisionScaleKeyIndex++;
		}
		float a;
		Vector3 a2;
		if (_collisionScaleKeyIndex == 0)
		{
			a = 0f;
			a2 = HitBoxInfo.collisionScale;
		}
		else
		{
			a = collisionScaleKeys[_collisionScaleKeyIndex - 1].Time;
			a2 = collisionScaleKeys[_collisionScaleKeyIndex - 1].Scale;
		}
		if (_collisionScaleKeyIndex < collisionScaleKeys.Length)
		{
			CollisionScaleKey collisionScaleKey = collisionScaleKeys[_collisionScaleKeyIndex];
			float t = Mathf.Clamp01((_elapsedTime - a) / (collisionScaleKey.Time - a));
			base.transform.localScale = Vector3.Lerp(a2, collisionScaleKey.Scale, t);
		}
		else
		{
			base.transform.localScale = a2;
		}
	}


	private void MoveTrace(Vector3 targetPosition)
	{
		Vector3 vector = targetPosition - base.transform.position;
		if (vector.magnitude < 0.2f)
		{
			DestroyHitBox();
			return;
		}
		vector = vector.normalized;
		_normalizedDir = vector;
		Move(vector);
	}


	private void LateUpdate()
	{
		if (base.IsHost)
		{
			UpdateHitCollision();
			if (_isDurationExpired)
			{
				DestroyHitBox();
			}
		}
	}



	private void Move(Vector3 direction)
	{
		base.transform.position += direction * GetCalculatedMoveSpeed() * Time.fixedDeltaTime;
	}



	public void MoveToPosition(Vector3 deltaMovePos)
	{
		if (_moveTween != null)
		{
			_moveTween.Kill();
		}
		Vector3 startPosition = base.transform.position;
		Vector3 targetPosition = startPosition + deltaMovePos;
		bool lockRotation = HitBoxInfo.LockRotationOnMove;
		_moveTween = DOTween.To(() => 0f, delegate(float t)
		{
			Vector3 vector = Vector3.Lerp(startPosition, targetPosition, t);
			vector.y += Mathf.Sin(t * MathF.PI) * _parabolaMoveHeight;
			if (!lockRotation)
			{
				Vector3 vector2 = vector - base.transform.position;
				if (vector2 != Vector3.zero)
				{
					base.transform.rotation = Quaternion.LookRotation(vector2, Vector3.up);
				}
			}
			base.transform.position = vector;
		}, 1f, HitBoxInfo.duration).SetEase(Ease.Linear);
		_moveTween.SetLink(base.gameObject);
	}


	public void MoveToPositionBySpeed(Vector3 deltaMovePos)
	{
		if (_moveTween != null)
		{
			_moveTween.Kill();
		}
		Vector3 startPosition = base.transform.position;
		Vector3 targetPosition = startPosition + deltaMovePos;
		bool lockRotation = HitBoxInfo.LockRotationOnMove;
		_moveTween = DOTween.To(() => 0f, delegate(float t)
		{
			Vector3 vector = Vector3.Lerp(startPosition, targetPosition, t);
			vector.y += Mathf.Sin(t * MathF.PI) * _parabolaMoveHeight;
			if (!lockRotation)
			{
				Vector3 vector2 = vector - base.transform.position;
				if (vector2 != Vector3.zero)
				{
					base.transform.rotation = Quaternion.LookRotation(vector2, Vector3.up);
				}
			}
			base.transform.position = vector;
		}, 1f, deltaMovePos.magnitude / _moveSpeedBase).SetEase(Ease.Linear);
		_moveTween.onComplete = (TweenCallback)Delegate.Combine(_moveTween.onComplete, new TweenCallback(DestroyHitBox));
		_moveTween.SetLink(base.gameObject);
	}


	public void SetMoveData(EHitBoxMoveType moveType, float moveSpeedBase = 0f, AnimationCurve moveSpeedCurve = null, Vector3 deltaAimPos = default(Vector3), float minMoveDistance = -1f, float maxMoveDistance = -1f, float parabolaMoveHeight = 0f, Vector3 startOffset = default(Vector3), GameObject moveTarget = null)
	{
		_moveSpeedBase = moveSpeedBase;
		_moveType = moveType;
		_moveSpeedCurve = moveSpeedCurve;
		_deltaAimPos = deltaAimPos;
		_normalizedDir = deltaAimPos.normalized;
		_parabolaMoveHeight = parabolaMoveHeight;
		_maxMoveDistance = maxMoveDistance;
		_moveStartOffset = startOffset;
		_minMoveDistance = minMoveDistance;
		_moveTarget = moveTarget;
		_rotatedStartOffset = MathUtility.LookRotationOrIdentity(_normalizedDir) * _moveStartOffset;
		if (_moveType == EHitBoxMoveType.ParabolaToAim || _moveType == EHitBoxMoveType.LinearToAim)
		{
			float num = _deltaAimPos.magnitude;
			if (num < _minMoveDistance)
			{
				num = _minMoveDistance;
			}
			else if (_maxMoveDistance < num)
			{
				num = _maxMoveDistance;
			}
			MoveToPosition(new Vector3(_normalizedDir.x * num - _rotatedStartOffset.x, 0f - _rotatedStartOffset.y, _normalizedDir.z * num - _rotatedStartOffset.z));
		}
		else if (_moveType == EHitBoxMoveType.LinearToAimBySpeed)
		{
			float num2 = _deltaAimPos.magnitude;
			if (num2 < _minMoveDistance)
			{
				num2 = _minMoveDistance;
			}
			else if (_maxMoveDistance < num2)
			{
				num2 = _maxMoveDistance;
			}
			MoveToPositionBySpeed(new Vector3(_normalizedDir.x * num2 - _rotatedStartOffset.x, 0f - _rotatedStartOffset.y, _normalizedDir.z * num2 - _rotatedStartOffset.z));
		}
		else if (_moveType == EHitBoxMoveType.Parabola)
		{
			float randomFloat = RandomUtility.GetRandomFloat(_minMoveDistance, _maxMoveDistance);
			MoveToPosition(new Vector3(_normalizedDir.x * randomFloat - _rotatedStartOffset.x, 0f - _rotatedStartOffset.y, _normalizedDir.z * randomFloat - _rotatedStartOffset.z));
		}
	}


	private void UpdateTracingRotate(MonoBehaviour traceTargetMono)
	{
		Vector3 position = base.transform.position;
		Vector3 vector = (traceTargetMono.transform.position + Vector3.up * _moveStartOffset.y - position).normalized;
		if (vector == Vector3.zero)
		{
			return;
		}
		Quaternion rotation = base.transform.rotation;
		Quaternion to = Quaternion.LookRotation(vector, Vector3.up);
		Quaternion rotation2 = Quaternion.RotateTowards(rotation, to, HitBoxInfo.tracingAngle);
		base.transform.rotation = rotation2;
		_normalizedDir = base.transform.rotation * Vector3.forward;
		_rotatedStartOffset = Quaternion.LookRotation(_normalizedDir, Vector3.up) * _moveStartOffset;
	}


	private bool CheckHitable(Collider target, bool includeHighObstacle, EHitTarget targetType, out IHitable hitable, IEnumerable<HitableCondition> conditions = null)
	{
		hitable = target.GetComponent<IHitable>();
		bool flag = MatchHitableCondition(hitable, conditions);
		if (target.IsBreakableProp())
		{
			return flag;
		}
		if (target.IsHighObstacle())
		{
			return flag && includeHighObstacle;
		}
		if (hitable != null && hitable.IsHitable && flag)
		{
			UnitModel component = target.GetComponent<UnitModel>();
			if (component != null && targetType.IsEnemyTargetType() && component.GetBuffEffects<InvincibilityEffect>().Any((BuffEffectInfo<InvincibilityEffect> _) => _.BuffEffect.BlockEnemyHit))
			{
				return false;
			}
			if (!HitBoxInfo.ignoreObstaclesForCollision)
			{
				float floatValue = TConst.ObstacleEdgeCheckMargin.FloatValue;
				Vector3 position = target.transform.position;
				float num = Vector3.Distance(_executionPosition, position);
				Vector3 normalized = (position - _executionPosition).normalized;
				Vector3 vector = Vector3.Cross(Vector3.up, normalized).normalized * floatValue;
				float num2 = (_ownerUnitModel ? _ownerUnitModel.HitRadius : 0f);
				if (hitable.HitRadius + num2 < num && Physics.Raycast(_executionPosition + vector, normalized, out var _, num, Const.LayerMask.HIGH_OBSTACLE) && Physics.Raycast(_executionPosition - vector, normalized, out var _, num, Const.LayerMask.HIGH_OBSTACLE))
				{
					return false;
				}
			}
			int targetHitLayer = ((_ownerUnitModel == null) ? 0 : _ownerUnitModel.TargetHitLayer);
			HitLayerInfo hitLayerInfo = hitable.GetHitLayerInfo();
			if (targetType.IsHitable(_hitLayerInfo, hitLayerInfo, targetHitLayer))
			{
				return true;
			}
		}
		return false;
	}




	private void OnHitTriggerStay(Collider other)
	{
		_currentHitColliders.Add(other);
	}


	private void UpdateHitCollision()
	{
		if (!_currentHitColliders.Any())
		{
			return;
		}
		List<Collider> list = _currentHitColliders.Where((Collider _) => _ != null && _.transform != null).OrderBy((Collider _) => Vector3.Distance(_executionPosition, _.transform.position)).ToList();
		_currentHitColliders.Clear();
		if (HitBoxInfo.collisionDestroy == ECollisionDestroyType.Once)
		{
			foreach (Collider item in list)
			{
				if (TryCollision(item))
				{
					DestroyHitBox();
					return;
				}
			}
		}
		if (HitBoxInfo.collisionDestroy == ECollisionDestroyType.Pierce)
		{
			foreach (Collider item2 in list)
			{
				if (item2.IsHighObstacle())
				{
					TryCollision(item2);
					DestroyHitBox();
					return;
				}
				if (!TryCollision(item2))
				{
					continue;
				}
				_pierceRemain--;
				if (_pierceRemain < 1)
				{
					DestroyHitBox();
					return;
				}
				int num = HitBoxInfo.pierceCount - _pierceRemain;
				if (HitBoxInfo.pierceChances == null || num - 1 < 0 || num - 1 >= HitBoxInfo.pierceChances.Length)
				{
					if (HitBoxInfo.pierceSfx.HasValue())
					{
						GameHost.SendToAll(new NtfPlaySfx
						{
							Sfx = HitBoxInfo.pierceSfx,
							Position = base.transform.position.ToProtoVector3()
						});
					}
				}
				else if (HitBoxInfo.pierceChances[num - 1] <= UnityEngine.Random.Range(0, 100))
				{
					DestroyHitBox();
					return;
				}
			}
		}
		if (HitBoxInfo.collisionDestroy == ECollisionDestroyType.WallOnly)
		{
			foreach (Collider item3 in list)
			{
				TryCollision(item3);
				if (item3.IsHighObstacle())
				{
					DestroyHitBox();
					return;
				}
			}
		}
		if (HitBoxInfo.collisionDestroy != ECollisionDestroyType.Never)
		{
			return;
		}
		bool flag = false;
		foreach (Collider item4 in list.Where((Collider _) => _ != null && (_.gameObject.layer == Const.Layer.UNIT || _.gameObject.layer == Const.Layer.BREAKABLE_PROP)))
		{
			if (TryCollision(item4))
			{
				flag = true;
			}
		}
		if (flag)
		{
			foreach (Collider item5 in list.Where((Collider _) => _ != null && _.gameObject.layer == Const.Layer.HIGH_OBSTACLE))
			{
				_triggeredSet.Add(item5);
			}
			return;
		}
		Collider collider = list.FirstOrDefault((Collider _) => _ != null && _.gameObject.layer == Const.Layer.HIGH_OBSTACLE);
		if (collider != null)
		{
			TryCollision(collider);
		}

		bool TryCollision(Collider other)
		{
			if (_triggeredSet.Contains(other))
			{
				return false;
			}
			bool flag2 = false;
			bool flag3 = false;
			bool parrySucceeded = false;
			other.TryGetComponent<UnitModel>(out var component);
			other.TryGetComponent<UnitController>(out var component2);
			if (component != null && !component.IsInitialized)
			{
				return false;
			}
			Vector3 hitPosition = ((HitBoxInfo.hitPointType == EHitPointType.NearByOwner) ? _executionPosition : base.transform.position);
			hitPosition = other.ClosestPoint(hitPosition);
			NtfDisplayFx ntfDisplayFx = CreateNtfDisplayFx(hitPosition);
			if (HitBoxInfo.UseBoneAttachHitVfx)
			{
				UnitModel component3 = other.GetComponent<UnitModel>();
				ntfDisplayFx.TargetNetworkObjectId = ((component3 == null) ? 0 : component3.NetworkObjectId);
				ntfDisplayFx.BoneType = HitBoxInfo.HitVfxBoneType;
				ntfDisplayFx.TargetPositionOffset = HitBoxInfo.HitVfxBoneOffset.ToProtoVector3();
			}
			if (HitBoxInfo.UseHitVfxRotate)
			{
				ntfDisplayFx.Direction = _normalizedDir.ToProtoVector3();
			}
			bool isParrying = false;
			if (HitBoxInfo.CanParry && component != null)
			{
				isParrying = component.HasBuffEffect(EBuffEffectType.Parry);
			}
			CollisionEvent[] collisionEvents = HitBoxInfo.collisionEvents;
			for (int i = 0; i < collisionEvents.Length; i++)
			{
				CollisionEvent collisionEvent = collisionEvents[i];
				if (collisionEvent != null && !_disabledCollisionEvents.Contains(collisionEvent) && HandleCollisionEvent(collisionEvent, other, hitPosition, isParrying, ref ntfDisplayFx, ref parrySucceeded))
				{
					flag3 = true;
					flag2 = true;
				}
			}
			if (parrySucceeded)
			{
				ParryEvent parryEvent = new ParryEvent
				{
					HitPosition = hitPosition
				};
				parryEvent.SourceAttacker = ((_ownerUnitModel != null && _ownerUnitModel.IsSpawned) ? _ownerUnitModel : null);
				parryEvent.SkillId = _skillId;
				component2.HandleUnitEvent(parryEvent);
			}
			if (!flag3)
			{
				if (!other.IsHighObstacle())
				{
					return false;
				}
				if (HitBoxInfo.ignoreObstaclesForCollision)
				{
					return false;
				}
			}
			_triggeredSet.Add(other);
			if (HitBoxInfo.CollisionTriggerDelay > 0f)
			{
				Observable.Timer(TimeSpan.FromSeconds(HitBoxInfo.CollisionTriggerDelay)).Where((long _) => other != null && _triggeredSet.Contains(other)).Subscribe(delegate
				{
					_triggeredSet.Remove(other);
				})
					.AddTo(this);
			}
			if (_ownerUnitModel != null && _ownerUnitModel.IsSpawned)
			{
				if (_ownerUnitModel is CharacterModel characterModel && HitBoxInfo.OwnerHitShakeAmplitude > 0f && HitBoxInfo.OwnerHitShakeFrequency > 0f && HitBoxInfo.OwnerHitShakeDuration > 0f)
				{
					GameHost.SendToClient(characterModel.ClientId, new NtfCameraShake
					{
						TargetClientId = characterModel.ClientId,
						IsGlobal = false,
						ShakeAmplitude = HitBoxInfo.OwnerHitShakeAmplitude,
						ShakeFrequency = HitBoxInfo.OwnerHitShakeFrequency,
						ShakeDuration = HitBoxInfo.OwnerHitShakeDuration
					});
				}
				UnitModel component4 = other.GetComponent<UnitModel>();
				UnitModel unitModel = ((_ownerUnitModel != null && _ownerUnitModel.IsSpawned) ? _ownerUnitModel : null);
				if (unitModel != null && component4)
				{
					GameHost.SendToAll(new NtfHitCollision
					{
						SourceNetworkObjectId = unitModel.NetworkObjectId,
						TargetNetworkObjectId = component4.NetworkObjectId
					});
				}
				if (ntfDisplayFx != null && !string.IsNullOrEmpty(ntfDisplayFx.Vfx))
				{
					if (other.IsHighObstacle() && HitBoxInfo.IgnoreHitVfxWall)
					{
						return true;
					}
					GameHost.SendToAll(ntfDisplayFx);
				}
				return true;
			}
			return false;
		}
	}

	private NtfDisplayFx CreateNtfDisplayFx(Vector3 hitPosition)
	{
		string vfx = null;
		if (!string.IsNullOrEmpty(HitBoxInfo.hitVfx))
		{
			vfx = ((!HitBoxInfo.UseElementalHitVfx) ? string.Format("Skill/{0}", HitBoxInfo.hitVfx) : string.Format("Skill/{0}", string.Format("{0}_{1}", HitBoxInfo.hitVfx, _ownerUnitModel.ElementalType)));
		}
		string sfx = ((!HitBoxInfo.UseElementalHitSfx) ? HitBoxInfo.hitSfx : string.Format("{0}_{1}", HitBoxInfo.hitSfx, _ownerUnitModel.ElementalType));
		return new NtfDisplayFx
		{
			Vfx = vfx,
			VfxDuration = HitBoxInfo.hitVfxDuration,
			VfxScale = 1f,
			Sfx = sfx,
			Position = hitPosition.ToProtoVector3(),
			AnimSpeeds = HitBoxInfo.hitVfxSpeeds,
			IsLoopVfx = HitBoxInfo.IsLoopHitVfx,
			LoopDuration = HitBoxInfo.HitVfxLoopDuration
		};
	}


	private bool HandleCollisionEvent(CollisionEvent collisionEvent, Collider other, Vector3 hitPosition, bool isParrying, ref NtfDisplayFx ntfDisplayFx, ref bool parrySucceeded)
	{
		EHitTarget targetType = collisionEvent.targetType;
		_isMadness = _ownerUnitModel != null && _ownerUnitModel.HasBuffEffect(EBuffEffectType.Madness) && targetType == EHitTarget.Enemy;
		bool madnessExtended = false;
		if (_isMadness)
		{
			madnessExtended = targetType == EHitTarget.Enemy;
			targetType = EHitTarget.AllExceptOwner;
		}
		IHitable hitable;
		bool isFreezeFireTouch = false;
		if (!CheckHitable(other, includeHighObstacle: true, targetType, out hitable, collisionEvent.HitableConditions))
		{
			if (!IsFreezeFireTouchCandidate(collisionEvent, other))
			{
				return false;
			}
			if (!CheckHitable(other, includeHighObstacle: true, EHitTarget.AllExceptOwner, out hitable, collisionEvent.HitableConditions))
			{
				return false;
			}
			isFreezeFireTouch = true;
		}
		if (!CheckTalentConditions() || !CheckBuffConditions())
		{
			return false;
		}
		if (collisionEvent.IsOnce && !isFreezeFireTouch)
		{
			_disabledCollisionEvents.Add(collisionEvent);
		}
		if (isParrying && CheckHitable(other, includeHighObstacle: false, EHitTarget.Enemy, out var _))
		{
			parrySucceeded = true;
			return true;
		}
		if (collisionEvent is CollisionTargetableHitBoxEvent collisionTargetableHitBoxEvent)
		{
			SpawnHitBoxOnCollision(collisionTargetableHitBoxEvent.hitBoxId);
		}
		if (collisionEvent is CollisionFxEvent fxEvent)
		{
			HandleFxEvent(fxEvent, hitPosition, other.gameObject);
		}
		else if (collisionEvent is CollisionSummonEvent summonEvent)
		{
			HandleSpawnMonster(summonEvent, hitPosition);
		}
		else if (collisionEvent is CollisionBuffEvent buffEvent)
		{
			HandleBuffEvent(buffEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionRemoveStatusEffectEvent statusEffectEvent)
		{
			HandleRemoveBuffEvent(statusEffectEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionDamageEvent damageEvent)
		{
			HandleDamageEvent(damageEvent, madnessExtended, hitPosition, other.gameObject, ref ntfDisplayFx, isFreezeFireTouch);
		}
		else if (collisionEvent is CollisionSpecialFieldEvent specialFieldEvent)
		{
			HandleSpecialFieldEvent(specialFieldEvent, hitPosition);
		}
		else if (collisionEvent is CollisionShieldEvent shieldEvent)
		{
			HandleShieldEvent(shieldEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionAddSuppressionEvent addSuppressionEvent)
		{
			HandleAddSuppressionEvent(addSuppressionEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionCaptureEvent captureEvent)
		{
			HandleCaptureEvent(captureEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionHealEvent healEvent)
		{
			HandleHealEvent(healEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionStressDamageEvent stressDamageEvent)
		{
			HandleStressDamageEvent(stressDamageEvent, hitPosition, other.gameObject, ref ntfDisplayFx);
		}
		else if (collisionEvent is CollisionStressRecoveryEvent stressRecoveryEvent)
		{
			HandleStressRecoveryEvent(stressRecoveryEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionStressRecoveryOwnerEvent stressRecoveryOwnerEvent)
		{
			HandleStressRecoveryOwnerEvent(stressRecoveryOwnerEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionBrightnessDamageEvent brightnessDamageEvent)
		{
			HandleBrightnessDamageEvent(brightnessDamageEvent, hitPosition, other.gameObject, ref ntfDisplayFx);
		}
		else if (collisionEvent is CollisionBrightnessRecoveryEvent brightnessRecoveryEvent)
		{
			HandleBrightnessRecoveryEvent(brightnessRecoveryEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionSoulRecoveryOnHitMonsterEvent soulRecoveryEvent)
		{
			HandleSoulRecoveryOwnerEvent(soulRecoveryEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionForceMonsterSetTargetEvent forceMonsterSetTargetEvent)
		{
			HandleForceMonsterSetTargetEvent(forceMonsterSetTargetEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionTryMonsterCuriousStateEvent tryCuriousStateEvent)
		{
			HandleTryMonsterCuriousStateEvent(tryCuriousStateEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionCrowdControlEvent crowdControlEvent)
		{
			HandleCrowdControlEvent(crowdControlEvent, other.gameObject, hitPosition);
		}
		else if (collisionEvent is CollisionAcquireParadoxEvent acquireParadoxEvent)
		{
			HandleAcquireParadoxEvent(acquireParadoxEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionRemoveParadoxEvent removeParadoxEvent)
		{
			HandleRemoveParadoxEvent(removeParadoxEvent, other.gameObject);
		}
		else if (collisionEvent is CollisionSkillChargeEvent chargeEvent)
		{
			HandleSkillChargeEvent(chargeEvent, other.gameObject);
		}
		else if (collisionEvent is not CollisionTargetableHitBoxEvent)
		{
			throw new ArgumentOutOfRangeException("collisionEvent", collisionEvent, "Unhandled collision event type: ");
		}
		if (_isFirstHit)
		{
			_isFirstHit = false;
		}
		return true;

		bool CheckTalentConditions()
		{
			if (HasOwnerUnit && collisionEvent.talentConditionList != null && collisionEvent.talentConditionList.Count > 0)
			{
				return collisionEvent.talentConditionList.All(delegate(TalentCondition condition)
				{
					switch (condition.TalentConditionType)
					{
					case ETalentConditionType.Owned:
						return _ownerUnitModel.HasTalent(condition.TalentId);
					case ETalentConditionType.NotOwned:
						return !_ownerUnitModel.HasTalent(condition.TalentId);
					default:
						return true;
					}
				});
			}
			return true;
		}

		bool CheckBuffConditions()
		{
			if (collisionEvent.BuffConditionList == null || collisionEvent.BuffConditionList.Count == 0)
			{
				return true;
			}
			UnitController targetController = ((hitable is UnitModel unitModel) ? unitModel.GetComponent<UnitController>() : null);
			return collisionEvent.BuffConditionList.All(delegate(BuffCondition condition)
			{
				switch (condition.ConditionOwnerType)
				{
				case EConditionTargetType.SkillOwner:
				{
					if (!HasOwnerUnit)
					{
						return false;
					}
					UnitController ownerUnitController = OwnerUnitController;
					return ownerUnitController.CheckBuffCondition(condition, OwnerUnitController.NetworkObjectId, (targetController == null) ? 0 : targetController.NetworkObjectId);
				}
				case EConditionTargetType.CollisionTarget:
					if (targetController == null)
					{
						return false;
					}
					return targetController.CheckBuffCondition(condition, (OwnerUnitController == null) ? 0 : OwnerUnitController.NetworkObjectId, targetController.NetworkObjectId);
				default:
					return true;
				}
			});
		}
	}

	private void HandleFxEvent(CollisionFxEvent fxEvent, Vector3 hitPosition, GameObject target)
	{
		NtfDisplayFx ntfDisplayFx = new NtfDisplayFx
		{
			Position = hitPosition.ToProtoVector3(),
			Vfx = (string.IsNullOrEmpty(fxEvent.Vfx) ? null : string.Format("Skill/{0}", fxEvent.Vfx)),
			VfxScale = 1f,
			Sfx = fxEvent.Sfx,
			VfxDuration = fxEvent.VfxDuration,
			AnimSpeeds = fxEvent.VfxSpeeds,
			IsLoopVfx = fxEvent.IsLoopVfx,
			LoopDuration = fxEvent.VfxLoopDuration
		};
		if (!fxEvent.IsIndependent)
		{
			UnitModel component = target.GetComponent<UnitModel>();
			ntfDisplayFx.TargetNetworkObjectId = ((component == null) ? 0 : component.NetworkObjectId);
			ntfDisplayFx.BoneType = fxEvent.BoneType;
			ntfDisplayFx.TargetPositionOffset = fxEvent.Offset.ToProtoVector3();
		}
		else
		{
			ntfDisplayFx.Position = (hitPosition + Quaternion.Euler(_normalizedDir) * fxEvent.Offset).ToProtoVector3();
		}
		if (fxEvent.UseRotation)
		{
			ntfDisplayFx.Direction = _normalizedDir.ToProtoVector3();
		}
		GameHost.SendToAll(ntfDisplayFx);
	}

	private void HandleBuffEvent(CollisionBuffEvent buffEvent, GameObject target)
	{
		UnitModel component = target.GetComponent<UnitModel>();
		if (component == null || !RandomUtility.GetRandomBoolFromPercentage(buffEvent.Probability))
		{
			return;
		}
		if (buffEvent.FactionCondition != EMonsterCategory.None && !(component.MonsterCategories ?? Array.Empty<EMonsterCategory>()).Contains(buffEvent.FactionCondition))
		{
			return;
		}
		if (buffEvent.CcCondition != ECrowdControlType.None && !component.IsCrowdControlled(buffEvent.CcCondition))
		{
			return;
		}
		if (buffEvent.EffectTag != EStatusEffectTag.None && !component.HasStatusEffectTag(buffEvent.EffectTag))
		{
			return;
		}
		if (buffEvent.MonsterCondition != EMonsterType.None)
		{
			if (!(component is MonsterModel))
			{
				return;
			}
			if (!TMonster.Table.TryGetValue(component.TableId, out var value))
			{
				return;
			}
			if (value.MonsterType != buffEvent.MonsterCondition)
			{
				return;
			}
		}
		UnitController component2 = target.GetComponent<UnitController>();
		if (component2 != null)
		{
			component2.HandleUnitEvent(new BuffEvent
			{
				SourceTarget = ((ITarget)AliveOwnerUnitModel ?? (ITarget)this),
				BuffId = buffEvent.buffId,
				BuffStack = buffEvent.BuffStack
			});
		}
	}

	private void HandleRemoveBuffEvent(CollisionRemoveStatusEffectEvent statusEffectEvent, GameObject target)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			component.HandleUnitEvent(new RemoveStatusEffectEvent
			{
				SourceTarget = ((ITarget)AliveOwnerUnitModel ?? (ITarget)this),
				EffectTags = statusEffectEvent.EffectTags
			});
		}
	}

	private void GrantOwnerSkillChargeOnDamageStart(CollisionDamageEvent damageEvent)
	{
		if (OwnerUnitController == null || damageEvent == null || damageEvent.DamageInfo == null)
		{
			return;
		}
		if (_skillId > 0 && TSkill.Table[_skillId].ChargeCost > 0)
		{
			return;
		}
		float num = damageEvent.DamageInfo.StatFactor;
		if (_statsData != null)
		{
			num = (float)damageEvent.DamageInfo.Power / _statsData.GetStat(EStatType.Atk) + num;
		}
		if (num <= 0f)
		{
			return;
		}
		float num2 = num + num;
		_skillChargeAccumulated.TryGetValue(damageEvent, out var value);
		if (value > 0f)
		{
			num *= 0.5f;
		}
		if (num2 < num + value)
		{
			num = num2 - value;
		}
		if (!(num <= 0f))
		{
			_skillChargeAccumulated[damageEvent] = value + num;
			OwnerUnitController.HandleUnitEvent(new SkillChargeEvent
			{
				Amount = num,
				SourceModel = _ownerUnitModel
			});
		}
	}

	private void DecreaseOwnerWeaponDurabilityOnDamageStart(int calculatedDamage)
	{
		if (OwnerUnitController is CharacterController characterController && calculatedDamage > 0)
		{
			float num = ((_statsData == null) ? 0f : _statsData.GetStat(EStatType.Atk));
			if (num > 0f)
			{
				float num2 = (float)calculatedDamage / num * TConst.DurabilityDecreaseFactorByDamageRate.FloatValue;
				if (num2 > 0f)
				{
					characterController.DecreaseWeaponDurability(num2);
				}
			}
		}
	}


	private void HandleDamageEvent(CollisionDamageEvent damageEvent, bool madnessExtended, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx, bool isFreezeFireTouch = false)
	{
		BreakableProp component = target.GetComponent<BreakableProp>();
		if (component != null)
		{
			component.OnDamage();
			return;
		}
		UnitModel unitModel = target.GetComponent<UnitModel>();
		if (unitModel == null)
		{
			return;
		}
		GrantOwnerSkillChargeOnDamageStart(damageEvent);
		UnitModel aliveOwnerUnitModel = AliveOwnerUnitModel;
		bool isBackAttack = damageEvent.CanBackAttack && CheckBackAttack(unitModel);
		InGameUtility.DamageResult damageResult = InGameUtility.CalculateDamage(damageEvent.DamageInfo, this, unitModel, isBackAttack, _level, aliveOwnerUnitModel, allowCritical: true, _ownerIdentity);
		int damage = damageResult.Damage;
		bool isCritical = damageResult.IsCritical;
		EElementalType resolvedElementalType = InGameUtility.GetResolvedElementalType(damageEvent.DamageInfo, AliveOwnerUnitModel, unitModel);
		if (!string.IsNullOrEmpty(ntfDisplayFx.Vfx))
		{
			ntfDisplayFx.Vfx = string.Format(HitBoxInfo.UseElementalHitVfx ? string.Format("{0}_{1}", "Skill/{0}", resolvedElementalType) : "Skill/{0}", HitBoxInfo.hitVfx);
		}
		else
		{
			ntfDisplayFx.Vfx = null;
		}
		if (isCritical && HitBoxInfo.criticalHitSfx.HasValue())
		{
			ntfDisplayFx.Sfx = (HitBoxInfo.UseElementalCritSfx ? string.Format("{0}_{1}", HitBoxInfo.criticalHitSfx, resolvedElementalType) : HitBoxInfo.criticalHitSfx);
		}
		if (madnessExtended && _ownerUnitModel != null && unitModel != null && _ownerUnitModel.TargetHitLayer == unitModel.TargetHitLayer)
		{
			damage = Math.Max(1, (int)((float)damage * TConst.MadnessAllyDamageFactor.FloatValue));
		}
		if (CheckFrontGuard(unitModel, out var frontGuardEffect))
		{
			Vector3 position = unitModel.transform.position;
			Vector3 normalized = unitModel.Forward.normalized;
			ntfDisplayFx.Position = (position + normalized * frontGuardEffect.VfxForwardPos + new Vector3(0f, 0.75f, 0f)).ToProtoVector3();
			damage = (int)((1f - frontGuardEffect.DamageReductionPercent / 100f) * (float)damage);
			if (!string.IsNullOrEmpty(frontGuardEffect.Vfx))
			{
				ntfDisplayFx.Vfx = string.Format("Skill/{0}", frontGuardEffect.Vfx);
				ntfDisplayFx.VfxDuration = frontGuardEffect.VfxDuration;
			}
			if (!string.IsNullOrEmpty(frontGuardEffect.Sfx))
			{
				ntfDisplayFx.Sfx = frontGuardEffect.Sfx;
			}
		}
		int num = (isFreezeFireTouch ? 0 : damage);
		UnitController component2 = target.GetComponent<UnitController>();
		if (OwnerUnitController is CharacterController characterController && num > 0)
		{
			float num2 = ((_statsData == null) ? 0f : _statsData.GetStat(EStatType.Atk));
			if (num2 > 0f)
			{
				float num3 = (float)num / num2 * TConst.DurabilityDecreaseFactorByDamageRate.FloatValue;
				if (num3 > 0f)
				{
					characterController.DecreaseWeaponDurability(num3);
				}
			}
		}
		EElementalDamagedType elementalDamagedType = InGameUtility.GetElementalDamagedType(damageEvent.DamageInfo, this, unitModel);
		component2.HandleUnitEvent(new HpDamageEvent
		{
			SourceModel = aliveOwnerUnitModel,
			SourceIdentity = _ownerIdentity,
			Amount = num,
			SkillId = _skillId,
			HitBoxId = _syncHitBoxId.Value,
			IsCritical = isCritical,
			IsBackAttack = isBackAttack,
			ElementalDamagedType = elementalDamagedType,
			IsFirstHit = _isFirstHit,
			HitPosition = hitPosition,
			ElementalType = resolvedElementalType,
			ReasonType = EHpDamageReasonType.Hit,
			IgnoreShield = damageEvent.DamageInfo.IgnoreShield
		});
		if (isBackAttack && !unitModel.IsDamageBlockedByInvincibility(isCritical, isBackAttack))
		{
			component2.HandleUnitEvent(new StressDamageEvent
			{
				SourceModel = aliveOwnerUnitModel,
				ReasonType = EStressReasonType.BackAttack,
				Amount = TConst.StressDamageOnBackAttack.IntValue,
				HitPosition = hitPosition,
				SkillId = _skillId
			});
		}
	}

	private void HandleHealEvent(CollisionHealEvent healEvent, GameObject target)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			float num = ((_statsData == null) ? 0f : _statsData.GetStat(healEvent.CoefficientStat));
			component.HandleUnitEvent(new HpRecoveryEvent
			{
				Amount = (int)((float)(healEvent.Power + healEvent.PowerGrowth * _level) + ((float)_level * healEvent.StatFactorGrowth + healEvent.StatFactor) / 100f * num),
				SourceModel = AliveOwnerUnitModel,
				SkillId = _skillId
			});
		}
	}

	private void HandleStressDamageEvent(CollisionStressDamageEvent StressDamageEvent, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx)
	{
		UnitModel unitModel = target.GetComponent<UnitModel>();
		if (unitModel == null)
		{
			return;
		}
		int amount = RandomUtility.GetRandomInt(StressDamageEvent.DamageMin, StressDamageEvent.DamageMax + 1);
		if (unitModel.GetBuffEffects<InvincibilityEffect>().Any((BuffEffectInfo<InvincibilityEffect> _) => _.BuffEffect.BlockStressDamage))
		{
			return;
		}
		if (CheckFrontGuard(unitModel, out var frontGuardEffect))
		{
			Vector3 position = unitModel.transform.position;
			Vector3 normalized = unitModel.Forward.normalized;
			ntfDisplayFx.Position = (position + normalized * frontGuardEffect.VfxForwardPos + new Vector3(0f, 0.75f, 0f)).ToProtoVector3();
			amount = (int)((frontGuardEffect.DamageReductionPercent / 100f + 1f) * (float)amount);
			if (!string.IsNullOrEmpty(frontGuardEffect.Vfx))
			{
				ntfDisplayFx.Vfx = string.Format("Skill/{0}", frontGuardEffect.Vfx);
				ntfDisplayFx.VfxDuration = frontGuardEffect.VfxDuration;
			}
			else
			{
				ntfDisplayFx.Vfx = string.Format("Skill/{0}", "1001/02_Shield_Hit");
				ntfDisplayFx.VfxDuration = 1f;
			}
			if (!string.IsNullOrEmpty(frontGuardEffect.Sfx))
			{
				ntfDisplayFx.Sfx = frontGuardEffect.Sfx;
			}
		}
		target.GetComponent<UnitController>().HandleUnitEvent(new StressDamageEvent
		{
			SourceModel = AliveOwnerUnitModel,
			ReasonType = EStressReasonType.StressDamaged,
			Amount = amount,
			HitPosition = hitPosition,
			SkillId = _skillId
		});
	}

	private void HandleStressRecoveryEvent(CollisionStressRecoveryEvent recoveryEvent, GameObject target)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			component.HandleUnitEvent(new StressRecoveryEvent
			{
				Amount = RandomUtility.GetRandomInt(recoveryEvent.AmountMin, recoveryEvent.AmountMax + 1)
			});
		}
	}

	private void HandleStressRecoveryOwnerEvent(CollisionStressRecoveryOwnerEvent recoveryEvent, GameObject target)
	{
		UnitModel component = target.GetComponent<UnitModel>();
		if (!(component == null) && component.State != EActionState.Dead && !(OwnerUnitController == null))
		{
			OwnerUnitController.HandleUnitEvent(new StressRecoveryEvent
			{
				Amount = RandomUtility.GetRandomInt(recoveryEvent.AmountMin, recoveryEvent.AmountMax + 1)
			});
		}
	}

	private void HandleBrightnessDamageEvent(CollisionBrightnessDamageEvent damageEvent, Vector3 hitPosition, GameObject target, ref NtfDisplayFx ntfDisplayFx)
	{
		UnitModel unitModel = target.GetComponent<UnitModel>();
		if (unitModel == null)
		{
			return;
		}
		int amount = RandomUtility.GetRandomInt(damageEvent.AmountMin, damageEvent.AmountMax + 1);
		if (CheckFrontGuard(unitModel, out var frontGuardEffect))
		{
			Vector3 position = unitModel.transform.position;
			Vector3 normalized = unitModel.Forward.normalized;
			ntfDisplayFx.Position = (position + normalized * frontGuardEffect.VfxForwardPos + new Vector3(0f, 0.75f, 0f)).ToProtoVector3();
			amount = (int)((frontGuardEffect.DamageReductionPercent / 100f + 1f) * (float)amount);
			if (!string.IsNullOrEmpty(frontGuardEffect.Vfx))
			{
				ntfDisplayFx.Vfx = string.Format("Skill/{0}", frontGuardEffect.Vfx);
				ntfDisplayFx.VfxDuration = frontGuardEffect.VfxDuration;
			}
			else
			{
				ntfDisplayFx.Vfx = string.Format("Skill/{0}", "1001/02_Shield_Hit");
				ntfDisplayFx.VfxDuration = 1f;
			}
			if (!string.IsNullOrEmpty(frontGuardEffect.Sfx))
			{
				ntfDisplayFx.Sfx = frontGuardEffect.Sfx;
			}
		}
		target.GetComponent<UnitController>().HandleUnitEvent(new LightFuelDamageEvent
		{
			Amount = amount,
			HitPosition = hitPosition,
			SkillId = _skillId
		});
	}

	private void HandleBrightnessRecoveryEvent(CollisionBrightnessRecoveryEvent recoveryEvent, GameObject target)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			component.HandleUnitEvent(new LightFuelRecoveryEvent
			{
				Amount = RandomUtility.GetRandomInt(recoveryEvent.AmountMin, recoveryEvent.AmountMax + 1)
			});
		}
	}

	private void HandleSoulRecoveryOwnerEvent(CollisionSoulRecoveryOnHitMonsterEvent recoveryEvent, GameObject target)
	{
		MonsterModel component = target.GetComponent<MonsterModel>();
		if (component == null)
		{
			return;
		}
		if (TMonster.Table.TryGetValue(component.MonsterId, out var value) && recoveryEvent.MonsterTypes != null)
		{
			if (!recoveryEvent.MonsterTypes.Contains(value.MonsterType) || component.State == EActionState.Dead || OwnerUnitController == null)
			{
				return;
			}
			OwnerUnitController.HandleUnitEvent(new SoulRecoveryEvent
			{
				Amount = RandomUtility.GetRandomInt(recoveryEvent.AmountMin, recoveryEvent.AmountMax + 1)
			});
		}
	}

	private void HandleForceMonsterSetTargetEvent(CollisionForceMonsterSetTargetEvent forceMonsterSetTargetEvent, GameObject target)
	{
		MonsterModel component = target.GetComponent<MonsterModel>();
		if (component == null)
		{
			return;
		}
		if (forceMonsterSetTargetEvent.MonsterCondition != EMonsterType.None)
		{
			if (!TMonster.Table.TryGetValue(component.TableId, out var value))
			{
				return;
			}
			if (value.MonsterType != forceMonsterSetTargetEvent.MonsterCondition)
			{
				return;
			}
		}
		UnitController component2 = target.GetComponent<UnitController>();
		if (!(component2 == null))
		{
			UnitModel aliveOwnerUnitModel = AliveOwnerUnitModel;
			if (!(aliveOwnerUnitModel == null))
			{
				component2.HandleUnitEvent(new ForceMonsterSetTargetEvent
				{
					TargetNetworkId = aliveOwnerUnitModel.NetworkObjectId
				});
			}
		}
	}

	private void HandleTryMonsterCuriousStateEvent(CollisionTryMonsterCuriousStateEvent tryCuriousStateEvent, GameObject target)
	{
		MonsterModel component = target.GetComponent<MonsterModel>();
		if (component == null)
		{
			return;
		}
		if (tryCuriousStateEvent.MonsterCondition != EMonsterType.None)
		{
			if (!TMonster.Table.TryGetValue(component.TableId, out var value))
			{
				return;
			}
			if (value.MonsterType != tryCuriousStateEvent.MonsterCondition)
			{
				return;
			}
		}
		UnitController component2 = target.GetComponent<UnitController>();
		if (!(component2 == null))
		{
			component2.HandleUnitEvent(new TryMonsterCuriousStateEvent
			{
				TargetPosition = base.transform.position
			});
		}
	}

	private void HandleCrowdControlEvent(CollisionCrowdControlEvent crowdControlEvent, GameObject target, Vector3 hitPosition)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (component == null)
		{
			return;
		}
		UnitModel aliveOwnerUnitModel = AliveOwnerUnitModel;
		Vector3 vector = Vector3.zero;
		float? forcedMoveDistance = null;
		if (crowdControlEvent.CcInfo.UseForcedMoveDirection)
		{
			switch (crowdControlEvent.CcInfo.ForcedMoveDirectionType)
			{
			case ECCforcedMoveDirectionType.OwnerAim:
				if (aliveOwnerUnitModel != null)
				{
					vector = new Vector3(aliveOwnerUnitModel.CalculatedDeltaAimPos.x, 0f, aliveOwnerUnitModel.CalculatedDeltaAimPos.z);
				}
				if (vector == Vector3.zero && aliveOwnerUnitModel != null)
				{
					vector = new Vector3(aliveOwnerUnitModel.Forward.x, 0f, aliveOwnerUnitModel.Forward.z);
				}
				break;
			case ECCforcedMoveDirectionType.OwnerMove:
				if (aliveOwnerUnitModel != null)
				{
					vector = new Vector3(aliveOwnerUnitModel.MoveDir.x, 0f, aliveOwnerUnitModel.MoveDir.z);
				}
				if (vector == Vector3.zero && aliveOwnerUnitModel != null)
				{
					vector = new Vector3(aliveOwnerUnitModel.CalculatedDeltaAimPos.x, 0f, aliveOwnerUnitModel.CalculatedDeltaAimPos.z);
				}
				break;
			case ECCforcedMoveDirectionType.HitBoxMove:
				vector = new Vector3(_normalizedDir.x, 0f, _normalizedDir.z);
				if (vector == Vector3.zero && aliveOwnerUnitModel != null)
				{
					vector = new Vector3(aliveOwnerUnitModel.CalculatedDeltaAimPos.x, 0f, aliveOwnerUnitModel.CalculatedDeltaAimPos.z);
				}
				break;
			case ECCforcedMoveDirectionType.MoveToHItBox:
				vector = new Vector3(base.transform.position.x - target.transform.position.x, 0f, base.transform.position.z - target.transform.position.z);
				forcedMoveDistance = Mathf.Min(vector.magnitude, crowdControlEvent.CcInfo.ccMoveDistance);
				break;
			default:
				vector = Vector3.zero;
				break;
			}
		}
		if (vector != Vector3.zero)
		{
			vector = vector.normalized;
		}
		component.HandleUnitEvent(new CrowdControlEvent
		{
			SourceModel = aliveOwnerUnitModel,
			CcInfo = crowdControlEvent.CcInfo,
			KnockbackHitPosition = hitPosition,
			ForcedMoveDirection = vector,
			ForcedMoveDistance = forcedMoveDistance
		});
	}

	private void HandleAcquireParadoxEvent(CollisionAcquireParadoxEvent acquireParadoxEvent, GameObject target)
	{
		if (target.GetComponent<UnitController>() == null || !RandomUtility.GetRandomBoolFromPercentage(acquireParadoxEvent.Probability))
		{
			return;
		}
		GamePlayer gamePlayer = _gameContext.Players[target.GetComponent<UnitController>().ClientId];
		if (acquireParadoxEvent.ParadoxId < 1)
		{
			gamePlayer.AcquireRandomParadox(acquireParadoxEvent.IncludePositive, acquireParadoxEvent.IncludeNegative);
		}
		else
		{
			gamePlayer.AcquireParadox(acquireParadoxEvent.ParadoxId);
		}
	}

	private void HandleRemoveParadoxEvent(CollisionRemoveParadoxEvent removeParadoxEvent, GameObject target)
	{
		if (target.GetComponent<UnitController>() == null || !RandomUtility.GetRandomBoolFromPercentage(removeParadoxEvent.Probability))
		{
			return;
		}
		GamePlayer gamePlayer = _gameContext.Players[target.GetComponent<UnitController>().ClientId];
		if (removeParadoxEvent.ParadoxId < 1)
		{
			gamePlayer.RemoveRandomParadox(removeParadoxEvent.IncludePositive, removeParadoxEvent.IncludeNegative);
		}
		else
		{
			gamePlayer.RemoveParadox(removeParadoxEvent.ParadoxId);
		}
	}

	private void OnTraceTargetEnter(Collider other)
	{
		if (CheckHitable(other, includeHighObstacle: false, HitBoxInfo.tracingTargetType, out var hitable))
		{
			_traceTarget = hitable;
		}
	}


	private void SpawnHitBoxOnCollision(int id)
	{
		bool isFlipped = _ownerUnitModel != null && _ownerUnitModel.GetComponent<UnitView>().IsFlipped;
		_objectManager.SpawnHitBoxAsync(id, _ownerUnitModel, _statsData, _hitLayerInfo, _deltaAimPos, base.transform.position, isFlipped, _skillId).Forget();
	}


	private void HandleSpawnMonster(CollisionSummonEvent summonEvent, Vector3 hitPosition)
	{
		if (_ownerUnitModel == null)
		{
			return;
		}
		Vector3 vector = MathUtility.LookRotationOrIdentity(_normalizedDir) * summonEvent.SummonPosOffset;
		_objectManager.TrySpawnMonster(summonEvent.MonsterInfos, hitPosition + vector, OwnerUnitNetworkObjectId);
	}

	private void SpawnExtraUnitOnDestroy(int extraUnitId)
	{
		_objectManager.TrySpawnExtraUnit(extraUnitId, base.transform.position, out var _);
	}


	private Vector3 CorrectAimDir(Vector3 originalDir)
	{
		if (_ownerUnitModel == null || originalDir.sqrMagnitude < 0.01f)
		{
			return originalDir;
		}
		PlaneSightUnit componentInChildren = _ownerUnitModel.GetComponentInChildren<PlaneSightUnit>();
		IReadOnlyCollection<MonsterModel> monstersInSight = _objectManager.GetMonstersInSight(componentInChildren);
		Vector3 from = originalDir.normalized;
		float num = float.MaxValue;
		Vector3? vector = null;
		foreach (MonsterModel item in monstersInSight)
		{
			if (item == null || !item.IsHitable)
			{
				continue;
			}
			Vector3 vector2 = new Vector3(item.Position.x - _executionPosition.x, 0f, item.Position.z - _executionPosition.z);
			float sqrMagnitude = vector2.sqrMagnitude;
			if (!(sqrMagnitude < 0.01f))
			{
				Vector3 normalized = vector2.normalized;
				if (Vector3.Angle(from, normalized) <= 10f && sqrMagnitude < num)
				{
					vector = vector2;
					num = sqrMagnitude;
				}
			}
		}
		if (vector.HasValue)
		{
			return vector.Value.normalized * originalDir.magnitude;
		}
		return originalDir;
	}





	private void DestroyHitBox()
	{
		AssertIfNotHost();
		if (_syncDespawnReason.Value != EHitBoxDespawnReason.None)
		{
			return;
		}
		if (HitBoxInfo.destroyHitBoxId != null && HitBoxInfo.destroyHitBoxId.Length != 0)
		{
			int[] destroyHitBoxId = HitBoxInfo.destroyHitBoxId;
			for (int i = 0; i < destroyHitBoxId.Length; i++)
			{
				SpawnHitBoxOnCollision(destroyHitBoxId[i]);
			}
		}
		if (HitBoxInfo.ExtraUnitIdOnDestroy > 0)
		{
			SpawnExtraUnitOnDestroy(HitBoxInfo.ExtraUnitIdOnDestroy);
		}
		HitLayerInfo hitLayerInfo = _hitLayerInfo;
		if (HitBoxInfo.ActionEventsOnDestroy != null)
		{
			HitBoxInfo.ActionEventsOnDestroy.ForEach(delegate(ActionEvent _)
			{
				Observable.Timer(TimeSpan.FromSeconds(_.startTime)).Subscribe(delegate
				{
					this.ExecuteAction(_, hitLayerInfo);
				}).AddTo(Disposable);
			});
		}
		_syncDespawnReason.Value = (_isDurationExpired ? EHitBoxDespawnReason.Timeout : EHitBoxDespawnReason.Destroy);
		base.gameObject.SetActive(value: false);
		ActionEvent[] actionEventsOnDestroy = HitBoxInfo.ActionEventsOnDestroy;
		float a = ((actionEventsOnDestroy == null) ? 0f : actionEventsOnDestroy.Select((ActionEvent _) => _.startTime + 0.1f).DefaultIfEmpty(0f).Max());
		float delay = Mathf.Max(a, 0.5f);
		Despawn(delay);
	}





	private void HandleSkillChargeEvent(CollisionSkillChargeEvent chargeEvent, GameObject target)
	{
		if (target.GetComponent<UnitModel>() == null)
		{
			return;
		}
		UnitController component = target.GetComponent<UnitController>();
		if (component == null)
		{
			return;
		}
		int randomInt = RandomUtility.GetRandomInt(chargeEvent.AmountMin, chargeEvent.AmountMax + 1);
		if (randomInt >= 1)
		{
			component.HandleUnitEvent(new SkillChargeEvent
			{
				Amount = randomInt,
				SourceModel = AliveOwnerUnitModel
			});
		}
	}

	private void HandleSpecialFieldEvent(CollisionSpecialFieldEvent specialFieldEvent, Vector3 hitPosition)
	{
		Vector3 normalized = (hitPosition - base.transform.position).normalized;
		Vector3 position = hitPosition + MathUtility.LookRotationOrIdentity(normalized) * specialFieldEvent.SpawnOffset;
		_objectManager.SpawnSpecialField(OwnerUnitNetworkObjectId, specialFieldEvent.SpecialFieldId, position, specialFieldEvent.Scale);
	}

	private void HandleShieldEvent(CollisionShieldEvent shieldEvent, GameObject target)
	{
		UnitModel component = target.GetComponent<UnitModel>();
		if (component == null)
		{
			return;
		}
		UnitController component2 = target.GetComponent<UnitController>();
		if (component2 == null || !RandomUtility.GetRandomBoolFromPercentage(shieldEvent.Probability))
		{
			return;
		}
		if (shieldEvent.FactionCondition != EMonsterCategory.None && !(component.MonsterCategories ?? Array.Empty<EMonsterCategory>()).Contains(shieldEvent.FactionCondition))
		{
			return;
		}
		if (shieldEvent.MonsterCondition != EMonsterType.None)
		{
			if (!(component is MonsterModel))
			{
				return;
			}
			if (!TMonster.Table.TryGetValue(component.TableId, out var value))
			{
				return;
			}
			if (value.MonsterType != shieldEvent.MonsterCondition)
			{
				return;
			}
		}
		UnitModel aliveOwnerUnitModel = AliveOwnerUnitModel;
		component2.HandleUnitEvent(new ShieldEvent
		{
			SourceModel = aliveOwnerUnitModel,
			ShieldId = shieldEvent.ShieldId,
			Amount = shieldEvent.Value,
			CasterId = ((aliveOwnerUnitModel == null) ? 0 : aliveOwnerUnitModel.NetworkObjectId)
		});
	}

	private void HandleAddSuppressionEvent(CollisionAddSuppressionEvent addSuppressionEvent, GameObject target)
	{
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			component.HandleUnitEvent(new SuppressionDamageEvent
			{
				Amount = addSuppressionEvent.Amount
			});
		}
	}

	private void HandleCaptureEvent(CollisionCaptureEvent captureEvent, GameObject target)
	{
		if (target.GetComponent<MonsterModel>() == null)
		{
			return;
		}
		UnitController component = target.GetComponent<UnitController>();
		if (!(component == null))
		{
			component.HandleUnitEvent(new CaptureEvent
			{
				HitPosition = base.transform.position
			});
		}
	}
}

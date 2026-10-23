using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using Spine;
using Spine.Unity;
using NemoLib;
using UniRx;
using System.Linq;
using GameCommon.Table;
using NemoLib.AssetManagement;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public abstract class UnitView : MonoBase
{
	protected enum UnitAnimationMode
	{
		None = 0,
		TwoWay = 1,
		FourWay = 2,
		EightWay = 3
	}

	[SerializeField]
	public NavMeshAgent Agent;

	[SerializeField]
	private UnitAnimationMode _animationMode;

	[SerializeField]
	protected SkeletonAnimation[] SkeletonAnimations;

	[SerializeField]
	protected UnitModel UnitModel;

	[SerializeField]
	private bool _isVisualDirectionLocked;

	[SerializeField]
	protected float SymbolPositionY = 2f;

	[SerializeField]
	public float StatusVfxScale = 1f;

	[SerializeField]
	protected CircleEffectIndicator CircleEffectIndicator;

	[SerializeField]
	protected CircleRangeIndicator CircleRangeIndicator;

	[SerializeField]
	protected LineTrajectoryIndicator LineTrajectoryIndicator;

	[SerializeField]
	protected ConeEffectIndicator ConeEffectIndicator;

	[SerializeField]
	protected MeshTrailVisualEffect SkeletonTrailVisualEffect;

	[SerializeField]
	protected UnitHighlightVisualEffect UnitHighlightVisualEffect;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected GameVfxManager VfxManager;

	[Inject]
	protected AssetManager AssetManager;

	[Inject]
	protected GameChannel GameChannel;

	[Inject]
	protected GameHost GameHost;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameFloatingTextManager FloatingTextManager;

	[Inject]
	protected GameObjectManager ObjectManager;

	private int _currentSkeletonIndex;

	private readonly HashSet<int> _multiSkeletonBuffIndices = new HashSet<int>();

	private Dictionary<EStatusEffectTag, StatusVfx> _statusVfxs = new Dictionary<EStatusEffectTag, StatusVfx>();

	private Dictionary<int, StatusVfx> _buffVfxs = new Dictionary<int, StatusVfx>();

	private EStatusEffectTag _statusEffectTags;

	private const string AIM_BONE = "aim_target";

	private List<Bone> _aimBones = new List<Bone>();

	private ObstacleAvoidanceType _defaultObstacleAvoidanceType;

	private Vector3 _lastDestination;

	private Vector3 _lastPositionOnSetDestination;

	private float _pathUpdateTimer;

	private CancellationTokenSource _animCancelToken;

	private int _currentFootstepCount;

	private const int MULTI_TRACK_INDEX = 1;

	private const string EMPTY_ANIMATION_NAME = "<empty>";

	public float Radius => Agent.radius;

	protected SkeletonAnimation CurrentSkeletonAnimation
	{
		get
		{
			if (_currentSkeletonIndex >= SkeletonAnimations.Length)
			{
				return null;
			}
			return SkeletonAnimations[_currentSkeletonIndex];
		}
	}

	private bool IsHost => UnitModel.IsHost;

	public Vector3 Forward => UnitModel.Forward;

	public Subject<EStatusEffectTag> OnStatusEffectTagAdded { get; } = new Subject<EStatusEffectTag>();

	public Subject<EStatusEffectTag> OnStatusEffectTagRemoved { get; } = new Subject<EStatusEffectTag>();

	public Subject<bool> OnModelAnchorActiveChanged { get; } = new Subject<bool>();

	public virtual bool IsFlipped
	{
		get
		{
			if (_currentSkeletonIndex < SkeletonAnimations.Length)
			{
				return SkeletonAnimations[_currentSkeletonIndex].skeleton.ScaleX < 0f;
			}
			return false;
		}
	}

	public virtual void Init()
	{
		InitAnimationVisible();
		UnitModel.RxBuffs.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<int, UnitBuff> _)
		{
			OnAddBuff(_.Key);
		}).AddTo(this);
		UnitModel.RxBuffs.ObserveReplace().Subscribe(delegate(DictionaryReplaceEvent<int, UnitBuff> _)
		{
			OnUpdateBuff(_.OldValue, _.NewValue);
		}).AddTo(this);
		UnitModel.RxBuffs.ObserveRemove().Subscribe(delegate(DictionaryRemoveEvent<int, UnitBuff> _)
		{
			OnRemoveBuff(_.Key);
		}).AddTo(this);
		UnitModel.RxCrowdControls.ObserveChange().Subscribe(delegate
		{
			OnUpdateCcVfx();
		}).AddTo(this);
		Agent.enabled = UnitModel.IsHost;
		Agent.stoppingDistance = TConst.AgentStoppingDistance.FloatValue;
		Agent.autoTraverseOffMeshLink = true;
		_defaultObstacleAvoidanceType = Agent.obstacleAvoidanceType;
		InitSkeleton();
		if (_isVisualDirectionLocked)
		{
			LookAtDirection();
		}
	}

	public void InitAgent()
	{
		Agent.enabled = UnitModel.IsHost;
		Agent.stoppingDistance = TConst.AgentStoppingDistance.FloatValue;
		Agent.autoTraverseOffMeshLink = true;
		_defaultObstacleAvoidanceType = Agent.obstacleAvoidanceType;
	}

	protected virtual void Start()
	{
		UnitModel.RxState.Subscribe(OnStateChanged).AddTo(this);
		UnitModel.RxUsingSkillId.Subscribe(OnUsingSkillIdChanged).AddTo(this);
		RegisterFloatingTextHandlers();
	}

	protected virtual void Update()
	{
		if (UnitModel.IsHost)
		{
			LookAtDirection();
			return;
		}
		if ((UnitModel.Position - Vector3.zero).sqrMagnitude < 1E-10f)
		{
			LookAtDirection();
			return;
		}
		if ((base.transform.position - UnitModel.Position).sqrMagnitude < 1E-10f)
		{
			LookAtDirection();
			return;
		}
		if (UnitModel.InterpolationSpeed < 0f)
		{
			base.transform.position = UnitModel.Position;
			LookAtDirection();
			return;
		}
		Vector3 vector = UnitModel.Position - base.transform.position;
		Vector3 vector2 = vector.normalized * UnitModel.InterpolationSpeed * Time.deltaTime;
		Vector3 vector3 = ((vector.magnitude <= vector2.magnitude) ? vector : vector2);
		base.transform.position = base.transform.position + vector3;
		LookAtDirection();
	}

	protected virtual void LateUpdate()
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (skeletonAnimation != null && GameManager.CharacterModels.Select((CharacterModel _) => _.transform.position).Any((Vector3 _) => Vector3.Distance(_, base.transform.position) <= 30f))
		{
			ForEachActiveSkeleton(delegate(SkeletonAnimation skeleton)
			{
				skeleton.Update(Time.deltaTime);
				skeleton.LateUpdate();
			});
		}
	}

	protected virtual void FixedUpdate()
	{
		if (UnitModel.IsHost && Agent.hasPath)
		{
			UpdatePosition(UnitModel.Position, base.transform.position, Agent.velocity.magnitude);
		}
	}

	public void Move(Vector3 direction, float speed, float deltaTime)
	{
		GameManager.AssertIfNotHost();
		Vector3 position = base.transform.position;
		Agent.Move(direction * (speed * deltaTime));
		UpdatePosition(position, base.transform.position, speed);
	}

	public void MoveWithNavMeshCheck(Vector3 direction, float speed, float deltaTime)
	{
		GameManager.AssertIfNotHost();
		Vector3 position = base.transform.position;
		Vector3 vector = direction * (speed * deltaTime);
		if (vector.magnitude > Agent.radius && NavMesh.Raycast(position, position + vector, out var hit, -1))
		{
			vector = hit.position - position;
		}
		Agent.Move(vector);
		UpdatePosition(position, base.transform.position, speed);
	}

	public void MoveTo(Vector3 destination)
	{
		GameManager.AssertIfNotHost();
		Vector3 position = base.transform.position;
		Agent.Warp(destination);
		UpdatePosition(position, base.transform.position);
	}

	public void UpdateAgentPath(Vector3 destination)
	{
		GameManager.AssertIfNotHost();
		_pathUpdateTimer += Time.fixedDeltaTime;
		Vector3 lastPositionOnSetDestination = _lastPositionOnSetDestination;
		Vector3 position = base.transform.position;
		if (Agent.pathStatus == NavMeshPathStatus.PathComplete && Vector3.Distance(lastPositionOnSetDestination, position) < 0.1f && 0.3f <= _pathUpdateTimer)
		{
			SetDestination();
			return;
		}
		float num = Vector3.Distance(_lastDestination, destination);
		if (Agent.hasPath && _pathUpdateTimer < 0.3f && num < 0.5f)
		{
			return;
		}
		SetDestination();
		void SetDestination()
		{
			Agent.speed = UnitModel.MoveSpeed;
			Agent.SetDestination(destination);
			_lastDestination = destination;
			_lastPositionOnSetDestination = base.transform.position;
			_pathUpdateTimer = 0f;
		}
	}

	public void ResetAgentPath()
	{
		if (Agent.enabled && Agent.isOnNavMesh)
		{
			Agent.ResetPath();
			return;
		}
		Logger.Warning(string.Format("[UnitView] ResetAgentPath 스킵 - {0}, enabled : {1}, isOnNavMesh : {2}", UnitModel, Agent.enabled, Agent.enabled && Agent.isOnNavMesh));
	}

	private void UpdatePosition(Vector3 oldPosition, Vector3 newPosition, float speed = -1f)
	{
		if (UnitModel.HasBuffEffect(EBuffEffectType.Root))
		{
			base.transform.position = oldPosition;
			UnitModel.Position = oldPosition;
			speed = -1f;
			UnitModel.InterpolationSpeed = speed;
			return;
		}
		IList<BuffEffectInfo<BindingFieldEffect>> buffEffects = UnitModel.GetBuffEffects<BindingFieldEffect>();
		foreach (BuffEffectInfo<BindingFieldEffect> item in buffEffects)
		{
			Vector3 point = item.Point;
			float num = item.BuffEffect.Range - Agent.radius;
			Vector3 vector = newPosition - point;
			if (num < vector.magnitude)
			{
				newPosition = point + vector.normalized * num;
			}
		}
		if (!((oldPosition - newPosition).sqrMagnitude < 1E-10f))
		{
			base.transform.position = newPosition;
			UnitModel.Position = newPosition;
			UnitModel.InterpolationSpeed = speed;
		}
	}

	public void SyncPosition(float speed = -1f)
	{
		UpdatePosition(UnitModel.Position, base.transform.position, speed);
	}

	private void OnAddBuff(int buffId)
	{
		UnitBuff unitBuff = UnitModel.Buffs[buffId];
		if (unitBuff == null || !unitBuff.IsActive || unitBuff.IsSuppressionBuff)
		{
			return;
		}
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.Trail))
		{
			UpdateTrailEffect();
		}
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.HighLight))
		{
			UpdateHighlightEffect();
		}
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.MultiSkeleton))
		{
			UpdateMultiSkeleton();
		}
		TryAddBuffVfx(buffId);
		EStatusEffectTag effectTag = TBuff.Table[buffId].EffectTag;
		if (effectTag != EStatusEffectTag.None)
		{
			AddStatusEffect(effectTag);
		}
	}

	private void OnUpdateBuff(UnitBuff oldBuff, UnitBuff newBuff)
	{
		if (newBuff == null || newBuff.IsSuppressionBuff || oldBuff == null)
		{
			return;
		}
		if (TBuff.Table[oldBuff.Id].BuffEffects != null && TBuff.Table[oldBuff.Id].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.Trail))
		{
			UpdateTrailEffect();
		}
		if (TBuff.Table[oldBuff.Id].BuffEffects != null && TBuff.Table[oldBuff.Id].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.HighLight))
		{
			UpdateHighlightEffect();
		}
		if (TBuff.Table[oldBuff.Id].BuffEffects != null && TBuff.Table[oldBuff.Id].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.MultiSkeleton))
		{
			UpdateMultiSkeleton();
		}
		if (!oldBuff.IsEffectActive && newBuff.IsEffectActive)
		{
			TryAddBuffVfx(newBuff.Id);
		}
		else if (oldBuff.IsEffectActive && !newBuff.IsEffectActive)
		{
			RemoveBuffVfx(newBuff.Id);
		}
		EStatusEffectTag effectTag = TBuff.Table[oldBuff.Id].EffectTag;
		if (effectTag != EStatusEffectTag.None)
		{
			if (!oldBuff.IsEffectActive && newBuff.IsEffectActive)
			{
				AddStatusEffect(effectTag);
			}
			else if (oldBuff.IsEffectActive && !newBuff.IsEffectActive)
			{
				RemoveStatusEffect(effectTag);
			}
		}
	}

	private void OnRemoveBuff(int buffId)
	{
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.Trail))
		{
			UpdateTrailEffect();
		}
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.HighLight))
		{
			UpdateHighlightEffect();
		}
		if (TBuff.Table[buffId].BuffEffects != null && TBuff.Table[buffId].BuffEffects.Any((BuffEffect _) => _.EffectType == EBuffEffectType.MultiSkeleton))
		{
			UpdateMultiSkeleton();
		}
		RemoveBuffVfx(buffId);
		EStatusEffectTag effectTag = TBuff.Table[buffId].EffectTag;
		if (effectTag != EStatusEffectTag.None)
		{
			RemoveStatusEffect(effectTag);
		}
	}

	private void UpdateTrailEffect()
	{
		IList<BuffEffectInfo<TrailBuffEffect>> buffEffects = UnitModel.GetBuffEffects<TrailBuffEffect>();
		if (buffEffects == null)
		{
			return;
		}
		if (buffEffects.Count == 0)
		{
			if (!(SkeletonTrailVisualEffect == null))
			{
				SkeletonTrailVisualEffect.SetActiveTrail(active: false);
			}
			return;
		}
		TrailBuffEffect buffEffect = buffEffects.MaxBy((BuffEffectInfo<TrailBuffEffect> _) => _.BuffEffect.TrailCount).BuffEffect;
		if (!(SkeletonTrailVisualEffect == null))
		{
			SkeletonTrailVisualEffect.SetTrailCount(buffEffect.TrailCount);
			SkeletonTrailVisualEffect.SetTrailPeriod(buffEffect.TrailPeriod);
			SkeletonTrailVisualEffect.SetTrailColor(buffEffect.StartColor, buffEffect.EndColor);
			SkeletonTrailVisualEffect.SetActiveTrail(active: true);
		}
	}

	private void UpdateMultiSkeleton()
	{
		IList<BuffEffectInfo<MultiSkeletonBuffEffect>> buffEffects = UnitModel.GetBuffEffects<MultiSkeletonBuffEffect>();
		HashSet<int> hashSet = new HashSet<int>();
		if (buffEffects == null)
		{
			return;
		}
		foreach (BuffEffectInfo<MultiSkeletonBuffEffect> item in buffEffects)
		{
			int[] additionalSkeletonIndices = item.BuffEffect.AdditionalSkeletonIndices;
			if (additionalSkeletonIndices == null)
			{
				continue;
			}
			for (int i = 0; i < additionalSkeletonIndices.Length; i++)
			{
				int num = additionalSkeletonIndices[i];
				if (num >= 0 && num < SkeletonAnimations.Length)
				{
					hashSet.Add(num);
				}
			}
		}
		foreach (int multiSkeletonBuffIndex in _multiSkeletonBuffIndices)
		{
			if (!hashSet.Contains(multiSkeletonBuffIndex) && multiSkeletonBuffIndex != _currentSkeletonIndex)
			{
				SkeletonAnimations[multiSkeletonBuffIndex].gameObject.SetActive(false);
			}
		}
		foreach (int item2 in hashSet)
		{
			if (!_multiSkeletonBuffIndices.Contains(item2) && item2 != _currentSkeletonIndex)
			{
				SkeletonAnimations[item2].gameObject.SetActive(true);
				SyncAnimationToSkeleton(item2);
			}
		}
		_multiSkeletonBuffIndices.Clear();
		foreach (int item3 in hashSet)
		{
			_multiSkeletonBuffIndices.Add(item3);
		}
	}

	private void SyncAnimationToSkeleton(int targetIndex)
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (skeletonAnimation == null)
		{
			return;
		}
		TrackEntry current = SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(0);
		if (current == null)
		{
			return;
		}
		SkeletonAnimation skeletonAnimation2 = SkeletonAnimations[targetIndex];
		skeletonAnimation2.timeScale = SkeletonAnimations[_currentSkeletonIndex].timeScale;
		if (current.Animation == null)
		{
			return;
		}
		TrackEntry trackEntry = skeletonAnimation2.state.SetAnimation(0, current.Animation.Name, current.Loop);
		if (trackEntry == null)
		{
			return;
		}
		trackEntry.MixBlend = MixBlend.Setup;
		trackEntry.MixDuration = 0f;
		trackEntry.TrackTime = current.TrackTime;
		TrackEntry current2 = SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(1);
		if (current2 != null && current2.Animation != null && !string.IsNullOrEmpty(current2.Animation.Name) && current2.Animation.Name != "<empty>")
		{
			TrackEntry trackEntry2 = skeletonAnimation2.state.SetAnimation(1, current2.Animation.Name, current2.Loop);
			if (trackEntry2 != null)
			{
				trackEntry2.MixDuration = 0f;
				trackEntry2.TrackTime = current2.TrackTime;
			}
		}
		else
		{
			skeletonAnimation2.state.SetEmptyAnimation(1, 0f);
		}
	}

	private void ForEachActiveSkeleton(Action<SkeletonAnimation> action)
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (skeletonAnimation == null)
		{
			return;
		}
		action(SkeletonAnimations[_currentSkeletonIndex]);
		foreach (int multiSkeletonBuffIndex in _multiSkeletonBuffIndices)
		{
			if (multiSkeletonBuffIndex != _currentSkeletonIndex && multiSkeletonBuffIndex >= 0 && multiSkeletonBuffIndex < SkeletonAnimations.Length)
			{
				action(SkeletonAnimations[multiSkeletonBuffIndex]);
			}
		}
	}

	private void UpdateHighlightEffect()
	{
		IList<BuffEffectInfo<HighlightBuffEffect>> buffEffects = UnitModel.GetBuffEffects<HighlightBuffEffect>();
		if (buffEffects == null)
		{
			return;
		}
		if (buffEffects.Count == 0)
		{
			if (!(UnitHighlightVisualEffect == null))
			{
				UnitHighlightVisualEffect.SetHighlightSilhouetteType(EUnitHighlightType.None);
				UnitHighlightVisualEffect.SetHighlightLineEnable(enabled: false);
			}
			return;
		}
		HighlightBuffEffect buffEffect = buffEffects.Last().BuffEffect;
		EUnitHighlightType highlightType = buffEffect.HighlightType;
		EUnitHighlightColor highlightColor = buffEffect.HighlightColor;
		if (!(UnitHighlightVisualEffect == null))
		{
			UnitHighlightVisualEffect.SetHighlightColorType(highlightColor);
			UnitHighlightVisualEffect.SetHighlightSilhouetteType(highlightType);
			UnitHighlightVisualEffect.SetHighlightLineEnable(enabled: true);
		}
	}

	private void AddStatusEffect(EStatusEffectTag effectTag)
	{
		TStatusEffectTag effectTagData = null;
		StatusVfx SpawnStatusVfx()
		{
			StatusVfx statusVfx = AssetManager.SpawnStatusVfx(effectTagData.Vfx);
			if (statusVfx == null)
			{
				return null;
			}
			float num = ((effectTagData.VfxBoneType == EUnitBoneType.Symbol) ? 1f : StatusVfxScale);
			if (effectTagData.VfxScaleByColliderRadius)
			{
				num = Mathf.Max(1f, UnitModel.HitRadius / 0.18f * 0.5f);
			}
			statusVfx.transform.localScale = Vector3.one * num;
			UnitFollower orAddComponent = statusVfx.gameObject.GetOrAddComponent<UnitFollower>();
			orAddComponent.Init(UnitModel, effectTagData.VfxBoneType, effectTagData.VfxOffset, false, true, false);
			return statusVfx;
		}
		OnStatusEffectTagAdded.OnNext(effectTag);
		if (effectTag == EStatusEffectTag.Freeze)
		{
			SetAnimationSpeed(0f);
		}
		if ((_statusEffectTags & effectTag) != effectTag)
		{
			_statusEffectTags |= effectTag;
		}
		effectTagData = TStatusEffectTag.Table[effectTag];
		if (effectTagData == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(effectTagData.Sfx))
		{
			SoundManager.PlaySfx3D(effectTagData.Sfx, base.transform);
		}
		if (string.IsNullOrEmpty(effectTagData.Vfx) || _statusVfxs.ContainsKey(effectTagData.Tag))
		{
			return;
		}
		if (effectTagData.VfxDuration < 0f)
		{
			if (!_statusVfxs.ContainsKey(effectTag))
			{
				StatusVfx statusVfx2 = SpawnStatusVfx();
				_statusVfxs.Add(effectTagData.Tag, statusVfx2);
				statusVfx2.GetComponent<UnitFollower>().OnDespawned.Subscribe(delegate
				{
					_statusVfxs.Remove(effectTag);
				}).AddTo(this);
			}
		}
		else
		{
			StatusVfx statusVfx3 = SpawnStatusVfx();
			statusVfx3.Despawn(effectTagData.VfxDuration);
		}
	}

	private void RemoveStatusEffect(EStatusEffectTag effectTag)
	{
		OnStatusEffectTagRemoved.OnNext(effectTag);
		TStatusEffectTag tStatusEffectTag = TStatusEffectTag.Table[effectTag];
		if (tStatusEffectTag == null || UnitModel.HasStatusEffectTag(tStatusEffectTag.Tag))
		{
			return;
		}
		if (effectTag == EStatusEffectTag.Freeze)
		{
			SetAnimationSpeed(1f);
			VfxManager.SpawnVfx("Status/Frozen_end", 2f, base.transform.position, Mathf.Max(1f, UnitModel.HitRadius / 0.18f * 0.5f), null, null, false, -1f);
			SoundManager.PlaySfx3D(TConst.FreezeEndSfx.StringValue, base.transform);
		}
		if ((_statusEffectTags & effectTag) == effectTag)
		{
			_statusEffectTags &= ~effectTag;
		}
		if (_statusVfxs.Remove(tStatusEffectTag.Tag, out var value))
		{
			value.Despawn();
		}
	}

	private float CalculateColliderRadiusVfxScale()
	{
		float num = UnitModel.HitRadius / 0.18f * 0.5f;
		if (!(1f <= num))
		{
			return 1f;
		}
		return num;
	}

	private void TryAddBuffVfx(int buffId)
	{
		UnitBuff unitBuff = UnitModel.Buffs[buffId];
		if (unitBuff == null || !unitBuff.IsActive || !TBuffVfx.Table.TryGetValue(buffId, out var value))
		{
			return;
		}
		if (!string.IsNullOrEmpty(value.Sfx))
		{
			SoundManager.PlaySfx3D(value.Sfx, base.transform);
		}
		if (string.IsNullOrEmpty(value.Vfx) || _buffVfxs.ContainsKey(buffId))
		{
			return;
		}
		StatusVfx statusVfx = AssetManager.SpawnStatusVfx(value.Vfx);
		if (statusVfx == null)
		{
			return;
		}
		float num = ((value.VfxBoneType == EUnitBoneType.Symbol) ? 1f : StatusVfxScale);
		statusVfx.transform.localScale = Vector3.one * num;
		UnitFollower orAddComponent = statusVfx.gameObject.GetOrAddComponent<UnitFollower>();
		orAddComponent.Init(UnitModel, value.VfxBoneType, value.VfxOffset, false, true, false);
		if (value.VfxDuration < 0f)
		{
			_buffVfxs.Add(buffId, statusVfx);
			orAddComponent.OnDespawned.Subscribe(delegate
			{
				if (_buffVfxs.TryGetValue(buffId, out var value2) && value2 == statusVfx)
				{
					_buffVfxs.Remove(buffId);
				}
			}).AddTo(this);
		}
		else
		{
			statusVfx.Despawn(value.VfxDuration);
		}
	}

	private void RemoveBuffVfx(int buffId)
	{
		if (_buffVfxs.Remove(buffId, out var value))
		{
			value.Despawn();
		}
	}

	private void OnUpdateCcVfx()
	{
		EStatusEffectTag ccStatusEffectTags = UnitModel.GetCcStatusEffectTags();
		foreach (EStatusEffectTag value in Enum.GetValues(typeof(EStatusEffectTag)))
		{
			if (value != EStatusEffectTag.None && (value & ccStatusEffectTags) == value && (_statusEffectTags & value) != value)
			{
				AddStatusEffect(value);
			}
		}
		foreach (EStatusEffectTag value2 in Enum.GetValues(typeof(EStatusEffectTag)))
		{
			if (value2 != EStatusEffectTag.None && (_statusEffectTags & value2) == value2 && (value2 & ccStatusEffectTags) != value2)
			{
				RemoveStatusEffect(value2);
			}
		}
	}

	public void SetNoObstacleAvoidanceType(bool noObstacleAvoidance)
	{
		ObstacleAvoidanceType obstacleAvoidanceType = (noObstacleAvoidance ? ObstacleAvoidanceType.NoObstacleAvoidance : _defaultObstacleAvoidanceType);
		if (Agent.obstacleAvoidanceType != obstacleAvoidanceType)
		{
			Agent.obstacleAvoidanceType = obstacleAvoidanceType;
		}
	}

	public virtual Vector3 GetBoneOffset(EUnitBoneType boneType)
	{
		switch (boneType)
		{
		case EUnitBoneType.None:
			return Vector3.zero;
		case EUnitBoneType.Symbol:
			return Vector3.up * SymbolPositionY;
		default:
		{
			SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
			if (!(skeletonAnimation == null))
			{
				Bone bone = SkeletonAnimations[_currentSkeletonIndex].Skeleton.FindBone("bone_" + boneType.ToString().ToLower());
				if (bone != null)
				{
					float x = CurrentSkeletonAnimation.transform.localScale.x;
					float y = CurrentSkeletonAnimation.transform.localPosition.y;
					Vector3 vector = Quaternion.Euler(30f, -45f, 0f) * new Vector3(x * bone.WorldX, x * bone.WorldY, 0f);
					return vector + Vector3.up * y;
				}
			}
			return Vector3.zero;
		}
		}
	}

	protected virtual bool GetIsVisible(EActionState state)
	{
		if (state == EActionState.StageReady)
		{
			return false;
		}
		return state != EActionState.Cutscene;
	}

	protected virtual void OnStateChanged(EActionState state)
	{
		bool isVisible = GetIsVisible(state);
		OnModelAnchorActiveChanged.OnNext(isVisible);
		SetStatusVfxVisible(isVisible);
		if (UnitModel.IsHost)
		{
			Agent.enabled = UnitModel.IsHitable;
		}
		if (state == EActionState.Dead)
		{
			Vector3 boneOffset = GetBoneOffset(EUnitBoneType.Death);
			if (UnitModel.BodyObject != null)
			{
				UnitModel.BodyObject.transform.localPosition = boneOffset;
			}
		}
		else if (state == EActionState.Revive && UnitModel.BodyObject != null)
		{
			UnitModel.BodyObject.transform.localPosition = Vector3.zero;
		}
	}

	private void SetStatusVfxVisible(bool visible)
	{
		foreach (StatusVfx value in _statusVfxs.Values)
		{
			value.SetVisible(visible);
		}
		foreach (StatusVfx value2 in _buffVfxs.Values)
		{
			value2.SetVisible(visible);
		}
	}

	protected abstract void PlayAnimationByActionState(EActionState state);

	protected virtual void OnUsingSkillIdChanged(int skillId)
	{
	}

	public void SetTrailEffect(int trailCount, float trailPeriod, Color startColor, Color endColor)
	{
		if (!(SkeletonTrailVisualEffect == null))
		{
			SkeletonTrailVisualEffect.SetTrailCount(trailCount);
			SkeletonTrailVisualEffect.SetTrailPeriod(trailPeriod);
			SkeletonTrailVisualEffect.SetTrailColor(startColor, endColor);
			SkeletonTrailVisualEffect.SetActiveTrail(active: true);
		}
	}

	public void ClearTrail()
	{
		if (!(SkeletonTrailVisualEffect == null))
		{
			SkeletonTrailVisualEffect.SetActiveTrail(active: false);
		}
	}

	private void SetHighlightEffect(EUnitHighlightType type, EUnitHighlightColor color)
	{
		if (!(UnitHighlightVisualEffect == null))
		{
			UnitHighlightVisualEffect.SetHighlightColorType(color);
			UnitHighlightVisualEffect.SetHighlightSilhouetteType(type);
			UnitHighlightVisualEffect.SetHighlightLineEnable(enabled: true);
		}
	}

	private void ClearHighlightEffect()
	{
		if (!(UnitHighlightVisualEffect == null))
		{
			UnitHighlightVisualEffect.SetHighlightSilhouetteType(EUnitHighlightType.None);
			UnitHighlightVisualEffect.SetHighlightLineEnable(enabled: false);
		}
	}

	protected virtual string ResolveFootstepSound(bool isLeft)
	{
		EGroundType groundType = BackgroundTileTypeCore.GetGroundType(UnitModel.Position);
		return GetFootstepSfx(groundType, isLeft) ?? string.Empty;
	}

	protected abstract string GetFootstepSfx(EGroundType groundType, bool isLeft);

	protected virtual void InitAnimationVisible()
	{
		SkeletonAnimation[] skeletonAnimations = SkeletonAnimations;
		for (int i = 0; i < skeletonAnimations.Length; i++)
		{
			SkeletonAnimation skeletonAnimation = skeletonAnimations[i];
			float initialDelay = UnityEngine.Random.Range(0f, 2f);
			float scanInterval = 3f;
			Renderer renderer = skeletonAnimation.gameObject.GetComponent<Renderer>();
			if (renderer == null)
			{
				Logger.Error("renderer is null: " + skeletonAnimation.gameObject.name);
				return;
			}
			Observable.Timer(TimeSpan.FromSeconds(initialDelay)).SelectMany((long _) => Observable.Interval(TimeSpan.FromSeconds(scanInterval))).Subscribe(delegate
			{
				skeletonAnimation.enabled = IsRendererVisibleFrom(renderer, Camera.main);
			}).AddTo(this);
		}
		static bool IsRendererVisibleFrom(Renderer renderer, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}

	protected virtual void InitSkeleton()
	{
		SkeletonAnimation[] skeletonAnimations = SkeletonAnimations;
		foreach (SkeletonAnimation skeletonAnimation in skeletonAnimations)
		{
			SkeletonAnimation skeletonAnimation2 = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
			skeletonAnimation.gameObject.SetActive(skeletonAnimation2 == skeletonAnimation);
			Bone bone = skeletonAnimation.Skeleton.FindBone("aim_target");
			if (bone != null)
			{
				_aimBones.Add(bone);
				skeletonAnimation.UpdateWorld += OnUpdateWorld;
				skeletonAnimation.state.Event -= OnSpineEvent;
				skeletonAnimation.state.Event += OnSpineEvent;
			}
		}
	}

	private void OnUpdateWorld(ISkeletonAnimation animated)
	{
		if (_currentSkeletonIndex < SkeletonAnimations.Length)
		{
			float scaleX = SkeletonAnimations[_currentSkeletonIndex].skeleton.ScaleX;
			Vector2 vector = MathUtility.IsometricToTopDown(UnitModel.CalculatedDeltaAimPos);
			vector.x *= scaleX;
			Vector2 normalized = vector.normalized;
			foreach (Bone aimBone in _aimBones)
			{
				aimBone.SetLocalPosition(normalized);
			}
		}
	}

	public virtual void UpdateAnimation(string animationName, string beforeAnimationName)
	{
		if (_currentSkeletonIndex >= SkeletonAnimations.Length)
		{
			return;
		}
		SkeletonData data = SkeletonAnimations[_currentSkeletonIndex].Skeleton.Data;
		if (animationName.HasValue() && data.FindAnimation(animationName) != null)
		{
			TrackEntry current = SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(0);
			if (current == null || current.Animation == null)
			{
				return;
			}
			if (current.Animation.Name != beforeAnimationName)
			{
				return;
			}
			float trackTime = current.TrackTime;
			bool loop = current.Loop;
			TrackEntry trackEntry = CurrentSkeletonAnimation.state.SetAnimation(0, animationName, loop);
			if (trackEntry != null)
			{
				trackEntry.TrackTime = trackTime;
				trackEntry.MixBlend = MixBlend.Setup;
				SyncAnimationToNonPrimarySkeletons();
			}
		}
		else
		{
			Logger.Warning("Animation '" + animationName + "' not found in skeleton '" + data.Name + "'.");
		}
	}

	public virtual async UniTask PlayAnimationAsync(string animationName, bool isLoop, float animStartOffsetTime, AnimationSpeed[] animSpeeds, float defaultSpeed = 1f, string multiTrackAnimationName = null)
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (skeletonAnimation == null)
		{
			return;
		}
		if (_animCancelToken != null)
		{
			_animCancelToken.Cancel();
			_animCancelToken.Dispose();
			_animCancelToken = null;
		}
		ClearMultiTrackAnimation();
		SetAnimationSpeed(defaultSpeed);
		if (!string.IsNullOrEmpty(multiTrackAnimationName) && multiTrackAnimationName != "<empty>")
		{
			CurrentSkeletonAnimation.state.SetAnimation(1, multiTrackAnimationName, isLoop).TrackTime = animStartOffsetTime;
		}
		if (animSpeeds != null && animSpeeds.Length != 0)
		{
			_animCancelToken = new CancellationTokenSource();
			while (true)
			{
				SkeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, animationName, false);
				SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(0).TrackTime = animStartOffsetTime;
				SyncAnimationToNonPrimarySkeletons();
				for (int i = 0; i < animSpeeds.Length; i++)
				{
					if (!(animSpeeds[i].speed > 0f))
					{
						continue;
					}
					SetAnimationSpeed(animSpeeds[i].speed);
					TrackEntry current = CurrentSkeletonAnimation.state.GetCurrent(0);
					if (current == null)
					{
						break;
					}
					SyncAnimationToNonPrimarySkeletons();
					float num = (current.AnimationEnd - current.TrackTime) / animSpeeds[i].speed;
					float num2 = ((i >= 1) ? animSpeeds[i - 1].endTime : 0f);
					float endTime = animSpeeds[i].endTime;
					if (current.TrackTime < endTime)
					{
						if (0f <= endTime)
						{
							float num3 = (endTime - num2) / animSpeeds[i].speed;
							if (num3 < num)
							{
								num = num3;
							}
						}
						if (0f < num)
						{
							await UniTask.Delay(TimeSpan.FromSeconds(num), false, PlayerLoopTiming.Update, _animCancelToken.Token);
						}
					}
				}
				if (!isLoop)
				{
					break;
				}
				animationName = SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(0).Animation.Name;
			}
		}
		else
		{
			SkeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, animationName, isLoop);
			SkeletonAnimations[_currentSkeletonIndex].state.GetCurrent(0).TrackTime = animStartOffsetTime;
			SyncAnimationToNonPrimarySkeletons();
		}
	}

	public void ClearMultiTrackAnimation()
	{
		ForEachActiveSkeleton(delegate(SkeletonAnimation skeleton)
		{
			skeleton.state.SetEmptyAnimation(1, 0f);
		});
	}

	private void OnSpineEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		if (!(name == "sfx_run_L") && !(name == "sfx_run_R") && !(name == "sfx_walk_L") && !(name == "sfx_walk_R"))
		{
			return;
		}
		_currentFootstepCount++;
		if (_currentFootstepCount >= 2)
		{
			string name2 = e.Data.Name;
			string text = ResolveFootstepSound(name2 == "sfx_run_L" || name2 == "sfx_walk_L");
			if (text.HasValue())
			{
				SoundManager.PlaySfx3D(text, UnitModel.transform);
			}
		}
	}

	protected virtual void ChangeSkin(string skinName)
	{
		SkeletonAnimation[] skeletonAnimations = SkeletonAnimations;
		for (int i = 0; i < skeletonAnimations.Length; i++)
		{
			Skeleton skeleton = skeletonAnimations[i].Skeleton;
			if (skeleton.Skin != null && skeleton.Skin.Name.Equals(skinName))
			{
				return;
			}
			if (skeleton.Data.FindSkin(skinName) == null)
			{
				Logger.Warning("Skin not found: " + skinName);
				return;
			}
			skeleton.SetSkin(skinName);
			skeleton.SetSlotsToSetupPose();
		}
	}

	protected virtual void LookAtDirection()
	{
		if (_isVisualDirectionLocked || (UnitModel.Forward - Vector3.zero).sqrMagnitude < 1E-10f)
		{
			return;
		}
		switch (_animationMode)
		{
		case UnitAnimationMode.TwoWay:
			LookAtDirectionAsTwoWay();
			break;
		case UnitAnimationMode.FourWay:
			LookAtDirectionAsFourWay();
			break;
		case UnitAnimationMode.EightWay:
			LookAtDirectionAsEightWay();
			break;
		}
	}

	private void LookAtDirectionAsTwoWay()
	{
		Vector2 vector = MathUtility.IsometricToTopDown(UnitModel.Forward);
		if (vector.x > Mathf.Epsilon)
		{
			SetActiveSkeletonScaleX(-1f);
		}
		else if (vector.x < 0f - Mathf.Epsilon)
		{
			SetActiveSkeletonScaleX(1f);
		}
	}

	private void LookAtDirectionAsFourWay()
	{
		Vector2 vector = MathUtility.IsometricToTopDown(UnitModel.Forward);
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		int num2;
		if (0f <= num && num < 90f)
		{
			UpdateSkeletonIndex(0);
			SetActiveSkeletonScaleX(-1f);
			return;
		}
		if (num < 90f || 180f <= num)
		{
			if (num < -180f || -90f <= num)
			{
				if (num < -90f || 0f <= num)
				{
					return;
				}
				UpdateSkeletonIndex(1);
				SetActiveSkeletonScaleX(-1f);
				return;
			}
			num2 = 1;
		}
		else
		{
			num2 = 0;
		}
		UpdateSkeletonIndex(num2);
		SetActiveSkeletonScaleX(1f);
	}

	private void LookAtDirectionAsEightWay()
	{
		Vector2 vector = MathUtility.IsometricToTopDown(UnitModel.Forward);
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		int num2;
		if (num < -22.5f || 22.5f <= num)
		{
			if (num < 22.5f || 67.5f <= num)
			{
				if (num < 67.5f || 112.5f <= num)
				{
					if (num < 112.5f || 157.5f <= num)
					{
						if (157.5f <= num || num < -157.5f)
						{
							num2 = 3;
						}
						else
						{
							if (num < -157.5f || -112.5f <= num)
							{
								if (num < -112.5f || -67.5f <= num)
								{
									if (num < -67.5f || -22.5f <= num)
									{
										return;
									}
									num2 = 1;
								}
								else
								{
									num2 = 4;
								}
								UpdateSkeletonIndex(num2);
								SetActiveSkeletonScaleX(1f);
								return;
							}
							num2 = 1;
						}
					}
					else
					{
						num2 = 0;
					}
					UpdateSkeletonIndex(num2);
					SetActiveSkeletonScaleX(-1f);
					return;
				}
				num2 = 2;
			}
			else
			{
				num2 = 0;
			}
		}
		else
		{
			num2 = 3;
		}
		UpdateSkeletonIndex(num2);
		SetActiveSkeletonScaleX(1f);
	}

	private void UpdateSkeletonIndex(int index)
	{
		if (index == _currentSkeletonIndex)
		{
			return;
		}
		SkeletonAnimation skeletonAnimation = SkeletonAnimations[_currentSkeletonIndex];
		TrackEntry current = skeletonAnimation.state.GetCurrent(0);
		string text = null;
		float num = 0f;
		bool flag = false;
		if (current != null)
		{
			text = current.Animation.Name;
			num = current.TrackTime;
			flag = current.Loop;
		}
		float timeScale = skeletonAnimation.timeScale;
		TrackEntry current2 = skeletonAnimation.state.GetCurrent(1);
		string text2 = null;
		float num2 = 0f;
		bool flag2 = false;
		if (current2 != null)
		{
			text2 = ((current2.Animation != null) ? current2.Animation.Name : null);
			num2 = current2.TrackTime;
			flag2 = current2.Loop;
		}
		int currentSkeletonIndex = _currentSkeletonIndex;
		HashSet<int> multiSkeletonBuffIndices = _multiSkeletonBuffIndices;
		_currentSkeletonIndex = index;
		_currentFootstepCount = 0;
		if (!multiSkeletonBuffIndices.Contains(currentSkeletonIndex))
		{
			skeletonAnimation.gameObject.SetActive(false);
		}
		SkeletonAnimations[_currentSkeletonIndex].gameObject.SetActive(true);
		SkeletonAnimations[_currentSkeletonIndex].timeScale = timeScale;
		if (text != null)
		{
			TrackEntry trackEntry = SkeletonAnimations[_currentSkeletonIndex].state.SetAnimation(0, text, flag);
			if (trackEntry == null)
			{
				return;
			}
			trackEntry.TrackTime = num;
			trackEntry.MixBlend = MixBlend.Setup;
		}
		if (!string.IsNullOrEmpty(text2) && text2 != "<empty>")
		{
			TrackEntry trackEntry2 = SkeletonAnimations[_currentSkeletonIndex].state.SetAnimation(1, text2, flag2);
			if (trackEntry2 != null)
			{
				trackEntry2.TrackTime = num2;
				trackEntry2.MixDuration = 0f;
				SyncAnimationToNonPrimarySkeletons();
			}
		}
		else
		{
			SkeletonAnimations[_currentSkeletonIndex].state.SetEmptyAnimation(1, 0f);
			SyncAnimationToNonPrimarySkeletons();
		}
	}

	public virtual void SetAnimationSpeed(float speed)
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (!(skeletonAnimation == null))
		{
			ForEachActiveSkeleton(delegate(SkeletonAnimation skeleton)
			{
				skeleton.timeScale = speed;
			});
		}
	}

	private void SetActiveSkeletonScaleX(float scaleX)
	{
		ForEachActiveSkeleton(delegate(SkeletonAnimation skeleton)
		{
			skeleton.skeleton.ScaleX = scaleX;
		});
	}

	private void SyncAnimationToNonPrimarySkeletons()
	{
		foreach (int multiSkeletonBuffIndex in _multiSkeletonBuffIndices)
		{
			if (multiSkeletonBuffIndex != _currentSkeletonIndex)
			{
				SyncAnimationToSkeleton(multiSkeletonBuffIndex);
			}
		}
	}

	public virtual float GetAnimationDuration(string animationName)
	{
		SkeletonAnimation skeletonAnimation = ((_currentSkeletonIndex < SkeletonAnimations.Length) ? SkeletonAnimations[_currentSkeletonIndex] : null);
		if (skeletonAnimation == null)
		{
			return 0f;
		}
		if (_currentSkeletonIndex < SkeletonAnimations.Length)
		{
			Spine.Animation animation = SkeletonAnimations[_currentSkeletonIndex].skeleton.Data.FindAnimation(animationName);
			if (animation == null)
			{
				return 0f;
			}
			return animation.Duration;
		}
		return 0f;
	}

	public abstract string GetIdleAnimation();

	public abstract string GetMoveAnimation();

	public abstract string GetDeathAnimation();

	public abstract string GetReviveAnimation();

	public abstract float GetMoveAnimationSpeed();

	private void RegisterFloatingTextHandlers()
	{
		GameChannel.Receive<NtfHpDamage>().Where((NtfHpDamage _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnDamageText).AddTo(this);
		GameChannel.Receive<NtfHpRecovery>().Where((NtfHpRecovery _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnRecoveryText).AddTo(this);
		GameChannel.Receive<NtfStressDamage>().Where((NtfStressDamage _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnStressDamageText).AddTo(this);
		GameChannel.Receive<NtfStressRecovery>().Where((NtfStressRecovery _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnStressRecoveryText).AddTo(this);
		GameChannel.Receive<NtfOnHitInvincibility>().Where((NtfOnHitInvincibility _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnInvincibilityText).AddTo(this);
		GameChannel.Receive<NtfOnFreezeFireDamage>().Where((NtfOnFreezeFireDamage _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(delegate
		{
			OnSpawnFreezeFireDamageText();
		}).AddTo(this);
		OnStatusEffectTagAdded.Subscribe(OnSpawnStatusEffectTagText).AddTo(this);
		GameChannel.Receive<NtfOnImmuneStatusEffect>().Where((NtfOnImmuneStatusEffect _) => _ != null && UnitModel != null && _.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnSpawnStatusEffectTagImmuneText).AddTo(this);
		UnitModel.RxBuffs.ObserveAdd().Where((DictionaryAddEvent<int, UnitBuff> _) => _.Value != null && TBuff.Table[_.Value.Id].ShowActiveText).Where((DictionaryAddEvent<int, UnitBuff> _) => _.Value.IsEffectActive).Subscribe(delegate(DictionaryAddEvent<int, UnitBuff> _)
		{
			OnSpawnBuffActiveText(_.Value.Id);
		})
			.AddTo(this);
		UnitModel.RxBuffs.ObserveReplace().Where((DictionaryReplaceEvent<int, UnitBuff> _) => _.NewValue != null && TBuff.Table[_.NewValue.Id].ShowActiveText).Where((DictionaryReplaceEvent<int, UnitBuff> _) => _.OldValue != null && !_.OldValue.IsEffectActive && _.NewValue.IsEffectActive).Subscribe(delegate(DictionaryReplaceEvent<int, UnitBuff> _)
		{
			OnSpawnBuffActiveText(_.NewValue.Id);
		})
			.AddTo(this);
	}

	private void OnSpawnDamageText(NtfHpDamage message)
	{
		if (message.Amount < 0)
		{
			return;
		}
		Vector3 position = message.HitPosition.ToVector3();
		MonsterModel monsterModel = UnitModel as MonsterModel;
		bool isPenalty = message.ElementalDamagedType.IsPenalty();
		bool isResistance = message.ElementalDamagedType.IsResistance();
		bool isCritical = message.IsCritical;
		EFloatingTextType floatingTextType = GetFloatingTextType(monsterModel != null, isCritical, isPenalty, isResistance);
		FloatingTextManager.SpawnText(floatingTextType, message.Amount.ToString(), position, false, isCritical);
		if (message.IsBackAttack)
		{
			FloatingTextManager.SpawnText(EFloatingTextType.BackAttackText, TLocalizedText.FloatingTextBackAttack.LocalText, position);
		}
	}

	private EFloatingTextType GetFloatingTextType(bool isMonster, bool isCritical, bool isPenalty, bool isResistance)
	{
		if (!isMonster)
		{
			if (isCritical)
			{
				return isResistance ? EFloatingTextType.HpCriticalDamageElementalResistanceText : EFloatingTextType.HpCriticalDamageText;
			}
			return isResistance ? EFloatingTextType.HpDamageElementalResistanceText : EFloatingTextType.HpDamageText;
		}
		if (isCritical)
		{
			if (isPenalty)
			{
				return EFloatingTextType.HpWeakPointCriticalDamageText;
			}
			return isResistance ? EFloatingTextType.HpCriticalDamageElementalResistanceText : EFloatingTextType.HpCriticalDamageText;
		}
		if (isPenalty)
		{
			return EFloatingTextType.HpWeakPointDamageText;
		}
		return isResistance ? EFloatingTextType.HpDamageElementalResistanceText : EFloatingTextType.HpDamageText;
	}

	private void OnSpawnRecoveryText(NtfHpRecovery message)
	{
		if (message.Amount < 0 || message.ReasonType == EHpRecoveryReasonType.Regen)
		{
			return;
		}
		UnitModel unitModel = ObjectManager.FindUnitModel(message.NetworkObjectId);
		if (unitModel == null)
		{
			return;
		}
		Vector3 position = unitModel.Position;
		string text = string.Format("+{0}", message.Amount);
		FloatingTextManager.SpawnText(EFloatingTextType.HpRecoveryText, text, position);
		if (message.ReasonType == EHpRecoveryReasonType.DamageConversion)
		{
			SoundManager.PlaySfx3D("ConsumeItem_Heal", base.transform);
			GameObject gameObject = VfxManager.SpawnVfx("Common/Heal_Middle", 1f, base.transform.position, 1f, null, null, false, -1f);
			UnitFollower orAddComponent = gameObject.GetOrAddComponent<UnitFollower>();
			orAddComponent.Init(UnitModel, EUnitBoneType.Body, Vector3.zero, false, false, false);
			FloatingTextManager.SpawnText(EFloatingTextType.StatusEffectText, TLocalizedText.ElementAbsorption.LocalText, position);
		}
	}

	private void OnSpawnStressDamageText(NtfStressDamage message)
	{
		int num = (int)message.Amount;
		if (0 < num)
		{
			Vector3 position = message.HitPosition.ToVector3();
			FloatingTextManager.SpawnText(EFloatingTextType.StressDamageText, string.Format("{0}", num), position);
		}
	}

	private void OnSpawnStressRecoveryText(NtfStressRecovery message)
	{
		int num = (int)message.Amount;
		if (0 < num)
		{
			UnitModel unitModel = ObjectManager.FindUnitModel(message.NetworkObjectId);
			if (!(unitModel == null))
			{
				Vector3 position = unitModel.Position;
				FloatingTextManager.SpawnText(EFloatingTextType.StressRecoveryText, string.Format("+{0}", num), position);
			}
		}
	}

	private void OnSpawnInvincibilityText(NtfOnHitInvincibility message)
	{
		Vector3 position = UnitModel.Position;
		FloatingTextManager.SpawnText(EFloatingTextType.StatusEffectText, TLocalizedText.Invincible.LocalText, position, true);
	}

	private void OnSpawnFreezeFireDamageText()
	{
		Vector3 position = UnitModel.Position;
		FloatingTextManager.SpawnText(EFloatingTextType.StatusEffectText, TLocalizedText.FreezeFireDamageFloatingText.LocalText, position);
		SoundManager.PlaySfx3D(TConst.FireDamageFreezeDurationReductionSfx.StringValue, base.transform);
	}

	private void OnSpawnStatusEffectTagText(EStatusEffectTag effectTag)
	{
		string text = effectTag.ToLocalText();
		Vector3 position = UnitModel.Position;
		if (!string.IsNullOrEmpty(text))
		{
			FloatingTextManager.SpawnText(EFloatingTextType.StatusEffectText, text, position);
		}
	}

	private void OnSpawnStatusEffectTagImmuneText(NtfOnImmuneStatusEffect message)
	{
		string text = message.StatusEffectTag.ToLocalText();
		string text2 = ((!message.IsFullyImmune) ? TLocalizedText.StatusEffectImmuneFormat.Format(text) : TLocalizedText.FullyImmuneText.LocalText);
		Vector3 position = UnitModel.Position;
		if (!string.IsNullOrEmpty(text2))
		{
			FloatingTextManager.SpawnText(EFloatingTextType.StatusEffectText, text2, position);
		}
	}

	private void OnSpawnBuffActiveText(int buffId)
	{
		FloatingTextManager.SpawnText(EFloatingTextType.BuffActiveText, TBuff.Table[buffId].Name, UnitModel.Position);
	}
}

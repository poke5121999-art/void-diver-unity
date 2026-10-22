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
using UniRx;
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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlayAnimationAsync_003Ed__101 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public UnitView _003C_003E4__this;

		public float defaultSpeed;

		public string multiTrackAnimationName;

		public bool isLoop;

		public float animStartOffsetTime;

		public AnimationSpeed[] animSpeeds;

		public string animationName;

		private int _003Ci_003E5__2;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
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
	protected float SymbolPositionY;

	[SerializeField]
	public float StatusVfxScale;

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

	private readonly HashSet<int> _multiSkeletonBuffIndices;

	private Dictionary<EStatusEffectTag, StatusVfx> _statusVfxs;

	private Dictionary<int, StatusVfx> _buffVfxs;

	private EStatusEffectTag _statusEffectTags;

	private const string AIM_BONE = "aim_target";

	private List<Bone> _aimBones;

	private ObstacleAvoidanceType _defaultObstacleAvoidanceType;

	private Vector3 _lastDestination;

	private Vector3 _lastPositionOnSetDestination;

	private float _pathUpdateTimer;

	private CancellationTokenSource _animCancelToken;

	private int _currentFootstepCount;

	private const int MULTI_TRACK_INDEX = 1;

	private const string EMPTY_ANIMATION_NAME = "<empty>";

	public float Radius => 0f;

	protected SkeletonAnimation CurrentSkeletonAnimation => null;

	private bool IsHost => false;

	public Vector3 Forward => default;

	public Subject<EStatusEffectTag> OnStatusEffectTagAdded { get; }

	public Subject<EStatusEffectTag> OnStatusEffectTagRemoved { get; }

	public Subject<bool> OnModelAnchorActiveChanged { get; }

	public virtual bool IsFlipped => false;

	public virtual void Init()
	{
	}

	public void InitAgent()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void LateUpdate()
	{
	}

	protected virtual void FixedUpdate()
	{
	}

	public void Move(Vector3 direction, float speed, float deltaTime)
	{
	}

	public void MoveWithNavMeshCheck(Vector3 direction, float speed, float deltaTime)
	{
	}

	public void MoveTo(Vector3 destination)
	{
	}

	public void UpdateAgentPath(Vector3 destination)
	{
	}

	public void ResetAgentPath()
	{
	}

	private void UpdatePosition(Vector3 oldPosition, Vector3 newPosition, float speed = -1f)
	{
	}

	public void SyncPosition(float speed = -1f)
	{
	}

	private void OnAddBuff(int buffId)
	{
	}

	private void OnUpdateBuff(UnitBuff oldBuff, UnitBuff newBuff)
	{
	}

	private void OnRemoveBuff(int buffId)
	{
	}

	private void UpdateTrailEffect()
	{
	}

	private void UpdateMultiSkeleton()
	{
	}

	private void SyncAnimationToSkeleton(int targetIndex)
	{
	}

	private void ForEachActiveSkeleton(Action<SkeletonAnimation> action)
	{
	}

	private void UpdateHighlightEffect()
	{
	}

	private void AddStatusEffect(EStatusEffectTag effectTag)
	{
	}

	private void RemoveStatusEffect(EStatusEffectTag effectTag)
	{
	}

	private float CalculateColliderRadiusVfxScale()
	{
		return 0f;
	}

	private void TryAddBuffVfx(int buffId)
	{
	}

	private void RemoveBuffVfx(int buffId)
	{
	}

	private void OnUpdateCcVfx()
	{
	}

	public void SetNoObstacleAvoidanceType(bool noObstacleAvoidance)
	{
	}

	public virtual Vector3 GetBoneOffset(EUnitBoneType boneType)
	{
		return default;
	}

	protected virtual bool GetIsVisible(EActionState state)
	{
		return false;
	}

	protected virtual void OnStateChanged(EActionState state)
	{
	}

	private void SetStatusVfxVisible(bool visible)
	{
	}

	protected abstract void PlayAnimationByActionState(EActionState state);

	protected virtual void OnUsingSkillIdChanged(int skillId)
	{
	}

	public void SetTrailEffect(int trailCount, float trailPeriod, Color startColor, Color endColor)
	{
	}

	public void ClearTrail()
	{
	}

	private void SetHighlightEffect(EUnitHighlightType type, EUnitHighlightColor color)
	{
	}

	private void ClearHighlightEffect()
	{
	}

	protected virtual string ResolveFootstepSound(bool isLeft)
	{
		return null;
	}

	protected abstract string GetFootstepSfx(EGroundType groundType, bool isLeft);

	protected virtual void InitAnimationVisible()
	{
	}

	protected virtual void InitSkeleton()
	{
	}

	private void OnUpdateWorld(ISkeletonAnimation animated)
	{
	}

	public virtual void UpdateAnimation(string animationName, string beforeAnimationName)
	{
	}

	[AsyncStateMachine(typeof(_003CPlayAnimationAsync_003Ed__101))]
	public virtual UniTask PlayAnimationAsync(string animationName, bool isLoop, float animStartOffsetTime, AnimationSpeed[] animSpeeds, float defaultSpeed = 1f, string multiTrackAnimationName = null)
	{
		return default;
	}

	public void ClearMultiTrackAnimation()
	{
	}

	private void OnSpineEvent(TrackEntry trackEntry, Spine.Event e)
	{
	}

	protected virtual void ChangeSkin(string skinName)
	{
	}

	protected virtual void LookAtDirection()
	{
	}

	private void LookAtDirectionAsTwoWay()
	{
	}

	private void LookAtDirectionAsFourWay()
	{
	}

	private void LookAtDirectionAsEightWay()
	{
	}

	private void UpdateSkeletonIndex(int index)
	{
	}

	public virtual void SetAnimationSpeed(float speed)
	{
	}

	private void SetActiveSkeletonScaleX(float scaleX)
	{
	}

	private void SyncAnimationToNonPrimarySkeletons()
	{
	}

	public virtual float GetAnimationDuration(string animationName)
	{
		return 0f;
	}

	public abstract string GetIdleAnimation();

	public abstract string GetMoveAnimation();

	public abstract string GetDeathAnimation();

	public abstract string GetReviveAnimation();

	public abstract float GetMoveAnimationSpeed();

	private void RegisterFloatingTextHandlers()
	{
	}

	private void OnSpawnDamageText(NtfHpDamage message)
	{
	}

	private EFloatingTextType GetFloatingTextType(bool isMonster, bool isCritical, bool isPenalty, bool isResistance)
	{
		return EFloatingTextType.DefaultFloatingText;
	}

	private void OnSpawnRecoveryText(NtfHpRecovery message)
	{
	}

	private void OnSpawnStressDamageText(NtfStressDamage message)
	{
	}

	private void OnSpawnStressRecoveryText(NtfStressRecovery message)
	{
	}

	private void OnSpawnInvincibilityText(NtfOnHitInvincibility message)
	{
	}

	private void OnSpawnFreezeFireDamageText()
	{
	}

	private void OnSpawnStatusEffectTagText(EStatusEffectTag effectTag)
	{
	}

	private void OnSpawnStatusEffectTagImmuneText(NtfOnImmuneStatusEffect message)
	{
	}

	private void OnSpawnBuffActiveText(int buffId)
	{
	}
}

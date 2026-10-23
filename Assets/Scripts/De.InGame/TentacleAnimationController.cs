using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(100)]
public class TentacleAnimationController : MonoBehaviour
{
	[Serializable]
	public struct StateProfileMapping
	{
		[FormerlySerializedAs("state")]
		public TentacleState State;

		[FormerlySerializedAs("profile")]
		public TentacleSquirmProfile Profile;
	}

	[Serializable]
	public struct PhaseConfig
	{
		[Tooltip("이 페이즈의 기본 프로필 (매칭 State가 없을 때 사용)")]
		[FormerlySerializedAs("defaultProfile")]
		public TentacleSquirmProfile DefaultProfile;

		[Tooltip("이 페이즈의 상태별 프로필 매핑")]
		[FormerlySerializedAs("stateProfiles")]
		public StateProfileMapping[] StateProfiles;
	}

	[Header("Bone Chain")]
	[Tooltip("촉수 루트 본. 비어있으면 자동으로 첫 번째 자식 체인을 탐색")]
	[FormerlySerializedAs("rootBone")]
	[SerializeField]
	private Transform _rootBone;

	[Tooltip("자동 탐색 시 체인 깊이 제한 (0 = 무제한)")]
	[FormerlySerializedAs("maxChainDepth")]
	[SerializeField]
	private int _maxChainDepth;

	[Header("Phase Profiles")]
	[FormerlySerializedAs("phases")]
	[SerializeField]
	private PhaseConfig[] _phases = new PhaseConfig[1];

	[Header("Idle")]
	[Tooltip("Idle 전환 트리거")]
	[SerializeField]
	private string _idleTrigger = "Idle";

	[Header("Animator")]
	[Tooltip("제어할 Animator (비어있으면 자동 탐색)")]
	[FormerlySerializedAs("targetAnimator")]
	[SerializeField]
	private Animator _targetAnimator;

	[Tooltip("Animator가 매 프레임 본 회전을 갱신하는 경우 true")]
	[FormerlySerializedAs("hasAnimator")]
	[SerializeField]
	private bool _hasAnimator = true;

	[Header("Phase Transition")]
	[Tooltip("Head Layer 인덱스")]
	[FormerlySerializedAs("headLayerIndex")]
	[SerializeField]
	private int _headLayerIndex = 1;

	[Tooltip("Eye Layer 인덱스")]
	[FormerlySerializedAs("eyeLayerIndex")]
	[SerializeField]
	private int _eyeLayerIndex = 2;

	[Tooltip("Eye Layer 전환 트리거")]
	[FormerlySerializedAs("eyeOpenTrigger")]
	[SerializeField]
	private string _eyeOpenTrigger = "EyeOpen";

	[Tooltip("Head Layer 전환 트리거")]
	[FormerlySerializedAs("headOpenTrigger")]
	[SerializeField]
	private string _headOpenTrigger = "HeadOpen";

	[Header("Attack - Slam")]
	[Tooltip("내려치기 Animator 트리거 이름")]
	[FormerlySerializedAs("slamTrigger")]
	[SerializeField]
	private string _slamTrigger = "AttackSlam";

	[Tooltip("내려치기 ImpactStretch")]
	[FormerlySerializedAs("slamStretch")]
	[SerializeField]
	private ImpactStretch _slamStretch;

	[Tooltip("범위(m) 최소/최대")]
	[FormerlySerializedAs("slamRangeMinMax")]
	[SerializeField]
	private Vector2 _slamRangeMinMax = new Vector2(15f, 25f);

	[Tooltip("Stretch Amount 최소/최대 (범위에 대응)")]
	[FormerlySerializedAs("slamStretchMinMax")]
	[SerializeField]
	private Vector2 _slamStretchMinMax = new Vector2(1f, 2f);

	[Header("Attack - Sweep")]
	[Tooltip("휘두르기 Animator 트리거 이름")]
	[FormerlySerializedAs("sweepTrigger")]
	[SerializeField]
	private string _sweepTrigger = "AttackSweep";

	[Tooltip("휘두르기 ImpactStretch")]
	[FormerlySerializedAs("sweepStretch")]
	[SerializeField]
	private ImpactStretch _sweepStretch;

	[Tooltip("범위(m) 최소/최대")]
	[FormerlySerializedAs("sweepRangeMinMax")]
	[SerializeField]
	private Vector2 _sweepRangeMinMax = new Vector2(10f, 20f);

	[Tooltip("Stretch Amount 최소/최대 (범위에 대응)")]
	[FormerlySerializedAs("sweepStretchMinMax")]
	[SerializeField]
	private Vector2 _sweepStretchMinMax = new Vector2(1f, 3f);

	[Header("Die")]
	[Tooltip("죽음 Animator 트리거 이름")]
	[FormerlySerializedAs("dieTrigger")]
	[SerializeField]
	private string _dieTrigger = "Die";

	[Header("Look At (Structure)")]
	[Tooltip("기본 바라볼 대상 (lookAtTarget이 None일 때 사용)")]
	[FormerlySerializedAs("lookAtDefault")]
	[SerializeField]
	private Transform _lookAtDefault;

	[Tooltip("바라볼 위치의 월드 오프셋")]
	[FormerlySerializedAs("lookAtOffset")]
	[SerializeField]
	private Vector3 _lookAtOffset;

	[Tooltip("머리 본")]
	[FormerlySerializedAs("headBone")]
	[SerializeField]
	private Transform _headBone;

	[Tooltip("머리 상위 본 중 LookAt에 참여할 개수")]
	[FormerlySerializedAs("headParentBoneCount")]
	[SerializeField]
	private int _headParentBoneCount = 2;

	[Tooltip("부모 본 가중치 감쇠. 0=선형, 1=급감쇠")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("parentWeightFalloff")]
	[SerializeField]
	private float _parentWeightFalloff = 0.5f;

	[Tooltip("눈 본 목록")]
	[FormerlySerializedAs("eyeBones")]
	[SerializeField]
	private Transform[] _eyeBones;

	[Tooltip("본의 로컬 전방 축")]
	[FormerlySerializedAs("localForwardAxis")]
	[SerializeField]
	private Vector3 _localForwardAxis = Vector3.up;

	[Header("References")]
	[Tooltip("RandomIdlePlayer (Head Layer Idle 관리)")]
	[FormerlySerializedAs("randomIdlePlayer")]
	[SerializeField]
	private TentacleRandomIdlePlayer _randomIdlePlayer;

	[SerializeField]
	private GameObject _tentacleAnimatorObject;

	[SerializeField]
	private float _transformScale = 1.2f;

	private float _transformScaleMultiplier = 1f;

	private Transform[] _boneChain;

	private Quaternion[] _baseLocalRotations;

	private TentacleSquirmProfile _currentProfile;

	private TentacleSquirmProfile _previousProfile;

	private TentacleSquirmProfile _snapshotProfile;

	private int _currentPhaseIndex;

	private float _transitionProgress = 1f;

	private float _transitionDuration = 0.3f;

	private Quaternion _headLookAtSmoothed = Quaternion.identity;

	private Quaternion[] _parentLookAtSmoothed;

	private Quaternion[] _eyeLookAtSmoothed;

	private float _lookAtBlend;

	private TentacleState _lastDetectedState;

	private int _idleTriggerHash;

	private int _eyeOpenHash;

	private int _headOpenHash;

	private int _slamTriggerHash;

	private int _sweepTriggerHash;

	private int _dieTriggerHash;

	private bool _waitingForClawFinish;

	[Header("Look At (Main)")]
	[Tooltip("바라볼 대상 (우선)")]
	[FormerlySerializedAs("lookAtTarget")]
	[SerializeField]
	private Transform _lookAtTarget;

	[SerializeField]
	private float _slamStrechRange = 25f;

	[SerializeField]
	private float _sweepStrechRange = 5f;

	public Animator TentacleAnimator => _targetAnimator;

	public GameObject TentacleAnimatorObject => _tentacleAnimatorObject;

	public float AnimatorSpeed
	{
		get
		{
			return (_targetAnimator != null) ? _targetAnimator.speed : 1f;
		}
		set
		{
			if (_targetAnimator != null)
			{
				_targetAnimator.speed = value;
			}
		}
	}

	public int CurrentStateHash => (_targetAnimator != null) ? _targetAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash : 0;

	public bool IsInTransition => _targetAnimator != null && _targetAnimator.IsInTransition(0);

	public Transform LookAtEffectiveTarget => (_lookAtTarget != null) ? _lookAtTarget : _lookAtDefault;

	private void Awake()
	{
		BuildBoneChain();
		CacheBaseTransforms();
		_snapshotProfile = ScriptableObject.CreateInstance<TentacleSquirmProfile>();
		TentacleSquirmProfile tentacleSquirmProfile = ((_phases != null && _phases.Length != 0) ? _phases[_currentPhaseIndex].DefaultProfile : null);
		_currentProfile = tentacleSquirmProfile;
		_previousProfile = tentacleSquirmProfile;
		InitLookAt();
		if (_targetAnimator != null)
		{
			RegisterToStateBehaviours();
		}
		if (_randomIdlePlayer == null)
		{
			_randomIdlePlayer = GetComponent<TentacleRandomIdlePlayer>();
		}
		if (_randomIdlePlayer == null)
		{
			_randomIdlePlayer = GetComponentInChildren<TentacleRandomIdlePlayer>();
		}
		_idleTriggerHash = Animator.StringToHash(_idleTrigger);
		_eyeOpenHash = Animator.StringToHash(_eyeOpenTrigger);
		_headOpenHash = Animator.StringToHash(_headOpenTrigger);
		_slamTriggerHash = Animator.StringToHash(_slamTrigger);
		_sweepTriggerHash = Animator.StringToHash(_sweepTrigger);
		_dieTriggerHash = Animator.StringToHash(_dieTrigger);
		_transformScaleMultiplier = 1f / Mathf.Max(0.1f, _transformScale);
	}

	private void LateUpdate()
	{
		if (_boneChain == null || _boneChain.Length == 0 || _currentProfile == null)
		{
			return;
		}
		if (_hasAnimator)
		{
			CacheBaseTransforms();
		}
		if (_transitionProgress < 1f)
		{
			_transitionProgress = Mathf.Clamp01(_transitionProgress + Time.deltaTime / Mathf.Max(_transitionDuration, 0.01f));
		}
		float time = Time.time;
		float deltaTime = Time.deltaTime;
		int num = _boneChain.Length;
		bool flag = _transitionProgress < 1f && _previousProfile != null;
		float num2 = Mathf.Lerp(0f, 1f, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_transitionProgress)));
		bool flag2 = _currentProfile.EnableSquirm;
		TentacleSquirmProfile tentacleSquirmProfile;
		if (flag)
		{
			flag2 = flag2 || _previousProfile.EnableSquirm;
			tentacleSquirmProfile = ((num2 >= 0.5f) ? _currentProfile : _previousProfile);
		}
		else
		{
			tentacleSquirmProfile = _currentProfile;
		}
		bool enableHeadStabilization = tentacleSquirmProfile.EnableHeadStabilization;
		Vector3 vector = Vector3.zero;
		if (enableHeadStabilization && num > 1)
		{
			vector = _boneChain[num - 1].position;
		}
		int num3 = -1;
		if (_headBone != null)
		{
			for (int i = 0; i < num; i++)
			{
				if (_boneChain[i] == _headBone)
				{
					num3 = i;
					break;
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			Transform transform = _boneChain[j];
			if (transform == null)
			{
				continue;
			}
			float normalizedIndex = ((num < 2) ? 0f : ((float)j / (float)(num - 1)));
			float angleX = 0f;
			float angleZ = 0f;
			if (flag2 && j != num3)
			{
				ComputeSquirmAngles(_currentProfile, j, normalizedIndex, time, out var angleX2, out var angleZ2);
				if (!flag)
				{
					angleX = angleX2;
					angleZ = angleZ2;
				}
				else
				{
					ComputeSquirmAngles(_previousProfile, j, normalizedIndex, time, out var angleX3, out var angleZ3);
					angleX = Mathf.Lerp(angleX3, angleX2, num2);
					angleZ = Mathf.Lerp(angleZ3, angleZ2, num2);
				}
			}
			Quaternion quaternion = Quaternion.Euler(angleX, 0f, angleZ);
			transform.localRotation = _baseLocalRotations[j] * quaternion;
		}
		if (enableHeadStabilization && num >= 2)
		{
			float t = ((!flag) ? _currentProfile.HeadStabilizationWeight : Mathf.Lerp(_previousProfile.HeadStabilizationWeight, _currentProfile.HeadStabilizationWeight, num2));
			int num4 = ((num3 < 0) ? (num - 1) : num3);
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < num4; l++)
				{
					Vector3 position = _boneChain[l].position;
					Vector3 fromDirection = _boneChain[num - 1].position - position;
					Vector3 toDirection = vector - position;
					if (fromDirection.sqrMagnitude >= 0.0001f && toDirection.sqrMagnitude >= 0.0001f)
					{
						Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(fromDirection, toDirection), t);
						_boneChain[l].rotation = quaternion2 * _boneChain[l].rotation;
					}
				}
			}
		}
		ApplyLookAt(deltaTime);
	}

	private void BeginTransition(TentacleSquirmProfile target)
	{
		if (_transitionProgress >= 1f)
		{
			_previousProfile = _currentProfile;
		}
		else
		{
			if (_previousProfile != null)
			{
				_snapshotProfile.LerpFrom(_previousProfile, _currentProfile, _transitionProgress);
			}
			_previousProfile = _snapshotProfile;
		}
		_currentProfile = target;
		_transitionDuration = _currentProfile.TransitionDuration;
		_transitionProgress = 0f;
	}

	private void ComputeSquirmAngles(TentacleSquirmProfile p, int boneIndex, float normalizedIndex, float time, out float angleX, out float angleZ)
	{
		angleX = 0f;
		angleZ = 0f;
		if (p.EnableSquirm)
		{
			float frequency = p.Frequency;
			float secondaryFrequencyRatio = p.SecondaryFrequencyRatio;
			float secondaryAmplitudeRatio = p.SecondaryAmplitudeRatio;
			float num = (float)boneIndex * p.PhaseOffsetPerBone;
			float num2 = p.AmplitudeCurve.Evaluate(normalizedIndex) * p.Amplitude;
			float num3 = frequency * time * secondaryFrequencyRatio * (float)Math.PI;
			float num4 = frequency * time * (float)Math.PI;
			angleX = (Mathf.Sin(num3 + num3 + num * 0.7f) * secondaryAmplitudeRatio + Mathf.Sin(num4 + num4 + num)) * num2 * p.AxisWeightX;
			angleZ = (Mathf.Cos(num3 + num3 + num * 1.1f) * secondaryAmplitudeRatio + Mathf.Cos(num4 + num4 + num * 1.3f)) * num2 * p.AxisWeightZ;
		}
	}

	private float GetLerpedValue(Func<TentacleSquirmProfile, float> getter)
	{
		if (_transitionProgress < 1f && _previousProfile != null)
		{
			float a = getter(_previousProfile);
			float b = getter(_currentProfile);
			return Mathf.Lerp(a, b, Mathf.SmoothStep(0f, 1f, _transitionProgress));
		}
		return getter(_currentProfile);
	}

	private TentacleSquirmProfile GetProfileForState(TentacleState state)
	{
		if (_phases == null || _phases.Length == 0)
		{
			return null;
		}
		PhaseConfig phaseConfig = _phases[_currentPhaseIndex];
		if (phaseConfig.StateProfiles != null)
		{
			StateProfileMapping[] stateProfiles = phaseConfig.StateProfiles;
			for (int i = 0; i < stateProfiles.Length; i++)
			{
				if (stateProfiles[i].State == state)
				{
					return stateProfiles[i].Profile;
				}
			}
		}
		return phaseConfig.DefaultProfile;
	}

	private TentacleSquirmProfile GetDefaultProfile()
	{
		if (_phases == null || _phases.Length == 0)
		{
			return null;
		}
		return _phases[_currentPhaseIndex].DefaultProfile;
	}

	private static float RangeToStretch(float rangeMeters, Vector2 rangeMinMax, Vector2 stretchMinMax)
	{
		float t = ((rangeMinMax.x == rangeMinMax.y) ? 0f : Mathf.Clamp01((rangeMeters - rangeMinMax.x) / (rangeMinMax.y - rangeMinMax.x)));
		return Mathf.Lerp(stretchMinMax.x, stretchMinMax.y, t);
	}

	private void InitLookAt()
	{
		if (_eyeBones != null && _eyeBones.Length != 0)
		{
			_eyeLookAtSmoothed = new Quaternion[_eyeBones.Length];
		}
		if (_headBone != null)
		{
			_headLookAtSmoothed = _headBone.rotation;
			int num = Mathf.Max(0, _headParentBoneCount);
			_parentLookAtSmoothed = new Quaternion[num];
			Transform parent = _headBone.parent;
			for (int i = 0; i < num; i++)
			{
				if (parent != null)
				{
					_parentLookAtSmoothed[i] = parent.rotation;
					parent = parent.parent;
				}
				else
				{
					_parentLookAtSmoothed[i] = Quaternion.identity;
				}
			}
		}
		if (_eyeLookAtSmoothed != null)
		{
			for (int j = 0; j < _eyeLookAtSmoothed.Length; j++)
			{
				_eyeLookAtSmoothed[j] = Quaternion.identity;
			}
		}
	}

	private void SyncLookAtSmoothed()
	{
		if (_headBone != null)
		{
			_headLookAtSmoothed = _headBone.rotation;
			if (_parentLookAtSmoothed != null)
			{
				Transform parent = _headBone.parent;
				for (int i = 0; i < _parentLookAtSmoothed.Length; i++)
				{
					_parentLookAtSmoothed[i] = ((parent != null) ? parent.rotation : Quaternion.identity);
					if (parent != null)
					{
						parent = parent.parent;
					}
				}
			}
		}
		if (_eyeLookAtSmoothed != null)
		{
			for (int j = 0; j < _eyeLookAtSmoothed.Length; j++)
			{
				_eyeLookAtSmoothed[j] = Quaternion.identity;
			}
		}
	}

	private void ApplyLookAt(float dt)
	{
		float lerpedValue = GetLerpedValue((TentacleSquirmProfile p) => p.EnableLookAt ? 1f : 0f);
		Transform lookAtEffectiveTarget = LookAtEffectiveTarget;
		float num = GetLerpedValue((TentacleSquirmProfile p) => p.LookAtSpeed) * dt;
		float target = ((lerpedValue > 0.5f && lookAtEffectiveTarget != null) ? 1f : 0f);
		_lookAtBlend = Mathf.MoveTowards(_lookAtBlend, target, num);
		if (_lookAtBlend < 0.001f)
		{
			SyncLookAtSmoothed();
			return;
		}
		float lerpedValue2 = GetLerpedValue((TentacleSquirmProfile p) => p.HeadWeight);
		float num2 = lerpedValue2 * _lookAtBlend;
		float num3 = GetLerpedValue((TentacleSquirmProfile p) => p.EyeWeight) * _lookAtBlend;
		float lerpedValue3 = GetLerpedValue((TentacleSquirmProfile p) => p.LookAtMaxAngle);
		if (lookAtEffectiveTarget == null)
		{
			return;
		}
		Vector3 vector = lookAtEffectiveTarget.position + _lookAtOffset;
		if (_headBone != null && num2 > 0f)
		{
			int num4 = Mathf.Min(_headParentBoneCount, (_parentLookAtSmoothed != null) ? _parentLookAtSmoothed.Length : 0);
			Transform[] array = new Transform[num4];
			Transform transform = _headBone.parent;
			for (int i = 0; i < num4; i++)
			{
				array[i] = transform;
				if (transform != null)
				{
					transform = transform.parent;
				}
			}
			Vector3 vector2 = vector - _headBone.position;
			if (vector2.sqrMagnitude > 0.0001f)
			{
				Vector3 normalized = vector2.normalized;
				Quaternion quaternion = Quaternion.FromToRotation(_headBone.rotation * _localForwardAxis.normalized, normalized);
				float num5 = Quaternion.Angle(Quaternion.identity, quaternion);
				if (num5 > lerpedValue3)
				{
					quaternion = Quaternion.Slerp(Quaternion.identity, quaternion, lerpedValue3 / num5);
				}
				Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, quaternion, num2);
				float[] array2 = new float[num4];
				if (num4 > 0)
				{
					float num6 = 1f;
					for (int j = 0; j < num4; j++)
					{
						if (!(array[j] == null))
						{
							float num7 = 1f - Mathf.Pow((float)(j + 1) / (float)(num4 + 1), 1f / Mathf.Max(1f - _parentWeightFalloff, 0.01f));
							array2[j] = num7;
							num6 += num7;
						}
					}
					for (int k = 0; k < num4; k++)
					{
						array2[k] /= num6;
					}
				}
				for (int num8 = num4 - 1; num8 >= 0; num8--)
				{
					if (!(array[num8] == null) && array2[num8] >= 0.001f)
					{
						Quaternion quaternion3 = Quaternion.Slerp(Quaternion.identity, quaternion2, array2[num8]);
						Quaternion b = quaternion3 * array[num8].rotation;
						_parentLookAtSmoothed[num8] = Quaternion.Slerp(_parentLookAtSmoothed[num8], b, num);
						array[num8].rotation = _parentLookAtSmoothed[num8];
					}
				}
				Quaternion quaternion4 = Quaternion.FromToRotation(_headBone.rotation * _localForwardAxis.normalized, normalized);
				float num9 = Quaternion.Angle(Quaternion.identity, quaternion4);
				if (num9 > lerpedValue3)
				{
					quaternion4 = Quaternion.Slerp(Quaternion.identity, quaternion4, lerpedValue3 / num9);
				}
				Quaternion quaternion5 = Quaternion.Slerp(Quaternion.identity, quaternion4, num2);
				Quaternion b2 = quaternion5 * _headBone.rotation;
				_headLookAtSmoothed = Quaternion.Slerp(_headLookAtSmoothed, b2, num);
				_headBone.rotation = _headLookAtSmoothed;
			}
		}
		if (_eyeBones != null && num3 > 0f)
		{
			for (int l = 0; l < _eyeBones.Length; l++)
			{
				if (!(_eyeBones[l] == null))
				{
					ApplyEyeLookAt(_eyeBones[l], ref _eyeLookAtSmoothed[l], vector, num3, lerpedValue3, num);
				}
			}
		}
	}

	private void ApplyEyeLookAt(Transform bone, ref Quaternion smoothedOffset, Vector3 targetPos, float weight, float maxAngle, float speed)
	{
		if (bone == null)
		{
			return;
		}
		Vector3 vector = targetPos - bone.position;
		if (!(vector.sqrMagnitude < 0.0001f))
		{
			Vector3 to = vector.normalized;
			Vector3 from = bone.rotation * _localForwardAxis.normalized;
			Quaternion quaternion = Quaternion.FromToRotation(from, to);
			float num = Quaternion.Angle(Quaternion.identity, quaternion);
			if (num > maxAngle)
			{
				quaternion = Quaternion.Slerp(Quaternion.identity, quaternion, maxAngle / num);
			}
			Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, quaternion, weight);
			Quaternion quaternion3 = ((bone.parent != null) ? bone.parent.rotation : Quaternion.identity);
			Quaternion b = Quaternion.Inverse(quaternion3) * quaternion2 * quaternion3;
			smoothedOffset = Quaternion.Slerp(smoothedOffset, b, speed);
			bone.localRotation = smoothedOffset * bone.localRotation;
		}
	}

	private void BuildBoneChain()
	{
		if (_rootBone != null)
		{
			List<Transform> list = new List<Transform>();
			list.Add(_rootBone);
			if (_rootBone.childCount != 0)
			{
				CollectChain(_rootBone.GetChild(0), list, 1);
			}
			_boneChain = list.ToArray();
		}
		else
		{
			_rootBone = ((base.transform.childCount < 1) ? base.transform : base.transform.GetChild(0));
		}
	}

	private void CollectChain(Transform current, List<Transform> list, int depth)
	{
		while (true)
		{
			list.Add(current);
			if ((_maxChainDepth > 0 && _maxChainDepth <= depth) || current.childCount == 0)
			{
				break;
			}
			depth++;
			current = current.GetChild(0);
		}
	}

	private void CacheBaseTransforms()
	{
		if (_boneChain != null)
		{
			int num = _boneChain.Length;
			if (_baseLocalRotations == null || _baseLocalRotations.Length != num)
			{
				_baseLocalRotations = new Quaternion[num];
			}
			for (int i = 0; i < num; i++)
			{
				if (_boneChain[i] != null)
				{
					_baseLocalRotations[i] = _boneChain[i].localRotation;
				}
			}
		}
	}

	private void RegisterToStateBehaviours()
	{
		if (_targetAnimator == null)
		{
			return;
		}
		TentacleStateBehaviour[] behaviours = _targetAnimator.GetBehaviours<TentacleStateBehaviour>();
		for (int i = 0; i < behaviours.Length; i++)
		{
			behaviours[i].OnTentacleStateChanged += SetTentacleState;
		}
	}

	public void SetSpeedParameter(string paramName, float speed)
	{
		if (_targetAnimator != null)
		{
			_targetAnimator.SetFloat(paramName, speed);
		}
	}

	public void SetFloat(string paramName, float value)
	{
		if (_targetAnimator != null)
		{
			_targetAnimator.SetFloat(paramName, value);
		}
	}

	public void SetTrigger(string triggerName)
	{
		if (_targetAnimator != null)
		{
			_targetAnimator.SetTrigger(triggerName);
		}
	}

	public void SetBool(string paramName, bool value)
	{
		if (_targetAnimator != null)
		{
			_targetAnimator.SetBool(paramName, value);
		}
	}

	public void SetLookAtTarget(Transform target)
	{
		_lookAtTarget = target;
	}

	private void SetTentacleState(TentacleState newState)
	{
		_lastDetectedState = newState;
		if (_waitingForClawFinish)
		{
			_waitingForClawFinish = false;
			if (_randomIdlePlayer != null)
			{
				_randomIdlePlayer.Resume();
			}
		}
		TentacleSquirmProfile profileForState = GetProfileForState(newState);
		if (!(profileForState == null) && !(profileForState == _currentProfile))
		{
			BeginTransition(profileForState);
		}
	}

	private void SetTentaclePhase(int phaseIndex)
	{
		if (_phases != null && phaseIndex >= 0 && phaseIndex < _phases.Length && phaseIndex != _currentPhaseIndex)
		{
			_currentPhaseIndex = phaseIndex;
			TentacleSquirmProfile profileForState = GetProfileForState(_lastDetectedState);
			if (!(profileForState == null) && !(profileForState == _currentProfile))
			{
				BeginTransition(profileForState);
			}
		}
	}

	[ContextMenu("Enter Phase 2")]
	public void EnterSecondPhase()
	{
		if (_targetAnimator == null)
		{
			return;
		}
		if (_randomIdlePlayer != null)
		{
			_randomIdlePlayer.Pause();
		}
		if (_phases != null && _phases.Length > 1 && _currentPhaseIndex != 1)
		{
			_currentPhaseIndex = 1;
			TentacleSquirmProfile profileForState = GetProfileForState(_lastDetectedState);
			if (!(profileForState == null) && !(profileForState == _currentProfile))
			{
				BeginTransition(profileForState);
			}
		}
		_targetAnimator.SetTrigger(_eyeOpenHash);
		_targetAnimator.SetTrigger(_headOpenHash);
		_waitingForClawFinish = true;
	}

	[ContextMenu("Play Attack Slam Animation")]
	private void AttackSlam()
	{
		PlayAttackSlamAnimation();
	}

	public void PlayAttackSlamAnimation()
	{
		if (!(_targetAnimator == null))
		{
			float num = RangeToStretch(_slamStrechRange, _slamRangeMinMax, _slamStretchMinMax);
			if (_slamStretch != null)
			{
				_slamStretch.SetStretchAmount(num * _transformScaleMultiplier);
			}
			_targetAnimator.SetTrigger(_slamTriggerHash);
		}
	}

	[ContextMenu("Play Attack Sweep Animation")]
	private void AttackSweep()
	{
		PlayAttackSweepAnimation();
	}

	public void PlayAttackSweepAnimation()
	{
		if (!(_targetAnimator == null))
		{
			float num = RangeToStretch(_sweepStrechRange, _sweepRangeMinMax, _sweepStretchMinMax);
			if (_sweepStretch != null)
			{
				_sweepStretch.SetStretchAmount(num * _transformScaleMultiplier);
			}
			_targetAnimator.SetTrigger(_sweepTriggerHash);
		}
	}

	[ContextMenu("Play Die Animation")]
	public void PlayDieAnimation()
	{
		if (!(_targetAnimator == null))
		{
			_targetAnimator.SetTrigger(_dieTriggerHash);
		}
	}

	[ContextMenu("Play Idle Animation")]
	public void PlayIdleAnimation()
	{
		if (!(_targetAnimator == null))
		{
			_targetAnimator.SetTrigger(_idleTriggerHash);
		}
	}

	[ContextMenu("Reset Animations")]
	public void ResetAnimations()
	{
		if (!(_targetAnimator == null))
		{
			_targetAnimator.ResetTrigger(_slamTriggerHash);
			_targetAnimator.ResetTrigger(_sweepTriggerHash);
			_targetAnimator.ResetTrigger(_dieTriggerHash);
			_targetAnimator.ResetTrigger(_eyeOpenHash);
			_targetAnimator.SetTrigger(_idleTriggerHash);
		}
	}

	public void SetSlamStrechRange(float target)
	{
		_slamStrechRange = Mathf.Clamp(target, _slamRangeMinMax.x, _slamRangeMinMax.y);
	}

	public void SetSweepStrechRange(float target)
	{
		_sweepStrechRange = Mathf.Clamp(target, _sweepRangeMinMax.x, _sweepRangeMinMax.y);
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(101)]
public class ImpactShake : MonoBehaviour
{
	[Header("Source")]
	[Tooltip("Float 파라미터를 읽을 Animator (비어있으면 자동 탐색)")]
	[FormerlySerializedAs("targetAnimator")]
	[SerializeField]
	private Animator _targetAnimator;

	[Tooltip("CurveStateBehaviour에서 기록하는 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName;

	[Header("Bone Chain")]
	[Tooltip("진동을 적용할 루트 본")]
	[FormerlySerializedAs("rootBone")]
	[SerializeField]
	private Transform _rootBone;

	[Tooltip("체인 깊이 제한 (0 = 무제한)")]
	[FormerlySerializedAs("maxChainDepth")]
	[SerializeField]
	private int _maxChainDepth;

	[Header("Primary Wave")]
	[Tooltip("1차 진동 주파수 (Hz)")]
	[FormerlySerializedAs("frequency")]
	[SerializeField]
	private float _frequency;

	[Tooltip("최대 진동 각도 (도)")]
	[FormerlySerializedAs("amplitude")]
	[SerializeField]
	private float _amplitude;

	[Tooltip("본 간 위상 오프셋 (라디안)")]
	[FormerlySerializedAs("phaseOffsetPerBone")]
	[SerializeField]
	private float _phaseOffsetPerBone;

	[Header("Secondary Wave")]
	[Tooltip("2차 파동 주파수 비율 (1차 대비)")]
	[FormerlySerializedAs("secondaryFrequencyRatio")]
	[SerializeField]
	private float _secondaryFrequencyRatio;

	[Tooltip("2차 파동 진폭 비율 (1차 대비)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("secondaryAmplitudeRatio")]
	[SerializeField]
	private float _secondaryAmplitudeRatio;

	[Header("Axis")]
	[Tooltip("로컬 X축 진동 가중치")]
	[FormerlySerializedAs("axisWeightX")]
	[SerializeField]
	private float _axisWeightX;

	[Tooltip("로컬 Z축 진동 가중치")]
	[FormerlySerializedAs("axisWeightZ")]
	[SerializeField]
	private float _axisWeightZ;

	[Tooltip("X-Z 위상차 (라디안). π/2=원형, 0=직선")]
	[FormerlySerializedAs("axisPhaseOffset")]
	[SerializeField]
	private float _axisPhaseOffset;

	[Header("Distribution")]
	[Tooltip("루트(0) → 팁(1) 진폭 배율 커브")]
	[FormerlySerializedAs("amplitudeCurve")]
	[SerializeField]
	private AnimationCurve _amplitudeCurve;

	private Transform[] _boneChain;

	private Animator _resolvedAnimator;

	private int _paramHash;

	private void Awake()
	{
	}

	private void LateUpdate()
	{
	}

	private void BuildBoneChain()
	{
	}

	private void CollectChain(Transform current, List<Transform> list, int depth)
	{
	}

	private void ResolveAnimator()
	{
	}
}

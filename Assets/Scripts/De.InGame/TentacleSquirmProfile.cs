using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TentacleSquirmProfile", menuName = "Project DE/Tentacle Squirm Profile")]
public class TentacleSquirmProfile : ScriptableObject
{
	[Header("Squirm")]
	[Tooltip("회전 꿈틀거림 활성화")]
	[FormerlySerializedAs("enableSquirm")]
	[SerializeField]
	private bool _enableSquirm;

	[Header("Primary Wave")]
	[Tooltip("꿈틀거림 주파수 (Hz)")]
	[FormerlySerializedAs("frequency")]
	[SerializeField]
	private float _frequency;

	[Tooltip("최대 회전 각도 (도)")]
	[FormerlySerializedAs("amplitude")]
	[SerializeField]
	private float _amplitude;

	[Tooltip("본 간 위상 오프셋 (라디안)")]
	[FormerlySerializedAs("phaseOffsetPerBone")]
	[SerializeField]
	private float _phaseOffsetPerBone;

	[Header("Secondary Wave")]
	[Tooltip("2차 파동 주파수 비율 (primary 대비)")]
	[FormerlySerializedAs("secondaryFrequencyRatio")]
	[SerializeField]
	private float _secondaryFrequencyRatio;

	[Tooltip("2차 파동 진폭 비율 (primary 대비)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("secondaryAmplitudeRatio")]
	[SerializeField]
	private float _secondaryAmplitudeRatio;

	[Header("Axis Weights")]
	[Tooltip("로컬 X축 회전 가중치")]
	[FormerlySerializedAs("axisWeightX")]
	[SerializeField]
	private float _axisWeightX;

	[Tooltip("로컬 Z축 회전 가중치")]
	[FormerlySerializedAs("axisWeightZ")]
	[SerializeField]
	private float _axisWeightZ;

	[Header("Amplitude Curve")]
	[Tooltip("루트(0) → 팁(1) 진폭 배율 커브")]
	[FormerlySerializedAs("amplitudeCurve")]
	[SerializeField]
	private AnimationCurve _amplitudeCurve;

	[Header("Look At")]
	[Tooltip("LookAt 기능 활성화")]
	[FormerlySerializedAs("enableLookAt")]
	[SerializeField]
	private bool _enableLookAt;

	[Tooltip("머리 회전 가중치 (0=무시, 1=완전히 바라봄)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("headWeight")]
	[SerializeField]
	private float _headWeight;

	[Tooltip("눈 회전 가중치")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("eyeWeight")]
	[SerializeField]
	private float _eyeWeight;

	[Tooltip("LookAt 회전 최대 각도 (도)")]
	[FormerlySerializedAs("lookAtMaxAngle")]
	[SerializeField]
	private float _lookAtMaxAngle;

	[Tooltip("LookAt 추적 속도 (높을수록 즉시 추적)")]
	[FormerlySerializedAs("lookAtSpeed")]
	[SerializeField]
	private float _lookAtSpeed;

	[Tooltip("머리(팁) 위치 안정화 활성화 — 꿈틀거림 시 머리 위치를 최대한 고정")]
	[FormerlySerializedAs("enableHeadStabilization")]
	[SerializeField]
	private bool _enableHeadStabilization;

	[Tooltip("안정화 강도 (0=없음, 1=완전 고정)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("headStabilizationWeight")]
	[SerializeField]
	private float _headStabilizationWeight;

	[Header("Transition")]
	[Tooltip("프로파일 전환 보간 시간 (초)")]
	[FormerlySerializedAs("transitionDuration")]
	[SerializeField]
	private float _transitionDuration;

	public bool EnableSquirm => false;

	public float Frequency => 0f;

	public float Amplitude => 0f;

	public float PhaseOffsetPerBone => 0f;

	public float SecondaryFrequencyRatio => 0f;

	public float SecondaryAmplitudeRatio => 0f;

	public float AxisWeightX => 0f;

	public float AxisWeightZ => 0f;

	public AnimationCurve AmplitudeCurve => null;

	public bool EnableLookAt => false;

	public float HeadWeight => 0f;

	public float EyeWeight => 0f;

	public float LookAtMaxAngle => 0f;

	public float LookAtSpeed => 0f;

	public bool EnableHeadStabilization => false;

	public float HeadStabilizationWeight => 0f;

	public float TransitionDuration => 0f;

	public void LerpFrom(TentacleSquirmProfile a, TentacleSquirmProfile b, float t)
	{
	}
}

using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TentacleSquirmProfile", menuName = "Project DE/Tentacle Squirm Profile")]
public class TentacleSquirmProfile : ScriptableObject
{
	[Header("Squirm")]
	[Tooltip("회전 꿈틀거림 활성화")]
	[FormerlySerializedAs("enableSquirm")]
	[SerializeField]
	private bool _enableSquirm = true;

	[Header("Primary Wave")]
	[Tooltip("꿈틀거림 주파수 (Hz)")]
	[FormerlySerializedAs("frequency")]
	[SerializeField]
	private float _frequency = 1.2f;

	[Tooltip("최대 회전 각도 (도)")]
	[FormerlySerializedAs("amplitude")]
	[SerializeField]
	private float _amplitude = 15f;

	[Tooltip("본 간 위상 오프셋 (라디안)")]
	[FormerlySerializedAs("phaseOffsetPerBone")]
	[SerializeField]
	private float _phaseOffsetPerBone = 0.5f;

	[Header("Secondary Wave")]
	[Tooltip("2차 파동 주파수 비율 (primary 대비)")]
	[FormerlySerializedAs("secondaryFrequencyRatio")]
	[SerializeField]
	private float _secondaryFrequencyRatio = 2.3f;

	[Tooltip("2차 파동 진폭 비율 (primary 대비)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("secondaryAmplitudeRatio")]
	[SerializeField]
	private float _secondaryAmplitudeRatio = 0.3f;

	[Header("Axis Weights")]
	[Tooltip("로컬 X축 회전 가중치")]
	[FormerlySerializedAs("axisWeightX")]
	[SerializeField]
	private float _axisWeightX = 1f;

	[Tooltip("로컬 Z축 회전 가중치")]
	[FormerlySerializedAs("axisWeightZ")]
	[SerializeField]
	private float _axisWeightZ = 0.6f;

	[Header("Amplitude Curve")]
	[Tooltip("루트(0) → 팁(1) 진폭 배율 커브")]
	[FormerlySerializedAs("amplitudeCurve")]
	[SerializeField]
	private AnimationCurve _amplitudeCurve = AnimationCurve.Linear(0f, 0.2f, 1f, 1f);

	[Header("Look At")]
	[Tooltip("LookAt 기능 활성화")]
	[FormerlySerializedAs("enableLookAt")]
	[SerializeField]
	private bool _enableLookAt = true;

	[Tooltip("머리 회전 가중치 (0=무시, 1=완전히 바라봄)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("headWeight")]
	[SerializeField]
	private float _headWeight = 0.6f;

	[Tooltip("눈 회전 가중치")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("eyeWeight")]
	[SerializeField]
	private float _eyeWeight = 0.9f;

	[Tooltip("LookAt 회전 최대 각도 (도)")]
	[FormerlySerializedAs("lookAtMaxAngle")]
	[SerializeField]
	private float _lookAtMaxAngle = 70f;

	[Tooltip("LookAt 추적 속도 (높을수록 즉시 추적)")]
	[FormerlySerializedAs("lookAtSpeed")]
	[SerializeField]
	private float _lookAtSpeed = 5f;

	[Tooltip("머리(팁) 위치 안정화 활성화 — 꿈틀거림 시 머리 위치를 최대한 고정")]
	[FormerlySerializedAs("enableHeadStabilization")]
	[SerializeField]
	private bool _enableHeadStabilization;

	[Tooltip("안정화 강도 (0=없음, 1=완전 고정)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("headStabilizationWeight")]
	[SerializeField]
	private float _headStabilizationWeight = 0.8f;

	[Header("Transition")]
	[Tooltip("프로파일 전환 보간 시간 (초)")]
	[FormerlySerializedAs("transitionDuration")]
	[SerializeField]
	private float _transitionDuration = 0.3f;

	public bool EnableSquirm => _enableSquirm;

	public float Frequency => _frequency;

	public float Amplitude => _amplitude;

	public float PhaseOffsetPerBone => _phaseOffsetPerBone;

	public float SecondaryFrequencyRatio => _secondaryFrequencyRatio;

	public float SecondaryAmplitudeRatio => _secondaryAmplitudeRatio;

	public float AxisWeightX => _axisWeightX;

	public float AxisWeightZ => _axisWeightZ;

	public AnimationCurve AmplitudeCurve => _amplitudeCurve;

	public bool EnableLookAt => _enableLookAt;

	public float HeadWeight => _headWeight;

	public float EyeWeight => _eyeWeight;

	public float LookAtMaxAngle => _lookAtMaxAngle;

	public float LookAtSpeed => _lookAtSpeed;

	public bool EnableHeadStabilization => _enableHeadStabilization;

	public float HeadStabilizationWeight => _headStabilizationWeight;

	public float TransitionDuration => _transitionDuration;

	public void LerpFrom(TentacleSquirmProfile a, TentacleSquirmProfile b, float t)
	{
		t = Mathf.Clamp01(t);
		t = Mathf.SmoothStep(0f, 1f, t);
		_enableSquirm = ((t < 0.5f) ? a._enableSquirm : b._enableSquirm);
		_frequency = Mathf.Lerp(a._frequency, b._frequency, t);
		_amplitude = Mathf.Lerp(a._amplitude, b._amplitude, t);
		_phaseOffsetPerBone = Mathf.Lerp(a._phaseOffsetPerBone, b._phaseOffsetPerBone, t);
		_secondaryFrequencyRatio = Mathf.Lerp(a._secondaryFrequencyRatio, b._secondaryFrequencyRatio, t);
		_secondaryAmplitudeRatio = Mathf.Lerp(a._secondaryAmplitudeRatio, b._secondaryAmplitudeRatio, t);
		_axisWeightX = Mathf.Lerp(a._axisWeightX, b._axisWeightX, t);
		_axisWeightZ = Mathf.Lerp(a._axisWeightZ, b._axisWeightZ, t);
		_enableLookAt = ((t < 0.5f) ? a._enableLookAt : b._enableLookAt);
		_headWeight = Mathf.Lerp(a._headWeight, b._headWeight, t);
		_eyeWeight = Mathf.Lerp(a._eyeWeight, b._eyeWeight, t);
		_lookAtMaxAngle = Mathf.Lerp(a._lookAtMaxAngle, b._lookAtMaxAngle, t);
		_lookAtSpeed = Mathf.Lerp(a._lookAtSpeed, b._lookAtSpeed, t);
		_enableHeadStabilization = ((t < 0.5f) ? a._enableHeadStabilization : b._enableHeadStabilization);
		_headStabilizationWeight = Mathf.Lerp(a._headStabilizationWeight, b._headStabilizationWeight, t);
	}
}

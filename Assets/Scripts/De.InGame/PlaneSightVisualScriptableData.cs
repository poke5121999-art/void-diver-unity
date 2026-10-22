using UnityEngine;

[CreateAssetMenu(fileName = "Plane Sight Visual Scriptable Data", menuName = "NEMO Art/Plane Sight Visual Scriptable Data")]
public class PlaneSightVisualScriptableData : ScriptableObject
{
	[Header("Color Setting")]
	[Tooltip("광원의 내부 색상입니다.")]
	[SerializeField]
	private Color _sightInnerColor;

	[Tooltip("광원의 외곽 경계 색상입니다.")]
	[SerializeField]
	private Color _sightBoundaryColor;

	[Tooltip("광원 색상의 Lerp 비율입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightColorLerp;

	[Tooltip("광원 색상의 경계 두께입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightColorWidth;

	[Header("Shape Setting")]
	[Tooltip("시야의 선명도 강도입니다.")]
	[SerializeField]
	private float _sightSharpnessPower;

	[Tooltip("시야 경계의 위치 조정값입니다.")]
	[Range(-1f, 1f)]
	[SerializeField]
	private float _sightSharpnessBoundary;

	[Tooltip("시야의 둥글기 비율입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightRoundness;

	[Tooltip("시야와 시야의 교차 부드러움입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightIntersectionSmoothness;

	[Header("SeeThrough Setting")]
	[Tooltip("SeeThrough 강도")]
	[SerializeField]
	private float _seeThroughPower;

	[Tooltip("SeeThrough 경계 위치")]
	[Range(-1f, 1f)]
	[SerializeField]
	private float _seeThroughBoundary;

	[Tooltip("SeeThrough 높이")]
	[SerializeField]
	private float _seeThroughHeight;

	[Tooltip("SeeThrough 교차 부드러움")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _seeThroughIntersectionSmoothness;

	[Header("거리 감쇠 정도")]
	[SerializeField]
	private float _seeThroughDistanceMax;

	[SerializeField]
	private float _seeThroughDistanceMin;

	public Color SightInnerColor => default;

	public Color SightBoundaryColor => default;

	public float SightColorLerp => 0f;

	public float SightColorWidth => 0f;

	public float SightSharpnessPower => 0f;

	public float SightSharpnessBoundary => 0f;

	public float SightRoundness => 0f;

	public float SightIntersectionSmoothness => 0f;

	public float SeeThroughPower => 0f;

	public float SeeThroughBoundary => 0f;

	public float SeeThroughHeight => 0f;

	public float SeeThroughIntersectionSmoothness => 0f;

	public float SeeThroughDistanceMax => 0f;

	public float SeeThroughDistanceMin => 0f;
}

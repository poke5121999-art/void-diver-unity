using UnityEngine;

[CreateAssetMenu(fileName = "Plane Sight Visual Scriptable Data", menuName = "NEMO Art/Plane Sight Visual Scriptable Data")]
public class PlaneSightVisualScriptableData : ScriptableObject
{
	[Header("Color Setting")]
	[Tooltip("광원의 내부 색상입니다.")]
	[SerializeField]
	private Color _sightInnerColor = Color.white;

	[Tooltip("광원의 외곽 경계 색상입니다.")]
	[SerializeField]
	private Color _sightBoundaryColor = Color.white;

	[Tooltip("광원 색상의 Lerp 비율입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightColorLerp = 0.25f;

	[Tooltip("광원 색상의 경계 두께입니다.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _sightColorWidth = 0.25f;

	[Header("Shape Setting")]
	[Tooltip("시야의 선명도 강도입니다.")]
	[SerializeField]
	private float _sightSharpnessPower = 0.25f;

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
	private float _seeThroughPower = 2f;

	[Tooltip("SeeThrough 경계 위치")]
	[Range(-1f, 1f)]
	[SerializeField]
	private float _seeThroughBoundary = -0.5f;

	[Tooltip("SeeThrough 높이")]
	[SerializeField]
	private float _seeThroughHeight = 2f;

	[Tooltip("SeeThrough 교차 부드러움")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _seeThroughIntersectionSmoothness;

	[Header("거리 감쇠 정도")]
	[SerializeField]
	private float _seeThroughDistanceMax = 13f;

	[SerializeField]
	private float _seeThroughDistanceMin = 8f;

	public Color SightInnerColor => _sightInnerColor;

	public Color SightBoundaryColor => _sightBoundaryColor;

	public float SightColorLerp => _sightColorLerp;

	public float SightColorWidth => _sightColorWidth;

	public float SightSharpnessPower => _sightSharpnessPower;

	public float SightSharpnessBoundary => _sightSharpnessBoundary;

	public float SightRoundness => _sightRoundness;

	public float SightIntersectionSmoothness => _sightIntersectionSmoothness;

	public float SeeThroughPower => _seeThroughPower;

	public float SeeThroughBoundary => _seeThroughBoundary;

	public float SeeThroughHeight => _seeThroughHeight;

	public float SeeThroughIntersectionSmoothness => _seeThroughIntersectionSmoothness;

	public float SeeThroughDistanceMax => _seeThroughDistanceMax;

	public float SeeThroughDistanceMin => _seeThroughDistanceMin;
}

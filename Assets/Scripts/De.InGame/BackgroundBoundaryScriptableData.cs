using UnityEngine;

[CreateAssetMenu(fileName = "Background Boundary Scriptable Data", menuName = "NEMO Art/Background Boundary Scriptable Data")]
public class BackgroundBoundaryScriptableData : ScriptableObject
{
	[Header("Box Size Setting")]
	[Tooltip("바운더리 박스의 Y축(높이). 2.5D 카메라 전체 영역 커버용.")]
	[SerializeField]
	private float _boxHeight = 50f;

	[Tooltip("바운더리 박스의 두께 (맵 외부 방향). 이 값이 클수록 어둠 영역이 바깥으로 깊게 확장됨.")]
	[SerializeField]
	private float _boxThickness = 20f;

	[Tooltip("바운더리 박스의 시각적 안쪽 두께 (맵 내부 방향). 이 값이 클수록 어둠 영역이 안쪽으로 깊게 확장됨.")]
	[SerializeField]
	private float _boxInnerThickness = 0.5f;

	[Header("Offset Setting")]
	[Tooltip("바운더리 중심 오프셋. 카메라 각도 차이 보정용.")]
	[SerializeField]
	private Vector3 _boundaryPivotOffset = new Vector3(1f, 0f, -1f);

	[Header("Visual Setting")]
	[Tooltip("바운더리 이펙트 타입. Darken / Shimmer / DarkenAndShimmer 중 선택.")]
	[SerializeField]
	private EBackgroundBoxAreaEffectType _effectType;

	[Tooltip("바운더리 오버레이 색상. Alpha는 어둠 강도.")]
	[SerializeField]
	private Color _overlayColor = new Color(0f, 0f, 0f, 0.8f);

	[Tooltip("경계 부드러움. 값이 클수록 박스 안쪽으로 부드럽게 감쇠.")]
	[SerializeField]
	private float _edgeSmooth = 1f;

	[Tooltip("바운더리 강도 타겟 (0~1).")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _targetIntensity = 1f;

	[Header("Shimmer Setting (EffectType이 Shimmer 포함일 때 유효)")]
	[Tooltip("일렁임 속도.")]
	[SerializeField]
	private float _shimmerSpeed = 1f;

	[Tooltip("일렁임 강도.")]
	[SerializeField]
	private float _shimmerStrength = 0.01f;

	[Header("Fog Setting (EffectType이 Fog일 때 유효)")]
	[Tooltip("Fog 노이즈 섞임 비율. 0=균일 색상, 1=완전 노이즈 패턴(얼룩덜룩).")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _fogDensity = 1f;

	public float BoxHeight => Mathf.Max(0f, _boxHeight);

	public float BoxThickness => Mathf.Max(0f, _boxThickness);

	public float BoxInnerThickness => Mathf.Max(0f, _boxInnerThickness);

	public Vector3 BoundaryPivotOffset => _boundaryPivotOffset;

	public EBackgroundBoxAreaEffectType EffectType => _effectType;

	public Color OverlayColor => _overlayColor;

	public float EdgeSmooth => Mathf.Max(0f, _edgeSmooth);

	public float TargetIntensity => _targetIntensity;

	public float ShimmerSpeed => _shimmerSpeed;

	public float ShimmerStrength => _shimmerStrength;

	public float FogDensity => _fogDensity;
}

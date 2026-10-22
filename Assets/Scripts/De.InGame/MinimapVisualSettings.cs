using UnityEngine;

[CreateAssetMenu(fileName = "MinimapVisualSetting", menuName = "NEMO Art/Minimap Visual Scriptable Data")]
public class MinimapVisualSettings : ScriptableObject
{
	[Header("In Stage Sight Setting")]
	[SerializeField]
	private bool _enableSightSystemInStage;

	[Tooltip("전장의 안개(Sight Mask) 시스템 마스터 토글. false면 Applier/Unit/Core의 fog RT 경로 전부 no-op")]
	[SerializeField]
	private bool _enableSightMaskSystem;

	[Header("Sight Color Setting")]
	[SerializeField]
	private Color _tintColor;

	[SerializeField]
	private Color _passedAreaColor;

	[SerializeField]
	private Color _sightMaskColor;

	[Header("Sight Setting")]
	[SerializeField]
	private float _unitRadius;

	[SerializeField]
	private Material _maskingMaterial;

	[Header("Sight Mask Noise")]
	[SerializeField]
	private Texture _sightNoiseTex;

	[SerializeField]
	[Range(0.01f, 8f)]
	private float _sightNoiseScale;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _sightNoiseSpeed;

	[SerializeField]
	private Vector2 _sightNoiseStretch;

	[SerializeField]
	[Range(-1f, 0f)]
	private float _sightDissolveOffset;

	[Header("Minimap Outer Setting")]
	[SerializeField]
	private float _outerSizeUnit;

	[SerializeField]
	private Color _outerColor;

	[SerializeField]
	private Material _outerMaterial;

	[Header("Outer Noise / Dissolve")]
	[SerializeField]
	private Texture _outerNoiseTex;

	[SerializeField]
	[Range(0.01f, 8f)]
	private float _outerNoiseScale;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _outerNoiseSpeed;

	[SerializeField]
	private Vector2 _outerNoiseStretch;

	[SerializeField]
	[Range(-1f, 0f)]
	private float _outerDissolveOffset;

	[SerializeField]
	[Range(0f, 4f)]
	private float _outerSmokeIntensity;

	[Header("Layer SDF Setting")]
	[Tooltip("SDF 출력 해상도 배율. 4=기준(map cell당 4픽셀), 16=4배 고해상도. 높을수록 부드러우나 비용↑")]
	[SerializeField]
	[Range(4f, 16f)]
	private int _layerSdfScale;

	[SerializeField]
	[Range(0.5f, 64f)]
	private float _layerSdfSpread;

	[Tooltip("입력 마스크 padding cell 수. -1=auto(OuterSizeUnit/sectorSize), >=0=override")]
	[SerializeField]
	private int _layerInputPaddingOverride;

	[Header("Outer SDF Range (smoothstep min/max, SDF 정규화 0=외부 깊숙, 0.5=경계, 1=내부 깊숙)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _outerSDFMin;

	[SerializeField]
	[Range(0f, 1f)]
	private float _outerSDFMax;

	[Header("Inner SDF Range (smoothstep min/max)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _innerSDFMin;

	[SerializeField]
	[Range(0f, 1f)]
	private float _innerSDFMax;

	public bool EnableSightSystemInStage => false;

	public bool EnableSightMaskSystem => false;

	public Color TintColor => default;

	public Color PassedAreaColor => default;

	public Color SightMaskColor => default;

	public float UnitRadius => 0f;

	public Material MaskingMaterial => null;

	public Texture SightNoiseTex => null;

	public float SightNoiseScale => 0f;

	public float SightNoiseSpeed => 0f;

	public Vector2 SightNoiseStretch => default;

	public float SightDissolveOffset => 0f;

	public float OuterSizeUnit => 0f;

	public Color OuterColor => default;

	public Material OuterMaterial => null;

	public Texture OuterNoiseTex => null;

	public float OuterNoiseScale => 0f;

	public float OuterNoiseSpeed => 0f;

	public Vector2 OuterNoiseStretch => default;

	public float OuterDissolveOffset => 0f;

	public float OuterSmokeIntensity => 0f;

	public int LayerSdfScale => 0;

	public float LayerSdfSpread => 0f;

	public int LayerInputPaddingOverride => 0;

	public float OuterSDFMin => 0f;

	public float OuterSDFMax => 0f;

	public float InnerSDFMin => 0f;

	public float InnerSDFMax => 0f;
}

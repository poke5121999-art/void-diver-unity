using UnityEngine;

[CreateAssetMenu(fileName = "MinimapVisualSetting", menuName = "NEMO Art/Minimap Visual Scriptable Data")]
public class MinimapVisualSettings : ScriptableObject
{
	[Header("In Stage Sight Setting")]
	[SerializeField]
	private bool _enableSightSystemInStage = true;

	[Tooltip("전장의 안개(Sight Mask) 시스템 마스터 토글. false면 Applier/Unit/Core의 fog RT 경로 전부 no-op")]
	[SerializeField]
	private bool _enableSightMaskSystem;

	[Header("Sight Color Setting")]
	[SerializeField]
	private Color _tintColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	[SerializeField]
	private Color _passedAreaColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	[SerializeField]
	private Color _sightMaskColor = new Color(0.1f, 0.1f, 0.1f, 1f);

	[Header("Sight Setting")]
	[SerializeField]
	private float _unitRadius = 7f;

	[SerializeField]
	private Material _maskingMaterial;

	[Header("Sight Mask Noise")]
	[SerializeField]
	private Texture _sightNoiseTex;

	[SerializeField]
	[Range(0.01f, 8f)]
	private float _sightNoiseScale = 0.5f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _sightNoiseSpeed = -0.05f;

	[SerializeField]
	private Vector2 _sightNoiseStretch = new Vector2(1f, 4f);

	[SerializeField]
	[Range(-1f, 0f)]
	private float _sightDissolveOffset = -0.5f;

	[Header("Minimap Outer Setting")]
	[SerializeField]
	private float _outerSizeUnit = 64f;

	[SerializeField]
	private Color _outerColor = new Color(0.1f, 0.1f, 0.1f, 1f);

	[SerializeField]
	private Material _outerMaterial;

	[Header("Outer Noise / Dissolve")]
	[SerializeField]
	private Texture _outerNoiseTex;

	[SerializeField]
	[Range(0.01f, 8f)]
	private float _outerNoiseScale = 1.5f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _outerNoiseSpeed = -0.05f;

	[SerializeField]
	private Vector2 _outerNoiseStretch = new Vector2(1f, 4f);

	[SerializeField]
	[Range(-1f, 0f)]
	private float _outerDissolveOffset = -0.5f;

	[SerializeField]
	[Range(0f, 4f)]
	private float _outerSmokeIntensity = 1f;

	[Header("Layer SDF Setting")]
	[Tooltip("SDF 출력 해상도 배율. 4=기준(map cell당 4픽셀), 16=4배 고해상도. 높을수록 부드러우나 비용↑")]
	[SerializeField]
	[Range(4f, 16f)]
	private int _layerSdfScale = 4;

	[SerializeField]
	[Range(0.5f, 64f)]
	private float _layerSdfSpread = 8f;

	[Tooltip("입력 마스크 padding cell 수. -1=auto(OuterSizeUnit/sectorSize), >=0=override")]
	[SerializeField]
	private int _layerInputPaddingOverride = -1;

	[Header("Outer SDF Range (smoothstep min/max, SDF 정규화 0=외부 깊숙, 0.5=경계, 1=내부 깊숙)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _outerSDFMin;

	[SerializeField]
	[Range(0f, 1f)]
	private float _outerSDFMax = 0.5f;

	[Header("Inner SDF Range (smoothstep min/max)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _innerSDFMin = 0.2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _innerSDFMax = 0.225f;

	public bool EnableSightSystemInStage => _enableSightSystemInStage;

	public bool EnableSightMaskSystem => _enableSightMaskSystem;

	public Color TintColor => _tintColor;

	public Color PassedAreaColor => _passedAreaColor;

	public Color SightMaskColor => _sightMaskColor;

	public float UnitRadius => _unitRadius;

	public Material MaskingMaterial => _maskingMaterial;

	public Texture SightNoiseTex => _sightNoiseTex;

	public float SightNoiseScale => _sightNoiseScale;

	public float SightNoiseSpeed => _sightNoiseSpeed;

	public Vector2 SightNoiseStretch => _sightNoiseStretch;

	public float SightDissolveOffset => _sightDissolveOffset;

	public float OuterSizeUnit => _outerSizeUnit;

	public Color OuterColor => _outerColor;

	public Material OuterMaterial => _outerMaterial;

	public Texture OuterNoiseTex => _outerNoiseTex;

	public float OuterNoiseScale => _outerNoiseScale;

	public float OuterNoiseSpeed => _outerNoiseSpeed;

	public Vector2 OuterNoiseStretch => _outerNoiseStretch;

	public float OuterDissolveOffset => _outerDissolveOffset;

	public float OuterSmokeIntensity => _outerSmokeIntensity;

	public int LayerSdfScale => _layerSdfScale;

	public float LayerSdfSpread => _layerSdfSpread;

	public int LayerInputPaddingOverride => _layerInputPaddingOverride;

	public float OuterSDFMin => _outerSDFMin;

	public float OuterSDFMax => _outerSDFMax;

	public float InnerSDFMin => _innerSDFMin;

	public float InnerSDFMax => _innerSDFMax;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Map Visual Area Setting", menuName = "NEMO Art/Map Visual Area Setting")]
public class MapVisualAreaSetting : ScriptableObject
{
	[Header("공통")]
	[SerializeField]
	private Texture2D _noiseTexture;

	[SerializeField]
	private Texture2D _normalMapTexture;

	[SerializeField]
	private float _waveSpeed;

	[SerializeField]
	private float _waveScale;

	[Range(0f, 1f)]
	[SerializeField]
	private float _waveDistortion;

	[Header("표면 수면 / 높이")]
	[SerializeField]
	private float _waterSurfaceLevel;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreHeightDrop;

	[Range(0f, 2f)]
	[SerializeField]
	private float _surfaceEdgeWidth;

	[Range(-1f, 1f)]
	[SerializeField]
	private float _depthOffset;

	[Header("표면 수면 / 색상")]
	[SerializeField]
	private Color _shallowColor;

	[SerializeField]
	private Color _deepColor;

	[Header("표면 수면 / 깊이")]
	[Range(0.5f, 0.7f)]
	[HideInInspector]
	[SerializeField]
	private float _depthMin;

	[Range(0.7f, 1f)]
	[HideInInspector]
	[SerializeField]
	private float _depthMax;

	[Range(0f, 1f)]
	[HideInInspector]
	[SerializeField]
	private float _depthLightFlatten;

	[Header("표면 수면 / 해안선")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreEdgeMin;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreEdgeMax;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreStroke;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreIntensity;

	[Range(0.01f, 1f)]
	[SerializeField]
	private float _shoreNoiseTiling;

	[Range(0f, 2f)]
	[SerializeField]
	private float _shoreNoiseStrength;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreNoisePower;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreNoiseIntensity;

	[Header("표면 수면 / 반사")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _reflectionIntensity;

	[Range(-0.5f, 0.5f)]
	[SerializeField]
	private float _reflectionYOffset;

	[Header("수면 Bottom / 높이")]
	[SerializeField]
	private float _waterBottomSurfaceHeight;

	[Header("수면 Bottom / 색상")]
	[SerializeField]
	private Color _waterBottomShallowColor;

	[SerializeField]
	private Color _waterBottomColor;

	[Header("수면 Bottom / 깊이 엣지")]
	[SerializeField]
	private float _waterBottomDepthDiffEnd;

	[SerializeField]
	private float _waterBottomEdgeDepthDiffEnd;

	[Range(1f, 10f)]
	[SerializeField]
	private float _waterBottomEdgePower;

	[Range(0f, 10f)]
	[SerializeField]
	private float _waterBottomEdgeIntensity;

	[Header("수면 Bottom / 반사")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _waterBottomReflectionIntensity;

	[SerializeField]
	private float _waterBottomReflectionYOffset;

	[Header("SDF 생성 (ComputeShader)")]
	[SerializeField]
	private ComputeShader _sdfComputeShader;

	[Range(1f, 32f)]
	[SerializeField]
	private float _sdfSpread;

	[Range(1f, 32f)]
	[SerializeField]
	private float _roofSdfSpread;

	[Range(1f, 8f)]
	[SerializeField]
	private int _sdfScale;

	[Header("파동 (WaveCompute)")]
	[SerializeField]
	private ComputeShader _waveComputeShader;

	[SerializeField]
	private int _waveMapResolution;

	[SerializeField]
	private float _waveMapSize;

	public Texture2D NoiseTexture => null;

	public Texture2D NormalMapTexture => null;

	public float WaveSpeed => 0f;

	public float WaveScale => 0f;

	public float WaveDistortion => 0f;

	public float WaterSurfaceLevel => 0f;

	public float ShoreHeightDrop => 0f;

	public float SurfaceEdgeWidth => 0f;

	public float DepthOffset => 0f;

	public Color ShallowColor => default;

	public Color DeepColor => default;

	public float DepthMin => 0f;

	public float DepthMax => 0f;

	public float DepthLightFlatten => 0f;

	public float ShoreEdgeMin => 0f;

	public float ShoreEdgeMax => 0f;

	public float ShoreStroke => 0f;

	public float ShoreIntensity => 0f;

	public float ShoreNoiseTiling => 0f;

	public float ShoreNoiseStrength => 0f;

	public float ShoreNoisePower => 0f;

	public float ShoreNoiseIntensity => 0f;

	public float ReflectionIntensity => 0f;

	public float ReflectionYOffset => 0f;

	public float WaterBottomSurfaceHeight => 0f;

	public Color WaterBottomShallowColor => default;

	public Color WaterBottomColor => default;

	public float WaterBottomDepthDiffEnd => 0f;

	public float WaterBottomEdgeDepthDiffEnd => 0f;

	public float WaterBottomEdgePower => 0f;

	public float WaterBottomEdgeIntensity => 0f;

	public float WaterBottomReflectionIntensity => 0f;

	public float WaterBottomReflectionYOffset => 0f;

	public ComputeShader SdfComputeShader => null;

	public float SdfSpread => 0f;

	public float RoofSdfSpread => 0f;

	public int SdfScale => 0;

	public ComputeShader WaveComputeShader => null;

	public int WaveMapResolution => 0;

	public float WaveMapSize => 0f;

	public void ApplyShaderGlobals()
	{
	}
}

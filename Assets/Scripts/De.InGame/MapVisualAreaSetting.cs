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
	private float _waveSpeed = 0.05f;

	[SerializeField]
	private float _waveScale = 0.3f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _waveDistortion = 0.1f;

	[Header("표면 수면 / 높이")]
	[SerializeField]
	private float _waterSurfaceLevel;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreHeightDrop = 0.3f;

	[Range(0f, 2f)]
	[SerializeField]
	private float _surfaceEdgeWidth = 0.2f;

	[Range(-1f, 1f)]
	[SerializeField]
	private float _depthOffset;

	[Header("표면 수면 / 색상")]
	[SerializeField]
	private Color _shallowColor = new Color(0.25f, 0.45f, 0.5f, 1f);

	[SerializeField]
	private Color _deepColor = new Color(0.06f, 0.12f, 0.18f, 1f);

	[Header("표면 수면 / 깊이")]
	[Range(0.5f, 0.7f)]
	[HideInInspector]
	[SerializeField]
	private float _depthMin = 0.5f;

	[Range(0.7f, 1f)]
	[HideInInspector]
	[SerializeField]
	private float _depthMax = 0.85f;

	[Range(0f, 1f)]
	[HideInInspector]
	[SerializeField]
	private float _depthLightFlatten = 1f;

	[Header("표면 수면 / 해안선")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreEdgeMin = 0.48f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreEdgeMax = 0.52f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _shoreStroke = 0.1f;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreIntensity = 4f;

	[Range(0.01f, 1f)]
	[SerializeField]
	private float _shoreNoiseTiling = 0.1f;

	[Range(0f, 2f)]
	[SerializeField]
	private float _shoreNoiseStrength = 0.1f;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreNoisePower = 4f;

	[Range(0f, 10f)]
	[SerializeField]
	private float _shoreNoiseIntensity = 4f;

	[Header("표면 수면 / 반사")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _reflectionIntensity = 0.3f;

	[Range(-0.5f, 0.5f)]
	[SerializeField]
	private float _reflectionYOffset;

	[Header("수면 Bottom / 높이")]
	[SerializeField]
	private float _waterBottomSurfaceHeight = -0.5f;

	[Header("수면 Bottom / 색상")]
	[SerializeField]
	private Color _waterBottomShallowColor = new Color(0.2f, 0.4f, 0.45f, 1f);

	[SerializeField]
	private Color _waterBottomColor = new Color(0.08f, 0.15f, 0.2f, 1f);

	[Header("수면 Bottom / 깊이 엣지")]
	[SerializeField]
	private float _waterBottomDepthDiffEnd = 0.125f;

	[SerializeField]
	private float _waterBottomEdgeDepthDiffEnd = 0.125f;

	[Range(1f, 10f)]
	[SerializeField]
	private float _waterBottomEdgePower = 4f;

	[Range(0f, 10f)]
	[SerializeField]
	private float _waterBottomEdgeIntensity = 4f;

	[Header("수면 Bottom / 반사")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _waterBottomReflectionIntensity = 0.5f;

	[SerializeField]
	private float _waterBottomReflectionYOffset = 0.5f;

	[Header("SDF 생성 (ComputeShader)")]
	[SerializeField]
	private ComputeShader _sdfComputeShader;

	[Range(1f, 32f)]
	[SerializeField]
	private float _sdfSpread = 8f;

	[Range(1f, 32f)]
	[SerializeField]
	private float _roofSdfSpread = 8f;

	[Range(1f, 8f)]
	[SerializeField]
	private int _sdfScale = 4;

	[Header("파동 (WaveCompute)")]
	[SerializeField]
	private ComputeShader _waveComputeShader;

	[SerializeField]
	private int _waveMapResolution = 512;

	[SerializeField]
	private float _waveMapSize = 50f;

	public Texture2D NoiseTexture => _noiseTexture;

	public Texture2D NormalMapTexture => _normalMapTexture;

	public float WaveSpeed => _waveSpeed;

	public float WaveScale => _waveScale;

	public float WaveDistortion => _waveDistortion;

	public float WaterSurfaceLevel => _waterSurfaceLevel;

	public float ShoreHeightDrop => _shoreHeightDrop;

	public float SurfaceEdgeWidth => _surfaceEdgeWidth;

	public float DepthOffset => _depthOffset;

	public Color ShallowColor => _shallowColor;

	public Color DeepColor => _deepColor;

	public float DepthMin => _depthMin;

	public float DepthMax => _depthMax;

	public float DepthLightFlatten => _depthLightFlatten;

	public float ShoreEdgeMin => _shoreEdgeMin;

	public float ShoreEdgeMax => _shoreEdgeMax;

	public float ShoreStroke => _shoreStroke;

	public float ShoreIntensity => _shoreIntensity;

	public float ShoreNoiseTiling => _shoreNoiseTiling;

	public float ShoreNoiseStrength => _shoreNoiseStrength;

	public float ShoreNoisePower => _shoreNoisePower;

	public float ShoreNoiseIntensity => _shoreNoiseIntensity;

	public float ReflectionIntensity => _reflectionIntensity;

	public float ReflectionYOffset => _reflectionYOffset;

	public float WaterBottomSurfaceHeight => _waterBottomSurfaceHeight;

	public Color WaterBottomShallowColor => _waterBottomShallowColor;

	public Color WaterBottomColor => _waterBottomColor;

	public float WaterBottomDepthDiffEnd => _waterBottomDepthDiffEnd;

	public float WaterBottomEdgeDepthDiffEnd => _waterBottomEdgeDepthDiffEnd;

	public float WaterBottomEdgePower => _waterBottomEdgePower;

	public float WaterBottomEdgeIntensity => _waterBottomEdgeIntensity;

	public float WaterBottomReflectionIntensity => _waterBottomReflectionIntensity;

	public float WaterBottomReflectionYOffset => _waterBottomReflectionYOffset;

	public ComputeShader SdfComputeShader => _sdfComputeShader;

	public float SdfSpread => _sdfSpread;

	public float RoofSdfSpread => _roofSdfSpread;

	public int SdfScale => _sdfScale;

	public ComputeShader WaveComputeShader => _waveComputeShader;

	public int WaveMapResolution => _waveMapResolution;

	public float WaveMapSize => _waveMapSize;

	public void ApplyShaderGlobals()
	{
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.SurfaceLevel, _waterSurfaceLevel);
		Shader.SetGlobalColor(MapVisualAreaShaderIds.ShallowColor, _shallowColor);
		Shader.SetGlobalColor(MapVisualAreaShaderIds.DeepColor, _deepColor);
		Shader.SetGlobalVector(MapVisualAreaShaderIds.ShoreEdge, new Vector4(_shoreEdgeMin, _shoreEdgeMax, 0f, 0f));
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.ShoreStroke, _shoreStroke);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.ShoreIntensity, _shoreIntensity);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.ShoreHeightDrop, _shoreHeightDrop);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.SurfaceEdgeWidth, _surfaceEdgeWidth);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.DepthOffset, _depthOffset);
		Shader.SetGlobalColor(MapVisualAreaShaderIds.BottomColor, _waterBottomColor);
		Shader.SetGlobalColor(MapVisualAreaShaderIds.BottomShallowColor, _waterBottomShallowColor);
		Shader.SetGlobalVector(MapVisualAreaShaderIds.BottomEdgeParams, new Vector4(_waterBottomEdgePower, _waterBottomEdgeIntensity, _waterBottomDepthDiffEnd, _waterBottomEdgeDepthDiffEnd));
		Shader.SetGlobalVector(MapVisualAreaShaderIds.BottomParams, new Vector4(_waterBottomSurfaceHeight, _waterBottomReflectionYOffset, _waterBottomReflectionIntensity, 0f));
		Shader.SetGlobalVector(MapVisualAreaShaderIds.WaveParams, new Vector4(_waveSpeed, _waveScale, _shoreNoisePower, _shoreNoiseIntensity));
		Shader.SetGlobalVector(MapVisualAreaShaderIds.WaveParams2, new Vector4(_waveDistortion, 0f, 0f, 0f));
		Shader.SetGlobalVector(MapVisualAreaShaderIds.DepthRange, new Vector4(_depthMin, _depthMax, _shoreNoiseStrength, _shoreNoiseTiling));
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.DepthLightFlatten, _depthLightFlatten);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.ReflectionIntensity, _reflectionIntensity);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.ReflectionYOffset, _reflectionYOffset);
		if (_noiseTexture != null)
		{
			Shader.SetGlobalTexture(MapVisualAreaShaderIds.NoiseTex, _noiseTexture);
		}
		if (_normalMapTexture != null)
		{
			Shader.SetGlobalTexture(MapVisualAreaShaderIds.NormalMap, _normalMapTexture);
		}
	}
}

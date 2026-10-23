using UnityEngine;

internal static class MapVisualAreaShaderIds
{
	public static readonly int AreaMap = Shader.PropertyToID("_GlobalWaterAreaMap");

	public static readonly int AreaEnable = Shader.PropertyToID("_GlobalWaterAreaEnable");

	public static readonly int SurfaceLevel = Shader.PropertyToID("_GlobalWaterSurfaceLevel");

	public static readonly int ShallowColor = Shader.PropertyToID("_GlobalWaterShallowColor");

	public static readonly int DeepColor = Shader.PropertyToID("_GlobalWaterDeepColor");

	public static readonly int ShoreEdge = Shader.PropertyToID("_GlobalWaterShoreEdge");

	public static readonly int ShoreStroke = Shader.PropertyToID("_GlobalWaterShoreStroke");

	public static readonly int ShoreIntensity = Shader.PropertyToID("_GlobalWaterShoreIntensity");

	public static readonly int ShoreHeightDrop = Shader.PropertyToID("_GlobalWaterShoreHeightDrop");

	public static readonly int SurfaceEdgeWidth = Shader.PropertyToID("_GlobalWaterSurfaceEdgeWidth");

	public static readonly int DepthOffset = Shader.PropertyToID("_GlobalWaterDepthOffset");

	public static readonly int BottomColor = Shader.PropertyToID("_GlobalWaterBottomColor");

	public static readonly int BottomShallowColor = Shader.PropertyToID("_GlobalWaterBottomShallowColor");

	public static readonly int BottomEdgeParams = Shader.PropertyToID("_GlobalWaterBottomEdgeParams");

	public static readonly int BottomParams = Shader.PropertyToID("_GlobalWaterBottomParams");

	public static readonly int NoiseTex = Shader.PropertyToID("_GlobalWaterNoiseTex");

	public static readonly int NormalMap = Shader.PropertyToID("_GlobalWaterNormalMap");

	public static readonly int WaveParams = Shader.PropertyToID("_GlobalWaterWaveParams");

	public static readonly int WaveParams2 = Shader.PropertyToID("_GlobalWaterWaveParams2");

	public static readonly int DepthRange = Shader.PropertyToID("_GlobalWaterDepthRange");

	public static readonly int DepthLightFlatten = Shader.PropertyToID("_GlobalWaterDepthLightFlatten");

	public static readonly int ReflectionIntensity = Shader.PropertyToID("_GlobalWaterReflectionIntensity");

	public static readonly int ReflectionYOffset = Shader.PropertyToID("_GlobalWaterReflectionYOffset");

	public static readonly int OverallMapBoundMin = Shader.PropertyToID("_GlobalOverallMapBoundMin");

	public static readonly int OverallMapBoundMax = Shader.PropertyToID("_GlobalOverallMapBoundMax");

	public static readonly int SectorWidth = Shader.PropertyToID("_GlobalSectorWidth");

	public static readonly int SectorHeight = Shader.PropertyToID("_GlobalSectorHeight");

	public static readonly int WaveResult = Shader.PropertyToID("_WaveResult");

	public static readonly int WaveMapParams = Shader.PropertyToID("_WaveMapParams");

	public static readonly int WaveMapResolution = Shader.PropertyToID("_WaveMapResolution");

	public static readonly int GlobalWaveMapTexture = Shader.PropertyToID("_GlobalWaterSurfaceWaveMap");

	public static readonly int GlobalWaveMapParams = Shader.PropertyToID("_GlobalWaterSurfaceWaveMapParams");
}

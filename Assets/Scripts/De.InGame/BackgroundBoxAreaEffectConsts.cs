using UnityEngine;

public class BackgroundBoxAreaEffectConsts
{
	public static readonly int NUMBER_BOX_AREA_COUNT_MAX = 8;

	public static readonly int SHADER_PROPERTY_ENABLE = Shader.PropertyToID("_GlobalBoxAreaEnable");

	public static readonly int SHADER_PROPERTY_CENTERS = Shader.PropertyToID("_GlobalBoxAreaCenters");

	public static readonly int SHADER_PROPERTY_EXTENTS = Shader.PropertyToID("_GlobalBoxAreaExtents");

	public static readonly int SHADER_PROPERTY_COLORS = Shader.PropertyToID("_GlobalBoxAreaColors");

	public static readonly int SHADER_PROPERTY_INTENSITIES = Shader.PropertyToID("_GlobalBoxAreaIntensities");

	public static readonly int SHADER_PROPERTY_SHIMMER_PARAMS = Shader.PropertyToID("_GlobalBoxAreaShimmerParams");

	public static readonly int SHADER_PROPERTY_COVER_CHARACTERS = Shader.PropertyToID("_GlobalBoxAreaCoverCharacters");

	public static readonly int SHADER_PROPERTY_COVER_WORLD_UIS = Shader.PropertyToID("_GlobalBoxAreaCoverWorldUIs");

	public static readonly int SHADER_PROPERTY_COVER_OUTLINES = Shader.PropertyToID("_GlobalBoxAreaCoverOutlines");
}

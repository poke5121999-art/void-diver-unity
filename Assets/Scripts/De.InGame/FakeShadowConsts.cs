using UnityEngine;

public class FakeShadowConsts
{
	public static readonly string SHADER_NAME = "LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow";

	public static readonly int SHADER_PROPERTY_POINT_COUNT = Shader.PropertyToID("_ScreenSpaceShadowPointCount");

	public static readonly int SHADER_PROPERTY_POINT_RANGES = Shader.PropertyToID("_ScreenSpaceShadowPointRanges");

	public static readonly int SHADER_PROPERTY_POINT_POSITIONS = Shader.PropertyToID("_ScreenSpaceShadowPointPositions");

	public static readonly int SHADER_PROPERTY_ALPHA = Shader.PropertyToID("_ScreenSpaceShadowAlpha");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_SHADOW_MASK = Shader.PropertyToID("_ScreenSpaceShadowMask");

	public static readonly int SHADER_PROPERTY_SHADOW_COLOR = Shader.PropertyToID("_GlobalShadowColor");

	public static readonly int SHADER_PROPERTY_SHADOW_STANDARD_Y = Shader.PropertyToID("_GlobalShadowStandardY");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_SHADOW_ALPHA = Shader.PropertyToID("_GlobalScreenSpaceShadowAlpha");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_ALPHA = Shader.PropertyToID("_GlobalScreenSpaceObjectShadowDirectionalAlpha");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_LENGTH = Shader.PropertyToID("_GlobalScreenSpaceObjectShadowDirectionalLength");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_ALPHA = Shader.PropertyToID("_GlobalScreenSpaceObjectShadowAdditionalAlpha");

	public static readonly int SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_LENGTH = Shader.PropertyToID("_GlobalScreenSpaceObjectShadowAdditionalLength");

	public static readonly int NUMBER_SHADOW_POINT_COUNT_MAX = 8;

	public static readonly int NUMBER_CAMERA_CHECK_COUNT_FRAME = 1;

	public static readonly float NUMBER_POINT_RANGE = 4.25f;

	public static readonly float NUMBER_POINT_CATSTER_RANGE = 3f;
}

using UnityEngine;

public class ScreenBlurConsts
{
	public static readonly string SHADER_NAME_SCREEN_BLUR = "Hidden/LCArt/PostProcessing/Shader_PP_ScreenBlur";

	public static readonly int SHADER_PROPERTY_SCREEN_BLUR_DEGREE = Shader.PropertyToID("_ScreenBlurDegree");

	public static readonly int SHADER_PROPERTY_SCREEN_BLUR_PROGRESS = Shader.PropertyToID("_ScreenBlurProgress");

	public static readonly int SHADER_PROPERTY_SCREEN_BLUR_CUSTOM_SHADER_PROGRESS = Shader.PropertyToID("_ScreenBlurCustomShaderProgress");

	public static readonly string SHADER_NAME_SCREEN_HEIGHT_BLUR_SHIFT = "Hidden/LCArt/PostProcessing/Shader_PP_ScreenHeightBlur";

	public static readonly string SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_BUFFER = "_ScreenHeightBlurBuffer";

	public static readonly int SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_DEGREE = Shader.PropertyToID("_GlobalScreenHeightBlurDegree");

	public static readonly string SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MAX_RATIO = "_GlobalScreenHeightBlurMaxRatio";

	public static readonly string SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MIN_RATIO = "_GlobalScreenHeightBlurMinRatio";

	public static readonly int SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_SCALE = Shader.PropertyToID("_GlobalScreenHeightBlurScale");

	public static readonly string SHADER_NAME_DEPTH_BLUR = "Hidden/LCArt/PostProcessing/Shader_PP_ScreenDepthBlur";

	public static readonly string SHADER_PROPERTY_SCREEN_DEPTH_BLUR_BUFFER = "_ScreenDepthBlurBuffer";

	public static readonly int SHADER_PROPERTY_SCREEN_DEPTH_BLUR_DEGREE = Shader.PropertyToID("_GlobalScreenDepthBlurDegree");

	public static readonly string SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MAX_DISTANCE = "_GlobalScreenDepthBlurMaxDistance";

	public static readonly string SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MIN_DISTANCE = "_GlobalScreenDepthBlurMinDistance";

	public static readonly string SHADER_NAME_SCREEN_BLUR_UNIFIED = "Hidden/LCArt/PostProcessing/Shader_PP_ScreenBlurUnified";

	public static readonly int SHADER_PROPERTY_UNIFIED_BLUR_DEPTH_TEXTURE = Shader.PropertyToID("_UnifiedBlurDepthTexture");

	public static readonly int SHADER_PROPERTY_KAWASE_OFFSET = Shader.PropertyToID("_KawaseOffset");

	public static readonly int SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_DEGREE = Shader.PropertyToID("_UnifiedHeightBlurDegree");

	public static readonly int SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MAX_RATIO = Shader.PropertyToID("_UnifiedHeightBlurMaxRatio");

	public static readonly int SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MIN_RATIO = Shader.PropertyToID("_UnifiedHeightBlurMinRatio");

	public static readonly int SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_DEGREE = Shader.PropertyToID("_UnifiedDepthBlurDegree");

	public static readonly int SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MAX_DISTANCE = Shader.PropertyToID("_UnifiedDepthBlurMaxDistance");

	public static readonly int SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MIN_DISTANCE = Shader.PropertyToID("_UnifiedDepthBlurMinDistance");
}

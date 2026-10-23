using UnityEngine;

public class OutlineConsts
{
	public static readonly string SHADER_NAME = "Hidden/LCArt/PostProcessing/Shader_PP_Outline";

	public static readonly string SHADER_PASS_NAME = "Outline";

	public static readonly string SHADER_TAG_ID = "Outline";

	public static readonly float NUMBER_STROKE_MAX = 5f;

	public static readonly int SHADER_PROPERTY_OBJECT_ENABLE = Shader.PropertyToID("_OutlineEnable");

	public static readonly int SHADER_PROPERTY_OBJECT_COLOR = Shader.PropertyToID("_OutlineColor");

	public static readonly int SHADER_PROPERTY_OBJECT_STROKE = Shader.PropertyToID("_OutlineStroke");

	public static readonly int SHADER_PROPERTY_SCREEN_STROKE = Shader.PropertyToID("_ScreenOutlineStroke");

	public static readonly int SHADER_PROPERTY_SCREEN_ALPHA = Shader.PropertyToID("_ScreenOutlineAlpha");

	public static readonly string SHADER_NAME_STENCIL_WRITE = "Hidden/LCArt/Outline/StencilWrite";

	public static readonly string SHADER_NAME_OVERLAY = "Hidden/LCArt/Outline/Overlay";

	public static readonly string SHADER_NAME_IMAGE_OUTLINE = "Hidden/LCArt/WorldUI/DropItemOutline";

	public static readonly int SHADER_PROPERTY_PIXEL_OFFSET = Shader.PropertyToID("_OutlinePixelOffset");

	public static readonly int SHADER_PROPERTY_MAIN_TEX = Shader.PropertyToID("_MainTex");

	public static readonly int SHADER_PROPERTY_FLATTEN_VIEW_Z = Shader.PropertyToID("_OutlineFlattenViewZ");

	public static readonly int SHADER_PROPERTY_OPTICAL_Y_THRESHOLD = Shader.PropertyToID("_OpticalRatioYThreshold");

	public static readonly int SHADER_PROPERTY_OPTICAL_Y_POSITIVE = Shader.PropertyToID("_OpticalRatioYPositiveRatio");

	public static readonly int SHADER_PROPERTY_OPTICAL_Y_NEGATIVE = Shader.PropertyToID("_OpticalRatioYNegativeUnit");
}

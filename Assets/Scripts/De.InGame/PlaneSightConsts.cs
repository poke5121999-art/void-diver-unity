using UnityEngine;

public class PlaneSightConsts
{
	public static readonly int SHADER_PROPERTY_RENDER_TARGET = Shader.PropertyToID("_PlaneSightRenderTarget");

	public static readonly int SHADER_PROPERTY_ENABLE = Shader.PropertyToID("_GlobalPlaneSystemEnable");

	public static readonly int SHADER_PROPERTY_SIGHT_COUNT = Shader.PropertyToID("_SightCount");

	public static readonly int SHADER_PROPERTY_RAY_BUFFER = Shader.PropertyToID("_SightBuffer");

	public static readonly int SHADER_PROPERTY_RAY_DETECTION_COUNT = Shader.PropertyToID("_SightRayDetectionCount");

	public static readonly int SHADER_PROPERTY_RAY_DETECTION_FRONT_COUNT = Shader.PropertyToID("_SightRayDetectionFrontCount");

	public static readonly int SHADER_PROPERTY_SIGHT_SHARPNESS_POWER = Shader.PropertyToID("_SightSharpnessPower");

	public static readonly int SHADER_PROPERTY_SIGHT_SHARPNESS_BOUNDARY = Shader.PropertyToID("_SightSharpnessBoundary");

	public static readonly int SHADER_PROPERTY_SIGHT_INTERSECTION_SMOOTHNESS = Shader.PropertyToID("_SightIntersectionSmoothness");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MIN = Shader.PropertyToID("_SeeThroughDistanceMin");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MAX = Shader.PropertyToID("_SeeThroughDistanceMax");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_BOUNDARY = Shader.PropertyToID("_SeeThroughBoundary");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_POWER = Shader.PropertyToID("_SeeThroughPower");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_HEIGHT = Shader.PropertyToID("_SeeThroughHeight");

	public static readonly int SHADER_PROPERTY_SEE_THROUGH_INTERSECTION_SMOOTHNESS = Shader.PropertyToID("_SeeThroughIntersectionSmoothness");

	public static readonly int SHADER_PROPERTY_SHAPER_COLOR = Shader.PropertyToID("_Color");

	public static readonly int SHADER_PROPERTY_SHAPER_INTENSITY = Shader.PropertyToID("_Intensity");

	public static readonly int SHADER_PROPERTY_SIGHT_POSITIONS = Shader.PropertyToID("_SightPositions");

	public static readonly int SHADER_PROPERTY_SIGHT_BACK_RANGES = Shader.PropertyToID("_SightBackRanges");

	public static readonly int SHADER_PROPERTY_SIGHT_FRONT_RANGES = Shader.PropertyToID("_SightFrontRanges");

	public static readonly int SHADER_PROPERTY_SIGHT_ALPHAS = Shader.PropertyToID("_SightAlphas");

	public static readonly int SHADER_PROPERTY_SIGHT_SILLHOUETTE_DISTANCE = Shader.PropertyToID("_SightSilleouetteDistance");

	public static readonly int SHADER_PROPERTY_SIGHT_COLOR_INNER = Shader.PropertyToID("_SightInnerColor");

	public static readonly int SHADER_PROPERTY_SIGHT_COLOR_BOUNDARY = Shader.PropertyToID("_SightBoundaryColor");

	public static readonly int SHADER_PROPERTY_SIGHT_COLOR_LERP = Shader.PropertyToID("_SightBoundaryLerp");

	public static readonly int SHADER_PROPERTY_SIGHT_COLOR_WIDTH = Shader.PropertyToID("_SightBoundaryWidth");
}

using UnityEngine;

public class PlanePointConsts
{
	public static readonly int NUMBER_PLANE_POINT_COUNT_MAX = 8;

	public static readonly int NUMBER_CAMERA_CHECK_INTERVAL = 1;

	public static readonly float NUMBER_CAMERA_TARGET_Y = 0f;

	public static readonly float NUMBER_CAMERA_RANGE_X = 15f;

	public static readonly float NUMBER_CAMERA_RANGE_Z = 15f;

	public static readonly int SHADER_PROPERTY_PLANE_POINT_COUNT = Shader.PropertyToID("_PlanePointCount");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_COLORS = Shader.PropertyToID("_PlanePointColors");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_POSITIONS = Shader.PropertyToID("_PlanePointPositions");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_RANGES = Shader.PropertyToID("_PlanePointRanges");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_INTENSITIES = Shader.PropertyToID("_PlanePointIntensities");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_ENABLE = Shader.PropertyToID("_GlobalPlanePointBufferEnable");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_COUNT = Shader.PropertyToID("_PlanePointBufferCount");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_COLORS = Shader.PropertyToID("_PlanePointBufferColors");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_POSITIONS = Shader.PropertyToID("_PlanePointBufferPositions");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_RANGES = Shader.PropertyToID("_PlanePointBufferRanges");

	public static readonly int SHADER_PROPERTY_PLANE_POINT_BUFFER_INTENSITIES = Shader.PropertyToID("_PlanePointBufferIntensities");

	public static readonly int SHADER_PROPERTY_RIM_EFFECT_COUNT = Shader.PropertyToID("_RimEffectCount");

	public static readonly int SHADER_PROPERTY_RIM_EFFECT_COLORS = Shader.PropertyToID("_RimEffectColors");

	public static readonly int SHADER_PROPERTY_RIM_EFFECT_POSITIONS = Shader.PropertyToID("_RimEffectPositions");

	public static readonly int SHADER_PROPERTY_RIM_EFFECT_RANGES = Shader.PropertyToID("_RimEffectRanges");

	public static readonly int SHADER_PROPERTY_RIM_EFFECT_INTENSITIES = Shader.PropertyToID("_RimEffectIntensities");

	public const string SHADER_PROPERTY_PLANE_POINT_BUFFER_RT_NAME = "_PlanePointBuffer";
}

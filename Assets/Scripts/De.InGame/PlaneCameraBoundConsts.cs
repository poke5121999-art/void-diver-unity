using UnityEngine;

public class PlaneCameraBoundConsts
{
	public static Vector3Int RESOLUTION_RENDER_TEXTURE = new Vector3Int(256, 1, 256);

	public static float NUMBER_CAMERA_BOUND_LENGTH = 40f;

	public static readonly int SHADER_PROPERTY_BOUND_POS_MAX = Shader.PropertyToID("_PlaneBoundMaxPos");

	public static readonly int SHADER_PROPERTY_BOUND_POS_MIN = Shader.PropertyToID("_PlaneBoundMinPos");

	public static readonly int SHADER_PROPERTY_BOUND_SIZE = Shader.PropertyToID("_PlaneBoundSize");

	public static readonly int SHADER_PROPERTY_BOUND_RESOLUTION = Shader.PropertyToID("_PlaneBoundResolution");
}

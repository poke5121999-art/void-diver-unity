using UnityEngine;
using VContainer;

[DefaultExecutionOrder(100)]
public class PlaneCameraBoundCore : MonoBase
{
	[Inject]
	private CameraManager _cameraManager;

	private Bounds _bounds;

	public static Vector3 CameraBoundsMinPos;

	public static Vector3 CameraBoundsMaxPos;

	public static Vector3 CameraBoundsResolution;

	public static Vector3 CameraBoundsSize;

	private Camera CurrentTargetCamera => (_cameraManager == null) ? null : _cameraManager.MainCamera;

	private void LateUpdate()
	{
		RenewBounds();
	}

	public static Vector3 NormalizePositionUsingBoundBox(in Vector3 worldPosition)
	{
		Vector3 result = default(Vector3);
		result.x = (worldPosition.x - CameraBoundsMinPos.x) / (CameraBoundsMaxPos.x - CameraBoundsMinPos.x);
		result.y = (worldPosition.y - CameraBoundsMinPos.y) / (CameraBoundsMaxPos.y - CameraBoundsMinPos.y);
		result.z = (worldPosition.z - CameraBoundsMinPos.z) / (CameraBoundsMaxPos.z - CameraBoundsMinPos.z);
		return result;
	}

	private void RenewBounds()
	{
		Camera currentTargetCamera = CurrentTargetCamera;
		if (currentTargetCamera == null)
		{
			return;
		}
		Transform transform = currentTargetCamera.transform;
		Vector3 forward = transform.forward;
		float num = Vector3.Dot(forward, Vector3.down);
		Vector3 vector;
		if (!Mathf.Approximately(0f, num))
		{
			float y = transform.position.y;
			vector = new Vector3((0f - forward.x) * y / num, (0f - forward.y) * y / num, (0f - forward.z) * y / num);
		}
		else
		{
			vector = Vector3.zero;
		}
		_bounds.center = transform.position - vector;
		_bounds.extents = Vector3.one * PlaneCameraBoundConsts.NUMBER_CAMERA_BOUND_LENGTH * 0.5f;
		CameraBoundsSize = _bounds.extents + _bounds.extents;
		CameraBoundsMinPos = _bounds.center - _bounds.extents;
		CameraBoundsMaxPos = _bounds.extents + _bounds.center;
		Vector3Int resolution = PlaneCameraBoundConsts.RESOLUTION_RENDER_TEXTURE;
		Shader.SetGlobalVector(PlaneCameraBoundConsts.SHADER_PROPERTY_BOUND_RESOLUTION, new Vector4(resolution.x, resolution.y, resolution.z, 1f));
		Shader.SetGlobalVector(PlaneCameraBoundConsts.SHADER_PROPERTY_BOUND_POS_MIN, CameraBoundsMinPos);
		Shader.SetGlobalVector(PlaneCameraBoundConsts.SHADER_PROPERTY_BOUND_POS_MAX, CameraBoundsMaxPos);
		Shader.SetGlobalVector(PlaneCameraBoundConsts.SHADER_PROPERTY_BOUND_SIZE, CameraBoundsSize);
	}
}

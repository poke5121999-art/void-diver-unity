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

	private Camera CurrentTargetCamera => null;

	private void LateUpdate()
	{
	}

	public static Vector3 NormalizePositionUsingBoundBox(in Vector3 worldPosition)
	{
		return default;
	}

	private void RenewBounds()
	{
	}
}

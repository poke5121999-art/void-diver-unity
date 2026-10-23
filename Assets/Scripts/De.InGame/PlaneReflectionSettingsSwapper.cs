using UnityEngine;
using VContainer;

public class PlaneReflectionSettingsSwapper : MonoBase
{
	[SerializeField]
	private PlaneReflectionSettings _defaultPlaneReflectionSettings;

	[SerializeField]
	private PlaneReflectionSettings _targetPlaneReflectionSettings;

	[Inject]
	private CameraManager _cameraManager;

	private PlaneReflectionCameraTarget _planeReflectionCameraTarget;

	protected override void Awake()
	{
		base.Awake();
		Camera camera = (_cameraManager == null) ? null : _cameraManager.MainCamera;
		_planeReflectionCameraTarget = (camera == null) ? null : camera.GetComponent<PlaneReflectionCameraTarget>();
	}

	private void OnEnable()
	{
		if (!(_planeReflectionCameraTarget == null))
		{
			_planeReflectionCameraTarget.ChangeSettings(_targetPlaneReflectionSettings);
		}
	}

	private void OnDisable()
	{
		if (!(_planeReflectionCameraTarget == null))
		{
			_planeReflectionCameraTarget.ChangeSettings(_defaultPlaneReflectionSettings);
		}
	}
}

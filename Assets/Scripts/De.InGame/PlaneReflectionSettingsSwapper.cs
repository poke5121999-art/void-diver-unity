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
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}

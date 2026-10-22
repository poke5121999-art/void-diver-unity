using UnityEngine;
using VContainer;

public class CutsceneCameraAdjuster : MonoBase
{
	[SerializeField]
	private Camera _cutsceneCamera;

	[Inject]
	private CameraManager _cameraManager;

	private CameraPostProcessingController _cameraPostProcessingController;

	private Camera _mainCamera;

	private Camera _uiCamera;

	protected override void Awake()
	{
		base.Awake();
		if (_cameraManager != null)
		{
			_mainCamera = _cameraManager.MainCamera;
			_uiCamera = _cameraManager.UiCamera;
			_cameraPostProcessingController = _cameraManager.GetComponent<CameraPostProcessingController>();
		}
	}

	private void OnEnable()
	{
		if (_cameraPostProcessingController != null)
		{
			_cameraPostProcessingController.SetGameplayPostProcessingVolumeEnable(enable: false);
		}
		if ((bool)_uiCamera)
		{
			_uiCamera.enabled = false;
		}
	}

	private void OnDisable()
	{
		if (_cameraPostProcessingController != null)
		{
			_cameraPostProcessingController.SetGameplayPostProcessingVolumeEnable(enable: true);
		}
		if ((bool)_uiCamera)
		{
			_uiCamera.enabled = true;
		}
	}
}

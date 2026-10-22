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
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}

using System;
using Cinemachine;
using NemoLib.Log;
using UnityEngine;
using VContainer;

public class CameraManager : MonoBase
{
	[SerializeField]
	private Camera _mainCamera;

	[SerializeField]
	private Camera _uiCamera;

	[SerializeField]
	private GameObject _playerVCamPrefab;

	[SerializeField]
	private Transform _playerVCamParent;

	[SerializeField]
	private GameObject _interiorVCamTemplate;

	[Inject]
	private SeriLogger _logger;

	private CinemachineVirtualCamera _playerCamera;

	private CinemachineBrain _cameraBrain;

	private bool _cameraShakeEnabled;

	private bool _blendOverridden;

	private CinemachineBlendDefinition _savedBlend;

	private IDisposable _blendRestoreTimer;

	public Camera MainCamera => null;

	public Camera UiCamera => null;

	protected override void Awake()
	{
	}

	public bool IsPlayerCameraActive()
	{
		return false;
	}

	public void ActivatePlayerCamera(Transform target)
	{
	}

	public void DeactivatePlayerCamera()
	{
	}

	public void WarpToFollow()
	{
	}

	public void SetCameraShakeEnabled(bool cameraShakeEnabled)
	{
	}

	public void Shake(float amplitude, float frequency, float duration)
	{
	}

	private void StopShake()
	{
	}

	public void FocusOnPosition(Vector3 position, float duration = -1f)
	{
	}

	public void ReleaseFocus(float duration = -1f)
	{
	}

	private void OverrideBlend(float duration)
	{
	}

	private void RestoreBlend()
	{
	}

	public virtual void Clear()
	{
	}

	public void ActivateUiCamera()
	{
	}

	public void DeactivateUiCamera()
	{
	}
}

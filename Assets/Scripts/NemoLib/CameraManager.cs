using System;
using Cinemachine;
using NemoLib.Log;
using UnityEngine;
using UniRx;
using VContainer;
using VContainer.Unity;

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

	private bool _cameraShakeEnabled = true;

	private bool _blendOverridden;

	private CinemachineBlendDefinition _savedBlend;

	private IDisposable _blendRestoreTimer;

	public Camera MainCamera => _mainCamera;

	public Camera UiCamera => _uiCamera;

	protected override void Awake()
	{
		this.InjectFromRootContainer();
		GameObject gameObject = UnityEngine.Object.Instantiate(_playerVCamPrefab, _playerVCamParent);
		_playerCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
		_cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public bool IsPlayerCameraActive()
	{
		if (!(_playerCamera != null))
		{
			return false;
		}
		return _playerCamera.gameObject.activeSelf;
	}

	public void ActivatePlayerCamera(Transform target)
	{
		_playerCamera.Follow = target;
		_playerCamera.gameObject.SetActive(true);
	}

	public void DeactivatePlayerCamera()
	{
		_playerCamera.Follow = null;
		_playerCamera.gameObject.SetActive(false);
	}

	public void WarpToFollow()
	{
		if (!(_playerCamera.Follow != null))
		{
			return;
		}
		CinemachineTransposer transposer = _playerCamera.GetCinemachineComponent<CinemachineTransposer>();
		if (transposer != null)
		{
			Vector3 offset = transposer.m_FollowOffset;
			Vector3 camPos = _playerCamera.transform.position;
			Vector3 delta = _playerCamera.Follow.position - (camPos - offset);
			_playerCamera.OnTargetObjectWarped(_playerCamera.Follow, delta);
			_playerCamera.transform.position = _playerCamera.Follow.position + offset;
		}
	}

	public void SetCameraShakeEnabled(bool cameraShakeEnabled)
	{
		_cameraShakeEnabled = cameraShakeEnabled;
	}

	public void Shake(float amplitude, float frequency, float duration)
	{
		if (_cameraShakeEnabled)
		{
			CinemachineBasicMultiChannelPerlin perlin = _playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			perlin.m_AmplitudeGain = amplitude;
			perlin.m_FrequencyGain = frequency;
			Observable.Timer(System.TimeSpan.FromSeconds(duration)).Subscribe(delegate
			{
				StopShake();
			}).AddTo(this);
		}
	}

	private void StopShake()
	{
		if (!(_playerCamera == null))
		{
			CinemachineBasicMultiChannelPerlin perlin = _playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			if (!(perlin == null))
			{
				perlin.m_AmplitudeGain = 0f;
				perlin.m_FrequencyGain = 0f;
			}
		}
	}

	public void FocusOnPosition(Vector3 position, float duration = -1f)
	{
		CinemachineTransposer transposer = _playerCamera.GetCinemachineComponent<CinemachineTransposer>();
		if (transposer != null)
		{
			Vector3 offset = transposer.m_FollowOffset;
			_interiorVCamTemplate.transform.position = offset + position;
			if (0f <= duration)
			{
				OverrideBlend(duration);
			}
			_interiorVCamTemplate.SetActive(true);
		}
	}

	public void ReleaseFocus(float duration = -1f)
	{
		if (0f <= duration)
		{
			OverrideBlend(duration);
			_interiorVCamTemplate.SetActive(false);
			_blendRestoreTimer = Observable.Timer(System.TimeSpan.FromSeconds(duration)).Subscribe(delegate
			{
				RestoreBlend();
			}).AddTo(this);
		}
		else
		{
			_interiorVCamTemplate.SetActive(false);
			_blendRestoreTimer?.Dispose();
			_blendRestoreTimer = null;
		}
	}

	private void OverrideBlend(float duration)
	{
		_blendRestoreTimer?.Dispose();
		_blendRestoreTimer = null;
		if (!_blendOverridden)
		{
			_savedBlend = _cameraBrain.m_DefaultBlend;
			_blendOverridden = true;
		}
		CinemachineBlendDefinition blend = _cameraBrain.m_DefaultBlend;
		blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
		blend.m_Time = duration;
		_cameraBrain.m_DefaultBlend = blend;
	}

	private void RestoreBlend()
	{
		_blendRestoreTimer?.Dispose();
		_blendRestoreTimer = null;
		if (_blendOverridden)
		{
			_cameraBrain.m_DefaultBlend = _savedBlend;
			_blendOverridden = false;
		}
	}

	public virtual void Clear()
	{
		StopShake();
		_interiorVCamTemplate.SetActive(false);
		RestoreBlend();
		if (_uiCamera != null)
		{
			_uiCamera.gameObject.SetActive(true);
		}
	}

	public void ActivateUiCamera()
	{
		if (_uiCamera != null)
		{
			_uiCamera.gameObject.SetActive(true);
		}
	}

	public void DeactivateUiCamera()
	{
		if (_uiCamera != null)
		{
			_uiCamera.gameObject.SetActive(false);
		}
	}
}

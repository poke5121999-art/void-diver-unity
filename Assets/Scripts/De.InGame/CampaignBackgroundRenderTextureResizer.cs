using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CampaignBackgroundRenderTextureResizer : MonoBehaviour
{
	[Header("Target Settings")]
	[SerializeField]
	private Camera _targetCamera;

	[SerializeField]
	private CinemachineVirtualCamera _virtualCamera;

	[SerializeField]
	private RawImage _targetRawImage;

	[Header("RenderTexture Settings")]
	[SerializeField]
	private int _defaultWidth = 1024;

	[SerializeField]
	private int _defaultHeight = 1024;

	[SerializeField]
	private int _depthBuffer = 24;

	[SerializeField]
	private RenderTextureFormat _format = (RenderTextureFormat)5;

	private int _lastWidth = -1;

	private int _lastHeight = -1;

	private RenderTexture _runtimeRenderTexture;

	private void OnEnable()
	{
		if (_targetCamera == null)
		{
			_targetCamera = GetComponent<Camera>();
		}
		if (_virtualCamera == null)
		{
			_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		}
		if (_targetCamera == null)
		{
			return;
		}
		CreateRuntimeRenderTexture();
		if (_targetRawImage != null)
		{
			_targetRawImage.gameObject.SetActive(value: true);
		}
		UpdateRenderTextureAndSensorSize();
	}

	private void OnDisable()
	{
		if (_targetRawImage != null)
		{
			_targetRawImage.gameObject.SetActive(value: false);
		}
		CleanupRuntimeRenderTexture();
	}

	private void OnDestroy()
	{
		CleanupRuntimeRenderTexture();
	}

	private void Update()
	{
		if (!(_targetRawImage == null) && CheckRawImageSizeChanged())
		{
			UpdateRenderTextureAndSensorSize();
		}
	}

	private bool CheckRawImageSizeChanged()
	{
		if (_targetRawImage == null)
		{
			return false;
		}
		RectTransform rectTransform = _targetRawImage.rectTransform;
		Canvas canvas = _targetRawImage.canvas;
		if (canvas == null)
		{
			return false;
		}
		float scaleFactor = canvas.scaleFactor;
		// FUN_180069e90 is Mathf.RoundToInt (machine code checked, same body as @0x183ee7ca0)
		int width = Mathf.RoundToInt(rectTransform.rect.width * scaleFactor);
		int height = Mathf.RoundToInt(rectTransform.rect.height * scaleFactor);
		if (width != _lastWidth || height != _lastHeight)
		{
			_lastWidth = width;
			_lastHeight = height;
			return true;
		}
		return false;
	}

	private void UpdateRenderTextureAndSensorSize()
	{
		if (_targetCamera == null || _targetRawImage == null || _runtimeRenderTexture == null)
		{
			return;
		}
		RectTransform rectTransform = _targetRawImage.rectTransform;
		Canvas canvas = _targetRawImage.canvas;
		if (canvas == null)
		{
			return;
		}
		float scaleFactor = canvas.scaleFactor;
		// FUN_180069e90 is Mathf.RoundToInt (machine code checked, same body as @0x183ee7ca0)
		int width = Mathf.RoundToInt(rectTransform.rect.width * scaleFactor);
		int height = Mathf.RoundToInt(rectTransform.rect.height * scaleFactor);
		if (width < 1 || height < 1)
		{
			return;
		}
		if (_runtimeRenderTexture.width != width || _runtimeRenderTexture.height != height)
		{
			RenderTexture runtimeRenderTexture = _runtimeRenderTexture;
			_runtimeRenderTexture = new RenderTexture(width, height, _depthBuffer, _format);
			_runtimeRenderTexture.name = "RuntimeCampaignRenderTexture";
			_runtimeRenderTexture.Create();
			_targetCamera.targetTexture = _runtimeRenderTexture;
			_targetRawImage.texture = _runtimeRenderTexture;
			DeferredGpuRelease.ReleasePersistentRT(runtimeRenderTexture);
		}
		if (_virtualCamera != null)
		{
			LensSettings lens = _virtualCamera.m_Lens;
			lens.SensorSize = new Vector2((float)width / (float)height, 1f);
			_virtualCamera.m_Lens = lens;
		}
	}

	private void CreateRuntimeRenderTexture()
	{
		if (_runtimeRenderTexture != null)
		{
			return;
		}
		_runtimeRenderTexture = new RenderTexture(_defaultWidth, _defaultHeight, _depthBuffer, _format);
		_runtimeRenderTexture.name = "RuntimeCampaignRenderTexture";
		_runtimeRenderTexture.Create();
		if (_targetCamera != null)
		{
			_targetCamera.targetTexture = _runtimeRenderTexture;
		}
		if (_targetRawImage != null)
		{
			_targetRawImage.texture = _runtimeRenderTexture;
		}
	}

	private void CleanupRuntimeRenderTexture()
	{
		if (_runtimeRenderTexture != null)
		{
			if (_targetCamera != null)
			{
				_targetCamera.targetTexture = null;
			}
			if (_targetRawImage != null)
			{
				_targetRawImage.texture = null;
			}
			DeferredGpuRelease.ReleasePersistentRT(_runtimeRenderTexture);
			_runtimeRenderTexture = null;
		}
	}
}

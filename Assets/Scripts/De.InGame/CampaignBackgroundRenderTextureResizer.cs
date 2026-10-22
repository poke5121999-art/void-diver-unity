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
	private int _defaultWidth;

	[SerializeField]
	private int _defaultHeight;

	[SerializeField]
	private int _depthBuffer;

	[SerializeField]
	private RenderTextureFormat _format;

	private int _lastWidth;

	private int _lastHeight;

	private RenderTexture _runtimeRenderTexture;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	private bool CheckRawImageSizeChanged()
	{
		return false;
	}

	private void UpdateRenderTextureAndSensorSize()
	{
	}

	private void CreateRuntimeRenderTexture()
	{
	}

	private void CleanupRuntimeRenderTexture()
	{
	}
}

using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

public class CameraRenderDataSwapper : MonoBase
{
	[SerializeField]
	private int _defaultRenderDataIndex;

	[SerializeField]
	private int _targetRenderDataIndex;

	[Inject]
	private CameraManager _cameraManager;

	private UniversalAdditionalCameraData _universalAdditionalCameraData;

	protected override void Awake()
	{
		base.Awake();
		_universalAdditionalCameraData = ((_cameraManager == null || _cameraManager.MainCamera == null) ? null : _cameraManager.MainCamera.GetComponent<UniversalAdditionalCameraData>());
	}

	private void OnEnable()
	{
		if (_universalAdditionalCameraData != null)
		{
			_universalAdditionalCameraData.SetRenderer(_targetRenderDataIndex);
		}
	}

	private void OnDisable()
	{
		if (_universalAdditionalCameraData != null)
		{
			_universalAdditionalCameraData.SetRenderer(_defaultRenderDataIndex);
		}
	}
}

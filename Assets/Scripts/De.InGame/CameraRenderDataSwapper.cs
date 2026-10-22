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
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}

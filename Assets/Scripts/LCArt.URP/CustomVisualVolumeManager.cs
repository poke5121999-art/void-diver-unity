using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class CustomVisualVolumeManager : MonoBehaviour
{
	[Header("Caching Component Setting")]
	[FormerlySerializedAs("_camera")]
	[SerializeField]
	protected Camera TargetCamera;

	[FormerlySerializedAs("_uacd")]
	[SerializeField]
	protected UniversalAdditionalCameraData TargetAdditionalCameraData;

	[Header("LCArt: Debug Info")]
	[SerializeField]
	private bool _customSSAOActive;

	[SerializeField]
	private bool _screenHeightBlurActive;

	[SerializeField]
	private bool _screenDepthBlurActive;

	[SerializeField]
	private bool _waterSurfaceActive;

	protected Light CachedDirectionalLight;

	protected VolumeStack VolumeStack;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	private void RefreshVolumeStack()
	{
	}

	private void LateUpdate()
	{
	}

	protected virtual void UpdateVisualEffects()
	{
	}

	protected virtual void SetDisableVisualEffects()
	{
	}

	private void UpdateCustomSSAO(VolumeStack volumeStack)
	{
	}

	private void UpdateScreenHeightBlur(VolumeStack volumeStack)
	{
	}

	private void UpdateScreenDepthBlur(VolumeStack volumeStack)
	{
	}

	protected virtual void UpdateScreenBlurUnified(VolumeStack volumeStack)
	{
	}

	private void UpdateWaterSurface(VolumeStack volumeStack)
	{
	}
}

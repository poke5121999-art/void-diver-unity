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
		EnsureComponents();
	}

	private void EnsureComponents()
	{
		if (TargetCamera == null)
		{
			TargetCamera = GetComponent<Camera>();
		}
		if (TargetAdditionalCameraData == null)
		{
			TargetAdditionalCameraData = GetComponent<UniversalAdditionalCameraData>();
		}
	}

	private void OnEnable()
	{
		EnsureComponents();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SetDisableVisualEffects();
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SetDisableVisualEffects();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		RefreshVolumeStack();
	}

	private void RefreshVolumeStack()
	{
		if (!(TargetAdditionalCameraData == null))
		{
			VolumeStack = VolumeManager.instance.stack;
			if (VolumeStack != null)
			{
				VolumeManager.instance.Update(VolumeStack, TargetAdditionalCameraData.volumeTrigger, TargetAdditionalCameraData.volumeLayerMask);
				UpdateVisualEffects();
			}
		}
	}

	private void LateUpdate()
	{
		VolumeStack = VolumeManager.instance.stack;
		if (VolumeStack != null)
		{
			VolumeManager.instance.Update(VolumeStack, TargetAdditionalCameraData.volumeTrigger, TargetAdditionalCameraData.volumeLayerMask);
			UpdateVisualEffects();
		}
	}

	protected virtual void UpdateVisualEffects()
	{
		UpdateCustomSSAO(VolumeStack);
		UpdateScreenHeightBlur(VolumeStack);
		UpdateScreenDepthBlur(VolumeStack);
	}

	protected virtual void SetDisableVisualEffects()
	{
		// body @0x182923fb0 read in full (machine code): exactly these seven globals, then one dword store clears the four bool flags at +0x30..0x33; the non-unified blur globals are not touched
		Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_ENABLE, 0f);
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_DEGREE, 0);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MAX_RATIO, 0.85f);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MIN_RATIO, 0.2f);
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_DEGREE, 0);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MAX_DISTANCE, 0f);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MIN_DISTANCE, -10f);
		_customSSAOActive = false;
		_screenHeightBlurActive = false;
		_screenDepthBlurActive = false;
		_waterSurfaceActive = false;
	}

	private void UpdateCustomSSAO(VolumeStack volumeStack)
	{
		CustomScreenSpaceAmbientOcclusionVolumeComponent component = volumeStack.GetComponent<CustomScreenSpaceAmbientOcclusionVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_ENABLE, 0f);
			_customSSAOActive = false;
			return;
		}
		Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_ENABLE, 1f);
		if (component.SSAOColor.overrideState)
		{
			Shader.SetGlobalColor(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_COLOR, component.SSAOColor.value);
		}
		if (component.SSAOIntensity.overrideState)
		{
			Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_INTENSITY, component.SSAOIntensity.value);
		}
		if (component.SSAORadius.overrideState)
		{
			Shader.SetGlobalFloat(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_OVERRIDE_RADIUS, component.SSAORadius.value);
		}
		_customSSAOActive = true;
	}

	private void UpdateScreenHeightBlur(VolumeStack volumeStack)
	{
		ScreenHeightBlurVolumeComponent component = volumeStack.GetComponent<ScreenHeightBlurVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_DEGREE, 1);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MAX_RATIO, 0.85f);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MIN_RATIO, 0.2f);
			_screenHeightBlurActive = false;
			return;
		}
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_DEGREE, component.ScreenHeightBlurDegree.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MAX_RATIO, component.ScreenHeightMaxRatio.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_MIN_RATIO, component.ScreenHeightMinRatio.value);
		_screenHeightBlurActive = true;
	}

	private void UpdateScreenDepthBlur(VolumeStack volumeStack)
	{
		ScreenDepthBlurVolumeComponent component = volumeStack.GetComponent<ScreenDepthBlurVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_DEGREE, 2);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MAX_DISTANCE, 0f);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MIN_DISTANCE, -10f);
			_screenDepthBlurActive = false;
			return;
		}
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_DEGREE, component.ScreenDepthBlurDegree.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MAX_DISTANCE, component.ScreenDepthMaxDistance.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_DEPTH_BLUR_MIN_DISTANCE, component.ScreenDepthMinDistance.value);
		_screenDepthBlurActive = true;
	}

	protected virtual void UpdateScreenBlurUnified(VolumeStack volumeStack)
	{
		ScreenBlurUnifiedVolumeComponent component = volumeStack.GetComponent<ScreenBlurUnifiedVolumeComponent>();
		if (component == null || !component.IsActive())
		{
			Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_DEGREE, 1);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MAX_RATIO, 0.85f);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MIN_RATIO, 0.2f);
			Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_DEGREE, 2);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MAX_DISTANCE, 0f);
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MIN_DISTANCE, -10f);
			_screenHeightBlurActive = false;
			_screenDepthBlurActive = false;
			return;
		}
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_DEGREE, component.ScreenHeightBlurDegree.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MAX_RATIO, component.ScreenHeightMaxRatio.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_HEIGHT_BLUR_MIN_RATIO, component.ScreenHeightMinRatio.value);
		Shader.SetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_DEGREE, component.ScreenDepthBlurDegree.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MAX_DISTANCE, component.ScreenDepthMaxDistance.value);
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_UNIFIED_DEPTH_BLUR_MIN_DISTANCE, component.ScreenDepthMinDistance.value);
		_screenHeightBlurActive = true;
		_screenDepthBlurActive = true;
	}

	private void UpdateWaterSurface(VolumeStack volumeStack)
	{
	}
}

using LCArt.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[RequireComponent(typeof(GraphicOptionModel))]
public class GraphicOptionController : MonoBehaviour
{
	[SerializeField]
	private bool _bloomActive = true;

	[SerializeField]
	[FormerlySerializedAs("_brightnessVinetteActive")]
	private bool _dynamicVinetteActive = true;

	[SerializeField]
	private bool _screenSpaceAmbientOcclusionActive = true;

	[SerializeField]
	private EGraphicOption _screenBlurOption = EGraphicOption.Medium;

	[SerializeField]
	private EGraphicOption _uiScreenBlurOption = EGraphicOption.Medium;

	[SerializeField]
	private bool _campaignTiltActive = true;

	[SerializeField]
	private bool _postProcessingActive = true;

	[SerializeField]
	private bool _blobShadowEnable;

	[SerializeField]
	private bool _casterDirectionalOnly;

	[SerializeField]
	private bool _mouseCursorActive;

	private const float _gammaDegreeMin = -1f;

	private const float _gammaDegreeMax = 1f;

	private const float _gammaExponentMin = 0.3f;

	private const float _gammaExponentMax = 1.7f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _gammaDegree;

	private int _globalGammaProperty = Shader.PropertyToID("_GlobalGammaDegree");

	private readonly int _globalShadowStepCountProperty = Shader.PropertyToID("_SSShadowStepCount");

	[SerializeField]
	private EGraphicOption _screenSpaceShadowOption = EGraphicOption.High;

	[SerializeField]
	private EGraphicOption _planeReflectionOption = EGraphicOption.High;

	[SerializeField]
	private EGraphicOption _customDepthOption = EGraphicOption.High;

	[SerializeField]
	private int _rtMaxHeight = 1080;

	[SerializeField]
	[Range(0.5f, 1f)]
	private float _renderScale = 1f;

	private int _particleRaycastBudget = 256;

	private bool _grabSpineEnable = true;

	private bool _grabGlassEnable = true;

	private bool _grabEffectEnable = true;

	private CustomCameraUtils.ECustomCameraResolutionType _grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	private CustomCameraUtils.ECustomCameraResolutionType _grabGlassRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	private CustomCameraUtils.ECustomCameraResolutionType _grabEffectRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	[Header("Quality Preset (SO 튜닝 테이블)")]
	[SerializeField]
	private GraphicQualityPreset[] _qualityPresets = new GraphicQualityPreset[4];

	[Header("Render Pass Events")]
	[SerializeField]
	private RenderPassEvent _screenSpaceShadowRenderPassEvent = (RenderPassEvent)300;

	[SerializeField]
	private RenderPassEvent _ambientOcclusionRenderPassEvent = (RenderPassEvent)300;

	private GraphicOptionModel _graphicOptionModel;

	private const string _blobShadowKeyword = "_DE_FAKE_SHADOW_BLOB";

	private const string _directionalOnlyKeyword = "_DE_FAKE_SHADOW_DIRECTIONAL_ONLY";

	private const string _gateAo = "_VD_DISABLE_AO";

	private const string _gateScreenBlur = "_VD_DISABLE_SCREENBLUR";

	private const string _gateUiBlur = "_VD_DISABLE_UIBLUR";

	private const string _gateDynamicVignette = "_VD_DISABLE_DYNVIGNETTE";

	private const string _gateCampaignTilt = "_VD_DISABLE_CAMPAIGNTILT";

	private const string _grabSpineProperty = "_VD_GRAB_SPINE";

	private const string _grabGlassProperty = "_VD_GRAB_GLASS";

	private const string _grabEffectProperty = "_VD_GRAB_EFFECT";

	private void Awake()
	{
		_graphicOptionModel = GetComponent<GraphicOptionModel>();
	}

	private void Start()
	{
		// Same quality-table + preset + refresh sequence as OnQualityLevelChanged (identical in the binary).
		int level = QualitySettings.GetQualityLevel();
		OnQualityLevelChanged(level, level);
	}

	private void OnEnable()
	{
		QualitySettings.activeQualityLevelChanged -= OnQualityLevelChanged;
		QualitySettings.activeQualityLevelChanged += OnQualityLevelChanged;
	}

	private void OnDisable()
	{
		QualitySettings.activeQualityLevelChanged -= OnQualityLevelChanged;
	}

	private void OnValidate()
	{
		UpdateVisualEffects();
	}

	private void OnQualityLevelChanged(int previousQuality, int newQuality)
	{
		_grabSpineEnable = true;
		_grabGlassEnable = true;
		_grabEffectRes = CustomCameraUtils.ECustomCameraResolutionType.Full;
		bool lowTable = true;
		if (newQuality == 0)
		{
			_bloomActive = false;
			_dynamicVinetteActive = false;
			_campaignTiltActive = false;
			_postProcessingActive = false;
			_blobShadowEnable = true;
			_casterDirectionalOnly = false;
			_rtMaxHeight = 480;
			_grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Quarter;
		}
		else if (newQuality == 1)
		{
			_bloomActive = false;
			_dynamicVinetteActive = true;
			_campaignTiltActive = true;
			_postProcessingActive = false;
			_blobShadowEnable = false;
			_casterDirectionalOnly = true;
			_rtMaxHeight = 720;
			_grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Half;
		}
		else
		{
			lowTable = false;
			_bloomActive = true;
			_dynamicVinetteActive = true;
			_screenSpaceAmbientOcclusionActive = true;
			_campaignTiltActive = true;
			_postProcessingActive = true;
			_blobShadowEnable = false;
			_casterDirectionalOnly = false;
			_customDepthOption = EGraphicOption.High;
			_grabGlassRes = CustomCameraUtils.ECustomCameraResolutionType.Full;
			_grabEffectEnable = true;
			if (newQuality == 2)
			{
				_screenSpaceShadowOption = EGraphicOption.Medium;
				_planeReflectionOption = EGraphicOption.Medium;
				_screenBlurOption = EGraphicOption.Medium;
				_uiScreenBlurOption = EGraphicOption.Medium;
				_rtMaxHeight = 1080;
				_grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Half;
			}
			else
			{
				_screenSpaceShadowOption = EGraphicOption.High;
				_planeReflectionOption = EGraphicOption.High;
				_screenBlurOption = EGraphicOption.High;
				_uiScreenBlurOption = EGraphicOption.High;
				_rtMaxHeight = 1440;
				_grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Full;
			}
		}
		if (lowTable)
		{
			_grabGlassRes = CustomCameraUtils.ECustomCameraResolutionType.Half;
			_grabEffectEnable = false;
			_screenBlurOption = EGraphicOption.Off;
			_uiScreenBlurOption = EGraphicOption.Off;
			_screenSpaceAmbientOcclusionActive = false;
			_customDepthOption = EGraphicOption.Medium;
			_screenSpaceShadowOption = EGraphicOption.Low;
			_planeReflectionOption = EGraphicOption.Off;
		}
		ApplyPresetIfAvailable(newQuality);
		UpdateVisualEffects();
	}

	private void ApplyPresetIfAvailable(int level)
	{
		if (_qualityPresets == null || level < 0 || level >= _qualityPresets.Length)
		{
			return;
		}
		GraphicQualityPreset preset = _qualityPresets[level];
		if (preset == null)
		{
			return;
		}
		_renderScale = preset.RenderScale;
		_bloomActive = preset.BloomActive;
		_screenSpaceShadowOption = preset.ScreenSpaceShadowOption;
		_planeReflectionOption = preset.PlaneReflectionOption;
		_screenSpaceAmbientOcclusionActive = preset.ScreenSpaceAmbientOcclusionActive;
		_dynamicVinetteActive = preset.DynamicVinetteActive;
		_screenBlurOption = preset.ScreenBlurOption;
		_uiScreenBlurOption = preset.UIScreenBlurOption;
		_campaignTiltActive = preset.CampaignTiltActive;
		_customDepthOption = preset.CustomDepthOption;
		_postProcessingActive = preset.PostProcessingActive;
		_blobShadowEnable = preset.BlobShadowEnable;
		_casterDirectionalOnly = preset.CasterDirectionalOnly;
		_grabSpineEnable = preset.GrabSpineEnable;
		_grabGlassEnable = preset.GrabGlassEnable;
		_grabEffectEnable = preset.GrabEffectEnable;
		_grabSpineRes = preset.GrabSpineRes;
		_grabGlassRes = preset.GrabGlassRes;
		_grabEffectRes = preset.GrabEffectRes;
		_rtMaxHeight = preset.RtMaxHeight;
		_particleRaycastBudget = preset.ParticleRaycastBudget;
	}

	private void UpdateVisualEffects()
	{
		UpdateScreenSpaceShadow();
		UpdatePlaneReflection();
		UpdateBloom();
		UpdateGamma();
		UpdateDynamicVignette();
		UpdateScreenBlur();
		UpdateUIScreenBlur();
		UpdateCampaignTilt();
		UpdateScreenSpaceAmbientOcclusion();
		UpdateCustomDepth();
		UpdateGrabpassEntries();
		UpdateBlobShadowEnable();
		UpdateCasterDirectionalOnly();
		UpdatePostProcessingActive();
		UpdateRtMaxHeight();
		UpdateCursorLine();
		UpdateRenderScale();
		UpdateParticleBudget();
	}

	private void UpdateRenderScale()
	{
		UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
		if (asset != null)
		{
			asset.renderScale = _renderScale;
		}
	}

	private void UpdateParticleBudget()
	{
		QualitySettings.particleRaycastBudget = _particleRaycastBudget;
	}

	private void UpdateRtMaxHeight()
	{
		Shader.SetGlobalFloat("_VD_RT_MAX_HEIGHT", _rtMaxHeight);
	}

	private void UpdateGrabpassEntries()
	{
		Shader.SetGlobalFloat("_VD_GRAB_SPINE", _grabSpineEnable ? (int)_grabSpineRes : 0);
		Shader.SetGlobalFloat("_VD_GRAB_GLASS", _grabGlassEnable ? (int)_grabGlassRes : 0);
		Shader.SetGlobalFloat("_VD_GRAB_EFFECT", _grabEffectEnable ? (int)_grabEffectRes : 0);
	}

	private static void SetGateGlobal(string property, bool enabled)
	{
		Shader.SetGlobalFloat(Shader.PropertyToID(property), enabled ? 0f : 1f);
	}

	private void UpdateBlobShadowEnable()
	{
		if (_blobShadowEnable)
		{
			Shader.EnableKeyword("_DE_FAKE_SHADOW_BLOB");
		}
		else
		{
			Shader.DisableKeyword("_DE_FAKE_SHADOW_BLOB");
		}
	}

	private void UpdateCasterDirectionalOnly()
	{
		if (_casterDirectionalOnly)
		{
			Shader.EnableKeyword("_DE_FAKE_SHADOW_DIRECTIONAL_ONLY");
		}
		else
		{
			Shader.DisableKeyword("_DE_FAKE_SHADOW_DIRECTIONAL_ONLY");
		}
	}

	public void SetScreenSpaceShadowOption(EGraphicOption option)
	{
		_screenSpaceShadowOption = option;
		UpdateScreenSpaceShadow();
	}

	private void UpdateScreenSpaceShadow()
	{
		if (_graphicOptionModel == null)
		{
			return;
		}
		bool active = _screenSpaceShadowOption != EGraphicOption.Off;
		float stepCount;
		switch (_screenSpaceShadowOption)
		{
		case EGraphicOption.Low:
			stepCount = 16f;
			break;
		case EGraphicOption.High:
			stepCount = 32f;
			break;
		default:
			stepCount = 24f;
			break;
		}
		Shader.SetGlobalFloat(_globalShadowStepCountProperty, stepCount);
		ScriptableRendererFeature feature = _graphicOptionModel.ScreenSpaceShadowRendererFeature;
		if (feature != null)
		{
			feature.SetActive(active);
			if (feature is IRenderPassEventAware aware)
			{
				aware.SetRenderPassEvent(_screenSpaceShadowRenderPassEvent);
			}
		}
		ScriptableRendererFeature cutscene = _graphicOptionModel.CutsceneScreenSpaceShadowRendererFeature;
		if (cutscene != null)
		{
			cutscene.SetActive(active);
			if (cutscene is IRenderPassEventAware cutsceneAware)
			{
				cutsceneAware.SetRenderPassEvent(_screenSpaceShadowRenderPassEvent);
			}
		}
	}

	public void SetPlaneReflectionOption(EGraphicOption option)
	{
		_planeReflectionOption = option;
		UpdatePlaneReflection();
	}

	private bool IsPlaneReflectionValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.ReflectionBackgroundFeature != null && _graphicOptionModel.ReflectionObjectFeature != null;
	}

	private bool IsCutscenePlaneReflectionValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CutsceneReflectionBackgroundFeature != null && _graphicOptionModel.CutsceneReflectionObjectFeature != null;
	}

	private void UpdatePlaneReflection()
	{
		bool active = _planeReflectionOption != EGraphicOption.Off;
		int resolutionType;
		switch (_planeReflectionOption)
		{
		case EGraphicOption.Low:
			resolutionType = 4;
			break;
		case EGraphicOption.Medium:
			resolutionType = 2;
			break;
		default:
			resolutionType = 1;
			break;
		}
		if (IsPlaneReflectionValid())
		{
			_graphicOptionModel.ReflectionBackgroundFeature.SetActive(active);
			_graphicOptionModel.ReflectionObjectFeature.SetActive(active);
		}
		if (IsCutscenePlaneReflectionValid())
		{
			_graphicOptionModel.CutsceneReflectionBackgroundFeature.SetActive(active);
			_graphicOptionModel.CutsceneReflectionObjectFeature.SetActive(active);
		}
		if (_planeReflectionOption == EGraphicOption.Off)
		{
			Shader.SetGlobalFloat(PlaneReflectionConsts.OPTION_DISABLE_REFLECTION, 1f);
			return;
		}
		Shader.SetGlobalFloat(PlaneReflectionConsts.OPTION_DISABLE_REFLECTION, 0f);
		Shader.SetGlobalInt(PlaneReflectionConsts.OPTION_RESOLUTION_TYPE, resolutionType);
	}

	public void SetBloomActive(bool active)
	{
		_bloomActive = active;
		UpdateBloom();
	}

	private Bloom GetValidBloom()
	{
		if (_graphicOptionModel == null || _graphicOptionModel.MainVolume == null)
		{
			return null;
		}
		VolumeProfile profile = _graphicOptionModel.MainVolume.profile;
		Bloom bloom = null;
		if (profile != null)
		{
			profile.TryGet(out bloom);
		}
		return bloom;
	}

	private void UpdateBloom()
	{
		Bloom bloom = GetValidBloom();
		if (bloom)
		{
			bloom.active = _bloomActive;
		}
	}

	public void SetGammaDegree(float degree)
	{
		_gammaDegree = degree;
		UpdateGamma();
	}

	private void UpdateGamma()
	{
		_gammaDegree = Mathf.Clamp(_gammaDegree, -1f, 1f);
		float t = Mathf.Clamp01(Mathf.Clamp01((_gammaDegree - -1f) * 0.5f));
		Shader.SetGlobalFloat(_globalGammaProperty, Mathf.Lerp(0.3f, 1.7f, t));
	}

	public void SetDynamicVignetteActive(bool active)
	{
		_dynamicVinetteActive = active;
		UpdateDynamicVignette();
	}

	private bool IsDynamicVignetteValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.DynamicVignetteResultFeature != null;
	}

	private void UpdateDynamicVignette()
	{
		SetGateGlobal("_VD_DISABLE_DYNVIGNETTE", _dynamicVinetteActive);
		if (IsDynamicVignetteValid())
		{
			_graphicOptionModel.DynamicVignetteResultFeature.SetActive(_dynamicVinetteActive);
		}
	}

	public void SetScreenBlurOption(EGraphicOption option)
	{
		_screenBlurOption = option;
		UpdateScreenBlur();
	}

	private void UpdateScreenBlur()
	{
		if (_graphicOptionModel == null)
		{
			return;
		}
		bool active = _screenBlurOption != EGraphicOption.Off;
		SetGateGlobal("_VD_DISABLE_SCREENBLUR", active);
		GetBlurParams(_screenBlurOption, out int downsample, out int iterations);
		ApplyBlurToFeature(_graphicOptionModel.UnifiedScreenBlurFeature, active, downsample, iterations);
		ApplyBlurToFeature(_graphicOptionModel.CutsceneUnifiedScreenBlurFeature, active, downsample, iterations);
	}

	private void ApplyBlurToFeature(ScreenBlurUnifiedRendererFeature feature, bool active, int downsample, int iterations)
	{
		if (!(feature == null))
		{
			feature.SetActive(active);
			if (active)
			{
				feature.SetDownsample(downsample);
				feature.SetIterations(iterations);
			}
		}
	}

	private void GetBlurParams(EGraphicOption option, out int downsample, out int iterations)
	{
		if (option == EGraphicOption.Low)
		{
			downsample = 2;
			iterations = 2;
		}
		else
		{
			downsample = 1;
			iterations = 2;
		}
	}

	public void SetUIScreenBlurOption(EGraphicOption option)
	{
		_uiScreenBlurOption = option;
		UpdateUIScreenBlur();
	}

	private void UpdateUIScreenBlur()
	{
		if (_graphicOptionModel == null)
		{
			return;
		}
		bool active = _uiScreenBlurOption != EGraphicOption.Off;
		SetGateGlobal("_VD_DISABLE_UIBLUR", active);
		ScreenBlurFeature feature = _graphicOptionModel.UIScreenBlurFeature;
		if (feature == null)
		{
			return;
		}
		feature.SetActive(active);
		if (active)
		{
			GetUIBlurParams(_uiScreenBlurOption, out int downsample, out int iterations);
			feature.SetDownsample(downsample);
			feature.SetIterations(iterations);
		}
	}

	private void GetUIBlurParams(EGraphicOption option, out int downsample, out int iterations)
	{
		if (option == EGraphicOption.Low)
		{
			downsample = 2;
			iterations = 2;
		}
		else
		{
			downsample = 1;
			iterations = 3;
		}
	}

	public void SetCampaignTiltActive(bool active)
	{
		_campaignTiltActive = active;
		UpdateCampaignTilt();
	}

	private void UpdateCampaignTilt()
	{
		SetGateGlobal("_VD_DISABLE_CAMPAIGNTILT", _campaignTiltActive);
		if (_graphicOptionModel != null && _graphicOptionModel.CampaignTiltFeature != null)
		{
			_graphicOptionModel.CampaignTiltFeature.SetActive(_campaignTiltActive);
		}
	}

	public void SetScreenSpaceAmbientOcclusionActive(bool active)
	{
		_screenSpaceAmbientOcclusionActive = active;
		UpdateScreenSpaceAmbientOcclusion();
	}

	private bool IsScreenSpaceAmbientOcclusionValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CustomScreenSpaceAmbientOcclusion != null;
	}

	private bool IsCutsceneScreenSpaceAmbientOcclusionValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CutsceneCustomScreenSpaceAmbientOcclusion != null;
	}

	private void UpdateScreenSpaceAmbientOcclusion()
	{
		SetGateGlobal("_VD_DISABLE_AO", _screenSpaceAmbientOcclusionActive);
		if (IsScreenSpaceAmbientOcclusionValid())
		{
			_graphicOptionModel.CustomScreenSpaceAmbientOcclusion.SetActive(_screenSpaceAmbientOcclusionActive);
			_graphicOptionModel.CustomScreenSpaceAmbientOcclusion.SetRenderPassEvent(_ambientOcclusionRenderPassEvent);
		}
		if (IsCutsceneScreenSpaceAmbientOcclusionValid())
		{
			_graphicOptionModel.CutsceneCustomScreenSpaceAmbientOcclusion.SetActive(_screenSpaceAmbientOcclusionActive);
			_graphicOptionModel.CutsceneCustomScreenSpaceAmbientOcclusion.SetRenderPassEvent(_ambientOcclusionRenderPassEvent);
		}
	}

	public void SetCustomDepthOption(EGraphicOption option)
	{
		_customDepthOption = option;
		UpdateCustomDepth();
	}

	private bool IsCustomDepthValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CustomDepthBufferRendererFeature != null;
	}

	private bool IsCutsceneCustomDepthValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CutsceneCustomDepthBufferRendererFeature != null;
	}

	private void UpdateCustomDepth()
	{
		CustomCameraUtils.ECustomCameraResolutionType resolutionType;
		switch (_customDepthOption)
		{
		case EGraphicOption.Medium:
			resolutionType = CustomCameraUtils.ECustomCameraResolutionType.Quarter;
			break;
		case EGraphicOption.High:
			resolutionType = CustomCameraUtils.ECustomCameraResolutionType.Half;
			break;
		default:
			resolutionType = CustomCameraUtils.ECustomCameraResolutionType.Eighth;
			break;
		}
		if (IsCustomDepthValid())
		{
			_graphicOptionModel.CustomDepthBufferRendererFeature.SetResolution(resolutionType);
		}
		if (IsCutsceneCustomDepthValid())
		{
			_graphicOptionModel.CutsceneCustomDepthBufferRendererFeature.SetResolution(resolutionType);
		}
	}

	public void SetPostProcessingActive(bool active)
	{
		_postProcessingActive = active;
		UpdatePostProcessingActive();
	}

	private bool IsPostProcessingValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.MainRenderingFeature != null;
	}

	private bool IsCutscenePostProcessingValid()
	{
		return _graphicOptionModel != null && _graphicOptionModel.CutsceneRenderingFeature != null;
	}

	private void UpdatePostProcessingActive()
	{
		if (IsPostProcessingValid())
		{
			_graphicOptionModel.MainRenderingFeature.postProcessData = (_postProcessingActive ? _graphicOptionModel.PostProcessData : null);
		}
		if (IsCutscenePostProcessingValid())
		{
			_graphicOptionModel.CutsceneRenderingFeature.postProcessData = (_postProcessingActive ? _graphicOptionModel.PostProcessData : null);
		}
	}

	public void SetCursorLineEnable(bool targetEnable)
	{
		_mouseCursorActive = targetEnable;
		UpdateCursorLine();
	}

	private void UpdateCursorLine()
	{
		if (_mouseCursorActive)
		{
			Shader.EnableKeyword("_CURSOR_LINE_ENABLE");
		}
		else
		{
			Shader.DisableKeyword("_CURSOR_LINE_ENABLE");
		}
	}
}

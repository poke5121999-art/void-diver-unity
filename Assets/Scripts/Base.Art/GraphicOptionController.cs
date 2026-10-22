using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[RequireComponent(typeof(GraphicOptionModel))]
public class GraphicOptionController : MonoBehaviour
{
	[SerializeField]
	private bool _bloomActive;

	[SerializeField]
	[FormerlySerializedAs("_brightnessVinetteActive")]
	private bool _dynamicVinetteActive;

	[SerializeField]
	private bool _screenSpaceAmbientOcclusionActive;

	[SerializeField]
	private EGraphicOption _screenBlurOption;

	[SerializeField]
	private EGraphicOption _uiScreenBlurOption;

	[SerializeField]
	private bool _campaignTiltActive;

	[SerializeField]
	private bool _postProcessingActive;

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

	private int _globalGammaProperty;

	private readonly int _globalShadowStepCountProperty;

	[SerializeField]
	private EGraphicOption _screenSpaceShadowOption;

	[SerializeField]
	private EGraphicOption _planeReflectionOption;

	[SerializeField]
	private EGraphicOption _customDepthOption;

	[SerializeField]
	private int _rtMaxHeight;

	[SerializeField]
	[Range(0.5f, 1f)]
	private float _renderScale;

	private int _particleRaycastBudget;

	private bool _grabSpineEnable;

	private bool _grabGlassEnable;

	private bool _grabEffectEnable;

	private CustomCameraUtils.ECustomCameraResolutionType _grabSpineRes;

	private CustomCameraUtils.ECustomCameraResolutionType _grabGlassRes;

	private CustomCameraUtils.ECustomCameraResolutionType _grabEffectRes;

	[Header("Quality Preset (SO 튜닝 테이블)")]
	[SerializeField]
	private GraphicQualityPreset[] _qualityPresets;

	[Header("Render Pass Events")]
	[SerializeField]
	private RenderPassEvent _screenSpaceShadowRenderPassEvent;

	[SerializeField]
	private RenderPassEvent _ambientOcclusionRenderPassEvent;

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
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnValidate()
	{
	}

	private void OnQualityLevelChanged(int previousQuality, int newQuality)
	{
	}

	private void ApplyPresetIfAvailable(int level)
	{
	}

	private void UpdateVisualEffects()
	{
	}

	private void UpdateRenderScale()
	{
	}

	private void UpdateParticleBudget()
	{
	}

	private void UpdateRtMaxHeight()
	{
	}

	private void UpdateGrabpassEntries()
	{
	}

	private static void SetGateGlobal(string property, bool enabled)
	{
	}

	private void UpdateBlobShadowEnable()
	{
	}

	private void UpdateCasterDirectionalOnly()
	{
	}

	public void SetScreenSpaceShadowOption(EGraphicOption option)
	{
	}

	private void UpdateScreenSpaceShadow()
	{
	}

	public void SetPlaneReflectionOption(EGraphicOption option)
	{
	}

	private bool IsPlaneReflectionValid()
	{
		return false;
	}

	private bool IsCutscenePlaneReflectionValid()
	{
		return false;
	}

	private void UpdatePlaneReflection()
	{
	}

	public void SetBloomActive(bool active)
	{
	}

	private Bloom GetValidBloom()
	{
		return null;
	}

	private void UpdateBloom()
	{
	}

	public void SetGammaDegree(float degree)
	{
	}

	private void UpdateGamma()
	{
	}

	public void SetDynamicVignetteActive(bool active)
	{
	}

	private bool IsDynamicVignetteValid()
	{
		return false;
	}

	private void UpdateDynamicVignette()
	{
	}

	public void SetScreenBlurOption(EGraphicOption option)
	{
	}

	private void UpdateScreenBlur()
	{
	}

	private void ApplyBlurToFeature(ScreenBlurUnifiedRendererFeature feature, bool active, int downsample, int iterations)
	{
	}

	private void GetBlurParams(EGraphicOption option, out int downsample, out int iterations)
	{
		downsample = default;
		iterations = default;
	}

	public void SetUIScreenBlurOption(EGraphicOption option)
	{
	}

	private void UpdateUIScreenBlur()
	{
	}

	private void GetUIBlurParams(EGraphicOption option, out int downsample, out int iterations)
	{
		downsample = default;
		iterations = default;
	}

	public void SetCampaignTiltActive(bool active)
	{
	}

	private void UpdateCampaignTilt()
	{
	}

	public void SetScreenSpaceAmbientOcclusionActive(bool active)
	{
	}

	private bool IsScreenSpaceAmbientOcclusionValid()
	{
		return false;
	}

	private bool IsCutsceneScreenSpaceAmbientOcclusionValid()
	{
		return false;
	}

	private void UpdateScreenSpaceAmbientOcclusion()
	{
	}

	public void SetCustomDepthOption(EGraphicOption option)
	{
	}

	private bool IsCustomDepthValid()
	{
		return false;
	}

	private bool IsCutsceneCustomDepthValid()
	{
		return false;
	}

	private void UpdateCustomDepth()
	{
	}

	public void SetPostProcessingActive(bool active)
	{
	}

	private bool IsPostProcessingValid()
	{
		return false;
	}

	private bool IsCutscenePostProcessingValid()
	{
		return false;
	}

	private void UpdatePostProcessingActive()
	{
	}

	public void SetCursorLineEnable(bool targetEnable)
	{
	}

	private void UpdateCursorLine()
	{
	}
}

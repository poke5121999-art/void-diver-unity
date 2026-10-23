using UnityEngine;

[CreateAssetMenu(menuName = "Project DE/Graphic Quality Preset")]
public class GraphicQualityPreset : ScriptableObject
{
	[Header("Render Scale")]
	[SerializeField]
	[Range(0.5f, 1f)]
	private float _renderScale = 1f;

	[Header("URP Features")]
	[SerializeField]
	private bool _bloomActive;

	[SerializeField]
	private EGraphicOption _screenSpaceShadowOption;

	[SerializeField]
	private EGraphicOption _planeReflectionOption;

	[SerializeField]
	private bool _screenSpaceAmbientOcclusionActive;

	[SerializeField]
	private bool _dynamicVinetteActive;

	[SerializeField]
	private EGraphicOption _screenBlurOption;

	[SerializeField]
	private EGraphicOption _uiScreenBlurOption;

	[SerializeField]
	private bool _campaignTiltActive;

	[SerializeField]
	private EGraphicOption _customDepthOption;

	[SerializeField]
	private bool _postProcessingActive;

	[SerializeField]
	private bool _blobShadowEnable;

	[SerializeField]
	private bool _casterDirectionalOnly;

	[Header("Grabpass")]
	[Tooltip("Spine 레이어 grab 활성 + 해상도")]
	[SerializeField]
	private bool _grabSpineEnable = true;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabSpineRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	[Tooltip("Glass 레이어 grab 활성 + 해상도")]
	[SerializeField]
	private bool _grabGlassEnable = true;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabGlassRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	[Tooltip("Effect 레이어 grab 활성 + 해상도")]
	[SerializeField]
	private bool _grabEffectEnable = true;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabEffectRes = CustomCameraUtils.ECustomCameraResolutionType.Full;

	[Header("중간버퍼 RT 높이 상한")]
	[SerializeField]
	private int _rtMaxHeight = 1080;

	[Header("Quality Settings")]
	[SerializeField]
	private int _particleRaycastBudget = 256;

	public float RenderScale => _renderScale;

	public bool BloomActive => _bloomActive;

	public EGraphicOption ScreenSpaceShadowOption => _screenSpaceShadowOption;

	public EGraphicOption PlaneReflectionOption => _planeReflectionOption;

	public bool ScreenSpaceAmbientOcclusionActive => _screenSpaceAmbientOcclusionActive;

	public bool DynamicVinetteActive => _dynamicVinetteActive;

	public EGraphicOption ScreenBlurOption => _screenBlurOption;

	public EGraphicOption UIScreenBlurOption => _uiScreenBlurOption;

	public bool CampaignTiltActive => _campaignTiltActive;

	public EGraphicOption CustomDepthOption => _customDepthOption;

	public bool PostProcessingActive => _postProcessingActive;

	public bool BlobShadowEnable => _blobShadowEnable;

	public bool CasterDirectionalOnly => _casterDirectionalOnly;

	public bool GrabSpineEnable => _grabSpineEnable;

	public bool GrabGlassEnable => _grabGlassEnable;

	public bool GrabEffectEnable => _grabEffectEnable;

	public CustomCameraUtils.ECustomCameraResolutionType GrabSpineRes => _grabSpineRes;

	public CustomCameraUtils.ECustomCameraResolutionType GrabGlassRes => _grabGlassRes;

	public CustomCameraUtils.ECustomCameraResolutionType GrabEffectRes => _grabEffectRes;

	public int RtMaxHeight => _rtMaxHeight;

	public int ParticleRaycastBudget => _particleRaycastBudget;
}

using UnityEngine;

[CreateAssetMenu(menuName = "Project DE/Graphic Quality Preset")]
public class GraphicQualityPreset : ScriptableObject
{
	[Header("Render Scale")]
	[SerializeField]
	[Range(0.5f, 1f)]
	private float _renderScale;

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
	private bool _grabSpineEnable;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabSpineRes;

	[Tooltip("Glass 레이어 grab 활성 + 해상도")]
	[SerializeField]
	private bool _grabGlassEnable;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabGlassRes;

	[Tooltip("Effect 레이어 grab 활성 + 해상도")]
	[SerializeField]
	private bool _grabEffectEnable;

	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _grabEffectRes;

	[Header("중간버퍼 RT 높이 상한")]
	[SerializeField]
	private int _rtMaxHeight;

	[Header("Quality Settings")]
	[SerializeField]
	private int _particleRaycastBudget;

	public float RenderScale => 0f;

	public bool BloomActive => false;

	public EGraphicOption ScreenSpaceShadowOption => EGraphicOption.Off;

	public EGraphicOption PlaneReflectionOption => EGraphicOption.Off;

	public bool ScreenSpaceAmbientOcclusionActive => false;

	public bool DynamicVinetteActive => false;

	public EGraphicOption ScreenBlurOption => EGraphicOption.Off;

	public EGraphicOption UIScreenBlurOption => EGraphicOption.Off;

	public bool CampaignTiltActive => false;

	public EGraphicOption CustomDepthOption => EGraphicOption.Off;

	public bool PostProcessingActive => false;

	public bool BlobShadowEnable => false;

	public bool CasterDirectionalOnly => false;

	public bool GrabSpineEnable => false;

	public bool GrabGlassEnable => false;

	public bool GrabEffectEnable => false;

	public CustomCameraUtils.ECustomCameraResolutionType GrabSpineRes => (CustomCameraUtils.ECustomCameraResolutionType)0;

	public CustomCameraUtils.ECustomCameraResolutionType GrabGlassRes => (CustomCameraUtils.ECustomCameraResolutionType)0;

	public CustomCameraUtils.ECustomCameraResolutionType GrabEffectRes => (CustomCameraUtils.ECustomCameraResolutionType)0;

	public int RtMaxHeight => 0;

	public int ParticleRaycastBudget => 0;
}

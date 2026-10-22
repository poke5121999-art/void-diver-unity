using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GraphicOptionModel : MonoBehaviour
{
	[SerializeField]
	private Volume _mainVolume;

	[Header("Post Processing Data")]
	[SerializeField]
	private PostProcessData _postProcessData;

	[SerializeField]
	private UniversalRenderPipelineAsset _mainRenderingData;

	[Header("Main Render Pipeline Passes")]
	[SerializeField]
	private UniversalRendererData _mainRenderingFeature;

	[SerializeField]
	private RenderObjects _reflectionBackgroundFeature;

	[SerializeField]
	private RenderObjects _reflectionObjectFeature;

	[SerializeField]
	private ScriptableRendererFeature _screenSpaceShadowRendererFeature;

	[SerializeField]
	[FormerlySerializedAs("_brightnessVignetteResultFeature")]
	private CustomScreenFeature _dynamicVignetteResultFeature;

	[SerializeField]
	private ScreenBlurUnifiedRendererFeature _unifiedScreenBlurFeature;

	[SerializeField]
	private ScreenBlurFeature _uiScreenBlurFeature;

	[SerializeField]
	private CustomScreenSpaceAmbientOcclusion _customScreenSpaceAmbientOcclusion;

	[SerializeField]
	private CustomScreenBufferRendererFeature _customDepthBufferRendererFeature;

	[Header("Cutscene Render Pipeline Passes")]
	[SerializeField]
	private UniversalRendererData _cutsceneRenderingFeature;

	[SerializeField]
	private RenderObjects _cutsceneReflectionBackgroundFeature;

	[SerializeField]
	private RenderObjects _cutsceneReflectionObjectFeature;

	[SerializeField]
	private ScriptableRendererFeature _cutsceneScreenSpaceShadowRendererFeature;

	[SerializeField]
	private ScreenBlurUnifiedRendererFeature _cutsceneUnifiedScreenBlurFeature;

	[SerializeField]
	private CustomScreenSpaceAmbientOcclusion _cutsceneCustomScreenSpaceAmbientOcclusion;

	[SerializeField]
	private CustomScreenBufferRendererFeature _cutsceneCustomDepthBufferRendererFeature;

	[Header("Campaign Render Pipeline Passes")]
	[SerializeField]
	private UniversalRendererData _campaignRenderingFeature;

	[SerializeField]
	private CustomScreenFeature _campaignTiltFeature;

	public Volume MainVolume => _mainVolume;

	public PostProcessData PostProcessData => _postProcessData;

	public UniversalRenderPipelineAsset MainRenderingData => _mainRenderingData;

	public UniversalRendererData MainRenderingFeature => _mainRenderingFeature;

	public RenderObjects ReflectionBackgroundFeature => _reflectionBackgroundFeature;

	public RenderObjects ReflectionObjectFeature => _reflectionObjectFeature;

	public ScriptableRendererFeature ScreenSpaceShadowRendererFeature => _screenSpaceShadowRendererFeature;

	public CustomScreenFeature DynamicVignetteResultFeature => _dynamicVignetteResultFeature;

	public ScreenBlurUnifiedRendererFeature UnifiedScreenBlurFeature => _unifiedScreenBlurFeature;

	public ScreenBlurFeature UIScreenBlurFeature => _uiScreenBlurFeature;

	public CustomScreenSpaceAmbientOcclusion CustomScreenSpaceAmbientOcclusion => _customScreenSpaceAmbientOcclusion;

	public CustomScreenBufferRendererFeature CustomDepthBufferRendererFeature => _customDepthBufferRendererFeature;

	public UniversalRendererData CutsceneRenderingFeature => _cutsceneRenderingFeature;

	public RenderObjects CutsceneReflectionBackgroundFeature => _cutsceneReflectionBackgroundFeature;

	public RenderObjects CutsceneReflectionObjectFeature => _cutsceneReflectionObjectFeature;

	public ScriptableRendererFeature CutsceneScreenSpaceShadowRendererFeature => _cutsceneScreenSpaceShadowRendererFeature;

	public ScreenBlurUnifiedRendererFeature CutsceneUnifiedScreenBlurFeature => _cutsceneUnifiedScreenBlurFeature;

	public CustomScreenSpaceAmbientOcclusion CutsceneCustomScreenSpaceAmbientOcclusion => _cutsceneCustomScreenSpaceAmbientOcclusion;

	public CustomScreenBufferRendererFeature CutsceneCustomDepthBufferRendererFeature => _cutsceneCustomDepthBufferRendererFeature;

	public UniversalRendererData CampaignRenderingFeature => _campaignRenderingFeature;

	public CustomScreenFeature CampaignTiltFeature => _campaignTiltFeature;
}

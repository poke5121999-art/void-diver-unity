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

	public Volume MainVolume => null;

	public PostProcessData PostProcessData => null;

	public UniversalRenderPipelineAsset MainRenderingData => null;

	public UniversalRendererData MainRenderingFeature => null;

	public RenderObjects ReflectionBackgroundFeature => null;

	public RenderObjects ReflectionObjectFeature => null;

	public ScriptableRendererFeature ScreenSpaceShadowRendererFeature => null;

	public CustomScreenFeature DynamicVignetteResultFeature => null;

	public ScreenBlurUnifiedRendererFeature UnifiedScreenBlurFeature => null;

	public ScreenBlurFeature UIScreenBlurFeature => null;

	public CustomScreenSpaceAmbientOcclusion CustomScreenSpaceAmbientOcclusion => null;

	public CustomScreenBufferRendererFeature CustomDepthBufferRendererFeature => null;

	public UniversalRendererData CutsceneRenderingFeature => null;

	public RenderObjects CutsceneReflectionBackgroundFeature => null;

	public RenderObjects CutsceneReflectionObjectFeature => null;

	public ScriptableRendererFeature CutsceneScreenSpaceShadowRendererFeature => null;

	public ScreenBlurUnifiedRendererFeature CutsceneUnifiedScreenBlurFeature => null;

	public CustomScreenSpaceAmbientOcclusion CutsceneCustomScreenSpaceAmbientOcclusion => null;

	public CustomScreenBufferRendererFeature CutsceneCustomDepthBufferRendererFeature => null;

	public UniversalRendererData CampaignRenderingFeature => null;

	public CustomScreenFeature CampaignTiltFeature => null;
}

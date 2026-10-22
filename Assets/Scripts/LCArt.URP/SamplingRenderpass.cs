using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

internal class SamplingRenderpass : ScriptableRenderPass
{
	private class PassData
	{
		public TextureHandle SourceTexture;

		public TextureHandle DestinationTexture;
	}

	private RTHandle _samplingTextureHandle;

	private FilterMode _samplingFilterMode;

	private ScriptableRenderer _scriptableRenderer;

	private string _samplingTextureName;

	private int _samplingTexturePropertyID;

	private float _downsamplingScale;

	private const string _profilerID = "SamplingRenderpass";

	public bool SkipCleanup { get; set; }

	public SamplingRenderpass(in RenderPassEvent renderPassEvent, in string renderPassName, in float downsamplingScale, in FilterMode filterMode = FilterMode.Bilinear)
	{
		_samplingFilterMode = FilterMode.Bilinear;
		_downsamplingScale = 1f;
		base.renderPassEvent = renderPassEvent;
		_samplingTextureName = renderPassName;
		_samplingTextureHandle = RTHandles.Alloc(new RenderTargetIdentifier(_samplingTextureName), _samplingTextureName);
		_samplingTexturePropertyID = Shader.PropertyToID(renderPassName);
		_samplingFilterMode = filterMode;
		_downsamplingScale = downsamplingScale;
	}

	public void SetDownsamplingScale(in float downsamplingScale)
	{
		_downsamplingScale = downsamplingScale;
	}

	public void UpdatePassSetting(in RenderPassEvent renderPassEvent, in string renderPassName, in float downsamplingScale, in FilterMode filterMode = FilterMode.Bilinear)
	{
		base.renderPassEvent = renderPassEvent;
		_samplingTextureName = renderPassName;
		_samplingTextureHandle = RTHandles.Alloc(new RenderTargetIdentifier(_samplingTextureName), _samplingTextureName);
		_samplingTexturePropertyID = Shader.PropertyToID(renderPassName);
		_samplingFilterMode = filterMode;
		_downsamplingScale = downsamplingScale;
	}

	public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
	{
		UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
		frameData.Get<UniversalRenderingData>();
		UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
		if (universalCameraData.isPreviewCamera || universalCameraData.isSceneViewCamera || !universalResourceData.activeColorTexture.IsValid())
		{
			return;
		}
		float renderScale = UniversalRenderPipeline.asset.renderScale;
		// TODO(decomp): source of the two ints read at UniversalCameraData+0x100/+0x104 (assumed cameraTargetDescriptor.width/height) @0x0000000182925830
		RenderTextureDescriptor targetDescriptor = universalCameraData.cameraTargetDescriptor;
		CustomCameraUtils.GetCappedRTSize((int)((float)targetDescriptor.width * renderScale), (int)((float)targetDescriptor.height * renderScale), out var width, out var height);
		TextureDesc desc = new TextureDesc((int)((float)width * _downsamplingScale), (int)((float)height * _downsamplingScale));
		// TODO(decomp): exact TextureDesc fields set (format source, clearBuffer and the second flag set to 1) @0x0000000182925830
		desc.format = targetDescriptor.graphicsFormat;
		desc.name = _samplingTextureName;
		desc.filterMode = _samplingFilterMode;
		desc.clearBuffer = true;
		using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("SamplingRenderpass", out var passData);
		passData.SourceTexture = universalResourceData.activeColorTexture;
		passData.DestinationTexture = renderGraph.CreateTexture(in desc);
		builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
		builder.SetRenderAttachment(passData.DestinationTexture, 0, AccessFlags.Write);
		builder.AllowPassCulling(false);
		builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
		{
			Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), 0f, bilinear: false);
		});
		builder.SetGlobalTextureAfterPass(in passData.DestinationTexture, _samplingTexturePropertyID);
	}

	public void SetBeforeEnqueue(ScriptableRenderer renderer)
	{
		_scriptableRenderer = renderer;
	}

	public void Dispose()
	{
		if (_samplingTextureHandle != null)
		{
			_samplingTextureHandle.Release();
		}
	}
}

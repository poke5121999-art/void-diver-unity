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
	}

	public void SetDownsamplingScale(in float downsamplingScale)
	{
	}

	public void UpdatePassSetting(in RenderPassEvent renderPassEvent, in string renderPassName, in float downsamplingScale, in FilterMode filterMode = FilterMode.Bilinear)
	{
	}

	public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
	{
	}

	public void SetBeforeEnqueue(ScriptableRenderer renderer)
	{
	}

	public void Dispose()
	{
	}
}

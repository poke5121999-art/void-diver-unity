using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenDepthBlurRenderFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenDepthBlurSettings
	{
		public RenderPassEvent RenderPassEvent;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations;
	}

	public class ScreenDepthBlurRenderPass : ScriptableRenderPass
	{
		private class DepthBlurPassData
		{
			public TextureHandle SourceTexture;

			public TextureHandle DestTexture;

			public Material Material;

			public int PassIndex;
		}

		private ScreenDepthBlurSettings _settings;

		private RenderTextureDescriptor _blurTextureDescriptor;

		private RTHandle _blurTextureHandle;

		private RTHandle _cameraColor;

		private Material _material;

		public ScreenDepthBlurRenderPass(ScreenDepthBlurSettings defaultSettings)
		{
		}

		public void SetMaterial(ref Material material)
		{
		}

		public void UpdatePassSetting(ScreenDepthBlurSettings setting)
		{
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}
	}

	[SerializeField]
	private ScreenDepthBlurSettings _settings;

	private ScreenDepthBlurRenderPass _blurRenderPass;

	private Material _material;

	public override void Create()
	{
	}

	private void OnValidate()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}
}

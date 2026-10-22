using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class CustomScreenBufferRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public class CustomScreenBufferPassSetting
	{
		[Header("Pass Settings")]
		public RenderPassEvent RenderPassEvent;

		public CustomCameraUtils.ECustomCameraResolutionType ScreenResolutionType;

		[Header("Rendering Settings")]
		public LayerMask CullingLayerMask;

		public RenderTextureFormat RenderTextureFormat;

		public string ShaderTagID;

		[Header("Buffer Settings")]
		public string BufferName;

		public Color BufferResetColor;

		public FilterMode BufferFilterMode;

		public SortingCriteria SortingCriteria;

		public string BufferDepthName => null;
	}

	private class CustomScreenBufferPass : ScriptableRenderPass
	{
		private class PassData
		{
			public RendererListHandle RendererListHandle;

			public TextureHandle ColorTexture;

			public TextureHandle DepthTexture;

			public string BufferName;
		}

		private CustomScreenBufferPassSetting _passSetting;

		private ProfilingSampler _profilingSampler;

		private RTHandle _customColor;

		private RTHandle _customDepth;

		private ShaderTagId _shaderTagID;

		private const string _profileTag = "CustomScreenBufferPass";

		public CustomScreenBufferPass(CustomScreenBufferPassSetting passSetting)
		{
		}

		public void UpdatePassSetting(CustomScreenBufferPassSetting passSetting)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}

		public void Dispose()
		{
		}
	}

	private CustomScreenBufferPass _scriptableBufferPass;

	[SerializeField]
	private CustomScreenBufferPassSetting _passSetting;

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

	public void SetResolution(CustomCameraUtils.ECustomCameraResolutionType resolutionType)
	{
	}
}

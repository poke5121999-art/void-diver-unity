using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenDepthBlurRenderFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenDepthBlurSettings
	{
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)600;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations = 1;
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
			_settings = defaultSettings;
			renderPassEvent = _settings.RenderPassEvent;
		}

		public void SetMaterial(ref Material material)
		{
			_material = material;
		}

		public void UpdatePassSetting(ScreenDepthBlurSettings setting)
		{
			_settings = setting;
			renderPassEvent = _settings.RenderPassEvent;
		}

		public void Dispose()
		{
			_blurTextureHandle?.Release();
			_cameraColor?.Release();
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_material == null)
			{
				return;
			}
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
			if (!resourceData.activeColorTexture.IsValid())
			{
				return;
			}
			int width = Mathf.Max(1, cameraData.cameraTargetDescriptor.width >> _settings.Downsample);
			int height = Mathf.Max(1, cameraData.cameraTargetDescriptor.height >> _settings.Downsample);
			bool supported = SystemInfo.IsFormatSupported((GraphicsFormat)48, GraphicsFormatUsage.Render);
			// TODO(decomp): exact TextureDesc field assignments after the ctor were read from raw offsets @0x182918f40
			TextureDesc desc = new TextureDesc(width, height, false, false);
			desc.format = (GraphicsFormat)(supported ? 48 : 8);
			desc.name = "_DepthBlurTexture";
			desc.filterMode = FilterMode.Bilinear;
			desc.wrapMode = TextureWrapMode.Clamp;
			desc.anisoLevel = 1;
			desc.clearBuffer = false;
			TextureHandle blurTexture = renderGraph.CreateTexture(desc);
			int iterations = Mathf.Max(1, _settings.Iterations);
			for (int i = 0; i < iterations; i++)
			{
				using (var builder = renderGraph.AddRasterRenderPass<DepthBlurPassData>(string.Format("ScreenDepthBlur_V_{0}", i), out var passData))
				{
					passData.SourceTexture = resourceData.activeColorTexture;
					passData.DestTexture = blurTexture;
					passData.Material = _material;
					passData.PassIndex = 0;
					builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(passData.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc((DepthBlurPassData data, RasterGraphContext context) =>
					{
						Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, data.PassIndex);
					});
				}
				using (var builder = renderGraph.AddRasterRenderPass<DepthBlurPassData>(string.Format("ScreenDepthBlur_H_{0}", i), out var passData))
				{
					passData.SourceTexture = blurTexture;
					passData.DestTexture = resourceData.activeColorTexture;
					passData.Material = _material;
					passData.PassIndex = 1;
					builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(passData.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc((DepthBlurPassData data, RasterGraphContext context) =>
					{
						Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, data.PassIndex);
					});
				}
			}
		}
	}

	[SerializeField]
	private ScreenDepthBlurSettings _settings = new ScreenDepthBlurSettings();

	private ScreenDepthBlurRenderPass _blurRenderPass;

	private Material _material;

	public override void Create()
	{
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenDepthBlurRenderPass(_settings);
		}
	}

	private void OnValidate()
	{
		if (_blurRenderPass != null)
		{
			_blurRenderPass.UpdatePassSetting(_settings);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (_material == null && Shader.Find(ScreenBlurConsts.SHADER_NAME_DEPTH_BLUR) != null)
		{
			_material = CoreUtils.CreateEngineMaterial(ScreenBlurConsts.SHADER_NAME_DEPTH_BLUR);
		}
		if (_blurRenderPass != null)
		{
			_blurRenderPass.SetMaterial(ref _material);
		}
		if (_blurRenderPass != null)
		{
			_blurRenderPass.UpdatePassSetting(_settings);
		}
		if (renderingData.cameraData.cameraType == CameraType.Game)
		{
			renderer.EnqueuePass(_blurRenderPass);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_blurRenderPass != null)
		{
			_blurRenderPass.Dispose();
		}
		_blurRenderPass = null;
	}
}

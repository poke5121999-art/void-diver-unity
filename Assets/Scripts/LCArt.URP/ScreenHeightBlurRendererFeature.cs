using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenHeightBlurRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenHeightBlurSetting
	{
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)600;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations = 1;
	}

	public class ScreenHeightBlurRenderPass : ScriptableRenderPass
	{
		private class HeightBlurPassData
		{
			public TextureHandle SourceTexture;

			public TextureHandle DestTexture;

			public Material Material;

			public int PassIndex;
		}

		private ScreenHeightBlurSetting _settings;

		private RenderTextureDescriptor _blurTextureDescriptor;

		private RTHandle _blurTextureHandle;

		private RTHandle _bokehSourceHandle;

		private RTHandle _cameraColor;

		private Material _material;

		private int _autoIterations = 1;

		private float _autoBlurScale = 1f;

		public void SetAutoBlur(int iterations, float blurScale)
		{
			_autoIterations = iterations;
			_autoBlurScale = blurScale;
		}

		public ScreenHeightBlurRenderPass(ScreenHeightBlurSetting defaultSettings)
		{
			_settings = defaultSettings;
			renderPassEvent = _settings.RenderPassEvent;
		}

		public void SetMaterial(ref Material material)
		{
			_material = material;
		}

		public void UpdatePassSetting(ScreenHeightBlurSetting setting)
		{
			_settings = setting;
			renderPassEvent = _settings.RenderPassEvent;
		}

		public void Dispose()
		{
			_blurTextureHandle?.Release();
			_bokehSourceHandle?.Release();
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
			// TextureDesc stores checked against the raw offsets @0x182919b60 (field = offset - 0x10): format, clearBuffer (0x76) = false, name (0x60), filterMode (0x34) = 1, wrapMode (0x38) = 1, msaaSamples (0x4c) = 1; anisoLevel is not written
			TextureDesc desc = new TextureDesc(width, height, false, false);
			desc.format = (GraphicsFormat)(supported ? 48 : 8);
			desc.clearBuffer = false;
			desc.name = "_HeightBlurTexture";
			desc.filterMode = FilterMode.Bilinear;
			desc.wrapMode = TextureWrapMode.Clamp;
			desc.msaaSamples = MSAASamples.None;
			TextureHandle blurTexture = renderGraph.CreateTexture(desc);
			TextureHandle copySource = renderGraph.CreateTexture(desc);
			using (var builder = renderGraph.AddRasterRenderPass<HeightBlurPassData>("ScreenHeightBlur_CopySource", out var passData))
			{
				passData.SourceTexture = resourceData.activeColorTexture;
				passData.DestTexture = copySource;
				builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(passData.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((HeightBlurPassData data, RasterGraphContext context) =>
				{
					Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), 0f, false);
				});
			}
			Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_SCALE, _autoBlurScale);
			int iterations = Mathf.Max(1, _autoIterations);
			for (int i = 0; i < iterations; i++)
			{
				using (var builder = renderGraph.AddRasterRenderPass<HeightBlurPassData>(string.Format("ScreenHeightBlur_V_{0}", i), out var passData))
				{
					passData.SourceTexture = resourceData.activeColorTexture;
					passData.DestTexture = blurTexture;
					passData.Material = _material;
					passData.PassIndex = 0;
					builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(passData.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc((HeightBlurPassData data, RasterGraphContext context) =>
					{
						Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, data.PassIndex);
					});
				}
				using (var builder = renderGraph.AddRasterRenderPass<HeightBlurPassData>(string.Format("ScreenHeightBlur_H_{0}", i), out var passData))
				{
					passData.SourceTexture = blurTexture;
					passData.DestTexture = resourceData.activeColorTexture;
					passData.Material = _material;
					passData.PassIndex = 1;
					builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(passData.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc((HeightBlurPassData data, RasterGraphContext context) =>
					{
						Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, data.PassIndex);
					});
				}
			}
		}
	}

	[SerializeField]
	private ScreenHeightBlurSetting _settings = new ScreenHeightBlurSetting();

	private ScreenHeightBlurRenderPass _blurRenderPass;

	private Material _material;

	private static void CalcAutoBlur(int degree, out int iterations, out float blurScale)
	{
		if (degree < 3)
		{
			iterations = 1;
			blurScale = 1f;
			return;
		}
		float n;
		if (degree < 5)
		{
			n = 2f;
			iterations = 2;
		}
		else
		{
			n = 3f;
			iterations = 3;
		}
		blurScale = 1f / Mathf.Sqrt(n);
	}

	public override void Create()
	{
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenHeightBlurRenderPass(_settings);
		}
		Shader.SetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_SCALE, 1f);
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
		if (renderingData.cameraData.cameraType != CameraType.Game)
		{
			return;
		}
		if (_material == null && Shader.Find(ScreenBlurConsts.SHADER_NAME_SCREEN_HEIGHT_BLUR_SHIFT) != null)
		{
			_material = CoreUtils.CreateEngineMaterial(ScreenBlurConsts.SHADER_NAME_SCREEN_HEIGHT_BLUR_SHIFT);
		}
		if (_blurRenderPass != null)
		{
			_blurRenderPass.SetMaterial(ref _material);
		}
		if (_blurRenderPass != null)
		{
			_blurRenderPass.UpdatePassSetting(_settings);
		}
		CalcAutoBlur(Shader.GetGlobalInt(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_HEIGHT_BLUR_DEGREE), out int iterations, out float blurScale);
		if (_blurRenderPass != null)
		{
			_blurRenderPass.SetAutoBlur(iterations, blurScale);
		}
		renderer.EnqueuePass(_blurRenderPass);
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

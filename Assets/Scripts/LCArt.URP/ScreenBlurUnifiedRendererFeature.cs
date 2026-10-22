using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenBlurUnifiedRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenBlurUnifiedSettings
	{
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)600;

		[Tooltip("다운샘플 레벨 (1 권장: 바이리니어 필터링으로 격자 아티팩트 감소)")]
		[Range(0f, 3f)]
		public int Downsample = 1;

		[Tooltip("Kawase 블러 반복 횟수 (높을수록 넓은 블러)")]
		[Range(1f, 8f)]
		public int Iterations = 3;
	}

	public class ScreenBlurUnifiedRenderPass : ScriptableRenderPass
	{
		private class BlurPassData
		{
			public TextureHandle SourceTexture;

			public TextureHandle DestTexture;

			public Material Material;

			public float KawaseOffset;
		}

		private class DepthCopyPassData
		{
			public TextureHandle SourceDepth;

			public TextureHandle DestTexture;
		}

		private class CompositePassData
		{
			public TextureHandle BlurTexture;

			public TextureHandle OriginalTexture;

			public TextureHandle DestTexture;

			public Material Material;
		}

		private ScreenBlurUnifiedSettings _settings;

		private Material _material;

		private static readonly int UnifiedBlurDepthTextureId = Shader.PropertyToID("_UnifiedBlurDepthTexture");

		private static readonly int KawaseOffsetId = Shader.PropertyToID("_KawaseOffset");

		private static readonly int OriginalCameraColorId = Shader.PropertyToID("_OriginalCameraColor");

		public ScreenBlurUnifiedRenderPass(ScreenBlurUnifiedSettings defaultSettings)
		{
			_settings = defaultSettings;
			base.renderPassEvent = _settings.RenderPassEvent;
		}

		public void UpdatePassSetting(ScreenBlurUnifiedSettings setting)
		{
			_settings = setting;
			base.renderPassEvent = _settings.RenderPassEvent;
		}

		public void SetMaterial(Material material)
		{
			_material = material;
		}

		public void SetRenderPassEvent(RenderPassEvent ev)
		{
			if (_settings != null)
			{
				_settings.RenderPassEvent = ev;
				base.renderPassEvent = ev;
			}
		}

		public void SetDownsample(int downsample)
		{
			if (_settings != null)
			{
				_settings.Downsample = Mathf.Clamp(downsample, 0, 3);
			}
		}

		public void SetIterations(int iterations)
		{
			if (_settings != null)
			{
				_settings.Iterations = Mathf.Clamp(iterations, 1, 8);
			}
		}

		public void Dispose()
		{
		}

		private static void BlitSource(DepthCopyPassData data, RasterGraphContext context)
		{
			Blitter.BlitTexture(context.cmd, data.SourceDepth, new Vector4(1f, 1f, 0f, 0f), 0f, false);
		}

		private static void BlitKawase(BlurPassData data, RasterGraphContext context)
		{
			data.Material.SetFloat(KawaseOffsetId, data.KawaseOffset);
			Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, 0);
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
			int camWidth = cameraData.cameraTargetDescriptor.width;
			int camHeight = cameraData.cameraTargetDescriptor.height;
			int blurWidth = Mathf.Max(camWidth >> _settings.Downsample, 1);
			int blurHeight = Mathf.Max(camHeight >> _settings.Downsample, 1);

			// Full-resolution R32 copy of the scene depth (exposed as a global after the pass).
			TextureDesc depthDesc = new TextureDesc(camWidth, camHeight, false, false);
			depthDesc.format = GraphicsFormat.R32_SFloat;
			depthDesc.filterMode = FilterMode.Point;
			depthDesc.wrapMode = TextureWrapMode.Clamp;
			depthDesc.msaaSamples = MSAASamples.None;
			depthDesc.name = "_UnifiedBlurDepthTexture";
			TextureHandle depthTexture = renderGraph.CreateTexture(depthDesc);
			if (resourceData.activeDepthTexture.IsValid())
			{
				using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<DepthCopyPassData>("UnifiedBlur_CopyDepth", out DepthCopyPassData data))
				{
					data.SourceDepth = resourceData.activeDepthTexture;
					data.DestTexture = depthTexture;
					builder.UseTexture(data.SourceDepth, AccessFlags.Read);
					builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
					builder.SetGlobalTextureAfterPass(data.DestTexture, UnifiedBlurDepthTextureId);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc<DepthCopyPassData>(BlitSource);
				}
			}

			// Copy of the camera colour, sampled by the composite pass.
			TextureDesc originalDesc = new TextureDesc(camWidth, camHeight, false, false);
			originalDesc.format = GraphicsFormat.R16G16B16A16_SFloat;
			originalDesc.filterMode = FilterMode.Bilinear;
			originalDesc.wrapMode = TextureWrapMode.Clamp;
			originalDesc.msaaSamples = MSAASamples.None;
			originalDesc.name = "_OriginalCameraColor";
			TextureHandle originalTexture = renderGraph.CreateTexture(originalDesc);
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<DepthCopyPassData>("UnifiedBlur_CopyColor", out DepthCopyPassData data))
			{
				data.SourceDepth = resourceData.activeColorTexture;
				data.DestTexture = originalTexture;
				builder.UseTexture(data.SourceDepth, AccessFlags.Read);
				builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<DepthCopyPassData>(BlitSource);
			}

			TextureDesc blurDesc = new TextureDesc(blurWidth, blurHeight, false, false);
			blurDesc.format = GraphicsFormat.R16G16B16A16_SFloat;
			blurDesc.filterMode = FilterMode.Bilinear;
			blurDesc.wrapMode = TextureWrapMode.Clamp;
			blurDesc.msaaSamples = MSAASamples.None;
			blurDesc.name = "_UnifiedBlurTextureA";
			TextureHandle blurA = renderGraph.CreateTexture(blurDesc);
			blurDesc.name = "_UnifiedBlurTextureB";
			TextureHandle blurB = renderGraph.CreateTexture(blurDesc);
			int iterations = Mathf.Max(_settings.Iterations, 1);
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>("KawaseBlur_0", out BlurPassData data))
			{
				data.SourceTexture = resourceData.activeColorTexture;
				data.DestTexture = blurA;
				data.Material = _material;
				data.KawaseOffset = 0f;
				builder.UseTexture(data.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<BlurPassData>(BlitKawase);
			}
			for (int i = 1; i < iterations; i++)
			{
				TextureHandle src = blurB;
				TextureHandle dst = blurA;
				if (i % 2 != 0)
				{
					src = blurA;
					dst = blurB;
				}
				using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>(string.Format("KawaseBlur_{0}", i), out BlurPassData data))
				{
					data.SourceTexture = src;
					data.DestTexture = dst;
					data.Material = _material;
					data.KawaseOffset = i;
					builder.UseTexture(data.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc<BlurPassData>(BlitKawase);
				}
			}
			TextureHandle finalBlur = ((iterations & 1) == 0) ? blurB : blurA;
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<CompositePassData>("UnifiedBlur_Composite", out CompositePassData data))
			{
				data.BlurTexture = finalBlur;
				data.OriginalTexture = originalTexture;
				data.DestTexture = resourceData.activeColorTexture;
				data.Material = _material;
				builder.UseTexture(data.BlurTexture, AccessFlags.Read);
				builder.UseTexture(data.OriginalTexture, AccessFlags.Read);
				builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<CompositePassData>(static (CompositePassData d, RasterGraphContext context) =>
				{
					d.Material.SetTexture(OriginalCameraColorId, d.OriginalTexture);
					Blitter.BlitTexture(context.cmd, d.BlurTexture, new Vector4(1f, 1f, 0f, 0f), d.Material, 1);
				});
			}
		}
	}

	[SerializeField]
	private ScreenBlurUnifiedSettings _settings = new ScreenBlurUnifiedSettings();

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty = "";

	private ScreenBlurUnifiedRenderPass _blurRenderPass;

	private Material _material;

	public override void Create()
	{
		if (_settings == null)
		{
			_settings = new ScreenBlurUnifiedSettings();
		}
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenBlurUnifiedRenderPass(_settings);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.cameraType != CameraType.Game)
		{
			return;
		}
		if (!string.IsNullOrEmpty(_graphicGateProperty) && Shader.GetGlobalFloat(_graphicGateProperty) >= 0.5f)
		{
			return;
		}
		if (_settings == null)
		{
			_settings = new ScreenBlurUnifiedSettings();
		}
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenBlurUnifiedRenderPass(_settings);
		}
		EnsureMaterial();
		_blurRenderPass.SetMaterial(_material);
		_blurRenderPass.UpdatePassSetting(_settings);
		renderer.EnqueuePass(_blurRenderPass);
	}

	protected override void Dispose(bool disposing)
	{
		if (_blurRenderPass != null)
		{
			_blurRenderPass.Dispose();
		}
		if (_material != null)
		{
			CoreUtils.Destroy(_material);
		}
		_material = null;
	}

	private void EnsureMaterial()
	{
		if (!(_material != null))
		{
			Shader shader = Shader.Find(ScreenBlurConsts.SHADER_NAME_SCREEN_BLUR_UNIFIED);
			if (!(shader == null))
			{
				_material = CoreUtils.CreateEngineMaterial(shader);
				if (_blurRenderPass != null)
				{
					_blurRenderPass.SetMaterial(_material);
				}
			}
		}
	}

	private void OnValidate()
	{
		if (_blurRenderPass != null)
		{
			_blurRenderPass.UpdatePassSetting(_settings);
		}
	}

	public void SetMaterial(Material material)
	{
		_material = material;
	}

	public void SetRenderPassEvent(RenderPassEvent ev)
	{
		if (_settings != null)
		{
			_settings.RenderPassEvent = ev;
			if (_blurRenderPass != null)
			{
				_blurRenderPass.SetRenderPassEvent(ev);
			}
		}
	}

	public void SetDownsample(int downsample)
	{
		if (_settings != null)
		{
			_settings.Downsample = Mathf.Clamp(downsample, 0, 3);
			if (_blurRenderPass != null)
			{
				_blurRenderPass.SetDownsample(_settings.Downsample);
			}
		}
	}

	public void SetIterations(int iterations)
	{
		if (_settings != null)
		{
			_settings.Iterations = Mathf.Clamp(iterations, 1, 8);
			if (_blurRenderPass != null)
			{
				_blurRenderPass.SetIterations(_settings.Iterations);
			}
		}
	}

	public void UpdateSettings(ScreenBlurUnifiedSettings settings)
	{
		if (settings != null)
		{
			_settings = settings;
			if (_blurRenderPass != null)
			{
				_blurRenderPass.UpdatePassSetting(_settings);
			}
		}
	}
}

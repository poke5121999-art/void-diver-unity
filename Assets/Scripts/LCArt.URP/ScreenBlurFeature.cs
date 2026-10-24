using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenBlurFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenBlurSettings
	{
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)600;

		[Range(0f, 5f)]
		public int ScreenBlurDegree = 1;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations = 1;

		public Material ScreenBlurCustomMaterial;
	}

	public class ScreenBlurRenderPass : ScriptableRenderPass
	{
		private class BlurPassData
		{
			public TextureHandle SourceTexture;

			public TextureHandle DestTexture;

			public Material Material;

			public int PassIndex;
		}

		private ScreenBlurSettings _screenBlurSetting;

		private Material _material;

		private Material _customMaterial;

		private RenderTextureDescriptor _tempTextureDescriptor;

		private RTHandle _blurTextureHandle;

		private RTHandle _customTextureHandle;

		private RTHandle _cameraColor;

		public ScreenBlurRenderPass(ScreenBlurSettings defaultSettings)
		{
			_screenBlurSetting = defaultSettings;
			base.renderPassEvent = _screenBlurSetting.RenderPassEvent;
		}

		public void UpdatePassSetting(ScreenBlurSettings setting)
		{
			_screenBlurSetting = setting;
			base.renderPassEvent = _screenBlurSetting.RenderPassEvent;
		}

		public void SetBlurMaterial(ref Material blurMaterial)
		{
			_material = blurMaterial;
		}

		public void SetCustomMaterial(ref Material customMaterial)
		{
			_customMaterial = customMaterial;
		}

		public void Dispose()
		{
			_blurTextureHandle?.Release();
			_customTextureHandle?.Release();
			_cameraColor?.Release();
		}

		private static void BlitWithMaterial(BlurPassData data, RasterGraphContext context)
		{
			Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, data.PassIndex);
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
			int width;
			int height;
			CustomCameraUtils.GetCappedRTSize(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height, out width, out height);
			int downsample = _screenBlurSetting.Downsample;
			int tempWidth = Mathf.Max(width >> downsample, 1);
			int tempHeight = Mathf.Max(height >> _screenBlurSetting.Downsample, 1);
			TextureDesc desc = new TextureDesc(tempWidth, tempHeight, false, false);
			desc.format = cameraData.cameraTargetDescriptor.graphicsFormat;
			desc.filterMode = FilterMode.Bilinear;
			desc.wrapMode = TextureWrapMode.Clamp;
			desc.msaaSamples = MSAASamples.None;
			desc.clearBuffer = false;
			desc.name = "_BlurTempTexture";
			TextureHandle temp = renderGraph.CreateTexture(desc);
			int iterations = Mathf.Max(_screenBlurSetting.Iterations, 1);
			for (int i = 0; i < iterations; i++)
			{
				using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>(string.Format("ScreenBlur_V_{0}", i), out BlurPassData data))
				{
					data.SourceTexture = resourceData.activeColorTexture;
					data.DestTexture = temp;
					data.Material = _material;
					data.PassIndex = 0;
					builder.UseTexture(data.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc<BlurPassData>(BlitWithMaterial);
				}
				using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>(string.Format("ScreenBlur_H_{0}", i), out BlurPassData data))
				{
					data.SourceTexture = temp;
					data.DestTexture = resourceData.activeColorTexture;
					data.Material = _material;
					data.PassIndex = 1;
					builder.UseTexture(data.SourceTexture, AccessFlags.Read);
					builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
					builder.AllowPassCulling(false);
					builder.SetRenderFunc<BlurPassData>(BlitWithMaterial);
				}
			}
			if (_customMaterial == null)
			{
				return;
			}
			TextureHandle customTemp = renderGraph.CreateTexture(desc);
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>("ScreenBlur_CustomMaterial", out BlurPassData data))
			{
				data.SourceTexture = resourceData.activeColorTexture;
				data.DestTexture = customTemp;
				data.Material = _customMaterial;
				data.PassIndex = 0;
				builder.UseTexture(data.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<BlurPassData>(BlitWithMaterial);
			}
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<BlurPassData>("ScreenBlur_CopyBack", out BlurPassData data))
			{
				data.SourceTexture = customTemp;
				data.DestTexture = resourceData.activeColorTexture;
				builder.UseTexture(data.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(data.DestTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<BlurPassData>(static (BlurPassData d, RasterGraphContext context) =>
				{
					Blitter.BlitTexture(context.cmd, d.SourceTexture, new Vector4(1f, 1f, 0f, 0f), 0f, false);
				});
			}
		}
	}

	[SerializeField]
	private ScreenBlurSettings _settings = new ScreenBlurSettings();

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty = "";

	private ScreenBlurRenderPass _blurRenderPass;

	private Material _material;

	private Material _customMaterial;

	private Material _customMaterialSource;

	public ScreenBlurSettings Setting => _settings;

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
			_settings.Iterations = Mathf.Clamp(iterations, 1, 4);
		}
	}

	public void SetScreenBlurDegree(int degree)
	{
		if (_settings != null)
		{
			_settings.ScreenBlurDegree = Mathf.Clamp(degree, 0, 5);
		}
	}

	public override void Create()
	{
		if (_settings == null)
		{
			_settings = new ScreenBlurSettings();
		}
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenBlurRenderPass(_settings);
		}
	}

	private void OnValidate()
	{
		// Body is COMDAT-folded with ScreenBlurUnifiedRendererFeature.OnValidate (@0x182906170): settings -> pass, event -> pass.
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
		if (!string.IsNullOrEmpty(_graphicGateProperty) && Shader.GetGlobalFloat(_graphicGateProperty) >= 0.5f)
		{
			return;
		}
		if (Shader.GetGlobalFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_BLUR_PROGRESS) < 0.1f)
		{
			return;
		}
		if (_settings == null)
		{
			_settings = new ScreenBlurSettings();
		}
		if (_blurRenderPass == null)
		{
			_blurRenderPass = new ScreenBlurRenderPass(_settings);
		}
		EnsureMaterials();
		_blurRenderPass.SetBlurMaterial(ref _material);
		_blurRenderPass.SetCustomMaterial(ref _customMaterial);
		_blurRenderPass.UpdatePassSetting(_settings);
		if (_material != null)
		{
			_material.SetFloat(ScreenBlurConsts.SHADER_PROPERTY_SCREEN_BLUR_DEGREE, _settings.ScreenBlurDegree);
		}
		renderer.EnqueuePass(_blurRenderPass);
	}

	private void EnsureMaterials()
	{
		if (_material == null)
		{
			Shader shader = Shader.Find(ScreenBlurConsts.SHADER_NAME_SCREEN_BLUR);
			if (shader != null)
			{
				_material = CoreUtils.CreateEngineMaterial(shader);
			}
		}
		Material source = _settings.ScreenBlurCustomMaterial;
		if (source == null)
		{
			if (_customMaterial != null)
			{
				CoreUtils.Destroy(_customMaterial);
				_customMaterial = null;
				_customMaterialSource = null;
			}
			return;
		}
		if (_customMaterial != null && !(_customMaterialSource != source))
		{
			return;
		}
		if (_customMaterial != null)
		{
			CoreUtils.Destroy(_customMaterial);
		}
		_customMaterial = UnityEngine.Object.Instantiate(source);
		_customMaterialSource = source;
	}

	protected override void Dispose(bool disposing)
	{
		if (_blurRenderPass != null)
		{
			_blurRenderPass.Dispose();
		}
		_blurRenderPass = null;
		if ((bool)_material)
		{
			CoreUtils.Destroy(_material);
		}
		_material = null;
		if ((bool)_customMaterial)
		{
			CoreUtils.Destroy(_customMaterial);
		}
		_customMaterial = null;
		_customMaterialSource = null;
	}
}

using System;
using UnityEngine.Experimental.Rendering;
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
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)500;

		public CustomCameraUtils.ECustomCameraResolutionType ScreenResolutionType = CustomCameraUtils.ECustomCameraResolutionType.Full;

		[Header("Rendering Settings")]
		public LayerMask CullingLayerMask = -1;

		public RenderTextureFormat RenderTextureFormat = RenderTextureFormat.ARGBHalf;

		public string ShaderTagID = "ShaderMode";

		[Header("Buffer Settings")]
		public string BufferName = "_TempBuffer";

		public Color BufferResetColor = new Color(1f, 1f, 1f, 0f);

		public FilterMode BufferFilterMode;

		public SortingCriteria SortingCriteria;

		public string BufferDepthName => string.Concat(BufferName, "Depth");
	}

	private class CustomScreenBufferPass : ScriptableRenderPass
	{
		private class PassData
		{
			public RendererListHandle RendererListHandle;

			public TextureHandle ColorTexture;

			public TextureHandle DepthTexture;

			public string BufferName = "_TempBuffer";
		}

		private CustomScreenBufferPassSetting _passSetting;

		private ProfilingSampler _profilingSampler;

		private RTHandle _customColor;

		private RTHandle _customDepth;

		private ShaderTagId _shaderTagID;

		private const string _profileTag = "CustomScreenBufferPass";

		public CustomScreenBufferPass(CustomScreenBufferPassSetting passSetting)
		{
			_profilingSampler = new ProfilingSampler("CustomScreenBufferPass");
			_passSetting = passSetting;
			_shaderTagID = new ShaderTagId(passSetting.ShaderTagID);
			renderPassEvent = _passSetting.RenderPassEvent;
		}

		public void UpdatePassSetting(CustomScreenBufferPassSetting passSetting)
		{
			_passSetting = passSetting;
			_shaderTagID = new ShaderTagId(passSetting.ShaderTagID);
			renderPassEvent = _passSetting.RenderPassEvent;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_passSetting == null)
			{
				return;
			}
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			if (cameraData.isPreviewCamera)
			{
				return;
			}
			UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
			float downsample = CustomCameraUtils.GetDownsamplingFloat(_passSetting.ScreenResolutionType);
			CustomCameraUtils.GetCappedRTSize(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height, out int cappedWidth, out int cappedHeight);
			int width = Mathf.RoundToInt((float)cappedWidth * downsample);
			int height = Mathf.RoundToInt((float)cappedHeight * downsample);
			GraphicsFormat colorFormat = GraphicsFormatUtility.GetGraphicsFormat(_passSetting.RenderTextureFormat, false);
			if (!SystemInfo.IsFormatSupported(colorFormat, GraphicsFormatUsage.Render))
			{
				colorFormat = (GraphicsFormat)8;
			}
			// TextureDesc stores checked against the raw offsets @0x182904a90 (field = offset - 0x10): color desc sets format, clearBuffer (0x76) = true, clearColor (0x78), name (0x60), filterMode (0x34), wrapMode (0x38) = 1, msaaSamples (0x4c) = 1;
			// the depth desc sets format = 0, depthBufferBits = 24, clearBuffer = true, name, filterMode, msaaSamples = 1 and writes no clearColor / wrapMode; anisoLevel is never written
			TextureDesc colorDesc = new TextureDesc(width, height, false, false);
			colorDesc.format = colorFormat;
			colorDesc.clearBuffer = true;
			colorDesc.clearColor = _passSetting.BufferResetColor;
			colorDesc.name = _passSetting.BufferName;
			colorDesc.filterMode = _passSetting.BufferFilterMode;
			colorDesc.wrapMode = TextureWrapMode.Clamp;
			colorDesc.msaaSamples = MSAASamples.None;
			TextureDesc depthDesc = new TextureDesc(width, height, false, false);
			depthDesc.format = GraphicsFormat.None;
			depthDesc.depthBufferBits = DepthBits.Depth24;
			depthDesc.clearBuffer = true;
			depthDesc.name = string.Concat(_passSetting.BufferName, "Depth");
			depthDesc.filterMode = _passSetting.BufferFilterMode;
			depthDesc.msaaSamples = MSAASamples.None;
			using (var builder = renderGraph.AddRasterRenderPass<PassData>("CustomScreenBufferPass", out var passData))
			{
				passData.ColorTexture = renderGraph.CreateTexture(colorDesc);
				passData.DepthTexture = renderGraph.CreateTexture(depthDesc);
				passData.BufferName = _passSetting.BufferName;
				SortingSettings sortingSettings = new SortingSettings(cameraData.camera);
				sortingSettings.criteria = _passSetting.SortingCriteria;
				DrawingSettings drawingSettings = new DrawingSettings(_shaderTagID, sortingSettings);
				drawingSettings.perObjectData = (PerObjectData)0x40f;
				FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all, _passSetting.CullingLayerMask, 0xFFFFFFFFu, 0);
				RendererListParams rendererListParams = new RendererListParams(renderingData.cullResults, drawingSettings, filteringSettings);
				passData.RendererListHandle = renderGraph.CreateRendererList(rendererListParams);
				builder.UseRendererList(passData.RendererListHandle);
				builder.SetRenderAttachment(passData.ColorTexture, 0, AccessFlags.Write);
				builder.SetRenderAttachmentDepth(passData.DepthTexture, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
				{
					context.cmd.DrawRendererList(data.RendererListHandle);
				});
				builder.SetGlobalTextureAfterPass(passData.ColorTexture, Shader.PropertyToID(_passSetting.BufferName));
			}
		}

		public void Dispose()
		{
			_customColor?.Release();
			_customDepth?.Release();
		}
	}

	private CustomScreenBufferPass _scriptableBufferPass;

	[SerializeField]
	private CustomScreenBufferPassSetting _passSetting;

	public override void Create()
	{
		if (_passSetting == null)
		{
			_passSetting = new CustomScreenBufferPassSetting();
		}
		if (_scriptableBufferPass == null)
		{
			_scriptableBufferPass = new CustomScreenBufferPass(_passSetting);
		}
	}

	private void OnValidate()
	{
		if (_scriptableBufferPass != null)
		{
			_scriptableBufferPass.UpdatePassSetting(_passSetting);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (!renderingData.cameraData.isPreviewCamera && _scriptableBufferPass != null && _passSetting != null)
		{
			_scriptableBufferPass.UpdatePassSetting(_passSetting);
			renderer.EnqueuePass(_scriptableBufferPass);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_scriptableBufferPass != null)
		{
			_scriptableBufferPass.Dispose();
		}
		_scriptableBufferPass = null;
	}

	public void SetResolution(CustomCameraUtils.ECustomCameraResolutionType resolutionType)
	{
		if (_passSetting != null)
		{
			_passSetting.ScreenResolutionType = resolutionType;
		}
	}
}

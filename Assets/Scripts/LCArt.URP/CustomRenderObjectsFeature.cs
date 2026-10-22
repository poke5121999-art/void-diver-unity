using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class CustomRenderObjectsFeature : ScriptableRendererFeature
{
	public enum CustomRenderQueueRange
	{
		All = 0,
		Opaque = 1,
		Transparent = 2
	}

	[Serializable]
	public class RenderPassSettings
	{
		public string PassTag = "Render Pass";

		public RenderPassEvent RenderPassEvent = (RenderPassEvent)300;

		public CustomRenderQueueRange RenderQueueRange;

		public SortingCriteria SortingCriteria = (SortingCriteria)59;

		public LayerMask LayerMask = -1;

		public Material OverrideMaterial;

		public int MaterialPassIndex;

		public Shader OverrideShader;

		public int ShaderPassIndex;

		public string[] LightModeTags;

		public bool OnlyGameView;
	}

	private class RenderObjectsPass : ScriptableRenderPass
	{
		private class PassData
		{
			public RendererListHandle RendererListHandle;
		}

		private FilteringSettings _filteringSettings;

		private Material _overrideMaterial;

		private int _overrideMaterialPassIndex;

		private Shader _overrideShader;

		private int _overrideShaderPassIndex;

		private List<ShaderTagId> _shaderTagIds;

		private string _profilerTag;

		private bool _onlyGameView;

		private SortingCriteria _sortingCriteria = (SortingCriteria)59;

		private RenderQueueRange ConvertCustomRenderQueueRange(CustomRenderQueueRange queueRange)
		{
			switch (queueRange)
			{
			case CustomRenderQueueRange.Opaque:
				return new RenderQueueRange(0, 2500);
			case CustomRenderQueueRange.Transparent:
				return new RenderQueueRange(2501, 5000);
			default:
				return new RenderQueueRange(0, 5000);
			}
		}

		public RenderObjectsPass(string profilerTag, CustomRenderQueueRange queueRange, LayerMask layerMask, Material material, int passIndex, Shader shader, int shaderPassIndex, string[] lightModes, SortingCriteria sortingCriteria, bool onlyGameView)
		{
			_profilerTag = profilerTag;
			_overrideMaterial = material;
			_overrideShader = shader;
			_overrideMaterialPassIndex = passIndex;
			_overrideShaderPassIndex = shaderPassIndex;
			_filteringSettings = new FilteringSettings(ConvertCustomRenderQueueRange(queueRange), layerMask, 0xFFFFFFFFu, 0);
			_sortingCriteria = sortingCriteria;
			_onlyGameView = onlyGameView;
			if (lightModes != null && lightModes.Length != 0)
			{
				_shaderTagIds = new List<ShaderTagId>(lightModes.Length);
				for (int i = 0; i < lightModes.Length; i++)
				{
					_shaderTagIds.Add(new ShaderTagId(lightModes[i]));
				}
			}
			else
			{
				_shaderTagIds = new List<ShaderTagId>();
				_shaderTagIds.Add(new ShaderTagId("UniversalForward"));
				_shaderTagIds.Add(new ShaderTagId("SRPDefaultUnlit"));
			}
		}

		public void UpdatePassSetting(string profilerTag, CustomRenderQueueRange queueRange, LayerMask layerMask, Material material, int passIndex, Shader shader, int shaderPassIndex, string[] lightModes, bool onlyGameView)
		{
			_profilerTag = profilerTag;
			_overrideMaterial = material;
			_overrideShader = shader;
			_overrideMaterialPassIndex = passIndex;
			_overrideShaderPassIndex = shaderPassIndex;
			_filteringSettings = new FilteringSettings(ConvertCustomRenderQueueRange(queueRange), layerMask, 0xFFFFFFFFu, 0);
			_onlyGameView = onlyGameView;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
			UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
			if (_onlyGameView && (cameraData.isSceneViewCamera || cameraData.isPreviewCamera))
			{
				return;
			}
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(_profilerTag, out var passData))
			{
				SortingSettings sortingSettings = new SortingSettings(cameraData.camera);
				sortingSettings.criteria = _sortingCriteria;
				DrawingSettings drawingSettings = new DrawingSettings(_shaderTagIds[0], sortingSettings);
				drawingSettings.perObjectData = PerObjectData.None;
				drawingSettings.overrideMaterial = _overrideMaterial;
				drawingSettings.overrideMaterialPassIndex = _overrideMaterialPassIndex;
				drawingSettings.overrideShader = _overrideShader;
				drawingSettings.overrideShaderPassIndex = _overrideShaderPassIndex;
				for (int i = 1; i < _shaderTagIds.Count; i++)
				{
					drawingSettings.SetShaderPassName(i, _shaderTagIds[i]);
				}
				RendererListParams rendererListParams = new RendererListParams(renderingData.cullResults, drawingSettings, _filteringSettings);
				passData.RendererListHandle = renderGraph.CreateRendererList(rendererListParams);
				builder.UseRendererList(passData.RendererListHandle);
				builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);
				builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.ReadWrite);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
				{
					context.cmd.DrawRendererList(data.RendererListHandle);
				});
			}
		}
	}

	public RenderPassSettings[] RenderPasses;

	private List<RenderObjectsPass> _passes = new List<RenderObjectsPass>();

	public override void Create()
	{
		_passes.Clear();
		if (RenderPasses == null)
		{
			return;
		}
		for (int i = 0; i < RenderPasses.Length; i++)
		{
			RenderPassSettings settings = RenderPasses[i];
			RenderObjectsPass pass = new RenderObjectsPass(settings.PassTag, settings.RenderQueueRange, settings.LayerMask, settings.OverrideMaterial, settings.MaterialPassIndex, settings.OverrideShader, settings.ShaderPassIndex, settings.LightModeTags, settings.SortingCriteria, settings.OnlyGameView);
			pass.renderPassEvent = settings.RenderPassEvent;
			_passes.Add(pass);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		for (int i = 0; i < _passes.Count; i++)
		{
			RenderPassSettings settings = RenderPasses[i];
			_passes[i].UpdatePassSetting(settings.PassTag, settings.RenderQueueRange, settings.LayerMask, settings.OverrideMaterial, settings.MaterialPassIndex, settings.OverrideShader, settings.ShaderPassIndex, settings.LightModeTags, settings.OnlyGameView);
		}
		foreach (RenderObjectsPass pass in _passes)
		{
			renderer.EnqueuePass(pass);
		}
	}
}

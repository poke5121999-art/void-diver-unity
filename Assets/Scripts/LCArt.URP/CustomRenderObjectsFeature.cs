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
		public string PassTag;

		public RenderPassEvent RenderPassEvent;

		public CustomRenderQueueRange RenderQueueRange;

		public SortingCriteria SortingCriteria;

		public LayerMask LayerMask;

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

		private SortingCriteria _sortingCriteria;

		private RenderQueueRange ConvertCustomRenderQueueRange(CustomRenderQueueRange queueRange)
		{
			return default;
		}

		public RenderObjectsPass(string profilerTag, CustomRenderQueueRange queueRange, LayerMask layerMask, Material material, int passIndex, Shader shader, int shaderPassIndex, string[] lightModes, SortingCriteria sortingCriteria, bool onlyGameView)
		{
		}

		public void UpdatePassSetting(string profilerTag, CustomRenderQueueRange queueRange, LayerMask layerMask, Material material, int passIndex, Shader shader, int shaderPassIndex, string[] lightModes, bool onlyGameView)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}
	}

	public RenderPassSettings[] RenderPasses;

	private List<RenderObjectsPass> _passes;

	public override void Create()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}
}

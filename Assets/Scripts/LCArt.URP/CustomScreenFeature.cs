using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class CustomScreenFeature : ScriptableRendererFeature
{
	[Serializable]
	public class MaterialPassInfo
	{
		public Material CustomMaterial;

		public bool UsingBlitTexture;
	}

	[Serializable]
	public class CustomScreenSetting
	{
		[Header("Custom Screen Render Order Setting")]
		public RenderPassEvent CustomScreenPassEvent;

		public List<MaterialPassInfo> CustomMaterialPassInfos;
	}

	public class CustomScreenPass : ScriptableRenderPass
	{
		private class PassData
		{
			public TextureHandle SourceTexture;

			public TextureHandle TempTexture;

			public Material Material;

			public bool UsingBlitTexture;

			public Mesh FullscreenMesh;
		}

		private CustomScreenSetting _customScreenSetting;

		private RTHandle _tempTexture;

		private const string _profilerTag = "CustomScreenPass";

		private static readonly Mesh _fullscreenTriangle;

		private static Mesh GenerateFullscreenTriangle()
		{
			return null;
		}

		public CustomScreenPass(CustomScreenSetting defaultSettings)
		{
		}

		public void UpdateSettings(CustomScreenSetting settings)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}

		private TextureHandle RenderWithBlit(RenderGraph renderGraph, UniversalResourceData resourceData, TextureHandle tempTexture, Material material, int passIndex)
		{
			return default;
		}

		private void RenderWithFullscreenMesh(RenderGraph renderGraph, UniversalResourceData resourceData, Material material, int passIndex)
		{
		}

		public void Dispose()
		{
		}
	}

	[SerializeField]
	private CustomScreenSetting _settings;

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty;

	private CustomScreenPass _renderPass;

	public CustomScreenSetting Setting => null;

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

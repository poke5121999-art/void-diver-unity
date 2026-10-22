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
		public RenderPassEvent RenderPassEvent;

		[Range(0f, 5f)]
		public int ScreenBlurDegree;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations;

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
		}

		public void UpdatePassSetting(ScreenBlurSettings setting)
		{
		}

		public void SetBlurMaterial(ref Material blurMaterial)
		{
		}

		public void SetCustomMaterial(ref Material customMaterial)
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
	private ScreenBlurSettings _settings;

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty;

	private ScreenBlurRenderPass _blurRenderPass;

	private Material _material;

	private Material _customMaterial;

	private Material _customMaterialSource;

	public ScreenBlurSettings Setting => null;

	public void SetDownsample(int downsample)
	{
	}

	public void SetIterations(int iterations)
	{
	}

	public void SetScreenBlurDegree(int degree)
	{
	}

	public override void Create()
	{
	}

	private void OnValidate()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	private void EnsureMaterials()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}
}

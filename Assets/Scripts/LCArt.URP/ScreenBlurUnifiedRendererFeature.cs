using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenBlurUnifiedRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenBlurUnifiedSettings
	{
		public RenderPassEvent RenderPassEvent;

		[Tooltip("다운샘플 레벨 (1 권장: 바이리니어 필터링으로 격자 아티팩트 감소)")]
		[Range(0f, 3f)]
		public int Downsample;

		[Tooltip("Kawase 블러 반복 횟수 (높을수록 넓은 블러)")]
		[Range(1f, 8f)]
		public int Iterations;
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

		private static readonly int UnifiedBlurDepthTextureId;

		private static readonly int KawaseOffsetId;

		private static readonly int OriginalCameraColorId;

		public ScreenBlurUnifiedRenderPass(ScreenBlurUnifiedSettings defaultSettings)
		{
		}

		public void UpdatePassSetting(ScreenBlurUnifiedSettings setting)
		{
		}

		public void SetMaterial(Material material)
		{
		}

		public void SetRenderPassEvent(RenderPassEvent ev)
		{
		}

		public void SetDownsample(int downsample)
		{
		}

		public void SetIterations(int iterations)
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
	private ScreenBlurUnifiedSettings _settings;

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty;

	private ScreenBlurUnifiedRenderPass _blurRenderPass;

	private Material _material;

	public override void Create()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	private void EnsureMaterial()
	{
	}

	private void OnValidate()
	{
	}

	public void SetMaterial(Material material)
	{
	}

	public void SetRenderPassEvent(RenderPassEvent ev)
	{
	}

	public void SetDownsample(int downsample)
	{
	}

	public void SetIterations(int iterations)
	{
	}

	public void UpdateSettings(ScreenBlurUnifiedSettings settings)
	{
	}
}

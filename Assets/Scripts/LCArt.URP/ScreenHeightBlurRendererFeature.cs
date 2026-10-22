using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ScreenHeightBlurRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public class ScreenHeightBlurSetting
	{
		public RenderPassEvent RenderPassEvent;

		[Range(0f, 3f)]
		public int Downsample;

		[Range(1f, 4f)]
		public int Iterations;
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

		private int _autoIterations;

		private float _autoBlurScale;

		public void SetAutoBlur(int iterations, float blurScale)
		{
		}

		public ScreenHeightBlurRenderPass(ScreenHeightBlurSetting defaultSettings)
		{
		}

		public void SetMaterial(ref Material material)
		{
		}

		public void UpdatePassSetting(ScreenHeightBlurSetting setting)
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
	private ScreenHeightBlurSetting _settings;

	private ScreenHeightBlurRenderPass _blurRenderPass;

	private Material _material;

	private static void CalcAutoBlur(int degree, out int iterations, out float blurScale)
	{
		iterations = default;
		blurScale = default;
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

	protected override void Dispose(bool disposing)
	{
	}
}

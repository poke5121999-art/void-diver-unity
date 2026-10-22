using System;
using LCArt.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ScreenSpaceShadowRendererFeature : ScriptableRendererFeature, IRenderPassEventAware
{
	[Serializable]
	private class ScreenSpaceShadowSetting
	{
		[FormerlySerializedAs("SSSRenderPassEvent")]
		public RenderPassEvent RenderPassEvent;

		public RenderTextureFormat RenderTextureFormat;

		[Header("Screen Mask")]
		[Tooltip("화면 외각 그림자 마스크 텍스처 (Vignette). 흰색=그림자 표시, 검정=그림자 숨김. 미지정 시 마스크 없음.")]
		public Texture2D ScreenMask;

		[Header("Shadow Caster Filter")]
		[Tooltip("그림자를 드로우할 레이어. 체크 해제된 레이어의 FakeShadowCaster는 스킵됨.")]
		public LayerMask ShadowCasterLayers;
	}

	private class ScreenSpaceShadowPass : ScriptableRenderPass
	{
		private class ComputePassData
		{
			public TextureHandle DestTexture;

			public TextureHandle PrevTexture;

			public Material Material;

			public int FrameIndex;

			public Matrix4x4 PrevVPMatrix;

			public bool IsFirstFrame;

			public Vector4 RTSize;
		}

		private class ObjectShadowPassData
		{
			public TextureHandle FullResShadowTexture;

			public TextureHandle LowResShadowTexture;

			public TextureHandle DepthTexture;

			public Material Material;

			public int ShadowCasterLayers;
		}

		private class ApplyPassData
		{
			public TextureHandle ShadowTexture;

			public Material Material;
		}

		private ScreenSpaceShadowSetting _setting;

		private Material _material;

		private RTHandle _shadowRT;

		private RTHandle _shadowRT_Ping;

		private RTHandle _shadowRT_Pong;

		private int _frameIndex;

		private Matrix4x4 _prevVPMatrix;

		private bool _isFirstFrame;

		private RTHandle _fullResShadowRT;

		private const string _profilerTag = "ScreenSpaceShadowPass";

		private const int SHADOW_RT_FIXED_HEIGHT = 360;

		private static readonly int _blitScaleBiasId;

		private static readonly int _frameIndexId;

		private static readonly int _prevVPMatrixId;

		private static readonly int _isFirstFrameId;

		private static readonly int _prevShadowTexId;

		private static readonly int _shadowRTSizeId;

		public ScreenSpaceShadowPass(ScreenSpaceShadowSetting setting)
		{
		}

		public void UpdateSettings(ScreenSpaceShadowSetting setting)
		{
		}

		public void SetMaterial(ref Material material)
		{
		}

		public void UpdatePassSetting(ScreenSpaceShadowSetting setting)
		{
		}

		private void EnsureShadowRTs(int width, int height, RenderTextureFormat format)
		{
		}

		private void EnsureFullResShadowRT(int width, int height, RenderTextureFormat format)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}

		public void Dispose()
		{
		}
	}

	[SerializeField]
	private ScreenSpaceShadowSetting _settings;

	private ScreenSpaceShadowPass _screenSpaceShadowPass;

	private Material _material;

	private Shader _materialSourceShader;

	public override void Create()
	{
	}

	private void OnValidate()
	{
	}

	private void EnsureMaterial()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void SetRenderPassEvent(RenderPassEvent renderPassEvent)
	{
	}
}

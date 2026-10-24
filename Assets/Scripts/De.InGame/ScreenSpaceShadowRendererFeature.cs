using System;
using System.Collections.Generic;
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
		public RenderPassEvent RenderPassEvent = (RenderPassEvent)300;

		public RenderTextureFormat RenderTextureFormat = RenderTextureFormat.Default;

		[Header("Screen Mask")]
		[Tooltip("화면 외각 그림자 마스크 텍스처 (Vignette). 흰색=그림자 표시, 검정=그림자 숨김. 미지정 시 마스크 없음.")]
		public Texture2D ScreenMask;

		[Header("Shadow Caster Filter")]
		[Tooltip("그림자를 드로우할 레이어. 체크 해제된 레이어의 FakeShadowCaster는 스킵됨.")]
		public LayerMask ShadowCasterLayers = -1;
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

		private ScreenSpaceShadowSetting _setting = new ScreenSpaceShadowSetting();

		private Material _material;

		private RTHandle _shadowRT;

		private RTHandle _shadowRT_Ping;

		private RTHandle _shadowRT_Pong;

		private int _frameIndex;

		private Matrix4x4 _prevVPMatrix = Matrix4x4.identity;

		private bool _isFirstFrame = true;

		private RTHandle _fullResShadowRT;

		private const string _profilerTag = "ScreenSpaceShadowPass";

		private const int SHADOW_RT_FIXED_HEIGHT = 360;

		private static readonly int _blitScaleBiasId = Shader.PropertyToID("_BlitScaleBias");

		private static readonly int _frameIndexId = Shader.PropertyToID("_SSShadowFrameIndex");

		private static readonly int _prevVPMatrixId = Shader.PropertyToID("_SSShadowPrevVPMatrix");

		private static readonly int _isFirstFrameId = Shader.PropertyToID("_SSShadowIsFirstFrame");

		private static readonly int _prevShadowTexId = Shader.PropertyToID("_PrevShadowTexture");

		private static readonly int _shadowRTSizeId = Shader.PropertyToID("_ShadowRTSize");

		public ScreenSpaceShadowPass(ScreenSpaceShadowSetting setting)
		{
			_setting = setting;
		}

		public void UpdateSettings(ScreenSpaceShadowSetting setting)
		{
			_setting = setting;
		}

		public void SetMaterial(ref Material material)
		{
			_material = material;
		}

		public void UpdatePassSetting(ScreenSpaceShadowSetting setting)
		{
			_setting = setting;
			base.renderPassEvent = setting.RenderPassEvent;
		}

		private void EnsureShadowRTs(int width, int height, RenderTextureFormat format)
		{
			RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height, format, 0, 0);
			descriptor.msaaSamples = 1;
			RenderingUtils.ReAllocateHandleIfNeeded(ref _shadowRT_Ping, in descriptor, FilterMode.Bilinear, TextureWrapMode.Repeat, 1, 0f, "_SSShadowRT_Ping");
			RenderingUtils.ReAllocateHandleIfNeeded(ref _shadowRT_Pong, in descriptor, FilterMode.Bilinear, TextureWrapMode.Repeat, 1, 0f, "_SSShadowRT_Pong");
		}

		private void EnsureFullResShadowRT(int width, int height, RenderTextureFormat format)
		{
			RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height, format, 0, 0);
			descriptor.msaaSamples = 1;
			RenderingUtils.ReAllocateHandleIfNeeded(ref _fullResShadowRT, in descriptor, FilterMode.Bilinear, TextureWrapMode.Repeat, 1, 0f, "_SSShadowRT_FullRes");
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_material == null || _setting == null)
			{
				return;
			}
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			if (!universalResourceData.activeColorTexture.IsValid())
			{
				return;
			}
			int cameraWidth = universalCameraData.cameraTargetDescriptor.width;
			int cameraHeight = universalCameraData.cameraTargetDescriptor.height;
			int shadowRTWidth = Mathf.RoundToInt((float)cameraWidth / (float)Mathf.Max(cameraHeight, 1) * 360f);
			if (shadowRTWidth < 1)
			{
				shadowRTWidth = 1;
			}
			// FUN_180069e90 is Mathf.RoundToInt (machine code checked, same body as @0x183ee7ca0)
			EnsureShadowRTs(shadowRTWidth, SHADOW_RT_FIXED_HEIGHT, _setting.RenderTextureFormat);
			EnsureFullResShadowRT(cameraWidth, cameraHeight, _setting.RenderTextureFormat);
			RTHandle destRT;
			RTHandle prevRT;
			if ((_frameIndex & 1) == 0)
			{
				destRT = _shadowRT_Ping;
				prevRT = _shadowRT_Pong;
			}
			else
			{
				destRT = _shadowRT_Pong;
				prevRT = _shadowRT_Ping;
			}
			TextureHandle destTexture = renderGraph.ImportTexture(destRT);
			TextureHandle prevTexture = renderGraph.ImportTexture(prevRT);
			TextureHandle fullResTexture = renderGraph.ImportTexture(_fullResShadowRT);
			Matrix4x4 viewMatrix = universalCameraData.GetViewMatrix();
			Matrix4x4 projectionMatrix = universalCameraData.GetProjectionMatrix();
			Matrix4x4 vpMatrix = GL.GetGPUProjectionMatrix(projectionMatrix, renderIntoTexture: true) * viewMatrix;
			ComputePassData computeData;
			using (IUnsafeRenderGraphBuilder builder = renderGraph.AddUnsafePass<ComputePassData>("ScreenSpaceShadowPass_Compute", out computeData))
			{
				computeData.DestTexture = destTexture;
				computeData.PrevTexture = prevTexture;
				computeData.Material = _material;
				computeData.FrameIndex = _frameIndex;
				computeData.PrevVPMatrix = _prevVPMatrix;
				computeData.IsFirstFrame = _isFirstFrame;
				computeData.RTSize = new Vector4(shadowRTWidth, 360f, 1f / (float)shadowRTWidth, 0.0027777778f);
				builder.UseTexture(in computeData.DestTexture, AccessFlags.WriteAll);
				builder.UseTexture(in computeData.PrevTexture);
				if (universalResourceData.activeDepthTexture.IsValid())
				{
					builder.UseTexture(universalResourceData.activeDepthTexture);
				}
				builder.AllowPassCulling(value: false);
				builder.SetRenderFunc<ComputePassData>(static (ComputePassData data, UnsafeGraphContext context) =>
				{
					UnsafeCommandBuffer cmd = context.cmd;
					cmd.SetGlobalVector(_blitScaleBiasId, new Vector4(1f, 1f, 0f, 0f));
					cmd.SetGlobalFloat(_frameIndexId, data.FrameIndex);
					cmd.SetGlobalFloat(_isFirstFrameId, data.IsFirstFrame ? 1f : 0f);
					cmd.SetGlobalMatrix(_prevVPMatrixId, data.PrevVPMatrix);
					cmd.SetGlobalVector(_shadowRTSizeId, data.RTSize);
					cmd.SetGlobalTexture(_prevShadowTexId, data.PrevTexture);
					cmd.SetRenderTarget(data.DestTexture);
					cmd.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
					cmd.DrawProcedural(Matrix4x4.identity, data.Material, 0, MeshTopology.Triangles, 3);
				});
			}
			ObjectShadowPassData objectData;
			using (IUnsafeRenderGraphBuilder builder2 = renderGraph.AddUnsafePass<ObjectShadowPassData>("ScreenSpaceShadowPass_ObjectShadow", out objectData))
			{
				objectData.FullResShadowTexture = fullResTexture;
				objectData.LowResShadowTexture = destTexture;
				objectData.DepthTexture = universalResourceData.activeDepthTexture;
				objectData.Material = _material;
				objectData.ShadowCasterLayers = _setting.ShadowCasterLayers.value;
				builder2.UseTexture(in objectData.FullResShadowTexture, AccessFlags.WriteAll);
				builder2.UseTexture(in objectData.LowResShadowTexture);
				if (universalResourceData.activeDepthTexture.IsValid())
				{
					builder2.UseTexture(in objectData.DepthTexture);
				}
				builder2.AllowPassCulling(value: false);
				builder2.SetRenderFunc<ObjectShadowPassData>(static (ObjectShadowPassData data, UnsafeGraphContext context) =>
				{
					UnsafeCommandBuffer cmd = context.cmd;
					cmd.SetRenderTarget(data.FullResShadowTexture);
					cmd.SetGlobalVector(_blitScaleBiasId, new Vector4(1f, 1f, 0f, 0f));
					cmd.SetGlobalTexture("_BlitTexture", data.LowResShadowTexture);
					cmd.DrawProcedural(Matrix4x4.identity, Blitter.GetBlitMaterial(TextureDimension.Tex2D), 0, MeshTopology.Triangles, 3);
					cmd.SetRenderTarget(data.FullResShadowTexture, data.DepthTexture);
					CommandBuffer nativeCmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
					IReadOnlyList<FakeShadowCaster> casters = FakeShadowCaster.ActiveCasters;
					int layers = data.ShadowCasterLayers;
					for (int i = 0; i < casters.Count; i++)
					{
						if (((layers >> casters[i].gameObject.layer) & 1) != 0)
						{
							casters[i].ExecuteDraw(nativeCmd);
						}
					}
				});
			}
			ApplyPassData applyData;
			using (IRasterRenderGraphBuilder builder3 = renderGraph.AddRasterRenderPass<ApplyPassData>("ScreenSpaceShadowPass_Apply", out applyData))
			{
				applyData.ShadowTexture = fullResTexture;
				applyData.Material = _material;
				builder3.UseTexture(in applyData.ShadowTexture);
				builder3.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder3.AllowPassCulling(value: false);
				builder3.SetRenderFunc<ApplyPassData>(static (ApplyPassData data, RasterGraphContext context) =>
				{
					Blitter.BlitTexture(context.cmd, data.ShadowTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, 2);
				});
			}
			_prevVPMatrix = vpMatrix;
			_frameIndex++;
			_isFirstFrame = false;
		}

		public void Dispose()
		{
			_shadowRT?.Release();
			_shadowRT_Ping?.Release();
			_shadowRT_Pong?.Release();
			_fullResShadowRT?.Release();
		}
	}

	[SerializeField]
	private ScreenSpaceShadowSetting _settings = new ScreenSpaceShadowSetting();

	private ScreenSpaceShadowPass _screenSpaceShadowPass;

	private Material _material;

	private Shader _materialSourceShader;

	public override void Create()
	{
		if (_settings == null)
		{
			_settings = new ScreenSpaceShadowSetting();
		}
		if (_screenSpaceShadowPass == null)
		{
			_screenSpaceShadowPass = new ScreenSpaceShadowPass(_settings);
		}
		_screenSpaceShadowPass.renderPassEvent = _settings.RenderPassEvent;
		EnsureMaterial();
		if (_material != null)
		{
			_screenSpaceShadowPass.SetMaterial(ref _material);
		}
	}

	private void OnValidate()
	{
		if (_screenSpaceShadowPass != null)
		{
			_screenSpaceShadowPass.UpdatePassSetting(_settings);
		}
	}

	private void EnsureMaterial()
	{
		Shader shader = Shader.Find(FakeShadowConsts.SHADER_NAME);
		if (!(shader == null))
		{
			if (!(_material == null) && !(_materialSourceShader != shader))
			{
				return;
			}
			if (_material != null)
			{
				CoreUtils.Destroy(_material);
			}
			_material = CoreUtils.CreateEngineMaterial(shader);
			_materialSourceShader = shader;
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.cameraType != CameraType.Game)
		{
			return;
		}
		if (_settings == null)
		{
			_settings = new ScreenSpaceShadowSetting();
		}
		if (_screenSpaceShadowPass == null)
		{
			_screenSpaceShadowPass = new ScreenSpaceShadowPass(_settings);
		}
		EnsureMaterial();
		_screenSpaceShadowPass.UpdateSettings(_settings);
		_screenSpaceShadowPass.SetMaterial(ref _material);
		_screenSpaceShadowPass.renderPassEvent = _settings.RenderPassEvent;
		if (_material != null)
		{
			_material.SetTexture(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_SHADOW_MASK, (_settings.ScreenMask != null) ? ((Texture)_settings.ScreenMask) : ((Texture)Texture2D.whiteTexture));
		}
		renderer.EnqueuePass(_screenSpaceShadowPass);
	}

	protected override void Dispose(bool disposing)
	{
		if (_screenSpaceShadowPass != null)
		{
			_screenSpaceShadowPass.Dispose();
		}
		_screenSpaceShadowPass = null;
	}

	public void SetRenderPassEvent(RenderPassEvent renderPassEvent)
	{
		if (_settings != null)
		{
			_settings.RenderPassEvent = renderPassEvent;
		}
		if (_screenSpaceShadowPass != null)
		{
			_screenSpaceShadowPass.renderPassEvent = renderPassEvent;
		}
	}
}

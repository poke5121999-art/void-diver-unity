using System;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	[Serializable]
	public class CustomScreenSpaceAmbientOcclusion : ScriptableRendererFeature
	{
		[Serializable]
		public class CustomScreenSpaceAmbientOcclusionSettings
		{
			internal enum CustomDepthSource
			{
				Depth = 0,
				DepthNormals = 1
			}

			internal enum CustomNormalQuality
			{
				Low = 0,
				Medium = 1,
				High = 2
			}

			internal enum CustomAOSampleOption
			{
				High = 0,
				Medium = 1,
				Low = 2
			}

			internal enum CustomAOMethodOptions
			{
				BlueNoise = 0,
				InterleavedGradient = 1
			}

			internal enum CustomBlurQualityOptions
			{
				High = 0,
				Medium = 1,
				Low = 2
			}

			[SerializeField]
			internal RenderPassEvent RenderPassEvent = (RenderPassEvent)300;

			[SerializeField]
			internal bool Downsample;

			[SerializeField]
			internal CustomDepthSource Source = CustomDepthSource.DepthNormals;

			[SerializeField]
			internal CustomNormalQuality NormalSamples = CustomNormalQuality.Medium;

			[SerializeField]
			internal float DirectLightingStrength = 0.25f;

			[SerializeField]
			internal CustomAOSampleOption Samples = CustomAOSampleOption.Medium;

			[SerializeField]
			internal CustomBlurQualityOptions BlurQuality;

			[SerializeField]
			public Color CustomColor = new Color(0f, 0f, 0f, 1f);

			public float Radius = 0.5f;

			public float Intensity = 5f;

			public float FallOff = 500f;
		}

		internal class CustomScreenSpaceAmbientOcclusionPass : ScriptableRenderPass
		{
			private static class CustomShaderConstants
			{
				public static readonly int SourceSize = Shader.PropertyToID("_SourceSize");
			}

			private enum CustomBlurTypes
			{
				Bilateral = 0,
				Gaussian = 1,
				Kawase = 2
			}

			private enum CustomShaderPasses
			{
				AmbientOcclusion = 0,
				BilateralBlurHorizontal = 1,
				BilateralBlurVertical = 2,
				BilateralBlurFinal = 3,
				BilateralAfterOpaque = 4,
				GaussianBlurHorizontal = 5,
				GaussianBlurVertical = 6,
				GaussianAfterOpaque = 7,
				KawaseBlur = 8,
				KawaseAfterOpaque = 9
			}

			private class SSAOPassData
			{
				internal Material Material;

				internal CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions BlurQuality;

				internal float DirectLightingStrength;

				internal TextureHandle CameraColor;

				internal bool IsDepthNormals;

				internal CustomScreenSpaceAmbientOcclusionSettings.CustomNormalQuality NormalSamples;

				internal CustomScreenSpaceAmbientOcclusionSettings.CustomAOSampleOption Samples;

				internal TextureHandle AOTexture;

				internal TextureHandle FinalTexture;

				internal TextureHandle BlurTexture;

				internal TextureHandle CameraNormalsTexture;
			}

			private bool _supportsR8RenderTextureFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8);

			private Material _material;

			private Texture2D[] _blueNoiseTextures;

			private Vector4[] _cameraTopLeftCorner = new Vector4[2];

			private Vector4[] _cameraXExtent = new Vector4[2];

			private Vector4[] _cameraYExtent = new Vector4[2];

			private Vector4[] _cameraZExtent = new Vector4[2];

			private RTHandle[] _ssaoTextures = new RTHandle[4];

			private RTHandle _cameraDepthTargetHandle;

			private CustomBlurTypes _blurType = CustomBlurTypes.Bilateral;

			private Matrix4x4[] _cameraViewProjections = new Matrix4x4[2];

			private ScriptableRenderer _renderer;

			private RenderTextureDescriptor _aoPassDescriptor;

			private CustomScreenSpaceAmbientOcclusionSettings _currentSettings;

			private const int _finalTexID = 3;

			private const string _ssaoTextureName = "_ScreenSpaceOcclusionTexture";

			private const string _ambientOcclusionParamName = "_AmbientOcclusionParam";

			internal static readonly int AmbientOcclusionParamID = Shader.PropertyToID("_AmbientOcclusionParam");

			private static readonly int _ssaoParamsID = Shader.PropertyToID("_SSAOParams");

			private static readonly int _ssaoBlueNoiseParamsID = Shader.PropertyToID("_SSAOBlueNoiseParams");

			private static readonly int _lastKawasePass = Shader.PropertyToID("_LastKawasePass");

			private static readonly int _blueNoiseTextureID = Shader.PropertyToID("_BlueNoiseTexture");

			private static readonly int _cameraViewXExtentID = Shader.PropertyToID("_CameraViewXExtent");

			private static readonly int _cameraViewYExtentID = Shader.PropertyToID("_CameraViewYExtent");

			private static readonly int _cameraViewZExtentID = Shader.PropertyToID("_CameraViewZExtent");

			private static readonly int _projectionParams2ID = Shader.PropertyToID("_ProjectionParams2");

			private static readonly int _kawaseBlurIterationID = Shader.PropertyToID("_KawaseBlurIteration");

			private static readonly int _cameraViewProjectionsID = Shader.PropertyToID("_CameraViewProjections");

			private static readonly int _cameraViewTopLeftCornerID = Shader.PropertyToID("_CameraViewTopLeftCorner");

			private static readonly int _ssaoColor = Shader.PropertyToID("_CustomColor");

			private static readonly int[] _bilateralTexturesIndices = new int[4] { 0, 1, 2, 3 };

			private static readonly CustomShaderPasses[] _bilateralPasses = new CustomShaderPasses[3]
			{
				CustomShaderPasses.BilateralBlurHorizontal,
				CustomShaderPasses.BilateralBlurVertical,
				CustomShaderPasses.BilateralBlurFinal
			};

			private static readonly CustomShaderPasses[] _bilateralAfterOpaquePasses = new CustomShaderPasses[3]
			{
				CustomShaderPasses.BilateralBlurHorizontal,
				CustomShaderPasses.BilateralBlurVertical,
				CustomShaderPasses.BilateralAfterOpaque
			};

			private static readonly int[] _gaussianTexturesIndices = new int[4] { 0, 1, 3, 3 };

			private static readonly CustomShaderPasses[] _gaussianPasses = new CustomShaderPasses[2]
			{
				CustomShaderPasses.GaussianBlurHorizontal,
				CustomShaderPasses.GaussianBlurVertical
			};

			private static readonly CustomShaderPasses[] _gaussianAfterOpaquePasses = new CustomShaderPasses[2]
			{
				CustomShaderPasses.GaussianBlurHorizontal,
				CustomShaderPasses.GaussianAfterOpaque
			};

			private static readonly int[] _kawaseTexturesIndices = new int[2] { 0, 3 };

			private static readonly CustomShaderPasses[] _kawasePasses = new CustomShaderPasses[1] { CustomShaderPasses.KawaseBlur };

			private static readonly CustomShaderPasses[] _kawaseAfterOpaquePasses = new CustomShaderPasses[1] { CustomShaderPasses.KawaseAfterOpaque };

			private static readonly int _cameraNormalsTextureID = Shader.PropertyToID("_CameraNormalsTexture");

			internal CustomScreenSpaceAmbientOcclusionPass()
			{
				_currentSettings = new CustomScreenSpaceAmbientOcclusionSettings();
				base.requiresIntermediateTexture = true;
			}

			internal void UpdatePassSetting(CustomScreenSpaceAmbientOcclusionSettings featureSettings)
			{
				_currentSettings = featureSettings;
			}

			internal bool Setup(ref CustomScreenSpaceAmbientOcclusionSettings featureSettings, ref ScriptableRenderer renderer, ref Material material, ref Texture2D[] blueNoiseTextures)
			{
				_blueNoiseTextures = blueNoiseTextures;
				_material = material;
				_renderer = renderer;
				_currentSettings = featureSettings;
				base.renderPassEvent = _currentSettings.RenderPassEvent;
				switch (_currentSettings.Source)
				{
				case CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.Depth:
					ConfigureInput(ScriptableRenderPassInput.Depth);
					break;
				case CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.DepthNormals:
					ConfigureInput(ScriptableRenderPassInput.Normal);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				switch (_currentSettings.BlurQuality)
				{
				case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.High:
					_blurType = CustomBlurTypes.Bilateral;
					break;
				case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Medium:
					_blurType = CustomBlurTypes.Gaussian;
					break;
				case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Low:
					_blurType = CustomBlurTypes.Kawase;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return _material != null;
			}

			private void CustomSetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
			{
				float num = desc.width;
				float num2 = desc.height;
				if (desc.useDynamicScale)
				{
					num *= ScalableBufferManager.widthScaleFactor;
					num2 *= ScalableBufferManager.heightScaleFactor;
				}
				cmd.SetGlobalVector(CustomShaderConstants.SourceSize, new Vector4(num, num2, 1f / num, 1f / num2));
			}

			private static void GetPassOrder(CustomBlurTypes blurType, bool isAfterOpaque, out int[] textureIndices, out CustomShaderPasses[] shaderPasses)
			{
				switch (blurType)
				{
				case CustomBlurTypes.Bilateral:
					textureIndices = _bilateralTexturesIndices;
					shaderPasses = (isAfterOpaque ? _bilateralAfterOpaquePasses : _bilateralPasses);
					break;
				case CustomBlurTypes.Gaussian:
					textureIndices = _gaussianTexturesIndices;
					shaderPasses = (isAfterOpaque ? _gaussianAfterOpaquePasses : _gaussianPasses);
					break;
				case CustomBlurTypes.Kawase:
					textureIndices = _kawaseTexturesIndices;
					shaderPasses = (isAfterOpaque ? _kawaseAfterOpaquePasses : _kawasePasses);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			private static bool IsAfterOpaquePass(ref CustomShaderPasses pass)
			{
				return pass == CustomShaderPasses.BilateralAfterOpaque || pass == CustomShaderPasses.GaussianAfterOpaque || pass == CustomShaderPasses.KawaseAfterOpaque;
			}

			public void Dispose()
			{
				_ssaoTextures[0]?.Release();
				_ssaoTextures[1]?.Release();
				_ssaoTextures[2]?.Release();
				_ssaoTextures[3]?.Release();
				_cameraDepthTargetHandle?.Release();
			}

			private static void SetSourceSize(CommandBuffer cmd, TextureHandle source)
			{
				if (!source.IsValid())
				{
					return;
				}
				RTHandle rTHandle = source;
				if (rTHandle != null && !(rTHandle.rt == null))
				{
					float num = rTHandle.rt.width;
					float num2 = rTHandle.rt.height;
					if (rTHandle.rt.useDynamicScale)
					{
						num *= ScalableBufferManager.widthScaleFactor;
						num2 *= ScalableBufferManager.heightScaleFactor;
					}
					cmd.SetGlobalVector(CustomShaderConstants.SourceSize, new Vector4(num, num2, 1f / num, 1f / num2));
				}
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
				if (_material == null)
				{
					Debug.LogErrorFormat("{0}.RecordRenderGraph(): Missing material. SSAO pass will not execute.", GetType().Name);
					return;
				}
				UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
				UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
				CreateRenderTextureHandles(renderGraph, resourceData, cameraData, out var aoTexture, out var blurTexture, out var finalTexture);
				TextureHandle cameraDepthTexture = resourceData.cameraDepthTexture;
				TextureHandle cameraNormalsTexture = resourceData.cameraNormalsTexture;
				SetupKeywordsAndParameters(ref cameraData);
				using IUnsafeRenderGraphBuilder builder = renderGraph.AddUnsafePass<SSAOPassData>("Custom SSAO Pass", out var passData);
				builder.AllowGlobalStateModification(true);
				passData.Material = _material;
				passData.BlurQuality = _currentSettings.BlurQuality;
				passData.DirectLightingStrength = _currentSettings.DirectLightingStrength;
				passData.CameraColor = resourceData.cameraColor;
				passData.AOTexture = aoTexture;
				passData.FinalTexture = finalTexture;
				passData.BlurTexture = blurTexture;
				passData.IsDepthNormals = _currentSettings.Source == CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.DepthNormals;
				passData.NormalSamples = _currentSettings.NormalSamples;
				passData.Samples = _currentSettings.Samples;
				passData.CameraNormalsTexture = TextureHandle.nullHandle;
				builder.UseTexture(passData.AOTexture, AccessFlags.ReadWrite);
				if (resourceData.cameraColor.IsValid())
				{
					builder.UseTexture(resourceData.cameraColor, AccessFlags.ReadWrite);
				}
				if (_currentSettings.BlurQuality != CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Low)
				{
					builder.UseTexture(passData.BlurTexture, AccessFlags.ReadWrite);
				}
				if (cameraDepthTexture.IsValid())
				{
					builder.UseTexture(cameraDepthTexture, AccessFlags.Read);
				}
				if (_currentSettings.Source == CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.DepthNormals && cameraNormalsTexture.IsValid())
				{
					builder.UseTexture(cameraNormalsTexture, AccessFlags.Read);
					passData.CameraNormalsTexture = cameraNormalsTexture;
				}
				if (finalTexture.IsValid())
				{
					builder.UseTexture(passData.FinalTexture, AccessFlags.ReadWrite);
				}
				builder.SetRenderFunc(static (SSAOPassData data, UnsafeGraphContext rgContext) =>
				{
					CommandBuffer nativeCommandBuffer = CommandBufferHelpers.GetNativeCommandBuffer(rgContext.cmd);
					SetSourceSize(nativeCommandBuffer, data.AOTexture);
					if (data.CameraNormalsTexture.IsValid())
					{
						data.Material.SetTexture(_cameraNormalsTextureID, data.CameraNormalsTexture);
					}
					Blitter.BlitCameraTexture(nativeCommandBuffer, data.AOTexture, data.AOTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.AmbientOcclusion);
					switch (data.BlurQuality)
					{
					case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.High:
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.AOTexture, data.BlurTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.BilateralBlurHorizontal);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.BlurTexture, data.AOTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.BilateralBlurVertical);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.AOTexture, data.FinalTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.BilateralBlurFinal);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.FinalTexture, data.CameraColor, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.BilateralAfterOpaque);
						break;
					case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Medium:
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.AOTexture, data.BlurTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.GaussianBlurHorizontal);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.BlurTexture, data.FinalTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.GaussianBlurVertical);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.FinalTexture, data.CameraColor, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.GaussianAfterOpaque);
						break;
					case CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Low:
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.AOTexture, data.FinalTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.KawaseBlur);
						Blitter.BlitCameraTexture(nativeCommandBuffer, data.FinalTexture, data.CameraColor, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, data.Material, (int)CustomShaderPasses.KawaseAfterOpaque);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					rgContext.cmd.SetGlobalVector(AmbientOcclusionParamID, new Vector4(1f, 0f, 0f, data.DirectLightingStrength));
				});
			}

			private void CreateRenderTextureHandles(RenderGraph renderGraph, UniversalResourceData resourceData, UniversalCameraData cameraData, out TextureHandle aoTexture, out TextureHandle blurTexture, out TextureHandle finalTexture)
			{
				RenderTextureDescriptor desc = cameraData.cameraTargetDescriptor;
				desc.colorFormat = (_supportsR8RenderTextureFormat ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
				desc.depthBufferBits = 0;
				desc.msaaSamples = 1;
				CustomCameraUtils.GetCappedRTSize(desc.width, desc.height, out var cappedWidth, out var cappedHeight);
				desc.width = cappedWidth;
				desc.height = cappedHeight;
				int num = (_currentSettings.Downsample ? 2 : 1);
				bool flag = _supportsR8RenderTextureFormat && _blurType > CustomBlurTypes.Bilateral;
				RenderTextureDescriptor desc2 = desc;
				desc2.colorFormat = (flag ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
				desc2.width /= num;
				desc2.height /= num;
				aoTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc2, "_SSAO_OcclusionTexture0", clear: false, FilterMode.Bilinear);
				finalTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_ScreenSpaceOcclusionTexture", clear: false, FilterMode.Bilinear);
				if (_currentSettings.BlurQuality != CustomScreenSpaceAmbientOcclusionSettings.CustomBlurQualityOptions.Low)
				{
					blurTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc2, "_SSAO_OcclusionTexture1", clear: false, FilterMode.Bilinear);
				}
				else
				{
					blurTexture = TextureHandle.nullHandle;
				}
			}

			private void SetupKeywordsAndParameters(ref UniversalCameraData cameraData)
			{
				for (int i = 0; i < 1; i++)
				{
					Matrix4x4 viewMatrix = cameraData.GetViewMatrix(i);
					Matrix4x4 projectionMatrix = cameraData.GetProjectionMatrix(i);
					_cameraViewProjections[i] = projectionMatrix * viewMatrix;
					Matrix4x4 matrix4x = viewMatrix;
					matrix4x.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
					Matrix4x4 inverse = (projectionMatrix * matrix4x).inverse;
					Vector4 vector = inverse.MultiplyPoint(new Vector4(-1f, 1f, -1f, 1f));
					Vector4 vector2 = inverse.MultiplyPoint(new Vector4(1f, 1f, -1f, 1f));
					Vector4 vector3 = inverse.MultiplyPoint(new Vector4(-1f, -1f, -1f, 1f));
					Vector4 vector4 = inverse.MultiplyPoint(new Vector4(0f, 0f, 1f, 1f));
					_cameraTopLeftCorner[i] = vector;
					_cameraXExtent[i] = vector2 - vector;
					_cameraYExtent[i] = vector3 - vector;
					_cameraZExtent[i] = vector4;
				}
				_material.SetVector(_projectionParams2ID, new Vector4(1f / cameraData.camera.nearClipPlane, 0f, 0f, 0f));
				_material.SetMatrixArray(_cameraViewProjectionsID, _cameraViewProjections);
				_material.SetVectorArray(_cameraViewTopLeftCornerID, _cameraTopLeftCorner);
				_material.SetVectorArray(_cameraViewXExtentID, _cameraXExtent);
				_material.SetVectorArray(_cameraViewYExtentID, _cameraYExtent);
				_material.SetVectorArray(_cameraViewZExtentID, _cameraZExtent);
				_material.SetColor(_ssaoColor, _currentSettings.CustomColor);
				CoreUtils.SetKeyword(_material, "_ORTHOGRAPHIC", cameraData.camera.orthographic);
				CoreUtils.SetKeyword(_material, "_BLUE_NOISE", state: false);
				CoreUtils.SetKeyword(_material, "_INTERLEAVED_GRADIENT", state: true);
				int value = ((_currentSettings.Source == CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.DepthNormals) ? 0 : ((int)(_currentSettings.NormalSamples + 1)));
				_material.SetInt("_SSAOSourceMode", value);
				int value2 = ((_currentSettings.Samples == CustomScreenSpaceAmbientOcclusionSettings.CustomAOSampleOption.High) ? 12 : ((_currentSettings.Samples == CustomScreenSpaceAmbientOcclusionSettings.CustomAOSampleOption.Medium) ? 8 : 4));
				_material.SetInt("_SSAOSampleCount", value2);
				bool downsample = _currentSettings.Downsample;
				if (cameraData.isSceneViewCamera || cameraData.isPreviewCamera)
				{
					_material.SetVector(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_EDITOR_VIEW, new Vector4(0.25f, 0.5f, 1f, 1f));
				}
				else
				{
					_material.SetVector(CustomScreenSpaceAmbientOcclusionConsts.SHADER_PROPERTY_EDITOR_VIEW, Vector4.one);
				}
				_material.SetVector(_ssaoParamsID, new Vector4(_currentSettings.Intensity, _currentSettings.Radius, 1f / (float)(downsample ? 2 : 1), _currentSettings.FallOff));
			}
		}

		[SerializeField]
		private CustomScreenSpaceAmbientOcclusionSettings _settings = new CustomScreenSpaceAmbientOcclusionSettings();

		[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
		[SerializeField]
		private string _graphicGateProperty = string.Empty;

		[SerializeField]
		[HideInInspector]
		[Reload("Textures/BlueNoise256/LDR_LLL1_{0}.png", 0, 7, ReloadAttribute.Package.Root)]
		internal Texture2D[] BlueNoise256Textures;

		private Material _material;

		private CustomScreenSpaceAmbientOcclusionPass _ssaoPass;

		internal const string AOInterleavedGradientKeyword = "_INTERLEAVED_GRADIENT";

		internal const string AOBlueNoiseKeyword = "_BLUE_NOISE";

		internal const string OrthographicCameraKeyword = "_ORTHOGRAPHIC";

		internal const string SourceDepthLowKeyword = "_SOURCE_DEPTH_LOW";

		internal const string SourceDepthMediumKeyword = "_SOURCE_DEPTH_MEDIUM";

		internal const string SourceDepthHighKeyword = "_SOURCE_DEPTH_HIGH";

		internal const string SourceDepthNormalsKeyword = "_SOURCE_DEPTH_NORMALS";

		internal const string SampleCountLowKeyword = "_SAMPLE_COUNT_LOW";

		internal const string SampleCountMediumKeyword = "_SAMPLE_COUNT_MEDIUM";

		internal const string SampleCountHighKeyword = "_SAMPLE_COUNT_HIGH";

		internal ref CustomScreenSpaceAmbientOcclusionSettings Settings => ref _settings;

		public override void Create()
		{
			if (_ssaoPass == null)
			{
				_ssaoPass = new CustomScreenSpaceAmbientOcclusionPass();
			}
		}

		private void OnValidate()
		{
			if (_ssaoPass != null)
			{
				_ssaoPass.UpdatePassSetting(_settings);
			}
			if (_ssaoPass != null)
			{
				_ssaoPass.renderPassEvent = _settings.RenderPassEvent;
			}
		}

		public void SetRenderPassEvent(RenderPassEvent renderPassEvent)
		{
			_settings.RenderPassEvent = renderPassEvent;
			if (_ssaoPass != null)
			{
				_ssaoPass.renderPassEvent = renderPassEvent;
			}
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (UniversalRenderer.IsOffscreenDepthTexture(ref renderingData.cameraData) || !RenderFeatureGate.IsOpen(_graphicGateProperty))
			{
				return;
			}
			if (_material == null)
			{
				_material = CoreUtils.CreateEngineMaterial(CustomScreenSpaceAmbientOcclusionConsts.SHADER_NAME_SSAO);
			}
			if (!(_material != null))
			{
				Debug.LogErrorFormat("{0}.AddRenderPasses(): Missing material. {1} render pass will not be added.", GetType().Name, base.name);
			}
			else if (_ssaoPass.Setup(ref _settings, ref renderer, ref _material, ref BlueNoise256Textures))
			{
				renderer.EnqueuePass(_ssaoPass);
			}
		}

		public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
			if (_ssaoPass != null && _settings.Source == CustomScreenSpaceAmbientOcclusionSettings.CustomDepthSource.DepthNormals)
			{
				_ssaoPass.ConfigureInput(ScriptableRenderPassInput.Normal);
			}
		}

		protected override void Dispose(bool disposing)
		{
			_ssaoPass?.Dispose();
			_ssaoPass = null;
			CoreUtils.Destroy(_material);
			_material = null;
		}

		private bool GetMaterials()
		{
			if (_material == null)
			{
				_material = CoreUtils.CreateEngineMaterial(CustomScreenSpaceAmbientOcclusionConsts.SHADER_NAME_SSAO);
			}
			return _material != null;
		}
	}
}

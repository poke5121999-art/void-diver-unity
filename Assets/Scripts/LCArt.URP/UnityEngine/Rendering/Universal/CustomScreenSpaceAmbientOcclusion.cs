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
			internal RenderPassEvent RenderPassEvent;

			[SerializeField]
			internal bool Downsample;

			[SerializeField]
			internal CustomDepthSource Source;

			[SerializeField]
			internal CustomNormalQuality NormalSamples;

			[SerializeField]
			internal float DirectLightingStrength;

			[SerializeField]
			internal CustomAOSampleOption Samples;

			[SerializeField]
			internal CustomBlurQualityOptions BlurQuality;

			[SerializeField]
			public Color CustomColor;

			public float Radius;

			public float Intensity;

			public float FallOff;
		}

		internal class CustomScreenSpaceAmbientOcclusionPass : ScriptableRenderPass
		{
			private static class CustomShaderConstants
			{
				public static readonly int SourceSize;
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

			private bool _supportsR8RenderTextureFormat;

			private Material _material;

			private Texture2D[] _blueNoiseTextures;

			private Vector4[] _cameraTopLeftCorner;

			private Vector4[] _cameraXExtent;

			private Vector4[] _cameraYExtent;

			private Vector4[] _cameraZExtent;

			private RTHandle[] _ssaoTextures;

			private RTHandle _cameraDepthTargetHandle;

			private CustomBlurTypes _blurType;

			private Matrix4x4[] _cameraViewProjections;

			private ScriptableRenderer _renderer;

			private RenderTextureDescriptor _aoPassDescriptor;

			private CustomScreenSpaceAmbientOcclusionSettings _currentSettings;

			private const int _finalTexID = 3;

			private const string _ssaoTextureName = "_ScreenSpaceOcclusionTexture";

			private const string _ambientOcclusionParamName = "_AmbientOcclusionParam";

			internal static readonly int AmbientOcclusionParamID;

			private static readonly int _ssaoParamsID;

			private static readonly int _ssaoBlueNoiseParamsID;

			private static readonly int _lastKawasePass;

			private static readonly int _blueNoiseTextureID;

			private static readonly int _cameraViewXExtentID;

			private static readonly int _cameraViewYExtentID;

			private static readonly int _cameraViewZExtentID;

			private static readonly int _projectionParams2ID;

			private static readonly int _kawaseBlurIterationID;

			private static readonly int _cameraViewProjectionsID;

			private static readonly int _cameraViewTopLeftCornerID;

			private static readonly int _ssaoColor;

			private static readonly int[] _bilateralTexturesIndices;

			private static readonly CustomShaderPasses[] _bilateralPasses;

			private static readonly CustomShaderPasses[] _bilateralAfterOpaquePasses;

			private static readonly int[] _gaussianTexturesIndices;

			private static readonly CustomShaderPasses[] _gaussianPasses;

			private static readonly CustomShaderPasses[] _gaussianAfterOpaquePasses;

			private static readonly int[] _kawaseTexturesIndices;

			private static readonly CustomShaderPasses[] _kawasePasses;

			private static readonly CustomShaderPasses[] _kawaseAfterOpaquePasses;

			private static readonly int _cameraNormalsTextureID;

			internal CustomScreenSpaceAmbientOcclusionPass()
			{
			}

			internal void UpdatePassSetting(CustomScreenSpaceAmbientOcclusionSettings featureSettings)
			{
			}

			internal bool Setup(ref CustomScreenSpaceAmbientOcclusionSettings featureSettings, ref ScriptableRenderer renderer, ref Material material, ref Texture2D[] blueNoiseTextures)
			{
				return false;
			}

			private void CustomSetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
			{
			}

			private static void GetPassOrder(CustomBlurTypes blurType, bool isAfterOpaque, out int[] textureIndices, out CustomShaderPasses[] shaderPasses)
			{
				textureIndices = null;
				shaderPasses = null;
			}

			private static bool IsAfterOpaquePass(ref CustomShaderPasses pass)
			{
				return false;
			}

			public void Dispose()
			{
			}

			private static void SetSourceSize(CommandBuffer cmd, TextureHandle source)
			{
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
			}

			private void CreateRenderTextureHandles(RenderGraph renderGraph, UniversalResourceData resourceData, UniversalCameraData cameraData, out TextureHandle aoTexture, out TextureHandle blurTexture, out TextureHandle finalTexture)
			{
				aoTexture = default;
				blurTexture = default;
				finalTexture = default;
			}

			private void SetupKeywordsAndParameters(ref UniversalCameraData cameraData)
			{
			}
		}

		[SerializeField]
		private CustomScreenSpaceAmbientOcclusionSettings _settings;

		[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
		[SerializeField]
		private string _graphicGateProperty;

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

		internal ref CustomScreenSpaceAmbientOcclusionSettings Settings
		{
			get
			{
				throw null;
			}
		}

		public override void Create()
		{
		}

		private void OnValidate()
		{
		}

		public void SetRenderPassEvent(RenderPassEvent renderPassEvent)
		{
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
		}

		public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
		}

		protected override void Dispose(bool disposing)
		{
		}

		private bool GetMaterials()
		{
			return false;
		}
	}
}

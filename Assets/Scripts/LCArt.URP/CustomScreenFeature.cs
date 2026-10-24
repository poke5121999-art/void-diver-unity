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
		public RenderPassEvent CustomScreenPassEvent = (RenderPassEvent)600;

		public List<MaterialPassInfo> CustomMaterialPassInfos = new List<MaterialPassInfo>();
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

		private static readonly Mesh _fullscreenTriangle = GenerateFullscreenTriangle();

		private static Mesh GenerateFullscreenTriangle()
		{
			Mesh mesh = new Mesh();
			mesh.name = "Fullscreen Triangle";
			List<Vector3> vertices = new List<Vector3>();
			vertices.Add(new Vector3(-1f, -1f, 0f));
			vertices.Add(new Vector3(-1f, 3f, 0f));
			vertices.Add(new Vector3(3f, -1f, 0f));
			mesh.SetVertices(vertices);
			mesh.SetIndices(new int[3] { 0, 1, 2 }, MeshTopology.Triangles, 0, false, 0);
			mesh.UploadMeshData(true);
			return mesh;
		}

		public CustomScreenPass(CustomScreenSetting defaultSettings)
		{
			_customScreenSetting = defaultSettings;
			renderPassEvent = _customScreenSetting.CustomScreenPassEvent;
		}

		public void UpdateSettings(CustomScreenSetting settings)
		{
			_customScreenSetting = settings;
			renderPassEvent = _customScreenSetting.CustomScreenPassEvent;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_customScreenSetting == null || _customScreenSetting.CustomMaterialPassInfos == null || _customScreenSetting.CustomMaterialPassInfos.Count == 0)
			{
				return;
			}
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
			if (cameraData.cameraType != CameraType.Game)
			{
				return;
			}
			if (!resourceData.activeColorTexture.IsValid())
			{
				return;
			}
			// TextureDesc @0x182906540 (machine code checked): ctor(width, height) from the descriptor at UniversalCameraData+0x100, then format = descriptor.graphicsFormat, name, clearBuffer = false, filterMode = 1 (Bilinear), wrapMode = 1 (Clamp), msaaSamples = 1 (None); anisoLevel is not written
			TextureDesc desc = new TextureDesc(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height, false, false);
			desc.format = cameraData.cameraTargetDescriptor.graphicsFormat;
			desc.name = "_CustomScreenTempTexture";
			desc.clearBuffer = false;
			desc.filterMode = FilterMode.Bilinear;
			desc.wrapMode = TextureWrapMode.Clamp;
			desc.msaaSamples = MSAASamples.None;
			TextureHandle tempTexture = renderGraph.CreateTexture(desc);
			for (int i = 0; i < _customScreenSetting.CustomMaterialPassInfos.Count; i++)
			{
				MaterialPassInfo info = _customScreenSetting.CustomMaterialPassInfos[i];
				if (info != null && !(info.CustomMaterial == null))
				{
					if (!info.UsingBlitTexture)
					{
						RenderWithFullscreenMesh(renderGraph, resourceData, info.CustomMaterial, i);
					}
					else
					{
						RenderWithBlit(renderGraph, resourceData, tempTexture, info.CustomMaterial, i);
					}
				}
			}
		}

		private TextureHandle RenderWithBlit(RenderGraph renderGraph, UniversalResourceData resourceData, TextureHandle tempTexture, Material material, int passIndex)
		{
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(string.Format("{0}_Blit_{1}_ToTemp", "CustomScreenPass", passIndex), out var passData))
			{
				passData.SourceTexture = resourceData.activeColorTexture;
				passData.TempTexture = tempTexture;
				passData.Material = material;
				builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(passData.TempTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
				{
					Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), data.Material, 0);
				});
			}
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(string.Format("{0}_Blit_{1}_ToSource", "CustomScreenPass", passIndex), out var passData))
			{
				passData.SourceTexture = tempTexture;
				passData.TempTexture = resourceData.activeColorTexture;
				builder.UseTexture(passData.SourceTexture, AccessFlags.Read);
				builder.SetRenderAttachment(passData.TempTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
				{
					Blitter.BlitTexture(context.cmd, data.SourceTexture, new Vector4(1f, 1f, 0f, 0f), 0f, false);
				});
			}
			return resourceData.activeColorTexture;
		}

		private void RenderWithFullscreenMesh(RenderGraph renderGraph, UniversalResourceData resourceData, Material material, int passIndex)
		{
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(string.Format("{0}_Mesh_{1}", "CustomScreenPass", passIndex), out var passData))
			{
				passData.Material = material;
				passData.FullscreenMesh = _fullscreenTriangle;
				builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);
				builder.AllowPassCulling(false);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
				{
					if (data.FullscreenMesh != null && data.Material != null)
					{
						context.cmd.DrawMesh(data.FullscreenMesh, Matrix4x4.identity, data.Material, 0, 0);
					}
				});
			}
		}

		public void Dispose()
		{
			_tempTexture?.Release();
		}
	}

	[SerializeField]
	private CustomScreenSetting _settings = new CustomScreenSetting();

	[Tooltip("그래픽 옵션 게이트용 전역 프로퍼티명 (>=0.5 이면 비활성). 비워두면 항상 활성")]
	[SerializeField]
	private string _graphicGateProperty = "";

	private CustomScreenPass _renderPass;

	public CustomScreenSetting Setting => _settings;

	public override void Create()
	{
		if (_settings == null)
		{
			_settings = new CustomScreenSetting();
		}
		if (_renderPass == null)
		{
			_renderPass = new CustomScreenPass(_settings);
		}
	}

	private void OnValidate()
	{
		if (_renderPass != null)
		{
			_renderPass.UpdateSettings(_settings);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.cameraType != CameraType.Game)
		{
			return;
		}
		if (!string.IsNullOrEmpty(_graphicGateProperty) && Shader.GetGlobalFloat(_graphicGateProperty) >= 0.5f)
		{
			return;
		}
		if (_settings == null)
		{
			_settings = new CustomScreenSetting();
		}
		if (_renderPass == null)
		{
			_renderPass = new CustomScreenPass(_settings);
		}
		if (_renderPass != null)
		{
			_renderPass.UpdateSettings(_settings);
			renderer.EnqueuePass(_renderPass);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_renderPass != null)
		{
			_renderPass.Dispose();
		}
		_renderPass = null;
	}
}

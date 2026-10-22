using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class MapBlockRenderingFeature : ScriptableRendererFeature
{
	private class MapBlockRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			internal Material MaterialData;

			internal Mesh MeshData;
		}

		private Material _material;

		private Mesh _mesh;

		private RTHandle _customColor;

		private float _offsetBump;

		private readonly int[] _quadIndices = new int[6] { 0, 1, 2, 0, 2, 3 };

		private readonly Vector3[] _boxLocalVertices = new Vector3[24]
		{
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f)
		};

		public MapBlockRenderPass(Material material, RenderPassEvent passEvent, float offsetBump)
		{
			_material = material;
			_offsetBump = offsetBump;
			renderPassEvent = passEvent;
			requiresIntermediateTexture = true;
		}

		public void UpdatePassSetting(Material material, RenderPassEvent passEvent, float offsetBump)
		{
			_material = material;
			_offsetBump = offsetBump;
			renderPassEvent = passEvent;
		}

		public void AddBoxColliderQuadToList(BoxCollider boxCollider, List<int> indices, List<Vector3> vertices, List<Color> colors, float bump = 0f)
		{
			Transform transform = boxCollider.transform;
			Vector3 center = boxCollider.center;
			Vector3 size = boxCollider.size;
			Bounds bounds = boxCollider.bounds;
			if (!(bounds.center.y + bounds.size.y * 0.5f >= 0.125f))
			{
				return;
			}
			Vector3 scaledSize = size + Vector3.one * bump;
			for (int i = 0; i < 24; i += 4)
			{
				int count = vertices.Count;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector = _boxLocalVertices[i + j];
					Vector3 position = new Vector3(scaledSize.x * vector.x + center.x, scaledSize.y * vector.y + center.y, scaledSize.z * vector.z + center.z);
					vertices.Add(transform.TransformPoint(position));
					colors.Add(new Color(0f, 1f, 0f, 1f));
				}
				for (int k = 0; k < _quadIndices.Length; k++)
				{
					indices.Add(_quadIndices[k] + count);
				}
			}
		}

		public void AddNavMeshModifierVolumeQuadToList(NavMeshModifierVolume volume, List<int> indices, List<Vector3> vertices, List<Color> colors, float bump = 0f)
		{
			Transform transform = volume.transform;
			Vector3 center = volume.center;
			Vector3 size = volume.size;
			Vector3 scaledSize = size + Vector3.one * bump;
			for (int i = 0; i < 24; i += 4)
			{
				int count = vertices.Count;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector = _boxLocalVertices[i + j];
					Vector3 position = new Vector3(scaledSize.x * vector.x + center.x, scaledSize.y * vector.y + center.y, scaledSize.z * vector.z + center.z);
					vertices.Add(transform.TransformPoint(position));
					colors.Add(new Color(0f, 1f, 0f, 1f));
				}
				for (int k = 0; k < _quadIndices.Length; k++)
				{
					indices.Add(_quadIndices[k] + count);
				}
			}
		}

		private Mesh GenerateBlockPlaneMesh(float bump = 0f)
		{
			if (_mesh != null)
			{
				CoreUtils.Destroy(_mesh);
				_mesh = null;
			}
			List<int> indices = new List<int>();
			List<Vector3> vertices = new List<Vector3>();
			List<Color> colors = new List<Color>();
			Mesh mesh = new Mesh();
			mesh.name = "MapBlockMesh";
			Object.FindObjectsByType<BoxCollider>(FindObjectsSortMode.None).ToList().Where((BoxCollider _) => !_.isTrigger).ToList()
				.ForEach(delegate(BoxCollider _)
				{
					AddBoxColliderQuadToList(_, indices, vertices, colors, bump);
				});
			Object.FindObjectsByType<NavMeshModifierVolume>(FindObjectsSortMode.None).ToList().ForEach(delegate(NavMeshModifierVolume _)
			{
				AddNavMeshModifierVolumeQuadToList(_, indices, vertices, colors, bump);
			});
			mesh.SetVertices(vertices);
			mesh.SetTriangles(indices, 0);
			mesh.SetColors(colors);
			mesh.RecalculateNormals();
			return mesh;
		}

		public void Dispose()
		{
			if (_customColor != null)
			{
				_customColor.Release();
			}
			if (_mesh != null)
			{
				CoreUtils.Destroy(_mesh);
				_mesh = null;
			}
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_material == null)
			{
				return;
			}
			_mesh = GenerateBlockPlaneMesh(_offsetBump);
			if (_mesh == null || _mesh.vertexCount == 0)
			{
				return;
			}
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			if (universalCameraData == null)
			{
				return;
			}
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			RenderTextureDescriptor cameraTargetDescriptor = universalCameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.colorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			cameraTargetDescriptor.depthBufferBits = 0;
			TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, NAME_TEXTURE, false);
			PassData passData;
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("MapBlockRenderPass", out passData))
			{
				passData.MaterialData = _material;
				passData.MeshData = _mesh;
				builder.SetRenderAttachment(textureHandle, 0, AccessFlags.Write);
				if (universalResourceData.activeDepthTexture.IsValid())
				{
					builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.Read);
				}
				builder.SetRenderFunc<PassData>(static (PassData data, RasterGraphContext context) =>
				{
					context.cmd.ClearRenderTarget(false, true, new Color(1f, 0f, 0f, 0f));
					if (data.MeshData != null)
					{
						context.cmd.DrawMesh(data.MeshData, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
				});
				builder.SetGlobalTextureAfterPass(in textureHandle, Shader.PropertyToID(NAME_TEXTURE));
			}
		}
	}

	public static readonly string NAME_TEXTURE = "_MapBlockMeshTexture";

	public Material Material;

	public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;

	public float OffsetBump;

	private MapBlockRenderPass _pass;

	public override void Create()
	{
		if (_pass == null)
		{
			_pass = new MapBlockRenderPass(Material, RenderPassEvent, OffsetBump);
		}
	}

	private void OnValidate()
	{
		if (_pass != null)
		{
			_pass.UpdatePassSetting(Material, RenderPassEvent, OffsetBump);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (_pass == null)
		{
			_pass = new MapBlockRenderPass(Material, RenderPassEvent, OffsetBump);
		}
		_pass.UpdatePassSetting(Material, RenderPassEvent, OffsetBump);
		renderer.EnqueuePass(_pass);
	}

	protected override void Dispose(bool disposing)
	{
		if (_pass != null)
		{
			_pass.Dispose();
		}
		_pass = null;
	}
}

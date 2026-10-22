using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class NavMeshRendererFeature : ScriptableRendererFeature
{
	private class NavMeshRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			internal Material MaterialData;

			internal Mesh NevMesh;

			internal Mesh NavMeshLU;

			internal Mesh NavMeshRU;

			internal Mesh NavMeshLD;

			internal Mesh NavMeshRD;
		}

		private Material _material;

		private Mesh _navMesh;

		private Mesh _navMeshLU;

		private Mesh _navMeshRU;

		private Mesh _navMeshLD;

		private Mesh _navMeshRD;

		private RTHandle _customColor;

		private float _offsetBump;

		public NavMeshRenderPass(Material material, RenderPassEvent passEvent, float offsetBump)
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

		private Mesh GenerateNavMesh(float offsetX = 0f, float offsetZ = 0f)
		{
			NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
			Vector3[] vertices = navMeshTriangulation.vertices;
			int[] indices = navMeshTriangulation.indices;
			int[] areas = navMeshTriangulation.areas;
			int areaFromName = NavMesh.GetAreaFromName("Walkable");
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			List<Color> list3 = new List<Color>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				int num = indices[i];
				int num2 = indices[i + 1];
				int num3 = indices[i + 2];
				Vector3 vector = vertices[num];
				Vector3 vector2 = vertices[num2];
				Vector3 vector3 = vertices[num3];
				Color item = ((areas[i / 3] != areaFromName) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f));
				int count = list.Count;
				list.Add(new Vector3(vector.x + offsetX, vector.y, vector.z + offsetZ));
				list.Add(new Vector3(vector2.x + offsetX, vector2.y, vector2.z + offsetZ));
				list.Add(new Vector3(vector3.x + offsetX, vector3.y, vector3.z + offsetZ));
				list3.Add(item);
				list3.Add(item);
				list3.Add(item);
				list2.Add(count);
				list2.Add(count + 1);
				list2.Add(count + 2);
			}
			if (list.Count == 0)
			{
				return null;
			}
			Mesh mesh = new Mesh();
			mesh.name = "ColoredNavMeshDebug";
			mesh.SetVertices(list);
			mesh.SetTriangles(list2, 0);
			mesh.SetColors(list3);
			mesh.RecalculateNormals();
			return mesh;
		}

		private void DestroyMesh(ref Mesh mesh)
		{
			if (mesh != null)
			{
				CoreUtils.Destroy(mesh);
				mesh = null;
			}
		}

		private void RegenerateNavMeshes()
		{
			DestroyMesh(ref _navMesh);
			DestroyMesh(ref _navMeshLU);
			DestroyMesh(ref _navMeshRU);
			DestroyMesh(ref _navMeshLD);
			DestroyMesh(ref _navMeshRD);
			_navMesh = GenerateNavMesh();
		}

		public void Dispose()
		{
			if (_customColor != null)
			{
				_customColor.Release();
			}
			DestroyMesh(ref _navMesh);
			DestroyMesh(ref _navMeshLU);
			DestroyMesh(ref _navMeshRU);
			DestroyMesh(ref _navMeshLD);
			DestroyMesh(ref _navMeshRD);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			if (_material == null)
			{
				return;
			}
			DestroyMesh(ref _navMesh);
			DestroyMesh(ref _navMeshLU);
			DestroyMesh(ref _navMeshRU);
			DestroyMesh(ref _navMeshLD);
			DestroyMesh(ref _navMeshRD);
			_navMesh = GenerateNavMesh();
			if (_navMesh == null && _navMeshLU == null && _navMeshRU == null && _navMeshLD == null && _navMeshRD == null)
			{
				return;
			}
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			RenderTextureDescriptor cameraTargetDescriptor = universalCameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.colorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			cameraTargetDescriptor.depthBufferBits = 0;
			TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, NAME_TEXTURE, false);
			PassData passData;
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("NavMeshRenderPass", out passData))
			{
				passData.MaterialData = _material;
				passData.NevMesh = _navMesh;
				passData.NavMeshLU = _navMeshLU;
				passData.NavMeshRU = _navMeshRU;
				passData.NavMeshLD = _navMeshLD;
				passData.NavMeshRD = _navMeshRD;
				builder.SetRenderAttachment(textureHandle, 0, AccessFlags.Write);
				if (universalResourceData.activeDepthTexture.IsValid())
				{
					builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.Read);
				}
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<PassData>(static (PassData data, RasterGraphContext context) =>
				{
					context.cmd.ClearRenderTarget(false, true, Color.clear);
					if (data.NevMesh != null)
					{
						context.cmd.DrawMesh(data.NevMesh, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
					if (data.NavMeshLU != null)
					{
						context.cmd.DrawMesh(data.NavMeshLU, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
					if (data.NavMeshRU != null)
					{
						context.cmd.DrawMesh(data.NavMeshRU, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
					if (data.NavMeshLD != null)
					{
						context.cmd.DrawMesh(data.NavMeshLD, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
					if (data.NavMeshRD != null)
					{
						context.cmd.DrawMesh(data.NavMeshRD, Matrix4x4.identity, data.MaterialData, 0, 0);
					}
				});
				builder.SetGlobalTextureAfterPass(in textureHandle, Shader.PropertyToID(NAME_TEXTURE));
			}
		}
	}

	public static readonly string NAME_TEXTURE = "_NavMeshTexture";

	public Material Material;

	public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;

	public float OffsetBump;

	private NavMeshRenderPass _pass;

	public override void Create()
	{
		if (_pass == null)
		{
			_pass = new NavMeshRenderPass(Material, RenderPassEvent, OffsetBump);
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
			_pass = new NavMeshRenderPass(Material, RenderPassEvent, OffsetBump);
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

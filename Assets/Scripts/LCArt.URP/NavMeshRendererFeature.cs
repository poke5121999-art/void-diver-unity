using UnityEngine;
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
		}

		public void UpdatePassSetting(Material material, RenderPassEvent passEvent, float offsetBump)
		{
		}

		private Mesh GenerateNavMesh(float offsetX = 0f, float offsetZ = 0f)
		{
			return null;
		}

		private void DestroyMesh(ref Mesh mesh)
		{
		}

		private void RegenerateNavMeshes()
		{
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}
	}

	public static readonly string NAME_TEXTURE;

	public Material Material;

	public RenderPassEvent RenderPassEvent;

	public float OffsetBump;

	private NavMeshRenderPass _pass;

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

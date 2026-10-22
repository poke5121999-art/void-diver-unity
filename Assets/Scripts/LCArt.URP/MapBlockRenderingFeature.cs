using System.Collections.Generic;
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

		private readonly int[] _quadIndices;

		private readonly Vector3[] _boxLocalVertices;

		public MapBlockRenderPass(Material material, RenderPassEvent passEvent, float offsetBump)
		{
		}

		public void UpdatePassSetting(Material material, RenderPassEvent passEvent, float offsetBump)
		{
		}

		public void AddBoxColliderQuadToList(BoxCollider boxCollider, List<int> indices, List<Vector3> vertices, List<Color> colors, float bump = 0f)
		{
		}

		public void AddNavMeshModifierVolumeQuadToList(NavMeshModifierVolume volume, List<int> indices, List<Vector3> vertices, List<Color> colors, float bump = 0f)
		{
		}

		private Mesh GenerateBlockPlaneMesh(float bump = 0f)
		{
			return null;
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

	private MapBlockRenderPass _pass;

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

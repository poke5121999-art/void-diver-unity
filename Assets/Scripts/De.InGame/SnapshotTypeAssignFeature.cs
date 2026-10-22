using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class SnapshotTypeAssignFeature : ScriptableRendererFeature
{
	[Serializable]
	public class SnapshotTypeKeyword
	{
		public string Label;

		public int SnapshotType;

		public string[] Keywords;
	}

	private class SnapshotRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			public List<(Renderer renderer, int type)> RenderItems;

			public Material[] TypeMaterials;
		}

		private readonly SnapshotTypeAssignFeature _feature;

		private readonly ProfilingSampler _profilingSampler;

		public SnapshotRenderPass(SnapshotTypeAssignFeature feature)
		{
		}

		private List<(Renderer, int)> CollectRenderItems()
		{
			return null;
		}

		private static void DrawItems(CommandBuffer cmd, List<(Renderer renderer, int type)> items, Material[] typeMaterials)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}
	}

	private const int MAX_SNAPSHOT_TYPES = 6;

	[Header("스냅샷 쉐이더")]
	public Shader SnapshotShader;

	[Header("렌더 패스 이벤트")]
	public RenderPassEvent PassEvent;

	[Header("하이어라키 이름 기반 _SnapshotType 키워드 매핑")]
	public SnapshotTypeKeyword[] TypeKeywords;

	private static readonly int _snapshotTypeId;

	private Material[] _typeMaterials;

	private SnapshotRenderPass _pass;

	public override void Create()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	internal int ResolveTypeFromHierarchy(Transform current)
	{
		return 0;
	}

	private int ResolveSnapshotType(string folderName)
	{
		return 0;
	}
}

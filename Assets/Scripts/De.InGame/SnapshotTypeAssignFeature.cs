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

		private readonly ProfilingSampler _profilingSampler = new ProfilingSampler("SnapshotTypeRender");

		public SnapshotRenderPass(SnapshotTypeAssignFeature feature)
		{
			_feature = feature;
			base.renderPassEvent = feature.PassEvent;
		}

		private List<(Renderer, int)> CollectRenderItems()
		{
			Renderer[] array = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
			List<(Renderer, int)> list = new List<(Renderer, int)>(array.Length);
			Renderer[] array2 = array;
			foreach (Renderer renderer in array2)
			{
				if (renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					int item = _feature.ResolveTypeFromHierarchy(renderer.transform);
					list.Add((renderer, item));
				}
			}
			return list;
		}

		private static void DrawItems(CommandBuffer cmd, List<(Renderer renderer, int type)> items, Material[] typeMaterials)
		{
			foreach (var (renderer, num) in items)
			{
				if (num < 0 || num >= typeMaterials.Length)
				{
					continue;
				}
				Material material = typeMaterials[num];
				int num2 = renderer.sharedMaterials.Length;
				for (int i = 0; i < num2; i++)
				{
					cmd.DrawRenderer(renderer, material, i);
				}
			}
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			PassData passData;
			using (IUnsafeRenderGraphBuilder builder = renderGraph.AddUnsafePass<PassData>(_profilingSampler.name, out passData, _profilingSampler))
			{
				passData.RenderItems = CollectRenderItems();
				passData.TypeMaterials = _feature._typeMaterials;
				builder.UseTexture(universalResourceData.activeColorTexture, AccessFlags.Write);
				builder.UseTexture(universalResourceData.activeDepthTexture, AccessFlags.ReadWrite);
				builder.AllowPassCulling(value: false);
				builder.SetRenderFunc<PassData>(static (PassData data, UnsafeGraphContext context) =>
				{
					DrawItems(CommandBufferHelpers.GetNativeCommandBuffer(context.cmd), data.RenderItems, data.TypeMaterials);
				});
			}
		}
	}

	private const int MAX_SNAPSHOT_TYPES = 6;

	[Header("스냅샷 쉐이더")]
	public Shader SnapshotShader;

	[Header("렌더 패스 이벤트")]
	public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingOpaques;

	[Header("하이어라키 이름 기반 _SnapshotType 키워드 매핑")]
	public SnapshotTypeKeyword[] TypeKeywords = new SnapshotTypeKeyword[5]
	{
		new SnapshotTypeKeyword
		{
			Label = "Wall",
			SnapshotType = 1,
			Keywords = new string[1] { "wall" }
		},
		new SnapshotTypeKeyword
		{
			Label = "Roof",
			SnapshotType = 2,
			Keywords = new string[1] { "roof" }
		},
		new SnapshotTypeKeyword
		{
			Label = "Prop",
			SnapshotType = 3,
			Keywords = new string[5] { "prop", "light", "moving", "interact", "building" }
		},
		new SnapshotTypeKeyword
		{
			Label = "Plant",
			SnapshotType = 4,
			Keywords = new string[1] { "plant" }
		},
		new SnapshotTypeKeyword
		{
			Label = "Decal",
			SnapshotType = 5,
			Keywords = new string[1] { "decal" }
		}
	};

	private static readonly int _snapshotTypeId = Shader.PropertyToID("_SnapshotType");

	private Material[] _typeMaterials;

	private SnapshotRenderPass _pass;

	public override void Create()
	{
		if (!(SnapshotShader == null))
		{
			_typeMaterials = new Material[6];
			for (int i = 0; i < 6; i++)
			{
				_typeMaterials[i] = CoreUtils.CreateEngineMaterial(SnapshotShader);
				_typeMaterials[i].SetInt(_snapshotTypeId, i);
			}
			_pass = new SnapshotRenderPass(this);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (_pass != null && _typeMaterials != null && !(SnapshotShader == null))
		{
			renderer.EnqueuePass(_pass);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_typeMaterials != null)
		{
			for (int i = 0; i < _typeMaterials.Length; i++)
			{
				CoreUtils.Destroy(_typeMaterials[i]);
			}
			_typeMaterials = null;
		}
	}

	internal int ResolveTypeFromHierarchy(Transform current)
	{
		Transform transform = current;
		while (true)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				return 0;
			}
			string text = parent.name;
			if (text.Equals("StaticObject"))
			{
				return 0;
			}
			if ((bool)current.GetComponent<BackgroundPlantFlag>())
			{
				return 4;
			}
			if (text.Equals("ActiveObject"))
			{
				break;
			}
			transform = parent;
		}
		return ResolveSnapshotType(transform.name);
	}

	private int ResolveSnapshotType(string folderName)
	{
		if (string.IsNullOrEmpty(folderName))
		{
			return 0;
		}
		string text = folderName.ToLowerInvariant();
		SnapshotTypeKeyword[] typeKeywords = TypeKeywords;
		foreach (SnapshotTypeKeyword snapshotTypeKeyword in typeKeywords)
		{
			if (snapshotTypeKeyword.Keywords == null)
			{
				continue;
			}
			string[] keywords = snapshotTypeKeyword.Keywords;
			foreach (string text2 in keywords)
			{
				if (!string.IsNullOrEmpty(text2) && text.Contains(text2.ToLowerInvariant()))
				{
					return snapshotTypeKeyword.SnapshotType;
				}
			}
		}
		return 0;
	}
}

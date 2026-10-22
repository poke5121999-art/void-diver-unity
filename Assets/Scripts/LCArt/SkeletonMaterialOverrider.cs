using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;

public class SkeletonMaterialOverrider : MonoBehaviour
{
	[Header("Skeleton Material Overrider Settings")]
	[SerializeField]
	private List<SkeletonRenderer> _skeletonRenderers = new List<SkeletonRenderer>();

	[Header("Skeleton Material Debug")]
	[SerializeField]
	private List<Material> _instancedMaterials = new List<Material>();

	private List<Material> _skeletonOriginalMaterials = new List<Material>();

	private Dictionary<Material, Material> _overrideMaterials = new Dictionary<Material, Material>();

	public List<Material> InstnancedMaterials => _instancedMaterials;

	private void Awake()
	{
		if (_skeletonRenderers == null || _skeletonRenderers.Count == 0)
		{
			_skeletonRenderers?.Clear();
			_skeletonRenderers = GetComponentsInChildren<SkeletonRenderer>(true).ToList();
		}
		_skeletonOriginalMaterials?.Clear();
		_overrideMaterials?.Clear();
		foreach (SkeletonRenderer skeletonRenderer in _skeletonRenderers)
		{
			if (skeletonRenderer == null)
			{
				continue;
			}
			SkeletonDataAsset dataAsset = skeletonRenderer.skeletonDataAsset;
			if (dataAsset == null)
			{
				continue;
			}
			if (dataAsset.atlasAssets != null)
			{
				AtlasAssetBase[] atlasAssets = dataAsset.atlasAssets;
				foreach (AtlasAssetBase atlasAsset in atlasAssets)
				{
					if (atlasAsset == null || atlasAsset.Materials == null)
					{
						continue;
					}
					foreach (Material material in atlasAsset.Materials)
					{
						if (material != null && !_skeletonOriginalMaterials.Contains(material))
						{
							_skeletonOriginalMaterials.Add(material);
						}
					}
				}
			}
			BlendModeMaterials blendModeMaterials = dataAsset.blendModeMaterials;
			if (blendModeMaterials == null)
			{
				continue;
			}
			if (blendModeMaterials.additiveMaterials != null)
			{
				AddReplacementMaterials(blendModeMaterials.additiveMaterials);
			}
			if (blendModeMaterials.multiplyMaterials != null)
			{
				AddReplacementMaterials(blendModeMaterials.multiplyMaterials);
			}
			if (blendModeMaterials.screenMaterials != null)
			{
				AddReplacementMaterials(blendModeMaterials.screenMaterials);
			}
		}
		foreach (Material original in _skeletonOriginalMaterials)
		{
			if (!(original == null))
			{
				_overrideMaterials[original] = new Material(original);
				_instancedMaterials.Add(_overrideMaterials[original]);
			}
		}
	}

	private void AddReplacementMaterials(List<BlendModeMaterials.ReplacementMaterial> replacements)
	{
		foreach (BlendModeMaterials.ReplacementMaterial replacement in replacements)
		{
			if (replacement.material != null && !_skeletonOriginalMaterials.Contains(replacement.material))
			{
				_skeletonOriginalMaterials.Add(replacement.material);
			}
		}
	}

	private void Start()
	{
		foreach (SkeletonRenderer skeletonRenderer in _skeletonRenderers.Where((SkeletonRenderer r) => r != null).ToList())
		{
			SkeletonRenderer renderer = skeletonRenderer;
			UpdateOverrideMaterials(ref renderer);
		}
	}

	private void OnDestroy()
	{
		if (_instancedMaterials != null)
		{
			foreach (Material material in _instancedMaterials.Where((Material m) => m != null).ToList())
			{
				Object.Destroy(material);
			}
			_overrideMaterials?.Clear();
			_instancedMaterials.Clear();
			_skeletonOriginalMaterials?.Clear();
			_overrideMaterials = null;
			_instancedMaterials = null;
			_skeletonOriginalMaterials = null;
		}
	}

	private void UpdateOverrideMaterials(ref SkeletonRenderer renderer)
	{
		if (renderer == null || _overrideMaterials == null || renderer.CustomMaterialOverride == null)
		{
			return;
		}
		foreach (KeyValuePair<Material, Material> pair in _overrideMaterials)
		{
			if (!(pair.Key == null) && !(pair.Value == null))
			{
				renderer.CustomMaterialOverride[pair.Key] = pair.Value;
			}
		}
	}

	[ContextMenu("Assign Skeleton Renderer Components")]
	private void GetRendererComponents()
	{
		_skeletonRenderers?.Clear();
		_skeletonRenderers = GetComponentsInChildren<SkeletonRenderer>(true).ToList();
	}
}

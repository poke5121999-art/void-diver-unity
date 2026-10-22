using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SkeletonMaterialOverrider : MonoBehaviour
{
	[Header("Skeleton Material Overrider Settings")]
	[SerializeField]
	private List<SkeletonRenderer> _skeletonRenderers;

	[Header("Skeleton Material Debug")]
	[SerializeField]
	private List<Material> _instancedMaterials;

	private List<Material> _skeletonOriginalMaterials;

	private Dictionary<Material, Material> _overrideMaterials;

	public List<Material> InstnancedMaterials => null;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateOverrideMaterials(ref SkeletonRenderer renderer)
	{
	}

	[ContextMenu("Assign Skeleton Renderer Components")]
	private void GetRendererComponents()
	{
	}
}

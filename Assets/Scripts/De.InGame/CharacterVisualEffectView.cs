using System.Linq;
using UnityEngine;

public class CharacterVisualEffectView : UnitVisualEffectView
{
	protected override void Initialize()
	{
		base.Initialize();
		foreach (Material material in SkeletonMaterialOverrider.InstnancedMaterials.Where((Material _) => _ != null))
		{
			material.SetFloat(IsEnemyProperty, 0f);
		}
	}
}

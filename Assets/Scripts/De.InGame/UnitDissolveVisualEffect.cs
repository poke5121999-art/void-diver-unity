using UnityEngine;

public class UnitDissolveVisualEffect : MonoBehaviour
{
	[SerializeField]
	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	[SerializeField]
	[Range(0f, 1f)]
	private float _dissolveIntensity;

	private float _cachedDissolveIntensity;

	private readonly int _dissolvePropertyID = Shader.PropertyToID("_DissolveProgress");

	public float DissolveIntensity
	{
		get
		{
			return _dissolveIntensity;
		}
		set
		{
			_dissolveIntensity = value;
			if (_skeletonMaterialOverrider != null && _skeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material material in _skeletonMaterialOverrider.InstnancedMaterials)
				{
					if (material != null)
					{
						material.SetFloat(_dissolvePropertyID, _dissolveIntensity);
					}
				}
			}
			_cachedDissolveIntensity = _dissolveIntensity;
		}
	}
}

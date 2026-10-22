using UnityEngine;

public class UnitDissolveVisualEffect : MonoBehaviour
{
	[SerializeField]
	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	[SerializeField]
	[Range(0f, 1f)]
	private float _dissolveIntensity;

	private float _cachedDissolveIntensity;

	private readonly int _dissolvePropertyID;

	public float DissolveIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}
}

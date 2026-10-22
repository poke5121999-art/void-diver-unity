using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinimapSightMaskApplier : MonoBehaviour
{
	[FormerlySerializedAs("_minimapSightSettings")]
	[SerializeField]
	private MinimapVisualSettings _minimapVisualSettings;

	private Image[] _mapImages;

	private readonly int _pivotWorldPositionProperty;

	private readonly int _minimapSightTextureProperty;

	private readonly int _noiseTexProperty;

	private readonly int _sightNoiseScaleProperty;

	private readonly int _sightNoiseSpeedProperty;

	private readonly int _sightNoiseStretchProperty;

	private readonly int _sightDissolveOffsetProperty;

	public Image[] MapImages => null;

	private void OnDestroy()
	{
	}

	public void Initialize(Vector3[] sectorOffsets)
	{
	}

	private void LateUpdate()
	{
	}

	private void ApplySightMaterialParams(Material mat)
	{
	}

	private void InitializeMinimapMaterials(Vector3[] sectorOffsets)
	{
	}
}

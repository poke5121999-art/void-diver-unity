using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinimapSightMaskApplier : MonoBehaviour
{
	[FormerlySerializedAs("_minimapSightSettings")]
	[SerializeField]
	private MinimapVisualSettings _minimapVisualSettings;

	private Image[] _mapImages;

	private readonly int _pivotWorldPositionProperty = Shader.PropertyToID("_PivotWorldPosition");

	private readonly int _minimapSightTextureProperty = Shader.PropertyToID("_MinimapSightTexture");

	private readonly int _noiseTexProperty = Shader.PropertyToID("_NoiseTex");

	private readonly int _sightNoiseScaleProperty = Shader.PropertyToID("_NoiseScale");

	private readonly int _sightNoiseSpeedProperty = Shader.PropertyToID("_NoiseSpeed");

	private readonly int _sightNoiseStretchProperty = Shader.PropertyToID("_NoiseStretch");

	private readonly int _sightDissolveOffsetProperty = Shader.PropertyToID("_DissolveOffset");

	public Image[] MapImages => _mapImages;

	private void OnDestroy()
	{
		if (_mapImages == null)
		{
			return;
		}
		for (int i = 0; i < _mapImages.Length; i++)
		{
			if (_mapImages[i] != null && _mapImages[i].material != null)
			{
				Object.Destroy(_mapImages[i].material);
			}
		}
	}

	public void Initialize(Vector3[] sectorOffsets)
	{
		InitializeMinimapMaterials(sectorOffsets);
	}

	private void LateUpdate()
	{
		if (_mapImages == null)
		{
			return;
		}
		Texture globalTexture = Shader.GetGlobalTexture(_minimapSightTextureProperty);
		for (int i = 0; i < _mapImages.Length; i++)
		{
			Material material = ((_mapImages[i] != null) ? _mapImages[i].material : null);
			if (material != null && globalTexture != null)
			{
				material.SetTexture(_minimapSightTextureProperty, globalTexture);
			}
		}
	}

	private void ApplySightMaterialParams(Material mat)
	{
		if (!(mat == null) && !(_minimapVisualSettings == null))
		{
			if (_minimapVisualSettings.SightNoiseTex != null)
			{
				mat.SetTexture(_noiseTexProperty, _minimapVisualSettings.SightNoiseTex);
			}
			mat.SetFloat(_sightNoiseScaleProperty, _minimapVisualSettings.SightNoiseScale);
			mat.SetFloat(_sightNoiseSpeedProperty, _minimapVisualSettings.SightNoiseSpeed);
			mat.SetVector(_sightNoiseStretchProperty, _minimapVisualSettings.SightNoiseStretch);
			mat.SetFloat(_sightDissolveOffsetProperty, _minimapVisualSettings.SightDissolveOffset);
		}
	}

	private void InitializeMinimapMaterials(Vector3[] sectorOffsets)
	{
		if (base.gameObject.transform.childCount == 0 || _minimapVisualSettings == null)
		{
			return;
		}
		if (_mapImages != null)
		{
			for (int i = 0; i < _mapImages.Length; i++)
			{
				if (_mapImages[i] != null && _mapImages[i].material != null)
				{
					Object.Destroy(_mapImages[i].material);
				}
			}
		}
		_mapImages = new Image[base.gameObject.transform.childCount];
		for (int j = 0; j < base.gameObject.transform.childCount; j++)
		{
			_mapImages[j] = base.gameObject.transform.GetChild(j).GetComponent<Image>();
			if (!(_mapImages[j] == null))
			{
				Material material = new Material(_minimapVisualSettings.MaskingMaterial);
				_mapImages[j].material = material;
				ApplySightMaterialParams(material);
				if (sectorOffsets != null && j < sectorOffsets.Length)
				{
					material.SetVector(_pivotWorldPositionProperty, new Vector4(sectorOffsets[j].x, sectorOffsets[j].y, sectorOffsets[j].z, 0f));
				}
			}
		}
	}
}

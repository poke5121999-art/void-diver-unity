using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GammaSliderPreviewApplier : MonoBehaviour
{
	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private List<Image> _gammaImages = new List<Image>();

	private const float _sliderMin = -50f;

	private const float _sliderMax = 50f;

	private const float _gammaExponentMin = 0.3f;

	private const float _gammaExponentMax = 1.7f;

	private readonly int _gammaDegreeId = Shader.PropertyToID("_GammaDegree");

	private readonly List<Material> _runtimeMaterials = new List<Material>();

	private void OnEnable()
	{
		if (!(_slider == null))
		{
			BuildRuntimeMaterials();
			_slider.onValueChanged.RemoveListener(Apply);
			_slider.onValueChanged.AddListener(Apply);
			Apply(_slider.value);
		}
	}

	private void OnDisable()
	{
		if (_slider != null)
		{
			_slider.onValueChanged.RemoveListener(Apply);
		}
	}

	private void OnDestroy()
	{
		foreach (Material runtimeMaterial in _runtimeMaterials)
		{
			if (runtimeMaterial != null)
			{
				Object.Destroy(runtimeMaterial);
			}
		}
		_runtimeMaterials.Clear();
	}

	public void Apply(float sliderValue)
	{
		float num = Mathf.Clamp01(Mathf.Clamp01((sliderValue - -50f) / 100f));
		foreach (Material runtimeMaterial in _runtimeMaterials)
		{
			if (runtimeMaterial != null)
			{
				runtimeMaterial.SetFloat(_gammaDegreeId, num * 1.4000001f + 0.3f);
			}
		}
	}

	private void BuildRuntimeMaterials()
	{
		if (_runtimeMaterials.Count > 0)
		{
			return;
		}
		foreach (Image gammaImage in _gammaImages)
		{
			if (!(gammaImage == null) && !(gammaImage.material == null))
			{
				Material material = new Material(gammaImage.material);
				gammaImage.material = material;
				_runtimeMaterials.Add(material);
			}
		}
	}
}

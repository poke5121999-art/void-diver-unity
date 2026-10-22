using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GammaSliderPreviewApplier : MonoBehaviour
{
	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private List<Image> _gammaImages;

	private const float _sliderMin = -50f;

	private const float _sliderMax = 50f;

	private const float _gammaExponentMin = 0.3f;

	private const float _gammaExponentMax = 1.7f;

	private readonly int _gammaDegreeId;

	private readonly List<Material> _runtimeMaterials;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public void Apply(float sliderValue)
	{
	}

	private void BuildRuntimeMaterials()
	{
	}
}

using GameCommon.Enum;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(Animator))]
public class SliderController : MonoBehaviour
{
	private Animator _animator;

	private Slider _slider;

	private bool _isPointerOver;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void SetLayout(ESliderLayout layout)
	{
	}
}

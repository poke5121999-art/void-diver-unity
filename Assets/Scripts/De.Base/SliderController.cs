using UnityEngine.EventSystems;
using UniRx.Triggers;
using UniRx;
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
		_animator = GetComponent<Animator>();
		_slider = GetComponent<Slider>();
	}

	private void OnEnable()
	{
		SetLayout((!_slider.interactable) ? ESliderLayout.Disabled : ESliderLayout.Normal);
	}

	private void Start()
	{
		_slider.OnPointerEnterAsObservable().Subscribe(delegate
		{
			_isPointerOver = true;
			SetLayout(ESliderLayout.Focused);
		}).AddTo(_slider);
		_slider.OnPointerExitAsObservable().Subscribe(delegate
		{
			_isPointerOver = false;
			SetLayout(ESliderLayout.Normal);
		}).AddTo(_slider);
		_slider.OnPointerDownAsObservable().Subscribe(delegate
		{
			SetLayout(ESliderLayout.Pressed);
		}).AddTo(_slider);
		_slider.OnPointerUpAsObservable().Subscribe(delegate
		{
			SetLayout((!_slider.interactable) ? ESliderLayout.Disabled : (_isPointerOver ? ESliderLayout.Focused : ESliderLayout.Normal));
		}).AddTo(_slider);
		_slider.OnDisableAsObservable().Subscribe(delegate
		{
			SetLayout(ESliderLayout.Disabled);
		}).AddTo(_slider);
		_slider.OnEnableAsObservable().Subscribe(delegate
		{
			SetLayout(ESliderLayout.Normal);
		}).AddTo(_slider);
		_slider.OnSelectAsObservable().Subscribe(delegate
		{
			SetLayout(ESliderLayout.Focused);
		}).AddTo(_slider);
		_slider.OnDeselectAsObservable().Subscribe(delegate
		{
			SetLayout(_isPointerOver ? ESliderLayout.Focused : ESliderLayout.Normal);
		}).AddTo(_slider);
		if (EventSystem.current?.currentSelectedGameObject == base.gameObject)
		{
			SetLayout(ESliderLayout.Focused);
		}
	}

	private void SetLayout(ESliderLayout layout)
	{
		_animator?.SetInteger("Layout", (int)layout);
	}
}

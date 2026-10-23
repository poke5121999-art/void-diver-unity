using UnityEngine.EventSystems;
using UniRx.Triggers;
using UniRx;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Animator))]
public class ToggleController : MonoBehaviour
{
	private Animator _animator;

	private Toggle _toggle;

	private bool _pendingIsOn;

	private bool _hasPendingIsOn;

	private string FieldName => "Layout";

	private void OnEnable()
	{
		SetLayout(_toggle.isOn ? EToggleLayout.On : EToggleLayout.Off);
	}

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_toggle = GetComponent<Toggle>();
		if (_hasPendingIsOn)
		{
			_hasPendingIsOn = false;
			_toggle.SetIsOnWithoutNotify(_pendingIsOn);
		}
	}

	private void Start()
	{
		_toggle.OnPointerEnterAsObservable().Where((PointerEventData _) => !_toggle.isOn).Subscribe(delegate
		{
			SetLayout(EToggleLayout.Focused);
		}).AddTo(_toggle);
		_toggle.OnPointerExitAsObservable().Where((PointerEventData _) => !_toggle.isOn).Subscribe(delegate
		{
			SetLayout(EToggleLayout.Off);
		}).AddTo(_toggle);
		_toggle.OnPointerDownAsObservable().Subscribe(delegate
		{
			SetLayout(EToggleLayout.Pressed);
		}).AddTo(_toggle);
		_toggle.OnValueChangedAsObservable().Subscribe(delegate(bool isOn)
		{
			SetLayout(isOn ? EToggleLayout.On : EToggleLayout.Off);
		}).AddTo(_toggle);
		_toggle.OnSelectAsObservable().Subscribe(delegate
		{
			SetLayout(EToggleLayout.Focused);
		}).AddTo(_toggle);
		_toggle.OnDeselectAsObservable().Subscribe(delegate
		{
			SetLayout(_toggle.isOn ? EToggleLayout.On : EToggleLayout.Off);
		}).AddTo(_toggle);
		if (EventSystem.current?.currentSelectedGameObject == base.gameObject)
		{
			SetLayout(EToggleLayout.Focused);
		}
	}

	private void SetLayout(EToggleLayout layout)
	{
		_animator?.SetInteger("Layout", (int)layout);
	}

	public void SetIsOnWithoutNotify(bool value)
	{
		if (_toggle == null)
		{
			_pendingIsOn = value;
			_hasPendingIsOn = true;
		}
		_toggle.SetIsOnWithoutNotify(value);
		_animator?.SetInteger("Layout", value ? 1 : 0);
	}
}

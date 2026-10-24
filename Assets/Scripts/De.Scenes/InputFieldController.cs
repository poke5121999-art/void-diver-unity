using UniRx.Triggers;
using UniRx;
using NemoLib.Popup;
using GameCommon.Table;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using TMPro;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(TMP_InputField))]
[RequireComponent(typeof(Animator))]
public class InputFieldController : MonoBase
{
	[SerializeField]
	private int _charLimit = 10;

	[SerializeField]
	private string _animatorParameter = "Layout";

	[Inject]
	private InputManager _inputManager;

	[Inject]
	private ToastManager _toastManager;

	private TMP_InputField _inputField;

	private Animator _animator;

	private string _description;

	private bool _isFocused;

	private bool _isShowingKeyboard;

	private EButtonState _currentState;

	private EButtonState _baseState;

	private bool _isPointerOver;

	protected override void Awake()
	{
		base.Awake();
		_inputField = GetComponent<TMP_InputField>();
		_animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		_currentState = (EButtonState)(-1);
		_isPointerOver = false;
		RefreshState();
	}

	private void Start()
	{
		_inputManager.RxControlDevice.Subscribe(OnControlDeviceChanged).AddTo(this);
		_inputField.OnSubmitWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSubmit();
		}).AddTo(this);
		_inputField.onSelect.AsObservable().Subscribe(delegate
		{
			OnSelected();
		}).AddTo(this);
		_inputField.onDeselect.AsObservable().Subscribe(delegate
		{
			OnDeselected();
		}).AddTo(this);
		_inputField.OnPointerEnterAsObservable().Subscribe(delegate
		{
			OnPointerEnter();
		}).AddTo(this);
		_inputField.OnPointerExitAsObservable().Subscribe(delegate
		{
			OnPointerExit();
		}).AddTo(this);
		_inputField.OnPointerDownAsObservable().Subscribe(delegate
		{
			SetBaseState(EButtonState.Pressed);
		}).AddTo(this);
		_inputField.OnPointerUpAsObservable().Subscribe(delegate
		{
			OnPointerUp();
		}).AddTo(this);
	}

	public void SetDescription(string description)
	{
		_description = description;
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
		if (device == EControlDevice.GamePad)
		{
			if (_isFocused)
			{
				_inputManager.UnblockInput();
				_isFocused = false;
			}
			bool flag = _inputManager.GetSelectedObject() == base.gameObject;
			_inputField.shouldActivateOnSelect = false;
			_inputField.DeactivateInputField();
			if (flag)
			{
				_inputManager.SelectObject(base.gameObject);
			}
		}
		else
		{
			_inputField.shouldActivateOnSelect = true;
			_inputField.ActivateInputField();
		}
	}

	private void OnSelected()
	{
		SetBaseState(EButtonState.Focused);
		if (_inputManager.RxControlDevice.Value != EControlDevice.GamePad)
		{
			_inputManager.BlockInput();
			_isFocused = true;
		}
	}

	private void OnDeselected()
	{
		SetBaseState(_isPointerOver ? EButtonState.Focused : EButtonState.Normal);
		if (_isFocused)
		{
			_inputManager.UnblockInput();
			_isFocused = false;
		}
	}

	private void OnPointerEnter()
	{
		if (_inputManager.RxControlDevice.Value != EControlDevice.GamePad)
		{
			_isPointerOver = true;
			SetBaseState(EButtonState.Focused);
		}
	}

	private void OnPointerExit()
	{
		if (_inputManager.RxControlDevice.Value != EControlDevice.GamePad)
		{
			_isPointerOver = false;
			SetBaseState(EButtonState.Normal);
		}
	}

	private void OnPointerUp()
	{
		EButtonState state = ((!_inputField.interactable) ? EButtonState.Disabled : (_isPointerOver ? EButtonState.Focused : EButtonState.Normal));
		SetBaseState(state);
	}

	private void SetBaseState(EButtonState state)
	{
		_baseState = state;
		RefreshState();
	}

	private void RefreshState()
	{
		EButtonState eButtonState = ResolveState();
		if (_currentState != eButtonState)
		{
			_currentState = eButtonState;
			_animator.SetInteger(_animatorParameter, (int)eButtonState);
		}
		EButtonState ResolveState()
		{
			if (!_inputField.interactable)
			{
				return EButtonState.Disabled;
			}
			return _baseState;
		}
	}

	private void OnSubmit()
	{
		if (_inputManager.RxControlDevice.Value == EControlDevice.GamePad && !_isShowingKeyboard)
		{
			ShowVirtualKeyboardAsync().Forget();
		}
	}

	private async UniTaskVoid ShowVirtualKeyboardAsync()
	{
		_isShowingKeyboard = true;
		_inputField.DeactivateInputField();
		var (ePadInputResult, text) = await UIInputUtility.ShowGamepadTextInputAsync(_description, _charLimit, _inputField.text);
		_isShowingKeyboard = false;
		if (ePadInputResult == EPadInputResult.NotAvailable)
		{
			_toastManager.NoticeFail(TLocalizedText.UseKeyboardInput.LocalText);
		}
		if (ePadInputResult == EPadInputResult.Success)
		{
			_inputField.text = text;
		}
	}
}

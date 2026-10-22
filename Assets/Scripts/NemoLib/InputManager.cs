using NemoLib.Log;
using UniRx;
using UnityEngine.EventSystems;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using VContainer;

public abstract class InputManager : MonoBase
{
	[Inject]
	protected SeriLogger Logger;

	public ReactiveProperty<EControlDevice> RxControlDevice = new ReactiveProperty<EControlDevice>(EControlDevice.Keyboard);

	private static EControlDevice _currentDevice = EControlDevice.Keyboard;

	private bool _isSelecting;

	private const float DEVICE_SWITCH_COOLDOWN = 0.3f;

	private float _lastDeviceSwitchTime = -1f;

	public static bool IsGamePad => _currentDevice == EControlDevice.GamePad;

	public static bool IsKeyboard => _currentDevice == EControlDevice.Keyboard;

	public Vector2? CurrentSelectedPosition { get; private set; }

	public abstract void BlockInput();

	public abstract void UnblockInput();

	protected override void Awake()
	{
		this.InjectFromRootContainer();
		UnityEngine.Object.DontDestroyOnLoad(this);
		RxControlDevice.Subscribe(delegate(EControlDevice device)
		{
			_currentDevice = device;
		}).AddTo(this);
		InputSystem.onEvent.AsObservable().Where(delegate(InputEventPtr eventPtr)
		{
			return eventPtr.IsA<StateEvent>() || eventPtr.IsA<DeltaStateEvent>();
		}).Subscribe(OnAnyInputEvent).AddTo(this);
	}

	public void SelectObject(GameObject go)
	{
		EventSystem current = EventSystem.current;
		if (current == null)
		{
			return;
		}
		if (go != null && !go.activeSelf)
		{
			return;
		}
		if (!(current.currentSelectedGameObject == go) && !_isSelecting)
		{
			_isSelecting = true;
			try
			{
				EventSystem.current.SetSelectedGameObject(go);
			}
			finally
			{
				_isSelecting = false;
			}
			RectTransform rectTransform = (go != null) ? go.GetComponent<RectTransform>() : null;
			if (rectTransform != null)
			{
				CurrentSelectedPosition = (Vector2)rectTransform.position;
			}
		}
	}

	public void DeselectObject(GameObject go)
	{
		EventSystem current = EventSystem.current;
		GameObject selected = (current != null) ? current.currentSelectedGameObject : null;
		if (!(selected != go) && !_isSelecting)
		{
			_isSelecting = true;
			try
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			finally
			{
				_isSelecting = false;
			}
		}
	}

	public void ClearSelectedPosition()
	{
		CurrentSelectedPosition = null;
	}

	public GameObject GetSelectedObject()
	{
		EventSystem current = EventSystem.current;
		if (current == null)
		{
			return null;
		}
		return current.currentSelectedGameObject;
	}

	private void OnAnyInputEvent(InputEventPtr eventPtr)
	{
		InputDevice deviceById = InputSystem.GetDeviceById(eventPtr.deviceId);
		if (deviceById == null)
		{
			return;
		}
		EControlDevice newDevice;
		if (deviceById is Gamepad gamepad)
		{
			if (!HasGamepadInput(gamepad))
			{
				return;
			}
			newDevice = EControlDevice.GamePad;
		}
		else
		{
			if (!(deviceById is Keyboard))
			{
				if (!(deviceById is Mouse mouse))
				{
					return;
				}
				if (mouse.delta.ReadValue().magnitude < 20f && mouse.CheckStateIsAtDefault())
				{
					return;
				}
			}
			newDevice = EControlDevice.Keyboard;
		}
		if (RxControlDevice.Value == newDevice)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!(0.3f <= realtimeSinceStartup - _lastDeviceSwitchTime))
		{
			return;
		}
		_lastDeviceSwitchTime = realtimeSinceStartup;
		if (newDevice == EControlDevice.GamePad)
		{
			ClearMouseHover();
		}
		else if (!(EventSystem.current == null))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		RxControlDevice.Value = newDevice;
	}

	private void ClearMouseHover()
	{
		EventSystem current = EventSystem.current;
		if (current == null || Mouse.current == null)
		{
			return;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Mouse.current.position.ReadValue();
		var results = new System.Collections.Generic.List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, results);
		foreach (RaycastResult item in results)
		{
			ExecuteEvents.ExecuteHierarchy(item.gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
		}
	}

	private void ClearGamePadFocus()
	{
		if (!(EventSystem.current == null))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	private static bool HasGamepadInput(Gamepad gamepad)
	{
		float deadzone = InputSystem.settings.defaultDeadzoneMin;
		if (gamepad.leftStick.ReadValue().magnitude > deadzone)
		{
			return true;
		}
		if (gamepad.rightStick.ReadValue().magnitude > deadzone)
		{
			return true;
		}
		if (gamepad.leftTrigger.ReadValue() > 0.1f)
		{
			return true;
		}
		if (gamepad.rightTrigger.ReadValue() > 0.1f)
		{
			return true;
		}
		return gamepad.buttonNorth.isPressed
			|| gamepad.buttonSouth.isPressed
			|| gamepad.buttonEast.isPressed
			|| gamepad.buttonWest.isPressed
			|| gamepad.leftShoulder.isPressed
			|| gamepad.rightShoulder.isPressed
			|| gamepad.startButton.isPressed
			|| gamepad.selectButton.isPressed
			|| gamepad.leftStickButton.isPressed
			|| gamepad.rightStickButton.isPressed
			|| gamepad.dpad.up.isPressed
			|| gamepad.dpad.down.isPressed
			|| gamepad.dpad.left.isPressed
			|| gamepad.dpad.right.isPressed;
	}
}

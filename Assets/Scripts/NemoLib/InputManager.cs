using NemoLib.Log;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using VContainer;

public abstract class InputManager : MonoBase
{
	[Inject]
	protected SeriLogger Logger;

	public ReactiveProperty<EControlDevice> RxControlDevice;

	private static EControlDevice _currentDevice;

	private bool _isSelecting;

	private const float DEVICE_SWITCH_COOLDOWN = 0.3f;

	private float _lastDeviceSwitchTime;

	public static bool IsGamePad => false;

	public static bool IsKeyboard => false;

	public Vector2? CurrentSelectedPosition { get; private set; }

	public abstract void BlockInput();

	public abstract void UnblockInput();

	protected override void Awake()
	{
	}

	public void SelectObject(GameObject go)
	{
	}

	public void DeselectObject(GameObject go)
	{
	}

	public void ClearSelectedPosition()
	{
	}

	public GameObject GetSelectedObject()
	{
		return null;
	}

	private void OnAnyInputEvent(InputEventPtr eventPtr)
	{
	}

	private void ClearMouseHover()
	{
	}

	private void ClearGamePadFocus()
	{
	}

	private static bool HasGamepadInput(Gamepad gamepad)
	{
		return false;
	}
}

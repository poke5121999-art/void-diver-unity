using NemoLib;
using UniRx;
using UnityEngine;
using VContainer;

public class CursorManager : MonoBase
{
	[SerializeField]
	private MouseCursor _mouseCursor;

	[SerializeField]
	private PadCursor _padCursor;

	[SerializeField]
	private PadFocusCursor _padFocusCursor;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	private bool UsePadFocusCursor => _environmentSettings.UsePadFocusCursor;

	private bool IsGamePad => InputManager.IsGamePad;

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		Cursor.visible = false;
		_inputManager.RxControlDevice.Subscribe(OnControlDeviceChanged).AddTo(this);
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
		_mouseCursor.gameObject.SetActive(!IsGamePad);
		_padCursor.gameObject.SetActive(IsGamePad);
		_padFocusCursor.gameObject.SetActive(IsGamePad && UsePadFocusCursor);
	}
}

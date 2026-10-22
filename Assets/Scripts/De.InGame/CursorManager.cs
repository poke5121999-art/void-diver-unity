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

	private bool UsePadFocusCursor => false;

	private bool IsGamePad => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
	}
}

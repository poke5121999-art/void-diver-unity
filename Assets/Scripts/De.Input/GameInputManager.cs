using System.Collections.Generic;
using UnityEngine.InputSystem;
using VContainer;

public class GameInputManager : InputManager
{
	private InputActions _inputActions;

	[Inject]
	private InputActionAsset _inputActionAsset;

	private readonly Dictionary<EInputActionType, int> _typeBlockCounts;

	private readonly Stack<List<EInputActionType>> _globalBlockSnapshots;

	public InputActions.PlayerActions PlayerActions => default;

	public InputActions.PlayerFuncActions PlayerFuncActions => default;

	public InputActions.InGameActions InGameActions => default;

	public InputActions.InGameKeyboardActions InGameKeyboardActions => default;

	public InputActions.InGamePadActions InGamePadActions => default;

	public InputActions.UiActions UiActions => default;

	public InputActions.UiPadActions UiPadActions => default;

	public InputActions.UiKeyboardActions UiKeyboardActions => default;

	public InputActions.CutsceneActions CutsceneActions => default;

	public InputAction FindAction(string actionName)
	{
		return null;
	}

	protected override void Awake()
	{
	}

	public void BlockInput(EInputActionType actionType)
	{
	}

	public void UnblockInput(EInputActionType actionType)
	{
	}

	public override void BlockInput()
	{
	}

	public override void UnblockInput()
	{
	}

	private void EnableActions(EInputActionType actionType)
	{
	}

	private void DisableActions(EInputActionType actionType)
	{
	}

	public void Clear()
	{
	}

	private void OnDestroy()
	{
	}
}

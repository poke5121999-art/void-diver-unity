using System;
using System.Collections.Generic;
using System.Linq;
using InputIcons;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class GameInputManager : InputManager
{
	private InputActions _inputActions;

	[Inject]
	private InputActionAsset _inputActionAsset;

	private readonly Dictionary<EInputActionType, int> _typeBlockCounts = new Dictionary<EInputActionType, int>();

	private readonly Stack<List<EInputActionType>> _globalBlockSnapshots = new Stack<List<EInputActionType>>();

	public InputActions.PlayerActions PlayerActions => _inputActions.Player;

	public InputActions.PlayerFuncActions PlayerFuncActions => _inputActions.PlayerFunc;

	public InputActions.InGameActions InGameActions => _inputActions.InGame;

	public InputActions.InGameKeyboardActions InGameKeyboardActions => _inputActions.InGameKeyboard;

	public InputActions.InGamePadActions InGamePadActions => _inputActions.InGamePad;

	public InputActions.UiActions UiActions => _inputActions.Ui;

	public InputActions.UiPadActions UiPadActions => _inputActions.UiPad;

	public InputActions.UiKeyboardActions UiKeyboardActions => _inputActions.UiKeyboard;

	public InputActions.CutsceneActions CutsceneActions => _inputActions.Cutscene;

	public InputAction FindAction(string actionName)
	{
		return _inputActionAsset.FindAction(actionName);
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
		_inputActions = new InputActions(_inputActionAsset);
		_inputActions.asset.Enable();
		II_ImagePrompt.LiveAsset = _inputActionAsset;
		Clear();
	}

	public void BlockInput(EInputActionType actionType)
	{
		_typeBlockCounts.TryGetValue(actionType, out var value);
		_typeBlockCounts[actionType] = value + 1;
		if (value == 0)
		{
			DisableActions(actionType);
		}
	}

	public void UnblockInput(EInputActionType actionType)
	{
		if (_typeBlockCounts.TryGetValue(actionType, out var value) && value > 0)
		{
			_typeBlockCounts[actionType] = value - 1;
			if (value == 1)
			{
				EnableActions(actionType);
			}
		}
		else
		{
			Logger.Warning(string.Format("{0}({1}) called more than {2}", "UnblockInput", actionType, "BlockInput"));
		}
	}

	public override void BlockInput()
	{
		List<EInputActionType> list = (from kvp in _typeBlockCounts
			where kvp.Value == 0
			select kvp.Key).ToList();
		foreach (EInputActionType item in list)
		{
			_typeBlockCounts.TryGetValue(item, out var value);
			_typeBlockCounts[item] = value + 1;
			if (value == 0)
			{
				DisableActions(item);
			}
		}
		_globalBlockSnapshots.Push(list);
	}

	public override void UnblockInput()
	{
		if (_globalBlockSnapshots.Count == 0)
		{
			Logger.Warning("Global UnblockInput called more than BlockInput");
			return;
		}
		List<EInputActionType> list = _globalBlockSnapshots.Pop();
		foreach (EInputActionType item in list)
		{
			if (_typeBlockCounts.TryGetValue(item, out var value) && value > 0)
			{
				_typeBlockCounts[item] = value - 1;
				if (value == 1)
				{
					EnableActions(item);
				}
			}
			else
			{
				Logger.Warning(string.Format("{0}({1}) called more than {2}", "UnblockInput", item, "BlockInput"));
			}
		}
	}

	private void EnableActions(EInputActionType actionType)
	{
		switch (actionType)
		{
		case EInputActionType.PlayerFunc:
			_inputActions.PlayerFunc.Enable();
			break;
		case EInputActionType.Player:
			_inputActions.Player.Enable();
			break;
		case EInputActionType.InGame:
			_inputActions.InGame.Enable();
			_inputActions.InGameKeyboard.Enable();
			_inputActions.InGamePad.Enable();
			break;
		case EInputActionType.Ui:
			_inputActions.Ui.Enable();
			_inputActions.UiPad.Enable();
			_inputActions.UiKeyboard.Enable();
			break;
		case EInputActionType.Cutscene:
			_inputActions.Cutscene.Enable();
			break;
		}
	}

	private void DisableActions(EInputActionType actionType)
	{
		switch (actionType)
		{
		case EInputActionType.PlayerFunc:
			_inputActions.PlayerFunc.Disable();
			break;
		case EInputActionType.Player:
			_inputActions.Player.Disable();
			break;
		case EInputActionType.InGame:
			_inputActions.InGame.Disable();
			_inputActions.InGameKeyboard.Disable();
			_inputActions.InGamePad.Disable();
			break;
		case EInputActionType.Ui:
			_inputActions.Ui.Disable();
			_inputActions.UiPad.Disable();
			_inputActions.UiKeyboard.Disable();
			break;
		case EInputActionType.Cutscene:
			_inputActions.Cutscene.Disable();
			break;
		}
	}

	public void Clear()
	{
		_typeBlockCounts.Clear();
		_globalBlockSnapshots.Clear();
		foreach (EInputActionType value in Enum.GetValues(typeof(EInputActionType)))
		{
			_typeBlockCounts[value] = 1;
		}
		_inputActions.Player.Disable();
		_inputActions.PlayerFunc.Disable();
		_inputActions.InGame.Disable();
		_inputActions.InGameKeyboard.Disable();
		_inputActions.InGamePad.Disable();
		_inputActions.Ui.Disable();
		_inputActions.UiPad.Disable();
		_inputActions.UiKeyboard.Disable();
		_inputActions.Cutscene.Disable();
	}

	private void OnDestroy()
	{
		Clear();
	}
}

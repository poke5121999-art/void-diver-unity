using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using InputIcons;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using VContainer;

public class InputService : NativeBase
{
	private const string INPUT_BINDINGS_FILE = "InputBindings";

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private BlockManager _blockManager;

	[Inject]
	private StorageService _storageService;

	[Inject]
	private InputActionAsset _inputActionAsset;

	private bool _isDirty;

	private bool _isInitialized;

	public async UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_isInitialized)
		{
			_inputActionAsset.RemoveAllBindingOverrides();
			await LoadAsync(cancellationToken);
			_isInitialized = true;
		}
	}

	public void MarkDirty()
	{
		_isDirty = true;
	}

	public void Flush()
	{
		if (_isDirty)
		{
			_isDirty = false;
			SaveAsync().Forget();
		}
	}

	private async UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = _inputActionAsset.SaveBindingOverridesAsJson();
		if (!string.IsNullOrEmpty(text))
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			await _storageService.SaveAsync("InputBindings", bytes, cancellationToken);
		}
		else
		{
			_storageService.Delete("InputBindings");
		}
	}

	private async UniTask LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		byte[] array = await _storageService.LoadAsync("InputBindings", cancellationToken);
		if (array != null && array.Length != 0)
		{
			string json = Encoding.UTF8.GetString(array);
			_inputActionAsset.LoadBindingOverridesFromJson(json, removeExisting: true);
		}
	}

	public bool CanRebind(int inputActionId)
	{
		TInputAction tInputAction = TInputAction.Table[inputActionId];
		if (!tInputAction.IsRebindable)
		{
			return false;
		}
		if (tInputAction.ControlDevice == EControlDevice.Keyboard && !InputManager.IsKeyboard)
		{
			return false;
		}
		if (tInputAction.ControlDevice == EControlDevice.GamePad && !InputManager.IsGamePad)
		{
			return false;
		}
		return _inputManager.FindAction(tInputAction.ActionName) != null;
	}

	public async UniTask RebindAsync(int inputActionId, CancellationToken cancellationToken = default(CancellationToken))
	{
		_blockManager.Block(calling: false, background: false);
		InputActionRebindingExtensions.RebindingOperation operation = null;
		try
		{
			operation = BuildRebindingOperation(inputActionId);
			if (operation == null)
			{
				return;
			}
			UniTaskCompletionSource<bool> tcs = new UniTaskCompletionSource<bool>();
			operation.OnComplete(delegate
			{
				tcs.TrySetResult(result: true);
			}).OnCancel(delegate
			{
				tcs.TrySetResult(result: false);
			}).Start();
			bool completed;
			using (cancellationToken.Register(delegate
			{
				operation.Cancel();
			}))
			{
				completed = await tcs.Task;
			}
			if (completed)
			{
				if (!TryClearConflictingBindings(inputActionId))
				{
					TInputAction tInputAction = TInputAction.Table[inputActionId];
					_inputManager.FindAction(tInputAction.ActionName).RemoveBindingOverride(tInputAction.BindingIndex);
				}
				ApplySyncBinding(inputActionId);
			}
			_isDirty = true;
			InputIconsManagerSO.onBindingsChanged?.Invoke();
			await WaitRebindControlReleaseAsync(operation, completed, inputActionId, cancellationToken);
		}
		finally
		{
			operation?.Dispose();
			_blockManager.Release(calling: false, background: false);
		}
	}

	private async UniTask WaitRebindControlReleaseAsync(InputActionRebindingExtensions.RebindingOperation operation, bool completed, int inputActionId, CancellationToken cancellationToken)
	{
		TInputAction tInputAction = TInputAction.Table[inputActionId];
		InputControl cancelControl = ((tInputAction.ControlDevice != EControlDevice.GamePad) ? ((InputControl)Keyboard.current?.escapeKey) : ((InputControl)Gamepad.current?.startButton));
		InputControl capturedControl = (completed ? operation.selectedControl : null);
		await UniTask.Yield(cancellationToken);
		await UniTask.WaitWhile(() => (cancelControl != null && cancelControl.IsActuated()) || (capturedControl != null && capturedControl.IsActuated()), PlayerLoopTiming.Update, cancellationToken);
	}

	private InputActionRebindingExtensions.RebindingOperation BuildRebindingOperation(int inputActionId)
	{
		TInputAction tInputAction = TInputAction.Table[inputActionId];
		if (!tInputAction.IsRebindable)
		{
			return null;
		}
		if (tInputAction.ControlDevice == EControlDevice.Keyboard && !InputManager.IsKeyboard)
		{
			return null;
		}
		if (tInputAction.ControlDevice == EControlDevice.GamePad && !InputManager.IsGamePad)
		{
			return null;
		}
		if (_inputManager.FindAction(tInputAction.ActionName) == null)
		{
			return null;
		}
		TInputAction data = TInputAction.Table[inputActionId];
		InputActionRebindingExtensions.RebindingOperation operation = _inputManager.FindAction(data.ActionName).PerformInteractiveRebinding(data.BindingIndex);
		ConfigureForDevice();
		return operation;
		void ConfigureForDevice()
		{
			if (data.ControlDevice == EControlDevice.Keyboard)
			{
				operation.WithControlsExcluding("<Gamepad>").WithCancelingThrough("<Keyboard>/escape");
			}
			else if (data.ControlDevice == EControlDevice.GamePad)
			{
				operation.WithControlsExcluding("<Mouse>").WithControlsExcluding("<Keyboard>").WithCancelingThrough("<Gamepad>/start");
			}
		}
	}

	private void ApplySyncBinding(int inputActionId)
	{
		TInputAction tInputAction = TInputAction.Table[inputActionId];
		if (string.IsNullOrEmpty(tInputAction.SyncActionName) || !TryGetEffectivePath(tInputAction.ActionName, tInputAction.BindingIndex, out var path))
		{
			return;
		}
		InputAction inputAction = _inputManager.FindAction(tInputAction.SyncActionName);
		if (inputAction != null && tInputAction.SyncBindingIndex >= 0 && tInputAction.SyncBindingIndex < inputAction.bindings.Count)
		{
			if (string.IsNullOrEmpty(path))
			{
				inputAction.RemoveBindingOverride(tInputAction.SyncBindingIndex);
			}
			else
			{
				inputAction.ApplyBindingOverride(tInputAction.SyncBindingIndex, path);
			}
		}
	}

	public void ResetBindings(EControlDevice device)
	{
		foreach (TInputAction item in TInputAction.Table.Values.Where((TInputAction _) => _.ControlDevice == device))
		{
			RemoveOverride(item.ActionName, item.BindingIndex);
			RemoveOverride(item.ModifierActionName, item.ModifierBindingIndex);
			RemoveOverride(item.SyncActionName, item.SyncBindingIndex);
		}
		_isDirty = true;
		InputIconsManagerSO.onBindingsChanged?.Invoke();
	}

	private void RemoveOverride(string actionName, int bindingIndex)
	{
		if (!string.IsNullOrEmpty(actionName))
		{
			InputAction inputAction = _inputManager.FindAction(actionName);
			if (inputAction != null && bindingIndex >= 0 && bindingIndex < inputAction.bindings.Count)
			{
				inputAction.RemoveBindingOverride(bindingIndex);
			}
		}
	}

	private bool TryClearConflictingBindings(int rebindingActionId)
	{
		TInputAction tInputAction = TInputAction.Table[rebindingActionId];
		if (!TryGetEffectivePath(tInputAction.ActionName, tInputAction.BindingIndex, out var path) || string.IsNullOrEmpty(path))
		{
			return true;
		}
		string text = null;
		if (!string.IsNullOrEmpty(tInputAction.ModifierActionName) && TryGetEffectivePath(tInputAction.ModifierActionName, tInputAction.ModifierBindingIndex, out var path2))
		{
			text = (string.IsNullOrEmpty(path2) ? null : path2);
		}
		List<TInputAction> list = new List<TInputAction>();
		foreach (TInputAction value in TInputAction.Table.Values)
		{
			if (value.Id != rebindingActionId && value.ControlDevice == tInputAction.ControlDevice && TryGetEffectivePath(value.ActionName, value.BindingIndex, out var path3) && !(path3 != path) && !(GetModifierPath(value) != text))
			{
				if (!value.IsRebindable)
				{
					return false;
				}
				list.Add(value);
			}
		}
		foreach (TInputAction item in list)
		{
			_inputManager.FindAction(item.ActionName).ApplyBindingOverride(item.BindingIndex, string.Empty);
		}
		return true;
	}

	private bool TryGetEffectivePath(string actionName, int bindingIndex, out string path)
	{
		path = null;
		InputAction inputAction = _inputManager.FindAction(actionName);
		if (inputAction != null && bindingIndex >= 0 && bindingIndex < inputAction.bindings.Count)
		{
			path = inputAction.bindings[bindingIndex].effectivePath;
			return true;
		}
		return false;
	}

	private string GetModifierPath(TInputAction data)
	{
		string path = null;
		if (!string.IsNullOrEmpty(data.ModifierActionName) && TryGetEffectivePath(data.ModifierActionName, data.ModifierBindingIndex, out path) && string.IsNullOrEmpty(path))
		{
			path = null;
		}
		return path;
	}
}

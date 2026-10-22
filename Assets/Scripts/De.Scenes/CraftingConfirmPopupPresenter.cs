using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class CraftingConfirmPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CraftingConfirmPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[View]
	private CraftingConfirmPopupView _view;

	[InjectModel]
	private CraftingConfirmPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameManager _gameManager;

	private long _maxCraftableCount;

	private long _craftingCount;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiKeyboardActions UiKeyboardActions => default;

	private PlayerInventory MyInventory => null;

	private bool AllowAccessStash => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__21))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	private void ConnectGridBottomTo(IReadOnlyList<Selectable> slots, Selectable target)
	{
	}

	private Selectable LastRowHead(IReadOnlyList<Selectable> slots)
	{
		return null;
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
	}

	private void UpdateUis()
	{
	}

	private void SetCount(long count)
	{
	}

	private void OnQuantitySliderValueChanged(float value)
	{
	}

	private void OnDecrease()
	{
	}

	private void OnIncrease()
	{
	}
}

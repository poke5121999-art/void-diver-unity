using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class RestorePlayerPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RestorePlayerPopupPresenter _003C_003E4__this;

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
	private RestorePlayerPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private PlayerSaveSlotScrollerModel _saveScrollerModel;

	private PlayerRestoreSlotScrollerModel _restoreScrollerModel;

	private PlayerSaveSlotModel _selectedSaveSlot;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__15))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnSaveSlotClick(PlayerSaveSlotModel slotModel)
	{
	}

	private void OnRestoreSlotClick(PlayerRestoreSlotModel slotModel)
	{
	}

	private void OnBack()
	{
	}
}

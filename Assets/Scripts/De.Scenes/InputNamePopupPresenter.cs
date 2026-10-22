using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class InputNamePopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CConfirmAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public InputNamePopupPresenter _003C_003E4__this;

		private string _003CinputText_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__9 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InputNamePopupPresenter _003C_003E4__this;

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
	private InputNamePopupView _view;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__9))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	[AsyncStateMachine(typeof(_003CConfirmAsync_003Ed__12))]
	private UniTask<bool> ConfirmAsync()
	{
		return default;
	}

	private void OnConfirmButtonClick()
	{
	}

	private void OnCancelButtonClick()
	{
	}

	private bool ContainsSpecialCharacter(string text)
	{
		return false;
	}

	private int GetTextLength(string text)
	{
		return 0;
	}
}

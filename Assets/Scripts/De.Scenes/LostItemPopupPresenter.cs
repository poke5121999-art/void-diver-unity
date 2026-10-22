using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.InputSystem;
using VContainer;

public class LostItemPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Scroller = 1,
		Btns = 2
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass36_0
	{
		public LostGoods lostGoods;

		internal bool _003CRecoverAsync_003Eb__0(LostItemSelectInfo info)
		{
			return false;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LostItemPopupPresenter _003C_003E4__this;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRecoverAsync_003Ed__36 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LostItemSlotModel slotModel;

		public LostItemPopupPresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass36_0 _003C_003E8__1;

		private LostItemSelectInfo _003CselectInfo_003E5__2;

		private int _003Cprice_003E5__3;

		private bool _003CisArtifact_003E5__4;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask<ResTakeLostItem>.Awaiter _003C_003Eu__2;

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
	private struct _003CRerollAsync_003Ed__39 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LostItemPopupPresenter _003C_003E4__this;

		private UniTask<ResRerollLostItems>.Awaiter _003C_003Eu__1;

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
	private LostItemPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	private LostItemScrollerModel _scrollerModel;

	private EFocusPanel _focusPanel;

	private CompositeDisposable _btnsNavDisposable;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	private InputAction MoveScrollAction => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private PlayerInventory MyInventory => null;

	private long RerollCost => 0L;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__25))]
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

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel)
	{
	}

	private void SetScroller()
	{
	}

	private void OnSlotEnter(LostItemSlotModel slotModel)
	{
	}

	private void OnSlotExit(LostItemSlotModel slotModel)
	{
	}

	private void OnSlotClick(LostItemSlotModel slotModel)
	{
	}

	private void OnRerollClick()
	{
	}

	private void OnListClick()
	{
	}

	[AsyncStateMachine(typeof(_003CRecoverAsync_003Ed__36))]
	private UniTask RecoverAsync(LostItemSlotModel slotModel)
	{
		return default;
	}

	private void RefreshBtnsInteractable()
	{
	}

	private void SetRerollCostView()
	{
	}

	[AsyncStateMachine(typeof(_003CRerollAsync_003Ed__39))]
	private UniTask RerollAsync()
	{
		return default;
	}
}

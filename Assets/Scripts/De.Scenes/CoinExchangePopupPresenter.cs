using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using VContainer;

public class CoinExchangePopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnConfirmAsync_003Ed__35 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CoinExchangePopupPresenter _003C_003E4__this;

		private long _003CsentQuantity_003E5__2;

		private long _003CmaxBeforeSend_003E5__3;

		private bool _003Csucceeded_003E5__4;

		private UniTask<ResExchangeCoin>.Awaiter _003C_003Eu__1;

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
	private struct _003COnOpenAsync_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CoinExchangePopupPresenter _003C_003E4__this;

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

	[InjectModel]
	private CoinExchangePopupModel _model;

	[View]
	private CoinExchangePopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private ECoinExchangeDirection _direction;

	private long _quantity;

	private long _quantityMax;

	private bool _isRequesting;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private InputActions.UiKeyboardActions UiKeyboardActions => default;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__23))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
	}

	private void OnDirectionChanged(ECoinExchangeDirection direction)
	{
	}

	private void UpdateUi(bool applyImmediately = false)
	{
	}

	private void UpdateDirectionUi()
	{
	}

	private void UpdateQuantityUi(long? overrideMax = null)
	{
	}

	private void OnDecrease()
	{
	}

	private void OnIncrease()
	{
	}

	private void OnSliderValueChanged(float value)
	{
	}

	private void SetQuantity(long quantity)
	{
	}

	private long GetRealCoinCount()
	{
		return 0L;
	}

	[AsyncStateMachine(typeof(_003COnConfirmAsync_003Ed__35))]
	private UniTask OnConfirmAsync()
	{
		return default;
	}

	private void OnCancel()
	{
	}

	private void OnUserUpdated()
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}

	private void OnDecreaseQuantityLarge()
	{
	}

	private void OnIncreaseQuantityLarge()
	{
	}

	private void InitGamePadNav()
	{
	}
}

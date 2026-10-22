using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using UnityEngine.InputSystem;
using VContainer;

public class GoodsQuantityPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GoodsQuantityPopupPresenter _003C_003E4__this;

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
	private GoodsQuantityPopupModel _model;

	[View]
	private GoodsQuantityPopupView _view;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiKeyboardActions UiKeyboardActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__11))]
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

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
	}

	private void OnConfirm()
	{
	}

	private void OnDecrease()
	{
	}

	private void OnIncrease()
	{
	}

	private void OnSliderValueChange(float value)
	{
	}

	private void SetCount(long count)
	{
	}

	private void UpdateValueUi()
	{
	}
}

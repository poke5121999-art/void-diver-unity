using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using VContainer;

public class RoomSettingPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RoomSettingPopupPresenter _003C_003E4__this;

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
	private RoomSettingPopupView _view;

	[InjectModel]
	private RoomSettingPopupModel _model;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__15))]
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

	private void OnOptionLeftClick()
	{
	}

	private void OnOptionRightClick()
	{
	}

	private void OnConfirmButtonClick()
	{
	}

	private void OnCancel()
	{
	}

	private void OnClickNextDifficulty()
	{
	}

	private void OnClickPrevDifficulty()
	{
	}

	private void OnClickNextPublishType()
	{
	}

	private void OnClickPrevPublishType()
	{
	}
}

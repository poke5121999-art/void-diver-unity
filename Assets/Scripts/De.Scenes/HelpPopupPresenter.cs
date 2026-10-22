using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using VContainer;

public class HelpPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__17 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public HelpPopupPresenter _003C_003E4__this;

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
	private HelpPopupView _view;

	[Inject]
	private SoundManager _soundManager;

	private int _currentPageIndex;

	private GameInputManager GameInputManager => null;

	private int PageCount => 0;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	public override string OpenSfx => null;

	public override string CloseSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__17))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}

	private void TryNextPage()
	{
	}

	private void TryPrevPage()
	{
	}

	protected void OnPageChanged(int page)
	{
	}

	private int ClampPage(int page)
	{
		return 0;
	}
}

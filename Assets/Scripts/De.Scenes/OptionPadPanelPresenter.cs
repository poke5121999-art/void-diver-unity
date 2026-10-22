using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class OptionPadPanelPresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		Base = 0,
		Scroller = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRebindAsync_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public OptionPadPanelPresenter _003C_003E4__this;

		public int inputActionId;

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
	private OptionPadPanelModel _model;

	[View]
	private OptionPadPanelView _view;

	[Inject]
	private InputService _inputService;

	private KeySettingSlotRowScrollerModel _scrollerModel;

	private KeySettingSlotModel _selectedSlot;

	private CancellationTokenSource _rebindCts;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(OptionPadPanelModel model)
	{
	}

	private void InitScroller()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnSlotClick(KeySettingSlotModel slotModel)
	{
	}

	[AsyncStateMachine(typeof(_003CRebindAsync_003Ed__18))]
	private UniTask RebindAsync(int inputActionId)
	{
		return default;
	}

	private void OnResetClick()
	{
	}

	private void OnBack()
	{
	}
}

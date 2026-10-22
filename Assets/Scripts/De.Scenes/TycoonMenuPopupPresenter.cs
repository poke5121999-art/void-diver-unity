using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class TycoonMenuPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TycoonMenuPopupPresenter _003C_003E4__this;

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
	private TycoonMenuPopupView _view;

	[InjectModel]
	private TycoonMenuPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SoundManager _soundManager;

	private TycoonTabModel[] _tabModels;

	private ETycoonMenuTab _currentTab;

	private BasePanelModel _selectedPageModel;

	private CompositeDisposable _pageDisposable;

	private GameInputManager GameInputManager => null;

	private InputActions.UiPadActions UiPadActions => default;

	public override bool IsInputActionPopup => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__14))]
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

	protected override void OnDestroy()
	{
	}

	private void InitTabs()
	{
	}

	private void OnTabClicked(TycoonTabModel tabModel)
	{
	}

	private void SwitchTab(ETycoonMenuTab tab)
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}
}

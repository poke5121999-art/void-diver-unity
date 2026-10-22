using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using VContainer;

public class SystemPagePresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnCloseGameButtonClickAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SystemPagePresenter _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003COnExitTheLoungeButtonClick_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SystemPagePresenter _003C_003E4__this;

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
	private struct _003COnGiveUpButtonClick_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SystemPagePresenter _003C_003E4__this;

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
	private struct _003CReturnToTitleButtonClick_003Ed__27 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SystemPagePresenter _003C_003E4__this;

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

	[View]
	private SystemPageView _view;

	[InjectModel]
	private SystemPageModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private AccountRecordManager _accountRecordManager;

	private CompositeDisposable _disposable;

	private bool _showGiveUp;

	private bool _showExitLounge;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private void OnInject(SystemPageModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnDisable()
	{
	}

	private void Close()
	{
	}

	private void OnContinueButtonClick()
	{
	}

	[AsyncStateMachine(typeof(_003COnGiveUpButtonClick_003Ed__26))]
	private UniTask OnGiveUpButtonClick()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CReturnToTitleButtonClick_003Ed__27))]
	private UniTask ReturnToTitleButtonClick()
	{
		return default;
	}

	private void OnManualButtonClick()
	{
	}

	[AsyncStateMachine(typeof(_003COnExitTheLoungeButtonClick_003Ed__29))]
	private UniTask OnExitTheLoungeButtonClick()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnCloseGameButtonClickAsync_003Ed__30))]
	private UniTask OnCloseGameButtonClickAsync()
	{
		return default;
	}
}

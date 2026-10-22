using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class SquadPageNotJoinedPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnCreateRoomButtonClickAsync_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SquadPageNotJoinedPanelPresenter _003C_003E4__this;

		private UniTask<RoomSettingPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<GameCommon.Enum.ECreateLobbyResult>.Awaiter _003C_003Eu__3;

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
	private SquadPageNotJoinedPanelModel _model;

	[View]
	private SquadPageNotJoinedPanelView _view;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(SquadPageNotJoinedPanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	[AsyncStateMachine(typeof(_003COnCreateRoomButtonClickAsync_003Ed__16))]
	private UniTask OnCreateRoomButtonClickAsync()
	{
		return default;
	}

	private void OnJoinRoomButtonClick()
	{
	}
}

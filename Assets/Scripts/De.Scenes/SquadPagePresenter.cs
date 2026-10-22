using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SquadPagePresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Joined = 1,
		NotJoined = 2
	}

	[InjectModel]
	private SquadPageModel _model;

	[View]
	private SquadPageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private GameInputManager _inputManager;

	private CompositeDisposable _disposable;

	private SquadPageJoinedPanelModel _joinedPanelModel;

	private SquadPageNotJoinedPanelModel _notJoinedPanelModel;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	private bool IsSingleMode => _roomContext.IsSingleMode;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void Start()
	{
		_roomMatchmaker.OnJoinLobby.Subscribe(delegate
		{
			OnJoinLobby();
		}).AddTo(this);
	}

	private void OnInject(SquadPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshUIs();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (_inputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
			FocusPanel(IsSingleMode ? EFocusPanel.NotJoined : EFocusPanel.Joined);
		}
	}

	protected override void OnInputDisabled()
	{
		if (_notJoinedPanelModel != null)
		{
			_notJoinedPanelModel.RxIsInputEnabled.Value = false;
		}
		if (_joinedPanelModel != null)
		{
			_joinedPanelModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private void OnJoinLobby()
	{
		RefreshUIs();
		if (IsInputEnabled)
		{
			FocusPanel(IsSingleMode ? EFocusPanel.NotJoined : EFocusPanel.Joined);
		}
	}

	private void FocusPanel(EFocusPanel panel)
	{
		if (_notJoinedPanelModel != null)
		{
			_notJoinedPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.NotJoined;
		}
		if (_joinedPanelModel != null)
		{
			_joinedPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Joined;
		}
	}

	private void RefreshUIs()
	{
		bool isHost = _gameManager.IsHost;
		_view.NotJoinedPanel.gameObject.SetActive(IsSingleMode);
		_view.JoinedPanel.gameObject.SetActive(!IsSingleMode);
		if (!IsSingleMode)
		{
			_joinedPanelModel = new SquadPageJoinedPanelModel
			{
				IsHost = isHost
			};
			_view.JoinedPanel.Inject(_joinedPanelModel);
		}
		else
		{
			_notJoinedPanelModel = new SquadPageNotJoinedPanelModel
			{
				IsHost = isHost
			};
			_view.NotJoinedPanel.Inject(_notJoinedPanelModel);
		}
	}
}

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

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private bool IsSingleMode => false;

	protected override void OnDestroy()
	{
	}

	private void Start()
	{
	}

	private void OnInject(SquadPageModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnDisable()
	{
	}

	private void Close()
	{
	}

	private void OnJoinLobby()
	{
	}

	private void FocusPanel(EFocusPanel panel)
	{
	}

	private void RefreshUIs()
	{
	}
}

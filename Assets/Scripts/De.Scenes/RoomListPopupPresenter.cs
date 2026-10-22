using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class RoomListPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Top = 0,
		Scroller = 1,
		Bottom = 2
	}

	[View]
	private RoomListPopupView _view;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private RoomScrollerModel _scrollerModel;

	private MatchingLobby _selectedLobby;

	private LoungeFilterPopupModel _filterModel;

	private EFocusPanel _focusPanel;

	private Selectable _lastFocusedBeforeScroller;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private RoomScrollerPresenter _scrollerPresenter => _view.ScrollList.GetComponent<RoomScrollerPresenter>();

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_filterModel = new LoungeFilterPopupModel();
		_filterModel.Load();
		_view.CancelButton.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.RefreshButton.OnClickWithSfx().Subscribe(delegate
		{
			OnRefreshButtonClick();
		}).AddTo(this);
		_view.FilterButton.OnClickWithSfx().Subscribe(delegate
		{
			OnFilterButtonClickAsync().Forget();
		}).AddTo(this);
		_view.EnterButton.OnClickWithSfx().Subscribe(delegate
		{
			OnEnterButtonClickAsync().Forget();
		}).AddTo(this);
		_view.EnterCodeButton.OnClickWithSfx().Subscribe(delegate
		{
			OnEnterByCodeClick();
		}).AddTo(this);
		_scrollerModel = new RoomScrollerModel();
		_scrollerModel.OnGameRoomClick.Subscribe(OnGameRoomClick).AddTo(this);
		_scrollerModel.OutOfTop.Subscribe(delegate
		{
			FocusTop();
		}).AddTo(this);
		_scrollerModel.OutOfBottom.Subscribe(delegate
		{
			FocusBottom();
		}).AddTo(this);
		_view.ScrollList.GetComponent<RoomScrollerPresenter>().Inject(_scrollerModel);
		_view.ScrollList.GetComponent<RoomScrollerPresenter>().RefreshRoomListAsync(_filterModel).Forget();
	}

	private void FocusTop()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		_focusPanel = EFocusPanel.Top;
		ClearNav();
		SetNavHorizontal(new Selectable[2] { _view.FilterButton, _view.RefreshButton });
		ResumeNav();
		SetDefaultNav((_lastFocusedBeforeScroller != null) ? MapToTop(_lastFocusedBeforeScroller) : _view.FilterButton);
	}

	private void FocusScroller(ENavDir entryDir)
	{
		_lastFocusedBeforeScroller = FocusedSelectable;
		if (_scrollerPresenter.ItemCount != 0)
		{
			_focusPanel = EFocusPanel.Scroller;
			ClearNav();
			SuspendNav();
			_scrollerModel.EntryDirection = entryDir;
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
		else if (entryDir == ENavDir.Down)
		{
			FocusBottom();
		}
		else
		{
			FocusTop();
		}
	}

	private void FocusBottom()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		_focusPanel = EFocusPanel.Bottom;
		ClearNav();
		SetNavHorizontal(new Selectable[3] { _view.EnterCodeButton, _view.EnterButton, _view.CancelButton });
		ResumeNav();
		SetDefaultNav((_lastFocusedBeforeScroller != null) ? MapToBottom(_lastFocusedBeforeScroller) : _view.EnterCodeButton);
	}

	private Selectable MapToTop(Selectable source)
	{
		if (source == _view.FilterButton)
		{
			return source;
		}
		if (source == _view.RefreshButton)
		{
			return source;
		}
		if (source == _view.CancelButton)
		{
			return _view.RefreshButton;
		}
		return _view.FilterButton;
	}

	private Selectable MapToBottom(Selectable source)
	{
		if (source == _view.EnterCodeButton)
		{
			return source;
		}
		if (source == _view.EnterButton)
		{
			return source;
		}
		if (source == _view.CancelButton)
		{
			return source;
		}
		if (source == _view.RefreshButton)
		{
			return _view.CancelButton;
		}
		return _view.EnterButton;
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
		if ((_focusPanel != EFocusPanel.Top || dir != ENavDir.Down) && (_focusPanel != EFocusPanel.Bottom || dir != ENavDir.Up))
		{
			base.OnNavOutOfBounds(dir);
		}
		else
		{
			FocusScroller(dir);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (GameInputManager == null)
		{
			return;
		}
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		switch (_focusPanel)
		{
		case EFocusPanel.Top:
			FocusTop();
			break;
		case EFocusPanel.Scroller:
			if (_scrollerPresenter.ItemCount == 0)
			{
				FocusTop();
				break;
			}
			SuspendNav();
			_scrollerModel.RxIsInputEnabled.Value = true;
			break;
		case EFocusPanel.Bottom:
			FocusBottom();
			break;
		}
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnRefreshButtonClick()
	{
		if (_appManager.IsOnline())
		{
			_scrollerPresenter.RefreshRoomListAsync(_filterModel).Forget();
		}
		else
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
		}
	}

	private async UniTask OnFilterButtonClickAsync()
	{
		LoungeFilterPopupPresenter loungeFilterPopupPresenter = await _popupManager.OpenAsync<LoungeFilterPopupPresenter>(_filterModel.Clone());
		if (loungeFilterPopupPresenter != null && await loungeFilterPopupPresenter.Task is LoungeFilterPopupModel filterModel)
		{
			_filterModel = filterModel;
			_filterModel.Save();
			_scrollerPresenter.RefreshRoomListAsync(_filterModel).Forget();
		}
	}

	private void OnGameRoomClick(MatchingLobby matchingLobby)
	{
		_selectedLobby = matchingLobby;
	}

	private async UniTask OnEnterButtonClickAsync()
	{
		if (!_appManager.IsOnline())
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
		}
		else if (_selectedLobby == null)
		{
			_toastManager.NoticeFail(TLocalizedText.GameLoomLobbyNotSelected.LocalText);
		}
		else if (_selectedLobby.IsGameStarted)
		{
			_toastManager.NoticeFail(TLocalizedText.LobbyAlreadyStarted.LocalText);
		}
		else
		{
			_popupManager.CloseAll();
			GamePlayerData playerData = _playerManager.CreateRoomPlayerData(isHostPlayer: false);
			int connectionResultCode = await _gameChannel.ConnectAndJoinBySteamLobbyAsync(_selectedLobby.LobbyId, playerData);
			_gameManager.CachedConnectionResultCode = connectionResultCode;
			_gameChannel.RxConnectState.Value = ERoomConnectState.BackToLocalRoom;
			await _gameChannel.ShutdownAsync();
			await _gameChannel.RecreateTransportAsync();
			_gameHost.CreateRoom(_playerManager.Difficulty);
			await _gameChannel.TryJoinGame(_playerManager.CreateRoomPlayerData(isHostPlayer: true));
			_gameManager.LoadGame();
		}
	}

	private void OnEnterByCodeClick()
	{
		if (_appManager.IsOnline())
		{
			_popupManager.Open<RoomConnectionPopupPresenter>();
		}
		else
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
		}
	}

	private void OnConnectionFailed(int resultCode)
	{
		_popupManager.CloseAll();
		string message;
		switch (resultCode)
		{
		case 3001:
			message = TLocalizedText.GameConnectionFailed.LocalText;
			break;
		case 3002:
			message = TLocalizedText.GameAlreadyStarted.LocalText;
			break;
		case 3004:
			message = TLocalizedText.PlayerLimitExceeded.LocalText;
			break;
		case 3006:
			message = TLocalizedText.CoopVersionMismatch.LocalText;
			break;
		default:
			message = TLocalizedText.NetworkErrorFormat.Format(resultCode);
			break;
		}
		_popupManager.ShowTextPopup(message);
	}
}

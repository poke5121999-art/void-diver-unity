using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class SystemPagePresenter : BasePanelPresenter
{
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

	private CompositeDisposable _disposable = new CompositeDisposable();

	private bool _showGiveUp;

	private bool _showExitLounge;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => InputManager.UiActions;

	private void OnInject(SystemPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_showGiveUp = _gameManager.IsInStage && !_gameContext.IsTutorial;
		_showExitLounge = _gameContext.Players.Count > 1;
		_view.GiveUpButton.gameObject.SetActive(_showGiveUp);
		_view.ExitTheLoungeButton.gameObject.SetActive(_showExitLounge);
		_view.ContinueButton.OnClickWithSfx().Subscribe(delegate
		{
			OnContinueButtonClick();
		}).AddTo(_disposable);
		_view.GiveUpButton.OnClickWithSfx().Subscribe(delegate
		{
			OnGiveUpButtonClick().Forget();
		}).AddTo(_disposable);
		_view.ExitTheLoungeButton.OnClickWithSfx().Subscribe(delegate
		{
			OnExitTheLoungeButtonClick().Forget();
		}).AddTo(_disposable);
		_view.ReturnToTitleButton.OnClickWithSfx().Subscribe(delegate
		{
			ReturnToTitleButtonClick().Forget();
		}).AddTo(_disposable);
		_view.ManualButton.OnClickWithSfx().Subscribe(delegate
		{
			OnManualButtonClick();
		}).AddTo(_disposable);
		_view.CloseGameButton.OnClickWithSfx().Subscribe(delegate
		{
			OnCloseGameButtonClickAsync().Forget();
		}).AddTo(_disposable);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		Selectable selectable = _view.ContinueButton;
		if (_showGiveUp)
		{
			ConnectNav(selectable, ENavDir.Down, _view.GiveUpButton);
			ConnectNav(_view.GiveUpButton, ENavDir.Up, selectable);
			selectable = _view.GiveUpButton;
		}
		if (_showExitLounge)
		{
			ConnectNav(selectable, ENavDir.Down, _view.ExitTheLoungeButton);
			ConnectNav(_view.ExitTheLoungeButton, ENavDir.Up, selectable);
			selectable = _view.ExitTheLoungeButton;
		}
		ConnectNav(selectable, ENavDir.Down, _view.ReturnToTitleButton);
		ConnectNav(_view.ReturnToTitleButton, ENavDir.Up, selectable);
		ConnectNav(_view.ReturnToTitleButton, ENavDir.Down, _view.ManualButton);
		ConnectNav(_view.ManualButton, ENavDir.Up, _view.ReturnToTitleButton);
		ConnectNav(_view.ManualButton, ENavDir.Down, _view.CloseGameButton);
		ConnectNav(_view.CloseGameButton, ENavDir.Up, _view.ManualButton);
		SetDefaultNav(_view.ContinueButton);
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private void OnContinueButtonClick()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private async UniTask OnGiveUpButtonClick()
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.GiveUpDescription.LocalText))
		{
			_gameChannel.SendToHost(new ReqKillCharacter());
			Close();
		}
	}

	private async UniTask ReturnToTitleButtonClick()
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ReturnToTitleDescription.LocalText))
		{
			_gameManager.ClearStageResult();
			_gameChannel.RxConnectState.Value = ERoomConnectState.BackToTitle;
			if (_gameManager.IsHost)
			{
				_gameHost.CloseRoom();
			}
			else
			{
				_gameChannel.LeftRoom();
			}
		}
	}

	private void OnManualButtonClick()
	{
		_popupManager.Open<HelpPopupPresenter>();
	}

	private async UniTask OnExitTheLoungeButtonClick()
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ExitTheLoungeDescription.LocalText))
		{
			GamePlayerData playerData = _playerManager.CreateRoomPlayerData(isHostPlayer: true);
			_gameManager.BackToSinglePlay(playerData, _playerManager.Difficulty, isUnexpectedBackToSinglePlay: false).Forget();
		}
	}

	private async UniTask OnCloseGameButtonClickAsync()
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.CloseGameDescription.LocalText))
		{
			await _playerManager.SaveAsync();
			await _accountRecordManager.SaveAsync();
			AppManager.Quit();
		}
	}
}

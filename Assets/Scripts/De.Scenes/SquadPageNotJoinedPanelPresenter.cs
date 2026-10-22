using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class SquadPageNotJoinedPanelPresenter : BasePanelPresenter
{
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

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(SquadPageNotJoinedPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.CreateRoom.SetActive(model.IsHost);
		_view.CreateRoom_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCreateRoomButtonClickAsync().Forget();
		}).AddTo(_disposable);
		_view.JoinRoom_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnJoinRoomButtonClick();
		}).AddTo(_disposable);
		List<Selectable> items = new List<Selectable>();
		items.Add(_view.JoinRoom_Button);
		items.Add(_view.CreateRoom_Button);
		SetNavHorizontal(items);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitGamePadNav()
	{
		List<Selectable> items = new List<Selectable>();
		items.Add(_view.JoinRoom_Button);
		items.Add(_view.CreateRoom_Button);
		SetNavHorizontal(items);
	}

	private async UniTask OnCreateRoomButtonClickAsync()
	{
		if (!_appManager.IsOnline())
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
			return;
		}
		_gameManager.AssertIfNotHost();
		if (_gameContext.CampaignId > 0)
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnPlaying.LocalText);
			return;
		}
		EPlatform platform = _environmentSettings.Platform;
		if (platform != EPlatform.Local && platform != EPlatform.Steam)
		{
			_popupManager.ShowTextPopup(string.Format("Unsupported platform type. {0}", platform));
			return;
		}
		RoomSettingPopupModel model = new RoomSettingPopupModel
		{
			RoomTitle = TLocalizedText.DefaultLobbyName.LocalText,
			PublishType = ERoomPublishType.Public,
			RoomDifficulty = _playerManager.Difficulty,
			IsCreate = true
		};
		RoomSettingPopupPresenter roomSettingPopupPresenter = await _popupManager.OpenAsync<RoomSettingPopupPresenter>(model);
		if (await roomSettingPopupPresenter.Task is RoomSettingPopupResult roomSettingPopupResult)
		{
			_playerManager.SetDifficulty(roomSettingPopupResult.RoomDifficulty);
			_playerManager.SetLastPlayedRoomDifficulty(roomSettingPopupResult.RoomDifficulty);
			ECreateLobbyResult eCreateLobbyResult = await _gameHost.PublishRoomAsync(roomSettingPopupResult.RoomTitle, roomSettingPopupResult.PlayerCountMax, _playerManager.Name, _playerManager.Level, roomSettingPopupResult.PublishType, roomSettingPopupResult.RoomDifficulty);
			if (eCreateLobbyResult != ECreateLobbyResult.Success)
			{
				_toastManager.NoticeFail(eCreateLobbyResult.ToLocalText());
			}
		}
	}

	private void OnJoinRoomButtonClick()
	{
		if (!_appManager.IsOnline())
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
		}
		else if (_gameContext.CampaignId > 0)
		{
			_toastManager.Notice(TLocalizedText.TryOpenRoomListOnPlaying.LocalText);
		}
		else
		{
			EPlatform platform = _environmentSettings.Platform;
			switch (platform)
			{
			case EPlatform.Local:
				_popupManager.Open<LocalRoomConnectionPopupPresenter>();
				break;
			case EPlatform.Steam:
				_popupManager.Open<RoomListPopupPresenter>();
				break;
			default:
				_popupManager.ShowTextPopup(string.Format("Unsupported platform type. {0}", platform));
				break;
			}
		}
	}
}

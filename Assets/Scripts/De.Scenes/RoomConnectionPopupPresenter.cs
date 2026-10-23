using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class RoomConnectionPopupPresenter : GamePopupPresenter
{
	[View]
	private RoomConnectionPopupView _view;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private PlayerManager _playerManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.BtnCancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.BtnConnect.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConnectButtonClickAsync().Forget();
		}).AddTo(this);
		_view.InputField.GetComponent<InputFieldController>().SetDescription(string.Empty);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.InputField, ENavDir.Down, _view.BtnConnect);
		ConnectNav(_view.BtnConnect, ENavDir.Up, _view.InputField);
		ConnectNav(_view.BtnConnect, ENavDir.Right, _view.BtnCancel);
		ConnectNav(_view.BtnCancel, ENavDir.Up, _view.InputField);
		ConnectNav(_view.BtnCancel, ENavDir.Left, _view.BtnConnect);
		SetDefaultNav(_view.BtnConnect);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private async UniTask OnConnectButtonClickAsync()
	{
		if (!_appManager.IsOnline())
		{
			_toastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
			return;
		}
		string text = _view.InputField.text;
		if (string.IsNullOrEmpty(text) || !ulong.TryParse(text, out var result))
		{
			_toastManager.NoticeFail(TLocalizedText.SteamInvalidLobbyId.Format(text));
			return;
		}
		_popupManager.CloseAll();
		GamePlayerData playerData = _playerManager.CreateRoomPlayerData(false);
		int num = await _gameChannel.ConnectAndJoinBySteamLobbyAsync(result, playerData);
		if (num != 0)
		{
			_gameManager.CachedConnectionResultCode = num;
			_gameChannel.RxConnectState.Value = ERoomConnectState.BackToLocalRoom;
			await _gameChannel.ShutdownAsync();
			await _gameChannel.RecreateTransportAsync();
			_gameHost.CreateRoom(_playerManager.Difficulty);
			await _gameChannel.TryJoinGame(_playerManager.CreateRoomPlayerData(true));
			_gameManager.LoadGame();
		}
	}
}

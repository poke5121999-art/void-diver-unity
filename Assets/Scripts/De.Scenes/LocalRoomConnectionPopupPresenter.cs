using System;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using VContainer;

public class LocalRoomConnectionPopupPresenter : GamePopupPresenter
{
	[View]
	private LocalRoomConnectionPopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.BtnCancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancel();
		}).AddTo(this);
		_view.BtnConnect.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConnectButtonClickAsync().Forget();
		}).AddTo(this);
		_view.InputFieldIp.text = "127.0.0.1";
		_view.InputFieldPort.text = "7777";
		_view.InputFieldIp.GetComponent<InputFieldController>().SetDescription(string.Empty);
		_view.InputFieldPort.GetComponent<InputFieldController>().SetDescription(string.Empty);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.InputFieldIp, ENavDir.Down, _view.InputFieldPort);
		ConnectNav(_view.InputFieldPort, ENavDir.Up, _view.InputFieldIp);
		ConnectNav(_view.InputFieldPort, ENavDir.Down, _view.BtnConnect);
		ConnectNav(_view.BtnConnect, ENavDir.Up, _view.InputFieldPort);
		ConnectNav(_view.BtnConnect, ENavDir.Right, _view.BtnCancel);
		ConnectNav(_view.BtnCancel, ENavDir.Up, _view.InputFieldPort);
		ConnectNav(_view.BtnCancel, ENavDir.Left, _view.BtnConnect);
		SetDefaultNav(_view.InputFieldIp);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancel();
		}).AddTo(InputDisposable);
	}

	private async UniTask OnConnectButtonClickAsync()
	{
		string text = _view.InputFieldIp.text;
		string text2 = _view.InputFieldPort.text;
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
		{
			_popupManager.ShowConnectionFailPopup(3001);
			return;
		}
		if (!ushort.TryParse(text2, out var result))
		{
			_popupManager.ShowConnectionFailPopup(3001);
			return;
		}
		_popupManager.CloseAll(Array.Empty<Type>());
		if (_networkManager.NetworkConfig.NetworkTransport is UnityTransport unityTransport)
		{
			string address = unityTransport.ConnectionData.Address;
			ushort port = unityTransport.ConnectionData.Port;
			GamePlayerData playerData = _playerManager.CreateRoomPlayerData(false);
			int num = await _gameChannel.ConnectAndJoinByAddressAsync(text, result, playerData);
			if (num != 0)
			{
				_gameManager.CachedConnectionResultCode = num;
				_gameChannel.RxConnectState.Value = ERoomConnectState.BackToLocalRoom;
				await _gameChannel.ShutdownAsync();
				_gameHost.SetUp(address, port);
				_gameHost.CreateRoom(_playerManager.Difficulty);
				await _gameChannel.TryJoinGame(_playerManager.CreateRoomPlayerData(true));
			}
			_gameManager.LoadGame();
		}
	}

	private void OnCancel()
	{
		Close();
	}
}

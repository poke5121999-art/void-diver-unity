using Cysharp.Threading.Tasks;
using GameCommon.Utility;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class LocalRoomCreationPopupPresenter : GamePopupPresenter
{
	[View]
	private LocalRoomCreationPopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.BtnBack_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancel();
		}).AddTo(this);
		_view.BtnConfirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCreateButtonClickAsync().Forget();
		}).AddTo(this);
		_view.InputFieldIp.text = "127.0.0.1";
		_view.InputFieldPort.text = "7777";
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.InputFieldIp, ENavDir.Down, _view.InputFieldPort);
		ConnectNav(_view.InputFieldPort, ENavDir.Up, _view.InputFieldIp);
		ConnectNav(_view.InputFieldPort, ENavDir.Down, _view.BtnConfirm_Button);
		ConnectNav(_view.BtnConfirm_Button, ENavDir.Up, _view.InputFieldPort);
		ConnectNav(_view.BtnConfirm_Button, ENavDir.Right, _view.BtnBack_Button);
		ConnectNav(_view.BtnBack_Button, ENavDir.Up, _view.InputFieldPort);
		ConnectNav(_view.BtnBack_Button, ENavDir.Left, _view.BtnConfirm_Button);
		SetDefaultNav(_view.BtnConfirm_Button);
	}

	private async UniTask OnCreateButtonClickAsync()
	{
		string text = _view.InputFieldIp.text;
		string text2 = _view.InputFieldPort.text;
		if (!NetworkUtility.ValidateAddress(text, text2))
		{
			_popupManager.ShowTextPopup("Please enter a valid address.");
			return;
		}
		_gameHost.SetUp(text, ushort.Parse(text2));
		_gameHost.CreateRoom(_playerManager.Difficulty);
		await _gameChannel.TryJoinGame(_playerManager.CreateRoomPlayerData(true));
		_gameManager.LoadGame();
		Close();
	}

	private void OnCancel()
	{
		Close();
	}
}

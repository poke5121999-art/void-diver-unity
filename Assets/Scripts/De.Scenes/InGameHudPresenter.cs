using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class InGameHudPresenter : Presenter
{
	[View]
	private InGameHudView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameMapManager _mapManager;

	private void Start()
	{
		_gameManager.RxActiveInGameHud.Subscribe(delegate(bool active)
		{
			_view.gameObject.SetActive(active);
		}).AddTo(this);
		_inputController.RxIsPingPressed.Subscribe(OnPingPressed).AddTo(this);
		_inputController.RxIsEmojiPressed.Subscribe(OnEmojiPressed).AddTo(this);
		_pingManager.OnPingTriggered.Subscribe(OnPingTriggered).AddTo(this);
	}

	private void OnPingPressed(bool isPressed)
	{
		if (isPressed)
		{
			_view.PingEmojiPanel.Open(EPingEmojiMode.Ping, _inputController.GetAimScreenPoint());
		}
	}

	private void OnEmojiPressed(bool isPressed)
	{
		if (isPressed)
		{
			_view.PingEmojiPanel.Open(EPingEmojiMode.Emoji, _inputController.GetAimScreenPoint());
		}
	}

	private void OnPingTriggered((EPingType pingType, Vector2 inputPosition) data)
	{
		if (!_mapManager.RxIsFullScreen.Value)
		{
			EPingResult ePingResult = _pingManager.TrySendPingByScreenPoint(data.pingType, data.inputPosition);
			if (ePingResult != EPingResult.Ok)
			{
				_toastManager.Notice(ePingResult.ToLocalText());
			}
		}
	}
}

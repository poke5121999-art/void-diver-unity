using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class PingEmojiPanelPresenter : Presenter
{
	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameInputManager _inputManager;

	[SerializeField]
	private PingPanelPresenter _pingPanel;

	[SerializeField]
	private EmojiPanelPresenter _emojiPanel;

	private EPingEmojiMode _currentMode;

	private Vector2 _cachedPadScreenPoint;

	private float _openTime;

	private const float _longPressThreshold = 0.3f;

	private bool _isOpen;

	private CompositeDisposable _disposable;

	private float _lastPingShortPressTime = float.MinValue;

	private Vector2 _pendingPingScreenPoint;

	private CancellationTokenSource _pendingPingCts;

	private const float _doubleClickThreshold = 0.4f;

	private bool IsGamePad => InputManager.IsGamePad;

	public void Open(EPingEmojiMode mode, Vector2 cachedPadScreenPoint)
	{
		if (!_isOpen)
		{
			_isOpen = true;
			_openTime = Time.time;
			_cachedPadScreenPoint = cachedPadScreenPoint;
			base.gameObject.SetActive(value: true);
			_disposable?.Dispose();
			_disposable = new CompositeDisposable();
			_inputController.RxIsPingPressed.Skip(1).Subscribe(OnPingPressedChanged).AddTo(_disposable);
			_inputController.RxIsEmojiPressed.Skip(1).Subscribe(OnEmojiPressedChanged).AddTo(_disposable);
			ActivatePanel(mode);
		}
	}

	public void Close()
	{
		if (_isOpen)
		{
			_isOpen = false;
			GetActivePanel().Cancel();
			_disposable?.Dispose();
			_disposable = null;
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		if (_isOpen)
		{
			_isOpen = false;
			_disposable?.Dispose();
			_disposable = null;
		}
	}

	private void ActivatePanel(EPingEmojiMode mode)
	{
		_currentMode = mode;
		_pingPanel.gameObject.SetActive(mode == EPingEmojiMode.Ping);
		_emojiPanel.gameObject.SetActive(mode == EPingEmojiMode.Emoji);
		GetActivePanel().Open(_cachedPadScreenPoint);
	}

	private void SwapPanel()
	{
		GetActivePanel().Cancel();
		ActivatePanel((_currentMode == EPingEmojiMode.Ping) ? EPingEmojiMode.Emoji : EPingEmojiMode.Ping);
		_openTime = Time.time;
	}

	private IWheelMenuPanel GetActivePanel()
	{
		if (_currentMode != EPingEmojiMode.Ping)
		{
			return _emojiPanel;
		}
		return _pingPanel;
	}

	private void OnPingPressedChanged(bool isPressed)
	{
		if (_currentMode == EPingEmojiMode.Ping)
		{
			if (!isPressed && _inputController.CompositeInputState == ECompositeInputState.None)
			{
				if (IsGamePad)
				{
					HandleGamePadPingRelease();
				}
				Close();
			}
		}
		else if (isPressed)
		{
			SwapPanel();
		}
	}

	private void OnEmojiPressedChanged(bool isPressed)
	{
		if (_currentMode == EPingEmojiMode.Emoji)
		{
			if (!isPressed && _inputController.CompositeInputState == ECompositeInputState.None)
			{
				if (IsGamePad)
				{
					HandleGamePadEmojiRelease();
				}
				Close();
			}
		}
		else if (isPressed)
		{
			SwapPanel();
		}
	}

	private void HandleGamePadPingRelease()
	{
		EPingType selectedPingType = _pingPanel.SelectedPingType;
		if (Time.time - _openTime < 0.3f)
		{
			if (Time.time - _lastPingShortPressTime >= 0.4f)
			{
				_lastPingShortPressTime = Time.time;
				_pendingPingScreenPoint = _cachedPadScreenPoint;
				_pendingPingCts?.Cancel();
				_pendingPingCts?.Dispose();
				_pendingPingCts = new CancellationTokenSource();
				FirePingAfterDelayAsync(EPingType.Notice, _pendingPingScreenPoint, _pendingPingCts.Token).Forget();
			}
			else
			{
				_pendingPingCts?.Cancel();
				_pendingPingCts?.Dispose();
				_pendingPingCts = null;
				_lastPingShortPressTime = float.MinValue;
				_pingManager.TriggerPing(EPingType.Alert, _cachedPadScreenPoint);
			}
			return;
		}
		_lastPingShortPressTime = float.MinValue;
		_pendingPingCts?.Cancel();
		_pendingPingCts?.Dispose();
		_pendingPingCts = null;
		if (selectedPingType == EPingType.Cancel || selectedPingType == EPingType.None)
		{
			_toastManager.Notice(TLocalizedText.PingCancelled.LocalText);
		}
		else
		{
			_pingManager.TriggerPing(selectedPingType, _cachedPadScreenPoint);
		}
	}

	private async UniTaskVoid FirePingAfterDelayAsync(EPingType pingType, Vector2 screenPoint, CancellationToken ct)
	{
		await UniTask.Delay(TimeSpan.FromSeconds(0.4), ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
		_pendingPingCts?.Dispose();
		_pendingPingCts = null;
		_pingManager.TriggerPing(pingType, screenPoint);
	}

	private void HandleGamePadEmojiRelease()
	{
		if (Time.time - _openTime < 0.3f || _emojiPanel.SelectedIndex < 0)
		{
			_toastManager.Notice(TLocalizedText.EmojiCancelled.LocalText);
		}
		else
		{
			_gameChannel.SendToHost(new ReqEmoji
			{
				EmojiId = _emojiPanel.SelectedIndex
			});
		}
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
		_pendingPingCts?.Cancel();
		_pendingPingCts = null;
	}
}

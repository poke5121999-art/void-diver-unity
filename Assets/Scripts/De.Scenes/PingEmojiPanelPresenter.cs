using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class PingEmojiPanelPresenter : Presenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CFirePingAfterDelayAsync_003Ed__28 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public CancellationToken ct;

		public PingEmojiPanelPresenter _003C_003E4__this;

		public EPingType pingType;

		public Vector2 screenPoint;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

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

	private float _lastPingShortPressTime;

	private Vector2 _pendingPingScreenPoint;

	private CancellationTokenSource _pendingPingCts;

	private const float _doubleClickThreshold = 0.4f;

	private bool IsGamePad => false;

	public void Open(EPingEmojiMode mode, Vector2 cachedPadScreenPoint)
	{
	}

	public void Close()
	{
	}

	private void OnDisable()
	{
	}

	private void ActivatePanel(EPingEmojiMode mode)
	{
	}

	private void SwapPanel()
	{
	}

	private IWheelMenuPanel GetActivePanel()
	{
		return null;
	}

	private void OnPingPressedChanged(bool isPressed)
	{
	}

	private void OnEmojiPressedChanged(bool isPressed)
	{
	}

	private void HandleGamePadPingRelease()
	{
	}

	[AsyncStateMachine(typeof(_003CFirePingAfterDelayAsync_003Ed__28))]
	private UniTaskVoid FirePingAfterDelayAsync(EPingType pingType, Vector2 screenPoint, CancellationToken ct)
	{
		return default;
	}

	private void HandleGamePadEmojiRelease()
	{
	}

	private void OnDestroy()
	{
	}
}

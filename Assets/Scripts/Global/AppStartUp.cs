using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using NemoLib.Popup;
using UniRx;
using VContainer;
using VContainer.Unity;

public class AppStartUp : NativeBase, IStartable
{
	private struct POINT
	{
		public int x;

		public int y;
	}

	private struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct MONITORINFO
	{
		public int cbSize;

		public RECT rcMonitor;

		public RECT rcWork;

		public uint dwFlags;
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRunShutdownSequenceAsync_003Ed__50 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public AppStartUp _003C_003E4__this;

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
	private AnalyticsController _analyticsController;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AccountRecordManager _accountRecordManager;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private PlayerInputController _playerInputController;

	[Inject]
	private GameObjectManager _gameObjectManager;

	[Inject]
	private GameSoundManager _gameSoundManager;

	[Inject]
	private GameVfxManager _gameVfxManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameFloatingTextManager _floatingTextManager;

	[Inject]
	private GameCameraController _cameraController;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameStageManager _stageManager;

	[Inject]
	private GameShopManager _shopManager;

	[Inject]
	private CursorManager _cursorManager;

	[Inject]
	private InputIconsManager _inputIconsManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private CompositeDisposable _disposable;

	private Tween _blurTween;

	private const uint MONITOR_DEFAULTTOPRIMARY = 1u;

	private const uint SWP_NOSIZE = 1u;

	private const uint SWP_NOZORDER = 4u;

	private bool _readyToQuit;

	private static bool _forceQuitScheduled;

	private float BlurEffectDuration => 0f;

	private CameraPostProcessingController BlurSlider => null;

	private float BlurValue
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public void Start()
	{
	}

	private void OnPopupOpened()
	{
	}

	private void OnPopupClosed()
	{
	}

	private void DoTweenBlurValue()
	{
	}

	[PreserveSig]
	private static extern IntPtr GetActiveWindow();

	[PreserveSig]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

	[PreserveSig]
	private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

	[PreserveSig]
	private static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

	[PreserveSig]
	private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

	private void MoveWindowToPrimaryMonitor()
	{
	}

	[PreserveSig]
	private static extern int TerminateProcess(IntPtr hProcess, uint exitCode);

	private bool OnWantsToQuit()
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CRunShutdownSequenceAsync_003Ed__50))]
	private UniTaskVoid RunShutdownSequenceAsync()
	{
		return default;
	}

	private void ScheduleForceQuit(float delaySeconds)
	{
	}
}

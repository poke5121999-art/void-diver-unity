using System;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using DG.Tweening;
using NemoLib.Popup;
using UniRx;
using VContainer;
using VContainer.Unity;
using UnityEngine;

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

	private float BlurEffectDuration => 0.3f;

	private CameraPostProcessingController BlurSlider => _cameraManager.GetComponent<CameraPostProcessingController>();

	private float BlurValue
	{
		get
		{
			return BlurSlider.ScreenBlurIntensity;
		}
		set
		{
			BlurSlider.ScreenBlurIntensity = value;
		}
	}

	public void Start()
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_inputIconsManager.Initialize();
		_popupManager.OnBeforeOpenPopup.Subscribe(delegate
		{
			DoTweenBlurValue();
		}).AddTo(_disposable);
		_popupManager.OnPopupClosed.Subscribe(delegate
		{
			DoTweenBlurValue();
		}).AddTo(_disposable);
		_popupManager.OnBlurEffectChanged.Subscribe(delegate
		{
			DoTweenBlurValue();
		}).AddTo(_disposable);
		if (_environmentSettings.Platform == EPlatform.Steam)
		{
			SteamManager.Initialize();
		}
		Application.wantsToQuit += OnWantsToQuit;
		MoveWindowToPrimaryMonitor();
	}

	private void OnPopupOpened()
	{
		DoTweenBlurValue();
	}

	private void OnPopupClosed()
	{
		DoTweenBlurValue();
	}

	private void DoTweenBlurValue()
	{
		float endValue = (_popupManager.FindAllPopup<BasePopupPresenter>().Any((BasePopupPresenter p) => p.IsBlurEffect) ? 1f : 0f);
		if (_blurTween != null)
		{
			_blurTween.Kill();
		}
		_blurTween = DOVirtual.Float(BlurValue, endValue, BlurEffectDuration, delegate(float v)
		{
			BlurValue = v;
		}).SetEase(Ease.OutQuart).Play();
	}

	private static bool IsWindows => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;

	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

	[DllImport("user32.dll")]
	private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

	[DllImport("user32.dll")]
	private static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

	private void MoveWindowToPrimaryMonitor()
	{
		// Port guard, not in the decomp: user32.dll exists only on Windows (DECODING trap 52).
		if (!IsWindows || _graphicSettings.ScreenMode == EScreenMode.Fullscreen)
		{
			return;
		}
		IntPtr activeWindow = GetActiveWindow();
		if (activeWindow == IntPtr.Zero)
		{
			return;
		}
		IntPtr intPtr = MonitorFromPoint(default(POINT), MONITOR_DEFAULTTOPRIMARY);
		if (MonitorFromWindow(activeWindow, MONITOR_DEFAULTTOPRIMARY) != intPtr)
		{
			MONITORINFO lpmi = default(MONITORINFO);
			lpmi.cbSize = Marshal.SizeOf<MONITORINFO>();
			if (GetMonitorInfo(intPtr, ref lpmi))
			{
				RECT rcWork = lpmi.rcWork;
				int x = rcWork.left + (rcWork.right - rcWork.left - Screen.width) / 2;
				int y = rcWork.top + (rcWork.bottom - rcWork.top - Screen.height) / 2;
				SetWindowPos(activeWindow, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
			}
		}
	}

	[DllImport("kernel32.dll")]
	private static extern int TerminateProcess(IntPtr hProcess, uint exitCode);

	private bool OnWantsToQuit()
	{
		if (_readyToQuit)
		{
			return true;
		}
		RunShutdownSequenceAsync();
		return false;
	}

	private async UniTaskVoid RunShutdownSequenceAsync()
	{
		ScheduleForceQuit(8f);
		try
		{
			_playerManager.BackupPlayer(DateTime.UtcNow.Ticks);
			await _gameChannel.ShutdownAsync();
			// interface calls @0x1828ae040 use interface method indices 0xd (bool) and 2 (void) = IRoomMatchmaker.get_IsAlreadyJoined and LeaveLobby (metadata method order checked)
			if (_roomMatchmaker != null && _roomMatchmaker.IsAlreadyJoined)
			{
				_roomMatchmaker.LeaveLobby();
			}
			_playerManager.Save(force: true);
			_accountRecordManager.Save();
			SteamManager.ShutdownSteamApi();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
		_readyToQuit = true;
		Application.Quit();
	}

	private void ScheduleForceQuit(float delaySeconds)
	{
		if (!_forceQuitScheduled)
		{
			_forceQuitScheduled = true;
			ThreadPool.QueueUserWorkItem(delegate
			{
				Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
				// Port guard (DECODING trap 52): kernel32.dll exists only on Windows; Kill is the same hard stop elsewhere.
				if (IsWindows)
				{
					TerminateProcess(System.Diagnostics.Process.GetCurrentProcess().Handle, 0u);
				}
				else
				{
					System.Diagnostics.Process.GetCurrentProcess().Kill();
				}
			});
		}
	}
}

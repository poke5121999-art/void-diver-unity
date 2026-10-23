using System;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GraphicSettingService : NativeBase
{
	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private GraphicOptionController _graphicOptionController;

	[Inject]
	private CameraManager _cameraManager;

	private const int BackgroundFrameRate = 60;

	private readonly CompositeDisposable _disposable = new CompositeDisposable();

	private GraphicSettingContext _context;

	private bool _dirty;

	private bool _gameplayVSyncActive;

	private const int GWL_STYLE = -16;

	private const int WS_CAPTION = 12582912;

	private const int WS_THICKFRAME = 262144;

	private const int WS_MINIMIZEBOX = 131072;

	private const int WS_MAXIMIZEBOX = 65536;

	private const int WS_SYSMENU = 524288;

	private const uint SWP_NOMOVE = 2u;

	private const uint SWP_NOSIZE = 1u;

	private const uint SWP_NOZORDER = 4u;

	private const uint SWP_FRAMECHANGED = 32u;

	private string FileName => "GraphicSettingContext";

	private string AccountId => (_appManager != null && _appManager.AccountId.HasValue()) ? _appManager.AccountId : string.Empty;

	public EScreenMode ScreenMode => (_context != null) ? _context.RxScreenMode.Value : EScreenMode.Windowed;

	public EResolution Resolution => (_context != null) ? _context.RxResolution.Value : EResolution._1920x1080;

	public EQualityLevel QualityLevel => (_context != null) ? _context.RxQualityLevel.Value : EQualityLevel.Ultra;

	public EFrameRate FrameRate => (_context != null) ? _context.RxFrameRate.Value : EFrameRate.Frame60;

	public bool VSyncOn => _context != null && _context.RxVSyncOn.Value;

	public bool CameraShakingOn => _context == null || _context.RxCameraShakingOn.Value;

	public int GammaPercent => (_context != null) ? _context.RxGammaPercent.Value : 50;

	public IReadOnlyReactiveProperty<EScreenMode> RxScreenMode => _context?.RxScreenMode;

	public IReadOnlyReactiveProperty<EResolution> RxResolution => _context?.RxResolution;

	public IReadOnlyReactiveProperty<EQualityLevel> RxQualityLevel => _context?.RxQualityLevel;

	public IReadOnlyReactiveProperty<EFrameRate> RxFrameRate => _context?.RxFrameRate;

	public IReadOnlyReactiveProperty<bool> RxVSyncOn => _context?.RxVSyncOn;

	public IReadOnlyReactiveProperty<bool> RxCameraShakingOn => _context?.RxCameraShakingOn;

	public IReadOnlyReactiveProperty<int> RxGammaPercent => _context?.RxGammaPercent;

	public async UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_context != null)
		{
			return;
		}
		StorageLoadResult<GraphicSettingContextData> result = await _saveStorage.LoadFromLocalAsync<GraphicSettingContextData>(FileName, cancellationToken);
		if (result.LoadResult != ELoadResult.Success)
		{
			_context = CreateDefault();
			await SaveAsync(cancellationToken);
		}
		else
		{
			_context = new GraphicSettingContext(result.Data);
		}
		_context.RxScreenMode.Subscribe(OnUpdateScreenMode).AddTo(_disposable);
		_context.RxResolution.Subscribe(delegate
		{
			OnUpdateScreenMode((_context != null) ? _context.RxScreenMode.Value : EScreenMode.Windowed);
		}).AddTo(_disposable);
		_context.RxQualityLevel.Subscribe(OnUpdateQuality).AddTo(_disposable);
		_context.RxFrameRate.Subscribe(OnUpdateFrameRate).AddTo(_disposable);
		_context.RxVSyncOn.Subscribe(OnUpdateVSync).AddTo(_disposable);
		_context.RxGammaPercent.Subscribe(OnUpdateGamma).AddTo(_disposable);
		_context.RxCameraShakingOn.Subscribe(OnUpdateCameraShaking).AddTo(_disposable);
		Observable.EveryApplicationFocus().Subscribe(OnApplicationFocusChanged).AddTo(_disposable);
	}

	private GraphicSettingContext CreateDefault()
	{
		GraphicSettingContextData data = new GraphicSettingContextData
		{
			AccountId = ((_appManager != null && _appManager.AccountId.HasValue()) ? _appManager.AccountId : string.Empty),
			ScreenMode = EScreenMode.Fullscreen,
			Resolution = GetDefaultResolution(),
			QualityLevel = EQualityLevel.High,
			FrameRate = EFrameRate.Frame60,
			VSyncOn = true,
			CameraShakingOn = true,
			GammaPercent = 0
		};
		return new GraphicSettingContext(data);
	}

	private static EResolution GetDefaultResolution()
	{
		int systemWidth = Display.main.systemWidth;
		int systemHeight = Display.main.systemHeight;
		EResolution[] array = new EResolution[6]
		{
			EResolution._2560x1440,
			EResolution._1920x1080,
			EResolution._1600x900,
			EResolution._1366x768,
			EResolution._1360x768,
			EResolution._1280x720
		};
		for (int i = 0; i < array.Length; i++)
		{
			EResolution eResolution = array[i];
			if (eResolution.ToWidth() <= systemWidth && eResolution.ToHeight() <= systemHeight)
			{
				return eResolution;
			}
		}
		return EResolution._1280x720;
	}

	public void Reset()
	{
		if (_context != null)
		{
			GraphicSettingContextData graphicSettingContextData = CreateDefault().ToData();
			_context.RxScreenMode.Value = graphicSettingContextData.ScreenMode;
			_context.RxResolution.Value = graphicSettingContextData.Resolution;
			_context.RxQualityLevel.Value = graphicSettingContextData.QualityLevel;
			_context.RxFrameRate.Value = graphicSettingContextData.FrameRate;
			_context.RxVSyncOn.Value = graphicSettingContextData.VSyncOn;
			_context.RxCameraShakingOn.Value = graphicSettingContextData.CameraShakingOn;
			_context.RxGammaPercent.Value = graphicSettingContextData.GammaPercent;
			_dirty = true;
		}
	}

	public void Flush()
	{
		if (_context != null && _dirty)
		{
			_dirty = false;
			SaveAsync().Forget();
		}
	}

	private async UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		_context.AccountId = AccountId;
		await _saveStorage.SaveToLocalAsync(FileName, _context.ToData(), cancellationToken);
	}

	public void SetScreenMode(EScreenMode screenMode)
	{
		if (_context.RxScreenMode.Value != screenMode)
		{
			_context.RxScreenMode.Value = screenMode;
			_dirty = true;
		}
	}

	public void SetResolution(EResolution resolution)
	{
		if (_context.RxResolution.Value != resolution)
		{
			_context.RxResolution.Value = resolution;
			_dirty = true;
		}
	}

	public void SetQualityLevel(EQualityLevel qualityLevel)
	{
		if (_context.RxQualityLevel.Value != qualityLevel)
		{
			_context.RxQualityLevel.Value = qualityLevel;
			_dirty = true;
		}
	}

	public void SetFrameRate(EFrameRate frameRate)
	{
		if (_context.RxFrameRate.Value != frameRate)
		{
			_context.RxFrameRate.Value = frameRate;
			_dirty = true;
		}
	}

	public void SetVSyncOn(bool vsyncOn)
	{
		if (_context.RxVSyncOn.Value != vsyncOn)
		{
			_context.RxVSyncOn.Value = vsyncOn;
			_dirty = true;
		}
	}

	public void SetCameraShakingOn(bool cameraShakingOn)
	{
		if (_context.RxCameraShakingOn.Value != cameraShakingOn)
		{
			_context.RxCameraShakingOn.Value = cameraShakingOn;
			_dirty = true;
		}
	}

	public void SetGamma(int gammaPercent)
	{
		if (_context.RxGammaPercent.Value != gammaPercent)
		{
			_context.RxGammaPercent.Value = gammaPercent;
			_dirty = true;
		}
	}

	public void SetGameplayVSyncActive(bool active)
	{
		if (_gameplayVSyncActive != active)
		{
			_gameplayVSyncActive = active;
			OnUpdateVSync(_context != null && _context.RxVSyncOn.Value);
		}
	}

	private void OnUpdateScreenMode(EScreenMode screenMode)
	{
		int width = ((_context != null) ? _context.RxResolution.Value : EResolution._1920x1080).ToWidth();
		int height = ((_context != null) ? _context.RxResolution.Value : EResolution._1920x1080).ToHeight();
		FullScreenMode fullScreenMode = GetFullScreenMode(screenMode, width, height);
		Screen.SetResolution(width, height, fullScreenMode);
		OnUpdateVSync(_context != null && _context.RxVSyncOn.Value);
		OnUpdateFrameRate((_context != null) ? _context.RxFrameRate.Value : EFrameRate.Frame60);
		if (fullScreenMode != FullScreenMode.FullScreenWindow)
		{
			ApplyWindowBorderDeferredAsync(screenMode, width, height).Forget();
		}
		static FullScreenMode GetFullScreenMode(EScreenMode mode, int width, int height)
		{
			switch (mode)
			{
			case EScreenMode.BorderlessWindowed:
				if (width < Display.main.systemWidth || height < Display.main.systemHeight)
				{
					return FullScreenMode.Windowed;
				}
				return FullScreenMode.FullScreenWindow;
			case EScreenMode.Fullscreen:
				return FullScreenMode.ExclusiveFullScreen;
			default:
				return FullScreenMode.Windowed;
			}
		}
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	private static async UniTask ApplyWindowBorderDeferredAsync(EScreenMode screenMode, int width, int height)
	{
		for (int i = 0; i < 30 && (Screen.width != width || Screen.height != height); i++)
		{
			await UniTask.NextFrame();
		}
		await UniTask.NextFrame();
		ApplyWindowBorder(screenMode);
	}

	private static void ApplyWindowBorder(EScreenMode mode)
	{
		if (mode != EScreenMode.Fullscreen)
		{
			IntPtr activeWindow = GetActiveWindow();
			if (activeWindow != IntPtr.Zero)
			{
				int windowLong = GetWindowLong(activeWindow, -16);
				int num = ((mode != EScreenMode.BorderlessWindowed) ? ((windowLong & -327681) | 0xCA0000) : (windowLong & -13500417));
				if (num != windowLong)
				{
					SetWindowLong(activeWindow, -16, num);
					SetWindowPos(activeWindow, IntPtr.Zero, 0, 0, 0, 0, 39u);
				}
			}
		}
	}

	private void OnUpdateQuality(EQualityLevel qualityLevel)
	{
		QualitySettings.SetQualityLevel((int)qualityLevel);
		OnUpdateVSync(_context != null && _context.RxVSyncOn.Value);
		OnUpdateFrameRate((_context != null) ? _context.RxFrameRate.Value : EFrameRate.Frame60);
	}

	private void OnUpdateFrameRate(EFrameRate frameRate)
	{
		Application.targetFrameRate = frameRate.ToFrameRate();
	}

	private void OnUpdateVSync(bool vsyncOn)
	{
		int num = ((vsyncOn && _gameplayVSyncActive) ? GetVSyncCount() : 0);
		if (QualitySettings.vSyncCount != num)
		{
			QualitySettings.vSyncCount = num;
		}
	}

	private static int GetVSyncCount()
	{
		int num = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
		if (num < 91)
		{
			return 1;
		}
		if (num < 145)
		{
			return 2;
		}
		return (num > 200) ? 4 : 3;
	}

	private void OnApplicationFocusChanged(bool hasFocus)
	{
		if (!hasFocus)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;
			return;
		}
		OnUpdateVSync(_context != null && _context.RxVSyncOn.Value);
		OnUpdateFrameRate((_context != null) ? _context.RxFrameRate.Value : EFrameRate.Frame60);
	}

	private void OnUpdateGamma(int gammaPercent)
	{
		_graphicOptionController.SetGammaDegree((float)gammaPercent / 100f);
	}

	private void OnUpdateCameraShaking(bool cameraShakingOn)
	{
		if (!(_cameraManager == null))
		{
			_cameraManager.SetCameraShakeEnabled(cameraShakingOn);
		}
	}
}

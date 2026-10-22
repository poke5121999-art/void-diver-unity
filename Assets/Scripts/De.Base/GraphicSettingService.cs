using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GraphicSettingService : NativeBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CApplyWindowBorderDeferredAsync_003Ed__71 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public int width;

		public int height;

		public EScreenMode screenMode;

		private int _003Ci_003E5__2;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitAsync_003Ed__42 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GraphicSettingService _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask<StorageLoadResult<GraphicSettingContextData>>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSaveAsync_003Ed__47 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GraphicSettingService _003C_003E4__this;

		public CancellationToken cancellationToken;

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

	private readonly CompositeDisposable _disposable;

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

	private string FileName => null;

	private string AccountId => null;

	public EScreenMode ScreenMode => EScreenMode.Windowed;

	public EResolution Resolution => EResolution.Native;

	public EQualityLevel QualityLevel => EQualityLevel.Low;

	public EFrameRate FrameRate => EFrameRate.Frame15;

	public bool VSyncOn => false;

	public bool CameraShakingOn => false;

	public int GammaPercent => 0;

	public IReadOnlyReactiveProperty<EScreenMode> RxScreenMode => null;

	public IReadOnlyReactiveProperty<EResolution> RxResolution => null;

	public IReadOnlyReactiveProperty<EQualityLevel> RxQualityLevel => null;

	public IReadOnlyReactiveProperty<EFrameRate> RxFrameRate => null;

	public IReadOnlyReactiveProperty<bool> RxVSyncOn => null;

	public IReadOnlyReactiveProperty<bool> RxCameraShakingOn => null;

	public IReadOnlyReactiveProperty<int> RxGammaPercent => null;

	[AsyncStateMachine(typeof(_003CInitAsync_003Ed__42))]
	public UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private GraphicSettingContext CreateDefault()
	{
		return null;
	}

	private static EResolution GetDefaultResolution()
	{
		return EResolution.Native;
	}

	public void Reset()
	{
	}

	public void Flush()
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__47))]
	private UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void SetScreenMode(EScreenMode screenMode)
	{
	}

	public void SetResolution(EResolution resolution)
	{
	}

	public void SetQualityLevel(EQualityLevel qualityLevel)
	{
	}

	public void SetFrameRate(EFrameRate frameRate)
	{
	}

	public void SetVSyncOn(bool vsyncOn)
	{
	}

	public void SetCameraShakingOn(bool cameraShakingOn)
	{
	}

	public void SetGamma(int gammaPercent)
	{
	}

	public void SetGameplayVSyncActive(bool active)
	{
	}

	private void OnUpdateScreenMode(EScreenMode screenMode)
	{
	}

	[PreserveSig]
	private static extern IntPtr GetActiveWindow();

	[PreserveSig]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[PreserveSig]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[PreserveSig]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	[AsyncStateMachine(typeof(_003CApplyWindowBorderDeferredAsync_003Ed__71))]
	private static UniTask ApplyWindowBorderDeferredAsync(EScreenMode screenMode, int width, int height)
	{
		return default;
	}

	private static void ApplyWindowBorder(EScreenMode mode)
	{
	}

	private void OnUpdateQuality(EQualityLevel qualityLevel)
	{
	}

	private void OnUpdateFrameRate(EFrameRate frameRate)
	{
	}

	private void OnUpdateVSync(bool vsyncOn)
	{
	}

	private static int GetVSyncCount()
	{
		return 0;
	}

	private void OnApplicationFocusChanged(bool hasFocus)
	{
	}

	private void OnUpdateGamma(int gammaPercent)
	{
	}

	private void OnUpdateCameraShaking(bool cameraShakingOn)
	{
	}
}

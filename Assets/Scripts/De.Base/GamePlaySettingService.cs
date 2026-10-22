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

public class GamePlaySettingService : NativeBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitAsync_003Ed__48 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GamePlaySettingService _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask<StorageLoadResult<GamePlaySettingContextData>>.Awaiter _003C_003Eu__1;

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
	private struct _003CSaveAsync_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GamePlaySettingService _003C_003E4__this;

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
	private GraphicSettingService _graphicSetting;

	[Inject]
	private GraphicOptionController _graphicOptionController;

	private readonly CompositeDisposable _disposable;

	private const int MIN_PAD_SENSITIVITY = 1;

	private GamePlaySettingContext _context;

	private bool _dirty;

	public readonly Subject<ELanguage> OnLanguageApplied;

	private int DefaultPadSensitivity => 0;

	private string FileName => null;

	private string AccountId => null;

	public ELanguage Language => ELanguage.None;

	public bool AimSupportOn => false;

	public bool AutoAttackOn => false;

	public bool LockMouseOn => false;

	public bool RstickAttackOn => false;

	public bool RunToggleOn => false;

	public bool AimLineOn => false;

	public int PadSensitivity => 0;

	public IReadOnlyReactiveProperty<ELanguage> RxLanguage => null;

	public IReadOnlyReactiveProperty<bool> RxAimSupportOn => null;

	public IReadOnlyReactiveProperty<bool> RxAutoAttackOn => null;

	public IReadOnlyReactiveProperty<bool> RxLockMouseOn => null;

	public IReadOnlyReactiveProperty<bool> RxRstickAttackOn => null;

	public IReadOnlyReactiveProperty<bool> RxRunToggleOn => null;

	public IReadOnlyReactiveProperty<bool> RxAimLineOn => null;

	public IReadOnlyReactiveProperty<int> RxPadSensitivity => null;

	[AsyncStateMachine(typeof(_003CInitAsync_003Ed__48))]
	public UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private GamePlaySettingContext CreateDefault()
	{
		return null;
	}

	private static ELanguage GetDefaultLanguage()
	{
		return ELanguage.None;
	}

	private static bool TryGetSteamLanguage(out ELanguage language)
	{
		language = default;
		return false;
	}

	private static ELanguage GetCultureLanguage()
	{
		return ELanguage.None;
	}

	public void Reset()
	{
	}

	public void Flush()
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__55))]
	private UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void SetLanguage(ELanguage language)
	{
	}

	public void SetAimSupportOn(bool aimSupportOn)
	{
	}

	public void SetAutoAttackOn(bool autoAttackOn)
	{
	}

	public void SetLockMouseOn(bool lockMouseOn)
	{
	}

	public void SetRstickAttackOn(bool rstickAttackOn)
	{
	}

	public void SetRunToggleOn(bool runToggleOn)
	{
	}

	public void SetAimLineOn(bool aimLineOn)
	{
	}

	public void SetPadSensitivity(int padSensitivity)
	{
	}

	private void OnUpdateLanguage(ELanguage language)
	{
	}

	private void OnUpdateLockMouse(bool lockMouseOn)
	{
	}

	private void OnUpdateAimLineOn(bool aimLineOn)
	{
	}
}

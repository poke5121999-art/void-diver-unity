using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using UniRx;
using VContainer;

public class SoundSettingService : NativeBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitAsync_003Ed__51 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SoundSettingService _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask<StorageLoadResult<SoundSettingContextData>>.Awaiter _003C_003Eu__1;

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

		public SoundSettingService _003C_003E4__this;

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

	public const int VolumeLevelMax = 100;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private SoundManager _soundManager;

	private SoundSettingContext _context;

	private bool _dirty;

	public readonly Subject<float> OnBgmVolumeApplied;

	public readonly Subject<float> OnSfxVolumeApplied;

	private string FileName => null;

	private string AccountId => null;

	public bool MasterVolumeOn => false;

	public int MasterVolumeLevel => 0;

	public bool BgmVolumeOn => false;

	public int BgmVolumeLevel => 0;

	public bool SfxVolumeOn => false;

	public int SfxVolumeLevel => 0;

	public IReadOnlyReactiveProperty<bool> RxMasterVolumeOn => null;

	public IReadOnlyReactiveProperty<int> RxMasterVolumeLevel => null;

	public IReadOnlyReactiveProperty<bool> RxBgmVolumeOn => null;

	public IReadOnlyReactiveProperty<int> RxBgmVolumeLevel => null;

	public IReadOnlyReactiveProperty<bool> RxSfxVolumeOn => null;

	public IReadOnlyReactiveProperty<int> RxSfxVolumeLevel => null;

	private float MasterVolume => 0f;

	private float BgmVolume => 0f;

	private float SfxVolume => 0f;

	public bool AppliedBgmOn => false;

	public bool AppliedSfxOn => false;

	public float AppliedBgmVolume => 0f;

	public float AppliedSfxVolume => 0f;

	[AsyncStateMachine(typeof(_003CInitAsync_003Ed__51))]
	public UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private SoundSettingContext CreateDefault()
	{
		return null;
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

	public void SetMasterVolumeOn(bool volumeOn)
	{
	}

	public void SetMasterVolumeLevel(int volumeLevel)
	{
	}

	public void SetBgmVolumeOn(bool volumeOn)
	{
	}

	public void SetBgmVolumeLevel(int volumeLevel)
	{
	}

	public void SetSfxVolumeOn(bool volumeOn)
	{
	}

	public void SetSfxVolumeLevel(int volumeLevel)
	{
	}

	private void ApplyAll()
	{
	}

	private void ApplyMasterVolume()
	{
	}

	private void ApplyBgmVolume()
	{
	}

	private void ApplySfxVolume()
	{
	}
}

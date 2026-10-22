using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UniRx;
using VContainer;

public class AccountRecordManager : NativeBase, IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSaveAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public AccountRecordManager _003C_003E4__this;

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
	private GameEventSystem _eventSystem;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private AppManager _appManager;

	private AccountRecordContext _context;

	private CompositeDisposable _disposable;

	private bool _isDirty;

	private bool _isInitialized;

	private string _accountId;

	private string ContextFileName => null;

	public void Initialize()
	{
	}

	public void Dispose()
	{
	}

	public long GetCount(int recordId)
	{
		return 0L;
	}

	public void Save()
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__14))]
	public UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
	}
}

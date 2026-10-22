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

public class StorageService : NativeBase
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0<T> where T : ISaveData
	{
		public T snapshot;

		internal byte[] _003CSaveAsync_003Eb__0()
		{
			return null;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CBackupAsync_003Ed__20<T> : IAsyncStateMachine where T : ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public T data;

		public CancellationToken cancellationToken;

		public StorageService _003C_003E4__this;

		public string fileName;

		public long ticks;

		private UniTask<byte[]>.Awaiter _003C_003Eu__1;

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
	private struct _003CDeleteAsync_003Ed__26<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public StorageService _003C_003E4__this;

		public string fileName;

		public CancellationToken cancellationToken;

		public string backupFileName;

		public long ticks;

		private UniTask<StorageLoadResult<T>>.Awaiter _003C_003Eu__1;

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
	private struct _003CLoadAsync_003Ed__18<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<StorageLoadResult<T>> _003C_003Et__builder;

		public StorageService _003C_003E4__this;

		public string fileName;

		public CancellationToken cancellationToken;

		public string accountId;

		private UniTask<StorageLoadResult<T>>.Awaiter _003C_003Eu__1;

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
	private struct _003CLoadBackupAsync_003Ed__24<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

		public StorageService _003C_003E4__this;

		public string fileName;

		public long ticks;

		public CancellationToken cancellationToken;

		private UniTask<T>.Awaiter _003C_003Eu__1;

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
	private struct _003CLoadFromLocalAsync_003Ed__16<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<StorageLoadResult<T>> _003C_003Et__builder;

		public string key;

		public CancellationToken cancellationToken;

		private UniTask<StorageLoadResult<T>>.Awaiter _003C_003Eu__1;

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
	private struct _003CSaveAsync_003Ed__11<T> : IAsyncStateMachine where T : ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public T data;

		public CancellationToken cancellationToken;

		public StorageService _003C_003E4__this;

		public string fileName;

		private _003C_003Ec__DisplayClass11_0<T> _003C_003E8__1;

		private UniTask<byte[]>.Awaiter _003C_003Eu__1;

		private UniTask<ESaveResult>.Awaiter _003C_003Eu__2;

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
	private struct _003CSaveAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public StorageService _003C_003E4__this;

		public string fileName;

		public byte[] bytes;

		public CancellationToken cancellationToken;

		private UniTask<ESaveResult>.Awaiter _003C_003Eu__1;

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
	private struct _003CSaveToLocalAsync_003Ed__15<T> : IAsyncStateMachine where T : ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public T data;

		public CancellationToken cancellationToken;

		public StorageService _003C_003E4__this;

		public string key;

		private UniTask<byte[]>.Awaiter _003C_003Eu__1;

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
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private IStorage _storage;

	[Inject]
	private IBackupStorage _backup;

	[Inject]
	private SeriLogger _logger;

	private readonly Subject<ESaveResult> _saveFailedSubject;

	private bool _saveFailureNotified;

	public IObservable<ESaveResult> OnSaveFailed => null;

	private bool TrySerialize<T>(string fileName, T data, out byte[] bytes) where T : ISaveData
	{
		bytes = null;
		return false;
	}

	private void NotifyIfSaveFailed(ESaveResult result)
	{
	}

	public void Save<T>(string fileName, T data) where T : ISaveData
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__11<>))]
	public UniTask SaveAsync<T>(string fileName, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__12))]
	public UniTask SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public UniTask<byte[]> LoadAsync(string fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void Delete(string fileName)
	{
	}

	[AsyncStateMachine(typeof(_003CSaveToLocalAsync_003Ed__15<>))]
	public UniTask SaveToLocalAsync<T>(string key, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CLoadFromLocalAsync_003Ed__16<>))]
	public UniTask<StorageLoadResult<T>> LoadFromLocalAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		return default;
	}

	public StorageLoadResult<T> Load<T>(string fileName, string accountId) where T : class, ISaveData
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CLoadAsync_003Ed__18<>))]
	public UniTask<StorageLoadResult<T>> LoadAsync<T>(string fileName, string accountId, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		return default;
	}

	public void Backup<T>(string fileName, T data, long ticks) where T : ISaveData
	{
	}

	[AsyncStateMachine(typeof(_003CBackupAsync_003Ed__20<>))]
	public UniTask BackupAsync<T>(string fileName, T data, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		return default;
	}

	public long[] GetBackupList()
	{
		return null;
	}

	public void DeleteBackup(long ticks)
	{
	}

	public T LoadBackup<T>(string fileName, long ticks) where T : class, ISaveData
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CLoadBackupAsync_003Ed__24<>))]
	public UniTask<T> LoadBackupAsync<T>(string fileName, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		return default;
	}

	public void Delete<T>(string fileName, long ticks) where T : class, ISaveData
	{
	}

	[AsyncStateMachine(typeof(_003CDeleteAsync_003Ed__26<>))]
	public UniTask DeleteAsync<T>(string fileName, long ticks, string backupFileName = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		return default;
	}
}

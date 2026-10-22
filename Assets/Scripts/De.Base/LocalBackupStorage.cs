using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using VContainer;

public class LocalBackupStorage : NativeBase, IBackupStorage
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public LocalBackupStorage _003C_003E4__this;

		public string fileName;

		public long ticks;

		public byte[] bytes;

		internal void _003CIBackupStorage_002ESaveAsync_003Eb__0()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0<T> where T : class, ISaveData
	{
		public string filePath;

		internal T _003CIBackupStorage_002ELoadAsync_003Eb__0()
		{
			return null;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CIBackupStorage_002DLoadAsync_003Ed__8<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

		public LocalBackupStorage _003C_003E4__this;

		public long ticks;

		public string fileName;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass8_0<T> _003C_003E8__1;

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
	private struct _003CIBackupStorage_002DSaveAsync_003Ed__5 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LocalBackupStorage _003C_003E4__this;

		public string fileName;

		public long ticks;

		public byte[] bytes;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

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

	private readonly string _saveDir;

	private string ToTicksPath(string fileName, long ticks)
	{
		return null;
	}

	void IBackupStorage.Save(string fileName, byte[] bytes, long ticks)
	{
	}

	[AsyncStateMachine(typeof(_003CIBackupStorage_002DSaveAsync_003Ed__5))]
	UniTask IBackupStorage.SaveAsync(string fileName, byte[] bytes, long ticks, CancellationToken cancellationToken)
	{
		return default;
	}

	long[] IBackupStorage.GetBackupList()
	{
		return null;
	}

	T IBackupStorage.Load<T>(string fileName, long ticks)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CIBackupStorage_002DLoadAsync_003Ed__8<>))]
	UniTask<T> IBackupStorage.LoadAsync<T>(string fileName, long ticks, CancellationToken cancellationToken)
	{
		return default;
	}

	private string BuildLoadError(string filePath, Exception e)
	{
		return null;
	}

	void IBackupStorage.Delete(long ticks)
	{
	}

	private static void WriteFileAtomic(string filePath, byte[] bytes)
	{
	}

	private string ArchiveFile(string filePath)
	{
		return null;
	}
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using VContainer;

public class LocalStorage : NativeBase, IStorage
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public LocalStorage _003C_003E4__this;

		public string fileName;

		public byte[] bytes;

		internal void _003CIStorage_002ESaveAsync_003Eb__0()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0<T> where T : class, ISaveData
	{
		public string filePath;

		public byte[] bytes;

		internal byte[] _003CIStorage_002ELoadAsync_003Eb__0()
		{
			return null;
		}

		internal T _003CIStorage_002ELoadAsync_003Eb__1()
		{
			return null;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public string filePath;

		internal byte[] _003CIStorage_002ELoadAsync_003Eb__0()
		{
			return null;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CIStorage_002DLoadAsync_003Ed__8<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<StorageLoadResult<T>> _003C_003Et__builder;

		public LocalStorage _003C_003E4__this;

		public string fileName;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass8_0<T> _003C_003E8__1;

		private UniTask<byte[]>.Awaiter _003C_003Eu__1;

		private UniTask<T>.Awaiter _003C_003Eu__2;

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
	private struct _003CIStorage_002DLoadAsync_003Ed__9 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<byte[]> _003C_003Et__builder;

		public LocalStorage _003C_003E4__this;

		public string fileName;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CIStorage_002DSaveAsync_003Ed__5 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<ESaveResult> _003C_003Et__builder;

		public LocalStorage _003C_003E4__this;

		public string fileName;

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

	private string ToPath(string fileName)
	{
		return null;
	}

	ESaveResult IStorage.Save(string fileName, byte[] bytes)
	{
		return ESaveResult.Success;
	}

	[AsyncStateMachine(typeof(_003CIStorage_002DSaveAsync_003Ed__5))]
	UniTask<ESaveResult> IStorage.SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken)
	{
		return default;
	}

	private static ESaveResult ToSaveResult(Exception e)
	{
		return ESaveResult.Success;
	}

	StorageLoadResult<T> IStorage.Load<T>(string fileName)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CIStorage_002DLoadAsync_003Ed__8<>))]
	UniTask<StorageLoadResult<T>> IStorage.LoadAsync<T>(string fileName, CancellationToken cancellationToken)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CIStorage_002DLoadAsync_003Ed__9))]
	UniTask<byte[]> IStorage.LoadAsync(string fileName, CancellationToken cancellationToken)
	{
		return default;
	}

	void IStorage.SaveJson(string fileName, string json)
	{
	}

	void IStorage.Delete(string fileName)
	{
	}

	private static void WriteFileAtomic(string filePath, byte[] bytes)
	{
	}
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using Steamworks;
using VContainer;

public class SteamStorage : NativeBase, IStorage, IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CIStorage_002DLoadAsync_003Ed__7<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<StorageLoadResult<T>> _003C_003Et__builder;

		public SteamStorage _003C_003E4__this;

		public string fileName;

		public CancellationToken cancellationToken;

		private UniTask<(ELoadResult result, T data, string error)>.Awaiter _003C_003Eu__1;

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
	private struct _003CIStorage_002DLoadAsync_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<byte[]> _003C_003Et__builder;

		public string fileName;

		public SteamStorage _003C_003E4__this;

		public CancellationToken cancellationToken;

		private string _003CfilePath_003E5__2;

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

		public SteamStorage _003C_003E4__this;

		public string fileName;

		public byte[] bytes;

		public CancellationToken cancellationToken;

		private UniTask<(ESaveResult result, string error)>.Awaiter _003C_003Eu__1;

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
	private struct _003CLoadAsyncInternal_003Ed__16<T> : IAsyncStateMachine where T : class, ISaveData
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<(ELoadResult result, T data, string error)> _003C_003Et__builder;

		public string filePath;

		public SteamStorage _003C_003E4__this;

		public CancellationToken cancellationToken;

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
	private struct _003CSaveAsyncInternal_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<(ESaveResult result, string error)> _003C_003Et__builder;

		public string filePath;

		public byte[] bytes;

		public SteamStorage _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask<EResult>.Awaiter _003C_003Eu__1;

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

	private CallResult<RemoteStorageFileWriteAsyncComplete_t> _steamWriteCallResult;

	private CallResult<RemoteStorageFileReadAsyncComplete_t> _steamReadCallResult;

	private static string ToPath(string fileName)
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

	StorageLoadResult<T> IStorage.Load<T>(string fileName)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CIStorage_002DLoadAsync_003Ed__7<>))]
	UniTask<StorageLoadResult<T>> IStorage.LoadAsync<T>(string fileName, CancellationToken cancellationToken)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CIStorage_002DLoadAsync_003Ed__8))]
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

	public void Dispose()
	{
	}

	private void LogSaveFailure(ESaveResult result, string error)
	{
	}

	private (ESaveResult, string) SaveInternal(string filePath, byte[] bytes)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSaveAsyncInternal_003Ed__14))]
	private UniTask<(ESaveResult, string)> SaveAsyncInternal(string filePath, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private (ELoadResult, T, string) LoadInternal<T>(string filePath) where T : class, ISaveData
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CLoadAsyncInternal_003Ed__16<>))]
	private UniTask<(ELoadResult, T, string)> LoadAsyncInternal<T>(string filePath, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		return default;
	}

	private UniTask<EResult> WaitForSteamWriteCall(SteamAPICall_t apiCall)
	{
		return default;
	}

	private UniTask<byte[]> WaitForSteamReadCall(SteamAPICall_t apiCall, uint fileSize)
	{
		return default;
	}
}

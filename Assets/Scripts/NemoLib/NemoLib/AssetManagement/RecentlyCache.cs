using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NemoLib.AssetManagement
{
	internal class RecentlyCache<T> : IRecentlyCache
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnsureAssetAsync_003Ed__8 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public RecentlyCache<T> _003C_003E4__this;

			public string key;

			private Dictionary<string, T> _003C_003E7__wrap1;

			private string _003C_003E7__wrap2;

			private UniTask<T>.Awaiter _003C_003Eu__1;

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
		private struct _003CGetAsync_003Ed__11 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<object> _003C_003Et__builder;

			public RecentlyCache<T> _003C_003E4__this;

			public string key;

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

		private readonly Dictionary<string, T> _objects;

		private readonly Dictionary<string, AsyncOperationHandle<T>> _handles;

		private readonly LinkedList<string> _recentlyKeys;

		public int Capacity { get; private set; }

		public RecentlyCache(int capacity = 5)
		{
		}

		[AsyncStateMachine(typeof(RecentlyCache<>._003CEnsureAssetAsync_003Ed__8))]
		public UniTask EnsureAssetAsync(string key)
		{
			return default;
		}

		public void Release(string key)
		{
		}

		public object Get(string key)
		{
			return null;
		}

		[AsyncStateMachine(typeof(RecentlyCache<>._003CGetAsync_003Ed__11))]
		public UniTask<object> GetAsync(string key)
		{
			return default;
		}

		public void SetCapacity(int capacity)
		{
		}
	}
}

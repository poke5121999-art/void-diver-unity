using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Collections;

namespace NemoLib.AssetManagement
{
	internal class AssetRecentlyCaches
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnsureAsync_003Ed__1<T> : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public AssetRecentlyCaches _003C_003E4__this;

			public string category;

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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CGetAsync_003Ed__5<T> : IAsyncStateMachine where T : class
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

			public AssetRecentlyCaches _003C_003E4__this;

			public string category;

			public string key;

			private UniTask<object>.Awaiter _003C_003Eu__1;

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

		private readonly MultiKeyDictionary<Type, string, IRecentlyCache> _caches;

		[AsyncStateMachine(typeof(_003CEnsureAsync_003Ed__1<>))]
		public UniTask EnsureAsync<T>(string category, string key)
		{
			return default;
		}

		public void Release<T>(string category, string key)
		{
		}

		public IRecentlyCache GetRecentlyCache<T>(string category) where T : class
		{
			return null;
		}

		public T Get<T>(string category, string key) where T : class
		{
			return null;
		}

		[AsyncStateMachine(typeof(_003CGetAsync_003Ed__5<>))]
		public UniTask<T> GetAsync<T>(string category, string key) where T : class
		{
			return default;
		}
	}
}

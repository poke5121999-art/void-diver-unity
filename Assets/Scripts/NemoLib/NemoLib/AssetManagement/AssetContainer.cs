using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace NemoLib.AssetManagement
{
	internal class AssetContainer<T> : IAssetContainer
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CNemoLib_002DAssetManagement_002DIAssetContainer_002DEnsureAssetAsync_003Ed__2 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public AssetContainer<T> _003C_003E4__this;

			public IList<IResourceLocation> locations;

			public string label;

			private IResourceLocation[] _003CloadableLocations_003E5__2;

			private AsyncOperationHandle<IList<T>> _003Chandle_003E5__3;

			private UniTask<IList<T>>.Awaiter _003C_003Eu__1;

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

		private readonly Dictionary<string, AsyncOperationHandle<IList<T>>> _handles;

		[AsyncStateMachine(typeof(AssetContainer<>._003CNemoLib_002DAssetManagement_002DIAssetContainer_002DEnsureAssetAsync_003Ed__2))]
		UniTask IAssetContainer.EnsureAssetAsync(string label, IList<IResourceLocation> locations)
		{
			return default;
		}

		void IAssetContainer.Release(string label, IList<IResourceLocation> locations)
		{
		}

		object IAssetContainer.Get(string key)
		{
			return null;
		}

		private IResourceLocation[] GetLoadableLocations(IList<IResourceLocation> locations)
		{
			return null;
		}
	}
}

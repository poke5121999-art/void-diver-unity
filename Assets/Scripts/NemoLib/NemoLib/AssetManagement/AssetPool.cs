using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UniRx;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	internal class AssetPool : MonoBehaviour, IAssetPool
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CDespawnDelayedAsync_003Ed__11 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public float delay;

			public PoolableObject poolableObj;

			public AssetPool _003C_003E4__this;

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

		private GameObject _prefab;

		private int _maxSize;

		private readonly Stack<PoolableObject> _objects;

		public Subject<Unit> OnDestroyed { get; }

		private void OnDestroy()
		{
		}

		public void Setup(GameObject prefab, int capacity, int maxSize)
		{
		}

		public GameObject Spawn(Transform parent)
		{
			return null;
		}

		public void Despawn(PoolableObject poolableObj, float delay = 0f)
		{
		}

		public void Clear()
		{
		}

		[AsyncStateMachine(typeof(_003CDespawnDelayedAsync_003Ed__11))]
		private UniTask DespawnDelayedAsync(PoolableObject poolableObj, float delay)
		{
			return default;
		}

		private void DespawnImmediate(PoolableObject poolableObj)
		{
		}

		private PoolableObject CreatePoolableObject()
		{
			return null;
		}
	}
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace NemoLib.AssetManagement
{
	public class AssetManager
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass28_0
		{
			public ReactiveProperty<float> progress;

			internal void _003CDownloadAsync_003Eb__0(float _)
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CDownloadAsync_003Ed__27 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

			public string label;

			public ReactiveProperty<float> progress;

			private AsyncOperationHandle _003Chandle_003E5__2;

			private UniTask<long>.Awaiter _003C_003Eu__1;

			private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__2;

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
		private struct _003CDownloadAsync_003Ed__28 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public ReactiveProperty<float> progress;

			public AssetLabelReference label;

			private _003C_003Ec__DisplayClass28_0 _003C_003E8__1;

			private UniTask<long>.Awaiter _003C_003Eu__1;

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
		private struct _003CGetDownloadSizeAsync_003Ed__25 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<long> _003C_003Et__builder;

			public string label;

			private UniTask<long>.Awaiter _003C_003Eu__1;

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
		private struct _003CGetDownloadSizeAsync_003Ed__26 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<long> _003C_003Et__builder;

			public AssetLabelReference label;

			private UniTask<long>.Awaiter _003C_003Eu__1;

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
		private struct _003CGetRecentlyCacheAsync_003Ed__23<T> : IAsyncStateMachine where T : Object
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

			public AssetManager _003C_003E4__this;

			public string category;

			public string key;

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
		private struct _003CGetRecentlyCacheSpineAsync_003Ed__11 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<SkeletonDataAsset> _003C_003Et__builder;

			public AssetManager _003C_003E4__this;

			public string category;

			public string key;

			private UniTask<SkeletonDataAsset>.Awaiter _003C_003Eu__1;

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
		private struct _003CGetRecentlyCacheTextureAsync_003Ed__9 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<Sprite> _003C_003Et__builder;

			public AssetManager _003C_003E4__this;

			public string category;

			public string key;

			public Vector2? pivot;

			private UniTask<Texture2D>.Awaiter _003C_003Eu__1;

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
		private struct _003CReleaseAsync_003Ed__6 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public AssetManager _003C_003E4__this;

			public string label;

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

		private readonly AssetPreloader _preloader;

		private readonly AssetRecentlyCaches _recentlyCaches;

		private readonly AssetPoolManager _poolManager;

		private readonly Dictionary<(string atlasName, string spriteName), Sprite> _spriteCache;

		public UniTask EnsureAsync(string label, ReactiveProperty<float> progress = null)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CReleaseAsync_003Ed__6))]
		public UniTask ReleaseAsync(string label)
		{
			return default;
		}

		public Sprite GetSprite(string atlasName, string spriteName)
		{
			return null;
		}

		public Sprite GetTexture(string key, Vector2? pivot = null)
		{
			return null;
		}

		[AsyncStateMachine(typeof(_003CGetRecentlyCacheTextureAsync_003Ed__9))]
		public UniTask<Sprite> GetRecentlyCacheTextureAsync(string category, string key, Vector2? pivot = null)
		{
			return default;
		}

		public SkeletonDataAsset GetSpine(string key)
		{
			return null;
		}

		[AsyncStateMachine(typeof(_003CGetRecentlyCacheSpineAsync_003Ed__11))]
		public UniTask<SkeletonDataAsset> GetRecentlyCacheSpineAsync(string category, string key)
		{
			return default;
		}

		private GameObject GetPrefab(string key)
		{
			return null;
		}

		public GameObject Instantiate(string key, Transform parentTf = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
		{
			return null;
		}

		public T Instantiate<T>(string key, Transform parentTf = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) where T : Object
		{
			return null;
		}

		public void SetCapacity<T>(string category, int capacity) where T : Object
		{
		}

		public GameObject Spawn(string key, Transform parent = null)
		{
			return null;
		}

		public T Spawn<T>(string key, Transform parent = null) where T : Component
		{
			return null;
		}

		public void EnsureAssetPool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene = false)
		{
		}

		public GameObject EnsureSpawn(string key, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null)
		{
			return null;
		}

		public T EnsureSpawn<T>(string key, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null) where T : Component
		{
			return null;
		}

		public void ClearPool(string key)
		{
		}

		public void ClearAllPools()
		{
		}

		[AsyncStateMachine(typeof(_003CGetRecentlyCacheAsync_003Ed__23<>))]
		public UniTask<T> GetRecentlyCacheAsync<T>(string category, string key) where T : Object
		{
			return default;
		}

		private SpriteAtlas GetSpriteAtlas(string key)
		{
			return null;
		}

		[AsyncStateMachine(typeof(_003CGetDownloadSizeAsync_003Ed__25))]
		public UniTask<long> GetDownloadSizeAsync(string label)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CGetDownloadSizeAsync_003Ed__26))]
		public UniTask<long> GetDownloadSizeAsync(AssetLabelReference label)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CDownloadAsync_003Ed__27))]
		public UniTask<bool> DownloadAsync(string label, ReactiveProperty<float> progress = null)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CDownloadAsync_003Ed__28))]
		public UniTask DownloadAsync(AssetLabelReference label, ReactiveProperty<float> progress = null)
		{
			return default;
		}
	}
}

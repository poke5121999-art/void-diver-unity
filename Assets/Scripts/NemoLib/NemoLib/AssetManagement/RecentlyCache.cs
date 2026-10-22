using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NemoLib.AssetManagement
{
	internal class RecentlyCache<T> : IRecentlyCache
	{
		private readonly Dictionary<string, T> _objects = new Dictionary<string, T>();

		private readonly Dictionary<string, AsyncOperationHandle<T>> _handles = new Dictionary<string, AsyncOperationHandle<T>>();

		private readonly LinkedList<string> _recentlyKeys = new LinkedList<string>();

		public int Capacity { get; private set; }

		public RecentlyCache(int capacity = 5)
		{
			Capacity = capacity;
		}

		// TODO(decomp): generic-shared body of RecentlyCache<T> is not exported (address 0x0); LRU logic reconstructed
		// from field layout (_objects, _handles, _recentlyKeys) and the EnsureAssetAsync state machine fields @0x0
		public async UniTask EnsureAssetAsync(string key)
		{
			if (_objects.ContainsKey(key))
			{
				Touch(key);
				return;
			}
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
			T asset = await handle.ToUniTask();
			_objects[key] = asset;
			_handles[key] = handle;
			Touch(key);
			Trim();
		}

		public void Release(string key)
		{
			_recentlyKeys.Remove(key);
			_objects.Remove(key);
			if (_handles.TryGetValue(key, out var handle))
			{
				_handles.Remove(key);
				Addressables.Release(handle);
			}
		}

		public object Get(string key)
		{
			if (_objects.TryGetValue(key, out var value))
			{
				Touch(key);
				return value;
			}
			return null;
		}

		public async UniTask<object> GetAsync(string key)
		{
			await EnsureAssetAsync(key);
			return Get(key);
		}

		public void SetCapacity(int capacity)
		{
			Capacity = capacity;
			Trim();
		}

		private void Touch(string key)
		{
			_recentlyKeys.Remove(key);
			_recentlyKeys.AddFirst(key);
		}

		private void Trim()
		{
			while (_recentlyKeys.Count > Capacity)
			{
				string last = _recentlyKeys.Last.Value;
				Release(last);
			}
		}
	}
}

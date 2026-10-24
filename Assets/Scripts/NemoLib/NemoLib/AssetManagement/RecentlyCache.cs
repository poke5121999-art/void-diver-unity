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

		// <EnsureAssetAsync>d__8<object>.MoveNext @0x1826f8c50 (machine code checked): the key list is touched first (evicting its last key, without releasing it, when full);
		// an asset that is already loading is only awaited, a new load stores the result in _objects
		public async UniTask EnsureAssetAsync(string key)
		{
			_recentlyKeys.Remove(key);
			if (_recentlyKeys.Count >= Capacity)
			{
				_recentlyKeys.RemoveLast();
			}
			_recentlyKeys.AddFirst(key);
			if (_objects.ContainsKey(key))
			{
				return;
			}
			if (_handles.TryGetValue(key, out var pending))
			{
				await pending;
				await UniTask.DelayFrame(1);
				return;
			}
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
			_handles.Add(key, handle);
			_objects[key] = await handle;
		}

		// Release<object> @0x181a231f0 (machine code checked): the handle is released but its entry stays in _handles
		public void Release(string key)
		{
			_objects.Remove(key);
			_recentlyKeys.Remove(key);
			if (_handles.TryGetValue(key, out var handle))
			{
				Addressables.Release(handle);
			}
		}

		// Get<object> @0x181a22fa0 (machine code checked)
		public object Get(string key)
		{
			if (_objects.TryGetValue(key, out var value))
			{
				_recentlyKeys.Remove(key);
				_recentlyKeys.AddFirst(key);
				return value;
			}
			return null;
		}

		// <GetAsync>d__11<object>.MoveNext @0x182708a30 (machine code checked)
		public async UniTask<object> GetAsync(string key)
		{
			await EnsureAssetAsync(key);
			return Get(key);
		}

		// SetCapacity shares the setter's body @0x1801b0c70 (machine code checked): it only stores the value
		public void SetCapacity(int capacity)
		{
			Capacity = capacity;
		}
	}
}

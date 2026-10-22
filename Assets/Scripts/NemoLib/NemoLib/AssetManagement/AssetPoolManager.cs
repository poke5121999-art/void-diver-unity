using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public class AssetPoolManager : MonoBehaviour
	{
		private readonly Dictionary<string, IAssetPool> _pools = new Dictionary<string, IAssetPool>();

		public IAssetPool EnsureAssetPool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene = false)
		{
			if (!_pools.TryGetValue(key, out var pool))
			{
				pool = CreatePool(key, prefab, capacity, maxSize, destroyWithScene);
			}
			return pool;
		}

		public GameObject EnsureSpawn(string key, GameObject prefab, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null)
		{
			IAssetPool pool = EnsureAssetPool(key, prefab, capacity, maxSize, destroyWithScene);
			if (pool == null)
			{
				return null;
			}
			return pool.Spawn(parent);
		}

		public GameObject Spawn(string key, Transform parent = null)
		{
			if (!_pools.TryGetValue(key, out var pool))
			{
				Debug.LogError("[AssetPoolManager] Pool not exists. Key : " + key);
				return null;
			}
			return pool.Spawn(parent);
		}

		public void Clear(string key)
		{
			if (_pools.TryGetValue(key, out var pool))
			{
				pool.Clear();
				_pools.Remove(key);
			}
		}

		public void ClearAll()
		{
			foreach (IAssetPool value in _pools.Values)
			{
				value.Clear();
			}
			_pools.Clear();
		}

		private IAssetPool CreatePool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene)
		{
			if (_pools.ContainsKey(key))
			{
				Debug.LogError("[AssetPoolManager] Pool already exists. Key : " + key);
				return null;
			}
			if (prefab == null)
			{
				Debug.LogError("[AssetPoolManager] Prefab is null. Key : " + key);
				return null;
			}
			GameObject poolObject = new GameObject(key + "Pool");
			if (!destroyWithScene)
			{
				poolObject.transform.SetParent(base.transform, worldPositionStays: false);
			}
			AssetPool pool = poolObject.AddComponent<AssetPool>();
			pool.Setup(prefab, capacity, maxSize);
			pool.OnDestroyed.Subscribe(delegate
			{
				_pools.Remove(key);
			}).AddTo(this);
			_pools.Add(key, pool);
			return pool;
		}
	}
}

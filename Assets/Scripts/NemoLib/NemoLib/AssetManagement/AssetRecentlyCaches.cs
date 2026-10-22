using System;
using Cysharp.Threading.Tasks;
using NemoLib.Collections;

namespace NemoLib.AssetManagement
{
	internal class AssetRecentlyCaches
	{
		private readonly MultiKeyDictionary<Type, string, IRecentlyCache> _caches = new MultiKeyDictionary<Type, string, IRecentlyCache>();

		// TODO(decomp): generic method bodies are not exported (address 0x0); delegation to the per-category cache reconstructed
		public async UniTask EnsureAsync<T>(string category, string key)
		{
			await GetOrCreate<T>(category).EnsureAssetAsync(key);
		}

		public void Release<T>(string category, string key)
		{
			GetOrCreate<T>(category).Release(key);
		}

		public IRecentlyCache GetRecentlyCache<T>(string category) where T : class
		{
			return GetOrCreate<T>(category);
		}

		private IRecentlyCache GetOrCreate<T>(string category)
		{
			if (!_caches.TryGetValue(typeof(T), category, out var cache))
			{
				cache = new RecentlyCache<T>();
				_caches.Add(typeof(T), category, cache);
			}
			return cache;
		}

		public T Get<T>(string category, string key) where T : class
		{
			return GetRecentlyCache<T>(category).Get(key) as T;
		}

		public async UniTask<T> GetAsync<T>(string category, string key) where T : class
		{
			return await GetRecentlyCache<T>(category).GetAsync(key) as T;
		}
	}
}

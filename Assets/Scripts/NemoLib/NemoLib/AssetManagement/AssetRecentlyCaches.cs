using System;
using Cysharp.Threading.Tasks;
using NemoLib.Collections;

namespace NemoLib.AssetManagement
{
	internal class AssetRecentlyCaches
	{
		private readonly MultiKeyDictionary<Type, string, IRecentlyCache> _caches = new MultiKeyDictionary<Type, string, IRecentlyCache>();

		// EnsureAsync<object> @0x180b191b0 and <EnsureAsync>d__1<object>.MoveNext @0x1826f94e0 (machine code checked): the cache lookup of GetRecentlyCache<T> is inlined
		public async UniTask EnsureAsync<T>(string category, string key)
		{
			await GetOrCreate<T>(category).EnsureAssetAsync(key);
		}

		// Release<object> @0x180b19750 (machine code checked): only an existing cache is touched, none is created
		public void Release<T>(string category, string key)
		{
			if (_caches.TryGetValue(typeof(T), category, out var cache))
			{
				cache.Release(key);
			}
		}

		// GetRecentlyCache<object> @0x180b194e0 (machine code checked): creates RecentlyCache<T>(5) on first use
		public IRecentlyCache GetRecentlyCache<T>(string category) where T : class
		{
			return GetOrCreate<T>(category);
		}

		private IRecentlyCache GetOrCreate<T>(string category)
		{
			if (!_caches.TryGetValue(typeof(T), category, out var cache))
			{
				cache = new RecentlyCache<T>(5);
				_caches.Add(typeof(T), category, cache);
			}
			return cache;
		}

		// Get<object> @0x180b19640 (machine code checked): no cache is created when the category is unknown
		public T Get<T>(string category, string key) where T : class
		{
			if (_caches.TryGetValue(typeof(T), category, out var cache))
			{
				return cache.Get(key) as T;
			}
			return null;
		}

		// <GetAsync>d__5<object>.MoveNext @0x182709090 (machine code checked): the cache lookup of GetRecentlyCache<T> is inlined
		public async UniTask<T> GetAsync<T>(string category, string key) where T : class
		{
			return await GetRecentlyCache<T>(category).GetAsync(key) as T;
		}
	}
}

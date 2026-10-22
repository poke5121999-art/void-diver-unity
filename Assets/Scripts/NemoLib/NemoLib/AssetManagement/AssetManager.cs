using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
		private readonly AssetPreloader _preloader = new AssetPreloader();

		private readonly AssetRecentlyCaches _recentlyCaches = new AssetRecentlyCaches();

		private readonly AssetPoolManager _poolManager;

		private readonly Dictionary<(string atlasName, string spriteName), Sprite> _spriteCache = new Dictionary<(string atlasName, string spriteName), Sprite>();

		public AssetManager()
		{
			GameObject gameObject = new GameObject("AssetPoolManager");
			Object.DontDestroyOnLoad(gameObject);
			_poolManager = gameObject.AddComponent<AssetPoolManager>();
		}

		public UniTask EnsureAsync(string label, ReactiveProperty<float> progress = null)
		{
			return _preloader.EnsureAsync(label, progress);
		}

		public async UniTask ReleaseAsync(string label)
		{
			_spriteCache.Clear();
			await _preloader.ReleaseAsync(label);
		}

		public Sprite GetSprite(string atlasName, string spriteName)
		{
			if (_spriteCache.TryGetValue((atlasName, spriteName), out var cached) && cached != null)
			{
				return cached;
			}
			SpriteAtlas atlas = GetSpriteAtlas(atlasName);
			Sprite sprite = ((atlas != null) ? atlas.GetSprite(spriteName) : null);
			if (sprite == null)
			{
				// TODO(decomp): the failure branch at 0x182D09F8A is tangled in the decompile; logging and returning null is the readable part
				Debug.LogError("Resource load failed. Atlas : " + atlasName + ", Sprite : " + spriteName);
				return null;
			}
			_spriteCache[(atlasName, spriteName)] = sprite;
			return sprite;
		}

		public Sprite GetTexture(string key, Vector2? pivot = null)
		{
			Texture2D texture = _preloader.Get<Texture2D>(key);
			if (texture == null)
			{
				Debug.LogError("Resource texture (" + key + ") not found");
				return null;
			}
			return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), pivot.GetValueOrDefault(new Vector2(0.5f, 0.5f)));
		}

		public async UniTask<Sprite> GetRecentlyCacheTextureAsync(string category, string key, Vector2? pivot = null)
		{
			Texture2D texture = await _recentlyCaches.GetAsync<Texture2D>(category, key);
			if (texture == null)
			{
				Debug.LogError("Resource texture (" + key + ") not found");
				return null;
			}
			return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), pivot.GetValueOrDefault(new Vector2(0.5f, 0.5f)));
		}

		public SkeletonDataAsset GetSpine(string key)
		{
			SkeletonDataAsset spine = _preloader.Get<SkeletonDataAsset>(key);
			if (spine == null)
			{
				Debug.LogError("Resource spine(" + key + ") not found");
			}
			return spine;
		}

		public async UniTask<SkeletonDataAsset> GetRecentlyCacheSpineAsync(string category, string key)
		{
			SkeletonDataAsset spine = await _recentlyCaches.GetAsync<SkeletonDataAsset>(category, key);
			if (spine == null)
			{
				Debug.LogError("Resource spine(" + key + ") not found");
			}
			return spine;
		}

		private GameObject GetPrefab(string key)
		{
			GameObject prefab = _preloader.Get<GameObject>(key);
			if (prefab == null)
			{
				Debug.LogError("Resource prefab(" + key + ") not found");
			}
			return prefab;
		}

		public GameObject Instantiate(string key, Transform parentTf = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
		{
			GameObject prefab = GetPrefab(key);
			if (prefab == null)
			{
				return null;
			}
			GameObject instance = Object.Instantiate(prefab, position, rotation, parentTf);
			instance.transform.localScale = Vector3.one;
			return instance;
		}

		// TODO(decomp): generic bodies are not exported (address 0x0); delegation to the non-generic overload is reconstructed
		public T Instantiate<T>(string key, Transform parentTf = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) where T : Object
		{
			GameObject instance = Instantiate(key, parentTf, position, rotation);
			if (instance == null)
			{
				return null;
			}
			return instance.GetComponent<T>();
		}

		public void SetCapacity<T>(string category, int capacity) where T : Object
		{
			_recentlyCaches.GetRecentlyCache<T>(category).SetCapacity(capacity);
		}

		public GameObject Spawn(string key, Transform parent = null)
		{
			return _poolManager.Spawn(key, parent);
		}

		public T Spawn<T>(string key, Transform parent = null) where T : Component
		{
			GameObject spawned = Spawn(key, parent);
			if (spawned == null)
			{
				return null;
			}
			return spawned.GetComponent<T>();
		}

		public void EnsureAssetPool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene = false)
		{
			_poolManager.EnsureAssetPool(key, prefab, capacity, maxSize, destroyWithScene);
		}

		public GameObject EnsureSpawn(string key, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null)
		{
			GameObject prefab = GetPrefab(key);
			if (prefab == null)
			{
				return null;
			}
			return _poolManager.EnsureSpawn(key, prefab, capacity, maxSize, destroyWithScene, parent);
		}

		public T EnsureSpawn<T>(string key, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null) where T : Component
		{
			GameObject spawned = EnsureSpawn(key, capacity, maxSize, destroyWithScene, parent);
			if (spawned == null)
			{
				return null;
			}
			return spawned.GetComponent<T>();
		}

		public void ClearPool(string key)
		{
			_poolManager.Clear(key);
		}

		public void ClearAllPools()
		{
			_poolManager.ClearAll();
		}

		public async UniTask<T> GetRecentlyCacheAsync<T>(string category, string key) where T : Object
		{
			return await _recentlyCaches.GetAsync<T>(category, key);
		}

		private SpriteAtlas GetSpriteAtlas(string key)
		{
			SpriteAtlas atlas = _preloader.Get<SpriteAtlas>(key);
			if (atlas == null)
			{
				Debug.LogError("Resource (" + key + ") not found");
			}
			return atlas;
		}

		public async UniTask<long> GetDownloadSizeAsync(string label)
		{
			return await Addressables.GetDownloadSizeAsync(label);
		}

		public async UniTask<long> GetDownloadSizeAsync(AssetLabelReference label)
		{
			return await Addressables.GetDownloadSizeAsync(label);
		}

		public async UniTask<bool> DownloadAsync(string label, ReactiveProperty<float> progress = null)
		{
			if (await Addressables.GetDownloadSizeAsync(label) == 0L)
			{
				return true;
			}
			AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(label, autoReleaseHandle: false);
			while (handle.Status == AsyncOperationStatus.None)
			{
				Debug.Log(string.Format("handle.Status = {0}", handle.Status));
				await UniTask.Yield();
				if (progress != null)
				{
					progress.Value = handle.GetDownloadStatus().Percent;
				}
			}
			bool succeeded = handle.Status == AsyncOperationStatus.Succeeded;
			Addressables.Release(handle);
			return succeeded;
		}

		public async UniTask DownloadAsync(AssetLabelReference label, ReactiveProperty<float> progress = null)
		{
			if (await Addressables.GetDownloadSizeAsync(label) > 0)
			{
				if (progress == null)
				{
					await Addressables.DownloadDependenciesAsync(label, autoReleaseHandle: true);
				}
				else
				{
					await Addressables.DownloadDependenciesAsync(label, autoReleaseHandle: true).ToUniTask(Progress.Create(delegate(float _)
					{
						progress.Value = _;
					}));
				}
			}
		}
	}
}

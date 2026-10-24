using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;
using Debug = UnityEngine.Debug;

namespace NemoLib.AssetManagement
{
	internal class AssetPreloader
	{
		private readonly Dictionary<Type, IAssetContainer> _assetContainers = new Dictionary<Type, IAssetContainer>();

		public AssetPreloader()
		{
			_assetContainers.Add(typeof(Texture2D), new AssetContainer<Texture2D>());
			_assetContainers.Add(typeof(SpriteAtlas), new AssetContainer<SpriteAtlas>());
			_assetContainers.Add(typeof(Spine.Unity.SkeletonDataAsset), new AssetContainer<Spine.Unity.SkeletonDataAsset>());
			_assetContainers.Add(typeof(GameObject), new AssetContainer<GameObject>());
			_assetContainers.Add(typeof(AudioClip), new AssetContainer<AudioClip>());
			SpriteAtlasManager.atlasRequested += OnAtlasRequested;
		}

		// AddContainer<object> @0x180b18fc0 (machine code checked): TryAdd, and the constructor does not call it (it adds the five containers inline)
		public void AddContainer<T>() where T : UnityEngine.Object
		{
			_assetContainers.TryAdd(typeof(T), new AssetContainer<T>());
		}

		public async UniTask EnsureAsync(string label, ReactiveProperty<float> progress = null)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(label, null).ToUniTask();
			if (locations == null || !locations.Any())
			{
				return;
			}
			// the original values are read into locals but MoveNext has only one set_ call per setting, no restore (machine code checked @0x182d1b4d0)
			ThreadPriority originalLoadingPriority = Application.backgroundLoadingPriority;
			Application.backgroundLoadingPriority = ThreadPriority.Normal;
			int originalUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
			int originalUploadBufferSize = QualitySettings.asyncUploadBufferSize;
			QualitySettings.asyncUploadTimeSlice = 16;
			QualitySettings.asyncUploadBufferSize = 64;
			UniTask[] tasks = _assetContainers.Select((KeyValuePair<Type, IAssetContainer> kv) => EnsureAssetWithLogAsync(kv.Key, kv.Value, label, locations, stopwatch)).ToArray();
			int totalTaskCount = tasks.Length;
			if (progress == null)
			{
				await UniTask.WhenAll(tasks);
			}
			else
			{
				while (true)
				{
					int completed = tasks.Count((UniTask t) => t.Status.IsCompleted());
					progress.Value = (float)completed / (float)totalTaskCount;
					if (completed == totalTaskCount)
					{
						break;
					}
					await UniTask.Delay(100);
				}
			}
			Debug.Log(string.Format("Ensured addressables - Label : {0}, Count : {1}", label, locations.Count));
		}

		private async UniTask EnsureAssetWithLogAsync(Type type, IAssetContainer container, string label, IList<IResourceLocation> locations, Stopwatch stopwatch)
		{
			await container.EnsureAssetAsync(label, locations);
		}

		public async UniTask ReleaseAsync(string label)
		{
			IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(label, null).ToUniTask();
			foreach (IAssetContainer value in _assetContainers.Values)
			{
				value.Release(label, locations);
			}
			Debug.Log("Released addressables - Label : " + label);
		}

		private void OnAtlasRequested(string tag, Action<SpriteAtlas> callback)
		{
			SpriteAtlas atlas = Get<SpriteAtlas>(tag);
			if (atlas == null)
			{
				atlas = Resources.Load<SpriteAtlas>(tag);
				if (atlas == null)
				{
					Debug.LogError("SpriteAtlas load error. : " + tag);
					return;
				}
			}
			callback?.Invoke(atlas);
		}

		// Get<object> @0x180b190a0 (machine code checked)
		public T Get<T>(string key) where T : class
		{
			if (_assetContainers.TryGetValue(typeof(T), out var container))
			{
				return container.Get(key) as T;
			}
			return null;
		}
	}
}

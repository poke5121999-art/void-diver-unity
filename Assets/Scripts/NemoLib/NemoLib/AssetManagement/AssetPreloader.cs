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
			AddContainer<Texture2D>();
			AddContainer<SpriteAtlas>();
			AddContainer<Spine.Unity.SkeletonDataAsset>();
			AddContainer<GameObject>();
			AddContainer<AudioClip>();
			SpriteAtlasManager.atlasRequested += OnAtlasRequested;
		}

		// TODO(decomp): generic body not exported (address 0x0); the constructor registers one AssetContainer<T> per type, so this mirrors that
		public void AddContainer<T>() where T : UnityEngine.Object
		{
			_assetContainers.Add(typeof(T), new AssetContainer<T>());
		}

		public async UniTask EnsureAsync(string label, ReactiveProperty<float> progress = null)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(label, null).ToUniTask();
			if (locations == null || !locations.Any())
			{
				return;
			}
			ThreadPriority originalLoadingPriority = Application.backgroundLoadingPriority;
			Application.backgroundLoadingPriority = ThreadPriority.Normal;
			int originalUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
			int originalUploadBufferSize = QualitySettings.asyncUploadBufferSize;
			QualitySettings.asyncUploadTimeSlice = 16;
			QualitySettings.asyncUploadBufferSize = 64;
			// TODO(decomp): the saved original settings are never restored in the decompiled MoveNext; restoring them in finally is a guess @0x182D1B4D0
			try
			{
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
			}
			finally
			{
				Application.backgroundLoadingPriority = originalLoadingPriority;
				QualitySettings.asyncUploadTimeSlice = originalUploadTimeSlice;
				QualitySettings.asyncUploadBufferSize = originalUploadBufferSize;
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

		// TODO(decomp): generic body not exported (address 0x0); lookup by typeof(T) reconstructed
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

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

namespace NemoLib.AssetManagement
{
	internal class AssetContainer<T> : IAssetContainer
	{
		private readonly Dictionary<string, T> _objects = new Dictionary<string, T>();

		private readonly Dictionary<string, AsyncOperationHandle<IList<T>>> _handles = new Dictionary<string, AsyncOperationHandle<IList<T>>>();

		// <NemoLib-AssetManagement-IAssetContainer-EnsureAssetAsync>d__2<object>.MoveNext @0x180f80f10 (machine code checked): the handle is added (not overwritten) and the loop is bounded by assets.Count only
		async UniTask IAssetContainer.EnsureAssetAsync(string label, IList<IResourceLocation> locations)
		{
			IResourceLocation[] loadableLocations = GetLoadableLocations(locations);
			if (!loadableLocations.Any())
			{
				return;
			}
			AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(loadableLocations, null);
			IList<T> assets = await handle;
			_handles.Add(label, handle);
			for (int i = 0; i < assets.Count; i++)
			{
				_objects[loadableLocations[i].PrimaryKey] = assets[i];
			}
		}

		// Release<object> @0x181b8e1a0 and its lambda b__3_0 @0x180f39180 (machine code checked): only locations of type T are removed, and there is no null check on locations
		void IAssetContainer.Release(string label, IList<IResourceLocation> locations)
		{
			foreach (IResourceLocation item in locations.Where((IResourceLocation _) => _.ResourceType == typeof(T)))
			{
				_objects.Remove(item.PrimaryKey);
			}
			if (_handles.TryGetValue(label, out var handle))
			{
				Addressables.Release(handle);
				_handles.Remove(label);
			}
		}

		object IAssetContainer.Get(string key)
		{
			if (_objects.TryGetValue(key, out var value))
			{
				return value;
			}
			return null;
		}

		// GetLoadableLocations<object> @0x181b8c8f0 and lambdas b__5_0..6 @0x180f38600 etc. (machine code checked): Texture2D containers skip textures whose key is also a SpriteAtlas key
		private IResourceLocation[] GetLoadableLocations(IList<IResourceLocation> locations)
		{
			if (typeof(T) == typeof(Texture2D))
			{
				HashSet<string> atlasKeys = locations.Where((IResourceLocation _) => _.ResourceType == typeof(SpriteAtlas)).Select((IResourceLocation _) => _.PrimaryKey).ToHashSet();
				return locations.Where((IResourceLocation _) => _.ResourceType == typeof(T)).Where((IResourceLocation _) => !_objects.ContainsKey(_.PrimaryKey)).Where((IResourceLocation _) => !atlasKeys.Contains(_.PrimaryKey)).ToArray();
			}
			return locations.Where((IResourceLocation _) => _.ResourceType == typeof(T)).Where((IResourceLocation _) => !_objects.ContainsKey(_.PrimaryKey)).ToArray();
		}
	}
}

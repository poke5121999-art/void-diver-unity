using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace NemoLib.AssetManagement
{
	internal class AssetContainer<T> : IAssetContainer
	{
		private readonly Dictionary<string, T> _objects = new Dictionary<string, T>();

		private readonly Dictionary<string, AsyncOperationHandle<IList<T>>> _handles = new Dictionary<string, AsyncOperationHandle<IList<T>>>();

		// TODO(decomp): generic-shared body of AssetContainer<T> is not exported (address 0x0); logic below is
		// reconstructed from the async state machine fields (loadableLocations, handle, UniTask<IList<T>>) @0x0
		async UniTask IAssetContainer.EnsureAssetAsync(string label, IList<IResourceLocation> locations)
		{
			IResourceLocation[] loadableLocations = GetLoadableLocations(locations);
			if (loadableLocations.Length == 0)
			{
				return;
			}
			AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(loadableLocations, null);
			IList<T> assets = await handle.ToUniTask();
			_handles[label] = handle;
			for (int i = 0; i < loadableLocations.Length && i < assets.Count; i++)
			{
				_objects[loadableLocations[i].PrimaryKey] = assets[i];
			}
		}

		void IAssetContainer.Release(string label, IList<IResourceLocation> locations)
		{
			if (locations != null)
			{
				foreach (IResourceLocation location in locations.Where((IResourceLocation _) => _ != null))
				{
					_objects.Remove(location.PrimaryKey);
				}
			}
			if (_handles.TryGetValue(label, out var handle))
			{
				_handles.Remove(label);
				Addressables.Release(handle);
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

		private IResourceLocation[] GetLoadableLocations(IList<IResourceLocation> locations)
		{
			// TODO(decomp): filter lambdas <GetLoadableLocations>b__5_0..6 not readable @0x0
			if (locations == null)
			{
				return Array.Empty<IResourceLocation>();
			}
			return locations.Where((IResourceLocation _) => _ != null && _.ResourceType != null && typeof(T).IsAssignableFrom(_.ResourceType)).GroupBy((IResourceLocation _) => _.PrimaryKey).Select((IGrouping<string, IResourceLocation> g) => g.First()).Where((IResourceLocation _) => !_objects.ContainsKey(_.PrimaryKey)).ToArray();
		}
	}
}

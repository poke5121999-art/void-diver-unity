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
#if UNITY_WEBGL && !UNITY_EDITOR
			WatchLoadAsync(handle, label).Forget();
#endif
			IList<T> assets = await handle;
			_handles.Add(label, handle);
			for (int i = 0; i < assets.Count; i++)
			{
				_objects[loadableLocations[i].PrimaryKey] = assets[i];
			}
		}

#if UNITY_WEBGL && !UNITY_EDITOR
		// Web diagnostics for the preload that sometimes never ended (2026-10-30): every 30 s a load is still running,
		// log the operations under it that are not done, so the stuck one has a name.
		private static async UniTaskVoid WatchLoadAsync(AsyncOperationHandle handle, string label)
		{
			for (int round = 1; round <= 4; round++)
			{
				await UniTask.Delay(30000, ignoreTimeScale: true);
				if (!handle.IsValid() || handle.IsDone)
				{
					return;
				}
				var lines = new List<string>();
				DumpPending(handle, 0, lines, new HashSet<object>());
				Debug.LogWarning(string.Format("[AssetWatch] {0} {1} still loading after {2} s ({3:P0}), {4} operations not done:\n{5}", label, typeof(T).Name, round * 30, handle.PercentComplete, lines.Count, string.Join("\n", lines.Take(40))));
			}
		}

		private static void DumpPending(AsyncOperationHandle handle, int depth, List<string> lines, HashSet<object> seen)
		{
			if (!handle.IsValid() || depth > 6 || !seen.Add(handle.DebugName + "#" + handle.GetHashCode()))
			{
				return;
			}
			if (!handle.IsDone)
			{
				lines.Add(string.Format("{0}{1} status={2} {3:P0}", new string(' ', depth * 2), handle.DebugName, handle.Status, handle.PercentComplete));
			}
			var deps = new List<AsyncOperationHandle>();
			handle.GetDependencies(deps);
			foreach (AsyncOperationHandle dep in deps)
			{
				DumpPending(dep, depth + 1, lines, seen);
			}
		}
#endif

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

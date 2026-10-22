using System.Collections.Generic;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public class AssetPoolManager : MonoBehaviour
	{
		private readonly Dictionary<string, IAssetPool> _pools;

		public IAssetPool EnsureAssetPool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene = false)
		{
			return null;
		}

		public GameObject EnsureSpawn(string key, GameObject prefab, int capacity = 1, int maxSize = 10, bool destroyWithScene = false, Transform parent = null)
		{
			return null;
		}

		public GameObject Spawn(string key, Transform parent = null)
		{
			return null;
		}

		public void Clear(string key)
		{
		}

		public void ClearAll()
		{
		}

		private IAssetPool CreatePool(string key, GameObject prefab, int capacity, int maxSize, bool destroyWithScene)
		{
			return null;
		}
	}
}

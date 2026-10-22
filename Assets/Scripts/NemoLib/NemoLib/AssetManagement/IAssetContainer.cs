using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace NemoLib.AssetManagement
{
	internal interface IAssetContainer
	{
		UniTask EnsureAssetAsync(string label, IList<IResourceLocation> locations);

		void Release(string label, IList<IResourceLocation> locations);

		object Get(string key);
	}
}

using Cysharp.Threading.Tasks;

namespace NemoLib.AssetManagement
{
	internal interface IRecentlyCache
	{
		int Capacity { get; }

		UniTask EnsureAssetAsync(string key);

		void Release(string key);

		object Get(string key);

		UniTask<object> GetAsync(string key);

		void SetCapacity(int capacity);
	}
}

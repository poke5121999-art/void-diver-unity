using UnityEngine;

namespace NemoLib.AssetManagement
{
	public interface IAssetPool
	{
		GameObject Spawn(Transform parent);

		void Despawn(PoolableObject poolableObj, float delay = 0f);

		void Clear();
	}
}

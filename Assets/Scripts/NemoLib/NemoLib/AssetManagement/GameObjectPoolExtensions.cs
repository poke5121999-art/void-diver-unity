using UnityEngine;

namespace NemoLib.AssetManagement
{
	public static class GameObjectPoolExtensions
	{
		public static void DespawnOrDestroy(this GameObject go, float delay = 0f)
		{
			if (go == null)
			{
				return;
			}
			PoolableObject poolable = go.GetComponent<PoolableObject>();
			if (poolable == null)
			{
				Object.Destroy(go, delay);
			}
			else
			{
				poolable.Despawn(delay);
			}
		}
	}
}

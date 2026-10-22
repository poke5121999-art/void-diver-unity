using System.Threading;
using UniRx;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public class PoolableObject : MonoBehaviour
	{
		private CancellationTokenSource _despawnCts;

		public IAssetPool Pool { get; set; }

		public Subject<PoolableObject> OnDespawned { get; }

		public bool IsInPool { get; private set; }

		public CancellationToken DespawnToken => default;

		public void Despawn(float delay = 0f)
		{
		}

		internal void OnSpawn()
		{
		}

		internal void OnDespawn()
		{
		}

		private void OnDestroy()
		{
		}
	}
}

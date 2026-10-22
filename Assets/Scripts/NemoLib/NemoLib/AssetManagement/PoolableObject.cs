using System.Threading;
using UniRx;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public class PoolableObject : MonoBehaviour
	{
		private CancellationTokenSource _despawnCts;

		public IAssetPool Pool { get; set; }

		public Subject<PoolableObject> OnDespawned { get; } = new Subject<PoolableObject>();

		public bool IsInPool { get; private set; }

		public CancellationToken DespawnToken
		{
			get
			{
				if (_despawnCts == null)
				{
					_despawnCts = new CancellationTokenSource();
				}
				return _despawnCts.Token;
			}
		}

		public void Despawn(float delay = 0f)
		{
			if (IsInPool)
			{
				return;
			}
			if (Pool != null)
			{
				Pool.Despawn(this, delay);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		internal void OnSpawn()
		{
			IsInPool = false;
			ResetCts();
		}

		private void ResetCts()
		{
			_despawnCts?.Cancel();
			_despawnCts?.Dispose();
			_despawnCts = null;
		}

		internal void OnDespawn()
		{
			IsInPool = true;
			_despawnCts?.Cancel();
			OnDespawned.OnNext(this);
		}

		private void OnDestroy()
		{
			if (!IsInPool)
			{
				OnDespawned.OnNext(this);
			}
			_despawnCts?.Cancel();
			_despawnCts?.Dispose();
			_despawnCts = null;
			OnDespawned.Dispose();
		}
	}
}

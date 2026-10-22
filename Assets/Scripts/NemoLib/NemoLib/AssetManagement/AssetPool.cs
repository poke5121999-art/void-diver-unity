using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	internal class AssetPool : MonoBehaviour, IAssetPool
	{
		private GameObject _prefab;

		private int _maxSize;

		private readonly Stack<PoolableObject> _objects = new Stack<PoolableObject>();

		public Subject<Unit> OnDestroyed { get; } = new Subject<Unit>();

		private void OnDestroy()
		{
			OnDestroyed.OnNext(Unit.Default);
			OnDestroyed.OnCompleted();
			OnDestroyed.Dispose();
		}

		public void Setup(GameObject prefab, int capacity, int maxSize)
		{
			_prefab = prefab;
			_maxSize = maxSize;
			for (int i = 0; i < capacity; i++)
			{
				if (_objects.Count >= _maxSize)
				{
					break;
				}
				PoolableObject poolableObject = CreatePoolableObject();
				if (poolableObject != null)
				{
					poolableObject.Despawn();
				}
			}
		}

		public GameObject Spawn(Transform parent)
		{
			PoolableObject poolableObject = ((_objects.Count > 0) ? _objects.Pop() : CreatePoolableObject());
			if (poolableObject == null)
			{
				UnityEngine.Debug.LogError("[AssetPool] Spawn instance is null. Prefab : " + ((_prefab != null) ? _prefab.name : "null"));
				return null;
			}
			poolableObject.OnSpawn();
			GameObject gameObject = poolableObject.gameObject;
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			gameObject.SetActive(value: true);
			return gameObject;
		}

		public void Despawn(PoolableObject poolableObj, float delay = 0f)
		{
			if (poolableObj == null)
			{
				return;
			}
			if (delay > 0f)
			{
				DespawnDelayedAsync(poolableObj, delay).Forget();
			}
			else
			{
				DespawnImmediate(poolableObj);
			}
		}

		public void Clear()
		{
			foreach (PoolableObject item in _objects)
			{
				Object.Destroy(item.gameObject);
			}
			_objects.Clear();
		}

		private async UniTask DespawnDelayedAsync(PoolableObject poolableObj, float delay)
		{
			await UniTask.Delay(System.TimeSpan.FromSeconds(delay), ignoreTimeScale: false, PlayerLoopTiming.Update, poolableObj.DespawnToken);
			if (!(this == null) && !(poolableObj == null))
			{
				DespawnImmediate(poolableObj);
			}
		}

		private void DespawnImmediate(PoolableObject poolableObj)
		{
			if (poolableObj == null || poolableObj.IsInPool)
			{
				return;
			}
			GameObject gameObject = poolableObj.gameObject;
			gameObject.SetActive(value: false);
			if (_objects.Count >= _maxSize)
			{
				Object.Destroy(gameObject);
				return;
			}
			poolableObj.OnDespawn();
			poolableObj.transform.SetParent(base.transform, worldPositionStays: false);
			_objects.Push(poolableObj);
		}

		private PoolableObject CreatePoolableObject()
		{
			GameObject gameObject = Object.Instantiate(_prefab);
			PoolableObject poolableObject = gameObject.GetComponent<PoolableObject>();
			if (poolableObject == null)
			{
				poolableObject = gameObject.AddComponent<PoolableObject>();
			}
			poolableObject.Pool = this;
			return poolableObject;
		}
	}
}

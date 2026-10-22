using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class CollisionTrigger : NetworkBase, ICollisionObject, ITriggerable
{
	[SerializeField]
	private int _triggerId;

	[SerializeField]
	private float _coolTime;

	[SerializeField]
	private bool _isIndividual;

	[SerializeField]
	private bool _canTriggered;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	private float _nextGlobalInteractableTime;

	private readonly Dictionary<ulong, float> _nextInteractableTimes = new Dictionary<ulong, float>();

	public int TriggerId => _triggerId;

	public bool IsIndividual => _isIndividual;

	public bool CanTriggered => _canTriggered;

	public Vector3 Position => base.transform.position;

	public NetworkBase NetworkBase => this;

	private float TimeNow => _gameManager.TimeNow;

	ulong ICollisionObject.NetworkObjectId => base.NetworkObjectId;

	public void Set(int triggerId, float coolTime, bool isIndividual, bool canTriggered)
	{
		_canTriggered = canTriggered;
		_coolTime = coolTime;
		_triggerId = triggerId;
		_isIndividual = isIndividual;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnCollisionObjectSpawned.OnNext(this);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnCollisionObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	public void SetTriggerable(bool canTriggered)
	{
		_canTriggered = canTriggered;
	}

	private float GetNextInteractableTime(ulong clientId)
	{
		if (!_isIndividual)
		{
			return _nextGlobalInteractableTime;
		}
		return _nextInteractableTimes.GetValueOrDefault(clientId);
	}

	private void SetNextInteractableTime(ulong clientId, float time)
	{
		if (!_isIndividual)
		{
			_nextGlobalInteractableTime = time;
			return;
		}
		_nextInteractableTimes[clientId] = time;
	}

	bool ICollisionObject.CanTrigger(ulong clientId)
	{
		if (_canTriggered && GetNextInteractableTime(clientId) <= TimeNow)
		{
			return true;
		}
		return false;
	}

	void ICollisionObject.OnTriggerEnter(ulong clientId)
	{
		if (base.IsHost)
		{
			float time = ((_coolTime < 0f) ? float.MaxValue : (TimeNow + _coolTime));
			SetNextInteractableTime(clientId, time);
		}
	}



}

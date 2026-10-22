using System.Collections.Generic;
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

	private readonly Dictionary<ulong, float> _nextInteractableTimes;

	public int TriggerId => 0;

	public bool IsIndividual => false;

	public bool CanTriggered => false;

	public Vector3 Position => default;

	public NetworkBase NetworkBase => null;

	private float TimeNow => 0f;

	ulong ICollisionObject.NetworkObjectId => 0uL;

	public void Set(int triggerId, float coolTime, bool isIndividual, bool canTriggered)
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	public void SetTriggerable(bool canTriggered)
	{
	}

	private float GetNextInteractableTime(ulong clientId)
	{
		return 0f;
	}

	private void SetNextInteractableTime(ulong clientId, float time)
	{
	}

	bool ICollisionObject.CanTrigger(ulong clientId)
	{
		return false;
	}

	void ICollisionObject.OnTriggerEnter(ulong clientId)
	{
	}



}

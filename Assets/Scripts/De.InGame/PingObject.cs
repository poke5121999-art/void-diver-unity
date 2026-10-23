using GameCommon.Enum;
using UnityEngine;
using VContainer;

public class PingObject : MonoBase
{
	[SerializeField]
	private EPingType _pingType;

	[Inject]
	private GameObjectManager _objectManager;

	public EPingType PingType => _pingType;

	private void Start()
	{
		_objectManager.OnPingObjectSpawned.OnNext(this);
	}

	private void OnDestroy()
	{
		_objectManager.OnPingObjectDespawned.OnNext(this);
	}
}

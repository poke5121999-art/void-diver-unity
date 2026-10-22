using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PersistentPlayer : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
	}

	public override void OnDestroy()
	{
	}

	public override void OnNetworkDespawn()
	{
	}



}

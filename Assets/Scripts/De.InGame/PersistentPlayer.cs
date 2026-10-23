using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PersistentPlayer : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
		base.gameObject.name = string.Format("[PersistentPlayer] {0}", base.OwnerClientId);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void OnNetworkDespawn()
	{
	}



}

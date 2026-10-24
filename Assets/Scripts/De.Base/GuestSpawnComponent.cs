using UnityEngine;

public class GuestSpawnComponent : MonoBehaviour
{
	public int Id = -1;

	public GuestSpawnData ToData()
	{
		return new GuestSpawnData
		{
			Id = Id,
			SpawnPosition = base.transform.position
		};
	}
}

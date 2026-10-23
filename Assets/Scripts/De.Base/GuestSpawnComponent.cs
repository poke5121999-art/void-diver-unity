using UnityEngine;

public class GuestSpawnComponent : MonoBehaviour
{
	public int Id;

	public GuestSpawnData ToData()
	{
		return new GuestSpawnData
		{
			Id = Id,
			SpawnPosition = base.transform.position
		};
	}
}

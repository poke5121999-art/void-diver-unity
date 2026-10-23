using UnityEngine;

public class PhoneBoothSpawnComponent : MonoBehaviour
{
	public PhoneBoothSpawnData ToData()
	{
		return new PhoneBoothSpawnData
		{
			SpawnPosition = base.transform.position
		};
	}
}

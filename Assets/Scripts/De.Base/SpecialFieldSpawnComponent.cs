using UnityEngine;

public class SpecialFieldSpawnComponent : MonoBehaviour
{
	public int SpecialFieldId;

	public Vector3 Scale = Vector3.one;

	public SpecialFieldSpawnData ToData()
	{
		return new SpecialFieldSpawnData
		{
			SpecialFieldId = SpecialFieldId,
			SpawnPosition = base.transform.position,
			Scale = Scale
		};
	}
}

using UnityEngine;

public class AreaDecorationObjectSpawnComponent : MonoBehaviour
{
	public int GroupId;

	public string[] PrefabNames;

	public bool IsFlipped;

	public AreaDecorationObjectSpawnData ToData()
	{
		AreaDecorationObjectSpawnData areaDecorationObjectSpawnData = new AreaDecorationObjectSpawnData();
		areaDecorationObjectSpawnData.GroupId = GroupId;
		areaDecorationObjectSpawnData.SpawnPosition = base.transform.position;
		areaDecorationObjectSpawnData.PrefabNames = PrefabNames;
		areaDecorationObjectSpawnData.IsFlipped = IsFlipped;
		return areaDecorationObjectSpawnData;
	}
}

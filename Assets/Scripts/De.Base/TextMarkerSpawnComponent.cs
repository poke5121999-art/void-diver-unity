using UnityEngine;

public class TextMarkerSpawnComponent : MonoBehaviour
{
	public string TextKey;

	public TextMarkerSpawnData ToData()
	{
		return new TextMarkerSpawnData
		{
			TextKey = TextKey,
			SpawnPosition = base.transform.position
		};
	}
}

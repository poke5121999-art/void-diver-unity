using UnityEngine;

public class MapLoadedPrefabMarker : MonoBehaviour
{
	[Tooltip("로드된 프리팹 이름")]
	public string PrefabName;

	[Tooltip("로드된 시간")]
	public string LoadedTime;

	[Tooltip("로드한 씬 이름")]
	public string SceneName;
}

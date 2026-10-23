using UnityEngine;

public abstract class SpawnGroupComponent : MonoBehaviour
{
	public bool SpawnAll;

	public float SpawnCount;

	private void Start()
	{
		Object.Destroy(base.gameObject);
	}
}

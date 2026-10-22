using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	private void Start()
	{
		if (base.enabled)
		{
			Object.Destroy(base.gameObject);
		}
	}
}

using UnityEngine;

public class ShaderBufferPoint : MonoBehaviour
{
	[SerializeField]
	private string _category;

	public string Category => _category;

	private void OnEnable()
	{
		ShaderBufferPointManager.Register(this);
	}

	private void OnDisable()
	{
		ShaderBufferPointManager.Unregister(this);
	}

	private void OnDrawGizmos()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		float radius = Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z));
		Gizmos.color = new Color(0f, 1f, 1f, 1f);
		Gizmos.DrawWireSphere(base.transform.position, radius);
	}
}

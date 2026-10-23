using UnityEngine;

public class WorldUIBoxAreaHider : MonoBehaviour
{
	private Vector3 _originalScale;

	private void Awake()
	{
		_originalScale = base.transform.localScale;
	}

	private void LateUpdate()
	{
		base.transform.localScale = (BackgroundBoxAreaEffectManager.IsInsideCoverWorldUI(base.transform.position) ? Vector3.zero : _originalScale);
	}
}

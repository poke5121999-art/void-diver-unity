using UnityEngine;

public class ColliderDrawer : MonoBehaviour
{
	[SerializeField]
	private bool _isDraw = true;

	[SerializeField]
	private Color _color = new Color(0f, 1f, 0f, 1f);

	private Collider _collider;
}

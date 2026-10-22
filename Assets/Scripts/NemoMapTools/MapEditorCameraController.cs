using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class MapEditorCameraController : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField]
	private float moveSpeed = 5f;

	[SerializeField]
	private float fastMoveMultiplier = 2f;

	[FormerlySerializedAs("camera")]
	[SerializeField]
	private Camera editorCamera;

	private Vector2 moveInput;

	private bool isFastMove;
}

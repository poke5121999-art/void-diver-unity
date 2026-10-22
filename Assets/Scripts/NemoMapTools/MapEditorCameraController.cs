using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class MapEditorCameraController : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float fastMoveMultiplier;

	[FormerlySerializedAs("camera")]
	[SerializeField]
	private Camera editorCamera;

	private Vector2 moveInput;

	private bool isFastMove;
}

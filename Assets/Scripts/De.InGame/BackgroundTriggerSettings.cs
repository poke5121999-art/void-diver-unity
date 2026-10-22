using UnityEngine;

[CreateAssetMenu(fileName = "Background Trigger Setting", menuName = "NEMO Art/Background Trigger Setting")]
public class BackgroundTriggerSettings : ScriptableObject
{
	[SerializeField]
	private float _roofColliderBump;

	[Header("Camera View (Roof Offset 계산용)")]
	[Tooltip("카메라 down angle (X-axis rotation degrees). 30 = 30도 아래로 기울임")]
	[SerializeField]
	private float _cameraDownAngleDeg;

	[Tooltip("카메라 yaw angle (Y-axis rotation degrees). 315 = -45 = 화면 우상단이 +X+Z 방향")]
	[SerializeField]
	private float _cameraYawAngleDeg;

	[Header("Roof Trigger Y Range")]
	[Tooltip("Roof trigger BC 의 Y 영역 (min, max). BC.size.y = max-min, BC.center.y = (max+min)/2")]
	[SerializeField]
	private Vector2 _roofTriggerYRange;

	public float RoofColliderBump => 0f;

	public Vector2 RoofTriggerYRange => default;

	public Vector3 GetRoofColliderOffset(float maxY)
	{
		return default;
	}
}

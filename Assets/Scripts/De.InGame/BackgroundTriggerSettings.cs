using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Background Trigger Setting", menuName = "NEMO Art/Background Trigger Setting")]
public class BackgroundTriggerSettings : ScriptableObject
{
	[SerializeField]
	private float _roofColliderBump = 5f;

	[Header("Camera View (Roof Offset 계산용)")]
	[Tooltip("카메라 down angle (X-axis rotation degrees). 30 = 30도 아래로 기울임")]
	[SerializeField]
	private float _cameraDownAngleDeg = 30f;

	[Tooltip("카메라 yaw angle (Y-axis rotation degrees). 315 = -45 = 화면 우상단이 +X+Z 방향")]
	[SerializeField]
	private float _cameraYawAngleDeg = 315f;

	[Header("Roof Trigger Y Range")]
	[Tooltip("Roof trigger BC 의 Y 영역 (min, max). BC.size.y = max-min, BC.center.y = (max+min)/2")]
	[SerializeField]
	private Vector2 _roofTriggerYRange = new Vector2(0f, 2f);

	public float RoofColliderBump => _roofColliderBump;

	public Vector2 RoofTriggerYRange => _roofTriggerYRange;

	public Vector3 GetRoofColliderOffset(float maxY)
	{
		float yaw = _cameraYawAngleDeg * ((float)Math.PI / 180f);
		float down = _cameraDownAngleDeg * ((float)Math.PI / 180f);
		float sinYaw = Mathf.Sin(yaw);
		float cosYaw = Mathf.Cos(yaw);
		float sinDown = Mathf.Sin(down);
		if (sinDown <= Mathf.Epsilon)
		{
			return Vector3.zero;
		}
		float dist = Mathf.Cos(down) / sinDown * maxY;
		return new Vector3(sinYaw * dist, 0f, cosYaw * dist);
	}
}

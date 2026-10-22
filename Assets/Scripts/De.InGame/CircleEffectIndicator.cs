using UnityEngine;

public class CircleEffectIndicator : SkillIndicator
{
	private float _castRange;

	public void Set(float radius, float castRange)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		base.transform.localScale = new Vector3(radius, 1f, radius);
		_castRange = castRange;
	}

	private void Update()
	{
		float castRange = _castRange;
		Vector3 position = unitModel.transform.position;
		Vector3 target;
		if (unitModel.DeltaAimPos.magnitude > castRange)
		{
			target = position + unitModel.DeltaAimPos.normalized * castRange;
		}
		else
		{
			target = position + unitModel.DeltaAimPos;
		}
		// Const.Layer.HIGH_OBSTACLE (De.Const is not referenced by the De.InGame asmdef here)
		Vector3 toTarget = target - position;
		float distance = toTarget.magnitude;
		if (distance > 0f && Physics.Raycast(position, toTarget.normalized, out var hitInfo, distance, 1 << LayerMask.NameToLayer("HighObstacle")))
		{
			target.x = hitInfo.point.x;
			target.z = hitInfo.point.z;
		}
		base.transform.position = target;
	}
}

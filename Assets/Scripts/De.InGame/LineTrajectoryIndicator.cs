using UnityEngine;

public class LineTrajectoryIndicator : SkillIndicator
{
	public void Set(float length)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		base.transform.localScale = new Vector3(1f, 1f, length);
	}

	private void Update()
	{
		Vector3 value = unitModel.RxDeltaAimPos.Value;
		if (value.sqrMagnitude > 0f)
		{
			base.transform.rotation = Quaternion.LookRotation(value, Vector3.up);
		}
	}
}

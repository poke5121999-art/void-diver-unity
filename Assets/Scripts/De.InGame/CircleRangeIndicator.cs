using UnityEngine;

public class CircleRangeIndicator : SkillIndicator
{
	public void Set(float radius)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		base.transform.localScale = new Vector3(radius, 1f, radius);
	}
}

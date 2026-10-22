using UnityEngine;

public class BackgroundRoofTriggerAdjuster : BackgroundTriggerAdjuster
{
	protected override Vector3 GetTriggerOffset()
	{
		if (BackgroundTriggerSettings != null)
		{
			return BackgroundTriggerSettings.GetRoofColliderOffset(BoxColliderMaxY);
		}
		return Vector3.zero;
	}

	protected override float GetTriggerBump()
	{
		if (!(BackgroundTriggerSettings != null))
		{
			return 0f;
		}
		return BackgroundTriggerSettings.RoofColliderBump;
	}

	protected override Vector2 GetTriggerYRange()
	{
		if (BackgroundTriggerSettings == null)
		{
			return new Vector2(float.NaN, float.NaN);
		}
		return BackgroundTriggerSettings.RoofTriggerYRange;
	}
}

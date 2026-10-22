using UnityEngine;

public class BackgroundRoofTriggerAdjuster : BackgroundTriggerAdjuster
{
	protected override Vector3 GetTriggerOffset()
	{
		return default;
	}

	protected override float GetTriggerBump()
	{
		return 0f;
	}

	protected override Vector2 GetTriggerYRange()
	{
		return default;
	}
}

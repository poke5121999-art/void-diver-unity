using UnityEngine;

public class ZonePointComponent : MonoBehaviour
{
	public EZoneFlags ZoneFlags;

	public ZonePointData ToData()
	{
		return new ZonePointData
		{
			ZoneFlags = ZoneFlags,
			Position = base.transform.position
		};
	}
}

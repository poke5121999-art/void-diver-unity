using UnityEngine;

public class PortalExitComponent : MonoBehaviour
{
	public int Id;

	public PortalExitData ToData()
	{
		return new PortalExitData
		{
			Id = Id,
			Position = base.transform.position
		};
	}
}

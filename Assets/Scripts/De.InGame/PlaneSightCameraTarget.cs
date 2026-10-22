using UnityEngine;

public class PlaneSightCameraTarget : MonoBehaviour
{
	[PlaneSightTargetCameraLayer]
	[SerializeField]
	private int _layerIndex;

	public int LayerIndex => 0;
}

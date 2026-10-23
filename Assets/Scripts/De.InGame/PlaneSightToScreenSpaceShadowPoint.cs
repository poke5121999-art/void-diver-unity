using UnityEngine;

public class PlaneSightToScreenSpaceShadowPoint : MonoBehaviour
{
	[SerializeField]
	private FakeShadowPoint _screenSpaceShadowPoint;

	private void Start()
	{
		if ((bool)_screenSpaceShadowPoint)
		{
			int globalInt = Shader.GetGlobalInt("_GlobalPlaneSystemEnable");
			_screenSpaceShadowPoint.enabled = globalInt == 1;
		}
	}
}

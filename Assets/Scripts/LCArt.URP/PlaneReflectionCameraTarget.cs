using UnityEngine;

public class PlaneReflectionCameraTarget : MonoBehaviour
{
	[Header("Reflection Setting")]
	[SerializeField]
	private PlaneReflectionSettings _planeReflectionSettings;

	public PlaneReflectionSettings PlaneReflectionSettings => null;

	public void ChangeSettings(PlaneReflectionSettings settings)
	{
	}
}

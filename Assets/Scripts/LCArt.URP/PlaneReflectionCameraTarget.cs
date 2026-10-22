using UnityEngine;

public class PlaneReflectionCameraTarget : MonoBehaviour
{
	[Header("Reflection Setting")]
	[SerializeField]
	private PlaneReflectionSettings _planeReflectionSettings;

	public PlaneReflectionSettings PlaneReflectionSettings => _planeReflectionSettings;

	public void ChangeSettings(PlaneReflectionSettings settings)
	{
		_planeReflectionSettings = settings;
	}
}

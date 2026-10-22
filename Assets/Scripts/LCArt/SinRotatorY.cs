using UnityEngine;

public class SinRotatorY : MonoBehaviour
{
	[Header("Rotation Settings")]
	[SerializeField]
	private float _rotationAngle;

	[SerializeField]
	private float _rotationSpeed;

	[Header("Control")]
	[SerializeField]
	private bool _useUnscaledTime;

	private Vector3 _initialRotation;

	private float _currentTime;

	private void Start()
	{
	}

	private void Update()
	{
	}
}

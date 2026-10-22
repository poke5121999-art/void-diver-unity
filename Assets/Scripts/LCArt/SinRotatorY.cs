using System;
using UnityEngine;

public class SinRotatorY : MonoBehaviour
{
	[Header("Rotation Settings")]
	[SerializeField]
	private float _rotationAngle = 30f;

	[SerializeField]
	private float _rotationSpeed = 1f;

	[Header("Control")]
	[SerializeField]
	private bool _useUnscaledTime;

	private Vector3 _initialRotation;

	private float _currentTime;

	private void Start()
	{
		_initialRotation = base.transform.eulerAngles;
		_currentTime = 0f;
	}

	private void Update()
	{
		_currentTime += (_useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
		float num = Mathf.Sin(_currentTime * _rotationSpeed * 2f * (float)Math.PI) * _rotationAngle;
		base.transform.rotation = Quaternion.Euler(_initialRotation.x, _initialRotation.y + num, _initialRotation.z);
	}
}

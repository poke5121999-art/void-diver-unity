using UnityEngine;

[ExecuteAlways]
public class IndicatorTransformKeeper : MonoBehaviour
{
	[SerializeField]
	private bool _isRotationFixed;

	[SerializeField]
	private Vector3 _targetRotation;

	[SerializeField]
	private bool _isLocalScaleFixed;

	[SerializeField]
	private Vector3 _targetLocalScale;

	private void LateUpdate()
	{
	}

	private void UpdateLocalScale()
	{
	}

	private void UpdateRotation()
	{
	}
}

using UnityEngine;

[ExecuteAlways]
public class IndicatorTransformKeeper : MonoBehaviour
{
	[SerializeField]
	private bool _isRotationFixed;

	[SerializeField]
	private Vector3 _targetRotation = Vector3.zero;

	[SerializeField]
	private bool _isLocalScaleFixed;

	[SerializeField]
	private Vector3 _targetLocalScale = Vector3.one;

	private void LateUpdate()
	{
		UpdateLocalScale();
		UpdateRotation();
	}

	private void UpdateLocalScale()
	{
		if (!_isLocalScaleFixed || _targetLocalScale.sqrMagnitude < Mathf.Epsilon)
		{
			return;
		}
		Vector3 vector = ((!(base.transform.parent != null)) ? Vector3.one : base.transform.parent.lossyScale);
		if (vector.x == 0f)
		{
			vector.x = 1f;
		}
		if (vector.y == 0f)
		{
			vector.y = 1f;
		}
		if (vector.z == 0f)
		{
			vector.z = 1f;
		}
		Vector3 vector2 = new Vector3(_targetLocalScale.x / vector.x, _targetLocalScale.y / vector.y, _targetLocalScale.z / vector.z);
		if (!((base.transform.localScale - vector2).sqrMagnitude < 1E-10f))
		{
			base.transform.localScale = vector2;
		}
	}

	private void UpdateRotation()
	{
		if (_isRotationFixed)
		{
			base.transform.rotation = Quaternion.Euler(_targetRotation);
		}
	}
}

using UnityEngine;

[ExecuteAlways]
public class PositionKeeper : MonoBehaviour
{
	[Header("보정할 축 선택")]
	public bool compensateX;

	public bool compensateY;

	public bool compensateZ;

	private Transform parentTransform;

	private Vector3 lastParentScale;

	private Vector3 calculatedTargetOffset;

	private void OnEnable()
	{
	}

	private void RecalculateTargetOffset()
	{
	}

	private void LateUpdate()
	{
	}

	private void ApplyCompensation(Vector3 parentScale)
	{
	}

	private void ApplyCompensation()
	{
	}
}

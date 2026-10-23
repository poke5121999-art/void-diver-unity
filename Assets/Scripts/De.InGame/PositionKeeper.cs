using UnityEngine;

[ExecuteAlways]
public class PositionKeeper : MonoBehaviour
{
	[Header("보정할 축 선택")]
	public bool compensateX = true;

	public bool compensateY = true;

	public bool compensateZ = true;

	private Transform parentTransform;

	private Vector3 lastParentScale;

	private Vector3 calculatedTargetOffset;

	private void OnEnable()
	{
		parentTransform = base.transform.parent;
		if (parentTransform == null)
		{
			if (Application.isPlaying)
			{
				Debug.LogWarning("CompensateParentScale: 부모가 없습니다.", this);
			}
		}
		else if (compensateX || compensateY || compensateZ)
		{
			RecalculateTargetOffset();
			lastParentScale = parentTransform.localScale;
			if (parentTransform != null)
			{
				ApplyCompensation(parentTransform.localScale);
			}
			return;
		}
		base.enabled = false;
	}

	private void RecalculateTargetOffset()
	{
		if (parentTransform == null)
		{
			return;
		}
		Vector3 localScale = parentTransform.localScale;
		if (localScale.x == 0f)
		{
			localScale.x = 1f;
		}
		if (localScale.y == 0f)
		{
			localScale.y = 1f;
		}
		if (localScale.z == 0f)
		{
			localScale.z = 1f;
		}
		calculatedTargetOffset = new Vector3(base.transform.localPosition.x * localScale.x, base.transform.localPosition.y * localScale.y, localScale.z * base.transform.localPosition.z);
	}

	private void LateUpdate()
	{
		if (parentTransform == null)
		{
			if (base.enabled)
			{
				base.enabled = false;
			}
			return;
		}
		Vector3 localScale = parentTransform.localScale;
		if (localScale != lastParentScale)
		{
			ApplyCompensation(localScale);
			lastParentScale = localScale;
		}
		else if (!Application.isPlaying)
		{
			RecalculateTargetOffset();
		}
	}

	private void ApplyCompensation(Vector3 parentScale)
	{
		if (parentScale.x == 0f)
		{
			parentScale.x = 0.0001f;
		}
		if (parentScale.y == 0f)
		{
			parentScale.y = 0.0001f;
		}
		if (parentScale.z == 0f)
		{
			parentScale.z = 0.0001f;
		}
		float x = calculatedTargetOffset.x;
		if (compensateX)
		{
			x /= parentScale.x;
		}
		float y = calculatedTargetOffset.y;
		if (compensateY)
		{
			y /= parentScale.y;
		}
		float z = calculatedTargetOffset.z;
		if (compensateZ)
		{
			z /= parentScale.z;
		}
		base.transform.localPosition = new Vector3(x, y, z);
	}

	private void ApplyCompensation()
	{
		if (parentTransform != null)
		{
			ApplyCompensation(parentTransform.localScale);
		}
	}
}

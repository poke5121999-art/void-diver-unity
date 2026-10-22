using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomMultiAnchor : MonoBehaviour
{
	private struct BoundsState
	{
		public float Left;

		public float Right;

		public float Bottom;

		public float Top;

		public float PivotX;

		public float PivotY;
	}

	private const string UsageWarningText = "Used to adjust effect positions or ranges in response to layout changes.\nDo not use on objects/situations that move or resize every frame.\nResizing may incur Canvas rebuild costs.";

	[SerializeField]
	[TextArea(3, 5)]
	private string usageWarning = "Used to adjust effect positions or ranges in response to layout changes.\nDo not use on objects/situations that move or resize every frame.\nResizing may incur Canvas rebuild costs.";

	[Header("Anchor Targets")]
	public RectTransform leftTarget;

	public RectTransform rightTarget;

	public RectTransform bottomTarget;

	public RectTransform topTarget;

	[Header("Offsets")]
	public int leftOffset;

	public int rightOffset;

	public int bottomOffset;

	public int topOffset;

	[Header("Pivot Follow")]
	public RectTransform pivotTarget;

	public bool followPivotX;

	public bool followPivotY;

	[Header("Editor")]
	public bool showGizmos = true;

	private const float MinSize = 2f;

	private const float Epsilon = 0.001f;

	private RectTransform rect;

	private Transform cachedParent;

	private bool isDirty = true;

	private bool isApplying;

	private bool isForcingCanvasUpdate;

	private int lastAutoApplyFrame = -1;

	private RectTransform lastLeftTarget;

	private RectTransform lastRightTarget;

	private RectTransform lastBottomTarget;

	private RectTransform lastTopTarget;

	private RectTransform lastPivotTarget;

	private bool lastFollowPivotX;

	private bool lastFollowPivotY;

	private int lastLeftOffset;

	private int lastRightOffset;

	private int lastBottomOffset;

	private int lastTopOffset;

	private Vector2 lastSizeDelta;

	private Vector3 lastLocalPosition;

	private Vector2 lastPivot;

	private BoundsState lastBoundsState;

	private readonly Vector3[] worldCorners = new Vector3[4];

	private void Awake()
	{
		CacheComponents();
	}

	private void OnEnable()
	{
		CacheComponents();
		Canvas.willRenderCanvases += HandleWillRenderCanvases;
		isDirty = true;
		ApplyIfNeeded(force: true);
	}

	private void OnDisable()
	{
		Canvas.willRenderCanvases -= HandleWillRenderCanvases;
	}

	private void OnValidate()
	{
		CacheComponents();
		RestoreUsageWarning();
		isDirty = true;
		ApplyIfNeeded(force: true);
	}

	private void Reset()
	{
		RestoreUsageWarning();
	}

	private void OnTransformParentChanged()
	{
		cachedParent = base.transform.parent;
		isDirty = true;
	}

	private void OnRectTransformDimensionsChange()
	{
		if (!isApplying)
		{
			isDirty = true;
		}
	}

	private void HandleWillRenderCanvases()
	{
		if (isApplying)
		{
			return;
		}
		if (Application.isPlaying && lastAutoApplyFrame == Time.frameCount)
		{
			return;
		}
		if (Application.isPlaying)
		{
			lastAutoApplyFrame = Time.frameCount;
		}
		ApplyIfNeeded(force: false);
	}

	public void Refresh()
	{
		isDirty = true;
		ApplyIfNeeded(force: true);
	}

	private void CacheComponents()
	{
		if (rect == null)
		{
			rect = GetComponent<RectTransform>();
		}
		cachedParent = base.transform.parent;
	}

	private void MarkDirty()
	{
		isDirty = true;
	}

	private void RestoreUsageWarning()
	{
		if (usageWarning != "Used to adjust effect positions or ranges in response to layout changes.\nDo not use on objects/situations that move or resize every frame.\nResizing may incur Canvas rebuild costs.")
		{
			usageWarning = "Used to adjust effect positions or ranges in response to layout changes.\nDo not use on objects/situations that move or resize every frame.\nResizing may incur Canvas rebuild costs.";
		}
	}

	private void ApplyIfNeeded(bool force)
	{
		if (rect == null || cachedParent == null)
		{
			return;
		}
		BoundsState boundsState = CalculateBoundsState();
		if (!force && !isDirty && !HasInspectorStateChanged() && !HasBoundsStateChanged(boundsState))
		{
			return;
		}
		isApplying = true;
		try
		{
			bool flag = ApplyBounds(boundsState);
			CacheAppliedState(boundsState);
			isDirty = false;
			if (flag)
			{
				ForceCanvasUpdateIfNeeded();
			}
		}
		finally
		{
			isApplying = false;
		}
	}

	private BoundsState CalculateBoundsState()
	{
		BoundsState result = default(BoundsState);
		rect.GetWorldCorners(worldCorners);
		Vector3 vector = cachedParent.InverseTransformPoint(worldCorners[0]);
		Vector3 vector2 = cachedParent.InverseTransformPoint(worldCorners[2]);
		float x = vector.x;
		float y = vector.y;
		float x2 = vector2.x;
		float y2 = vector2.y;
		Vector2 pivot = rect.pivot;
		if (leftTarget != null)
		{
			x = cachedParent.InverseTransformPoint(leftTarget.position).x + (float)leftOffset;
		}
		if (rightTarget != null)
		{
			x2 = cachedParent.InverseTransformPoint(rightTarget.position).x + (float)rightOffset;
		}
		if (bottomTarget != null)
		{
			y = cachedParent.InverseTransformPoint(bottomTarget.position).y + (float)bottomOffset;
		}
		if (topTarget != null)
		{
			y2 = cachedParent.InverseTransformPoint(topTarget.position).y + (float)topOffset;
		}
		float num = Mathf.Max(2f, x2 - x);
		float num2 = Mathf.Max(2f, y2 - y);
		float x3 = pivot.x;
		float y3 = pivot.y;
		if (pivotTarget != null && (followPivotX || followPivotY))
		{
			Vector3 vector3 = cachedParent.InverseTransformPoint(pivotTarget.position);
			if (followPivotX)
			{
				x3 = (vector3.x - x) / num;
			}
			if (followPivotY)
			{
				y3 = (vector3.y - y) / num2;
			}
		}
		result.Left = x;
		result.Right = x + num;
		result.Bottom = y;
		result.Top = y + num2;
		result.PivotX = x3;
		result.PivotY = y3;
		return result;
	}

	private BoundsState CalculateCurrentBounds()
	{
		BoundsState result = default(BoundsState);
		rect.GetWorldCorners(worldCorners);
		Vector3 vector = cachedParent.InverseTransformPoint(worldCorners[0]);
		Vector3 vector2 = cachedParent.InverseTransformPoint(worldCorners[2]);
		result.Left = vector.x;
		result.Right = vector2.x;
		result.Bottom = vector.y;
		result.Top = vector2.y;
		result.PivotX = rect.pivot.x;
		result.PivotY = rect.pivot.y;
		return result;
	}

	private bool ApplyBounds(BoundsState boundsState)
	{
		float num = boundsState.Right - boundsState.Left;
		float num2 = boundsState.Top - boundsState.Bottom;
		Vector2 vector = new Vector2(boundsState.PivotX, boundsState.PivotY);
		float x = boundsState.Left + vector.x * num;
		float y = boundsState.Bottom + vector.y * num2;
		float z = rect.localPosition.z;
		if (!Approximately(rect.pivot, vector))
		{
			rect.pivot = vector;
		}
		bool flag = !Approximately(rect.rect.size, new Vector2(num, num2));
		if (flag)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
		}
		Vector3 vector2 = new Vector3(x, y, z);
		if (!Approximately(rect.localPosition, vector2))
		{
			rect.localPosition = vector2;
		}
		return flag;
	}

	private void ForceCanvasUpdateIfNeeded()
	{
		if (isForcingCanvasUpdate || !Application.isPlaying)
		{
			return;
		}
		isForcingCanvasUpdate = true;
		try
		{
			Canvas.ForceUpdateCanvases();
		}
		finally
		{
			isForcingCanvasUpdate = false;
		}
	}

	private void CacheAppliedState(BoundsState boundsState)
	{
		lastLeftTarget = leftTarget;
		lastRightTarget = rightTarget;
		lastBottomTarget = bottomTarget;
		lastTopTarget = topTarget;
		lastPivotTarget = pivotTarget;
		lastFollowPivotX = followPivotX;
		lastFollowPivotY = followPivotY;
		lastLeftOffset = leftOffset;
		lastRightOffset = rightOffset;
		lastBottomOffset = bottomOffset;
		lastTopOffset = topOffset;
		lastSizeDelta = rect.sizeDelta;
		lastLocalPosition = rect.localPosition;
		lastPivot = rect.pivot;
		lastBoundsState = boundsState;
	}

	private bool HasInspectorStateChanged()
	{
		if (lastLeftTarget != leftTarget || lastRightTarget != rightTarget || lastBottomTarget != bottomTarget || lastTopTarget != topTarget || lastPivotTarget != pivotTarget)
		{
			return true;
		}
		if (lastFollowPivotX != followPivotX || lastFollowPivotY != followPivotY)
		{
			return true;
		}
		if (lastLeftOffset != leftOffset || lastRightOffset != rightOffset || lastBottomOffset != bottomOffset || lastTopOffset != topOffset)
		{
			return true;
		}
		if (!Approximately(lastSizeDelta, rect.sizeDelta))
		{
			return true;
		}
		if (!Approximately(lastLocalPosition, rect.localPosition))
		{
			return true;
		}
		return !Approximately(lastPivot, rect.pivot);
	}

	private bool HasBoundsStateChanged(BoundsState current)
	{
		if (Approximately(lastBoundsState.Left, current.Left) && Approximately(lastBoundsState.Right, current.Right) && Approximately(lastBoundsState.Bottom, current.Bottom) && Approximately(lastBoundsState.Top, current.Top) && Approximately(lastBoundsState.PivotX, current.PivotX))
		{
			return !Approximately(lastBoundsState.PivotY, current.PivotY);
		}
		return true;
	}

	private static bool Approximately(float a, float b)
	{
		return Mathf.Abs(a - b) <= 0.001f;
	}

	private static bool Approximately(Vector2 a, Vector2 b)
	{
		return (a - b).sqrMagnitude <= 1E-06f;
	}

	private static bool Approximately(Vector3 a, Vector3 b)
	{
		return (a - b).sqrMagnitude <= 1E-06f;
	}

	private void OnDrawGizmos()
	{
		if (showGizmos)
		{
			CacheComponents();
			if (!(rect == null) && !(cachedParent == null))
			{
				BoundsState boundsState = CalculateBoundsState();
				DrawBoundsGizmo(boundsState);
				DrawPivotGizmo(boundsState);
			}
		}
	}

	private void DrawBoundsGizmo(BoundsState boundsState)
	{
		Vector3 vector = cachedParent.TransformPoint(new Vector3(boundsState.Left, boundsState.Bottom, 0f));
		Vector3 vector2 = cachedParent.TransformPoint(new Vector3(boundsState.Left, boundsState.Top, 0f));
		Vector3 vector3 = cachedParent.TransformPoint(new Vector3(boundsState.Right, boundsState.Top, 0f));
		Vector3 vector4 = cachedParent.TransformPoint(new Vector3(boundsState.Right, boundsState.Bottom, 0f));
		Gizmos.color = new Color(0.2f, 0.7f, 1f, 0.9f);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
	}

	private void DrawPivotGizmo(BoundsState boundsState)
	{
		float x = boundsState.Left + (boundsState.Right - boundsState.Left) * boundsState.PivotX;
		float y = boundsState.Bottom + (boundsState.Top - boundsState.Bottom) * boundsState.PivotY;
		Vector3 vector = cachedParent.TransformPoint(new Vector3(x, y, 0f));
		Gizmos.color = new Color(1f, 0.8f, 0.15f, 0.95f);
		Gizmos.DrawSphere(vector, 8f);
		if (!(pivotTarget == null))
		{
			Gizmos.color = new Color(1f, 0.45f, 0.15f, 0.9f);
			Gizmos.DrawLine(vector, pivotTarget.position);
			Gizmos.DrawWireSphere(pivotTarget.position, 10f);
		}
	}
}

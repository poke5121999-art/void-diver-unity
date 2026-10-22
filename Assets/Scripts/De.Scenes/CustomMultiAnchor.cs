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
	private string usageWarning;

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
	public bool showGizmos;

	private const float MinSize = 2f;

	private const float Epsilon = 0.001f;

	private RectTransform rect;

	private Transform cachedParent;

	private bool isDirty;

	private bool isApplying;

	private bool isForcingCanvasUpdate;

	private int lastAutoApplyFrame;

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

	private readonly Vector3[] worldCorners;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnValidate()
	{
	}

	private void Reset()
	{
	}

	private void OnTransformParentChanged()
	{
	}

	private void OnRectTransformDimensionsChange()
	{
	}

	private void HandleWillRenderCanvases()
	{
	}

	public void Refresh()
	{
	}

	private void CacheComponents()
	{
	}

	private void MarkDirty()
	{
	}

	private void RestoreUsageWarning()
	{
	}

	private void ApplyIfNeeded(bool force)
	{
	}

	private BoundsState CalculateBoundsState()
	{
		return default;
	}

	private BoundsState CalculateCurrentBounds()
	{
		return default;
	}

	private bool ApplyBounds(BoundsState boundsState)
	{
		return false;
	}

	private void ForceCanvasUpdateIfNeeded()
	{
	}

	private void CacheAppliedState(BoundsState boundsState)
	{
	}

	private bool HasInspectorStateChanged()
	{
		return false;
	}

	private bool HasBoundsStateChanged(BoundsState current)
	{
		return false;
	}

	private static bool Approximately(float a, float b)
	{
		return false;
	}

	private static bool Approximately(Vector2 a, Vector2 b)
	{
		return false;
	}

	private static bool Approximately(Vector3 a, Vector3 b)
	{
		return false;
	}

	private void OnDrawGizmos()
	{
	}

	private void DrawBoundsGizmo(BoundsState boundsState)
	{
	}

	private void DrawPivotGizmo(BoundsState boundsState)
	{
	}
}

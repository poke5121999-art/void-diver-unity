using UnityEngine;
using VContainer;

public class PadFocusCursor : MonoBase
{
	[SerializeField]
	private RectTransform _outlineRect;

	[SerializeField]
	private Vector2 _padding;

	[Inject]
	private GameInputManager _inputManager;

	private readonly Vector3[] _corners = new Vector3[4];

	private RectTransform _outlineParentRect;

	private Canvas _outlineCanvas;

	protected override void Awake()
	{
		base.Awake();
		_outlineParentRect = _outlineRect.parent as RectTransform;
		_outlineCanvas = _outlineRect.GetComponentInParent<Canvas>().rootCanvas;
	}

	private void Update()
	{
		if (!TryGetTargetRect(out var targetRect) || !TryGetRootCanvas(targetRect, out var rootCanvas))
		{
			_outlineRect.gameObject.SetActive(false);
			return;
		}
		Camera canvasCamera = GetCanvasCamera(rootCanvas);
		Camera canvasCamera2 = GetCanvasCamera(_outlineCanvas);
		targetRect.GetWorldCorners(_corners);
		Vector2 vector = WorldToLocalPoint(_corners[0], canvasCamera, canvasCamera2);
		Vector2 vector2 = WorldToLocalPoint(_corners[2], canvasCamera, canvasCamera2);
		_outlineRect.gameObject.SetActive(true);
		_outlineRect.localPosition = (vector2 + vector) * 0.5f;
		_outlineRect.sizeDelta = new Vector2(_padding.x + (vector2.x - vector.x), _padding.y + (vector2.y - vector.y));
	}

	private bool TryGetTargetRect(out RectTransform targetRect)
	{
		GameObject selectedObject = _inputManager.GetSelectedObject();
		if (selectedObject != null && selectedObject.TryGetComponent<RectTransform>(out targetRect))
		{
			return true;
		}
		targetRect = null;
		return false;
	}

	private static bool TryGetRootCanvas(RectTransform rect, out Canvas rootCanvas)
	{
		Canvas componentInParent = rect.GetComponentInParent<Canvas>();
		if (componentInParent != null)
		{
			rootCanvas = componentInParent.rootCanvas;
			return true;
		}
		rootCanvas = null;
		return false;
	}

	private static Camera GetCanvasCamera(Canvas canvas)
	{
		if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
		{
			return null;
		}
		return canvas.worldCamera;
	}

	private Vector2 WorldToLocalPoint(Vector3 worldPoint, Camera targetCamera, Camera outlineCamera)
	{
		Vector2 localPoint = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_outlineParentRect, RectTransformUtility.WorldToScreenPoint(targetCamera, worldPoint), outlineCamera, out localPoint);
		return localPoint;
	}
}

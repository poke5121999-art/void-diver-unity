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

	private readonly Vector3[] _corners;

	private RectTransform _outlineParentRect;

	private Canvas _outlineCanvas;

	protected override void Awake()
	{
	}

	private void Update()
	{
	}

	private bool TryGetTargetRect(out RectTransform targetRect)
	{
		targetRect = null;
		return false;
	}

	private static bool TryGetRootCanvas(RectTransform rect, out Canvas rootCanvas)
	{
		rootCanvas = null;
		return false;
	}

	private static Camera GetCanvasCamera(Canvas canvas)
	{
		return null;
	}

	private Vector2 WorldToLocalPoint(Vector3 worldPoint, Camera targetCamera, Camera outlineCamera)
	{
		return default;
	}
}

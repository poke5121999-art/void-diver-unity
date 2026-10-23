using UnityEngine;

public static class UiUtility
{
	public static Vector2 WorldToCanvasPosition(Vector3 worldPosition, Canvas canvas, Camera camera)
	{
		Vector2 localPoint = Vector2.zero;
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldPosition);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, (canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? camera : null, out localPoint);
		return localPoint;
	}

	public static Color HexToColor(string hex)
	{
		Color color = default;
		ColorUtility.TryParseHtmlString(hex, out color);
		return color;
	}
}

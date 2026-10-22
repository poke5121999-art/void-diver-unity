using UnityEngine;

public class PadCursor : CursorBase
{
	protected override Vector2 GetCursorPosition()
	{
		return default;
	}

	protected override bool IsCursorVisible()
	{
		return false;
	}
}

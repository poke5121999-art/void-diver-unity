using GameCommon.Enum;
using UnityEngine;

public class PadCursor : CursorBase
{
	protected override Vector2 GetCursorPosition()
	{
		return _inputController.GetAimScreenPoint();
	}

	protected override bool IsCursorVisible()
	{
		if (_inputController.IsAiming)
		{
			return _inputController.CompositeInputState == ECompositeInputState.None;
		}
		return false;
	}
}

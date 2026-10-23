using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class TeleportActionEvent : ActionEvent
	{
		public ETeleportDirectionType DirectionType;

		public float MinDistance;

		public float MaxDistance;

		public float TeleportPositionOffset;

		public float TeleportAngleOffset;

		public bool CanTeleportWithoutPath;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "mindistance")
			{
				result = MinDistance.ToFloatString();
			}
			else if (text == "maxdistance")
			{
				result = MaxDistance.ToFloatString();
			}
			else if (text == "teleportpositionoffset")
			{
				result = TeleportPositionOffset.ToFloatString();
			}
			else if (text == "teleportangleoffset")
			{
				result = TeleportAngleOffset.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[4] { "mindistance", "maxdistance", "teleportpositionoffset", "teleportangleoffset" }).ToArray();
		}
	}
}

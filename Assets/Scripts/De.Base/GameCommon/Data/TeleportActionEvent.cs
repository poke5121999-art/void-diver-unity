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
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}

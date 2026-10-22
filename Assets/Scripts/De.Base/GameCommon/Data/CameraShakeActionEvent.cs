using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CameraShakeActionEvent : ActionEvent
	{
		public bool IsLocalCharacterOnly;

		public float ShakeAmplitude;

		public float ShakeFrequency;

		public float ShakeDuration;

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

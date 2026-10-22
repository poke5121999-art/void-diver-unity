using GameCommon.Table;
using System.Text;
using System.Linq;
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
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			if (text == "shakeamplitude")
			{
				result = ShakeAmplitude.ToFloatString();
			}
			else if (text == "shakefrequency")
			{
				result = ShakeFrequency.ToFloatString();
			}
			else
			{
				result = (text == "shakeduration") ? ShakeDuration.ToFloatString() : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[3] { "shakeamplitude", "shakefrequency", "shakeduration" }).ToArray();
		}
	}
}

using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class IndicatorVfxEvent : ActionEvent
	{
		public ESpawnPositionType SpawnPositionType;

		[FormerlySerializedAs("MaxTargetPlayerDistance")]
		public float TargetPlayerMaxDistance = -1f;

		public IndicatorInfo IndicatorInfo = new IndicatorInfo();

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "targetplayermaxdistance")
			{
				result = TargetPlayerMaxDistance.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "targetplayermaxdistance" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = true;
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (!IndicatorInfo.Validate(out string infoMessage))
			{
				isValid = false;
				stringBuilder.AppendLine(infoMessage);
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public struct CcInfo
	{
		[Tooltip("CcType")]
		public ECrowdControlType ccType;

		[Tooltip("CcDuration")]
		public float ccDuration;

		[Tooltip("CcMoveDistance")]
		public float ccMoveDistance;

		[Tooltip("CcMoveDuration")]
		public float ccMoveDuration;

		[Tooltip("CcMoveOffset")]
		public float ccMoveOffset;

		[Tooltip("UseForcedMoveDirection")]
		public bool UseForcedMoveDirection;

		[Tooltip("ForcedMoveDirectionType")]
		public ECCforcedMoveDirectionType ForcedMoveDirectionType;

		[Tooltip("SuppressionValue")]
		public float SuppressionValue;

		[Tooltip("MonsterCondition")]
		public EMonsterType MonsterCondition;

		[Tooltip("FactionCondition")]
		public EMonsterCategory FactionCondition;

		public bool TryResolveExpression(string expression, out string result)
		{
			switch (expression)
			{
			case "CcType":
				result = ccType.ToExpressionText();
				break;
			case "CcDuration":
				result = ccDuration.ToFloatString();
				break;
			case "CcMoveDistance":
				result = ccMoveDistance.ToFloatString();
				break;
			case "CcMoveDuration":
				result = ccMoveDuration.ToFloatString();
				break;
			case "CcMoveOffset":
				result = ccMoveOffset.ToFloatString();
				break;
			case "SuppressionValue":
				result = SuppressionValue.ToFloatString();
				break;
			default:
				result = null;
				break;
			}
			return result != null;
		}

		public string[] ExtractExpressions()
		{
			return new string[6] { "CcType", "CcDuration", "CcMoveDistance", "CcMoveDuration", "CcMoveOffset", "SuppressionValue" };
		}
	}
}

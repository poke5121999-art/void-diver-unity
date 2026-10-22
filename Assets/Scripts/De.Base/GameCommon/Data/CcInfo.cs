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
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}
	}
}

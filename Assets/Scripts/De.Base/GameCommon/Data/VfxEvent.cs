using System;
using System.Linq;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class VfxEvent : ActionEvent
	{
		public string prefab;

		public float duration;

		public bool IsLoop;

		public float LoopDuration;

		public EUnitBoneType boneType;

		public Vector3 offset;

		public bool InheritAimDir;

		public bool InheritMoveDir;

		public bool UpdateByAimDir;

		public AnimationSpeed[] VfxSpeeds;

		public bool DestroyOnActionEnd;

		public bool DestroyOnSkillEnd;

		public bool DestroyOnUnitFollowerTargetDeath;

		public bool IsIndependent;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = ((expression.ToLower() == "starttime") ? startTime.ToFloatString() : null);
			if (result != null)
			{
				return true;
			}
			string text = expression.ToLower();
			result = ((text == "duration") ? duration.ToFloatString() : ((text == "loopduration") ? LoopDuration.ToFloatString() : null));
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "duration", "loopduration" }).ToArray();
		}
	}
}

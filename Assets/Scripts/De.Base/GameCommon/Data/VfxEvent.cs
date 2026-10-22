using System;
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
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}

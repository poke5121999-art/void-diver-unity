using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SpawnHitBoxOnEndEffect : BuffEffect
	{
		public int HitBoxId;

		public bool SpawnOnExpire;

		public bool SpawnOnDie;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

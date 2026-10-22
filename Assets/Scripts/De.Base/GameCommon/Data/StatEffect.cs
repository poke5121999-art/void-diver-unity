using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StatEffect : BuffEffect
	{
		public EStatType statType;

		public float statValue;

		public bool showEffect;

		public override EBuffEffectType EffectType => EBuffEffectType.Stat;

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

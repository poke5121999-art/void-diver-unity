using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class RecoveryActionEvent : ActionEvent
	{
		public EHitEffectType EffectType;

		public int Amount;

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

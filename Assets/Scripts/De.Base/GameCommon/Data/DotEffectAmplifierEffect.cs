using System.Linq;
using System;
using GameCommon.Enum;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class DotEffectAmplifierEffect : BuffEffect
	{
		[FormerlySerializedAs("TargetStatusEffectTag")]
		public EStatusEffectTag TargetDotType;

		public EBuffEffectType[] TargetEffectTypes;

		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.DotDamageAmplifier;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			result = ((expression == "Percent") ? Percent.ToFloatString() : null);
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "Percent" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			return ValidateBaseBuffEffect(out errorMessage);
		}
	}
}

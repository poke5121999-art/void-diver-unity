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

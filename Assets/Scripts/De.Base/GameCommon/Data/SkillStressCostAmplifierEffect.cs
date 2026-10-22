using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillStressCostAmplifierEffect : BuffEffect
	{
		public float Percent;

		public ESkillTagType[] SkillTags;

		public override EBuffEffectType EffectType => EBuffEffectType.SkillStressCostAmplifier;

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

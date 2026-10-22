using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HpDamageTakenAmplifierEffect : BuffEffect
	{
		public float Percent;

		public bool ShowEffect;

		public ESkillTagType[] SkillTags;

		public override EBuffEffectType EffectType => EBuffEffectType.HpDamageTakenAmplifier;

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

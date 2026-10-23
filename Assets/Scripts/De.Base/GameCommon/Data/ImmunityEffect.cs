using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ImmunityEffect : BuffEffect
	{
		public EStatusEffectTag ImmunityEffectTags;

		public bool DispelOnApply;

		public override EBuffEffectType EffectType => EBuffEffectType.Immunity;

		public bool IsFullImmunity => (int)ImmunityEffectTags == -1;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out string baseMessage))
			{
				result = false;
				stringBuilder.Append(baseMessage);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}

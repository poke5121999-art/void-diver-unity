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
			errorMessage = null;
			return false;
		}
	}
}

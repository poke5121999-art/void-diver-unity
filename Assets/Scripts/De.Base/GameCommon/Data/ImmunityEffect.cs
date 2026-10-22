using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ImmunityEffect : BuffEffect
	{
		public EStatusEffectTag ImmunityEffectTags;

		public bool DispelOnApply;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public bool IsFullImmunity => false;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

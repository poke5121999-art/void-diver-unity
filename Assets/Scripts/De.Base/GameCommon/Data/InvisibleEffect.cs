using GameCommon.Enum;

namespace GameCommon.Data
{
	public class InvisibleEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Invisible;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

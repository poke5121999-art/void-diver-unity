using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BlindEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Blind;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

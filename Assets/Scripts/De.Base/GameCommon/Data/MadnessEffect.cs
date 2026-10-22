using GameCommon.Enum;

namespace GameCommon.Data
{
	public class MadnessEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Madness;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

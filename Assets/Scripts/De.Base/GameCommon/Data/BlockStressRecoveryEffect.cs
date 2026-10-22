using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BlockStressRecoveryEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

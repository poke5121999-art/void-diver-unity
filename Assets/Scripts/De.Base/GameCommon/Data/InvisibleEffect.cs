using GameCommon.Enum;

namespace GameCommon.Data
{
	public class InvisibleEffect : BuffEffect
	{
		public override EBuffEffectType EffectType { get; }

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

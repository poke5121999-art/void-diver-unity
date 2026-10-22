using System.Text;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BlockStressRecoveryEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.BlockStressRecovery;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				flag = false;
				stringBuilder.Append(errorMessage2);
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}

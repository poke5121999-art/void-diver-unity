using System.Text;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BlindEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Blind;

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

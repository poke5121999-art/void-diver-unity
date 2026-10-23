using System.Text;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class MadnessEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Madness;

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

using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BlockSkillChargeEffect : BuffEffect
	{
		public int[] SkillIds;

		public override EBuffEffectType EffectType => EBuffEffectType.BlockSkillCharge;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				flag = false;
				stringBuilder.Append(errorMessage2);
			}
			if (SkillIds == null || SkillIds.Length == 0)
			{
				flag = false;
				stringBuilder.Append("SkillIds가 비어있습니다. 최소 1개 필요. ");
			}
			else if (SkillIds.Any((int id) => id < 1))
			{
				flag = false;
				stringBuilder.Append("SkillIds에 0 이하 값이 포함되어 있습니다. 모두 >0 필요. ");
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}

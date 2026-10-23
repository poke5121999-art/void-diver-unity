using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class EquipmentReviveEffect : BuffEffect
	{
		public int EquipmentId;

		public float HpPercent;

		public override EBuffEffectType EffectType => EBuffEffectType.EquipmentRevive;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "EquipmentId")
			{
				result = EquipmentId.ToString();
			}
			else if (expression == "HpPercent")
			{
				result = HpPercent.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "EquipmentId", "HpPercent" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
				stringBuilder.Append(errorMessage2);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}

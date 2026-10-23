using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SpawnHitBoxOnEndEffect : BuffEffect
	{
		public int HitBoxId;

		public bool SpawnOnExpire = true;

		public bool SpawnOnDie = true;

		public override EBuffEffectType EffectType => EBuffEffectType.SpawnHitBoxOnEnd;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "HitBoxId")
			{
				result = HitBoxId.ToString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "HitBoxId" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out string baseMessage))
			{
				result = false;
				stringBuilder.Append(baseMessage);
			}
			if (HitBoxId <= 0)
			{
				result = false;
				stringBuilder.Append("HitBoxId > 0 이어야 합니다");
			}
			if (!SpawnOnExpire && !SpawnOnDie)
			{
				result = false;
				stringBuilder.Append("SpawnOnExpire 또는 SpawnOnDie 중 하나는 true 여야 합니다");
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}

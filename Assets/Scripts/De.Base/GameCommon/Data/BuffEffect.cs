using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class BuffEffect
	{
		public bool UseStatValue;

		public EStatType StatType;

		public float StatPercent;

		public bool RealtimeStatValue;

		public abstract EBuffEffectType EffectType { get; }

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public virtual string[] ExtractExpressions()
		{
			return null;
		}

		public abstract bool Validate(out string errorMessage);

		public virtual bool ValidateBaseBuffEffect(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

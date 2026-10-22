using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class InvincibilityEffect : BuffEffect
	{
		public bool BlockDamage;

		public bool IgnoreBackAttack;

		public bool IgnoreCriticalHit;

		public bool BlockStressDamage;

		public bool BlockEnemyHit;

		public override EBuffEffectType EffectType => EBuffEffectType.Invincibility;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

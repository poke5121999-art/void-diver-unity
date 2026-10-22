using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class TrainingRoomEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.TrainingRoom;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

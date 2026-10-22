using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionForceMonsterSetTargetEvent : CollisionEvent
	{
		public EMonsterType MonsterCondition;
	}
}

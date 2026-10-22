using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionTryMonsterCuriousStateEvent : CollisionEvent
	{
		public EMonsterType MonsterCondition;
	}
}

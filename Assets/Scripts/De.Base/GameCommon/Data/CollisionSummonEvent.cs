using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionSummonEvent : CollisionEvent
	{
		public List<SummonMonsterInfo> MonsterInfos;

		public Vector3 SummonPosOffset;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

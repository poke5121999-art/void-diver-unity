using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class SummonActionEvent : ActionEvent
	{
		public List<SummonMonsterInfo> MonsterInfos;

		public Vector3 SpawnOffset;

		public bool IgnoreOffsetRotation;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

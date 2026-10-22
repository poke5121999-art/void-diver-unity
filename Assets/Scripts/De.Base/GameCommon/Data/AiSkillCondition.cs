using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class AiSkillCondition
	{
		public int Weight;

		[SerializeReference]
		public AiTargetCondition[] TargetConditions;

		[SerializeReference]
		public AiCasterCondition[] CasterConditions;
	}
}

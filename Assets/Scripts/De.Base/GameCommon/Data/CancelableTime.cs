using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class CancelableTime
	{
		[Range(0f, 100f)]
		public float startTime;

		[Range(0f, 100f)]
		public float endTime;

		public ESkillActionExecuteType ExecuteType;

		public float ExecuteTime;

		public bool IsCancelableByAnySkill;

		public bool IsCancelableByMove;

		public int[] cancelSkillIds;
	}
}

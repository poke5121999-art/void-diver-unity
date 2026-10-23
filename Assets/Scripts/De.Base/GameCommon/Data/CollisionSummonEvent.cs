using System.Text;
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
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				isValid = false;
			}
			if (MonsterInfos != null)
			{
				MonsterInfos.ForEach(delegate(SummonMonsterInfo _)
				{
					if (!_.Validate(out var errorMessage3))
					{
						isValid = false;
						stringBuilder.AppendLine(errorMessage3);
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

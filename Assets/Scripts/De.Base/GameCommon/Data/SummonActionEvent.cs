using System.Text;
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
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (MonsterInfos != null)
			{
				MonsterInfos.ForEach(delegate(SummonMonsterInfo info)
				{
					if (!info.Validate(out string infoMessage))
					{
						isValid = false;
						stringBuilder.AppendLine(infoMessage);
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

using System;
using System.Text;
using GameCommon.Table;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneRewardBoxSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward = Vector3.back;

		public int RewardBoxId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TRewardBox.Table.ContainsKey(RewardBoxId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TRewardBox", "Id", "RewardBoxId", RewardBoxId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

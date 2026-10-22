using System;
using System.Text;
using GameCommon.Table;
using UnityEngine;

[Serializable]
public class DropRewardSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public DropRewardSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		bool isValid = true;
		errorMessage = null;
		if (SpawnDatas == null || SpawnDatas.Length == 0)
		{
			errorMessage = "SpawnDatas cannot be null or empty.";
			return false;
		}
		StringBuilder sb = new StringBuilder();
		Array.ForEach(SpawnDatas, delegate(DropRewardSpawnData _)
		{
			if (!TDropReward.Table.ContainsKey(_.DropRewardGroupId))
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains ", _.SpawnPosition, "TDropReward", "Id") + string.Format("\"[{0}] {1}\"", "DropRewardGroupId", _.DropRewardGroupId));
			}
			if (!SpawnAll && (float)SpawnDatas.Length < SpawnCount)
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] SpawnCount cannot be greater than the number of SpawnDatas.", _.SpawnPosition));
			}
		});
		errorMessage = sb.ToString();
		return isValid;
	}
}

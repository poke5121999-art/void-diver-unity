using System;
using System.Text;
using GameCommon.Table;
using UnityEngine;

[Serializable]
public class ExitSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public ExitSpawnData[] SpawnDatas;

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
		Array.ForEach(SpawnDatas, delegate(ExitSpawnData _)
		{
			if (!TExit.Table.ContainsKey(_.ExitId))
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains ", _.SpawnPosition, "TExit", "Id") + string.Format("\"[{0}] {1}\"", "ExitId", _.ExitId));
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

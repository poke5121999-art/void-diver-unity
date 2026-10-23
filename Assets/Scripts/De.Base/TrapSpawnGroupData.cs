using GameCommon.Table;
using System.Text;
using System;

[Serializable]
public class TrapSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public TrapSpawnData[] SpawnDatas;

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
		Array.ForEach(SpawnDatas, delegate(TrapSpawnData _)
		{
			if (!TTrap.Table.ContainsKey(_.TrapId))
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains ", _.SpawnPosition, "TTrap", "Id") + string.Format("\"[{0}] {1}\"", "TrapId", _.TrapId));
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

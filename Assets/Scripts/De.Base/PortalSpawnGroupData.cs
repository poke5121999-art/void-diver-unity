using GameCommon.Table;
using System.Text;
using System;

[Serializable]
public class PortalSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public PortalSpawnData[] SpawnDatas;

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
		Array.ForEach(SpawnDatas, delegate(PortalSpawnData _)
		{
			if (string.IsNullOrEmpty(_.PrefabName))
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] {1} cannot be null or empty.", _.SpawnPosition, "PrefabName"));
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

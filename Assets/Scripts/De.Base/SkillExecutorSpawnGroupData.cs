using GameCommon.Table;
using System.Text;
using System;

[Serializable]
public class SkillExecutorSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public SkillExecutorSpawnData[] SpawnDatas;

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
		Array.ForEach(SpawnDatas, delegate(SkillExecutorSpawnData _)
		{
			if (!TSkillExecutor.Table.ContainsKey(_.SkillExecutorId))
			{
				isValid = false;
				sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains ", _.SpawnPosition, "TSkillExecutor", "Id") + string.Format("\"[{0}] {1}\"", "SkillExecutorId", _.SkillExecutorId));
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

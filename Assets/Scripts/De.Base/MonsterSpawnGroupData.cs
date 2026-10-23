using GameCommon.Table;
using System.Text;
using System;

[Serializable]
public class MonsterSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public MonsterSpawnData[] SpawnDatas;

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
		Array.ForEach(SpawnDatas, delegate(MonsterSpawnData _)
		{
			if (!_.UseMonsterSpawnTable)
			{
				if (_.MonsterSpawnGroupId != 0)
				{
					isValid = false;
					sb.AppendLine(string.Format("{0}] UseMonsterSpawnTable=false but [{1}]={2} is set. Toggle/Field \ubaa8\uc21c.", _.SpawnPosition, "MonsterSpawnGroupId", _.MonsterSpawnGroupId));
				}
				if (!TMonster.Table.TryGetValue(_.MonsterId, out var value))
				{
					isValid = false;
					sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains \"[{3}] {4}\"", _.SpawnPosition, "TMonster", "Id", "MonsterId", _.MonsterId));
				}
				else if (!value.IsBasePhaseMonster)
				{
					isValid = false;
					sb.AppendLine(string.Format("{0}] [{1}]={2} is a non-base phase variant (BasePhaseMonsterId={3}). Only base-phase monsters can be spawned.", _.SpawnPosition, "MonsterId", _.MonsterId, value.BasePhaseMonsterId));
				}
			}
			else
			{
				if (_.MonsterId != 0)
				{
					isValid = false;
					sb.AppendLine(string.Format("{0}] UseMonsterSpawnTable=true but [{1}]={2} is set. Toggle/Field \ubaa8\uc21c.", _.SpawnPosition, "MonsterId", _.MonsterId));
				}
				if (!TMonsterSpawn.GroupTable.ContainsKey(_.MonsterSpawnGroupId))
				{
					isValid = false;
					sb.AppendLine(string.Format("{0}] [{1}.{2}] not contains \"[{3}] {4}\"", _.SpawnPosition, "TMonsterSpawn", "GroupId", "MonsterSpawnGroupId", _.MonsterSpawnGroupId));
				}
			}
			if (!SpawnAll && (float)SpawnDatas.Length < SpawnCount)
			{
				isValid = false;
				sb.AppendLine("SpawnCount cannot be greater than the number of SpawnDatas.");
			}
		});
		errorMessage = sb.ToString();
		return isValid;
	}
}

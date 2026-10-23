using System;
using System.Text;
using GameCommon.Table;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneMonsterSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward = Vector3.back;

		public bool UseSpawnGroup;

		public int MonsterSpawnGroupId;

		public int MonsterId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (!UseSpawnGroup)
			{
				if (MonsterSpawnGroupId != 0)
				{
					isValid = false;
					stringBuilder.AppendLine(string.Format("UseSpawnGroup=false but [{0}]={1} is set. Toggle/Field 모순.", "MonsterSpawnGroupId", MonsterSpawnGroupId));
				}
				if (TMonster.Table.TryGetValue(MonsterId, out var monster))
				{
					if (!monster.IsBasePhaseMonster)
					{
						stringBuilder.AppendLine(string.Format("[{0}]={1} is a non-base phase variant (BasePhaseMonsterId={2}). Only base-phase monsters can be spawned.", "MonsterId", MonsterId, monster.BasePhaseMonsterId));
						errorMessage = stringBuilder.ToString();
						return false;
					}
				}
				else
				{
					stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TMonster", "Id", "MonsterId", MonsterId));
					errorMessage = stringBuilder.ToString();
					return false;
				}
			}
			else
			{
				if (MonsterId != 0)
				{
					isValid = false;
					stringBuilder.AppendLine(string.Format("UseSpawnGroup=true but [{0}]={1} is set. Toggle/Field 모순.", "MonsterId", MonsterId));
				}
				if (!TMonsterSpawn.GroupTable.ContainsKey(MonsterSpawnGroupId))
				{
					stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TMonsterSpawn", "GroupId", "MonsterSpawnGroupId", MonsterSpawnGroupId));
					errorMessage = stringBuilder.ToString();
					return false;
				}
			}
			if (isValid)
			{
				return true;
			}
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

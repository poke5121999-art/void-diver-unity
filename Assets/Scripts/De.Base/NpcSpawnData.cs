using GameCommon.Table;
using System.Text;
using System;
using UnityEngine;

[Serializable]
public class NpcSpawnData
{
	public int NpcId;

	public Vector3 SpawnPosition;

	public NpcSpawnData Clone()
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		if (TNpc.Table.ContainsKey(NpcId))
		{
			return true;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TNpc", "Id", "NpcId", NpcId));
		errorMessage = stringBuilder.ToString();
		return false;
	}
}

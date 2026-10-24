using GameCommon.Table;
using System.Text;
using System;
using UnityEngine;

[Serializable]
public class NpcSpawnData
{
	public int NpcId;

	public Vector3 SpawnPosition;

	// @0x1801FA7D0: new NpcSpawnData, copies NpcId and SpawnPosition.
	public NpcSpawnData Clone()
	{
		return new NpcSpawnData
		{
			NpcId = NpcId,
			SpawnPosition = SpawnPosition
		};
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

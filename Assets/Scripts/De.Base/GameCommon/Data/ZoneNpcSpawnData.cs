using System;
using System.Text;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneNpcSpawnData : ZoneObjectSpawnData
	{
		public int NpcId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TNpc.Table.ContainsKey(NpcId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TNpc", "Id", "NpcId", NpcId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

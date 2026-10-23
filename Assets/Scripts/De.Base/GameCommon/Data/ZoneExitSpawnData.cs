using System;
using System.Text;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneExitSpawnData : ZoneObjectSpawnData
	{
		public int ExitId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TExit.Table.ContainsKey(ExitId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TExit", "Id", "ExitId", ExitId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

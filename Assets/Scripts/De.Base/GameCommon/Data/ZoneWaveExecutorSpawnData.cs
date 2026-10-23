using System;
using System.Text;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneWaveExecutorSpawnData : ZoneObjectSpawnData
	{
		public int WaveExecutorId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TWaveExecutor.Table.ContainsKey(WaveExecutorId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TWaveExecutor", "Id", "WaveExecutorId", WaveExecutorId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

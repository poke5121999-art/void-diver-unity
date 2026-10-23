using System;
using System.Text;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneSkillExecutorSpawnData : ZoneObjectSpawnData
	{
		public int SkillExecutorId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TSkillExecutor.Table.ContainsKey(SkillExecutorId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TSkillExecutor", "Id", "TSkillExecutor", SkillExecutorId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

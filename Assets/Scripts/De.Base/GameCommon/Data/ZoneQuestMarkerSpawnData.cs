using System;
using System.Text;
using GameCommon.Table;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneQuestMarkerSpawnData : ZoneObjectSpawnData
	{
		[FormerlySerializedAs("QuestTaskId")]
		[FormerlySerializedAs("CampaignQuestTaskId")]
		public int CampaignTaskId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (TCampaignTask.Table.ContainsKey(CampaignTaskId))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Format("[{0}.{1}] not contains \"[{2}] {3}\"", "TCampaignTask", "Id", "CampaignTaskId", CampaignTaskId));
			errorMessage = stringBuilder.ToString();
			return false;
		}
	}
}

using System;
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
			return false;
		}
	}
}

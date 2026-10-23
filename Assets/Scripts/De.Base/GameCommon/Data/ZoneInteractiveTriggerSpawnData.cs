using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneInteractiveTriggerSpawnData : ZoneObjectSpawnData
	{
		public string PrefabName;

		public int TriggerId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return true;
		}
	}
}

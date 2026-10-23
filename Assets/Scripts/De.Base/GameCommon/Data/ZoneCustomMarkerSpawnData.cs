using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneCustomMarkerSpawnData : ZoneObjectSpawnData
	{
		public int CustomId;

		public string MarkerName;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return true;
		}
	}
}

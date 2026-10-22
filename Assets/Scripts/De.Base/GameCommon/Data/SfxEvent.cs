using System;

namespace GameCommon.Data
{
	[Serializable]
	public class SfxEvent : ActionEvent
	{
		public string Sfx;

		public bool IsIndependent;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

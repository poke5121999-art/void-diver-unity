using GameCommon.Table;
using System.Text;
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
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = true;
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (!ValidationExtensions.ContainsSfx(Sfx))
			{
				isValid = false;
				stringBuilder.AppendLine("MasterAudio not contains \"[SfxEvent.Sfx] " + Sfx + "\"");
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

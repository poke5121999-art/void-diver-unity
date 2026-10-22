using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class RemoveStatusEffectActionEvent : ActionEvent
	{
		public EStatusEffectTag EffectTags;
	}
}

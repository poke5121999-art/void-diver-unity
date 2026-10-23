using GameCommon.Table;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ConsumeItemSkillTrigger : SkillTrigger
	{
		public int ItemId;

		public ConsumeItemSkillTrigger()
		{
			Text = "설명 없음";
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = string.Empty;
			if (ItemId > 0 && !TItem.Table.ContainsKey(ItemId))
			{
				errorMessage = string.Format("{0} [{1}] is not exist in {2}", "ItemId", ItemId, "TItem");
				return false;
			}
			return true;
		}
	}
}

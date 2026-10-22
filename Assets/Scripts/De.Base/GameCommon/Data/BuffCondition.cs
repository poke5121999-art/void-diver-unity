using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffCondition
	{
		public EConditionTargetType ConditionOwnerType;

		public EBuffConditionType BuffConditionType;

		public EBuffConditionCasterType BuffConditionCasterType;

		public bool CheckNotContainsCasterType;

		public int BuffId;

		public int BuffStack;
	}
}

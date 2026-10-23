using GameCommon.Table;
using System.Text;
using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxIterator
	{
		public EHitBoxIteratorType IteratorType;

		public int TotalAngle;

		public int TotalCount;

		public float HitBoxDelay;

		public bool UpdateBasePositionEveryFrame;

		public bool UpdateAimEveryFrame;

		public HitBoxIteratorCustomData[] CustomData = Array.Empty<HitBoxIteratorCustomData>();

		public float StartAngleOffsetMin;

		public float StartAngleOffsetMax;

		public float StartPositionOffsetMin;

		public float StartPositionOffsetMax;

		public IndicatorInfo IndicatorInfo;

		public int HitBoxId = -1;

		public List<SummonMonsterInfo> MonsterInfos = new List<SummonMonsterInfo>();

		public bool UseCustomOrder;

		public List<int> CustomOrderIdx = new List<int>();

		public bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (HitBoxId > 0 && !THitBox.Table.ContainsKey(HitBoxId))
			{
				isValid = false;
				stringBuilder.AppendLine(string.Format("{0} HitBoxId [{1}] is not exist in  {2}", "IndicatorInfo", HitBoxId, "THitBox"));
			}
			if (IndicatorInfo != null && !IndicatorInfo.Validate(out var errorMessage2))
			{
				isValid = false;
				stringBuilder.AppendLine(errorMessage2);
			}
			if (MonsterInfos != null)
			{
				MonsterInfos.ForEach(delegate(SummonMonsterInfo _)
				{
					if (!_.Validate(out var errorMessage3))
					{
						isValid = false;
						stringBuilder.AppendLine(errorMessage3);
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

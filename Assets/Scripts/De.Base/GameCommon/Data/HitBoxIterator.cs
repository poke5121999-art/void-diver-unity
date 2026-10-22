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

		public HitBoxIteratorCustomData[] CustomData;

		public float StartAngleOffsetMin;

		public float StartAngleOffsetMax;

		public float StartPositionOffsetMin;

		public float StartPositionOffsetMax;

		public IndicatorInfo IndicatorInfo;

		public int HitBoxId;

		public List<SummonMonsterInfo> MonsterInfos;

		public bool UseCustomOrder;

		public List<int> CustomOrderIdx;

		public bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

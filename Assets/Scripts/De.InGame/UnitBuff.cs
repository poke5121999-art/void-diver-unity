using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitBuff : ITrackableDictionaryData<int, UnitBuff>
{
	public int Key => Id;

	public int StackCount => StackEntries?.Length ?? SyncStackCount;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public float EndTime { get; set; }

	[ProtoMember(4)]
	public bool IsActive { get; set; }

	[ProtoMember(5)]
	public ProtoVector3 Point { get; set; }

	[ProtoMember(6)]
	public ulong[] CasterIds { get; set; }

	[ProtoMember(7)]
	public int SyncStackCount { get; set; }

	[ProtoMember(8)]
	public bool IsSuppressionBuff { get; set; }

	public bool IsEffectActive => IsActive && !IsSuppressionBuff;

	public BuffStackEntry[] StackEntries { get; set; }

	public Dictionary<(ulong casterId, EStatType statType), float> RealtimeCasterStats { get; set; }

	public float GetRealtimeCasterStatSum(EStatType statType)
	{
		float num = 0f;
		if (RealtimeCasterStats != null)
		{
			foreach (KeyValuePair<(ulong casterId, EStatType statType), float> realtimeCasterStat in RealtimeCasterStats)
			{
				if (realtimeCasterStat.Key.statType == statType)
				{
					num += realtimeCasterStat.Value;
				}
			}
		}
		return num;
	}

	public UnitBuff Clone()
	{
		return new UnitBuff
		{
			Operation = Operation,
			Id = Id,
			EndTime = EndTime,
			IsActive = IsActive,
			IsSuppressionBuff = IsSuppressionBuff,
			Point = Point,
			StackEntries = StackEntries?.Select((BuffStackEntry e) => e.Clone()).ToArray(),
			CasterIds = CasterIds.ToArray(),
			SyncStackCount = SyncStackCount,
			RealtimeCasterStats = ((RealtimeCasterStats != null) ? new Dictionary<(ulong casterId, EStatType statType), float>(RealtimeCasterStats) : null)
		};
	}
}

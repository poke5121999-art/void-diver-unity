using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitBuff : ITrackableDictionaryData<int, UnitBuff>
{
	public int Key => 0;

	public int StackCount => 0;

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

	public bool IsEffectActive => false;

	public BuffStackEntry[] StackEntries { get; set; }

	public Dictionary<(ulong casterId, EStatType statType), float> RealtimeCasterStats { get; set; }

	public float GetRealtimeCasterStatSum(EStatType statType)
	{
		return 0f;
	}

	public UnitBuff Clone()
	{
		return null;
	}
}

using System.Linq;
using ProtoBuf;

[ProtoContract]
public class BuffStackEntry
{
	[ProtoMember(1)]
	public UnitStat[] CasterStats { get; set; }

	public BuffStackEntry Clone()
	{
		return new BuffStackEntry
		{
			CasterStats = CasterStats?.Select(s => s == null ? null : new UnitStat
			{
				Operation = s.Operation,
				StatType = s.StatType,
				Value = s.Value
			}).ToArray()
		};
	}
}

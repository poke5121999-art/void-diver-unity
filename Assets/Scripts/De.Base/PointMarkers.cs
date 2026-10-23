using GameCommon.Data;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class PointMarkers : ITrackableData<PointMarkers>
{
	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public ProtoVector3[] Positions { get; set; }

	public PointMarkers Clone()
	{
		return new PointMarkers
		{
			Positions = (ProtoVector3[])Positions?.Clone()
		};
	}
}

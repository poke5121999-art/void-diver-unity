using ProtoBuf;
using Tracker;

[ProtoContract]
public class Talent : ITrackableData<Talent>
{
	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public BitFlags BitFlags { get; set; }

	public Talent Clone()
	{
		return new Talent
		{
			BitFlags = BitFlags?.Clone()
		};
	}
}

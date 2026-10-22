using ProtoBuf;

[ProtoContract]
public class LostItemSelectInfo
{
	[ProtoMember(1)]
	public int SlotIndex { get; set; }

	[ProtoMember(2)]
	public float PriceMultiplier { get; set; }

	public LostItemSelectInfo Clone()
	{
		return null;
	}
}

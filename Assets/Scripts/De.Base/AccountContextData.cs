using ProtoBuf;

[ProtoContract]
public class AccountContextData : ISaveData
{
	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public long SaveTimeTicks { get; set; }

	[ProtoMember(3)]
	public long CreateTimeTicks { get; set; }

	[ProtoMember(4)]
	public long PlayTimeTicks { get; set; }

	[ProtoMember(5)]
	public int LastPlayerSlotIndex { get; set; }

	[ProtoMember(6)]
	public PlayerSlot[] PlayerSlots { get; set; }

	public ISaveData Clone()
	{
		return null;
	}

	public void Normalize()
	{
	}
}

using System.Linq;
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
		return new AccountContextData
		{
			AccountId = AccountId,
			SaveTimeTicks = SaveTimeTicks,
			PlayTimeTicks = PlayTimeTicks,
			LastPlayerSlotIndex = LastPlayerSlotIndex,
			PlayerSlots = ((PlayerSlots == null) ? null : PlayerSlots.Select((PlayerSlot s) => s.Clone()).ToArray()),
			CreateTimeTicks = CreateTimeTicks
		};
	}

	public void Normalize()
	{
	}
}

using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
public class PlayerSlot
{
	[ProtoMember(1)]
	public AppVersion Version { get; set; }

	[ProtoMember(2)]
	public int SlotIndex { get; set; }

	[ProtoMember(3)]
	public string PlayerName { get; set; }

	[ProtoMember(4)]
	public int Level { get; set; }

	[ProtoMember(5)]
	public long SaveTimeTicks { get; set; }

	[ProtoMember(6)]
	public long PlayTimeTicks { get; set; }

	public bool IsEmpty => string.IsNullOrEmpty(PlayerName);

	public PlayerSlot Clone()
	{
		return new PlayerSlot
		{
			Version = Version,
			SlotIndex = SlotIndex,
			PlayerName = PlayerName,
			Level = Level,
			SaveTimeTicks = SaveTimeTicks,
			PlayTimeTicks = PlayTimeTicks
		};
	}
}

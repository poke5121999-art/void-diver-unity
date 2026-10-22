using ProtoBuf;

[ProtoContract]
public class GamePlayerData
{
	[ProtoMember(1)]
	public ulong SteamId { get; set; }

	[ProtoMember(2)]
	public ulong ClientId { get; set; }

	[ProtoMember(3)]
	public string Name { get; set; }

	[ProtoMember(4)]
	public bool IsHostPlayer { get; set; }

	[ProtoMember(5)]
	public Profile Profile { get; set; }

	[ProtoMember(6)]
	public Character Character { get; set; }

	[ProtoMember(7)]
	public GoodsSlot[] InventorySlots { get; set; }

	[ProtoMember(8)]
	public GoodsSlot[] SafeInventorySlots { get; set; }

	[ProtoMember(9)]
	public Talent Talent { get; set; }

	[ProtoMember(10)]
	public int PlayerIndex { get; set; }

	[ProtoMember(11)]
	public PointMarkers PointMarkers { get; set; }
}

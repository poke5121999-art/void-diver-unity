using ProtoBuf;

[ProtoContract]
public class PlayerStoreEntry
{
	[ProtoMember(1)]
	public string Key { get; set; }

	[ProtoMember(2)]
	public string Value { get; set; }
}

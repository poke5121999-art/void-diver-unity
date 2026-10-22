using ProtoBuf;

[ProtoContract]
public class AccountRecordEntry
{
	[ProtoMember(1)]
	public int RecordId { get; set; }

	[ProtoMember(2)]
	public long Count { get; set; }
}

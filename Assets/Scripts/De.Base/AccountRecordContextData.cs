using ProtoBuf;

[ProtoContract]
public class AccountRecordContextData : ISaveData
{
	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public AccountRecordEntry[] Entries { get; set; }

	public ISaveData Clone()
	{
		return null;
	}

	public void Normalize()
	{
	}
}

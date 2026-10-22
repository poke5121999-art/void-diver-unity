using System.Linq;
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
		return new AccountRecordContextData
		{
			AccountId = AccountId,
			Entries = ((Entries == null) ? null : Entries.Select((AccountRecordEntry e) => new AccountRecordEntry
			{
				RecordId = e.RecordId,
				Count = e.Count
			}).ToArray())
		};
	}

	public void Normalize()
	{
	}
}

using System.Collections.Generic;
using System.Linq;

public class AccountRecordContext
{
	private readonly Dictionary<int, long> _counts;

	public AccountRecordContext(AccountRecordContextData data)
	{
		_counts = new Dictionary<int, long>();
		if (data.Entries != null)
		{
			foreach (AccountRecordEntry e in data.Entries)
			{
				_counts[e.RecordId] = e.Count;
			}
		}
	}

	public long GetCount(int recordId)
	{
		_counts.TryGetValue(recordId, out var value);
		return value;
	}

	public long Add(int recordId, long delta)
	{
		_counts.TryGetValue(recordId, out var value);
		long num = delta + value;
		_counts[recordId] = num;
		return num;
	}

	public AccountRecordContextData ToData(string accountId)
	{
		return new AccountRecordContextData
		{
			AccountId = accountId,
			Entries = ((_counts.Count > 0) ? _counts.Select((KeyValuePair<int, long> kv) => new AccountRecordEntry
			{
				RecordId = kv.Key,
				Count = kv.Value
			}).ToArray() : null)
		};
	}
}

using System.Collections.Generic;

public class AccountRecordContext
{
	private readonly Dictionary<int, long> _counts;

	public AccountRecordContext(AccountRecordContextData data)
	{
	}

	public long GetCount(int recordId)
	{
		return 0L;
	}

	public long Add(int recordId, long delta)
	{
		return 0L;
	}

	public AccountRecordContextData ToData(string accountId)
	{
		return null;
	}
}

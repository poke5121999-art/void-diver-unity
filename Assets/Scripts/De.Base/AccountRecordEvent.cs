using GameCommon.Data;

public class AccountRecordEvent : IBaseEvent
{
	public int RecordId { get; }

	public long NewCount { get; }

	public AccountRecordEvent(int recordId, long newCount)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}

using System;
using GameCommon.Data;

public class AccountRecordEvent : IBaseEvent
{
	public int RecordId { get; }

	public long NewCount { get; }

	public AccountRecordEvent(int recordId, long newCount)
	{
		RecordId = recordId;
		NewCount = newCount;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.Type == GameCommon.Enum.EEventConditionType.AccountRecord && eventCondition.Value1 == RecordId)
		{
			currentProgress = Math.Min(NewCount, goal);
		}
		return currentProgress;
	}
}

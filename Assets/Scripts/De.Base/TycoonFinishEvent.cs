using GameCommon.Data;

public class TycoonFinishEvent : IBaseEvent
{
	public bool IsPanic { get; }

	public int EmployeeId { get; }

	public Goods Artifact { get; }

	public long ResultCoin { get; }

	public TycoonFinishEvent(bool isPanic, int employeeId, Goods artifact, long resultCoin)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}

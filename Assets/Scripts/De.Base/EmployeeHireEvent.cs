using GameCommon.Data;

public class EmployeeHireEvent : IBaseEvent
{
	public int EmployeeId { get; private set; }

	public EmployeeHireEvent(int employeeId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}

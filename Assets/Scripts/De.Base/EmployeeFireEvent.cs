using GameCommon.Enum;
using System;
using GameCommon.Data;

public class EmployeeFireEvent : IBaseEvent
{
	public int EmployeeId { get; private set; }

	public EmployeeFireEvent(int employeeId)
	{
		EmployeeId = employeeId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.EmployeeFire && (eventCondition.Value1 < 1 || EmployeeId == eventCondition.Value1))
		{
			currentProgress = Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}

using System;
using GameCommon.Enum;
using GameCommon.Data;

public class EmployeeHireEvent : IBaseEvent
{
	public int EmployeeId { get; private set; }

	public EmployeeHireEvent(int employeeId)
	{
		EmployeeId = employeeId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.EmployeeHire && (eventCondition.Value1 < 1 || EmployeeId == eventCondition.Value1))
		{
			currentProgress = Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}

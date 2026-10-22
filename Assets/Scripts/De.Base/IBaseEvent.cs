using GameCommon.Data;

public interface IBaseEvent
{
	long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal);
}

using GameCommon.Data;

public class TutorialEvent : IBaseEvent
{
	public string EventName { get; private set; }

	public TutorialEvent(string eventName)
	{
		EventName = eventName;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}

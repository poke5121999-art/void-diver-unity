using GameCommon.Data;
using GameCommon.Enum;

public class FeatureUnlockEvent : IBaseEvent
{
	public EFeatureUnlock[] Features { get; private set; }

	public FeatureUnlockEvent(EFeatureUnlock[] features)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}

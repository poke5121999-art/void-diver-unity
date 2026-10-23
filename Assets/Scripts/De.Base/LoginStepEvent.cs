using GameCommon.Data;

public class LoginStepEvent : IBaseEvent
{
	public ELoginType LoginType { get; }

	public ELoginStep LoginStep { get; }

	public ELoginDtl Dtl { get; }

	public ELoginRsn Rsn { get; }

	public long EngagementTimeMsec { get; }

	public LoginStepEvent(ELoginStep step, ELoginDtl dtl, ELoginRsn rsn, ELoginType type = ELoginType.LC, long engagementTimeMsec = 0L)
	{
		LoginStep = step;
		Dtl = dtl;
		Rsn = rsn;
		LoginType = type;
		EngagementTimeMsec = engagementTimeMsec;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}

using GameCommon.Data;

public class UserinflowEvent : IBaseEvent
{
	public string InflowIndex { get; }

	public EInflowCat InflowCat { get; }

	public EInflowValue InflowValue { get; }

	public string InflowDtl { get; }

	public UserinflowEvent(EInflowCat cat, EInflowValue value, string dtl, string index = null)
	{
		InflowCat = cat;
		InflowValue = value;
		InflowDtl = dtl;
		InflowIndex = index;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}

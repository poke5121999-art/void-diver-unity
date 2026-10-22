public class MultiAnalyticsEvent : GameAnalyticsEvent
{
	public MultiAnalyticsEvent()
	{
		base.Properties = new MultiProperties();
	}

	public override string Event => "multi";

	public new MultiProperties Properties => (MultiProperties)base.Properties;
}

public class AppCheckAnalyticsEvent : GameAnalyticsEvent
{
	public AppCheckAnalyticsEvent()
	{
		base.Properties = new AppCheckProperties();
	}

	public override string Event => "app_check";

	public new AppCheckProperties Properties => (AppCheckProperties)base.Properties;
}

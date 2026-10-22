public class UserinflowAnalyticsEvent : GameAnalyticsEvent
{
	public UserinflowAnalyticsEvent()
	{
		base.Properties = new UserinflowProperties();
	}

	public override string Event => "userinflow";

	public new UserinflowProperties Properties => (UserinflowProperties)base.Properties;
}

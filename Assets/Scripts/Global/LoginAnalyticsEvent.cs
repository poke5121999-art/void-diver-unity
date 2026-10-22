public class LoginAnalyticsEvent : GameAnalyticsEvent
{
	public LoginAnalyticsEvent()
	{
		base.Properties = new LoginProperties();
	}

	public override string Event => "login";

	public new LoginProperties Properties => (LoginProperties)base.Properties;
}

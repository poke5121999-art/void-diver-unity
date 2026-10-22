public class PlayendAnalyticsEvent : GameAnalyticsEvent
{
	public PlayendAnalyticsEvent()
	{
		base.Properties = new PlayendProperties();
	}

	public override string Event => "playend";

	public new PlayendProperties Properties => (PlayendProperties)base.Properties;
}

public class LevelAnalyticsEvent : GameAnalyticsEvent
{
	public LevelAnalyticsEvent()
	{
		base.Properties = new LevelEventProperties();
	}

	public override string Event => "LevelUp";

	public new LevelEventProperties Properties => (LevelEventProperties)base.Properties;
}

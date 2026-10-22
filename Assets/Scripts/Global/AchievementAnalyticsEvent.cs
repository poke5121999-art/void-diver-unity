public class AchievementAnalyticsEvent : GameAnalyticsEvent
{
	public AchievementAnalyticsEvent()
	{
		base.Properties = new AchievementProperties();
	}

	public override string Event => "achievement";

	public new AchievementProperties Properties => (AchievementProperties)base.Properties;
}

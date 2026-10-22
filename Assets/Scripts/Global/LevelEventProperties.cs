using Newtonsoft.Json;

public class LevelEventProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("level")]
	public int Level;
}

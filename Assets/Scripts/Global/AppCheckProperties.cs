using Newtonsoft.Json;

public class AppCheckProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("fps")]
	public int Fps;

	[JsonProperty("memory_usage_mb")]
	public int MemoryUsageMb;
}

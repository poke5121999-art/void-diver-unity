using Newtonsoft.Json;

public class MultiProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("tag")]
	public string Tag;

	[JsonProperty("u_lounge_id")]
	public string ULoungeId;

	[JsonProperty("multi_mod")]
	public string MultiMod;

	[JsonProperty("multi_open")]
	public string MultiOpen;

	[JsonProperty("multi_name")]
	public string MultiName;

	[JsonProperty("multi_rslt")]
	public int MultiRslt;
}

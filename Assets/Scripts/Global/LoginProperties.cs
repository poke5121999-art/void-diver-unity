using Newtonsoft.Json;

public class LoginProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("login_cat")]
	public string LoginCat;

	[JsonProperty("login_type")]
	public string LoginType;

	[JsonProperty("login_step")]
	public string LoginStep;

	[JsonProperty("dtl")]
	public string Dtl;

	[JsonProperty("rsn")]
	public string Rsn;

	[JsonProperty("engagement_time_msec")]
	public long EngagementTimeMsec;
}

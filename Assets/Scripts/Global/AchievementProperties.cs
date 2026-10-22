using Newtonsoft.Json;

public class AchievementProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("tag")]
	public string Tag;

	[JsonProperty("achievement_cat")]
	public string AchievementCat;

	[JsonProperty("u_achv_id")]
	public string UAchvId;

	[JsonProperty("achievement_name")]
	public string AchievementName;

	[JsonProperty("achievement_id")]
	public string AchievementId;

	[JsonProperty("achievement_lv")]
	public int? AchievementLv;

	[JsonProperty("achievement_rslt")]
	public string AchievementRslt;

	[JsonProperty("achievement_rslt_dtl")]
	public string AchievementRsltDtl;
}

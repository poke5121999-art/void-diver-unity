using Newtonsoft.Json;

public class PlayendProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("tag")]
	public string Tag;

	[JsonProperty("play_cat")]
	public string PlayCat;

	[JsonProperty("u_play_id")]
	public string UPlayId;

	[JsonProperty("stg")]
	public string Stg;

	[JsonProperty("round")]
	public int Round;

	[JsonProperty("rep")]
	public string Rep;

	[JsonProperty("pwin")]
	public string Pwin;

	[JsonProperty("pwin_dtl")]
	public string PwinDtl;

	[JsonProperty("ptm")]
	public int Ptm;

	[JsonProperty("use_char")]
	public string UseChar;

	[JsonProperty("use_skill")]
	public string UseSkill;

	[JsonProperty("use_equipment")]
	public string UseEquipment;

	[JsonProperty("player_count")]
	public int? PlayerCount;

	[JsonProperty("anomaly_id")]
	public int? AnomalyId;

	[JsonProperty("artifact_count")]
	public int? ArtifactCount;

	[JsonProperty("unlock_talent")]
	public string UnlockTalent;

	[JsonProperty("get_soft_currency")]
	public int? GetSoftCurrency;

	[JsonProperty("get_hard_currency")]
	public int? GetHardCurrency;
}

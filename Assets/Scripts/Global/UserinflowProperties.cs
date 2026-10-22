using Newtonsoft.Json;

public class UserinflowProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("inflow_index")]
	public string InflowIndex;

	[JsonProperty("inflow_cat")]
	public string InflowCat;

	[JsonProperty("inflow_value")]
	public string InflowValue;

	[JsonProperty("inflow_dtl")]
	public string InflowDtl;
}

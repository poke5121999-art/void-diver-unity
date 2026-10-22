using Newtonsoft.Json;

public class ErrorReportProperties : GameAnalyticsEvent.EventProperties
{
	[JsonProperty("issue_id")]
	public string IssueId;

	[JsonProperty("variant_id")]
	public string VariantId;

	[JsonProperty("issue_title")]
	public string IssueTitle;

	[JsonProperty("issue_subtitle")]
	public string IssueSubtitle;

	[JsonProperty("severity")]
	public string Severity;

	[JsonProperty("error_type")]
	public string ErrorType;

	[JsonProperty("library_name")]
	public string LibraryName;

	[JsonProperty("free_ram_mb")]
	public int FreeRamMb;

	[JsonProperty("free_disk_mb")]
	public int FreeDiskMb;

	[JsonProperty("location_context")]
	public string LocationContext;
}

public class ErrorReportAnalyticsEvent : GameAnalyticsEvent
{
	public ErrorReportAnalyticsEvent()
	{
		base.Properties = new ErrorReportProperties();
	}

	public override string Event => "error_report";

	public new ErrorReportProperties Properties => (ErrorReportProperties)base.Properties;
}

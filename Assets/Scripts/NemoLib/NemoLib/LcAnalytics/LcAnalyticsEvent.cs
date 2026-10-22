using Newtonsoft.Json;

namespace NemoLib.LcAnalytics
{
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class LcAnalyticsEvent
	{
		[JsonProperty("userId")]
		public string UserId;

		[JsonProperty("event")]
		public abstract string Event { get; }
	}
}

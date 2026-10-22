using NemoLib.LcAnalytics;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public abstract class GameAnalyticsEvent : LcAnalyticsEvent
{
	[JsonObject(MemberSerialization.OptIn)]
	public class EventContext
	{
		[JsonProperty("os")]
		public readonly OsContext Os = new OsContext();

		[JsonProperty("traits")]
		public readonly TraitsContext Traits = new TraitsContext();

		[JsonProperty("timezone")]
		public string Timezone;

		[JsonProperty("locale")]
		public string Locale;

		[JsonProperty("sessionstart")]
		public bool SessionStart;

		[JsonProperty("sessionId")]
		public long SessionId;
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class OsContext
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("version")]
		public string Version;
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class TraitsContext
	{
		[JsonProperty("install_datetime")]
		public string InstallDatetime;

		[JsonProperty("install_date")]
		public string InstallDate;

		[JsonProperty("install_version")]
		public string InstallVersion;

		[JsonProperty("country")]
		public string Country;

		[JsonProperty("dev_build")]
		public bool DevBuild;
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class EventProperties
	{
		[JsonProperty("user_state")]
		public readonly UserState UserState = new UserState();
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class UserState
	{
		[JsonProperty("playtime")]
		public double Playtime;

		[JsonProperty("best_stage")]
		public int BestStage;

		[JsonProperty("account_power_score")]
		public double AccountPowerScore;

		[JsonProperty("u_lounge_id")]
		public string ULoungeId;

		[JsonProperty("multi_host_id")]
		public string MultiHostId;
	}

	[JsonProperty("context")]
	public readonly EventContext Context = new EventContext();

	[JsonProperty("properties")]
	public EventProperties Properties { get; protected set; } = new EventProperties();
}

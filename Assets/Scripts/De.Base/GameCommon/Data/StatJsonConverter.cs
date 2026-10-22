using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class StatJsonConverter : JsonConverter<Stat>
	{
		public override void WriteJson(JsonWriter writer, Stat value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override Stat ReadJson(JsonReader reader, Type objectType, Stat existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<Stat>(reader, Stat.TryParse);
		}
	}
}

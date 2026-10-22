using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class InteriorJsonConverter : JsonConverter<Interior>
	{
		public override void WriteJson(JsonWriter writer, Interior value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override Interior ReadJson(JsonReader reader, Type objectType, Interior existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<Interior>(reader, Interior.TryParse);
		}
	}
}

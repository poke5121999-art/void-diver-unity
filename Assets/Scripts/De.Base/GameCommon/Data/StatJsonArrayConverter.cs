using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class StatJsonArrayConverter : JsonConverter<Stat[]>
	{
		public override void WriteJson(JsonWriter writer, Stat[] value, JsonSerializer serializer)
		{
		}

		public override Stat[] ReadJson(JsonReader reader, Type objectType, Stat[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

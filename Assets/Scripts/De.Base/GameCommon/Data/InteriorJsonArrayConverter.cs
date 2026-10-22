using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class InteriorJsonArrayConverter : JsonConverter<Interior[]>
	{
		public override void WriteJson(JsonWriter writer, Interior[] value, JsonSerializer serializer)
		{
		}

		public override Interior[] ReadJson(JsonReader reader, Type objectType, Interior[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

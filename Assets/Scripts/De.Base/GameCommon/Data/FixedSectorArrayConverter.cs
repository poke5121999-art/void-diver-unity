using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class FixedSectorArrayConverter : JsonConverter<FixedSector[]>
	{
		public override void WriteJson(JsonWriter writer, FixedSector[] value, JsonSerializer serializer)
		{
			TokenJson.WriteArray(writer, value);
		}

		public override FixedSector[] ReadJson(JsonReader reader, Type objectType, FixedSector[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.ReadArray<FixedSector>(reader, FixedSector.TryParse);
		}
	}
}

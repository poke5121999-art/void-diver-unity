using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class FixedSectorConverter : JsonConverter<FixedSector>
	{
		public override void WriteJson(JsonWriter writer, FixedSector value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override FixedSector ReadJson(JsonReader reader, Type objectType, FixedSector existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<FixedSector>(reader, FixedSector.TryParse);
		}
	}
}

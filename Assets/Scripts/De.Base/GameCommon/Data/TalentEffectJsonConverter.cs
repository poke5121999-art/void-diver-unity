using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class TalentEffectJsonConverter : JsonConverter<TalentEffect>
	{
		public override void WriteJson(JsonWriter writer, TalentEffect value, JsonSerializer serializer)
		{
		}

		public override TalentEffect ReadJson(JsonReader reader, Type objectType, TalentEffect existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return default;
		}
	}
}

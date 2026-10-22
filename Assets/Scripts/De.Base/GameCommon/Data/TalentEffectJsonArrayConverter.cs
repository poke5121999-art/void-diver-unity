using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class TalentEffectJsonArrayConverter : JsonConverter<TalentEffect[]>
	{
		public override void WriteJson(JsonWriter writer, TalentEffect[] value, JsonSerializer serializer)
		{
			TokenJson.WriteArray(writer, value);
		}

		public override TalentEffect[] ReadJson(JsonReader reader, Type objectType, TalentEffect[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.ReadArray<TalentEffect>(reader, TalentEffect.TryParse);
		}
	}
}

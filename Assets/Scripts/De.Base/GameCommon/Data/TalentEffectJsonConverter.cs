using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class TalentEffectJsonConverter : JsonConverter<TalentEffect>
	{
		public override void WriteJson(JsonWriter writer, TalentEffect value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override TalentEffect ReadJson(JsonReader reader, Type objectType, TalentEffect existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<TalentEffect>(reader, TalentEffect.TryParse);
		}
	}
}

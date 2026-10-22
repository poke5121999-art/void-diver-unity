using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class ArtifactDealCardEffectJsonArrayConverter : JsonConverter<ArtifactDealCardEffect[]>
	{
		public override void WriteJson(JsonWriter writer, ArtifactDealCardEffect[] value, JsonSerializer serializer)
		{
			TokenJson.WriteArray(writer, value);
		}

		public override ArtifactDealCardEffect[] ReadJson(JsonReader reader, Type objectType, ArtifactDealCardEffect[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.ReadArray<ArtifactDealCardEffect>(reader, ArtifactDealCardEffect.TryParse);
		}
	}
}

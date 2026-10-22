using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class ArtifactDealCardEffectJsonConverter : JsonConverter<ArtifactDealCardEffect>
	{
		public override void WriteJson(JsonWriter writer, ArtifactDealCardEffect value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override ArtifactDealCardEffect ReadJson(JsonReader reader, Type objectType, ArtifactDealCardEffect existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<ArtifactDealCardEffect>(reader, ArtifactDealCardEffect.TryParse);
		}
	}
}

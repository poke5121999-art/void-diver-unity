using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class ArtifactDealCardEffectJsonArrayConverter : JsonConverter<ArtifactDealCardEffect[]>
	{
		public override void WriteJson(JsonWriter writer, ArtifactDealCardEffect[] value, JsonSerializer serializer)
		{
		}

		public override ArtifactDealCardEffect[] ReadJson(JsonReader reader, Type objectType, ArtifactDealCardEffect[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

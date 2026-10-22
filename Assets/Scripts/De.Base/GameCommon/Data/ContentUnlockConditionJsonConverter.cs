using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class ContentUnlockConditionJsonConverter : JsonConverter<EventCondition>
	{
		public override void WriteJson(JsonWriter writer, EventCondition value, JsonSerializer serializer)
		{
		}

		public override EventCondition ReadJson(JsonReader reader, Type objectType, EventCondition existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return default;
		}
	}
}

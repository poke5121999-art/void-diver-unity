using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class ContentUnlockConditionJsonArrayConverter : JsonConverter<EventCondition[]>
	{
		public override void WriteJson(JsonWriter writer, EventCondition[] value, JsonSerializer serializer)
		{
		}

		public override EventCondition[] ReadJson(JsonReader reader, Type objectType, EventCondition[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

using System;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	public class Vector2JsonArrayConverter : JsonConverter<Vector2[]>
	{
		public override void WriteJson(JsonWriter writer, Vector2[] value, JsonSerializer serializer)
		{
		}

		public override Vector2[] ReadJson(JsonReader reader, Type objectType, Vector2[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

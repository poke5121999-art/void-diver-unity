using System;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	public class Vector3JsonArrayConverter : JsonConverter<Vector3[]>
	{
		public override void WriteJson(JsonWriter writer, Vector3[] value, JsonSerializer serializer)
		{
		}

		public override Vector3[] ReadJson(JsonReader reader, Type objectType, Vector3[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

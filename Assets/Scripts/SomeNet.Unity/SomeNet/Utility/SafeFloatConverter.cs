using System;
using Newtonsoft.Json;

namespace SomeNet.Utility
{
	public class SafeFloatConverter : JsonConverter<float>
	{
		public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
		{
		}

		public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return 0f;
		}
	}
}

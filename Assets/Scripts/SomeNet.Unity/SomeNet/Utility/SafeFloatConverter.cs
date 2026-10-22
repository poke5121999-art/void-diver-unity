using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SomeNet.Utility
{
	public class SafeFloatConverter : JsonConverter<float>
	{
		public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString("F3", CultureInfo.InvariantCulture));
		}

		public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.Value is string text && float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
			{
				return parsed;
			}
			return Convert.ToSingle(reader.Value, CultureInfo.InvariantCulture);
		}
	}
}

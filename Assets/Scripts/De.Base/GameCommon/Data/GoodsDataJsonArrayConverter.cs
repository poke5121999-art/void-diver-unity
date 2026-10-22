using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class GoodsDataJsonArrayConverter : JsonConverter<GoodsData[]>
	{
		public override void WriteJson(JsonWriter writer, GoodsData[] value, JsonSerializer serializer)
		{
		}

		public override GoodsData[] ReadJson(JsonReader reader, Type objectType, GoodsData[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}
	}
}

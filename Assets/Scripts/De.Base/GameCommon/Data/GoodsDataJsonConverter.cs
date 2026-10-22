using System;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	public class GoodsDataJsonConverter : JsonConverter<GoodsData>
	{
		public override void WriteJson(JsonWriter writer, GoodsData value, JsonSerializer serializer)
		{
			TokenJson.Write(writer, value);
		}

		public override GoodsData ReadJson(JsonReader reader, Type objectType, GoodsData existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return TokenJson.Read<GoodsData>(reader, GoodsData.TryParse);
		}
	}
}

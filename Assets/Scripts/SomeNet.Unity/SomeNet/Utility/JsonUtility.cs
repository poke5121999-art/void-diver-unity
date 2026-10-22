using Newtonsoft.Json;

namespace SomeNet.Utility
{
	public static class JsonUtility
	{
		private static JsonSerializerSettings settings;

		public static string Serialize(object value)
		{
			return null;
		}

		public static T Deserialize<T>(string value)
		{
			return default;
		}

		public static void AddConverter(params JsonConverter[] converters)
		{
		}
	}
}

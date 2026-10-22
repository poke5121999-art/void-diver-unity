using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	public delegate bool TokenParser<T>(string text, out T value);

	// Shared reading/writing for table JSON converters whose values are "a:b:c" strings, arrays being "x|y|z".
	public static class TokenJson
	{
		public static T Read<T>(JsonReader reader, TokenParser<T> parser)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return default;
			}
			string text = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);
			if (string.IsNullOrEmpty(text))
			{
				return default;
			}
			if (!parser(text, out T value))
			{
				throw new JsonSerializationException($"invalid {typeof(T).Name} '{text}'");
			}
			return value;
		}

		public static T[] ReadArray<T>(JsonReader reader, TokenParser<T> parser)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			var list = new List<T>();
			if (reader.TokenType == JsonToken.StartArray)
			{
				while (reader.Read() && reader.TokenType != JsonToken.EndArray)
				{
					list.Add(Read(reader, parser));
				}
				return list.ToArray();
			}
			string text = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			foreach (string token in text.Split('|'))
			{
				if (!parser(token, out T value))
				{
					throw new JsonSerializationException($"invalid {typeof(T).Name} '{token}'");
				}
				list.Add(value);
			}
			return list.ToArray();
		}

		public static void Write<T>(JsonWriter writer, T value)
		{
			writer.WriteValue(Format(value));
		}

		public static void WriteArray<T>(JsonWriter writer, T[] values)
		{
			if (values == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue(string.Join("|", Array.ConvertAll(values, Format)));
		}

		public static bool TryParseVector3(string text, out Vector3 value)
		{
			value = default;
			string[] parts = text.Split(':');
			if (parts.Length != 3 || !TryFloat(parts[0], out float x) || !TryFloat(parts[1], out float y) || !TryFloat(parts[2], out float z))
			{
				return false;
			}
			value = new Vector3(x, y, z);
			return true;
		}

		public static bool TryParseVector2(string text, out Vector2 value)
		{
			value = default;
			string[] parts = text.Split(':');
			if (parts.Length != 2 || !TryFloat(parts[0], out float x) || !TryFloat(parts[1], out float y))
			{
				return false;
			}
			value = new Vector2(x, y);
			return true;
		}

		private static bool TryFloat(string text, out float value)
		{
			return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		private static string Format<T>(T value)
		{
			switch (value)
			{
			case Vector3 v3:
				return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", v3.x, v3.y, v3.z);
			case Vector2 v2:
				return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", v2.x, v2.y);
			default:
				return value?.ToString();
			}
		}
	}
}

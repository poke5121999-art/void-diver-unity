using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace System
{
	public static class StringExtensions
	{
		public static bool HasValue(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		public static string ToFirstLetterLower(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			return char.ToLower(input[0]).ToString() + input.Substring(1);
		}

		public static string ToPascalCase(this string text)
		{
			Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
			Regex whiteSpace = new Regex("(?<=\\s)");
			Regex startsWithLowerCaseChar = new Regex("^[a-z]");
			Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
			Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
			Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");
			IEnumerable<string> words = invalidCharsRgx.Replace(whiteSpace.Replace(text, "_"), string.Empty)
				.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries)
				.Select((string w) => startsWithLowerCaseChar.Replace(w, (Match m) => m.Value.ToUpper(CultureInfo.CurrentCulture)))
				.Select((string w) => firstCharFollowedByUpperCasesOnly.Replace(w, (Match m) => m.Value.ToLower(CultureInfo.CurrentCulture)))
				.Select((string w) => lowerCaseNextToNumber.Replace(w, (Match m) => m.Value.ToUpper(CultureInfo.CurrentCulture)))
				.Select((string w) => upperCaseInside.Replace(w, (Match m) => m.Value.ToLower(CultureInfo.CurrentCulture)));
			return string.Concat(words);
		}

		public static string ToCamelCase(this string text)
		{
			string pascal = text.ToPascalCase();
			return char.ToLower(pascal[0]).ToString() + pascal.Substring(1);
		}

		public static string ToSnakeCase(this string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length < 2)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(char.ToLowerInvariant(text[0]));
			for (int i = 1; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsUpper(c))
				{
					stringBuilder.Append('_');
					c = char.ToLowerInvariant(c);
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		public static bool IsEquals(this string textA, string textB)
		{
			if (textA == null || textB == null)
			{
				return textA == textB;
			}
			return textA.NormalizeForCompare() == textB.NormalizeForCompare();
		}

		private static string NormalizeForCompare(this string text)
		{
			return text.Replace("\r\n", "\n").Replace("\r", "\n").Normalize();
		}

		public static void ForEach<T>(this T[] source, Action<T> action)
		{
			// plain indexed loop; null source returns silently, null action raises NRE (@0x180dea430 machine code checked)
			if (source == null)
			{
				return;
			}
			for (int i = 0; i < source.Length; i++)
			{
				action(source[i]);
			}
		}

		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			// shuffles a ToList copy with a new System.Random, Next(n) per step (@0x180dece50 machine code checked)
			System.Random random = new System.Random();
			int count = list.Count;
			List<T> copy = list.ToList();
			while (count > 1)
			{
				int k = random.Next(count);
				count--;
				T value = copy[count];
				copy[count] = copy[k];
				copy[k] = value;
			}
			return copy;
		}

		public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
		{
			// null source returns default, null selector throws ArgumentNullException, first element seeds, strict < wins (@0x180dec460 machine code checked)
			if (source == null)
			{
				return default(T);
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			T result = default(T);
			TKey bestKey = default(TKey);
			bool hasAny = false;
			foreach (T item in source)
			{
				TKey key = keySelector(item);
				if (!hasAny || key.CompareTo(bestKey) < 0)
				{
					result = item;
					bestKey = key;
					hasAny = true;
				}
			}
			return result;
		}

		public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
		{
			// mirror of MinBy with strict > (@0x180deb7a0 machine code checked)
			if (source == null)
			{
				return default(T);
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			T result = default(T);
			TKey bestKey = default(TKey);
			bool hasAny = false;
			foreach (T item in source)
			{
				TKey key = keySelector(item);
				if (!hasAny || key.CompareTo(bestKey) > 0)
				{
					result = item;
					bestKey = key;
					hasAny = true;
				}
			}
			return result;
		}

		public static int IndexOf<T>(this IEnumerable<T> collection, T value)
		{
			// EqualityComparer<T>.Default over the enumerator, -1 when absent; null collection throws ArgumentNullException (@0x180deab50 machine code checked)
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			int num = 0;
			foreach (T item in collection)
			{
				if (EqualityComparer<T>.Default.Equals(item, value))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static string WithColor(this string text, string colorCode)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (!colorCode.StartsWith("#"))
			{
				colorCode = "#" + colorCode;
			}
			if (!Regex.IsMatch(colorCode, "^#[0-9A-Fa-f]{6}$"))
			{
				return string.Concat("<color=#FF0000> wrong color code ", colorCode, " | text ", text, "</color>");
			}
			return string.Concat("<color=", colorCode, ">", text, "</color>");
		}

		public static string WithColor(this string text, Color color)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			return text.WithColor(ColorUtility.ToHtmlStringRGB(color));
		}
	}
}

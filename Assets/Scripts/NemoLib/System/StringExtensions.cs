using System.Collections.Generic;
using UnityEngine;

namespace System
{
	public static class StringExtensions
	{
		public static bool HasValue(this string str)
		{
			return false;
		}

		public static string ToFirstLetterLower(this string input)
		{
			return null;
		}

		public static string ToPascalCase(this string text)
		{
			return null;
		}

		public static string ToCamelCase(this string text)
		{
			return null;
		}

		public static string ToSnakeCase(this string text)
		{
			return null;
		}

		public static bool IsEquals(this string textA, string textB)
		{
			return false;
		}

		private static string NormalizeForCompare(this string text)
		{
			return null;
		}

		public static void ForEach<T>(this T[] source, Action<T> action)
		{
		}

		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			return null;
		}

		public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
		{
			return default;
		}

		public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
		{
			return default;
		}

		public static int IndexOf<T>(this IEnumerable<T> collection, T value)
		{
			return 0;
		}

		public static string WithColor(this string text, string colorCode)
		{
			return null;
		}

		public static string WithColor(this string text, Color color)
		{
			return null;
		}
	}
}

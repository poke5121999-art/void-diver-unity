using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SomeNet.Utility
{
	public static class StringExtensions
	{
		private static readonly Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");

		private static readonly Regex whiteSpace = new Regex("(?<=\\s)");

		private static readonly Regex startsWithLowerCaseChar = new Regex("^[a-z]");

		private static readonly Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");

		private static readonly Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");

		private static readonly Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

		public static string ToPascalCase(this string text)
		{
			var words = invalidCharsRgx.Replace(whiteSpace.Replace(text, "_"), string.Empty)
				.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
				.Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
				.Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
				.Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));
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
			StringBuilder sb = new StringBuilder();
			sb.Append(char.ToLowerInvariant(text[0]));
			for (int i = 1; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsUpper(c))
				{
					sb.Append('_');
					c = char.ToLowerInvariant(c);
				}
				sb.Append(c);
			}
			return sb.ToString();
		}
	}
}

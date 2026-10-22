using System;
using System.Collections.Generic;
using System.Text;

namespace SomeNet.Table
{
	public static class CsvHelper
	{
		private const char OptionPrefix = '~';

		// Quoted cells lose their outer quotes but keep "" pairs; UnescapeForCsv resolves them for JSON cells only.
		public static string[][] Parse(string text, bool includeOptions)
		{
			var rows = new List<string[]>();
			if (string.IsNullOrEmpty(text))
			{
				return rows.ToArray();
			}
			if (text[0] == '﻿')
			{
				text = text.Substring(1);
			}
			string[] lines = text.Split('\n');
			foreach (string rawLine in lines)
			{
				string line = rawLine.TrimEnd('\r');
				if (line.Length == 0)
				{
					continue;
				}
				rows.Add(SplitLine(line));
			}
			if (includeOptions || rows.Count == 0)
			{
				return rows.ToArray();
			}
			string[] header = rows[0];
			var keep = new List<int>();
			for (int i = 0; i < header.Length; i++)
			{
				if (!header[i].StartsWith(OptionPrefix.ToString(), StringComparison.Ordinal))
				{
					keep.Add(i);
				}
			}
			var result = new string[rows.Count][];
			for (int r = 0; r < rows.Count; r++)
			{
				string[] src = rows[r];
				var dst = new string[keep.Count];
				for (int c = 0; c < keep.Count; c++)
				{
					dst[c] = keep[c] < src.Length ? src[keep[c]] : string.Empty;
				}
				result[r] = dst;
			}
			return result;
		}

		private static string[] SplitLine(string line)
		{
			var cells = new List<string>();
			var sb = new StringBuilder();
			int i = 0;
			while (true)
			{
				sb.Clear();
				if (i < line.Length && line[i] == '"')
				{
					i++;
					while (i < line.Length)
					{
						if (line[i] == '"')
						{
							if (i + 1 < line.Length && line[i + 1] == '"')
							{
								sb.Append("\"\"");
								i += 2;
								continue;
							}
							i++;
							break;
						}
						sb.Append(line[i++]);
					}
					while (i < line.Length && line[i] != ',')
					{
						sb.Append(line[i++]);
					}
				}
				else
				{
					while (i < line.Length && line[i] != ',')
					{
						sb.Append(line[i++]);
					}
				}
				cells.Add(sb.ToString());
				if (i >= line.Length)
				{
					break;
				}
				i++;
			}
			return cells.ToArray();
		}

		public static string EscapeForCsv(string value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) < 0)
			{
				return value;
			}
			return "\"" + value.Replace("\"", "\"\"") + "\"";
		}

		// Plain values become JSON string literals; object/array cells keep their JSON and only lose CSV quote doubling.
		public static string UnescapeForCsv(string value)
		{
			if (value == "\"\"")
			{
				return value;
			}
			if (value.StartsWith("{", StringComparison.Ordinal) || value.StartsWith("[", StringComparison.Ordinal))
			{
				return value.Replace("\"\"", "\"");
			}
			return "\"" + value + "\"";
		}
	}
}

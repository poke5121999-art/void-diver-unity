using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Serilog;

namespace GameCommon.Table
{
	public static class BitFlagIndexValidator
	{
		private const string FieldName = "BitFlagIndex";

		public static bool HasBitFlagIndex(Type dataType)
		{
			return dataType.GetProperty("BitFlagIndex", BindingFlags.Instance | BindingFlags.Public) != null;
		}

		public static bool Validate(Type dataType, out string error)
		{
			error = null;
			PropertyInfo bitFlagProperty = dataType.GetProperty("BitFlagIndex", BindingFlags.Instance | BindingFlags.Public);
			if (bitFlagProperty == null)
			{
				return true;
			}
			PropertyInfo pkProperty = dataType.GetProperty("Pk", BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo tableProperty = dataType.GetProperty("Table", BindingFlags.Static | BindingFlags.Public);
			object table = tableProperty.GetValue(null);
			object values = table.GetType().GetProperty("Values").GetValue(table);
			Dictionary<int, List<object>> byIndex = new Dictionary<int, List<object>>();
			foreach (object item in (IEnumerable)values)
			{
				int index = (int)bitFlagProperty.GetValue(item);
				if (index > 0)
				{
					object pk = pkProperty == null ? null : pkProperty.GetValue(item);
					if (!byIndex.TryGetValue(index, out List<object> list))
					{
						list = new List<object>();
						byIndex[index] = list;
					}
					list.Add(pk);
				}
			}
			if (byIndex.Count == 0)
			{
				return true;
			}
			List<int> sortedKeys = byIndex.Keys.OrderBy(_ => _).ToList();
			int min = sortedKeys[0];
			int max = sortedKeys[sortedKeys.Count - 1];
			List<string> errors = new List<string>();
			List<KeyValuePair<int, List<object>>> duplicates = byIndex.Where(_ => _.Value.Count > 1).OrderBy(_ => _.Key).ToList();
			if (duplicates.Any())
			{
				errors.Add("중복된 BitFlagIndex: " + string.Join(", ", duplicates.Select(_ => string.Format("{0}(Pk: {1})", _.Key, string.Join("/", _.Value)))));
			}
			if (min != 1)
			{
				errors.Add(string.Format("{0} 가 1 이 아닌 {1} 부터 시작합니다.", "BitFlagIndex", min));
			}
			HashSet<int> present = new HashSet<int>(sortedKeys);
			List<int> missing = new List<int>();
			for (int i = min; i <= max; i++)
			{
				if (!present.Contains(i))
				{
					missing.Add(i);
				}
			}
			if (missing.Any())
			{
				errors.Add(string.Format("{0} 빈자리({1}개): {2}", "BitFlagIndex", missing.Count, FormatRanges(missing)));
			}
			if (errors.Count == 0)
			{
				return true;
			}
			error = "[" + dataType.Name + "] " + string.Join(" / ", errors);
			// ILogger interface slot 0x39 (57) is ILogger.Error(string messageTemplate) per the metadata (machine code checked @0x180589e90)
			Log.Error(error);
			return false;
		}

		private static string FormatRanges(IReadOnlyList<int> sortedNumbers)
		{
			StringBuilder sb = new StringBuilder();
			int start = sortedNumbers[0];
			int end = start;
			for (int i = 1; i <= sortedNumbers.Count; i++)
			{
				if (i < sortedNumbers.Count && sortedNumbers[i] == end + 1)
				{
					end = sortedNumbers[i];
					continue;
				}
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				sb.Append(start == end ? string.Format("{0}", start) : string.Format("{0}-{1}", start, end));
				if (i < sortedNumbers.Count)
				{
					start = sortedNumbers[i];
					end = start;
				}
			}
			return sb.ToString();
		}
	}
}

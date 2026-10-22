using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using GameCommon.Data;
using SomeNet.Table;
using SomeNet.Utility;

namespace GameCommon.Table
{
	// Cell conversions shared by every generated TXxx.Parse / TXxx.ToCsv.
	public static class TableParse
	{
		public const char ArraySeparator = '|';

		public static int Int(string cell)
		{
			return int.Parse(cell, CultureInfo.InvariantCulture);
		}

		public static long Long(string cell)
		{
			return long.Parse(cell, CultureInfo.InvariantCulture);
		}

		public static byte Byte(string cell)
		{
			return byte.Parse(cell, CultureInfo.InvariantCulture);
		}

		public static float Float(string cell)
		{
			return float.Parse(cell, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// "{$}" stands for a comma inside a cell.
		public static string Str(string cell)
		{
			return cell.Replace("{$}", ",");
		}

		public static bool Bool(string cell)
		{
			return bool.Parse(cell);
		}

		public static T Enum<T>(string cell) where T : struct
		{
			return (T)System.Enum.Parse(typeof(T), cell);
		}

		public static T Scalar<T>(string cell)
		{
			Type type = typeof(T);
			if (type == typeof(string))
			{
				return (T)(object)cell;
			}
			if (type.IsEnum)
			{
				return (T)System.Enum.Parse(type, cell);
			}
			if (type == typeof(float))
			{
				return (T)(object)Float(cell);
			}
			if (type == typeof(bool))
			{
				return (T)(object)Bool(cell);
			}
			return (T)Convert.ChangeType(cell, type, CultureInfo.InvariantCulture);
		}

		public static T[] Array<T>(string cell)
		{
			if (string.IsNullOrEmpty(cell))
			{
				return null;
			}
			string[] parts = cell.Split(ArraySeparator);
			var result = new T[parts.Length];
			for (int i = 0; i < parts.Length; i++)
			{
				result[i] = Scalar<T>(parts[i]);
			}
			return result;
		}

		public static T Json<T>(string cell)
		{
			if (string.IsNullOrEmpty(cell))
			{
				return default;
			}
			return JsonUtility.Deserialize<T>(CsvHelper.UnescapeForCsv(cell));
		}

		public static string ToCsv(params object[] cells)
		{
			var parts = new string[cells.Length];
			for (int i = 0; i < cells.Length; i++)
			{
				parts[i] = CsvHelper.EscapeForCsv(Cell(cells[i]));
			}
			return string.Join(",", parts);
		}

		public static string Cell(object value)
		{
			switch (value)
			{
			case null:
				return string.Empty;
			case string text:
				return text.Replace(",", "{$}");
			case bool flag:
				return flag ? "TRUE" : "FALSE";
			case float number:
				return number.ToString("R", CultureInfo.InvariantCulture);
			case IFormattable formattable when value.GetType().IsPrimitive:
				return formattable.ToString(null, CultureInfo.InvariantCulture);
			case System.Enum:
				return value.ToString();
			case GoodsData _:
			case EventCondition _:
			case Stat _:
			case ArtifactDealCardEffect _:
			case TalentEffect _:
			case FixedSector _:
			case Interior _:
				return value.ToString();
			}
			if (value is System.Array array && IsScalarArray(array.GetType().GetElementType()))
			{
				var items = new List<string>();
				foreach (object item in (IEnumerable)array)
				{
					items.Add(item is string text ? text : Cell(item));
				}
				return string.Join(ArraySeparator.ToString(), items);
			}
			string json = JsonUtility.Serialize(value);
			return json.Length > 1 && json[0] == '"' ? JsonUtility.Deserialize<string>(json) : json;
		}

		private static bool IsScalarArray(Type element)
		{
			return element.IsPrimitive || element.IsEnum || element == typeof(string) || element == typeof(GoodsData) || element == typeof(EventCondition) || element == typeof(Stat) || element == typeof(ArtifactDealCardEffect) || element == typeof(TalentEffect) || element == typeof(FixedSector) || element == typeof(Interior);
		}
	}
}

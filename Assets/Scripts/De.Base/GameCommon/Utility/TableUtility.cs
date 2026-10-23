using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GameCommon.Table;
using Serilog;

namespace GameCommon.Utility
{
	public static class TableUtility
	{
		private static Dictionary<string, Type> _commonTypes;

		private static readonly Type[] PriorityTypes = new Type[] { typeof(TItem), typeof(TEquipment) };

		private static int GetLoadPriority(Type dataType)
		{
			if (dataType != null)
			{
				int index = Array.IndexOf(PriorityTypes, dataType);
				if (index > -1)
				{
					return index;
				}
			}
			return int.MaxValue;
		}

		private static Type ResolveDataType(string fileName, IList<Type> tableDataTypes)
		{
			foreach (Type type in tableDataTypes)
			{
				string tableName = type.Name.Substring(1);
				if (tableName == fileName || fileName.StartsWith(tableName + "_", StringComparison.Ordinal))
				{
					return type;
				}
			}
			return null;
		}

		// Types that fail to load (e.g. when the game sources are recompiled into a tool assembly) are skipped.
		private static IEnumerable<Type> LoadedTypes()
		{
			try
			{
				return typeof(TItem).Assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null);
			}
		}

		public static IList<Type> GetTableDataTypes()
		{
			return LoadedTypes().Where(IsTableDataType).ToList();
		}

		private static bool IsTableDataType(Type type)
		{
			try
			{
				return type.Namespace == "GameCommon.Table" && type.GetInterfaces().Any(i => i.Name.StartsWith("ITableData", StringComparison.Ordinal));
			}
			catch (Exception e) when (e is BadImageFormatException || e is TypeLoadException)
			{
				return false;
			}
		}

		public static Type GetCommonType(string typeName)
		{
			if (_commonTypes == null)
			{
				_commonTypes = LoadedTypes()
					.Where(IsCommonType)
					.GroupBy(t => t.Name)
					.ToDictionary(g => g.Key, g => g.First());
			}
			_commonTypes.TryGetValue(typeName, out Type type);
			return type;
		}

		private static bool IsCommonType(Type type)
		{
			try
			{
				return type.Namespace == "GameCommon.Data" || type.Namespace == "GameCommon.Enum";
			}
			catch (Exception e) when (e is BadImageFormatException || e is TypeLoadException)
			{
				return false;
			}
		}

		public static bool HasType(string typeName)
		{
			return GetCommonType(typeName) != null;
		}

		public static void LoadTables(string dirPath)
		{
			IList<Type> tableDataTypes = GetTableDataTypes();
			LoadFiles(dirPath, "*.csv", ".csv", tableDataTypes, LoadTable);
		}

		public static void LoadTable(Type dataType, string filePath)
		{
			if (!File.Exists(filePath))
			{
				Log.Error(filePath + " not exists.");
				return;
			}
			dataType.GetMethod("LoadFromPath", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { filePath });
		}

		public static void LoadEncryptedTables(string dirPath, byte key)
		{
			IList<Type> tableDataTypes = GetTableDataTypes();
			LoadFiles(dirPath, "*.csv", ".csv", tableDataTypes, LoadTable);
			LoadFiles(dirPath, "*.bytes", ".bytes", tableDataTypes, (type, path) => LoadDecryptedTable(type, path, key));
		}

		// TODO(decomp): the original calls NemoLib EncryptUtility.DecryptFile(filePath, key), which XORs with key ^ 18 ^ 52.
		// TableHelper passes 0xCC (not Const.Encrypt.TABLE_KEY = 234), so the plain XOR below is kept to match it.
		public static void LoadDecryptedTable(Type dataType, string filePath, byte key)
		{
			if (!File.Exists(filePath))
			{
				Log.Error(filePath + " not exists.");
				return;
			}
			byte[] bytes = File.ReadAllBytes(filePath);
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] ^= key;
			}
			string text = new UTF8Encoding(false).GetString(bytes);
			dataType.GetMethod("LoadFromText", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { text });
		}

		public static bool ValidateTables()
		{
			bool ok = true;
			foreach (Type type in GetTableDataTypes())
			{
				ok &= (bool)type.GetMethod("ValidateTable", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
			}
			return ok;
		}

		// Items and equipment load first because GoodsData cells resolve their goods type from them.
		private static void LoadFiles(string dirPath, string pattern, string extension, IList<Type> tableDataTypes, Action<Type, string> load)
		{
			List<(string filePath, string fileName, Type type)> files = Directory.GetFiles(dirPath, pattern)
				.Select(filePath =>
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					return (filePath, fileName, ResolveDataType(fileName, tableDataTypes));
				})
				.OrderBy(file => GetLoadPriority(file.Item3))
				.ToList();
			foreach ((string filePath, string fileName, Type type) in files)
			{
				if (type == null)
				{
					Log.Error("No matching data type for " + fileName + extension);
				}
				else
				{
					load(type, filePath);
				}
			}
		}

		public static void Localize()
		{
			foreach (Type dataType in GetTableDataTypes())
			{
				LocalizeTable(dataType);
			}
		}

		public static void LocalizeTable(Type dataType)
		{
			PropertyInfo tableProperty = dataType?.GetProperty("Table", BindingFlags.Public | BindingFlags.Static);
			if (tableProperty == null)
			{
				return;
			}
			PropertyInfo[] tableProperties = tableProperty.PropertyType.GetProperties();
			object table = tableProperty.GetValue(null);
			PropertyInfo valueTypeProperty = tableProperties.First(p => p.Name == "ValueType");
			if (!(valueTypeProperty.GetValue(table) is Type valueType))
			{
				return;
			}
			PropertyInfo[] localizedProperties = valueType.GetProperties()
				.Where(p => p.IsDefined(typeof(LocalizedAttribute)))
				.Where(p => p.CanWrite)
				.ToArray();
			if (!localizedProperties.Any())
			{
				return;
			}
			PropertyInfo valuesProperty = tableProperties.First(p => p.Name == "Values");
			if (!(valuesProperty.GetValue(table) is IEnumerable rows))
			{
				return;
			}
			foreach (object row in rows)
			{
				foreach (PropertyInfo property in localizedProperties)
				{
					if (!property.PropertyType.IsArray)
					{
						string key = property.GetValue(row) as string;
						if (!string.IsNullOrEmpty(key))
						{
							property.SetValue(row, LocalizationUtility.GetLocalizedText(key));
						}
					}
					else if (property.GetValue(row) is string[] keys)
					{
						property.SetValue(row, keys.Select(LocalizationUtility.GetLocalizedText).ToArray());
					}
				}
			}
		}

		private static bool IsSystemType(string type)
		{
			switch (type)
			{
			case "byte":
			case "sbyte":
			case "short":
			case "int":
			case "long":
			case "ushort":
			case "uint":
			case "ulong":
			case "float":
			case "double":
			case "bool":
			case "char":
			case "decimal":
			case "string":
				return true;
			default:
				return false;
			}
		}

		private static bool CanParseSystemType(string type, string value)
		{
			switch (type)
			{
			case "string":
				return true;
			case "decimal":
				return decimal.TryParse(value, out _);
			case "ushort":
				return ushort.TryParse(value, out _);
			case "byte":
				return byte.TryParse(value, out _);
			case "int":
				return int.TryParse(value, out _);
			case "ulong":
				return ulong.TryParse(value, out _);
			case "double":
				return double.TryParse(value, out _);
			case "float":
				return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
			case "char":
				return char.TryParse(value, out _);
			case "short":
				return short.TryParse(value, out _);
			case "long":
				return long.TryParse(value, out _);
			case "bool":
				return bool.TryParse(value, out _);
			case "uint":
				return uint.TryParse(value, out _);
			case "sbyte":
				return sbyte.TryParse(value, out _);
			default:
				return false;
			}
		}

		private static string GetColorString(string value)
		{
			return "\u001b[93m" + value + "\u001b[0m";
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GameCommon.Table;

namespace GameCommon.Utility
{
	public static class TableUtility
	{
		private static Dictionary<string, Type> _commonTypes;

		private static readonly Type[] PriorityTypes = new Type[] { typeof(TItem), typeof(TEquipment) };

		private static int GetLoadPriority(Type dataType)
		{
			int index = Array.IndexOf(PriorityTypes, dataType);
			return index < 0 ? PriorityTypes.Length : index;
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
			LoadAll(dirPath, ("*.csv", LoadTable));
		}

		public static void LoadTable(Type dataType, string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(filePath + " not exists.");
			}
			dataType.GetMethod("LoadFromPath", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { filePath });
		}

		public static void LoadEncryptedTables(string dirPath, byte key)
		{
			LoadAll(dirPath, ("*.csv", LoadTable), ("*.bytes", (type, path) => LoadDecryptedTable(type, path, key)));
		}

		public static void LoadDecryptedTable(Type dataType, string filePath, byte key)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(filePath + " not exists.");
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
		private static void LoadAll(string dirPath, params (string pattern, Action<Type, string> load)[] sources)
		{
			IList<Type> dataTypes = GetTableDataTypes();
			var jobs = new List<(string fileName, string path, Type type, Action<Type, string> load)>();
			foreach ((string pattern, Action<Type, string> load) in sources)
			{
				foreach (string path in Directory.GetFiles(dirPath, pattern))
				{
					string fileName = Path.GetFileNameWithoutExtension(path);
					Type type = ResolveDataType(fileName, dataTypes);
					if (type == null)
					{
						throw new InvalidOperationException("No matching data type for " + Path.GetFileName(path) + ".");
					}
					jobs.Add((fileName, path, type, load));
				}
			}
			foreach (var job in jobs.OrderBy(j => GetLoadPriority(j.type)).ThenBy(j => j.fileName, StringComparer.Ordinal))
			{
				job.load(job.type, job.path);
			}
		}

		public static void Localize()
		{
		}

		public static void LocalizeTable(Type dataType)
		{
		}

		private static bool IsSystemType(string type)
		{
			return false;
		}

		private static bool CanParseSystemType(string type, string value)
		{
			return false;
		}

		private static string GetColorString(string value)
		{
			return null;
		}
	}
}

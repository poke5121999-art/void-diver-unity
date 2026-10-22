using System;
using System.Collections.Generic;

namespace GameCommon.Utility
{
	public static class TableUtility
	{
		private static Dictionary<string, Type> _commonTypes;

		private static readonly Type[] PriorityTypes;

		private static int GetLoadPriority(Type dataType)
		{
			return 0;
		}

		private static Type ResolveDataType(string fileName, IList<Type> tableDataTypes)
		{
			return null;
		}

		public static IList<Type> GetTableDataTypes()
		{
			return null;
		}

		public static Type GetCommonType(string typeName)
		{
			return null;
		}

		public static bool HasType(string typeName)
		{
			return false;
		}

		public static void LoadTables(string dirPath)
		{
		}

		public static void LoadTable(Type dataType, string filePath)
		{
		}

		public static void LoadEncryptedTables(string dirPath, byte key)
		{
		}

		public static void LoadDecryptedTable(Type dataType, string filePath, byte key)
		{
		}

		public static bool ValidateTables()
		{
			return false;
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

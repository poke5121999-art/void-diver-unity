using System;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	public static class ValidationExtensions
	{
		private enum ETableType
		{
			None = 0,
			Table = 1,
			SubTable = 2,
			GroupTable = 3
		}

		public static Func<string, bool> SfxGroupExists { get; set; }

		public static void LogError<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
		}

		public static void LogInformation<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
		}

		public static bool Check<TKey>(this ITableData<TKey> data, bool condition, string message) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type sourceType, Type refType) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefSubKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return false;
		}

		private static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, object sourceValue, Type refType, ETableType tableType) where TKey : IComparable
		{
			return false;
		}

		private static bool ValidateRefMultiKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType, ETableType tableType) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateSpawnableMonsterId<TKey>(this ITableData<TKey> data, string sourceField, int monsterId) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateSpawnableMonster<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}

		public static bool ContainsSfx(string sfxName)
		{
			return false;
		}

		public static bool ValidateRefSfx<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefGoodsData<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefEventCondition<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefTalentEffect<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}

		private static bool CheckSkillRef<TKey>(ITableData<TKey> data, string sourceField, TalentEffect effect, int skillId, string valueField) where TKey : IComparable
		{
			return false;
		}

		public static bool ValidateRefInterior<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			return false;
		}
	}
}

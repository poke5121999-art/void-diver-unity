using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using GameCommon.Data;
using Serilog;
using SomeNet.Table;

namespace GameCommon.Table
{
	// TODO(decomp): every method here except ContainsSfx and SfxGroupExists is generic and has no pseudo-C in the binary.
	// The key checks are rebuilt from their signatures and call sites; the content checks only report that the data was reached.
	public static class ValidationExtensions
	{
		private enum ETableType
		{
			None = 0,
			Table = 1,
			SubTable = 2,
			GroupTable = 3
		}

		private const BindingFlags InstanceAny = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static Func<string, bool> SfxGroupExists { get; set; }

		public static void LogError<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
			Log.Error("[{Table}:{Key}] {Message}", data.GetType().Name, data.Pk, message);
		}

		public static void LogInformation<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
			Log.Information("[{Table}:{Key}] {Message}", data.GetType().Name, data.Pk, message);
		}

		public static bool Check<TKey>(this ITableData<TKey> data, bool condition, string message) where TKey : IComparable
		{
			if (!condition)
			{
				data.LogError(message);
			}
			return condition;
		}

		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			return data.ValidateRefKey(sourceField, GetFieldValue(data, sourceField), refType, ETableType.Table);
		}

		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type sourceType, Type refType) where TKey : IComparable
		{
			return data.ValidateRefKey(sourceField, GetFieldValue(data, sourceField), refType, ETableType.Table);
		}

		public static bool ValidateRefSubKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return data.ValidateRefMultiKey(sourceField1, sourceField2, refType, ETableType.SubTable);
		}

		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			return data.ValidateRefKey(sourceField, GetFieldValue(data, sourceField), refType, ETableType.GroupTable);
		}

		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return data.ValidateRefMultiKey(sourceField1, sourceField2, refType, ETableType.GroupTable);
		}

		private static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, object sourceValue, Type refType, ETableType tableType) where TKey : IComparable
		{
			object table = GetStaticTable(refType, tableType);
			if (table == null)
			{
				return data.Check(false, refType.Name + "." + tableType + " is not loaded (referenced by " + sourceField + ")");
			}
			if (sourceValue is IEnumerable values && !(sourceValue is string))
			{
				bool all = true;
				foreach (object value in values)
				{
					all &= data.Check(ContainsKey(table, value), sourceField + " " + value + " not found in " + refType.Name);
				}
				return all;
			}
			return data.Check(ContainsKey(table, sourceValue), sourceField + " " + sourceValue + " not found in " + refType.Name);
		}

		private static bool ValidateRefMultiKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType, ETableType tableType) where TKey : IComparable
		{
			object table = GetStaticTable(refType, tableType);
			if (table == null)
			{
				return data.Check(false, refType.Name + "." + tableType + " is not loaded (referenced by " + sourceField1 + ", " + sourceField2 + ")");
			}
			object value1 = GetFieldValue(data, sourceField1);
			object value2 = GetFieldValue(data, sourceField2);
			return data.Check(ContainsKey(table, value1, value2), sourceField1 + " " + value1 + ", " + sourceField2 + " " + value2 + " not found in " + refType.Name);
		}

		public static bool ValidateSpawnableMonsterId<TKey>(this ITableData<TKey> data, string sourceField, int monsterId) where TKey : IComparable
		{
			// TODO(decomp): spawnable rules (phase, category) are not recoverable; only existence is checked
			return data.Check(TMonster.Table != null && TMonster.Table.ContainsKey(monsterId), sourceField + " monster " + monsterId + " not found");
		}

		public static bool ValidateSpawnableMonster<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			// TODO(decomp): the field's element type is not recoverable
			return true;
		}

		public static bool ContainsSfx(string sfxName)
		{
			if (SfxGroupExists != null && !string.IsNullOrEmpty(sfxName))
			{
				return SfxGroupExists(sfxName);
			}
			return true;
		}

		public static bool ValidateRefSfx<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			object value = GetFieldValue(data, sourceField);
			if (value is IEnumerable values && !(value is string))
			{
				bool all = true;
				foreach (object sfx in values)
				{
					all &= data.Check(ContainsSfx(sfx as string), sourceField + " sfx " + sfx + " not found");
				}
				return all;
			}
			return data.Check(ContainsSfx(value as string), sourceField + " sfx " + value + " not found");
		}

		public static bool ValidateRefGoodsData<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			// TODO(decomp): per-goods table lookups are not recoverable
			return true;
		}

		public static bool ValidateRefEventCondition<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			// TODO(decomp): per-condition reference rules are not recoverable
			return true;
		}

		public static bool ValidateRefTalentEffect<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			// TODO(decomp): per-effect reference rules are not recoverable
			return true;
		}

		private static bool CheckSkillRef<TKey>(ITableData<TKey> data, string sourceField, TalentEffect effect, int skillId, string valueField) where TKey : IComparable
		{
			// TODO(decomp): skill reference rules are not recoverable
			return true;
		}

		public static bool ValidateRefInterior<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			// TODO(decomp): interior reference rules are not recoverable
			return true;
		}

		private static object GetFieldValue(object data, string name)
		{
			Type type = data.GetType();
			PropertyInfo property = type.GetProperty(name, InstanceAny);
			if (property != null)
			{
				return property.GetValue(data);
			}
			return type.GetField(name, InstanceAny)?.GetValue(data);
		}

		private static object GetStaticTable(Type refType, ETableType tableType)
		{
			string name = tableType == ETableType.SubTable ? "SubTable" : tableType == ETableType.GroupTable ? "GroupTable" : "Table";
			return refType.GetProperty(name, BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
		}

		private static bool ContainsKey(object table, params object[] keys)
		{
			if (keys.Any(k => k == null))
			{
				return false;
			}
			MethodInfo method = table.GetType().GetMethods().FirstOrDefault(m => m.Name == "ContainsKey" && m.GetParameters().Length == keys.Length);
			if (method == null)
			{
				return false;
			}
			ParameterInfo[] parameters = method.GetParameters();
			object[] args = new object[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				args[i] = parameterType.IsEnum ? System.Enum.ToObject(parameterType, keys[i]) : Convert.ChangeType(keys[i], parameterType);
			}
			return (bool)method.Invoke(table, args);
		}
	}
}

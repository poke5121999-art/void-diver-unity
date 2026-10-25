using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCommon.Data;
using GameCommon.Enum;
using Serilog;
using SomeNet.Table;

namespace GameCommon.Table
{
	// Every method except ContainsSfx and SfxGroupExists is generic; the bodies below were read from the shared
	// instantiations (ValidationExtensions.<name><object / int> in GameAssembly), see the address on each method.
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

		// LogError<object> @0x180ee1c90 (machine code checked): Pk is read by reflection, and the line goes through Log.Error(string)
		public static void LogError<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
			object pk = data.GetType().GetProperty("Pk")?.GetValue(data);
			Log.Error(string.Format("[{0}:{1}] {2}", data.GetType().Name, pk, message));
		}

		// LogInformation<object> @0x180ee1e10 (machine code checked): same line through Log.Information(string)
		public static void LogInformation<TKey>(this ITableData<TKey> data, string message) where TKey : IComparable
		{
			object pk = data.GetType().GetProperty("Pk")?.GetValue(data);
			Log.Information(string.Format("[{0}:{1}] {2}", data.GetType().Name, pk, message));
		}

		// Check<object> @0x180ee14f0 (machine code checked): the LogError call is inlined
		public static bool Check<TKey>(this ITableData<TKey> data, bool condition, string message) where TKey : IComparable
		{
			if (!condition)
			{
				data.LogError(message);
			}
			return condition;
		}

		// ValidateRefKey<object> (3 args) @0x180eeaf20 (machine code checked)
		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			PropertyInfo property = data.GetType().GetProperty(sourceField, InstanceAny);
			object value = property.GetValue(data);
			if (property.PropertyType == typeof(int) && (int)value <= 0)
			{
				return true;
			}
			return data.ValidateRefKey(sourceField, value, refType, ETableType.Table);
		}

		// ValidateRefKey<object> (4 args) @0x180eeb080 (machine code checked)
		public static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, Type sourceType, Type refType) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField)?.GetValue(data);
			return data.ValidateRefKey(sourceField, Convert.ChangeType(value, sourceType), refType, ETableType.Table);
		}

		// ValidateRefSubKey<object> @0x180eee5e0 (machine code checked)
		public static bool ValidateRefSubKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return data.ValidateRefMultiKey(sourceField1, sourceField2, refType, ETableType.SubTable);
		}

		// ValidateRefGroupKey<object> (3 args) @0x180ee9a60 (machine code checked)
		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField, Type refType) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField).GetValue(data);
			return data.ValidateRefKey(sourceField, value, refType, ETableType.GroupTable);
		}

		// ValidateRefGroupKey<object> (4 args) @0x180ee9b10 (machine code checked)
		public static bool ValidateRefGroupKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType) where TKey : IComparable
		{
			return data.ValidateRefMultiKey(sourceField1, sourceField2, refType, ETableType.GroupTable);
		}

		// ValidateRefKey<object> (5 args) @0x180eea3e0 (machine code checked)
		private static bool ValidateRefKey<TKey>(this ITableData<TKey> data, string sourceField, object sourceValue, Type refType, ETableType tableType) where TKey : IComparable
		{
			object table = refType.GetProperty(string.Format("{0}", tableType), BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
			MethodInfo containsKey = table.GetType().GetMethod("ContainsKey");
			if (data.GetType().GetProperty(sourceField, InstanceAny).PropertyType.IsArray)
			{
				if (sourceValue == null)
				{
					return true;
				}
				List<object> missing = new List<object>();
				foreach (object item in (IEnumerable)sourceValue)
				{
					if (!(bool)containsKey.Invoke(table, new object[1] { item }))
					{
						missing.Add(item);
					}
				}
				if (!missing.Any())
				{
					return true;
				}
				data.LogError(string.Format("{0}.{1} not contains \"[{2}] {3}\"", refType.Name, tableType, sourceField, string.Join(", ", missing.Select((object _) => string.Format("{0}", _)))));
				return false;
			}
			if (sourceValue.GetType() == typeof(int) && (int)sourceValue <= 0)
			{
				return true;
			}
			if (sourceValue.GetType() == typeof(string) && string.IsNullOrEmpty((string)sourceValue))
			{
				return true;
			}
			if ((bool)containsKey.Invoke(table, new object[1] { sourceValue }))
			{
				return true;
			}
			data.LogError(string.Format("{0}.{1} not contains \"[{2}] {3}\"", refType.Name, tableType, sourceField, sourceValue));
			return false;
		}

		// ValidateRefMultiKey<object> @0x180eed730 and its lambda b__10_0 @0x180f57230 (machine code checked)
		private static bool ValidateRefMultiKey<TKey>(this ITableData<TKey> data, string sourceField1, string sourceField2, Type refType, ETableType tableType) where TKey : IComparable
		{
			object value1 = data.GetType().GetProperty(sourceField1).GetValue(data);
			object value2 = data.GetType().GetProperty(sourceField2).GetValue(data);
			object table = refType.GetProperty(string.Format("{0}", tableType), BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
			MethodInfo containsKey = table.GetType().GetMethods().Where((MethodInfo _) => _.Name == "ContainsKey").First();
			if ((bool)containsKey.Invoke(table, new object[2] { value1, value2 }))
			{
				return true;
			}
			data.LogError(string.Format("{0}.{1} not contains \"[{2}] {3}\", \"[{4}] {5}\"", refType.Name, tableType, sourceField1, value1, sourceField2, value2));
			return false;
		}

		// ValidateSpawnableMonsterId<int> @0x180eeee60 (machine code checked)
		public static bool ValidateSpawnableMonsterId<TKey>(this ITableData<TKey> data, string sourceField, int monsterId) where TKey : IComparable
		{
			if (monsterId <= 0)
			{
				return true;
			}
			if (!TMonster.Table.TryGetValue(monsterId, out var monster))
			{
				data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}\"", "TMonster", sourceField, monsterId));
				return false;
			}
			if (monster.IsBasePhaseMonster)
			{
				return true;
			}
			data.LogError(string.Format("\"[{0}] {1}\" is a non-base phase variant (BasePhaseMonsterId={2}). Only base-phase monsters can be spawned.", sourceField, monsterId, monster.BasePhaseMonsterId));
			return false;
		}

		// ValidateSpawnableMonster<int> @0x180eef3c0 (machine code checked): the property holds a monster id or an array of them
		public static bool ValidateSpawnableMonster<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			PropertyInfo property = data.GetType().GetProperty(sourceField, InstanceAny);
			object value = property.GetValue(data);
			if (value == null)
			{
				return true;
			}
			bool ok = true;
			if (property.PropertyType.IsArray)
			{
				foreach (object item in (IEnumerable)value)
				{
					if (!data.ValidateSpawnableMonsterId(sourceField, Convert.ToInt32(item)))
					{
						ok = false;
					}
				}
				return ok;
			}
			return data.ValidateSpawnableMonsterId(sourceField, Convert.ToInt32(value));
		}

		public static bool ContainsSfx(string sfxName)
		{
			if (SfxGroupExists != null && !string.IsNullOrEmpty(sfxName))
			{
				return SfxGroupExists(sfxName);
			}
			return true;
		}

		// ValidateRefSfx<int> @0x180eee320 (machine code checked)
		public static bool ValidateRefSfx<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			PropertyInfo property = data.GetType().GetProperty(sourceField);
			object value = property.GetValue(data);
			if (value == null)
			{
				return true;
			}
			string[] sfxNames = (property.PropertyType.IsArray ? ((string[])value) : new string[1] { (string)value });
			bool ok = true;
			foreach (string sfx in sfxNames)
			{
				if (!ContainsSfx(sfx))
				{
					data.LogError("MasterAudio not contains \"[" + sourceField + "] " + sfx + "\"");
					ok = false;
				}
			}
			return ok;
		}

		// ValidateRefGoodsData<int> @0x180ee91c0 (machine code checked): Item (2), Equipment (3) and Bag (4) goods must exist in their tables
		public static bool ValidateRefGoodsData<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField).GetValue(data);
			if (value == null)
			{
				return true;
			}
			GoodsData[] goods = (value.GetType().IsArray ? ((GoodsData[])value) : new GoodsData[1] { (GoodsData)value });
			bool ok = true;
			foreach (GoodsData goodsData in goods)
			{
				switch (goodsData.Category)
				{
				case EGoodsCategory.Item:
					if (!TItem.Table.ContainsKey(goodsData.Id))
					{
						data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}\"", TItem.Table.GetType().Name, sourceField, goodsData.Type, goodsData.Id));
						ok = false;
					}
					break;
				case EGoodsCategory.Equipment:
					if (!TEquipment.Table.ContainsKey(goodsData.Id))
					{
						data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}\"", TEquipment.Table.GetType().Name, sourceField, goodsData.Type, goodsData.Id));
						ok = false;
					}
					break;
				case EGoodsCategory.Bag:
					if (!TBag.Table.ContainsKey(goodsData.Id))
					{
						data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}\"", TBag.Table.GetType().Name, sourceField, goodsData.Type, goodsData.Id));
						ok = false;
					}
					break;
				}
			}
			return ok;
		}

		// ValidateRefEventCondition<int> @0x180ee65f0 (decomp read): a Dictionary<EEventConditionType, (table, ContainsKey, name)> is built with every
		// table touched (a null table throws), then each condition with a mapped type and Value1 > 0 must have (int)Value1 in that table
		public static bool ValidateRefEventCondition<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField).GetValue(data);
			if (value == null)
			{
				return true;
			}
			EventCondition[] conditions = (value.GetType().IsArray ? ((EventCondition[])value) : new EventCondition[1] { (EventCondition)value });
			Dictionary<EEventConditionType, (object, MethodInfo, string)> tables = new Dictionary<EEventConditionType, (object, MethodInfo, string)>();
			void Map(EEventConditionType type, object table, string name)
			{
				tables.Add(type, (table, table.GetType().GetMethod("ContainsKey"), name));
			}
			Map(EEventConditionType.MonsterKill, TMonster.Table, "TMonster");
			Map(EEventConditionType.WaveClear, TWave.Table, "TWave");
			Map(EEventConditionType.OpenRewardBox, TRewardBox.Table, "TRewardBox");
			Map(EEventConditionType.CampaignClear, TCampaign.Table, "TCampaign");
			Map(EEventConditionType.MissionClear, TMission.Table, "TMission");
			Map(EEventConditionType.ItemUse, TItem.Table, "TItem");
			Map(EEventConditionType.MyItemCountInInventory, TItem.Table, "TItem");
			Map(EEventConditionType.MyItemCountTotal, TItem.Table, "TItem");
			Map(EEventConditionType.TeamItemCountInInventory, TItem.Table, "TItem");
			Map(EEventConditionType.UserLevel, TLevel.Table, "TLevel");
			Map(EEventConditionType.CampaignCleared, TCampaign.Table, "TCampaign");
			Map(EEventConditionType.MissionCleared, TMission.Table, "TMission");
			Map(EEventConditionType.ItemCollected, TItem.Table, "TItem");
			Map(EEventConditionType.Talent, TTalent.Table, "TTalent");
			Map(EEventConditionType.ItemUsed, TItem.Table, "TItem");
			Map(EEventConditionType.LoungeQuestCleared, TLoungeQuest.Table, "TLoungeQuest");
			Map(EEventConditionType.AreaDecorationUnlocked, TAreaDecoration.Table, "TAreaDecoration");
			Map(EEventConditionType.AccountRecord, TAccountRecord.Table, "TAccountRecord");
			Map(EEventConditionType.StageEscape, TCampaign.Table, "TCampaign");
			Map(EEventConditionType.ArtifactDealFinish, TArtifactDeal.Table, "TArtifactDeal");
			Map(EEventConditionType.EmployeeHire, TEmployee.Table, "TEmployee");
			Map(EEventConditionType.EmployeeFire, TEmployee.Table, "TEmployee");
			Map(EEventConditionType.AreaUnlocked, TArea.Table, "TArea");
			Map(EEventConditionType.NpcLevel, TNpc.Table, "TNpc");
			bool ok = true;
			foreach (EventCondition condition in conditions)
			{
				if (condition.Type != EEventConditionType.None && tables.TryGetValue(condition.Type, out var entry) && (int)condition.Value1 > 0)
				{
					if (!(bool)entry.Item2.Invoke(entry.Item1, new object[1] { (int)condition.Value1 }))
					{
						data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}:{4}\"", entry.Item3, sourceField, condition.Type, condition.Value1, condition.Value2));
						ok = false;
					}
				}
			}
			return ok;
		}

		// ValidateRefTalentEffect<int> @0x180eee970 (machine code checked): AddSkill/UpgradeSkill (201, 202) check Value1, UnlockSkillMode/ChangeSkill (203, 204) check Value1 and Value2
		public static bool ValidateRefTalentEffect<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField).GetValue(data);
			if (value == null)
			{
				return true;
			}
			TalentEffect[] effects = (value.GetType().IsArray ? ((TalentEffect[])value) : new TalentEffect[1] { (TalentEffect)value });
			bool ok = true;
			foreach (TalentEffect effect in effects)
			{
				if (effect.Type == ETalentEffectType.AddSkill || effect.Type == ETalentEffectType.UpgradeSkill)
				{
					if (!CheckSkillRef(data, sourceField, effect, effect.Value1, "Value1"))
					{
						ok = false;
					}
				}
				else if (effect.Type == ETalentEffectType.UnlockSkillMode || effect.Type == ETalentEffectType.ChangeSkill)
				{
					bool first = CheckSkillRef(data, sourceField, effect, effect.Value1, "Value1");
					bool second = CheckSkillRef(data, sourceField, effect, effect.Value2, "Value2");
					ok = ok && first && second;
				}
			}
			return ok;
		}

		// CheckSkillRef<int> @0x180ee10a0 (machine code checked)
		private static bool CheckSkillRef<TKey>(ITableData<TKey> data, string sourceField, TalentEffect effect, int skillId, string valueField) where TKey : IComparable
		{
			if (skillId <= 0)
			{
				return true;
			}
			if (TSkill.Table.ContainsKey(skillId))
			{
				return true;
			}
			data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}={4}\"", "TSkill", sourceField, effect.Type, valueField, skillId));
			return false;
		}

		// ValidateRefInterior<int> @0x180eea010 (machine code checked): Area (1), NpcLevel (2) and AreaDecoration (3) ids must exist in TArea, TNpc and TAreaDecoration
		public static bool ValidateRefInterior<TKey>(this ITableData<TKey> data, string sourceField) where TKey : IComparable
		{
			object value = data.GetType().GetProperty(sourceField).GetValue(data);
			if (value == null)
			{
				return true;
			}
			Interior[] interiors = (value.GetType().IsArray ? ((Interior[])value) : new Interior[1] { (Interior)value });
			bool ok = true;
			foreach (Interior interior in interiors)
			{
				string tableName;
				switch (interior.Type)
				{
				case EInteriorType.Area:
					if (TArea.Table.ContainsKey(interior.Id))
					{
						continue;
					}
					tableName = "TArea";
					break;
				case EInteriorType.NpcLevel:
					if (TNpc.Table.ContainsKey(interior.Id))
					{
						continue;
					}
					tableName = "TNpc";
					break;
				case EInteriorType.AreaDecoration:
					if (TAreaDecoration.Table.ContainsKey(interior.Id))
					{
						continue;
					}
					tableName = "TAreaDecoration";
					break;
				default:
					continue;
				}
				data.LogError(string.Format("{0}.Table not contains \"[{1}] {2}\"", tableName, sourceField, interior));
				ok = false;
			}
			return ok;
		}
	}
}

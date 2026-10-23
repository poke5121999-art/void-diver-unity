using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TItem : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ItemTable Table { get; set; } = new ItemTable();

		public int Pk
		{
			get
			{
				return Id;
			}
			set
			{
			}
		}

		public int Id { get; set; }

		public int BitFlagIndex { get; set; }

		public EItemCategory Category { get; set; }

		public EGoodsType GoodsType { get; set; }

		public EGoodsGradeType Grade { get; set; }

		public EConsumableType ConsumableType { get; set; }

		public EKeyItemType[] KeyTypes { get; set; }

		public ENoteType NoteType { get; set; }

		public float Cooltime { get; set; }

		public int SellPrice { get; set; }

		public long DefaultCount { get; set; }

		public int InventoryCountMax { get; set; }

		public int SkillId { get; set; }

		public int[] AddAfterConsumeIds { get; set; }

		public bool CanUseFromQuickSlot { get; set; }

		public bool CanUseFromInventory { get; set; }

		public int Worth { get; set; }

		public bool IsVisibleInArchive { get; set; }

		public EBagType BagType { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		public string Note => LocalizationUtility.GetLocalizedText(NoteKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TItem", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TItem", "Desc", Id);

		private string NoteKey => string.Format("{0}_{1}_{2}", "TItem", "Note", Id);

		public bool IsEventItem => Category == EItemCategory.Quest || Category == EItemCategory.Mission;

		public static event Action OnTableChanged
		{
			[CompilerGenerated]
			add
			{
				m_OnTableChanged += value;
			}
			[CompilerGenerated]
			remove
			{
				m_OnTableChanged -= value;
			}
		}

		public void Parse(string[] columns)
		{
			Id = TableParse.Int(columns[0]);
			BitFlagIndex = TableParse.Int(columns[1]);
			Category = TableParse.Enum<EItemCategory>(columns[2]);
			GoodsType = TableParse.Enum<EGoodsType>(columns[3]);
			Grade = TableParse.Enum<EGoodsGradeType>(columns[4]);
			ConsumableType = TableParse.Enum<EConsumableType>(columns[5]);
			KeyTypes = TableParse.Array<EKeyItemType>(columns[6]);
			NoteType = TableParse.Enum<ENoteType>(columns[7]);
			Cooltime = TableParse.Float(columns[8]);
			SellPrice = TableParse.Int(columns[9]);
			DefaultCount = TableParse.Long(columns[10]);
			InventoryCountMax = TableParse.Int(columns[11]);
			SkillId = TableParse.Int(columns[12]);
			AddAfterConsumeIds = TableParse.Array<int>(columns[13]);
			CanUseFromQuickSlot = TableParse.Bool(columns[14]);
			CanUseFromInventory = TableParse.Bool(columns[15]);
			Worth = TableParse.Int(columns[16]);
			IsVisibleInArchive = TableParse.Bool(columns[17]);
			BagType = TableParse.Enum<EBagType>(columns[18]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, Category, GoodsType, Grade, ConsumableType, KeyTypes, NoteType, Cooltime, SellPrice, DefaultCount, InventoryCountMax, SkillId, AddAfterConsumeIds, CanUseFromQuickSlot, CanUseFromInventory, Worth, IsVisibleInArchive, BagType);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,Category,GoodsType,Grade,ConsumableType,KeyTypes,NoteType,Cooltime,SellPrice,DefaultCount,InventoryCountMax,SkillId,AddAfterConsumeIds,CanUseFromQuickSlot,CanUseFromInventory,Worth,IsVisibleInArchive,BagType";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EItemCategory,EGoodsType,EGoodsGradeType,EConsumableType,EKeyItemType[],ENoteType,float,int,long,int,int,int[],bool,bool,int,bool,EBagType";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ItemTable();
			table.Load(new CsvTableLoader<int, TItem>(text));
			Table = table;
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
		}

		public static bool ValidateTable()
		{
			bool result = true;
			if (Table != null)
			{
				foreach (TItem row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		private string GetDesc()
		{
			if (TLocalizedText.Table.TryGetValue(DescFormatKey, out var data))
			{
				return data.FormatWithExpression();
			}
			return DescFormatKey;
		}

		public bool Validate()
		{
			bool result = this.Check(InventoryCountMax > 0, string.Format("{0}: {1} must be > 0", "InventoryCountMax", InventoryCountMax))
				&& this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescFormatKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("SkillId", typeof(TSkill))
				&& this.ValidateRefKey("AddAfterConsumeIds", typeof(TItem));
			if (GoodsType == EGoodsType.Blueprint && result)
			{
				result = ValidateBlueprint();
			}
			if (GoodsType == EGoodsType.MusicDisc && result)
			{
				result = ValidateMusicDisc();
			}
			if (GoodsType == EGoodsType.Note)
			{
				if (!result)
				{
					return false;
				}
				result = this.ValidateRefKey("NoteKey", typeof(TLocalizedText));
			}
			return result;
		}

		private bool ValidateBlueprint()
		{
			TCrafting crafting = System.Linq.Enumerable.FirstOrDefault(
				System.Linq.Enumerable.Where(TCrafting.Table.Values, (TCrafting recipe) => recipe.UnlockConditions != null && recipe.UnlockConditions.Length > 0),
				(TCrafting recipe) => System.Linq.Enumerable.Any(recipe.UnlockConditions, (GameCommon.Data.EventCondition condition) => condition.Type == GameCommon.Enum.EEventConditionType.ItemUsed && condition.Value1 == Id));
			return this.Check(crafting != null, string.Format("Blueprint item (Id: {0}) must be referenced by at least one crafting recipe's.", Id));
		}

		private bool ValidateMusicDisc()
		{
			TLoungeBgm bgm = System.Linq.Enumerable.FirstOrDefault(
				System.Linq.Enumerable.Where(TLoungeBgm.Table.Values, (TLoungeBgm entry) => entry.UnlockConditions != null && entry.UnlockConditions.Length > 0),
				(TLoungeBgm entry) => System.Linq.Enumerable.Any(entry.UnlockConditions, (GameCommon.Data.EventCondition condition) => condition.Type == GameCommon.Enum.EEventConditionType.ItemUsed && condition.Value1 == Id));
			return this.Check(bgm != null, string.Format("MusicDisc item (Id: {0}) must be referenced by at least one lounge BGM's unlock condition.", Id));
		}
	}
}

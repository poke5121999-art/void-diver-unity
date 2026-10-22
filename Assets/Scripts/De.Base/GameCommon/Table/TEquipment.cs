using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEquipment : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EquipmentTable Table { get; set; }

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

		public int EquipPartGroup { get; set; }

		public EGoodsType GoodsType { get; set; }

		public EGoodsGradeType Grade { get; set; }

		public string WeaponSkinName { get; set; }

		public EWeaponType WeaponType { get; set; }

		public EElementalType ElementalType { get; set; }

		public Stat[] Stats { get; set; }

		public Stat[] BrokenStats { get; set; }

		public int MaxDurability { get; set; }

		public int RepairCostPerDurability { get; set; }

		public float RepairMaxDurabilityLoss { get; set; }

		public int EquipmentSetGroupId { get; set; }

		public int[] EquipmentEffectIds { get; set; }

		public bool IsDefault { get; set; }

		public int SellPrice { get; set; }

		public EArtifactCategory ArtifactCategory { get; set; }

		public int[] SpecifiedArtifactPrefixIds { get; set; }

		public int Worth { get; set; }

		public int Corruption { get; set; }

		public EBagType BagType { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string DescFormatKey => null;

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
			EquipPartGroup = TableParse.Int(columns[2]);
			GoodsType = TableParse.Enum<EGoodsType>(columns[3]);
			Grade = TableParse.Enum<EGoodsGradeType>(columns[4]);
			WeaponSkinName = TableParse.Str(columns[5]);
			WeaponType = TableParse.Enum<EWeaponType>(columns[6]);
			ElementalType = TableParse.Enum<EElementalType>(columns[7]);
			Stats = TableParse.Json<Stat[]>(columns[8]);
			BrokenStats = TableParse.Json<Stat[]>(columns[9]);
			MaxDurability = TableParse.Int(columns[10]);
			RepairCostPerDurability = TableParse.Int(columns[11]);
			RepairMaxDurabilityLoss = TableParse.Float(columns[12]);
			EquipmentSetGroupId = TableParse.Int(columns[13]);
			EquipmentEffectIds = TableParse.Array<int>(columns[14]);
			IsDefault = TableParse.Bool(columns[15]);
			SellPrice = TableParse.Int(columns[16]);
			ArtifactCategory = TableParse.Enum<EArtifactCategory>(columns[17]);
			SpecifiedArtifactPrefixIds = TableParse.Array<int>(columns[18]);
			Worth = TableParse.Int(columns[19]);
			Corruption = TableParse.Int(columns[20]);
			BagType = TableParse.Enum<EBagType>(columns[21]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, EquipPartGroup, GoodsType, Grade, WeaponSkinName, WeaponType, ElementalType, Stats, BrokenStats, MaxDurability, RepairCostPerDurability, RepairMaxDurabilityLoss, EquipmentSetGroupId, EquipmentEffectIds, IsDefault, SellPrice, ArtifactCategory, SpecifiedArtifactPrefixIds, Worth, Corruption, BagType);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,EquipPartGroup,GoodsType,Grade,WeaponSkinName,WeaponType,ElementalType,Stats,BrokenStats,MaxDurability,RepairCostPerDurability,RepairMaxDurabilityLoss,EquipmentSetGroupId,EquipmentEffectIds,IsDefault,SellPrice,ArtifactCategory,SpecifiedArtifactPrefixIds,Worth,Corruption,BagType";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,EGoodsType,EGoodsGradeType,string,EWeaponType,EElementalType,Stat[],Stat[],int,int,float,int,int[],bool,int,EArtifactCategory,int[],int,int,EBagType";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EquipmentTable();
			table.Load(new CsvTableLoader<int, TEquipment>(text));
			Table = table;
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
		}

		public static bool ValidateTable()
		{
			return false;
		}

		private string GetDesc()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}

		public bool CheckHasInvalidPrefixIds()
		{
			return false;
		}

		public bool CanPickZeroPrefixes()
		{
			return false;
		}

		public float GetStatValue(EStatType statType)
		{
			return 0f;
		}

		public float GetBrokenStatValue(EStatType statType)
		{
			return 0f;
		}
	}
}

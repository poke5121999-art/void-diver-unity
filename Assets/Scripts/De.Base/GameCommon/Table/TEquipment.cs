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
				return 0;
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
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
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

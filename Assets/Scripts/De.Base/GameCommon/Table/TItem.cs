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

		public static ItemTable Table { get; set; }

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
		public string Name => null;

		[Localized]
		public string Desc => null;

		public string Note => null;

		private string NameKey => null;

		private string DescFormatKey => null;

		private string NoteKey => null;

		public bool IsEventItem => false;

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

		private bool ValidateBlueprint()
		{
			return false;
		}

		private bool ValidateMusicDisc()
		{
			return false;
		}
	}
}

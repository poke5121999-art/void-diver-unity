using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCrafting : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		private static Dictionary<int, GoodsData> _conditionItemIdToResultGoodsData;

		public static CraftingTable Table { get; set; }

		public static GroupCraftingTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public GoodsData ResultGoodsData { get; set; }

		public GoodsData[] MaterialGoodsDatas { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public long NeedGold => 0L;

		public long NeedCoin => 0L;

		public long NeedRealCoin => 0L;

		public GoodsData[] MaterialItemDatas => null;

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

		public static GoodsData GetResultGoodsDataByUsedItem(int itemId)
		{
			return default;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

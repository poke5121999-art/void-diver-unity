using System;
using System.Linq;
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
				return Id;
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
			GroupId = TableParse.Int(columns[1]);
			ResultGoodsData = TableParse.Json<GoodsData>(columns[2]);
			MaterialGoodsDatas = TableParse.Json<GoodsData[]>(columns[3]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, ResultGoodsData, MaterialGoodsDatas, UnlockConditions);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,ResultGoodsData,MaterialGoodsDatas,UnlockConditions";
		}

		public static string GetTypesCsv()
		{
			return "int,int,GoodsData,GoodsData[],EventCondition[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CraftingTable();
			table.Load(new CsvTableLoader<int, TCrafting>(text));
			Table = table;
			GroupTable = new GroupCraftingTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

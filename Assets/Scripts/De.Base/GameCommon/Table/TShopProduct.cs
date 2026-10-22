using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TShopProduct : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ShopProductTable Table { get; set; }

		public static GroupShopProductTable GroupTable { get; set; }

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

		public GoodsData GainGoodsData { get; set; }

		public GoodsData ConsumeGoodsData { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public bool IsEnabled { get; set; }

		public int AppearProbability { get; set; }

		public int StockCount { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public bool HasNoPurchaseLimit => false;

		public bool HasPurchaseLimit => false;

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
			GainGoodsData = TableParse.Json<GoodsData>(columns[2]);
			ConsumeGoodsData = TableParse.Json<GoodsData>(columns[3]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[4]);
			IsEnabled = TableParse.Bool(columns[5]);
			AppearProbability = TableParse.Int(columns[6]);
			StockCount = TableParse.Int(columns[7]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, GainGoodsData, ConsumeGoodsData, UnlockConditions, IsEnabled, AppearProbability, StockCount);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,GainGoodsData,ConsumeGoodsData,UnlockConditions,IsEnabled,AppearProbability,StockCount";
		}

		public static string GetTypesCsv()
		{
			return "int,int,GoodsData,GoodsData,EventCondition[],bool,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ShopProductTable();
			table.Load(new CsvTableLoader<int, TShopProduct>(text));
			Table = table;
			GroupTable = new GroupShopProductTable();
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

		public bool Validate()
		{
			return false;
		}
	}
}

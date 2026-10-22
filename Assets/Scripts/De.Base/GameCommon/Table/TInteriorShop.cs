using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TInteriorShop : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static InteriorShopTable Table { get; set; }

		public static GroupInteriorShopTable GroupTable { get; set; }

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

		public int AreaId { get; set; }

		public Interior Interior { get; set; }

		public int ConsumeCoin { get; set; }

		public int ConsumeGold { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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
			AreaId = TableParse.Int(columns[1]);
			Interior = TableParse.Json<Interior>(columns[2]);
			ConsumeCoin = TableParse.Int(columns[3]);
			ConsumeGold = TableParse.Int(columns[4]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, AreaId, Interior, ConsumeCoin, ConsumeGold, UnlockConditions);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,AreaId,Interior,ConsumeCoin,ConsumeGold,UnlockConditions";
		}

		public static string GetTypesCsv()
		{
			return "int,int,Interior,int,int,EventCondition[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new InteriorShopTable();
			table.Load(new CsvTableLoader<int, TInteriorShop>(text));
			Table = table;
			GroupTable = new GroupInteriorShopTable();
			GroupTable.Init(table.Values.GroupBy(row => row.AreaId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TScrap : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ScrapTable Table { get; set; }

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

		public GoodsData InsertGoodsData { get; set; }

		public GoodsData[] ResultGoodsData { get; set; }

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
			InsertGoodsData = TableParse.Json<GoodsData>(columns[1]);
			ResultGoodsData = TableParse.Json<GoodsData[]>(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, InsertGoodsData, ResultGoodsData);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,InsertGoodsData,ResultGoodsData";
		}

		public static string GetTypesCsv()
		{
			return "int,GoodsData,GoodsData[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ScrapTable();
			table.Load(new CsvTableLoader<int, TScrap>(text));
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

		public bool Validate()
		{
			return false;
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TLevel : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static LevelTable Table { get; set; }

		public int Pk
		{
			get
			{
				return Level;
			}
			set
			{
			}
		}

		public int Level { get; set; }

		public long NeedExp { get; set; }

		public GoodsData[] ConsumeGoodsData { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public long NeedGold => 0L;

		public long NeedCoin => 0L;

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
			Level = TableParse.Int(columns[0]);
			NeedExp = TableParse.Long(columns[1]);
			ConsumeGoodsData = TableParse.Json<GoodsData[]>(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Level, NeedExp, ConsumeGoodsData);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Level,NeedExp,ConsumeGoodsData";
		}

		public static string GetTypesCsv()
		{
			return "int,long,GoodsData[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new LevelTable();
			table.Load(new CsvTableLoader<int, TLevel>(text));
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

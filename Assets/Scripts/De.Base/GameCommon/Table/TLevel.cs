using GameCommon.Enum;
using System.Linq;
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

		public static LevelTable Table { get; set; } = new LevelTable();

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

		public long NeedGold => ConsumeGoodsData == null ? 0 : ConsumeGoodsData.Where(g => g.Type == EGoodsType.Gold).Sum(g => g.Count);

		public long NeedCoin => ConsumeGoodsData == null ? 0 : ConsumeGoodsData.Where(g => g.Type == EGoodsType.Coin).Sum(g => g.Count);

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
			bool result = true;
			if (Table != null)
			{
				foreach (TLevel row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public bool Validate()
		{
			return this.Check(Level > 0, string.Format("{0}: {1} must be >= 1", "Level", Level))
				&& this.ValidateRefGoodsData("ConsumeGoodsData");
		}
	}
}

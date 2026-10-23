using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TParadoxLevel : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ParadoxLevelTable Table { get; set; } = new ParadoxLevelTable();

		public static SubParadoxLevelTable SubTable { get; set; }

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

		public int Level { get; set; }

		public bool IsPositive { get; set; }

		public int[] NextLevelProbability { get; set; }

		public GoodsData RemovalGoodsData { get; set; }

		public GoodsData LockGoodsData { get; set; }

		public GoodsData UnLockGoodsData { get; set; }

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
			Level = TableParse.Int(columns[1]);
			IsPositive = TableParse.Bool(columns[2]);
			NextLevelProbability = TableParse.Array<int>(columns[3]);
			RemovalGoodsData = TableParse.Json<GoodsData>(columns[4]);
			LockGoodsData = TableParse.Json<GoodsData>(columns[5]);
			UnLockGoodsData = TableParse.Json<GoodsData>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Level, IsPositive, NextLevelProbability, RemovalGoodsData, LockGoodsData, UnLockGoodsData);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Level,IsPositive,NextLevelProbability,RemovalGoodsData,LockGoodsData,UnLockGoodsData";
		}

		public static string GetTypesCsv()
		{
			return "int,int,bool,int[],GoodsData,GoodsData,GoodsData";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ParadoxLevelTable();
			table.Load(new CsvTableLoader<int, TParadoxLevel>(text));
			Table = table;
			SubTable = new SubParadoxLevelTable();
			SubTable.Init(table.Values.Select(row => (row.Level, row.IsPositive, row)));
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
				foreach (TParadoxLevel row in Table.Values)
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
			return this.Check(Level >= 0, string.Format("{0}: {1} must be >= 0", "Level", Level))
				&& this.Check(RemovalGoodsData.Count >= 0, string.Format("{0}: {1} must be >= 0", "RemovalGoodsData", RemovalGoodsData))
				&& this.Check(LockGoodsData.Count >= 0, string.Format("{0}: {1} must be >= 0", "LockGoodsData", LockGoodsData))
				&& this.Check(UnLockGoodsData.Count >= 0, string.Format("{0}: {1} must be >= 0", "UnLockGoodsData", UnLockGoodsData))
				&& this.ValidateRefGoodsData("RemovalGoodsData")
				&& this.ValidateRefGoodsData("LockGoodsData")
				&& this.ValidateRefGoodsData("UnLockGoodsData");
		}
	}
}

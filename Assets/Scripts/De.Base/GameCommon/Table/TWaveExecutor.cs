using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TWaveExecutor : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static WaveExecutorTable Table { get; set; }

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

		public int WaveId { get; set; }

		public string PrefabName { get; set; }

		public GoodsData ClearReward { get; set; }

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
			WaveId = TableParse.Int(columns[1]);
			PrefabName = TableParse.Str(columns[2]);
			ClearReward = TableParse.Json<GoodsData>(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, WaveId, PrefabName, ClearReward);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,WaveId,PrefabName,ClearReward";
		}

		public static string GetTypesCsv()
		{
			return "int,int,string,GoodsData";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new WaveExecutorTable();
			table.Load(new CsvTableLoader<int, TWaveExecutor>(text));
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

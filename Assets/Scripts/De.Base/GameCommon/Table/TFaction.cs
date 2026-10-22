using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TFaction : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static FactionTable Table { get; set; }

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

		public int[] AllyFactionIds { get; set; }

		public int[] EnemyFactionIds { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string EnemyFactionMonologue => null;

		private string NameKey => null;

		public string EnemyFactionMonologueKey => null;

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
			AllyFactionIds = TableParse.Array<int>(columns[1]);
			EnemyFactionIds = TableParse.Array<int>(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, AllyFactionIds, EnemyFactionIds);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,AllyFactionIds,EnemyFactionIds";
		}

		public static string GetTypesCsv()
		{
			return "int,int[],int[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new FactionTable();
			table.Load(new CsvTableLoader<int, TFaction>(text));
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

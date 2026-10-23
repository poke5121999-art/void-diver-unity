using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEmployeeLevel : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EmployeeLevelTable Table { get; set; } = new EmployeeLevelTable();

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
			Level = TableParse.Int(columns[0]);
			NeedExp = TableParse.Long(columns[1]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Level, NeedExp);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Level,NeedExp";
		}

		public static string GetTypesCsv()
		{
			return "int,long";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EmployeeLevelTable();
			table.Load(new CsvTableLoader<int, TEmployeeLevel>(text));
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
			foreach (TEmployeeLevel row in Table.Values)
			{
				if (!row.Validate())
				{
					result = false;
				}
			}
			return result;
		}

		public bool Validate()
		{
			return true;
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TShield : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ShieldTable Table { get; set; } = new ShieldTable();

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

		public EShieldMaxValueType MaxValueType { get; set; }

		public float MaxValue { get; set; }

		public float Duration { get; set; }

		public bool IsStackable { get; set; }

		public bool IsDecrease { get; set; }

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
			MaxValueType = TableParse.Enum<EShieldMaxValueType>(columns[1]);
			MaxValue = TableParse.Float(columns[2]);
			Duration = TableParse.Float(columns[3]);
			IsStackable = TableParse.Bool(columns[4]);
			IsDecrease = TableParse.Bool(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, MaxValueType, MaxValue, Duration, IsStackable, IsDecrease);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,MaxValueType,MaxValue,Duration,IsStackable,IsDecrease";
		}

		public static string GetTypesCsv()
		{
			return "int,EShieldMaxValueType,float,float,bool,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ShieldTable();
			table.Load(new CsvTableLoader<int, TShield>(text));
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
			foreach (TShield row in Table.Values)
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

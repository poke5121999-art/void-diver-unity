using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TStress : ITableData<EStressState>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static StressTable Table { get; set; }

		public EStressState Pk
		{
			get
			{
				return State;
			}
			set
			{
			}
		}

		public EStressState State { get; set; }

		public int StressMax { get; set; }

		public int[] DebuffIds { get; set; }

		public int IntervalDebuffId { get; set; }

		public float DebuffInterval { get; set; }

		public float IntervalDebuffStartDelay { get; set; }

		public Action<ITableData<EStressState>> OnDataLoaded { get; set; }

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
			State = TableParse.Enum<EStressState>(columns[0]);
			StressMax = TableParse.Int(columns[1]);
			DebuffIds = TableParse.Array<int>(columns[2]);
			IntervalDebuffId = TableParse.Int(columns[3]);
			DebuffInterval = TableParse.Float(columns[4]);
			IntervalDebuffStartDelay = TableParse.Float(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(State, StressMax, DebuffIds, IntervalDebuffId, DebuffInterval, IntervalDebuffStartDelay);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "State,StressMax,DebuffIds,IntervalDebuffId,DebuffInterval,IntervalDebuffStartDelay";
		}

		public static string GetTypesCsv()
		{
			return "EStressState,int,int[],int,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new StressTable();
			table.Load(new CsvTableLoader<EStressState, TStress>(text));
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

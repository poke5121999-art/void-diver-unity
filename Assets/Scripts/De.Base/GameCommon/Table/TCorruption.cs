using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCorruption : ITableData<ECorruptionState>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CorruptionTable Table { get; set; } = new CorruptionTable();

		public ECorruptionState Pk
		{
			get
			{
				return State;
			}
			set
			{
			}
		}

		public ECorruptionState State { get; set; }

		public int Threshold { get; set; }

		public int[] DebuffIds { get; set; }

		public float StressDamageIntervalTime { get; set; }

		public float StressDamageAmount { get; set; }

		public Action<ITableData<ECorruptionState>> OnDataLoaded { get; set; }

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
			State = TableParse.Enum<ECorruptionState>(columns[0]);
			Threshold = TableParse.Int(columns[1]);
			DebuffIds = TableParse.Array<int>(columns[2]);
			StressDamageIntervalTime = TableParse.Float(columns[3]);
			StressDamageAmount = TableParse.Float(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(State, Threshold, DebuffIds, StressDamageIntervalTime, StressDamageAmount);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "State,Threshold,DebuffIds,StressDamageIntervalTime,StressDamageAmount";
		}

		public static string GetTypesCsv()
		{
			return "ECorruptionState,int,int[],float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CorruptionTable();
			table.Load(new CsvTableLoader<ECorruptionState, TCorruption>(text));
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
				foreach (TCorruption row in Table.Values)
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
			return this.ValidateRefKey("DebuffIds", typeof(TBuff));
		}
	}
}

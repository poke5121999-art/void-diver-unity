using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TAccountRecord : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static AccountRecordTable Table { get; set; } = new AccountRecordTable();

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

		public EventCondition EventCondition { get; set; }

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
			EventCondition = TableParse.Json<EventCondition>(columns[1]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, EventCondition);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,EventCondition";
		}

		public static string GetTypesCsv()
		{
			return "int,EventCondition";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new AccountRecordTable();
			table.Load(new CsvTableLoader<int, TAccountRecord>(text));
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
				foreach (TAccountRecord row in Table.Values)
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
			if (!this.ValidateRefEventCondition("EventCondition"))
			{
				return false;
			}
			EEventConditionValueType valueType = EventCondition.Type.GetValueType();
			return this.Check(valueType == EEventConditionValueType.Increment, string.Format("{0}.Type: {1} must be Increment type", "EventCondition", EventCondition.Type));
		}
	}
}

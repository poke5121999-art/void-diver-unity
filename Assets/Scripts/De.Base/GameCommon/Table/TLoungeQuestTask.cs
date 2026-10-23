using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TLoungeQuestTask : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static LoungeQuestTaskTable Table { get; set; }

		public static GroupLoungeQuestTaskTable GroupTable { get; set; }

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

		public int LoungeQuestId { get; set; }

		public EventCondition EventCondition { get; set; }

		public long Goal { get; set; }

		public bool DisplayInHud { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string DescKey => string.Format("{0}_{1}_{2}", "TLoungeQuestTask", "Desc", Id);

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
			LoungeQuestId = TableParse.Int(columns[1]);
			EventCondition = TableParse.Json<EventCondition>(columns[2]);
			Goal = TableParse.Long(columns[3]);
			DisplayInHud = TableParse.Bool(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, LoungeQuestId, EventCondition, Goal, DisplayInHud);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,LoungeQuestId,EventCondition,Goal,DisplayInHud";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EventCondition,long,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new LoungeQuestTaskTable();
			table.Load(new CsvTableLoader<int, TLoungeQuestTask>(text));
			Table = table;
			GroupTable = new GroupLoungeQuestTaskTable();
			GroupTable.Init(table.Values.GroupBy(row => row.LoungeQuestId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

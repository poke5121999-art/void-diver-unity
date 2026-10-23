using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCampaignTask : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CampaignTaskTable Table { get; set; }

		public static GroupCampaignTaskTable GroupTable { get; set; }

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

		public int CampaignId { get; set; }

		public EventCondition EventCondition { get; set; }

		public long Goal { get; set; }

		public int[] QuestRewardBoxIds { get; set; }

		public int[] QuestItemIds { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string DescKey => string.Format("{0}_{1}_{2}", "TCampaignTask", "Desc", Id);

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
			CampaignId = TableParse.Int(columns[1]);
			EventCondition = TableParse.Json<EventCondition>(columns[2]);
			Goal = TableParse.Long(columns[3]);
			QuestRewardBoxIds = TableParse.Array<int>(columns[4]);
			QuestItemIds = TableParse.Array<int>(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, CampaignId, EventCondition, Goal, QuestRewardBoxIds, QuestItemIds);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,CampaignId,EventCondition,Goal,QuestRewardBoxIds,QuestItemIds";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EventCondition,long,int[],int[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CampaignTaskTable();
			table.Load(new CsvTableLoader<int, TCampaignTask>(text));
			Table = table;
			GroupTable = new GroupCampaignTaskTable();
			GroupTable.Init(table.Values.GroupBy(row => row.CampaignId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

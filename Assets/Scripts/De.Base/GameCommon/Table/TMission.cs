using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMission : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MissionTable Table { get; set; }

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

		public int BitFlagIndex { get; set; }

		public EThemeType[] ThemeTypes { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public int[] ZoneSpawnIds { get; set; }

		public EventCondition EventCondition { get; set; }

		public long Goal { get; set; }

		public GoodsData[] Rewards { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TMission", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TMission", "Desc", Id);

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
			BitFlagIndex = TableParse.Int(columns[1]);
			ThemeTypes = TableParse.Array<EThemeType>(columns[2]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[3]);
			ZoneSpawnIds = TableParse.Array<int>(columns[4]);
			EventCondition = TableParse.Json<EventCondition>(columns[5]);
			Goal = TableParse.Long(columns[6]);
			Rewards = TableParse.Json<GoodsData[]>(columns[7]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, ThemeTypes, UnlockConditions, ZoneSpawnIds, EventCondition, Goal, Rewards);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,ThemeTypes,UnlockConditions,ZoneSpawnIds,EventCondition,Goal,Rewards";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EThemeType[],EventCondition[],int[],EventCondition,long,GoodsData[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MissionTable();
			table.Load(new CsvTableLoader<int, TMission>(text));
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

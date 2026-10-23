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

		public static MissionTable Table { get; set; } = new MissionTable();

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
			bool result = true;
			if (Table != null)
			{
				foreach (TMission row in Table.Values)
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
			bool result = this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("ZoneSpawnIds", typeof(TZoneSpawn))
				&& this.ValidateRefEventCondition("UnlockConditions")
				&& this.ValidateRefEventCondition("EventCondition")
				&& this.ValidateRefGoodsData("Rewards");
			if (Id != System.Linq.Enumerable.Min(Table.Keys))
			{
				return result;
			}
			int required = TConst.MissionCount.IntValue - System.Linq.Enumerable.Count(Table.Values, (TMission mission) => mission.ThemeTypes == null || mission.ThemeTypes.Length == 0);
			foreach (EThemeType theme in System.Enum.GetValues(typeof(EThemeType)))
			{
				if (theme != EThemeType.None && theme != EThemeType.Lounge)
				{
					int count = System.Linq.Enumerable.Count(
						System.Linq.Enumerable.Where(Table.Values, (TMission mission) => mission.ThemeTypes != null),
						(TMission mission) => System.Linq.Enumerable.Any(mission.ThemeTypes, (EThemeType type) => type == theme));
					if (count < required)
					{
						result = false;
						this.LogError(string.Format("{0} theme mission is lacking {1} items.", theme, required - count));
					}
				}
			}
			return result;
		}
	}
}

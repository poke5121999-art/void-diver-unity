using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCampaign : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CampaignTable Table { get; set; } = new CampaignTable();

		public static GroupCampaignTable GroupTable { get; set; } = new GroupCampaignTable();

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

		public ECampaignCategory Category { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public bool IsBossCampaign { get; set; }

		public GoodsData[] FirstRewards { get; set; }

		public GoodsData[] RepeatRewards { get; set; }

		public int VariantDifficulty { get; set; }

		public long GainedExp { get; set; }

		public int RecommendedLevel { get; set; }

		public int Weight { get; set; }

		public EThemeType ThemeType { get; set; }

		public int[] MapSize { get; set; }

		public FixedSector[] FixedSectors { get; set; }

		public int[] RandomPlacedSectorIds { get; set; }

		public int[] ZoneSpawnIds { get; set; }

		public int BoundarySpecialFieldId { get; set; }

		public int PatrolMonsterId { get; set; }

		public int PatrolMonsterCount { get; set; }

		public bool IsFixedAnomaly { get; set; }

		public string[] Cutscenes { get; set; }

		public EAnomalyType AnomalyType { get; set; }

		public bool HasEscapeExpReward { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		[Localized]
		public string FailConditionDesc => LocalizationUtility.GetLocalizedText(FailConditionDescKey);

		[Localized]
		public string SectorName => LocalizationUtility.GetLocalizedText(SectorNameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TCampaign", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TCampaign", "Desc", Id);

		private string FailConditionDescKey => string.Format("{0}_{1}_{2}", "TCampaign", "FailConditionDesc", Id);

		private string SectorNameKey => string.Format("{0}_{1}_{2}", "TCampaign", "SectorName", Id);

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
			Category = TableParse.Enum<ECampaignCategory>(columns[2]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[3]);
			IsBossCampaign = TableParse.Bool(columns[4]);
			FirstRewards = TableParse.Json<GoodsData[]>(columns[5]);
			RepeatRewards = TableParse.Json<GoodsData[]>(columns[6]);
			VariantDifficulty = TableParse.Int(columns[7]);
			GainedExp = TableParse.Long(columns[8]);
			RecommendedLevel = TableParse.Int(columns[9]);
			Weight = TableParse.Int(columns[10]);
			ThemeType = TableParse.Enum<EThemeType>(columns[11]);
			MapSize = TableParse.Array<int>(columns[12]);
			FixedSectors = TableParse.Json<FixedSector[]>(columns[13]);
			RandomPlacedSectorIds = TableParse.Array<int>(columns[14]);
			ZoneSpawnIds = TableParse.Array<int>(columns[15]);
			BoundarySpecialFieldId = TableParse.Int(columns[16]);
			PatrolMonsterId = TableParse.Int(columns[17]);
			PatrolMonsterCount = TableParse.Int(columns[18]);
			IsFixedAnomaly = TableParse.Bool(columns[19]);
			Cutscenes = TableParse.Array<string>(columns[20]);
			AnomalyType = TableParse.Enum<EAnomalyType>(columns[21]);
			HasEscapeExpReward = TableParse.Bool(columns[22]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, Category, UnlockConditions, IsBossCampaign, FirstRewards, RepeatRewards, VariantDifficulty, GainedExp, RecommendedLevel, Weight, ThemeType, MapSize, FixedSectors, RandomPlacedSectorIds, ZoneSpawnIds, BoundarySpecialFieldId, PatrolMonsterId, PatrolMonsterCount, IsFixedAnomaly, Cutscenes, AnomalyType, HasEscapeExpReward);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,Category,UnlockConditions,IsBossCampaign,FirstRewards,RepeatRewards,VariantDifficulty,GainedExp,RecommendedLevel,Weight,ThemeType,MapSize,FixedSectors,RandomPlacedSectorIds,ZoneSpawnIds,BoundarySpecialFieldId,PatrolMonsterId,PatrolMonsterCount,IsFixedAnomaly,Cutscenes,AnomalyType,HasEscapeExpReward";
		}

		public static string GetTypesCsv()
		{
			return "int,int,ECampaignCategory,EventCondition[],bool,GoodsData[],GoodsData[],int,long,int,int,EThemeType,int[],FixedSector[],int[],int[],int,int,int,bool,string[],EAnomalyType,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CampaignTable();
			table.Load(new CsvTableLoader<int, TCampaign>(text));
			Table = table;
			GroupTable = new GroupCampaignTable();
			GroupTable.Init(table.Values.GroupBy(row => row.Category).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TCampaign row in Table.Values)
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
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("FailConditionDescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("SectorNameKey", typeof(TLocalizedText))
				&& this.ValidateSpawnableMonster("PatrolMonsterId")
				&& this.ValidateRefKey("RandomPlacedSectorIds", typeof(TSector))
				&& this.ValidateRefKey("ZoneSpawnIds", typeof(TZoneSpawn))
				&& this.ValidateRefKey("BoundarySpecialFieldId", typeof(TSpecialField))
				&& this.ValidateRefGoodsData("FirstRewards")
				&& this.ValidateRefGoodsData("RepeatRewards")
				&& this.ValidateRefEventCondition("UnlockConditions");
		}
	}
}

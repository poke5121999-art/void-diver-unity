using System;
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

		public static CampaignTable Table { get; set; }

		public static GroupCampaignTable GroupTable { get; set; }

		public int Pk
		{
			get
			{
				return 0;
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
		public string Name => null;

		[Localized]
		public string Desc => null;

		[Localized]
		public string FailConditionDesc => null;

		[Localized]
		public string SectorName => null;

		private string NameKey => null;

		private string DescKey => null;

		private string FailConditionDescKey => null;

		private string SectorNameKey => null;

		public static event Action OnTableChanged
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
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

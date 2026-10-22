using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSector : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SectorTable Table { get; set; }

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

		public string Memo { get; set; }

		public ESectorAreaType AreaType { get; set; }

		public EThemeType ThemeType { get; set; }

		public string Bgm { get; set; }

		public string CombatBgm { get; set; }

		public float StressIntervalPercent { get; set; }

		public ZonePointData[] ZonePointDatas { get; set; }

		public MonsterSpawnGroupData[] MonsterSpawnGroupDatas { get; set; }

		public DropRewardSpawnGroupData[] DropRewardSpawnGroupDatas { get; set; }

		public RewardBoxSpawnGroupData[] RewardBoxSpawnGroupDatas { get; set; }

		public TrapSpawnGroupData[] TrapSpawnGroupDatas { get; set; }

		public ExitSpawnGroupData[] ExitSpawnGroupDatas { get; set; }

		public SkillExecutorSpawnGroupData[] SkillExecutorSpawnGroupDatas { get; set; }

		public WaveExecutorSpawnGroupData[] WaveExecutorSpawnGroupDatas { get; set; }

		public PhoneBoothSpawnData[] PhoneBoothSpawnDatas { get; set; }

		public NpcSpawnData[] NpcSpawnDatas { get; set; }

		public GuestSpawnData[] GuestSpawnDatas { get; set; }

		public DisposableObjectSpawnGroupData[] DisposableObjectSpawnGroupDatas { get; set; }

		public BreakablePropSpawnGroupData[] BreakablePropSpawnGroupDatas { get; set; }

		public EntranceSpawnGroupData[] EntranceSpawnGroupDatas { get; set; }

		public CollisionTriggerSpawnData[] CollisionTriggerSpawnDatas { get; set; }

		public TextMarkerSpawnData[] TextMarkerSpawnDatas { get; set; }

		public PortalSpawnGroupData[] PortalSpawnGroupDatas { get; set; }

		public PortalExitData[] PortalExitDatas { get; set; }

		public SpecialFieldSpawnGroupData[] SpecialFieldSpawnGroupDatas { get; set; }

		public LockedAreaObjectSpawnData[] LockedAreaObjectSpawnDatas { get; set; }

		public AreaDecorationObjectSpawnData[] AreaDecorationObjectSpawnDatas { get; set; }

		public MimicSpawnGroupData[] MimicSpawnGroupDatas { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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

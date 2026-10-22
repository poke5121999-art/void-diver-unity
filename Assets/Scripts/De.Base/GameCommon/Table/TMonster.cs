using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMonster : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MonsterTable Table { get; set; }

		public static GroupMonsterTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public EMonsterCategory[] MonsterCategories { get; set; }

		public int FactionId { get; set; }

		public EMonsterType MonsterType { get; set; }

		public GoodsData CapturedDropGoodsData { get; set; }

		public int BasePhaseMonsterId { get; set; }

		public int Phase { get; set; }

		public float DeadDuration { get; set; }

		public float BossDetectionRange { get; set; }

		public int BgmPriority { get; set; }

		public string BossBgm { get; set; }

		public bool IgnoreReceiveAggroSpreadEvent { get; set; }

		public int Atk { get; set; }

		public int Def { get; set; }

		public float AtkSpeed { get; set; }

		public float MoveSpeed { get; set; }

		public float WalkAnimation1xSpeed { get; set; }

		public int[] OnCombatBuffIds { get; set; }

		public int[] OnCuriousBuffIds { get; set; }

		public int Hp { get; set; }

		public float HpRegen { get; set; }

		public float WeakpointGaugeHpPercent { get; set; }

		public int SuppressionGauge { get; set; }

		public float FireResistancePercent { get; set; }

		public float WaterResistancePercent { get; set; }

		public float WindResistancePercent { get; set; }

		public int[] ActiveSkillIds { get; set; }

		public int[] PassiveSkillIds { get; set; }

		public float GlobalSkillCoolTime { get; set; }

		public int RespawnTime { get; set; }

		public int DropRewardGroupId { get; set; }

		public float SightAngle { get; set; }

		public float BattleSightRangePercent { get; set; }

		public float SightRange { get; set; }

		public float SightBackRange { get; set; }

		public bool IgnoreObstaclesForDetection { get; set; }

		public bool TargetHighStressPlayer { get; set; }

		public float StartMoveToTargetRange { get; set; }

		public float EndMoveToTargetRange { get; set; }

		public float KiteStartRange { get; set; }

		public float KiteEndRange { get; set; }

		public float WanderRange { get; set; }

		public float FleeHpPercent { get; set; }

		public float FleeDuration { get; set; }

		public int FleeCooltime { get; set; }

		public float FleePercent { get; set; }

		public float PauseStatePercent { get; set; }

		public float PauseStateDuration { get; set; }

		public float AvoidRange { get; set; }

		public float AvoidStatePercent { get; set; }

		public float AvoidStateDuration { get; set; }

		public string DyingSfx { get; set; }

		public string SpawnSfx { get; set; }

		public string SpawnVfx { get; set; }

		public float SpawnVfxOffsetY { get; set; }

		public float SpawnDelay { get; set; }

		public string HitSfx { get; set; }

		public float HitSfxPercent { get; set; }

		public string AggroSfx { get; set; }

		public int RecoveryStressAmountOnDead { get; set; }

		public float RecoveryStressRangeOnDead { get; set; }

		public int SummonMonsterLimit { get; set; }

		public int[] LightBuffId { get; set; }

		public int[] LightStackBuffId { get; set; }

		public float BackAttackAngle { get; set; }

		public bool AlwaysShowHpBar { get; set; }

		public EOutlineType OutlineType { get; set; }

		public ESpeedTag SpeedTag { get; set; }

		public ESightTag SightTag { get; set; }

		public EThemeType[] ThemeTypes { get; set; }

		public float WeaknessRevealPercent { get; set; }

		public float DescriptionRevealPercent { get; set; }

		public byte DescriptionRevealKillCount { get; set; }

		public EMonsterFootstepSfxMode FootstepSfxMode { get; set; }

		public string RunLeftSfx { get; set; }

		public string RunRightSfx { get; set; }

		public string WalkLeftSfx { get; set; }

		public string WalkRightSfx { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public static Func<int, bool> PrefabExists { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		[Localized]
		public string BossRadioText => null;

		private string NameKey => null;

		private string DescKey => null;

		private string BossRadioTextKey => null;

		public bool IsBasePhaseMonster => false;

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

		private bool ValidateMonsterPrefab()
		{
			return false;
		}
	}
}

using System;
using System.Linq;
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

		public static MonsterTable Table { get; set; } = new MonsterTable();

		public static GroupMonsterTable GroupTable { get; set; } = new GroupMonsterTable();

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
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		[Localized]
		public string BossRadioText => LocalizationUtility.GetLocalizedText(BossRadioTextKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TMonster", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TMonster", "Desc", Id);

		private string BossRadioTextKey => string.Format("{0}_{1}_{2}", "TMonster", "BossRadioText", Id);

		public bool IsBasePhaseMonster => BasePhaseMonsterId < 1 || BasePhaseMonsterId == Id;

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
			GroupId = TableParse.Int(columns[1]);
			MonsterCategories = TableParse.Array<EMonsterCategory>(columns[2]);
			FactionId = TableParse.Int(columns[3]);
			MonsterType = TableParse.Enum<EMonsterType>(columns[4]);
			CapturedDropGoodsData = TableParse.Json<GoodsData>(columns[5]);
			BasePhaseMonsterId = TableParse.Int(columns[6]);
			Phase = TableParse.Int(columns[7]);
			DeadDuration = TableParse.Float(columns[8]);
			BossDetectionRange = TableParse.Float(columns[9]);
			BgmPriority = TableParse.Int(columns[10]);
			BossBgm = TableParse.Str(columns[11]);
			IgnoreReceiveAggroSpreadEvent = TableParse.Bool(columns[12]);
			Atk = TableParse.Int(columns[13]);
			Def = TableParse.Int(columns[14]);
			AtkSpeed = TableParse.Float(columns[15]);
			MoveSpeed = TableParse.Float(columns[16]);
			WalkAnimation1xSpeed = TableParse.Float(columns[17]);
			OnCombatBuffIds = TableParse.Array<int>(columns[18]);
			OnCuriousBuffIds = TableParse.Array<int>(columns[19]);
			Hp = TableParse.Int(columns[20]);
			HpRegen = TableParse.Float(columns[21]);
			WeakpointGaugeHpPercent = TableParse.Float(columns[22]);
			SuppressionGauge = TableParse.Int(columns[23]);
			FireResistancePercent = TableParse.Float(columns[24]);
			WaterResistancePercent = TableParse.Float(columns[25]);
			WindResistancePercent = TableParse.Float(columns[26]);
			ActiveSkillIds = TableParse.Array<int>(columns[27]);
			PassiveSkillIds = TableParse.Array<int>(columns[28]);
			GlobalSkillCoolTime = TableParse.Float(columns[29]);
			RespawnTime = TableParse.Int(columns[30]);
			DropRewardGroupId = TableParse.Int(columns[31]);
			SightAngle = TableParse.Float(columns[32]);
			BattleSightRangePercent = TableParse.Float(columns[33]);
			SightRange = TableParse.Float(columns[34]);
			SightBackRange = TableParse.Float(columns[35]);
			IgnoreObstaclesForDetection = TableParse.Bool(columns[36]);
			TargetHighStressPlayer = TableParse.Bool(columns[37]);
			StartMoveToTargetRange = TableParse.Float(columns[38]);
			EndMoveToTargetRange = TableParse.Float(columns[39]);
			KiteStartRange = TableParse.Float(columns[40]);
			KiteEndRange = TableParse.Float(columns[41]);
			WanderRange = TableParse.Float(columns[42]);
			FleeHpPercent = TableParse.Float(columns[43]);
			FleeDuration = TableParse.Float(columns[44]);
			FleeCooltime = TableParse.Int(columns[45]);
			FleePercent = TableParse.Float(columns[46]);
			PauseStatePercent = TableParse.Float(columns[47]);
			PauseStateDuration = TableParse.Float(columns[48]);
			AvoidRange = TableParse.Float(columns[49]);
			AvoidStatePercent = TableParse.Float(columns[50]);
			AvoidStateDuration = TableParse.Float(columns[51]);
			DyingSfx = TableParse.Str(columns[52]);
			SpawnSfx = TableParse.Str(columns[53]);
			SpawnVfx = TableParse.Str(columns[54]);
			SpawnVfxOffsetY = TableParse.Float(columns[55]);
			SpawnDelay = TableParse.Float(columns[56]);
			HitSfx = TableParse.Str(columns[57]);
			HitSfxPercent = TableParse.Float(columns[58]);
			AggroSfx = TableParse.Str(columns[59]);
			RecoveryStressAmountOnDead = TableParse.Int(columns[60]);
			RecoveryStressRangeOnDead = TableParse.Float(columns[61]);
			SummonMonsterLimit = TableParse.Int(columns[62]);
			LightBuffId = TableParse.Array<int>(columns[63]);
			LightStackBuffId = TableParse.Array<int>(columns[64]);
			BackAttackAngle = TableParse.Float(columns[65]);
			AlwaysShowHpBar = TableParse.Bool(columns[66]);
			OutlineType = TableParse.Enum<EOutlineType>(columns[67]);
			SpeedTag = TableParse.Enum<ESpeedTag>(columns[68]);
			SightTag = TableParse.Enum<ESightTag>(columns[69]);
			ThemeTypes = TableParse.Array<EThemeType>(columns[70]);
			WeaknessRevealPercent = TableParse.Float(columns[71]);
			DescriptionRevealPercent = TableParse.Float(columns[72]);
			DescriptionRevealKillCount = TableParse.Byte(columns[73]);
			FootstepSfxMode = TableParse.Enum<EMonsterFootstepSfxMode>(columns[74]);
			RunLeftSfx = TableParse.Str(columns[75]);
			RunRightSfx = TableParse.Str(columns[76]);
			WalkLeftSfx = TableParse.Str(columns[77]);
			WalkRightSfx = TableParse.Str(columns[78]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, MonsterCategories, FactionId, MonsterType, CapturedDropGoodsData, BasePhaseMonsterId, Phase, DeadDuration, BossDetectionRange, BgmPriority, BossBgm, IgnoreReceiveAggroSpreadEvent, Atk, Def, AtkSpeed, MoveSpeed, WalkAnimation1xSpeed, OnCombatBuffIds, OnCuriousBuffIds, Hp, HpRegen, WeakpointGaugeHpPercent, SuppressionGauge, FireResistancePercent, WaterResistancePercent, WindResistancePercent, ActiveSkillIds, PassiveSkillIds, GlobalSkillCoolTime, RespawnTime, DropRewardGroupId, SightAngle, BattleSightRangePercent, SightRange, SightBackRange, IgnoreObstaclesForDetection, TargetHighStressPlayer, StartMoveToTargetRange, EndMoveToTargetRange, KiteStartRange, KiteEndRange, WanderRange, FleeHpPercent, FleeDuration, FleeCooltime, FleePercent, PauseStatePercent, PauseStateDuration, AvoidRange, AvoidStatePercent, AvoidStateDuration, DyingSfx, SpawnSfx, SpawnVfx, SpawnVfxOffsetY, SpawnDelay, HitSfx, HitSfxPercent, AggroSfx, RecoveryStressAmountOnDead, RecoveryStressRangeOnDead, SummonMonsterLimit, LightBuffId, LightStackBuffId, BackAttackAngle, AlwaysShowHpBar, OutlineType, SpeedTag, SightTag, ThemeTypes, WeaknessRevealPercent, DescriptionRevealPercent, DescriptionRevealKillCount, FootstepSfxMode, RunLeftSfx, RunRightSfx, WalkLeftSfx, WalkRightSfx);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,MonsterCategories,FactionId,MonsterType,CapturedDropGoodsData,BasePhaseMonsterId,Phase,DeadDuration,BossDetectionRange,BgmPriority,BossBgm,IgnoreReceiveAggroSpreadEvent,Atk,Def,AtkSpeed,MoveSpeed,WalkAnimation1xSpeed,OnCombatBuffIds,OnCuriousBuffIds,Hp,HpRegen,WeakpointGaugeHpPercent,SuppressionGauge,FireResistancePercent,WaterResistancePercent,WindResistancePercent,ActiveSkillIds,PassiveSkillIds,GlobalSkillCoolTime,RespawnTime,DropRewardGroupId,SightAngle,BattleSightRangePercent,SightRange,SightBackRange,IgnoreObstaclesForDetection,TargetHighStressPlayer,StartMoveToTargetRange,EndMoveToTargetRange,KiteStartRange,KiteEndRange,WanderRange,FleeHpPercent,FleeDuration,FleeCooltime,FleePercent,PauseStatePercent,PauseStateDuration,AvoidRange,AvoidStatePercent,AvoidStateDuration,DyingSfx,SpawnSfx,SpawnVfx,SpawnVfxOffsetY,SpawnDelay,HitSfx,HitSfxPercent,AggroSfx,RecoveryStressAmountOnDead,RecoveryStressRangeOnDead,SummonMonsterLimit,LightBuffId,LightStackBuffId,BackAttackAngle,AlwaysShowHpBar,OutlineType,SpeedTag,SightTag,ThemeTypes,WeaknessRevealPercent,DescriptionRevealPercent,DescriptionRevealKillCount,FootstepSfxMode,RunLeftSfx,RunRightSfx,WalkLeftSfx,WalkRightSfx";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EMonsterCategory[],int,EMonsterType,GoodsData,int,int,float,float,int,string,bool,int,int,float,float,float,int[],int[],int,float,float,int,float,float,float,int[],int[],float,int,int,float,float,float,float,bool,bool,float,float,float,float,float,float,float,int,float,float,float,float,float,float,string,string,string,float,float,string,float,string,int,float,int,int[],int[],float,bool,EOutlineType,ESpeedTag,ESightTag,EThemeType[],float,float,byte,EMonsterFootstepSfxMode,string,string,string,string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MonsterTable();
			table.Load(new CsvTableLoader<int, TMonster>(text));
			Table = table;
			GroupTable = new GroupMonsterTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TMonster row in Table.Values)
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
			bool result = this.Check(Hp > 0, string.Format("{0}: {1} must be > 0", "Hp", Hp))
				&& this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("OnCombatBuffIds", typeof(TBuff))
				&& this.ValidateRefKey("OnCuriousBuffIds", typeof(TBuff))
				&& this.ValidateRefKey("LightBuffId", typeof(TBuff))
				&& this.ValidateRefKey("LightStackBuffId", typeof(TBuff))
				&& this.ValidateRefKey("ActiveSkillIds", typeof(TSkill))
				&& this.ValidateRefKey("PassiveSkillIds", typeof(TSkill))
				&& this.ValidateRefGroupKey("DropRewardGroupId", typeof(TDropReward))
				&& this.ValidateRefKey("FactionId", typeof(TFaction))
				&& this.ValidateRefGoodsData("CapturedDropGoodsData")
				&& this.ValidateRefSfx("DyingSfx")
				&& this.ValidateRefSfx("SpawnSfx")
				&& this.ValidateRefSfx("HitSfx")
				&& this.ValidateRefSfx("AggroSfx")
				&& this.ValidateRefSfx("RunLeftSfx")
				&& this.ValidateRefSfx("RunRightSfx")
				&& this.ValidateRefSfx("WalkLeftSfx")
				&& this.ValidateRefSfx("WalkRightSfx");
			if (result)
			{
				result = ValidateMonsterPrefab();
			}
			if (AvoidStatePercent > 0f)
			{
				result = result
					&& this.Check(AvoidRange > 0f, "AvoidRange must be > 0")
					&& this.Check(AvoidStateDuration > 0f, "AvoidStateDuration must be > 0");
			}
			if (!result)
			{
				return false;
			}
			int negativeResistanceCount = (WaterResistancePercent < 0f ? 1 : 0) + (WindResistancePercent < 0f ? 1 : 0) + (FireResistancePercent < 0f ? 1 : 0);
			if (!this.Check(negativeResistanceCount < 2, "More than one elemental resistance is negative."))
			{
				return false;
			}
			bool bossRangeValid = true;
			if (MonsterType.IsBoss())
			{
				bossRangeValid = BossDetectionRange > 0f;
			}
			return this.Check(bossRangeValid, string.Format("{0}({1}) must be > 0 for boss", "BossDetectionRange", BossDetectionRange));
		}

		private bool ValidateMonsterPrefab()
		{
			if (PrefabExists != null && (BasePhaseMonsterId < 1 || BasePhaseMonsterId == Id) && !PrefabExists(Id))
			{
				this.LogError(string.Format("Monster prefab missing or has no MonsterController. (Unit/Monster/{0}.prefab)", Id));
				return false;
			}
			return true;
		}
	}
}

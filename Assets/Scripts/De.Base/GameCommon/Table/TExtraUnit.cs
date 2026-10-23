using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TExtraUnit : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ExtraUnitTable Table { get; set; }

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

		public EExtraUnitType ExtraUnitType { get; set; }

		public EMonsterCategory[] MonsterCategories { get; set; }

		public int[] ActiveSkillIds { get; set; }

		public int[] PassiveSkillIds { get; set; }

		public int Atk { get; set; }

		public int Def { get; set; }

		public float AtkSpeed { get; set; }

		public float MoveSpeed { get; set; }

		public int Hp { get; set; }

		public float HpRegen { get; set; }

		public float FireResistancePercent { get; set; }

		public float WaterResistancePercent { get; set; }

		public float LightningResistancePercent { get; set; }

		public float AttackRange { get; set; }

		public int AttackSkillId { get; set; }

		public int ActionSkillId { get; set; }

		public int DropRewardGroupId { get; set; }

		public float DetectRange { get; set; }

		public float FleeHpPercent { get; set; }

		public float FleeDuration { get; set; }

		public float GlobalSkillCoolTime { get; set; }

		public float PauseStatePercent { get; set; }

		public float PauseStateDuration { get; set; }

		public float AvoidStatePercent { get; set; }

		public float AvoidStateDuration { get; set; }

		public string DyingSfx { get; set; }

		public int RespawnTime { get; set; }

		public float SightAngle { get; set; }

		public float SightRange { get; set; }

		public float SightBackRange { get; set; }

		public float WanderRangeMin { get; set; }

		public float WanderRangeMax { get; set; }

		public float BackAttackAngle { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TExtraUnit", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TExtraUnit", "Desc", Id);

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
			ExtraUnitType = TableParse.Enum<EExtraUnitType>(columns[1]);
			MonsterCategories = TableParse.Array<EMonsterCategory>(columns[2]);
			ActiveSkillIds = TableParse.Array<int>(columns[3]);
			PassiveSkillIds = TableParse.Array<int>(columns[4]);
			Atk = TableParse.Int(columns[5]);
			Def = TableParse.Int(columns[6]);
			AtkSpeed = TableParse.Float(columns[7]);
			MoveSpeed = TableParse.Float(columns[8]);
			Hp = TableParse.Int(columns[9]);
			HpRegen = TableParse.Float(columns[10]);
			FireResistancePercent = TableParse.Float(columns[11]);
			WaterResistancePercent = TableParse.Float(columns[12]);
			LightningResistancePercent = TableParse.Float(columns[13]);
			AttackRange = TableParse.Float(columns[14]);
			AttackSkillId = TableParse.Int(columns[15]);
			ActionSkillId = TableParse.Int(columns[16]);
			DropRewardGroupId = TableParse.Int(columns[17]);
			DetectRange = TableParse.Float(columns[18]);
			FleeHpPercent = TableParse.Float(columns[19]);
			FleeDuration = TableParse.Float(columns[20]);
			GlobalSkillCoolTime = TableParse.Float(columns[21]);
			PauseStatePercent = TableParse.Float(columns[22]);
			PauseStateDuration = TableParse.Float(columns[23]);
			AvoidStatePercent = TableParse.Float(columns[24]);
			AvoidStateDuration = TableParse.Float(columns[25]);
			DyingSfx = TableParse.Str(columns[26]);
			RespawnTime = TableParse.Int(columns[27]);
			SightAngle = TableParse.Float(columns[28]);
			SightRange = TableParse.Float(columns[29]);
			SightBackRange = TableParse.Float(columns[30]);
			WanderRangeMin = TableParse.Float(columns[31]);
			WanderRangeMax = TableParse.Float(columns[32]);
			BackAttackAngle = TableParse.Float(columns[33]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, ExtraUnitType, MonsterCategories, ActiveSkillIds, PassiveSkillIds, Atk, Def, AtkSpeed, MoveSpeed, Hp, HpRegen, FireResistancePercent, WaterResistancePercent, LightningResistancePercent, AttackRange, AttackSkillId, ActionSkillId, DropRewardGroupId, DetectRange, FleeHpPercent, FleeDuration, GlobalSkillCoolTime, PauseStatePercent, PauseStateDuration, AvoidStatePercent, AvoidStateDuration, DyingSfx, RespawnTime, SightAngle, SightRange, SightBackRange, WanderRangeMin, WanderRangeMax, BackAttackAngle);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,ExtraUnitType,MonsterCategories,ActiveSkillIds,PassiveSkillIds,Atk,Def,AtkSpeed,MoveSpeed,Hp,HpRegen,FireResistancePercent,WaterResistancePercent,LightningResistancePercent,AttackRange,AttackSkillId,ActionSkillId,DropRewardGroupId,DetectRange,FleeHpPercent,FleeDuration,GlobalSkillCoolTime,PauseStatePercent,PauseStateDuration,AvoidStatePercent,AvoidStateDuration,DyingSfx,RespawnTime,SightAngle,SightRange,SightBackRange,WanderRangeMin,WanderRangeMax,BackAttackAngle";
		}

		public static string GetTypesCsv()
		{
			return "int,EExtraUnitType ,EMonsterCategory[],int[],int[],int,int,float,float,int,float,float,float,float,float,int,int,int,float,float,float,float,float,float,float,float,string,int,float,float,float,float,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ExtraUnitTable();
			table.Load(new CsvTableLoader<int, TExtraUnit>(text));
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

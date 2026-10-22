using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacter : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterTable Table { get; set; }

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

		[Localized]
		public string[] Tags { get; set; }

		public bool IsUsable { get; set; }

		public EWeaponType WeaponType { get; set; }

		public int DefaultWeaponId { get; set; }

		public float WeaponAtkMultiplier { get; set; }

		public int Atk { get; set; }

		public int Def { get; set; }

		public float AtkSpeed { get; set; }

		public float Stamina { get; set; }

		public float StaminaRegen { get; set; }

		public float RunStaminaCost { get; set; }

		public float RunIncreaseSpeed { get; set; }

		public float MoveSpeed { get; set; }

		public float WalkAnimation1xSpeed { get; set; }

		public int Hp { get; set; }

		public float HpRegen { get; set; }

		public float FireResistancePercent { get; set; }

		public float WaterResistancePercent { get; set; }

		public float WindResistancePercent { get; set; }

		public int AttackSkillId { get; set; }

		public int DashSkillId { get; set; }

		public int[] ActiveSkillIds { get; set; }

		public int PolymorphAttackSkillId { get; set; }

		public int PolymorphDashSkillId { get; set; }

		public int[] PolymorphActiveSkillIds { get; set; }

		public int[] PassiveSkillIds { get; set; }

		public float SightAngle { get; set; }

		public float SightRange { get; set; }

		public float SightBackRange { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		[Localized]
		public string Title => null;

		private string NameKey => null;

		private string DescKey => null;

		private string TitleKey => null;

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
			Tags = TableParse.Array<string>(columns[1]);
			IsUsable = TableParse.Bool(columns[2]);
			WeaponType = TableParse.Enum<EWeaponType>(columns[3]);
			DefaultWeaponId = TableParse.Int(columns[4]);
			WeaponAtkMultiplier = TableParse.Float(columns[5]);
			Atk = TableParse.Int(columns[6]);
			Def = TableParse.Int(columns[7]);
			AtkSpeed = TableParse.Float(columns[8]);
			Stamina = TableParse.Float(columns[9]);
			StaminaRegen = TableParse.Float(columns[10]);
			RunStaminaCost = TableParse.Float(columns[11]);
			RunIncreaseSpeed = TableParse.Float(columns[12]);
			MoveSpeed = TableParse.Float(columns[13]);
			WalkAnimation1xSpeed = TableParse.Float(columns[14]);
			Hp = TableParse.Int(columns[15]);
			HpRegen = TableParse.Float(columns[16]);
			FireResistancePercent = TableParse.Float(columns[17]);
			WaterResistancePercent = TableParse.Float(columns[18]);
			WindResistancePercent = TableParse.Float(columns[19]);
			AttackSkillId = TableParse.Int(columns[20]);
			DashSkillId = TableParse.Int(columns[21]);
			ActiveSkillIds = TableParse.Array<int>(columns[22]);
			PolymorphAttackSkillId = TableParse.Int(columns[23]);
			PolymorphDashSkillId = TableParse.Int(columns[24]);
			PolymorphActiveSkillIds = TableParse.Array<int>(columns[25]);
			PassiveSkillIds = TableParse.Array<int>(columns[26]);
			SightAngle = TableParse.Float(columns[27]);
			SightRange = TableParse.Float(columns[28]);
			SightBackRange = TableParse.Float(columns[29]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Tags, IsUsable, WeaponType, DefaultWeaponId, WeaponAtkMultiplier, Atk, Def, AtkSpeed, Stamina, StaminaRegen, RunStaminaCost, RunIncreaseSpeed, MoveSpeed, WalkAnimation1xSpeed, Hp, HpRegen, FireResistancePercent, WaterResistancePercent, WindResistancePercent, AttackSkillId, DashSkillId, ActiveSkillIds, PolymorphAttackSkillId, PolymorphDashSkillId, PolymorphActiveSkillIds, PassiveSkillIds, SightAngle, SightRange, SightBackRange);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Tags:Localized,IsUsable,WeaponType,DefaultWeaponId,WeaponAtkMultiplier,Atk,Def,AtkSpeed,Stamina,StaminaRegen,RunStaminaCost,RunIncreaseSpeed,MoveSpeed,WalkAnimation1xSpeed,Hp,HpRegen,FireResistancePercent,WaterResistancePercent,WindResistancePercent,AttackSkillId,DashSkillId,ActiveSkillIds,PolymorphAttackSkillId,PolymorphDashSkillId,PolymorphActiveSkillIds,PassiveSkillIds,SightAngle,SightRange,SightBackRange";
		}

		public static string GetTypesCsv()
		{
			return "int,string[],bool,EWeaponType,int,float,int,int,float,float,float,float,float,float,float,int,float,float,float,float,int,int,int[],int,int,int[],int[],float,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CharacterTable();
			table.Load(new CsvTableLoader<int, TCharacter>(text));
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

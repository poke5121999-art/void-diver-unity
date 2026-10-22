using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDifficulty : ITableData<EDifficulty>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DifficultyTable Table { get; set; }

		public EDifficulty Pk
		{
			get
			{
				return Difficulty;
			}
			set
			{
			}
		}

		public EDifficulty Difficulty { get; set; }

		public float MonsterHpPercent { get; set; }

		public float MonsterAtkPercent { get; set; }

		public float MonsterDefPercent { get; set; }

		public float MonsterSpawnPercent { get; set; }

		public float MimicSpawnPercent { get; set; }

		public float TrapAtkPercent { get; set; }

		public float RewardBoxSpawnPercent { get; set; }

		public float DropRewardSpawnPercent { get; set; }

		public int NormalItemPickPercent { get; set; }

		public int RareItemPickPercent { get; set; }

		public int EliteItemPickPercent { get; set; }

		public int EpicItemPickPercent { get; set; }

		public int LegendItemPickPercent { get; set; }

		public int UniqueItemPickPercent { get; set; }

		public int EpicArtifactPickPercent { get; set; }

		public float UserExpPercent { get; set; }

		public float CooldownReductionPercent { get; set; }

		public float GlobalSkillCoolTimePercent { get; set; }

		public float StressDamagePercent { get; set; }

		public Action<ITableData<EDifficulty>> OnDataLoaded { get; set; }

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
			Difficulty = TableParse.Enum<EDifficulty>(columns[0]);
			MonsterHpPercent = TableParse.Float(columns[1]);
			MonsterAtkPercent = TableParse.Float(columns[2]);
			MonsterDefPercent = TableParse.Float(columns[3]);
			MonsterSpawnPercent = TableParse.Float(columns[4]);
			MimicSpawnPercent = TableParse.Float(columns[5]);
			TrapAtkPercent = TableParse.Float(columns[6]);
			RewardBoxSpawnPercent = TableParse.Float(columns[7]);
			DropRewardSpawnPercent = TableParse.Float(columns[8]);
			NormalItemPickPercent = TableParse.Int(columns[9]);
			RareItemPickPercent = TableParse.Int(columns[10]);
			EliteItemPickPercent = TableParse.Int(columns[11]);
			EpicItemPickPercent = TableParse.Int(columns[12]);
			LegendItemPickPercent = TableParse.Int(columns[13]);
			UniqueItemPickPercent = TableParse.Int(columns[14]);
			EpicArtifactPickPercent = TableParse.Int(columns[15]);
			UserExpPercent = TableParse.Float(columns[16]);
			CooldownReductionPercent = TableParse.Float(columns[17]);
			GlobalSkillCoolTimePercent = TableParse.Float(columns[18]);
			StressDamagePercent = TableParse.Float(columns[19]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Difficulty, MonsterHpPercent, MonsterAtkPercent, MonsterDefPercent, MonsterSpawnPercent, MimicSpawnPercent, TrapAtkPercent, RewardBoxSpawnPercent, DropRewardSpawnPercent, NormalItemPickPercent, RareItemPickPercent, EliteItemPickPercent, EpicItemPickPercent, LegendItemPickPercent, UniqueItemPickPercent, EpicArtifactPickPercent, UserExpPercent, CooldownReductionPercent, GlobalSkillCoolTimePercent, StressDamagePercent);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Difficulty,MonsterHpPercent,MonsterAtkPercent,MonsterDefPercent,MonsterSpawnPercent,MimicSpawnPercent,TrapAtkPercent,RewardBoxSpawnPercent,DropRewardSpawnPercent,NormalItemPickPercent,RareItemPickPercent,EliteItemPickPercent,EpicItemPickPercent,LegendItemPickPercent,UniqueItemPickPercent,EpicArtifactPickPercent,UserExpPercent,CooldownReductionPercent,GlobalSkillCoolTimePercent,StressDamagePercent";
		}

		public static string GetTypesCsv()
		{
			return "EDifficulty,float,float,float,float,float,float,float,float,int,int,int,int,int,int,int,float,float,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DifficultyTable();
			table.Load(new CsvTableLoader<EDifficulty, TDifficulty>(text));
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

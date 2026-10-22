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
				return EDifficulty.Easy;
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

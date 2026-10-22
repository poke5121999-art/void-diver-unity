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
				return 0;
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
		public string Name => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string DescKey => null;

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

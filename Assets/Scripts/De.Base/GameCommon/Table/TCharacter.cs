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
				return 0;
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

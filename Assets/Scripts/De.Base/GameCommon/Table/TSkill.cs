using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSkill : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SkillTable Table { get; set; }

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

		public ESkillType SkillType { get; set; }

		public bool RequireWeapon { get; set; }

		public ESkillTagType[] SkillTags { get; set; }

		public bool NotifySkillReady { get; set; }

		public EHitTarget TargetType { get; set; }

		public int StressCost { get; set; }

		public int HpCost { get; set; }

		public int StaminaCost { get; set; }

		public int ChargeCost { get; set; }

		public float ChargePerSec { get; set; }

		public float CoolTime { get; set; }

		public int StackCount { get; set; }

		public float StackChargeInterval { get; set; }

		public string ActionSfx { get; set; }

		public bool IsBasicAttack { get; set; }

		public bool IgnoreSilence { get; set; }

		public bool UseIcon { get; set; }

		public SkillActionNode RootActionNode { get; set; }

		public AiSkillCondition AiSkillCondition { get; set; }

		public SkillBuffStackCondition DeactivateBuffCondition { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string DescFormatKey => null;

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

		private string GetDesc()
		{
			return null;
		}

		public static string ResolveExpression(string expression)
		{
			return null;
		}

		public string[] ExtractExpressions(long nodeUniqueId, SkillAction skillAction)
		{
			return null;
		}

		private static bool TryFindNodePath(SkillActionNode node, long targetUniqueId, string currentPath, out string foundPath)
		{
			foundPath = null;
			return false;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

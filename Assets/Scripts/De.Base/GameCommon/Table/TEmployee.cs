using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEmployee : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EmployeeTable Table { get; set; }

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

		public int NpcId { get; set; }

		public int Charm { get; set; }

		public float CharmGrowth { get; set; }

		public int Persuasion { get; set; }

		public float PersuasionGrowth { get; set; }

		public int Resistance { get; set; }

		public float ResistanceGrowth { get; set; }

		public int HireCost { get; set; }

		public int[] SkillIds { get; set; }

		public int[] ForcedSkillLearnLevels { get; set; }

		[Localized]
		public string[] BubbleTextsOnReadyToSell { get; set; }

		[Localized]
		public string[] BubbleTextsOnReadyToSellHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnSelling { get; set; }

		[Localized]
		public string[] BubbleTextsOnSellingHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinish { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinishHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinishPanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnHire { get; set; }

		[Localized]
		public string[] BubbleTextsOnInfo { get; set; }

		[Localized]
		public string[] BubbleTextsOnInfoPanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnFire { get; set; }

		[Localized]
		public string[] BubbleTextsOnFirePanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnLevelUp { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string SubName => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string SubNameKey => null;

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

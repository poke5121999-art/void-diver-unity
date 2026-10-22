using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TArtifactDeal : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ArtifactDealTable Table { get; set; }

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

		public int SpineId { get; set; }

		public int MultiSkinId { get; set; }

		public EArtifactCategory[] PreferCategories { get; set; }

		public bool PrefersNegativeTags { get; set; }

		public int[] PreferPrefixIds { get; set; }

		public int PreferPrefixCountMin { get; set; }

		public int PreferPrefixCountMax { get; set; }

		public int[] DislikePrefixIds { get; set; }

		public int DislikePrefixCountMin { get; set; }

		public int DislikePrefixCountMax { get; set; }

		public bool AllowGiveUp { get; set; }

		[Localized]
		public string[] BubbleTextsOnDealStart { get; set; }

		[Localized]
		public string[] BubbleTextsOnReroll { get; set; }

		[Localized]
		public string[] BubbleTextsOnPlayCountExhausted { get; set; }

		[Localized]
		public string[] BubbleTextsOnDealAccepted { get; set; }

		[Localized]
		public string[] BubbleTextsOnResultSuccess { get; set; }

		[Localized]
		public string[] BubbleTextsOnDealGiveUp { get; set; }

		[Localized]
		public string[] BubbleTextsOnResultFail { get; set; }

		[Localized]
		public string[] BubbleTextsOnCardSuccessPrice { get; set; }

		[Localized]
		public string[] BubbleTextsOnCardSuccessPreferTag { get; set; }

		[Localized]
		public string[] BubbleTextsOnCardSuccessDislikeTag { get; set; }

		[Localized]
		public string[] BubbleTextsOnCardFail { get; set; }

		[Localized]
		public string[] BubbleTextsOnHesitationMidChance { get; set; }

		[Localized]
		public string[] BubbleTextsOnHesitationLowChance { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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

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
				return Id;
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
			NpcId = TableParse.Int(columns[1]);
			SpineId = TableParse.Int(columns[2]);
			MultiSkinId = TableParse.Int(columns[3]);
			PreferCategories = TableParse.Array<EArtifactCategory>(columns[4]);
			PrefersNegativeTags = TableParse.Bool(columns[5]);
			PreferPrefixIds = TableParse.Array<int>(columns[6]);
			PreferPrefixCountMin = TableParse.Int(columns[7]);
			PreferPrefixCountMax = TableParse.Int(columns[8]);
			DislikePrefixIds = TableParse.Array<int>(columns[9]);
			DislikePrefixCountMin = TableParse.Int(columns[10]);
			DislikePrefixCountMax = TableParse.Int(columns[11]);
			AllowGiveUp = TableParse.Bool(columns[12]);
			BubbleTextsOnDealStart = TableParse.Array<string>(columns[13]);
			BubbleTextsOnReroll = TableParse.Array<string>(columns[14]);
			BubbleTextsOnPlayCountExhausted = TableParse.Array<string>(columns[15]);
			BubbleTextsOnDealAccepted = TableParse.Array<string>(columns[16]);
			BubbleTextsOnResultSuccess = TableParse.Array<string>(columns[17]);
			BubbleTextsOnDealGiveUp = TableParse.Array<string>(columns[18]);
			BubbleTextsOnResultFail = TableParse.Array<string>(columns[19]);
			BubbleTextsOnCardSuccessPrice = TableParse.Array<string>(columns[20]);
			BubbleTextsOnCardSuccessPreferTag = TableParse.Array<string>(columns[21]);
			BubbleTextsOnCardSuccessDislikeTag = TableParse.Array<string>(columns[22]);
			BubbleTextsOnCardFail = TableParse.Array<string>(columns[23]);
			BubbleTextsOnHesitationMidChance = TableParse.Array<string>(columns[24]);
			BubbleTextsOnHesitationLowChance = TableParse.Array<string>(columns[25]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, NpcId, SpineId, MultiSkinId, PreferCategories, PrefersNegativeTags, PreferPrefixIds, PreferPrefixCountMin, PreferPrefixCountMax, DislikePrefixIds, DislikePrefixCountMin, DislikePrefixCountMax, AllowGiveUp, BubbleTextsOnDealStart, BubbleTextsOnReroll, BubbleTextsOnPlayCountExhausted, BubbleTextsOnDealAccepted, BubbleTextsOnResultSuccess, BubbleTextsOnDealGiveUp, BubbleTextsOnResultFail, BubbleTextsOnCardSuccessPrice, BubbleTextsOnCardSuccessPreferTag, BubbleTextsOnCardSuccessDislikeTag, BubbleTextsOnCardFail, BubbleTextsOnHesitationMidChance, BubbleTextsOnHesitationLowChance);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,NpcId,SpineId,MultiSkinId,PreferCategories,PrefersNegativeTags,PreferPrefixIds,PreferPrefixCountMin,PreferPrefixCountMax,DislikePrefixIds,DislikePrefixCountMin,DislikePrefixCountMax,AllowGiveUp,BubbleTextsOnDealStart:Localized,BubbleTextsOnReroll:Localized,BubbleTextsOnPlayCountExhausted:Localized,BubbleTextsOnDealAccepted:Localized,BubbleTextsOnResultSuccess:Localized,BubbleTextsOnDealGiveUp:Localized,BubbleTextsOnResultFail:Localized,BubbleTextsOnCardSuccessPrice:Localized,BubbleTextsOnCardSuccessPreferTag:Localized,BubbleTextsOnCardSuccessDislikeTag:Localized,BubbleTextsOnCardFail:Localized,BubbleTextsOnHesitationMidChance:Localized,BubbleTextsOnHesitationLowChance:Localized";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int,EArtifactCategory[],bool,int[],int,int,int[],int,int,bool,string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ArtifactDealTable();
			table.Load(new CsvTableLoader<int, TArtifactDeal>(text));
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

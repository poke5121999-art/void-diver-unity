using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TTalent : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static TalentTable Table { get; set; }

		public static SubTalentTable SubTable { get; set; }

		public static GroupTalentTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public int BitFlagIndex { get; set; }

		public int[] RootTalentIds { get; set; }

		public TalentEffect[] Effects { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public GoodsData[] MaterialGoodsDatas { get; set; }

		public int[] DisplayPoint { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public long NeedGold => 0L;

		public long NeedCoin => 0L;

		public long NeedRealCoin => 0L;

		public GoodsData[] MaterialItemDatas => null;

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

		public bool Validate()
		{
			return false;
		}
	}
}

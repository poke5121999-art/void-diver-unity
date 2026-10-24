using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TTalent : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static TalentTable Table { get; set; } = new TalentTable();

		public static SubTalentTable SubTable { get; set; } = new SubTalentTable();

		public static GroupTalentTable GroupTable { get; set; } = new GroupTalentTable();

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

		public int GroupId { get; set; }

		public int BitFlagIndex { get; set; }

		public int[] RootTalentIds { get; set; }

		public TalentEffect[] Effects { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public GoodsData[] MaterialGoodsDatas { get; set; }

		public int[] DisplayPoint { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public long NeedGold => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Gold).Sum(g => g.Count);

		public long NeedCoin => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Coin).Sum(g => g.Count);

		public long NeedRealCoin => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Coin && g.Id == 10001).Sum(g => g.Count);

		public GoodsData[] MaterialItemDatas => MaterialGoodsDatas?.Where(g => g.Type.GoodsCategory() == EGoodsCategory.Equipment || (g.Type.GoodsCategory() == EGoodsCategory.Item && g.Id != 10001)).ToArray();

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		private string NameKey => string.Format("{0}_{1}_{2}", "TTalent", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TTalent", "Desc", Id);

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
			GroupId = TableParse.Int(columns[1]);
			BitFlagIndex = TableParse.Int(columns[2]);
			RootTalentIds = TableParse.Array<int>(columns[3]);
			Effects = TableParse.Json<TalentEffect[]>(columns[4]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[5]);
			MaterialGoodsDatas = TableParse.Json<GoodsData[]>(columns[6]);
			DisplayPoint = TableParse.Array<int>(columns[7]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, BitFlagIndex, RootTalentIds, Effects, UnlockConditions, MaterialGoodsDatas, DisplayPoint);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,BitFlagIndex,RootTalentIds,Effects,UnlockConditions,MaterialGoodsDatas,DisplayPoint";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int[],TalentEffect[],EventCondition[],GoodsData[],int[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new TalentTable();
			table.Load(new CsvTableLoader<int, TTalent>(text));
			Table = table;
			GroupTable = new GroupTalentTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
			SubTable = new SubTalentTable();
			SubTable.Init(table.Values.Select(row => (row.BitFlagIndex, row)));
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
		}

		public static bool ValidateTable()
		{
			bool result = true;
			if (Table != null)
			{
				foreach (TTalent row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		private string GetDesc()
		{
			if (TLocalizedText.Table.TryGetValue(DescFormatKey, out var data))
			{
				return data.FormatWithExpression();
			}
			return DescFormatKey;
		}

		public bool Validate()
		{
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescFormatKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("RootTalentIds", typeof(TTalent))
				&& this.ValidateRefGoodsData("MaterialGoodsDatas")
				&& this.ValidateRefEventCondition("UnlockConditions")
				&& this.ValidateRefTalentEffect("Effects");
		}
	}
}

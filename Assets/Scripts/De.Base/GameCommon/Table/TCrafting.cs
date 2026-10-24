using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCrafting : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		private static Dictionary<int, GoodsData> _conditionItemIdToResultGoodsData;

		public static CraftingTable Table { get; set; } = new CraftingTable();

		public static GroupCraftingTable GroupTable { get; set; } = new GroupCraftingTable();

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

		public GoodsData ResultGoodsData { get; set; }

		public GoodsData[] MaterialGoodsDatas { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public long NeedGold => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Gold).Sum(g => g.Count);

		public long NeedCoin => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Coin).Sum(g => g.Count);

		public long NeedRealCoin => MaterialGoodsDatas == null ? 0 : MaterialGoodsDatas.Where(g => g.Type == EGoodsType.Coin && g.Id == 10001).Sum(g => g.Count);

		public GoodsData[] MaterialItemDatas => MaterialGoodsDatas?.Where(g => g.Type.GoodsCategory() == EGoodsCategory.Equipment || (g.Type.GoodsCategory() == EGoodsCategory.Item && g.Id != 10001)).ToArray();

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
			ResultGoodsData = TableParse.Json<GoodsData>(columns[2]);
			MaterialGoodsDatas = TableParse.Json<GoodsData[]>(columns[3]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, ResultGoodsData, MaterialGoodsDatas, UnlockConditions);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,ResultGoodsData,MaterialGoodsDatas,UnlockConditions";
		}

		public static string GetTypesCsv()
		{
			return "int,int,GoodsData,GoodsData[],EventCondition[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CraftingTable();
			table.Load(new CsvTableLoader<int, TCrafting>(text));
			Table = table;
			GroupTable = new GroupCraftingTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
		}

		public static bool ValidateTable()
		{
			bool result = true;
			foreach (TCrafting row in Table.Values)
			{
				if (!row.Validate())
				{
					result = false;
				}
			}
			return result;
		}

		public static GoodsData GetResultGoodsDataByUsedItem(int itemId)
		{
			if (_conditionItemIdToResultGoodsData == null)
			{
				_conditionItemIdToResultGoodsData = Table.Values
					.Select(c => new { Craft = c, Condition = c.UnlockConditions?.FirstOrDefault() })
					.Where(x => x.Condition.HasValue && x.Condition.GetValueOrDefault().Type == EEventConditionType.ItemUsed)
					.GroupBy(x => (int)x.Condition.Value.Value1)
					.ToDictionary(g => g.Key, g => g.First().Craft.ResultGoodsData);
			}
			if (!_conditionItemIdToResultGoodsData.TryGetValue(itemId, out var result))
			{
				return GoodsData.Empty();
			}
			return result;
		}

		public bool Validate()
		{
			if (!(this.ValidateRefGoodsData("ResultGoodsData")
				&& this.ValidateRefGoodsData("MaterialGoodsDatas")
				&& this.ValidateRefEventCondition("UnlockConditions")
				&& this.Check(ResultGoodsData.Count > 0, "ResultGoodsData count must be at least 1")))
			{
				return false;
			}
			EGoodsCategory category = ResultGoodsData.Category;
			bool countOk = (category != EGoodsCategory.Equipment && category != EGoodsCategory.Bag) || ResultGoodsData.Count == 1;
			return this.Check(countOk, "ResultGoodsData equipment/bag count must be 1");
		}
	}
}

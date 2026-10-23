using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDropRewardProbability : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DropRewardProbabilityTable Table { get; set; } = new DropRewardProbabilityTable();

		public static GroupDropRewardProbabilityTable GroupTable { get; set; }

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

		public int DropRewardId { get; set; }

		public int PickOrder { get; set; }

		public GoodsData GoodsData { get; set; }

		public int Weight { get; set; }

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
			DropRewardId = TableParse.Int(columns[1]);
			PickOrder = TableParse.Int(columns[2]);
			GoodsData = TableParse.Json<GoodsData>(columns[3]);
			Weight = TableParse.Int(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, DropRewardId, PickOrder, GoodsData, Weight);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,DropRewardId,PickOrder,GoodsData,Weight";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,GoodsData,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DropRewardProbabilityTable();
			table.Load(new CsvTableLoader<int, TDropRewardProbability>(text));
			Table = table;
			GroupTable = new GroupDropRewardProbabilityTable();
			GroupTable.Init(table.Values.GroupBy(row => row.DropRewardId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TDropRewardProbability row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public bool Validate()
		{
			return this.ValidateRefKey("DropRewardId", typeof(TDropReward))
				&& this.ValidateRefGoodsData("GoodsData");
		}
	}
}

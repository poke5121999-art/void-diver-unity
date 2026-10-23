using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDropRewardArtifactProbability : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DropRewardArtifactProbabilityTable Table { get; set; } = new DropRewardArtifactProbabilityTable();

		public static GroupDropRewardArtifactProbabilityTable GroupTable { get; set; }

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

		public EGoodsGradeType Grade { get; set; }

		public int Count { get; set; }

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
			Grade = TableParse.Enum<EGoodsGradeType>(columns[3]);
			Count = TableParse.Int(columns[4]);
			Weight = TableParse.Int(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, DropRewardId, PickOrder, Grade, Count, Weight);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,DropRewardId,PickOrder,Grade,Count,Weight";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,EGoodsGradeType,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DropRewardArtifactProbabilityTable();
			table.Load(new CsvTableLoader<int, TDropRewardArtifactProbability>(text));
			Table = table;
			GroupTable = new GroupDropRewardArtifactProbabilityTable();
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
				foreach (TDropRewardArtifactProbability row in Table.Values)
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
				&& this.Check(Count > 0, string.Format("{0}: {1} must be > 0", "Count", Count))
				&& this.Check(Weight > 0, string.Format("{0}: {1} must be > 0", "Weight", Weight))
				&& this.Check(System.Linq.Enumerable.Any(TEquipment.Table.Values, (TEquipment equipment) => equipment.IsArtifact() && equipment.Grade == Grade), string.Format("{0}: {1} has no matching artifact in {2}", "Grade", Grade, "TEquipment"));
		}
	}
}

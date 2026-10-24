using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDropReward : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DropRewardTable Table { get; set; } = new DropRewardTable();

		public static GroupDropRewardTable GroupTable { get; set; } = new GroupDropRewardTable();

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

		public int NoneWeight { get; set; }

		public int NormalWeight { get; set; }

		public int RareWeight { get; set; }

		public int EliteWeight { get; set; }

		public int EpicWeight { get; set; }

		public int LegendWeight { get; set; }

		public int UniqueWeight { get; set; }

		public bool PickGradeBeforeProbability { get; set; }

		public int NormalBp { get; set; }

		public int ArtifactBp { get; set; }

		public int PickCount { get; set; }

		public bool IgnoreDifficulty { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public int BaseNormalPickBp => NormalBp;

		public int TotalGradeWeight => UniqueWeight + LegendWeight + EpicWeight + EliteWeight + RareWeight + NormalWeight + NoneWeight;

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
			NoneWeight = TableParse.Int(columns[2]);
			NormalWeight = TableParse.Int(columns[3]);
			RareWeight = TableParse.Int(columns[4]);
			EliteWeight = TableParse.Int(columns[5]);
			EpicWeight = TableParse.Int(columns[6]);
			LegendWeight = TableParse.Int(columns[7]);
			UniqueWeight = TableParse.Int(columns[8]);
			PickGradeBeforeProbability = TableParse.Bool(columns[9]);
			NormalBp = TableParse.Int(columns[10]);
			ArtifactBp = TableParse.Int(columns[11]);
			PickCount = TableParse.Int(columns[12]);
			IgnoreDifficulty = TableParse.Bool(columns[13]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, NoneWeight, NormalWeight, RareWeight, EliteWeight, EpicWeight, LegendWeight, UniqueWeight, PickGradeBeforeProbability, NormalBp, ArtifactBp, PickCount, IgnoreDifficulty);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,NoneWeight,NormalWeight,RareWeight,EliteWeight,EpicWeight,LegendWeight,UniqueWeight,PickGradeBeforeProbability,NormalBp,ArtifactBp,PickCount,IgnoreDifficulty";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int,int,int,int,int,int,bool,int,int,int,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DropRewardTable();
			table.Load(new CsvTableLoader<int, TDropReward>(text));
			Table = table;
			GroupTable = new GroupDropRewardTable();
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
			if (Table != null)
			{
				foreach (TDropReward row in Table.Values)
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
			bool result = this.Check(PickCount > 0, string.Format("{0}: {1} must be > 0", "PickCount", PickCount))
				& this.Check(NormalBp >= 0, string.Format("{0}: {1} must be >= 0", "NormalBp", NormalBp))
				& this.Check(ArtifactBp >= 0, string.Format("{0}: {1} must be >= 0", "ArtifactBp", ArtifactBp))
				& this.Check(NormalBp + ArtifactBp <= 10000, string.Format("{0}({1}) + {2}({3}) = {4} must be <= 10000", "NormalBp", NormalBp, "ArtifactBp", ArtifactBp, NormalBp + ArtifactBp));
			if (PickGradeBeforeProbability)
			{
				result &= this.Check(TotalGradeWeight > 0, "PickGradeBeforeProbability is true but TotalGradeWeight is 0 (no grade can be selected)");
			}
			if (NormalBp > 0)
			{
				result &= this.Check(TDropRewardProbability.GroupTable.ContainsKey(Id), string.Format("{0} > 0 but TDropRewardProbability.GroupTable has no group for Id {1}", "NormalBp", Id));
			}
			if (ArtifactBp > 0)
			{
				result &= this.Check(TDropRewardArtifactProbability.GroupTable.ContainsKey(Id), string.Format("{0} > 0 but TDropRewardArtifactProbability.GroupTable has no group for Id {1}", "ArtifactBp", Id));
			}
			return result;
		}
	}
}

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

		public static DropRewardTable Table { get; set; }

		public static GroupDropRewardTable GroupTable { get; set; }

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

		public int BaseNormalPickBp => 0;

		public int TotalGradeWeight => 0;

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
			return false;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

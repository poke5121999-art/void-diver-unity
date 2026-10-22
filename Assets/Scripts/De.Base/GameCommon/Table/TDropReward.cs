using System;
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
				return 0;
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

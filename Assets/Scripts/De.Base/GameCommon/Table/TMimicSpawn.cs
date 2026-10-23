using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMimicSpawn : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MimicSpawnTable Table { get; set; } = new MimicSpawnTable();

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

		public int MimicId { get; set; }

		public int RewardBoxId { get; set; }

		public float MimicPercent { get; set; }

		public float RewardBoxPercent { get; set; }

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
			MimicId = TableParse.Int(columns[1]);
			RewardBoxId = TableParse.Int(columns[2]);
			MimicPercent = TableParse.Float(columns[3]);
			RewardBoxPercent = TableParse.Float(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, MimicId, RewardBoxId, MimicPercent, RewardBoxPercent);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,MimicId,RewardBoxId,MimicPercent,RewardBoxPercent";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MimicSpawnTable();
			table.Load(new CsvTableLoader<int, TMimicSpawn>(text));
			Table = table;
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
				foreach (TMimicSpawn row in Table.Values)
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
			bool result = this.ValidateRefKey("MimicId", typeof(TMimic))
				&& this.Check(MimicPercent >= 0f && MimicPercent <= 100f, string.Format("{0}: {1} must be between 0 and 100", "MimicPercent", MimicPercent))
				&& this.Check(RewardBoxPercent >= 0f && RewardBoxPercent <= 100f, string.Format("{0}: {1} must be between 0 and 100", "RewardBoxPercent", RewardBoxPercent))
				&& this.Check(RewardBoxPercent + MimicPercent <= 100f, string.Format("{0} + {1}: {2} must be <= 100", "RewardBoxPercent", "MimicPercent", RewardBoxPercent + MimicPercent))
				&& this.Check(!(RewardBoxPercent > 0f) || RewardBoxId > 0, string.Format("{0} must be set when {1}: {2} > 0", "RewardBoxId", "RewardBoxPercent", RewardBoxPercent));
			if (RewardBoxId > 0)
			{
				if (!result)
				{
					return false;
				}
				result = this.ValidateRefKey("RewardBoxId", typeof(TRewardBox));
			}
			return result;
		}
	}
}

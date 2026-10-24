using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TShadowMonsterSpawn : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ShadowMonsterSpawnTable Table { get; set; } = new ShadowMonsterSpawnTable();

		public static SubShadowMonsterSpawnTable SubTable { get; set; } = new SubShadowMonsterSpawnTable();

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

		public EDifficulty Difficulty { get; set; }

		public EStressState StressState { get; set; }

		public int ShadowMonsterGroupSpawnId { get; set; }

		public int AnomalyGroupSpawnId { get; set; }

		public float ShadowSpawnInterval { get; set; }

		public float ShadowSpawnPercent { get; set; }

		public int ShadowMonsterCountMin { get; set; }

		public int ShadowMonsterCountMax { get; set; }

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
			Difficulty = TableParse.Enum<EDifficulty>(columns[1]);
			StressState = TableParse.Enum<EStressState>(columns[2]);
			ShadowMonsterGroupSpawnId = TableParse.Int(columns[3]);
			AnomalyGroupSpawnId = TableParse.Int(columns[4]);
			ShadowSpawnInterval = TableParse.Float(columns[5]);
			ShadowSpawnPercent = TableParse.Float(columns[6]);
			ShadowMonsterCountMin = TableParse.Int(columns[7]);
			ShadowMonsterCountMax = TableParse.Int(columns[8]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Difficulty, StressState, ShadowMonsterGroupSpawnId, AnomalyGroupSpawnId, ShadowSpawnInterval, ShadowSpawnPercent, ShadowMonsterCountMin, ShadowMonsterCountMax);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Difficulty,StressState,ShadowMonsterGroupSpawnId,AnomalyGroupSpawnId,ShadowSpawnInterval,ShadowSpawnPercent,ShadowMonsterCountMin,ShadowMonsterCountMax";
		}

		public static string GetTypesCsv()
		{
			return "int,EDifficulty,EStressState,int,int,float,float,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ShadowMonsterSpawnTable();
			table.Load(new CsvTableLoader<int, TShadowMonsterSpawn>(text));
			Table = table;
			SubTable = new SubShadowMonsterSpawnTable();
			SubTable.Init(table.Values.Select(row => (row.Difficulty, row.StressState, row)));
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
				foreach (TShadowMonsterSpawn row in Table.Values)
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
			return this.ValidateRefGroupKey("ShadowMonsterGroupSpawnId", typeof(TMonsterSpawn))
				&& this.ValidateRefGroupKey("AnomalyGroupSpawnId", typeof(TMonsterSpawn));
		}
	}
}

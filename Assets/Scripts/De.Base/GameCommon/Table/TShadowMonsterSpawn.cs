using System;
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

		public static ShadowMonsterSpawnTable Table { get; set; }

		public static SubShadowMonsterSpawnTable SubTable { get; set; }

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

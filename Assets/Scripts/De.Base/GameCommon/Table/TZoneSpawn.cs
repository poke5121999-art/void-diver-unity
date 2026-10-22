using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TZoneSpawn : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ZoneSpawnTable Table { get; set; }

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

		public string Memo { get; set; }

		public float ExcludedNormalSpawnRange { get; set; }

		public EZoneFlags RequiredZoneFlags { get; set; }

		public EZoneFlags ExcludedZoneFlags { get; set; }

		public ZoneSpawnDistanceData[] SpawnDistanceDatas { get; set; }

		public ZoneObjectSpawnData[] SpawnDatas { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		private string NameKey => null;

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

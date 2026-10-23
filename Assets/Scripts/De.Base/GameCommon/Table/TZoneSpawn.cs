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
				return Id;
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
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TZoneSpawn", "Name", Id);

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
			Memo = TableParse.Str(columns[1]);
			ExcludedNormalSpawnRange = TableParse.Float(columns[2]);
			RequiredZoneFlags = TableParse.Enum<EZoneFlags>(columns[3]);
			ExcludedZoneFlags = TableParse.Enum<EZoneFlags>(columns[4]);
			SpawnDistanceDatas = TableParse.Json<ZoneSpawnDistanceData[]>(columns[5]);
			SpawnDatas = TableParse.Json<ZoneObjectSpawnData[]>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, ExcludedNormalSpawnRange, RequiredZoneFlags, ExcludedZoneFlags, SpawnDistanceDatas, SpawnDatas);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,ExcludedNormalSpawnRange,RequiredZoneFlags,ExcludedZoneFlags,SpawnDistanceDatas,SpawnDatas";
		}

		public static string GetTypesCsv()
		{
			return "int,string,float,EZoneFlags,EZoneFlags,ZoneSpawnDistanceData[],ZoneObjectSpawnData[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ZoneSpawnTable();
			table.Load(new CsvTableLoader<int, TZoneSpawn>(text));
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
				foreach (TZoneSpawn row in Table.Values)
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
			bool isValid = this.ValidateRefKey("NameKey", typeof(TLocalizedText));
			if (SpawnDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(SpawnDatas, delegate(ZoneObjectSpawnData spawnData)
				{
					string errorMessage = null;
					if (!spawnData.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			return isValid;
		}
	}
}

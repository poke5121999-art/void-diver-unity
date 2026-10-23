using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TWave : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static WaveTable Table { get; set; }

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

		public float Duration { get; set; }

		public EWaveClearCondition ClearCondition { get; set; }

		public int ConditionTargetId { get; set; }

		public int SpecialFieldId { get; set; }

		public float SpecialFieldRange { get; set; }

		public WavePhase[] WavePhases { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TWave", "Name", Id);

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
			Duration = TableParse.Float(columns[2]);
			ClearCondition = TableParse.Enum<EWaveClearCondition>(columns[3]);
			ConditionTargetId = TableParse.Int(columns[4]);
			SpecialFieldId = TableParse.Int(columns[5]);
			SpecialFieldRange = TableParse.Float(columns[6]);
			WavePhases = TableParse.Json<WavePhase[]>(columns[7]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, Duration, ClearCondition, ConditionTargetId, SpecialFieldId, SpecialFieldRange, WavePhases);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,Duration,ClearCondition,ConditionTargetId,SpecialFieldId,SpecialFieldRange,WavePhases";
		}

		public static string GetTypesCsv()
		{
			return "int,string,float,EWaveClearCondition,int,int,float,WavePhase[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new WaveTable();
			table.Load(new CsvTableLoader<int, TWave>(text));
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
				foreach (TWave row in Table.Values)
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
			bool result = this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("SpecialFieldId", typeof(TSpecialField));
			if (WavePhases != null)
			{
				for (int i = 0; i < WavePhases.Length; i++)
				{
					WavePhase phase = WavePhases[i];
					if (phase != null && phase.WaveSpawnDatas != null)
					{
						foreach (WaveSpawnData spawnData in phase.WaveSpawnDatas)
						{
							result &= this.ValidateSpawnableMonsterId("WavePhases", spawnData.monsterId);
						}
					}
				}
			}
			return result;
		}
	}
}

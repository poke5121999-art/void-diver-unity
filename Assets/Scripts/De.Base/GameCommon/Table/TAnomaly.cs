using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TAnomaly : ITableData<EAnomalyType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static AnomalyTable Table { get; set; } = new AnomalyTable();

		public EAnomalyType Pk
		{
			get
			{
				return Type;
			}
			set
			{
			}
		}

		public EAnomalyType Type { get; set; }

		public int Weight { get; set; }

		public int[] PlayerBuffIds { get; set; }

		public int[] MonsterBuffIds { get; set; }

		public EAnomalyEffectType[] EffectTypes { get; set; }

		public string AmbientSfx { get; set; }

		public Action<ITableData<EAnomalyType>> OnDataLoaded { get; set; }

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
			Type = TableParse.Enum<EAnomalyType>(columns[0]);
			Weight = TableParse.Int(columns[1]);
			PlayerBuffIds = TableParse.Array<int>(columns[2]);
			MonsterBuffIds = TableParse.Array<int>(columns[3]);
			EffectTypes = TableParse.Array<EAnomalyEffectType>(columns[4]);
			AmbientSfx = TableParse.Str(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Type, Weight, PlayerBuffIds, MonsterBuffIds, EffectTypes, AmbientSfx);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Type,Weight,PlayerBuffIds,MonsterBuffIds,EffectTypes,AmbientSfx";
		}

		public static string GetTypesCsv()
		{
			return "EAnomalyType,int,int[],int[],EAnomalyEffectType[],string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new AnomalyTable();
			table.Load(new CsvTableLoader<EAnomalyType, TAnomaly>(text));
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
				foreach (TAnomaly row in Table.Values)
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
			return this.ValidateRefKey("PlayerBuffIds", typeof(TBuff))
				&& this.ValidateRefKey("MonsterBuffIds", typeof(TBuff))
				&& this.ValidateRefSfx("AmbientSfx");
		}
	}
}

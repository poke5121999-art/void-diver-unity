using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TArtifactPrefix : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ArtifactPrefixTable Table { get; set; } = new ArtifactPrefixTable();

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

		public float PriceMultiplier { get; set; }

		public int Weight { get; set; }

		public float MaxWorkProgressWeight { get; set; }

		public Stat[] Stats { get; set; }

		public int PreferPriceBonusPercent { get; set; }

		public int DislikePricePenaltyPercent { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TArtifactPrefix", "Name", Id);

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
			PriceMultiplier = TableParse.Float(columns[1]);
			Weight = TableParse.Int(columns[2]);
			MaxWorkProgressWeight = TableParse.Float(columns[3]);
			Stats = TableParse.Json<Stat[]>(columns[4]);
			PreferPriceBonusPercent = TableParse.Int(columns[5]);
			DislikePricePenaltyPercent = TableParse.Int(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PriceMultiplier, Weight, MaxWorkProgressWeight, Stats, PreferPriceBonusPercent, DislikePricePenaltyPercent);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PriceMultiplier,Weight,MaxWorkProgressWeight,Stats,PreferPriceBonusPercent,DislikePricePenaltyPercent";
		}

		public static string GetTypesCsv()
		{
			return "int,float,int,float,Stat[],int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ArtifactPrefixTable();
			table.Load(new CsvTableLoader<int, TArtifactPrefix>(text));
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
				foreach (TArtifactPrefix row in Table.Values)
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
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText));
		}
	}
}

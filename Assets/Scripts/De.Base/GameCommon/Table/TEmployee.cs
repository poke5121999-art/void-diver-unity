using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEmployee : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EmployeeTable Table { get; set; } = new EmployeeTable();

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

		public int NpcId { get; set; }

		public int Charm { get; set; }

		public float CharmGrowth { get; set; }

		public int Persuasion { get; set; }

		public float PersuasionGrowth { get; set; }

		public int Resistance { get; set; }

		public float ResistanceGrowth { get; set; }

		public int HireCost { get; set; }

		public int[] SkillIds { get; set; }

		public int[] ForcedSkillLearnLevels { get; set; }

		[Localized]
		public string[] BubbleTextsOnReadyToSell { get; set; }

		[Localized]
		public string[] BubbleTextsOnReadyToSellHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnSelling { get; set; }

		[Localized]
		public string[] BubbleTextsOnSellingHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinish { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinishHighStress { get; set; }

		[Localized]
		public string[] BubbleTextsOnFinishPanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnHire { get; set; }

		[Localized]
		public string[] BubbleTextsOnInfo { get; set; }

		[Localized]
		public string[] BubbleTextsOnInfoPanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnFire { get; set; }

		[Localized]
		public string[] BubbleTextsOnFirePanic { get; set; }

		[Localized]
		public string[] BubbleTextsOnLevelUp { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string SubName => LocalizationUtility.GetLocalizedText(SubNameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TEmployee", "Name", Id);

		private string SubNameKey => string.Format("{0}_{1}_{2}", "TEmployee", "SubName", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TEmployee", "Desc", Id);

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
			NpcId = TableParse.Int(columns[1]);
			Charm = TableParse.Int(columns[2]);
			CharmGrowth = TableParse.Float(columns[3]);
			Persuasion = TableParse.Int(columns[4]);
			PersuasionGrowth = TableParse.Float(columns[5]);
			Resistance = TableParse.Int(columns[6]);
			ResistanceGrowth = TableParse.Float(columns[7]);
			HireCost = TableParse.Int(columns[8]);
			SkillIds = TableParse.Array<int>(columns[9]);
			ForcedSkillLearnLevels = TableParse.Array<int>(columns[10]);
			BubbleTextsOnReadyToSell = TableParse.Array<string>(columns[11]);
			BubbleTextsOnReadyToSellHighStress = TableParse.Array<string>(columns[12]);
			BubbleTextsOnSelling = TableParse.Array<string>(columns[13]);
			BubbleTextsOnSellingHighStress = TableParse.Array<string>(columns[14]);
			BubbleTextsOnFinish = TableParse.Array<string>(columns[15]);
			BubbleTextsOnFinishHighStress = TableParse.Array<string>(columns[16]);
			BubbleTextsOnFinishPanic = TableParse.Array<string>(columns[17]);
			BubbleTextsOnHire = TableParse.Array<string>(columns[18]);
			BubbleTextsOnInfo = TableParse.Array<string>(columns[19]);
			BubbleTextsOnInfoPanic = TableParse.Array<string>(columns[20]);
			BubbleTextsOnFire = TableParse.Array<string>(columns[21]);
			BubbleTextsOnFirePanic = TableParse.Array<string>(columns[22]);
			BubbleTextsOnLevelUp = TableParse.Array<string>(columns[23]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, NpcId, Charm, CharmGrowth, Persuasion, PersuasionGrowth, Resistance, ResistanceGrowth, HireCost, SkillIds, ForcedSkillLearnLevels, BubbleTextsOnReadyToSell, BubbleTextsOnReadyToSellHighStress, BubbleTextsOnSelling, BubbleTextsOnSellingHighStress, BubbleTextsOnFinish, BubbleTextsOnFinishHighStress, BubbleTextsOnFinishPanic, BubbleTextsOnHire, BubbleTextsOnInfo, BubbleTextsOnInfoPanic, BubbleTextsOnFire, BubbleTextsOnFirePanic, BubbleTextsOnLevelUp);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,NpcId,Charm,CharmGrowth,Persuasion,PersuasionGrowth,Resistance,ResistanceGrowth,HireCost,SkillIds,ForcedSkillLearnLevels,BubbleTextsOnReadyToSell:Localized,BubbleTextsOnReadyToSellHighStress:Localized,BubbleTextsOnSelling:Localized,BubbleTextsOnSellingHighStress:Localized,BubbleTextsOnFinish:Localized,BubbleTextsOnFinishHighStress:Localized,BubbleTextsOnFinishPanic:Localized,BubbleTextsOnHire:Localized,BubbleTextsOnInfo:Localized,BubbleTextsOnInfoPanic:Localized,BubbleTextsOnFire:Localized,BubbleTextsOnFirePanic:Localized,BubbleTextsOnLevelUp:Localized";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,float,int,float,int,float,int,int[],int[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[],string[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EmployeeTable();
			table.Load(new CsvTableLoader<int, TEmployee>(text));
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
				foreach (TEmployee row in Table.Values)
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
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("SubNameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("NpcId", typeof(TNpc))
				&& this.ValidateRefKey("SkillIds", typeof(TEmployeeSkill));
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TArtifactDealCard : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ArtifactDealCardTable Table { get; set; } = new ArtifactDealCardTable();

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

		public int Weight { get; set; }

		public EArtifactDealCardRarityType RarityType { get; set; }

		public int SuccessPercent { get; set; }

		public ArtifactDealCardEffect[] SuccessEffects { get; set; }

		public ArtifactDealCardEffect[] FailEffects { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TArtifactDealCard", "Name", Id);

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
			Weight = TableParse.Int(columns[1]);
			RarityType = TableParse.Enum<EArtifactDealCardRarityType>(columns[2]);
			SuccessPercent = TableParse.Int(columns[3]);
			SuccessEffects = TableParse.Json<ArtifactDealCardEffect[]>(columns[4]);
			FailEffects = TableParse.Json<ArtifactDealCardEffect[]>(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Weight, RarityType, SuccessPercent, SuccessEffects, FailEffects);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Weight,RarityType,SuccessPercent,SuccessEffects,FailEffects";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EArtifactDealCardRarityType,int,ArtifactDealCardEffect[],ArtifactDealCardEffect[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ArtifactDealCardTable();
			table.Load(new CsvTableLoader<int, TArtifactDealCard>(text));
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
				foreach (TArtifactDealCard row in Table.Values)
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
				&& this.Check(SuccessPercent >= 0 && SuccessPercent <= 100, string.Format("{0}({1}) must be within 0~100.", "SuccessPercent", SuccessPercent))
				&& this.Check(Weight >= 0, string.Format("{0}({1}) must be >= 0.", "Weight", Weight))
				&& this.Check(SuccessEffects == null || SuccessEffects.Length <= 1, string.Format("{0} length({1}) must be <= 1 (카드당 효과 1개).", "SuccessEffects", (SuccessEffects != null) ? SuccessEffects.Length : 0))
				&& this.Check(FailEffects == null || FailEffects.Length <= 1, string.Format("{0} length({1}) must be <= 1 (카드당 효과 1개).", "FailEffects", (FailEffects != null) ? FailEffects.Length : 0))
				&& ValidateEffectPrefixRefs(SuccessEffects, "SuccessEffects")
				&& ValidateEffectPrefixRefs(FailEffects, "FailEffects");
		}

		private bool ValidateEffectPrefixRefs(ArtifactDealCardEffect[] effects, string fieldName)
		{
			if (effects == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < effects.Length; i++)
			{
				ArtifactDealCardEffect effect = effects[i];
				if (effect.EffectType == EArtifactDealCardEffectType.AddSpecificPreferTag && !TArtifactPrefix.Table.ContainsKey(effect.Value))
				{
					this.LogError(string.Format("{0}.Table not contains \"[{1}] {2}:{3}\"", "TArtifactPrefix", fieldName, effect.EffectType, effect.Value));
					result = false;
				}
			}
			return result;
		}
	}
}

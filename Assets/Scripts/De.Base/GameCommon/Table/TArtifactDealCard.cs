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

		public static ArtifactDealCardTable Table { get; set; }

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
		public string Name => null;

		private string NameKey => null;

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
			return false;
		}

		public bool Validate()
		{
			return false;
		}

		private bool ValidateEffectPrefixRefs(ArtifactDealCardEffect[] effects, string fieldName)
		{
			return false;
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEmployeeSkill : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EmployeeSkillTable Table { get; set; } = new EmployeeSkillTable();

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

		public int[] TargetPrefix { get; set; }

		public EArtifactCategory[] TargetCategory { get; set; }

		public EGoodsGradeType[] TargetGrade { get; set; }

		public EEmployeeSkillType SkillType { get; set; }

		public float Value { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TEmployeeSkill", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TEmployeeSkill", "Desc", Id);

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
			TargetPrefix = TableParse.Array<int>(columns[1]);
			TargetCategory = TableParse.Array<EArtifactCategory>(columns[2]);
			TargetGrade = TableParse.Array<EGoodsGradeType>(columns[3]);
			SkillType = TableParse.Enum<EEmployeeSkillType>(columns[4]);
			Value = TableParse.Float(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, TargetPrefix, TargetCategory, TargetGrade, SkillType, Value);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,TargetPrefix,TargetCategory,TargetGrade,SkillType,Value";
		}

		public static string GetTypesCsv()
		{
			return "int,int[],EArtifactCategory[],EGoodsGradeType[],EEmployeeSkillType,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EmployeeSkillTable();
			table.Load(new CsvTableLoader<int, TEmployeeSkill>(text));
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
				foreach (TEmployeeSkill row in Table.Values)
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
			return this.ValidateRefKey("TargetPrefix", typeof(TArtifactPrefix))
				&& this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText));
		}
	}
}

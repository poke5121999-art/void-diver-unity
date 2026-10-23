using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TVariantDifficulty : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static VariantDifficultyTable Table { get; set; } = new VariantDifficultyTable();

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

		public float MonsterHpPercent { get; set; }

		public float MonsterAtkPercent { get; set; }

		public float MonsterDefPercent { get; set; }

		public float TrapAtkPercent { get; set; }

		public float UserExpPercent { get; set; }

		public float CooldownReductionPercent { get; set; }

		public float GlobalSkillCoolTimePercent { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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
			MonsterHpPercent = TableParse.Float(columns[1]);
			MonsterAtkPercent = TableParse.Float(columns[2]);
			MonsterDefPercent = TableParse.Float(columns[3]);
			TrapAtkPercent = TableParse.Float(columns[4]);
			UserExpPercent = TableParse.Float(columns[5]);
			CooldownReductionPercent = TableParse.Float(columns[6]);
			GlobalSkillCoolTimePercent = TableParse.Float(columns[7]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, MonsterHpPercent, MonsterAtkPercent, MonsterDefPercent, TrapAtkPercent, UserExpPercent, CooldownReductionPercent, GlobalSkillCoolTimePercent);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,MonsterHpPercent,MonsterAtkPercent,MonsterDefPercent,TrapAtkPercent,UserExpPercent,CooldownReductionPercent,GlobalSkillCoolTimePercent";
		}

		public static string GetTypesCsv()
		{
			return "int,float,float,float,float,float,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new VariantDifficultyTable();
			table.Load(new CsvTableLoader<int, TVariantDifficulty>(text));
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
			foreach (TVariantDifficulty row in Table.Values)
			{
				if (!row.Validate())
				{
					result = false;
				}
			}
			return result;
		}

		public bool Validate()
		{
			return true;
		}
	}
}

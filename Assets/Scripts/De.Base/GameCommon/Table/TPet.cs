using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TPet : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static PetTable Table { get; set; } = new PetTable();

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

		public float WalkAnimation1xSpeed { get; set; }

		public string CombatSfx { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string CombatText => LocalizationUtility.GetLocalizedText(CombatKey);

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string CombatKey => string.Format("{0}_{1}_{2}", "TPet", "CombatText", Id);

		private string NameKey => string.Format("{0}_{1}_{2}", "TPet", "Name", Id);

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
			WalkAnimation1xSpeed = TableParse.Float(columns[1]);
			CombatSfx = TableParse.Str(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, WalkAnimation1xSpeed, CombatSfx);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,WalkAnimation1xSpeed,CombatSfx";
		}

		public static string GetTypesCsv()
		{
			return "int,float,string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new PetTable();
			table.Load(new CsvTableLoader<int, TPet>(text));
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
				foreach (TPet row in Table.Values)
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
				&& this.ValidateRefKey("CombatKey", typeof(TLocalizedText))
				&& this.ValidateRefSfx("CombatSfx");
		}
	}
}

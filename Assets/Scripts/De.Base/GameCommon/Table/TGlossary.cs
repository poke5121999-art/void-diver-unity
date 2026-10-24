using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TGlossary : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static GlossaryTable Table { get; set; } = new GlossaryTable();

		public static GroupGlossaryTable GroupTable { get; set; } = new GroupGlossaryTable();

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

		public EGlossaryCategory Type { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TGlossary", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TGlossary", "Desc", Id);

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
			Type = TableParse.Enum<EGlossaryCategory>(columns[1]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Type, UnlockConditions);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Type,UnlockConditions";
		}

		public static string GetTypesCsv()
		{
			return "int,EGlossaryCategory,EventCondition[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new GlossaryTable();
			table.Load(new CsvTableLoader<int, TGlossary>(text));
			Table = table;
			GroupTable = new GroupGlossaryTable();
			GroupTable.Init(table.Values.GroupBy(row => row.Type).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TGlossary row in Table.Values)
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
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefEventCondition("UnlockConditions");
		}
	}
}

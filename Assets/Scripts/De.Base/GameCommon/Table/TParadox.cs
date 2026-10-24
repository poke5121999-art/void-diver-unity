using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TParadox : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ParadoxTable Table { get; set; } = new ParadoxTable();

		public static GroupParadoxTable GroupTable { get; set; } = new GroupParadoxTable();

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

		public int Level { get; set; }

		public int DebuffId { get; set; }

		public bool IsPositive { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		private string NameKey => string.Format("{0}_{1}_{2}", "TParadox", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TParadox", "Desc", Id);

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
			Level = TableParse.Int(columns[1]);
			DebuffId = TableParse.Int(columns[2]);
			IsPositive = TableParse.Bool(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Level, DebuffId, IsPositive);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Level,DebuffId,IsPositive";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ParadoxTable();
			table.Load(new CsvTableLoader<int, TParadox>(text));
			Table = table;
			GroupTable = new GroupParadoxTable();
			GroupTable.Init(table.Values.GroupBy(row => row.Level).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TParadox row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		private string GetDesc()
		{
			if (TLocalizedText.Table.TryGetValue(DescFormatKey, out var data))
			{
				return data.FormatWithExpression();
			}
			return DescFormatKey;
		}

		public bool Validate()
		{
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescFormatKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DebuffId", typeof(TBuff));
		}
	}
}

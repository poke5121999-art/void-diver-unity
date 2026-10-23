using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEquipmentSet : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EquipmentSetTable Table { get; set; }

		public static SubEquipmentSetTable SubTable { get; set; }

		public static GroupEquipmentSetTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public int Count { get; set; }

		public int BuffId { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TEquipmentSet", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TEquipmentSet", "Desc", Id);

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
			GroupId = TableParse.Int(columns[1]);
			Count = TableParse.Int(columns[2]);
			BuffId = TableParse.Int(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, Count, BuffId);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,Count,BuffId";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EquipmentSetTable();
			table.Load(new CsvTableLoader<int, TEquipmentSet>(text));
			Table = table;
			GroupTable = new GroupEquipmentSetTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
			SubTable = new SubEquipmentSetTable();
			SubTable.Init(table.Values.Select(row => (row.GroupId, row.Count, row)));
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
	}
}

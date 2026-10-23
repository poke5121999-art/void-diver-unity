using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TAreaDecoration : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static AreaDecorationTable Table { get; set; }

		public static GroupAreaDecorationTable GroupTable { get; set; }

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

		public int BitFlagIndex { get; set; }

		public int GroupId { get; set; }

		public int Index { get; set; }

		public EAreaDecorationType AreaDecorationType { get; set; }

		public int AreaId { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TAreaDecoration", "Name", Id);

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
			BitFlagIndex = TableParse.Int(columns[1]);
			GroupId = TableParse.Int(columns[2]);
			Index = TableParse.Int(columns[3]);
			AreaDecorationType = TableParse.Enum<EAreaDecorationType>(columns[4]);
			AreaId = TableParse.Int(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, GroupId, Index, AreaDecorationType, AreaId);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex ,GroupId,Index,AreaDecorationType,AreaId";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int,EAreaDecorationType,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new AreaDecorationTable();
			table.Load(new CsvTableLoader<int, TAreaDecoration>(text));
			Table = table;
			GroupTable = new GroupAreaDecorationTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

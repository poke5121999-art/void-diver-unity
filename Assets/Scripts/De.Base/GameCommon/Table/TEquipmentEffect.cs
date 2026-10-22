using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEquipmentEffect : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EquipmentEffectTable Table { get; set; }

		public static SubEquipmentEffectTable SubTable { get; set; }

		public static GroupEquipmentEffectTable GroupTable { get; set; }

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

		public int Level { get; set; }

		public int BuffId { get; set; }

		public int[] SkillIds { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string DescFormatKey => null;

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
			Level = TableParse.Int(columns[2]);
			BuffId = TableParse.Int(columns[3]);
			SkillIds = TableParse.Array<int>(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, Level, BuffId, SkillIds);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,Level,BuffId,SkillIds";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int,int[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EquipmentEffectTable();
			table.Load(new CsvTableLoader<int, TEquipmentEffect>(text));
			Table = table;
			GroupTable = new GroupEquipmentEffectTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
			SubTable = new SubEquipmentEffectTable();
			SubTable.Init(table.Values.Select(row => (row.GroupId, row.Level, row)));
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

		private string GetDesc()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

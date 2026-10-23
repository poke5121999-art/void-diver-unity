using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacterSkin : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterSkinTable Table { get; set; }

		public static GroupCharacterSkinTable GroupTable { get; set; }

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

		public int CharacterId { get; set; }

		public int BitFlagIndex { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TCharacter", "Name", Id);

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
			CharacterId = TableParse.Int(columns[1]);
			BitFlagIndex = TableParse.Int(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, CharacterId, BitFlagIndex);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,CharacterId,BitFlagIndex";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CharacterSkinTable();
			table.Load(new CsvTableLoader<int, TCharacterSkin>(text));
			Table = table;
			GroupTable = new GroupCharacterSkinTable();
			GroupTable.Init(table.Values.GroupBy(row => row.CharacterId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

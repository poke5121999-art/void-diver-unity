using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacterSkinPreset : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterSkinPresetTable Table { get; set; }

		public static GroupCharacterSkinPresetTable GroupTable { get; set; }

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

		public int CharacterSkinId { get; set; }

		public int SkinBaseId { get; set; }

		public int EyeSkinId { get; set; }

		public int HairSkinId { get; set; }

		public int CostumeSkinId { get; set; }

		public int AccSkinId { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public int CharacterId => 0;

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
			CharacterSkinId = TableParse.Int(columns[1]);
			SkinBaseId = TableParse.Int(columns[2]);
			EyeSkinId = TableParse.Int(columns[3]);
			HairSkinId = TableParse.Int(columns[4]);
			CostumeSkinId = TableParse.Int(columns[5]);
			AccSkinId = TableParse.Int(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, CharacterSkinId, SkinBaseId, EyeSkinId, HairSkinId, CostumeSkinId, AccSkinId);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,CharacterSkinId,SkinBaseId,EyeSkinId,HairSkinId,CostumeSkinId,AccSkinId";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,int,int,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CharacterSkinPresetTable();
			table.Load(new CsvTableLoader<int, TCharacterSkinPreset>(text));
			Table = table;
			GroupTable = new GroupCharacterSkinPresetTable();
			GroupTable.Init(table.Values.GroupBy(row => row.CharacterSkinId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

		public static int GetDefaultSkinPresetId(int characterId)
		{
			return 0;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

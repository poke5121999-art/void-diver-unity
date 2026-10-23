using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacterGroundMoveSfx : ITableData<EGroundType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterGroundMoveSfxTable Table { get; set; } = new CharacterGroundMoveSfxTable();

		public EGroundType Pk
		{
			get
			{
				return GroundType;
			}
			set
			{
			}
		}

		public EGroundType GroundType { get; set; }

		public string RunLeft { get; set; }

		public string RunRight { get; set; }

		public string WalkLeft { get; set; }

		public string WalkRight { get; set; }

		public Action<ITableData<EGroundType>> OnDataLoaded { get; set; }

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
			GroundType = TableParse.Enum<EGroundType>(columns[0]);
			RunLeft = TableParse.Str(columns[1]);
			RunRight = TableParse.Str(columns[2]);
			WalkLeft = TableParse.Str(columns[3]);
			WalkRight = TableParse.Str(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(GroundType, RunLeft, RunRight, WalkLeft, WalkRight);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "GroundType,RunLeft,RunRight,WalkLeft,WalkRight";
		}

		public static string GetTypesCsv()
		{
			return "EGroundType,string,string,string,string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new CharacterGroundMoveSfxTable();
			table.Load(new CsvTableLoader<EGroundType, TCharacterGroundMoveSfx>(text));
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
				foreach (TCharacterGroundMoveSfx row in Table.Values)
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
			return this.ValidateRefSfx("RunLeft")
				&& this.ValidateRefSfx("RunRight")
				&& this.ValidateRefSfx("WalkLeft")
				&& this.ValidateRefSfx("WalkRight");
		}
	}
}

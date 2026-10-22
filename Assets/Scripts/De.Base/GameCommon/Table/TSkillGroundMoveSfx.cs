using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSkillGroundMoveSfx : ITableData<EGroundType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SkillGroundMoveSfxTable Table { get; set; }

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

		public string[] SfxNames { get; set; }

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
			SfxNames = TableParse.Array<string>(columns[1]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(GroundType, SfxNames);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "GroundType,SfxNames";
		}

		public static string GetTypesCsv()
		{
			return "EGroundType,string[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new SkillGroundMoveSfxTable();
			table.Load(new CsvTableLoader<EGroundType, TSkillGroundMoveSfx>(text));
			Table = table;
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

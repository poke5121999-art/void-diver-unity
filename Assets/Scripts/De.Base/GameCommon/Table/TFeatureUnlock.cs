using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TFeatureUnlock : ITableData<EFeatureUnlock>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static FeatureUnlockTable Table { get; set; }

		public EFeatureUnlock Pk
		{
			get
			{
				return Feature;
			}
			set
			{
			}
		}

		public EFeatureUnlock Feature { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public Action<ITableData<EFeatureUnlock>> OnDataLoaded { get; set; }

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
			Feature = TableParse.Enum<EFeatureUnlock>(columns[0]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[1]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Feature, UnlockConditions);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Feature,UnlockConditions";
		}

		public static string GetTypesCsv()
		{
			return "EFeatureUnlock,EventCondition[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new FeatureUnlockTable();
			table.Load(new CsvTableLoader<EFeatureUnlock, TFeatureUnlock>(text));
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

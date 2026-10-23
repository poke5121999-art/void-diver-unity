using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TStatusEffectSuppression : ITableData<EStatusEffectTag>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static StatusEffectSuppressionTable Table { get; set; } = new StatusEffectSuppressionTable();

		public EStatusEffectTag Pk
		{
			get
			{
				return Tag;
			}
			set
			{
			}
		}

		public EStatusEffectTag Tag { get; set; }

		public float Value { get; set; }

		public float ValuePerDurationSec { get; set; }

		public Action<ITableData<EStatusEffectTag>> OnDataLoaded { get; set; }

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
			Tag = TableParse.Enum<EStatusEffectTag>(columns[0]);
			Value = TableParse.Float(columns[1]);
			ValuePerDurationSec = TableParse.Float(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Tag, Value, ValuePerDurationSec);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Tag,Value,ValuePerDurationSec";
		}

		public static string GetTypesCsv()
		{
			return "EStatusEffectTag,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new StatusEffectSuppressionTable();
			table.Load(new CsvTableLoader<EStatusEffectTag, TStatusEffectSuppression>(text));
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
			foreach (TStatusEffectSuppression row in Table.Values)
			{
				if (!row.Validate())
				{
					result = false;
				}
			}
			return result;
		}

		public bool Validate()
		{
			return true;
		}
	}
}

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

		public static StatusEffectSuppressionTable Table { get; set; }

		public EStatusEffectTag Pk
		{
			get
			{
				return EStatusEffectTag.None;
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
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
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

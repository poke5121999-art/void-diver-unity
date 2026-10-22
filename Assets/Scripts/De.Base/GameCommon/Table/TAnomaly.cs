using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TAnomaly : ITableData<EAnomalyType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static AnomalyTable Table { get; set; }

		public EAnomalyType Pk
		{
			get
			{
				return EAnomalyType.None;
			}
			set
			{
			}
		}

		public EAnomalyType Type { get; set; }

		public int Weight { get; set; }

		public int[] PlayerBuffIds { get; set; }

		public int[] MonsterBuffIds { get; set; }

		public EAnomalyEffectType[] EffectTypes { get; set; }

		public string AmbientSfx { get; set; }

		public Action<ITableData<EAnomalyType>> OnDataLoaded { get; set; }

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

using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TStress : ITableData<EStressState>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static StressTable Table { get; set; }

		public EStressState Pk
		{
			get
			{
				return EStressState.Alert;
			}
			set
			{
			}
		}

		public EStressState State { get; set; }

		public int StressMax { get; set; }

		public int[] DebuffIds { get; set; }

		public int IntervalDebuffId { get; set; }

		public float DebuffInterval { get; set; }

		public float IntervalDebuffStartDelay { get; set; }

		public Action<ITableData<EStressState>> OnDataLoaded { get; set; }

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

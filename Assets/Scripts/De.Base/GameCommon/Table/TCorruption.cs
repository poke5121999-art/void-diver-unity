using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCorruption : ITableData<ECorruptionState>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CorruptionTable Table { get; set; }

		public ECorruptionState Pk
		{
			get
			{
				return ECorruptionState.Safe;
			}
			set
			{
			}
		}

		public ECorruptionState State { get; set; }

		public int Threshold { get; set; }

		public int[] DebuffIds { get; set; }

		public float StressDamageIntervalTime { get; set; }

		public float StressDamageAmount { get; set; }

		public Action<ITableData<ECorruptionState>> OnDataLoaded { get; set; }

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

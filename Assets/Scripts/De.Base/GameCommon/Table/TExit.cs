using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TExit : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ExitTable Table { get; set; }

		public int Pk
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int Id { get; set; }

		public EExitType Type { get; set; }

		public int WaveId { get; set; }

		public int SpecialFieldId { get; set; }

		public string ActivatingStateHoldingSfx { get; set; }

		public string HoldingSfx { get; set; }

		public string EscapeSfx { get; set; }

		public bool DisplayOnMinimapAtStart { get; set; }

		public bool UseExitCostTable { get; set; }

		public GoodsData CustomUnlockGoodsData { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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

		public GoodsData GetUnlockGoodsData(int level)
		{
			return default;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

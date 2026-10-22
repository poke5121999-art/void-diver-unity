using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEmployeeLevel : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EmployeeLevelTable Table { get; set; }

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

		public int Level { get; set; }

		public long NeedExp { get; set; }

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

		public bool Validate()
		{
			return false;
		}
	}
}

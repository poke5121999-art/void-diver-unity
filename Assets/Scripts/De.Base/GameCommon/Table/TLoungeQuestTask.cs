using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TLoungeQuestTask : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static LoungeQuestTaskTable Table { get; set; }

		public static GroupLoungeQuestTaskTable GroupTable { get; set; }

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

		public int LoungeQuestId { get; set; }

		public EventCondition EventCondition { get; set; }

		public long Goal { get; set; }

		public bool DisplayInHud { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Desc => null;

		private string DescKey => null;

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

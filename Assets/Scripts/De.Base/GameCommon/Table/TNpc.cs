using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TNpc : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static NpcTable Table { get; set; }

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

		public ENpcType Type { get; set; }

		public int BitFlagIndex { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public string DefaultImageName { get; set; }

		public bool ShowNewContentPopup { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		[Localized]
		public string SubName => null;

		[Localized]
		public string ToastText => null;

		[Localized]
		public string DefaultDialog => null;

		private string NameKey => null;

		private string DescKey => null;

		private string SubNameKey => null;

		private string ToastTextKey => null;

		private string DefaultDialogKey => null;

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

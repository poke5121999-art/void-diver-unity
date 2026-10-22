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
				return EFeatureUnlock.None;
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

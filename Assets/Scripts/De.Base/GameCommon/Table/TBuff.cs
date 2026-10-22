using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TBuff : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		private static int[] _despairBuffIds;

		public static BuffTable Table { get; set; }

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

		public string Memo { get; set; }

		public int MaxStackCount { get; set; }

		public float Duration { get; set; }

		public EStatusEffectTag EffectTag { get; set; }

		public BuffEffect[] BuffEffects { get; set; }

		public bool ShowBuffIcon { get; set; }

		public bool ShowActiveText { get; set; }

		public BuffTriggerBase[] Triggers { get; set; }

		public bool InactiveInLounge { get; set; }

		public bool IsDespairEffect { get; set; }

		public bool ExpirePerStack { get; set; }

		public BuffDurationOverride DurationOverride { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		private string NameKey => null;

		private string DescFormatKey => null;

		public static IReadOnlyList<int> DespairBuffIds => null;

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

		static TBuff()
		{
		}

		private static void RebuildDespairCache()
		{
		}

		private string GetDesc()
		{
			return null;
		}

		public static string ResolveExpression(string expression)
		{
			return null;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}
	}
}

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
				return Id;
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
				m_OnTableChanged += value;
			}
			[CompilerGenerated]
			remove
			{
				m_OnTableChanged -= value;
			}
		}

		public void Parse(string[] columns)
		{
			Id = TableParse.Int(columns[0]);
			Memo = TableParse.Str(columns[1]);
			MaxStackCount = TableParse.Int(columns[2]);
			Duration = TableParse.Float(columns[3]);
			EffectTag = TableParse.Enum<EStatusEffectTag>(columns[4]);
			BuffEffects = TableParse.Json<BuffEffect[]>(columns[5]);
			ShowBuffIcon = TableParse.Bool(columns[6]);
			ShowActiveText = TableParse.Bool(columns[7]);
			Triggers = TableParse.Json<BuffTriggerBase[]>(columns[8]);
			InactiveInLounge = TableParse.Bool(columns[9]);
			IsDespairEffect = TableParse.Bool(columns[10]);
			ExpirePerStack = TableParse.Bool(columns[11]);
			DurationOverride = TableParse.Json<BuffDurationOverride>(columns[12]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, MaxStackCount, Duration, EffectTag, BuffEffects, ShowBuffIcon, ShowActiveText, Triggers, InactiveInLounge, IsDespairEffect, ExpirePerStack, DurationOverride);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,MaxStackCount,Duration,EffectTag,BuffEffects,ShowBuffIcon,ShowActiveText,Triggers,InactiveInLounge,IsDespairEffect,ExpirePerStack,DurationOverride";
		}

		public static string GetTypesCsv()
		{
			return "int,string,int,float,EStatusEffectTag,BuffEffect[],bool,bool,BuffTriggerBase[],bool,bool,bool,BuffDurationOverride";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new BuffTable();
			table.Load(new CsvTableLoader<int, TBuff>(text));
			Table = table;
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
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

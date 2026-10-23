using System;
using System.Collections.Generic;
using System.Linq;
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

		private static int[] _despairBuffIds = Array.Empty<int>();

		public static BuffTable Table { get; set; } = new BuffTable();

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
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		private string NameKey => string.Format("{0}_{1}_{2}", "TBuff", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TBuff", "Desc", Id);

		public static IReadOnlyList<int> DespairBuffIds => _despairBuffIds;

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
			bool result = true;
			if (Table != null)
			{
				foreach (TBuff row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		static TBuff()
		{
			OnTableChanged += RebuildDespairCache;
		}

		private static void RebuildDespairCache()
		{
			_despairBuffIds = Table == null ? Array.Empty<int>() : Table.Values.Where(b => b.IsDespairEffect).Select(b => b.Id).ToArray();
		}

		private string GetDesc()
		{
			if (TLocalizedText.Table.TryGetValue(DescFormatKey, out var data))
			{
				return data.FormatWithExpression();
			}
			return DescFormatKey;
		}

		public static string ResolveExpression(string expression)
		{
			int colon = expression.IndexOf(':');
			if (colon < 0 || !int.TryParse(expression.Substring(0, colon), out int id))
			{
				return "[?id]";
			}
			if (!Table.TryGetValue(id, out TBuff buff))
			{
				return string.Format("[?{0}]", id);
			}
			string field = expression.Substring(colon + 1);
			int dot = field.IndexOf('.');
			if (dot < 0)
			{
				if (field.Equals("Duration"))
				{
					return buff.Duration.ToFloatString();
				}
				if (field.Equals("MaxStackCount"))
				{
					return string.Format("{0}", buff.MaxStackCount);
				}
			}
			else
			{
				string head = field.Substring(0, dot);
				string tail = field.Substring(dot + 1);
				int bracket = head.IndexOf('[');
				if (bracket >= 0 && head.EndsWith("]") && int.TryParse(head.Substring(bracket + 1, head.Length - (bracket + 1) - 1), out int index))
				{
					string name = head.Substring(0, bracket);
					if (name.Equals("BuffEffects", StringComparison.OrdinalIgnoreCase))
					{
						if (buff.BuffEffects != null && index < buff.BuffEffects.Length && buff.BuffEffects[index].TryResolveExpression(tail, out string result))
						{
							return result;
						}
					}
					else if (name.Equals("Triggers", StringComparison.OrdinalIgnoreCase))
					{
						if (buff.Triggers != null && index < buff.Triggers.Length && buff.Triggers[index].TryResolveExpression(tail, out string result))
						{
							return result;
						}
					}
				}
			}
			return "[?field]";
		}

		public string[] ExtractExpressions()
		{
			var list = new List<string>();
			string prefix = string.Format("buff:{0}:", Id);
			list.Add(prefix + "Duration");
			list.Add(prefix + "MaxStackCount");
			if (BuffEffects != null)
			{
				for (int i = 0; i < BuffEffects.Length; i++)
				{
					string effectPrefix = prefix + string.Format("BuffEffects[{0}].", i);
					foreach (string expression in BuffEffects[i].ExtractExpressions())
					{
						list.Add(effectPrefix + expression);
					}
				}
			}
			if (Triggers != null)
			{
				for (int i = 0; i < Triggers.Length; i++)
				{
					string triggerPrefix = prefix + string.Format("Triggers[{0}].", i);
					foreach (string expression in Triggers[i].ExtractExpressions())
					{
						list.Add(triggerPrefix + expression);
					}
				}
			}
			return list.ToArray();
		}

		public bool Validate()
		{
			bool result = this.Check(MaxStackCount > 0, string.Format("{0}: {1} must be > 0", "MaxStackCount", MaxStackCount))
				&& this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescFormatKey", typeof(TLocalizedText));
			if (BuffEffects != null)
			{
				foreach (BuffEffect effect in BuffEffects)
				{
					if (effect is FrontGuardEffect frontGuardEffect && !ValidationExtensions.ContainsSfx(frontGuardEffect.Sfx))
					{
						this.LogError(string.Concat("MasterAudio not contains \"[BuffEffects] FrontGuardEffect.Sfx ", frontGuardEffect.Sfx, "\""));
						result = false;
					}
				}
			}
			return result;
		}
	}
}

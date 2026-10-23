using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSkill : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SkillTable Table { get; set; } = new SkillTable();

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

		public ESkillType SkillType { get; set; }

		public bool RequireWeapon { get; set; }

		public ESkillTagType[] SkillTags { get; set; }

		public bool NotifySkillReady { get; set; }

		public EHitTarget TargetType { get; set; }

		public int StressCost { get; set; }

		public int HpCost { get; set; }

		public int StaminaCost { get; set; }

		public int ChargeCost { get; set; }

		public float ChargePerSec { get; set; }

		public float CoolTime { get; set; }

		public int StackCount { get; set; }

		public float StackChargeInterval { get; set; }

		public string ActionSfx { get; set; }

		public bool IsBasicAttack { get; set; }

		public bool IgnoreSilence { get; set; }

		public bool UseIcon { get; set; }

		public SkillActionNode RootActionNode { get; set; }

		public AiSkillCondition AiSkillCondition { get; set; }

		public SkillBuffStackCondition DeactivateBuffCondition { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		private string NameKey => string.Format("{0}_{1}_{2}", "TSkill", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TSkill", "Desc", Id);

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
			SkillType = TableParse.Enum<ESkillType>(columns[2]);
			RequireWeapon = TableParse.Bool(columns[3]);
			SkillTags = TableParse.Array<ESkillTagType>(columns[4]);
			NotifySkillReady = TableParse.Bool(columns[5]);
			TargetType = TableParse.Enum<EHitTarget>(columns[6]);
			StressCost = TableParse.Int(columns[7]);
			HpCost = TableParse.Int(columns[8]);
			StaminaCost = TableParse.Int(columns[9]);
			ChargeCost = TableParse.Int(columns[10]);
			ChargePerSec = TableParse.Float(columns[11]);
			CoolTime = TableParse.Float(columns[12]);
			StackCount = TableParse.Int(columns[13]);
			StackChargeInterval = TableParse.Float(columns[14]);
			ActionSfx = TableParse.Str(columns[15]);
			IsBasicAttack = TableParse.Bool(columns[16]);
			IgnoreSilence = TableParse.Bool(columns[17]);
			UseIcon = TableParse.Bool(columns[18]);
			RootActionNode = TableParse.Json<SkillActionNode>(columns[19]);
			AiSkillCondition = TableParse.Json<AiSkillCondition>(columns[20]);
			DeactivateBuffCondition = TableParse.Json<SkillBuffStackCondition>(columns[21]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, SkillType, RequireWeapon, SkillTags, NotifySkillReady, TargetType, StressCost, HpCost, StaminaCost, ChargeCost, ChargePerSec, CoolTime, StackCount, StackChargeInterval, ActionSfx, IsBasicAttack, IgnoreSilence, UseIcon, RootActionNode, AiSkillCondition, DeactivateBuffCondition);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,SkillType,RequireWeapon,SkillTags,NotifySkillReady,TargetType,StressCost,HpCost,StaminaCost,ChargeCost,ChargePerSec,CoolTime,StackCount,StackChargeInterval,ActionSfx,IsBasicAttack,IgnoreSilence,UseIcon,RootActionNode,AiSkillCondition,DeactivateBuffCondition";
		}

		public static string GetTypesCsv()
		{
			return "int,string,ESkillType,bool,ESkillTagType[],bool,EHitTarget,int,int,int,int,float,float,int,float,string,bool,bool,bool,SkillActionNode,AiSkillCondition,SkillBuffStackCondition";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new SkillTable();
			table.Load(new CsvTableLoader<int, TSkill>(text));
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
				foreach (TSkill row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
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
			if (!Table.TryGetValue(id, out TSkill skill))
			{
				return string.Format("[?{0}]", id);
			}
			string field = expression.Substring(colon + 1);
			int dot = field.IndexOf('.');
			if (dot < 0)
			{
				switch (field.ToLower())
				{
				case "stresscost":
					return string.Format("{0}", skill.StressCost);
				case "hpcost":
					return string.Format("{0}", skill.HpCost);
				case "staminacost":
					return string.Format("{0}", skill.StaminaCost);
				case "chargecost":
					return string.Format("{0}", skill.ChargeCost);
				case "chargepersec":
					return skill.ChargePerSec.ToFloatString();
				case "cooltime":
					return skill.CoolTime.ToFloatString();
				case "stackcount":
					return string.Format("{0}", skill.StackCount);
				case "stackchargeinterval":
					return skill.StackChargeInterval.ToFloatString();
				default:
					return "[?field]";
				}
			}
			string head = field.Substring(0, dot);
			string tail = field.Substring(dot + 1);
			if (!head.Equals("RootActionNode", StringComparison.OrdinalIgnoreCase))
			{
				return "[?field]";
			}
			if (skill.RootActionNode == null)
			{
				return "[?field]";
			}
			if (skill.RootActionNode.TryResolveExpression(tail, out string result))
			{
				return result;
			}
			return "[?field]";
		}

		public string[] ExtractExpressions(long nodeUniqueId, SkillAction skillAction)
		{
			var list = new List<string>();
			if (RootActionNode != null && TryFindNodePath(RootActionNode, nodeUniqueId, "RootActionNode", out string path))
			{
				string prefix = string.Format("skill:{0}:{1}.", Id, path);
				foreach (string expression in skillAction.ExtractExpressions())
				{
					list.Add(prefix + expression);
				}
				if (skillAction.actionEvents != null)
				{
					for (int i = 0; i < skillAction.actionEvents.Length; i++)
					{
						string eventPrefix = prefix + string.Format("ActionEvents[{0}].", i);
						foreach (string expression in skillAction.actionEvents[i].ExtractExpressions())
						{
							list.Add(eventPrefix + expression);
						}
					}
				}
			}
			return list.ToArray();
		}

		private static bool TryFindNodePath(SkillActionNode node, long targetUniqueId, string currentPath, out string foundPath)
		{
			if (node.UniqueId == targetUniqueId)
			{
				foundPath = currentPath;
				return true;
			}
			for (int i = 0; i < node.childNodes.Length; i++)
			{
				if (TryFindNodePath(node.childNodes[i], targetUniqueId, string.Format("{0}.ChildNodes[{1}]", currentPath, i), out foundPath))
				{
					return true;
				}
			}
			foundPath = null;
			return false;
		}

		public bool Validate()
		{
			bool result = this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescFormatKey", typeof(TLocalizedText))
				&& this.ValidateRefSfx("ActionSfx");
			if (RootActionNode != null)
			{
				string errorMessage = null;
				if (!RootActionNode.Validate(out errorMessage))
				{
					this.LogError("RootActionNode: " + errorMessage.TrimEnd());
					result = false;
				}
			}
			return result;
		}
	}
}

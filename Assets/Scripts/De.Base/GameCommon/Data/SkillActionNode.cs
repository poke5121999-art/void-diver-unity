using System;
using GameCommon.Enum;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillActionNode
	{
		public SkillAction skillAction = new SkillAction();

		public SkillActionNode[] childNodes;

		[JsonIgnore]
		public bool HasChild => childNodes != null && childNodes.Length != 0;

		public int SkillId => skillAction.SkillId;

		public long UniqueId => skillAction.UniqueId;

		public ESkillType SkillType => skillAction.SkillType;

		[JsonIgnore]
		public string Text
		{
			get
			{
				return skillAction.text;
			}
			set
			{
				skillAction.text = value;
			}
		}

		[JsonIgnore]
		public SkillTrigger SkillTrigger
		{
			get
			{
				return skillAction.SkillTrigger;
			}
			set
			{
				skillAction.SkillTrigger = value;
			}
		}

		private void OnAddChildNode()
		{
			if (GUILayout.Button("+", GUILayout.Width(20f)))
			{
				int length = ((childNodes != null) ? childNodes.Length : 0);
				SkillActionNode[] array = new SkillActionNode[length + 1];
				if (childNodes != null)
				{
					childNodes.CopyTo(array, 0);
				}
				SkillActionNode skillActionNode = new SkillActionNode();
				skillActionNode.SetSkillId(skillAction.SkillId);
				skillActionNode.SetSkillType(skillAction.SkillType);
				array[length] = skillActionNode;
				childNodes = array;
			}
		}

		private void OnChildNodesChanged()
		{
			SetSkillId(skillAction.SkillId);
			SetSkillType(skillAction.SkillType);
			Debug.Log("childNodes 배열이 변경되었습니다!");
		}

		public SkillActionNode FindNode(long uniqueId)
		{
			if (skillAction.UniqueId == uniqueId)
			{
				return this;
			}
			if (childNodes != null)
			{
				for (int i = 0; i < childNodes.Length; i++)
				{
					SkillActionNode skillActionNode = childNodes[i].FindNode(uniqueId);
					if (skillActionNode != null)
					{
						return skillActionNode;
					}
				}
			}
			return null;
		}

		public void SetSkillId(int skillId)
		{
			skillAction.SkillId = skillId;
			if (childNodes != null)
			{
				for (int i = 0; i < childNodes.Length; i++)
				{
					childNodes[i].SetSkillId(skillId);
				}
			}
		}

		public void SetSkillType(ESkillType skillType)
		{
			skillAction.SkillType = skillType;
			if (childNodes != null)
			{
				for (int i = 0; i < childNodes.Length; i++)
				{
					childNodes[i].SetSkillType(skillType);
				}
			}
		}

		public void RefreshUniqueIds()
		{
			skillAction.RefreshUniqueId();
			if (childNodes != null)
			{
				for (int i = 0; i < childNodes.Length; i++)
				{
					childNodes[i].RefreshUniqueIds();
				}
			}
		}

		public bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			if (text.StartsWith("childnodes["))
			{
				const string prefix = "childnodes[";
				int close = expression.IndexOf(']', prefix.Length);
				if (close > prefix.Length)
				{
					string indexText = expression.Substring(prefix.Length, close - prefix.Length);
					if (int.TryParse(indexText, out int index) && childNodes != null && index < childNodes.Length && close + 2 < expression.Length && expression[close + 1] == '.')
					{
						string rest = expression.Substring(close + 2, expression.Length - (close + 2));
						return childNodes[index].TryResolveExpression(rest, out result);
					}
				}
				result = null;
				return false;
			}
			return skillAction.TryResolveExpression(expression, out result);
		}

		public string[] ExtractExpressions()
		{
			return skillAction.ExtractExpressions();
		}

		public bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (skillAction != null && !skillAction.Validate(out string actionMessage))
			{
				isValid = false;
				stringBuilder.AppendLine(actionMessage);
			}
			if (childNodes != null)
			{
				childNodes.ForEach(delegate(SkillActionNode _)
				{
					if (!_.Validate(out string childMessage))
					{
						isValid = false;
						stringBuilder.AppendLine(childMessage);
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

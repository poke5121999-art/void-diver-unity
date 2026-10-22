using System;
using GameCommon.Enum;
using Newtonsoft.Json;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillActionNode
	{
		public SkillAction skillAction;

		public SkillActionNode[] childNodes;

		[JsonIgnore]
		public bool HasChild => false;

		public int SkillId => 0;

		public long UniqueId => 0L;

		public ESkillType SkillType => ESkillType.None;

		[JsonIgnore]
		public string Text
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		[JsonIgnore]
		public SkillTrigger SkillTrigger
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		private void OnAddChildNode()
		{
		}

		private void OnChildNodesChanged()
		{
		}

		public SkillActionNode FindNode(long uniqueId)
		{
			return null;
		}

		public void SetSkillId(int skillId)
		{
		}

		public void SetSkillType(ESkillType skillType)
		{
		}

		public void RefreshUniqueIds()
		{
		}

		public bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}

		public bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

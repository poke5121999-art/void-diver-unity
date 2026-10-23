using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxCollectActionEvent : ActionEvent
	{
		[FormerlySerializedAs("TargetableHitBoxIds")]
		public int[] HitBoxIds = Array.Empty<int>();

		public float CollectingHitBoxSpeed;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			result = (text == "collectinghitboxspeed") ? CollectingHitBoxSpeed.ToFloatString() : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "collectinghitboxspeed" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				isValid = false;
			}
			if (HitBoxIds != null)
			{
				Array.ForEach(HitBoxIds, delegate(int _)
				{
					if (!THitBox.Table.ContainsKey(_))
					{
						isValid = false;
						stringBuilder.AppendLine(string.Format("{0} HitBoxId [{1}] is not exist in  {2}", "HitBoxIds", _, "THitBox"));
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

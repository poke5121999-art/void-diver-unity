using System.Text;
using GameCommon.Table;
using System.Linq;
using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class SpecialFieldActionEvent : ActionEvent
	{
		public int SpecialFieldId;

		public Vector3 SpawnOffset;

		public ESpecialFieldShapeType ShapeType;

		public float Radius = 1f;

		public float ScaleX = 1f;

		public float ScaleZ = 1f;

		public Vector3 Scale
		{
			get
			{
				switch (ShapeType)
				{
				case ESpecialFieldShapeType.Circle:
					return new Vector3(Radius * 2f, 1f, Radius * 2f);
				case ESpecialFieldShapeType.Rectangle:
					return new Vector3(ScaleX, 1f, ScaleZ);
				default:
					return Vector3.one;
				}
			}
		}

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "specialfieldid")
			{
				result = string.Format("{0}", SpecialFieldId);
			}
			else if (text == "radius")
			{
				result = Radius.ToFloatString();
			}
			else if (text == "scalex")
			{
				result = ScaleX.ToFloatString();
			}
			else if (text == "scalez")
			{
				result = ScaleZ.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[4] { "specialfieldid", "radius", "scalex", "scalez" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = true;
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (!TSpecialField.Table.ContainsKey(SpecialFieldId))
			{
				isValid = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "SpecialFieldId", SpecialFieldId, "TSpecialField"));
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}

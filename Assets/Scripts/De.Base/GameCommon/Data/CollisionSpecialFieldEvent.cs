using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionSpecialFieldEvent : CollisionEvent
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
				if (ShapeType == ESpecialFieldShapeType.Circle)
				{
					return new Vector3(Radius * 2f, 1f, Radius * 2f);
				}
				if (ShapeType == ESpecialFieldShapeType.Rectangle)
				{
					return new Vector3(ScaleX, 1f, ScaleZ);
				}
				return Vector3.one;
			}
		}

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "SpecialFieldId")
			{
				result = string.Format("{0}", SpecialFieldId);
			}
			else if (expression == "Radius")
			{
				result = Radius.ToFloatString();
			}
			else if (expression == "ScaleX")
			{
				result = ScaleX.ToFloatString();
			}
			else if (expression == "ScaleZ")
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
			return Array.Empty<string>().Concat(new string[4] { "SpecialFieldId", "Radius", "ScaleX", "ScaleZ" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				flag = false;
			}
			if (!TSpecialField.Table.ContainsKey(SpecialFieldId))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("{0}.{1} [{2}] is not exist in  {3}", "CollisionSpecialFieldEvent", "SpecialFieldId", SpecialFieldId, "TSpecialField"));
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}

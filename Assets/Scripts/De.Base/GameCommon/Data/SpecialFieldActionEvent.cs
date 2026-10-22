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

		public float Radius;

		public float ScaleX;

		public float ScaleZ;

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
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}

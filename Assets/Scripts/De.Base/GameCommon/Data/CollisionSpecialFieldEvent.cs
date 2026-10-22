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

		public float Radius;

		public float ScaleX;

		public float ScaleZ;

		public Vector3 Scale => default;

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

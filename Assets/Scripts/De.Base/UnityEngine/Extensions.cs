using System.Collections.Generic;
using GameCommon.Data;

namespace UnityEngine
{
	public static class Extensions
	{
		public static bool IsHighObstacle(this Collider collider)
		{
			return false;
		}

		public static bool IsBreakableProp(this Collider collider)
		{
			return false;
		}

		public static bool IsHitableObject(this Collider collider)
		{
			return false;
		}

		public static ProtoVector2 ToProtoVector2(this Vector2 value)
		{
			return null;
		}

		public static ProtoVector3 ToProtoVector3(this Vector3 value)
		{
			return null;
		}

		public static Vector2 ToVector2(this ProtoVector2 value)
		{
			return default;
		}

		public static Vector3 ToVector3(this ProtoVector3 value)
		{
			return default;
		}

		public static bool TryConvertToVector3(this string s, out Vector3 result)
		{
			result = default;
			return false;
		}

		public static T GetOrAddComponent<T>(this Component self) where T : Component
		{
			return null;
		}

		public static void DestroyIfExist<T>(this Component self) where T : Component
		{
		}

		public static void Flip25D(this Transform tf)
		{
		}

		public static List<Transform> GetParents(this Transform child)
		{
			return null;
		}

		public static void SetFullStretch(this RectTransform tf)
		{
		}
	}
}

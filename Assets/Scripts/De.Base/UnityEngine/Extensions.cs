using System.Collections.Generic;
using System.Globalization;
using GameCommon.Data;

namespace UnityEngine
{
	public static class Extensions
	{
		public static bool IsHighObstacle(this Collider collider)
		{
			return collider.gameObject.layer == Const.Layer.HIGH_OBSTACLE;
		}

		public static bool IsBreakableProp(this Collider collider)
		{
			return collider.gameObject.layer == Const.Layer.BREAKABLE_PROP;
		}

		public static bool IsHitableObject(this Collider collider)
		{
			if (collider.gameObject.layer == Const.Layer.UNIT)
			{
				return true;
			}
			return collider.gameObject.layer == Const.Layer.BREAKABLE_PROP;
		}

		public static ProtoVector2 ToProtoVector2(this Vector2 value)
		{
			return new ProtoVector2
			{
				X = value.x,
				Y = value.y
			};
		}

		public static ProtoVector3 ToProtoVector3(this Vector3 value)
		{
			return new ProtoVector3
			{
				X = value.x,
				Y = value.y,
				Z = value.z
			};
		}

		public static Vector2 ToVector2(this ProtoVector2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		public static Vector3 ToVector3(this ProtoVector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		public static bool TryConvertToVector3(this string s, out Vector3 result)
		{
			result = Vector3.zero;
			if (string.IsNullOrWhiteSpace(s))
			{
				return false;
			}
			// TODO(decomp): the 3-char trim set is read from a static data blob; '(' ')' ' ' assumed @0x1801f5830
			string[] parts = s.Trim('(', ')', ' ').Split(',');
			if (parts.Length == 3
				&& float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x)
				&& float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y)
				&& float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
			{
				result = new Vector3(x, y, z);
				return true;
			}
			return false;
		}

		// TODO(decomp): generic method has no pseudo-C in the binary
		public static T GetOrAddComponent<T>(this Component self) where T : Component
		{
			T component = self.GetComponent<T>();
			if (component == null)
			{
				component = self.gameObject.AddComponent<T>();
			}
			return component;
		}

		// TODO(decomp): generic method has no pseudo-C in the binary
		public static void DestroyIfExist<T>(this Component self) where T : Component
		{
			T component = self.GetComponent<T>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}

		public static void Flip25D(this Transform tf)
		{
			if (tf == null)
			{
				return;
			}
			tf.localEulerAngles = new Vector3(0f, -90f, 0f);
			tf.localScale = new Vector3(-1f, 1f, 1f);
		}

		public static List<Transform> GetParents(this Transform child)
		{
			var parents = new List<Transform>();
			while (child != null)
			{
				parents.Add(child);
				child = child.parent;
			}
			parents.Reverse();
			return parents;
		}

		public static void SetFullStretch(this RectTransform tf)
		{
			if (tf == null)
			{
				return;
			}
			tf.anchorMin = Vector2.zero;
			tf.anchorMax = Vector2.one;
			tf.pivot = new Vector2(0.5f, 0.5f);
			tf.offsetMin = Vector2.zero;
			tf.offsetMax = Vector2.zero;
			Vector3 position = tf.localPosition;
			tf.localPosition = new Vector3(position.x, position.y, 0f);
			tf.localRotation = Quaternion.identity;
			tf.localScale = Vector3.one;
		}
	}
}

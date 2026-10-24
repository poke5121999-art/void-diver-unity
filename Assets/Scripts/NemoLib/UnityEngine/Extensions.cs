using System.Linq;
using UniRx;

namespace UnityEngine
{
	public static class Extensions
	{
		public static T FindChildComponent<T>(this GameObject parentGo, string childName) where T : class
		{
			// Transform enumeration of direct children, name match then GetComponent<T>, else recurse into the child (@0x180c03690 machine code checked)
			foreach (Transform child in parentGo.transform)
			{
				if (child.name == childName)
				{
					T component = child.GetComponent<T>();
					if (component != null)
					{
						return component;
					}
				}
				T found = child.gameObject.FindChildComponent<T>(childName);
				if (found != null)
				{
					return found;
				}
			}
			return null;
		}

		public static T GetOrAddComponent<T>(this GameObject go) where T : Component
		{
			if (!go.TryGetComponent<T>(out var component))
			{
				component = go.AddComponent<T>();
			}
			return component;
		}

		public static bool HasComponent<T>(this GameObject go)
		{
			return go.TryGetComponent<T>(out var _);
		}

		public static void DelayDestroy(this GameObject go, float delay)
		{
			go.GetOrAddComponent<AutoDestroy>().SetDelay(delay);
		}

		public static bool HasTrigger(this Animator animator, string triggerName)
		{
			if (animator == null || string.IsNullOrEmpty(triggerName))
			{
				return false;
			}
			if (animator.parameters == null)
			{
				return false;
			}
			int hash = Animator.StringToHash(triggerName);
			return animator.parameters.Any((AnimatorControllerParameter p) => p.type == AnimatorControllerParameterType.Trigger && p.nameHash == hash);
		}
	}
}

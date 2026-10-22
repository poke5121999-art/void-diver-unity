using System.Linq;
using UniRx;

namespace UnityEngine
{
	public static class Extensions
	{
		public static T FindChildComponent<T>(this GameObject parentGo, string childName) where T : class
		{
			// TODO(decomp): generic method has no pseudo-C; recursive search by name (including inactive children) is assumed
			if (parentGo == null)
			{
				return null;
			}
			foreach (Transform child in parentGo.GetComponentsInChildren<Transform>(true))
			{
				if (child.name == childName)
				{
					return child.GetComponent<T>();
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

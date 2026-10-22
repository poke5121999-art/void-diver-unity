namespace UnityEngine
{
	public static class Extensions
	{
		public static T FindChildComponent<T>(this GameObject parentGo, string childName) where T : class
		{
			return null;
		}

		public static T GetOrAddComponent<T>(this GameObject go) where T : Component
		{
			return null;
		}

		public static bool HasComponent<T>(this GameObject go)
		{
			return false;
		}

		public static void DelayDestroy(this GameObject go, float delay)
		{
		}

		public static bool HasTrigger(this Animator animator, string triggerName)
		{
			return false;
		}
	}
}

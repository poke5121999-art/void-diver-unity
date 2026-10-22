namespace VContainer.Unity
{
	public static class Extensions
	{
		public static void InjectFromRootContainer(this object obj)
		{
			VContainerSettings instance = VContainerSettings.Instance;
			if (instance == null)
			{
				return;
			}
			LifetimeScope rootScope = instance.GetOrCreateRootLifetimeScopeInstance();
			if (rootScope != null && rootScope.Container != null)
			{
				rootScope.Container.Inject(obj);
			}
		}
	}
}

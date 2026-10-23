namespace VContainer.Unity
{
	public static class Extensions
	{
		public static void InjectFromRootContainer(this object obj)
		{
			VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance().Container.Inject(obj);
		}
	}
}

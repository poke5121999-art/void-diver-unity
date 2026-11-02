namespace VContainer.Unity
{
	public static class Extensions
	{
		public static void InjectFromRootContainer(this object obj)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			// The web root does not build itself (RootLifetimeScope.Awake sets autoRun = false so WebBoot can preload
			// its Addressables first). An object that asks for the root container before any child scope built it
			// would call Inject on a null container, which the web player reports as a MethodAccessException.
			LifetimeScope root = VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();
			if (root.Container == null)
			{
				root.Build();
			}
			root.Container.Inject(obj);
#else
			VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance().Container.Inject(obj);
#endif
		}
	}
}

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer.Unity;

public abstract class MonoBase : MonoBehaviour
{
	protected virtual void Awake()
	{
		VContainerSettings instance = VContainerSettings.Instance;
		if (instance != null)
		{
			LifetimeScope root = instance.GetOrCreateRootLifetimeScopeInstance();
			if (root != null)
			{
				root.Container.Inject(this);
			}
		}
	}

	protected void DelayDestroy(float delay)
	{
		Observable.Timer(System.TimeSpan.FromSeconds(delay)).Subscribe(delegate
		{
			if (this != null)
			{
				Object.Destroy(base.gameObject);
			}
		}).AddTo(this);
	}
}

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer.Unity;

public abstract class MonoBase : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.InjectFromRootContainer();
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

using System;
using UniRx;
using UnityEngine;

public static class ObservableFixed
{
	public static IObservable<long> FixedTimer(TimeSpan dueTime)
	{
		return Observable.Create(delegate(IObserver<long> observer)
		{
			float elapsed = 0f;
			float targetSeconds = (float)dueTime.TotalSeconds;
			long ticks = 0L;
			IDisposable disposable = null;
			disposable = Observable.EveryFixedUpdate().Subscribe(delegate
			{
				elapsed += Time.fixedDeltaTime;
				if (!(elapsed < targetSeconds))
				{
					observer.OnNext(ticks++);
					observer.OnCompleted();
					disposable.Dispose();
				}
			});
			return disposable;
		});
	}
}

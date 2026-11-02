using System;
using System.Threading;
using Cysharp.Threading.Tasks;

// UniTask.RunOnThreadPool for the game's file and table work. The web player has no threads: a work item queued on
// the thread pool never runs and the await never returns (the splash waited forever on the tables). There the work
// runs on the main thread, which is what a single-threaded player can do; elsewhere nothing changes.
public static class OffMainThread
{
	public static UniTask Run(Action action, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		cancellationToken.ThrowIfCancellationRequested();
		action();
		return UniTask.CompletedTask;
#else
		return UniTask.RunOnThreadPool(action, configureAwait, cancellationToken);
#endif
	}

	public static UniTask<T> Run<T>(Func<T> func, bool configureAwait = true, CancellationToken cancellationToken = default)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		cancellationToken.ThrowIfCancellationRequested();
		return UniTask.FromResult(func());
#else
		return UniTask.RunOnThreadPool(func, configureAwait, cancellationToken);
#endif
	}
}

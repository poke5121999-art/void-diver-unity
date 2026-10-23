using System.Threading;
using Cysharp.Threading.Tasks;
using SomeNet.Utility;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineRandomPlayer : MonoBehaviour
{
	[SerializeField]
	private PlayableDirector[] _loopList;

	[SerializeField]
	private PlayableDirector[] _onceList;

	[Range(0f, 100f)]
	[SerializeField]
	private float _loopPercent;

	private CancellationTokenSource _cancellationTokenSource;

	private void Start()
	{
		DisableAll();
		if (!RandomUtility.GetRandomBoolFromPercentage(_loopPercent))
		{
			StartOnceAnimationAsync().Forget();
		}
		else
		{
			StartLoopAnimation();
		}
	}

	private void OnDisable()
	{
		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource?.Dispose();
		_cancellationTokenSource = null;
	}

	private void StartLoopAnimation()
	{
		if (_loopList != null && _loopList.Length != 0)
		{
			PlayableDirector playableDirector = _loopList[RandomUtility.GetRandomInt(_loopList.Length)];
			if (playableDirector != null)
			{
				playableDirector.gameObject.SetActive(true);
			}
		}
	}

	private async UniTask StartOnceAnimationAsync()
	{
		if (_onceList == null || _onceList.Length == 0)
		{
			return;
		}
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		while (!token.IsCancellationRequested)
		{
			DisableAll();
			PlayableDirector playableDirector = _onceList[RandomUtility.GetRandomInt(_onceList.Length)];
			playableDirector.gameObject.SetActive(true);
			await UniTask.Delay((int)(playableDirector.duration * 1000.0), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
		}
	}

	private void DisableAll()
	{
		if (_loopList != null)
		{
			foreach (PlayableDirector playableDirector in _loopList)
			{
				if (playableDirector != null)
				{
					playableDirector.gameObject.SetActive(false);
				}
			}
		}
		if (_onceList != null)
		{
			foreach (PlayableDirector playableDirector2 in _onceList)
			{
				if (playableDirector2 != null)
				{
					playableDirector2.gameObject.SetActive(false);
				}
			}
		}
	}
}

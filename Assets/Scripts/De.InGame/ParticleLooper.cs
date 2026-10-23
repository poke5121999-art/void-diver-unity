using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ParticleLooper : MonoBehaviour
{
	private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	private float _loopInterval;

	private CancellationTokenSource _cts;

	private void Awake()
	{
		CollectParticleSystems();
		CalculateMaxDuration();
	}

	private void OnEnable()
	{
		_cts?.Cancel();
		_cts?.Dispose();
		_cts = new CancellationTokenSource();
		StartLooping(_cts.Token).Forget();
	}

	private void OnDisable()
	{
		_cts?.Cancel();
		_cts?.Dispose();
		_cts = null;
	}

	private void CollectParticleSystems()
	{
		_particleSystems.Clear();
		_particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>(true));
	}

	private void CalculateMaxDuration()
	{
		float num = 0f;
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			float num2 = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
			if (num < num2)
			{
				num = num2;
			}
		}
		_loopInterval = num;
	}

	private async UniTaskVoid StartLooping(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			RestartAllParticles();
			await UniTask.Delay((int)(_loopInterval * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
		}
	}

	private void RestartAllParticles()
	{
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			particleSystem.Play(withChildren: true);
		}
	}
}

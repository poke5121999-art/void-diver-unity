using NemoLib.AssetManagement;
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using UnityEngine;

public class SpeedVfxObject : MonoBase
{
	protected IList<AnimationSpeed> _animSpeeds;

	protected ParticleSystem[] _particleSystems;

	private float _loopDuration;

	public async UniTask SetSpeed(IList<AnimationSpeed> animationSpeeds, bool loop = true, float loopDuration = -1f)
	{
		_animSpeeds = animationSpeeds;
		_particleSystems = GetComponentsInChildren<ParticleSystem>();
		_loopDuration = ((loopDuration > 0f) ? loopDuration : 99f);
		PoolableObject component = GetComponent<PoolableObject>();
		CancellationToken token = ((component != null) ? component.DespawnToken : base.destroyCancellationToken);
		// TODO(decomp): the original state machine has a compiler-generated finally (FUN_1805dd0f0) on every exit path; its body is not recoverable @0x1805f73a0
		do
		{
			if (this == null || !base.gameObject.activeInHierarchy)
			{
				break;
			}
			RestartAllParticles();
			if (_animSpeeds != null && _animSpeeds.Count > 0)
			{
				await ApplySpeedSequence(_animSpeeds, token);
			}
			float maxRemainingDuration = GetMaxRemainingDuration();
			if (maxRemainingDuration > 0f)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(maxRemainingDuration), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
			}
		}
		while (loop);
	}

	private void RestartAllParticles()
	{
		ParticleSystem[] particleSystems = _particleSystems;
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			particleSystems[i].Play(withChildren: true);
		}
	}

	private async UniTask ApplySpeedSequence(IList<AnimationSpeed> speeds, CancellationToken token)
	{
		for (int i = 0; i < speeds.Count; i++)
		{
			float speed = speeds[i].speed;
			if (speed <= 0f)
			{
				continue;
			}
			ParticleSystem[] particleSystems = _particleSystems;
			for (int j = 0; j < particleSystems.Length; j++)
			{
				ParticleSystem.MainModule main = particleSystems[j].main;
				main.simulationSpeed = speed;
			}
			float num = ((i > 0) ? speeds[i - 1].endTime : 0f);
			float num2 = Mathf.Max(0f, (speeds[i].endTime - num) / speed);
			num2 = Mathf.Min(num2, GetMaxRemainingDuration());
			if (!(num2 <= 0f))
			{
				await UniTask.Delay(TimeSpan.FromSeconds(num2), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
			}
		}
	}

	private float GetMaxRemainingDuration()
	{
		float num = 0f;
		ParticleSystem[] particleSystems = _particleSystems;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			float num2 = Mathf.Max(particleSystem.main.simulationSpeed, 0.01f);
			float num3 = Mathf.Max(_loopDuration - particleSystem.time, 0f);
			num = Mathf.Max(num, num3 / num2);
		}
		return num;
	}

	private void ResetSimulationSpeed()
	{
	}
}

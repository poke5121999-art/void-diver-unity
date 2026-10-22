using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRandomSeedController : MonoBehaviour
{
	[Header("필수 설정")]
	public ParticleSystem particlePrefab;

	public int[] allowedSeeds = new int[3] { 123, -1, 456 };

	public bool isLooping;

	private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

	private Coroutine loopCoroutine;

	private bool hasInitializedPool;

	private Dictionary<ParticleSystem, Coroutine> disableCoroutines = new Dictionary<ParticleSystem, Coroutine>();

	private void OnEnable()
	{
		if (particlePrefab == null)
		{
			string parentName = ((transform.parent != null) ? transform.parent.name : "ROOT");
			string fullPath = GetGameObjectPath(transform);
			Debug.LogError("[EffectRandomSeedController] Particle Prefab이 연결되지 않았습니다!\nTarget Object: <color=yellow>" + name + "</color>\nParent: " + parentName + "\nFull Path: " + fullPath + "\nTip: 인스펙터에서 'Particle Prefab' 변수에 파티클 시스템을 드래그해서 넣어주세요.", gameObject);
			return;
		}
		if (!isLooping)
		{
			PlayOnceWithRandomSeed();
			return;
		}
		if (transform != transform.root || loopCoroutine != null)
		{
			return;
		}
		if (!hasInitializedPool)
		{
			InitLoopingPool();
			hasInitializedPool = true;
		}
		foreach (ParticleSystem item in pool)
		{
			item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			item.gameObject.SetActive(false);
		}
		loopCoroutine = StartCoroutine(RunLoopingPool());
	}

	private void OnDisable()
	{
		if (loopCoroutine != null)
		{
			StopCoroutine(loopCoroutine);
			loopCoroutine = null;
		}
		foreach (KeyValuePair<ParticleSystem, Coroutine> item in disableCoroutines)
		{
			if (item.Value != null)
			{
				StopCoroutine(item.Value);
			}
		}
		disableCoroutines.Clear();
	}

	private void PlayOnceWithRandomSeed()
	{
		particlePrefab.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		particlePrefab.useAutoRandomSeed = false;
		if (allowedSeeds.Length == 0)
		{
			particlePrefab.randomSeed = (uint)UnityEngine.Random.Range(0, int.MaxValue);
		}
		else
		{
			particlePrefab.randomSeed = (uint)allowedSeeds[UnityEngine.Random.Range(0, allowedSeeds.Length)];
		}
		particlePrefab.Play(true);
	}

	private void InitLoopingPool()
	{
		foreach (int seed in allowedSeeds)
		{
			ParticleSystem ps = UnityEngine.Object.Instantiate(particlePrefab, transform);
			ps.useAutoRandomSeed = false;
			ps.randomSeed = (uint)seed;
			ps.gameObject.SetActive(false);
			pool.Enqueue(ps);
		}
	}

	private IEnumerator RunLoopingPool()
	{
		float loopInterval = particlePrefab.main.duration;
		while (pool.Count != 0)
		{
			ParticleSystem ps = pool.Dequeue();
			if (disableCoroutines.TryGetValue(ps, out var running))
			{
				if (running != null)
				{
					StopCoroutine(running);
				}
				disableCoroutines.Remove(ps);
			}
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps.gameObject.SetActive(true);
			float delay = ps.main.duration + ps.main.startLifetime.constant;
			disableCoroutines[ps] = StartCoroutine(DisableAfter(ps, delay));
			pool.Enqueue(ps);
			yield return new WaitForSeconds(loopInterval);
		}
	}

	private IEnumerator DisableAfter(ParticleSystem ps, float delay)
	{
		yield return new WaitForSeconds(delay);
		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		ps.gameObject.SetActive(false);
		disableCoroutines.Remove(ps);
	}

	private string GetGameObjectPath(Transform transform)
	{
		string text = transform.name;
		while (transform.parent != null)
		{
			transform = transform.parent;
			text = transform.name + "/" + text;
		}
		return text;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleSetupTool : MonoBehaviour
{
	[Serializable]
	public class TransformEntry
	{
		public Transform targetTransform;

		public bool lockX;

		public bool lockY;

		public bool lockZ;

		public float additionalDelay;

		[HideInInspector]
		public Vector3 backupScale;

		[HideInInspector]
		public bool hasBackup;
	}

	[Serializable]
	public class ParticleEntry
	{
		public ParticleSystem particleSystem;

		public bool ignoreSize;

		public bool lockX;

		public bool lockY;

		public bool lockZ;

		[HideInInspector]
		public bool hasBackup;

		[HideInInspector]
		public bool was3DSize;

		[HideInInspector]
		public float backupSizeMultiplier;

		[HideInInspector]
		public float backupSizeXMultiplier;

		[HideInInspector]
		public float backupSizeYMultiplier;

		[HideInInspector]
		public float backupSizeZMultiplier;

		[HideInInspector]
		public ParticleSystem.MinMaxCurve backupStartSize;

		[HideInInspector]
		public ParticleSystem.MinMaxCurve backupStartSizeX;

		[HideInInspector]
		public ParticleSystem.MinMaxCurve backupStartSizeY;

		[HideInInspector]
		public ParticleSystem.MinMaxCurve backupStartSizeZ;

		[HideInInspector]
		public ParticleSystem.MinMaxCurve backupDelay;

		[HideInInspector]
		public bool backupEmissionEnabled;
	}

	[Serializable]
	public class AnimatorEntry
	{
		public Animator animator;

		[HideInInspector]
		public SimpleAnimatorDelay delayComponent;

		[HideInInspector]
		public float backupDelayTime;

		[HideInInspector]
		public float backupSpeedMultiplier;

		[HideInInspector]
		public bool hasBackup;
	}

	public enum DurationMode
	{
		StretchAllFrames = 0,
		HoldAtEnd = 1
	}

	[Header("Global Settings")]
	public float sizeMultiplier = 1f;

	public float delayOffset;

	[Header("Duration Settings")]
	public bool useDuration;

	[FormerlySerializedAs("durationFrames")]
	[Min(0f)]
	public float durationSeconds;

	public DurationMode durationMode;

	public bool disableParticleEmissionAfterDuration = true;

	public List<AnimationClip> durationClips = new List<AnimationClip>();

	[Space(10f)]
	public List<TransformEntry> targetTransforms = new List<TransformEntry>();

	public List<ParticleEntry> particles = new List<ParticleEntry>();

	public List<AnimatorEntry> animators = new List<AnimatorEntry>();

	[HideInInspector]
	public bool isPreviewing;

	private readonly List<Coroutine> runtimeDurationCoroutines = new List<Coroutine>();

	private void Awake()
	{
		if (!isPreviewing && !Application.isPlaying)
		{
			ApplyLogic(createBackup: false);
		}
	}

	private void OnEnable()
	{
		if (!isPreviewing && Application.isPlaying)
		{
			ApplyLogic(createBackup: true);
			RestartPresentationFromBeginning();
		}
	}

	private void OnDisable()
	{
		if (!isPreviewing && Application.isPlaying)
		{
			RevertLogic();
			ResetParticleEmissionForReplay();
		}
		else
		{
			ClearRuntimeDurationCoroutines();
		}
	}

	public void ApplyLogic(bool createBackup)
	{
		if (Application.isPlaying)
		{
			ClearRuntimeDurationCoroutines();
		}
		foreach (TransformEntry targetTransform in targetTransforms)
		{
			if (targetTransform.targetTransform == null)
			{
				continue;
			}
			if (createBackup)
			{
				targetTransform.backupScale = targetTransform.targetTransform.localScale;
				targetTransform.hasBackup = true;
			}
			Vector3 localScale = targetTransform.targetTransform.localScale;
			float num = sizeMultiplier;
			Vector3 localScale2 = new Vector3(targetTransform.lockX ? localScale.x : (localScale.x * num), targetTransform.lockY ? localScale.y : (localScale.y * num), targetTransform.lockZ ? localScale.z : (localScale.z * num));
			targetTransform.targetTransform.localScale = localScale2;
		}
		foreach (ParticleEntry particle in particles)
		{
			if (particle.particleSystem == null)
			{
				continue;
			}
			ParticleSystem.MainModule main = particle.particleSystem.main;
			if (createBackup)
			{
				particle.was3DSize = main.startSize3D;
				if (!particle.was3DSize)
				{
					particle.backupSizeMultiplier = main.startSizeMultiplier;
					particle.backupStartSize = main.startSize;
				}
				else
				{
					particle.backupSizeXMultiplier = main.startSizeXMultiplier;
					particle.backupSizeYMultiplier = main.startSizeYMultiplier;
					particle.backupSizeZMultiplier = main.startSizeZMultiplier;
					particle.backupStartSizeX = main.startSizeX;
					particle.backupStartSizeY = main.startSizeY;
					particle.backupStartSizeZ = main.startSizeZ;
				}
				particle.backupDelay = main.startDelay;
				particle.backupEmissionEnabled = particle.particleSystem.emission.enabled;
				particle.hasBackup = true;
			}
			if (!particle.ignoreSize)
			{
				if (!main.startSize3D)
				{
					main.startSizeMultiplier = sizeMultiplier * main.startSizeMultiplier;
				}
				else
				{
					if (!particle.lockX)
					{
						main.startSizeXMultiplier = sizeMultiplier * main.startSizeXMultiplier;
					}
					if (!particle.lockY)
					{
						main.startSizeYMultiplier = sizeMultiplier * main.startSizeYMultiplier;
					}
					if (!particle.lockZ)
					{
						main.startSizeZMultiplier = sizeMultiplier * main.startSizeZMultiplier;
					}
				}
			}
			float num2 = delayOffset + GetAdditionalDelayForTransform(particle.particleSystem.transform);
			if (Mathf.Abs(num2) > 0.0001f)
			{
				ParticleSystem.MinMaxCurve startDelay = main.startDelay;
				if (startDelay.mode == ParticleSystemCurveMode.Constant)
				{
					startDelay.constant += num2;
				}
				else if (startDelay.mode == ParticleSystemCurveMode.TwoConstants)
				{
					startDelay.constantMin += num2;
					startDelay.constantMax += num2;
				}
				// TODO(decomp): getter/setter pairing for the TwoConstants branch inferred from address aliasing (constantMin/constantMax order) @0x1805DA000
				main.startDelay = startDelay;
			}
		}
		float num3 = 0f;
		bool flag = HasRegisteredDurationClips();
		foreach (AnimatorEntry animator in animators)
		{
			if (animator.animator == null)
			{
				continue;
			}
			if (animator.delayComponent == null)
			{
				animator.delayComponent = animator.animator.GetComponent<SimpleAnimatorDelay>();
			}
			if (animator.delayComponent == null)
			{
				animator.delayComponent = animator.animator.gameObject.AddComponent<SimpleAnimatorDelay>();
			}
			if (createBackup)
			{
				animator.backupDelayTime = animator.delayComponent.delayTime;
				animator.backupSpeedMultiplier = animator.delayComponent.playbackSpeedMultiplier;
				animator.hasBackup = true;
			}
			float additionalDelayForTransform = GetAdditionalDelayForTransform(animator.animator.transform);
			animator.delayComponent.delayTime += delayOffset + additionalDelayForTransform;
			animator.delayComponent.playbackSpeedMultiplier = 1f;
			bool flag2 = useDuration && durationSeconds > 0f;
			if (animator.delayComponent.ConfigureDuration(flag2 && flag, durationSeconds, durationMode, durationClips) && num3 <= durationSeconds)
			{
				num3 = durationSeconds;
			}
		}
		if (useDuration && durationSeconds > 0f && num3 <= durationSeconds)
		{
			num3 = durationSeconds;
		}
		if (Application.isPlaying && useDuration && disableParticleEmissionAfterDuration && num3 > 0f)
		{
			ScheduleParticleEmissionDisable(num3);
		}
	}

	private void ScheduleParticleEmissionDisable(float maxEffectiveDuration)
	{
		foreach (ParticleEntry particle in particles)
		{
			if (!(particle.particleSystem == null))
			{
				StartRuntimeDurationCoroutine(DisableEmissionAfterDelay(particle.particleSystem, (maxEffectiveDuration >= 0f) ? maxEffectiveDuration : 0f));
			}
		}
	}

	private IEnumerator DisableEmissionAfterDelay(ParticleSystem particleSystem, float delaySeconds)
	{
		if (delaySeconds > 0f)
		{
			yield return new WaitForSeconds(delaySeconds);
		}
		if (!(particleSystem == null))
		{
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.enabled = false;
		}
	}

	private void StartRuntimeDurationCoroutine(IEnumerator routine)
	{
		if (routine != null)
		{
			runtimeDurationCoroutines.Add(StartCoroutine(routine));
		}
	}

	private void ClearRuntimeDurationCoroutines()
	{
		foreach (Coroutine runtimeDurationCoroutine in runtimeDurationCoroutines)
		{
			if (runtimeDurationCoroutine != null)
			{
				StopCoroutine(runtimeDurationCoroutine);
			}
		}
		runtimeDurationCoroutines.Clear();
	}

	private bool HasRegisteredDurationClips()
	{
		for (int i = 0; i < durationClips.Count; i++)
		{
			if (durationClips[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	private void RestartPresentationFromBeginning()
	{
		foreach (AnimatorEntry animator in animators)
		{
			if (!(animator.animator == null))
			{
				animator.animator.Rebind();
				animator.animator.Update(0f);
				if (animator.delayComponent != null)
				{
					animator.delayComponent.RestartPlayback();
				}
			}
		}
		foreach (ParticleEntry particle in particles)
		{
			if (!(particle.particleSystem == null))
			{
				ParticleSystem.EmissionModule emission = particle.particleSystem.emission;
				emission.enabled = true;
				particle.particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				particle.particleSystem.Play(withChildren: true);
			}
		}
	}

	private void ResetParticleEmissionForReplay()
	{
		foreach (ParticleEntry particle in particles)
		{
			if (!(particle.particleSystem == null))
			{
				ParticleSystem.EmissionModule emission = particle.particleSystem.emission;
				emission.enabled = true;
			}
		}
	}

	private void ApplySizeMultiplierToCurve(ref ParticleSystem.MinMaxCurve curve, float multiplier)
	{
		if (curve.mode == ParticleSystemCurveMode.Constant)
		{
			curve.constant *= multiplier;
		}
		else if (curve.mode == ParticleSystemCurveMode.TwoConstants)
		{
			curve.constantMin *= multiplier;
			curve.constantMax *= multiplier;
		}
	}

	private float GetAdditionalDelayForTransform(Transform target)
	{
		float num = 0f;
		foreach (TransformEntry targetTransform in targetTransforms)
		{
			if (!(targetTransform.targetTransform == null) && IsChildOf(target, targetTransform.targetTransform) && num <= targetTransform.additionalDelay)
			{
				num = targetTransform.additionalDelay;
			}
		}
		return num;
	}

	private bool IsChildOf(Transform child, Transform parent)
	{
		if (child == parent)
		{
			return true;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	public void RevertLogic()
	{
		ClearRuntimeDurationCoroutines();
		foreach (TransformEntry targetTransform in targetTransforms)
		{
			if (targetTransform.targetTransform != null && targetTransform.hasBackup)
			{
				targetTransform.targetTransform.localScale = targetTransform.backupScale;
				targetTransform.hasBackup = false;
			}
		}
		foreach (ParticleEntry particle in particles)
		{
			if (particle.particleSystem == null || !particle.hasBackup)
			{
				continue;
			}
			ParticleSystem.MainModule main = particle.particleSystem.main;
			if (!particle.was3DSize)
			{
				main.startSizeMultiplier = particle.backupSizeMultiplier;
				main.startSize = particle.backupStartSize;
			}
			else
			{
				main.startSizeXMultiplier = particle.backupSizeXMultiplier;
				main.startSizeYMultiplier = particle.backupSizeYMultiplier;
				main.startSizeZMultiplier = particle.backupSizeZMultiplier;
				main.startSizeX = particle.backupStartSizeX;
				main.startSizeY = particle.backupStartSizeY;
				main.startSizeZ = particle.backupStartSizeZ;
			}
			main.startDelay = particle.backupDelay;
			ParticleSystem.EmissionModule emission = particle.particleSystem.emission;
			emission.enabled = particle.backupEmissionEnabled;
			particle.hasBackup = false;
		}
		foreach (AnimatorEntry animator in animators)
		{
			if (!(animator.animator == null) && animator.hasBackup && !(animator.delayComponent == null))
			{
				animator.delayComponent.delayTime = animator.backupDelayTime;
				animator.delayComponent.playbackSpeedMultiplier = animator.backupSpeedMultiplier;
				animator.delayComponent.ConfigureDuration(useDuration: false, 0f, durationMode, null);
				animator.delayComponent.ApplyConfiguredSpeed();
				animator.hasBackup = false;
			}
		}
	}
}

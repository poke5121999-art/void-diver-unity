using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

	[CompilerGenerated]
	private sealed class _003CDisableEmissionAfterDelay_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delaySeconds;

		public ParticleSystem particleSystem;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CDisableEmissionAfterDelay_003Ed__21(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Global Settings")]
	public float sizeMultiplier;

	public float delayOffset;

	[Header("Duration Settings")]
	public bool useDuration;

	[FormerlySerializedAs("durationFrames")]
	[Min(0f)]
	public float durationSeconds;

	public DurationMode durationMode;

	public bool disableParticleEmissionAfterDuration;

	public List<AnimationClip> durationClips;

	[Space(10f)]
	public List<TransformEntry> targetTransforms;

	public List<ParticleEntry> particles;

	public List<AnimatorEntry> animators;

	[HideInInspector]
	public bool isPreviewing;

	private readonly List<Coroutine> runtimeDurationCoroutines;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void ApplyLogic(bool createBackup)
	{
	}

	private void ScheduleParticleEmissionDisable(float maxEffectiveDuration)
	{
	}

	[IteratorStateMachine(typeof(_003CDisableEmissionAfterDelay_003Ed__21))]
	private IEnumerator DisableEmissionAfterDelay(ParticleSystem particleSystem, float delaySeconds)
	{
		return null;
	}

	private void StartRuntimeDurationCoroutine(IEnumerator routine)
	{
	}

	private void ClearRuntimeDurationCoroutines()
	{
	}

	private bool HasRegisteredDurationClips()
	{
		return false;
	}

	private void RestartPresentationFromBeginning()
	{
	}

	private void ResetParticleEmissionForReplay()
	{
	}

	private void ApplySizeMultiplierToCurve(ref ParticleSystem.MinMaxCurve curve, float multiplier)
	{
	}

	private float GetAdditionalDelayForTransform(Transform target)
	{
		return 0f;
	}

	private bool IsChildOf(Transform child, Transform parent)
	{
		return false;
	}

	public void RevertLogic()
	{
	}
}

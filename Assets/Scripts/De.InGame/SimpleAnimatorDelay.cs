using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SimpleAnimatorDelay : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHoldAtEndOfClip_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AnimationClip expectedClip;

		public SimpleAnimatorDelay _003C_003E4__this;

		public float holdSeconds;

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
		public _003CHoldAtEndOfClip_003Ed__28(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CWaitAndPlay_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SimpleAnimatorDelay _003C_003E4__this;

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
		public _003CWaitAndPlay_003Ed__18(int _003C_003E1__state)
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

	public float delayTime;

	public float playbackSpeedMultiplier;

	private Animator _animator;

	private float _originalSpeed;

	private Coroutine _delayCoroutine;

	private Coroutine _holdCoroutine;

	private bool _isForcePaused;

	private float _durationTargetSeconds;

	private bool _useClipScopedDuration;

	private ParticleSetupTool.DurationMode _durationMode;

	private readonly List<AnimationClip> _durationTargetClips;

	private AnimationClip _holdScheduledClip;

	public Animator CachedAnimator => null;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitAndPlay_003Ed__18))]
	private IEnumerator WaitAndPlay()
	{
		return null;
	}

	public float GetEffectiveSpeed()
	{
		return 0f;
	}

	public void ApplyConfiguredSpeed()
	{
	}

	public void RestartPlayback()
	{
	}

	public bool ConfigureDuration(bool useDuration, float targetSeconds, ParticleSetupTool.DurationMode durationMode, List<AnimationClip> targetClips)
	{
		return false;
	}

	public bool TryGetCurrentRegisteredClip(out AnimationClip clip)
	{
		clip = null;
		return false;
	}

	private float GetActivePlaybackMultiplier()
	{
		return 0f;
	}

	private float GetDurationMultiplierForClip(AnimationClip clip)
	{
		return 0f;
	}

	private AnimationClip GetCurrentRegisteredClip()
	{
		return null;
	}

	private void UpdateClipScopedHoldState()
	{
	}

	[IteratorStateMachine(typeof(_003CHoldAtEndOfClip_003Ed__28))]
	private IEnumerator HoldAtEndOfClip(AnimationClip expectedClip, float holdSeconds)
	{
		return null;
	}

	private void StopHoldCoroutine()
	{
	}
}

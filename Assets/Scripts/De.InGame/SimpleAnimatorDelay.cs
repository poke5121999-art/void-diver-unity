using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimatorDelay : MonoBehaviour
{
	public float delayTime;

	public float playbackSpeedMultiplier = 1f;

	private Animator _animator;

	private float _originalSpeed = 1f;

	private Coroutine _delayCoroutine;

	private Coroutine _holdCoroutine;

	private bool _isForcePaused;

	private float _durationTargetSeconds;

	private bool _useClipScopedDuration;

	private ParticleSetupTool.DurationMode _durationMode;

	private readonly List<AnimationClip> _durationTargetClips = new List<AnimationClip>();

	private AnimationClip _holdScheduledClip;

	public Animator CachedAnimator => _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		if (_animator != null)
		{
			_originalSpeed = _animator.speed;
		}
	}

	private void OnEnable()
	{
		RestartPlayback();
	}

	private void Update()
	{
		if (!(_animator == null))
		{
			UpdateClipScopedHoldState();
			if (_delayCoroutine == null && !_isForcePaused)
			{
				ApplyConfiguredSpeed();
			}
		}
	}

	private void OnDisable()
	{
		if (_animator != null)
		{
			_animator.speed = _originalSpeed;
		}
		if (_delayCoroutine != null)
		{
			StopCoroutine(_delayCoroutine);
			_delayCoroutine = null;
		}
		if (_holdCoroutine != null)
		{
			StopCoroutine(_holdCoroutine);
			_holdCoroutine = null;
		}
		_holdScheduledClip = null;
		_isForcePaused = false;
	}

	private IEnumerator WaitAndPlay()
	{
		yield return new WaitForSeconds(delayTime);
		ApplyConfiguredSpeed();
		_delayCoroutine = null;
	}

	public float GetEffectiveSpeed()
	{
		return Mathf.Abs(_originalSpeed * GetActivePlaybackMultiplier());
	}

	public void ApplyConfiguredSpeed()
	{
		if (!(_animator == null))
		{
			_animator.speed = (_isForcePaused ? 0f : (_originalSpeed * GetActivePlaybackMultiplier()));
		}
	}

	public void RestartPlayback()
	{
		if (_animator == null)
		{
			return;
		}
		if (_delayCoroutine != null)
		{
			StopCoroutine(_delayCoroutine);
			_delayCoroutine = null;
		}
		if (_holdCoroutine != null)
		{
			StopCoroutine(_holdCoroutine);
			_holdCoroutine = null;
		}
		_holdScheduledClip = null;
		_isForcePaused = false;
		if (delayTime <= 0f)
		{
			ApplyConfiguredSpeed();
			return;
		}
		_animator.speed = 0f;
		_delayCoroutine = StartCoroutine(WaitAndPlay());
	}

	public bool ConfigureDuration(bool useDuration, float targetSeconds, ParticleSetupTool.DurationMode durationMode, List<AnimationClip> targetClips)
	{
		_useClipScopedDuration = useDuration;
		_durationTargetSeconds = Mathf.Max(0f, targetSeconds);
		_durationMode = durationMode;
		_durationTargetClips.Clear();
		if (targetClips != null)
		{
			foreach (AnimationClip targetClip in targetClips)
			{
				if (targetClip != null && !_durationTargetClips.Contains(targetClip))
				{
					_durationTargetClips.Add(targetClip);
				}
			}
		}
		if (_useClipScopedDuration && _durationTargetSeconds > 0f && _durationTargetClips.Count != 0)
		{
			if (_holdCoroutine != null)
			{
				StopCoroutine(_holdCoroutine);
				_holdCoroutine = null;
			}
			_holdScheduledClip = null;
			_isForcePaused = false;
			return true;
		}
		_useClipScopedDuration = false;
		if (_holdCoroutine != null)
		{
			StopCoroutine(_holdCoroutine);
			_holdCoroutine = null;
		}
		_holdScheduledClip = null;
		_isForcePaused = false;
		return false;
	}

	public bool TryGetCurrentRegisteredClip(out AnimationClip clip)
	{
		clip = GetCurrentRegisteredClip();
		return clip != null;
	}

	private float GetActivePlaybackMultiplier()
	{
		return GetDurationMultiplierForClip(GetCurrentRegisteredClip()) * playbackSpeedMultiplier;
	}

	private float GetDurationMultiplierForClip(AnimationClip clip)
	{
		if (!_useClipScopedDuration)
		{
			return 1f;
		}
		if (clip == null)
		{
			return 1f;
		}
		float num = Mathf.Max(_durationTargetSeconds, 0.0001f);
		if (_durationMode != ParticleSetupTool.DurationMode.StretchAllFrames && clip.length <= num)
		{
			return 1f;
		}
		return Mathf.Clamp(clip.length / num, 0.0001f, 100f);
	}

	private AnimationClip GetCurrentRegisteredClip()
	{
		if (_animator == null || !_useClipScopedDuration)
		{
			return null;
		}
		if (_durationTargetClips.Count == 0)
		{
			return null;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
		for (int i = 0; i < currentAnimatorClipInfo.Length; i++)
		{
			AnimationClip clip = currentAnimatorClipInfo[i].clip;
			if (clip != null && _durationTargetClips.Contains(clip))
			{
				return clip;
			}
		}
		return null;
	}

	private void UpdateClipScopedHoldState()
	{
		if (_durationMode == ParticleSetupTool.DurationMode.HoldAtEnd)
		{
			AnimationClip currentRegisteredClip = GetCurrentRegisteredClip();
			if (currentRegisteredClip != null)
			{
				float num = _durationTargetSeconds - currentRegisteredClip.length;
				if (num > 0f)
				{
					if (_holdScheduledClip == currentRegisteredClip)
					{
						return;
					}
					if (_holdCoroutine != null)
					{
						StopCoroutine(_holdCoroutine);
						_holdCoroutine = null;
					}
					_holdScheduledClip = null;
					_holdScheduledClip = currentRegisteredClip;
					_holdCoroutine = StartCoroutine(HoldAtEndOfClip(currentRegisteredClip, num));
					return;
				}
				if (_holdScheduledClip == null)
				{
					return;
				}
				if (_holdCoroutine != null)
				{
					StopCoroutine(_holdCoroutine);
					_holdCoroutine = null;
				}
				_holdScheduledClip = null;
				_isForcePaused = false;
				return;
			}
		}
		if (_holdScheduledClip != null)
		{
			if (_holdCoroutine != null)
			{
				StopCoroutine(_holdCoroutine);
				_holdCoroutine = null;
			}
			_holdScheduledClip = null;
			_isForcePaused = false;
		}
	}

	private IEnumerator HoldAtEndOfClip(AnimationClip expectedClip, float holdSeconds)
	{
		float length = expectedClip.length;
		float num = Mathf.Max(Mathf.Abs(GetActivePlaybackMultiplier() * _originalSpeed), 0.0001f);
		float num2 = length / num;
		if (num2 > 0f)
		{
			yield return new WaitForSeconds(num2);
		}
		if (GetCurrentRegisteredClip() == expectedClip)
		{
			_isForcePaused = true;
			ApplyConfiguredSpeed();
			if (holdSeconds > 0f)
			{
				yield return new WaitForSeconds(holdSeconds);
			}
			_isForcePaused = false;
			ApplyConfiguredSpeed();
		}
		if (_holdCoroutine != null)
		{
			StopCoroutine(_holdCoroutine);
			_holdCoroutine = null;
		}
		_holdScheduledClip = null;
	}

	private void StopHoldCoroutine()
	{
		if (_holdCoroutine != null)
		{
			StopCoroutine(_holdCoroutine);
			_holdCoroutine = null;
		}
		_holdScheduledClip = null;
	}
}

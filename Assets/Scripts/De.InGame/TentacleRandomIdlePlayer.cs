using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TentacleRandomIdlePlayer : MonoBehaviour
{
	[Header("Animator")]
	[Tooltip("제어할 Animator (비어있으면 자신에서 탐색)")]
	[FormerlySerializedAs("animator")]
	[SerializeField]
	private Animator _animator;

	[Tooltip("재생할 Animator Layer 인덱스 (Base=0, Head=1, ...)")]
	[FormerlySerializedAs("layerIndex")]
	[SerializeField]
	private int _layerIndex = 1;

	[Header("Idle Clips")]
	[Tooltip("랜덤 재생할 Animator State 이름 목록")]
	[FormerlySerializedAs("stateNames")]
	[SerializeField]
	private string[] _stateNames = new string[3] { "Anim_Boss_Tentacle_Head_Idle_01", "Anim_Boss_Tentacle_Head_Idle_02", "Anim_Boss_Tentacle_Head_Idle_03" };

	[Header("Speed")]
	[Tooltip("최소 재생 속도")]
	[FormerlySerializedAs("minSpeed")]
	[SerializeField]
	private float _minSpeed = 0.8f;

	[Tooltip("최대 재생 속도")]
	[FormerlySerializedAs("maxSpeed")]
	[SerializeField]
	private float _maxSpeed = 1.2f;

	[Tooltip("속도를 적용할 Animator Float 파라미터 이름 (State의 Speed Multiplier Parameter로 설정)")]
	[FormerlySerializedAs("IdleSpeed")]
	[SerializeField]
	private string _speedParam = "IdleSpeed";

	[Header("Transition")]
	[Tooltip("CrossFade 전환 시간 (초)")]
	[FormerlySerializedAs("crossFadeDuration")]
	[SerializeField]
	private float _crossFadeDuration = 0.3f;

	[Tooltip("전환 트리거 시점 (normalizedTime 기준)")]
	[Range(0.5f, 1f)]
	[FormerlySerializedAs("transitionPoint")]
	[SerializeField]
	private float _transitionPoint = 0.85f;

	private int[] _stateHashes;

	private int _lastIndex = -1;

	private Coroutine _loopCoroutine;

	public bool IsPaused { get; private set; }

	public void Pause()
	{
		IsPaused = true;
		StopLoop();
	}

	public void Resume()
	{
		IsPaused = false;
		if (!(_animator == null) && _stateHashes != null && _stateHashes.Length != 0)
		{
			if (!IsCurrentStateIdle())
			{
				CrossFadeToNext();
			}
			StartLoop();
		}
	}

	private void Awake()
	{
		if (_animator == null)
		{
			_animator = GetComponent<Animator>();
		}
	}

	private void Start()
	{
		if (!(_animator == null) && _stateNames != null && _stateNames.Length != 0)
		{
			_stateHashes = new int[_stateNames.Length];
			for (int i = 0; i < _stateNames.Length; i++)
			{
				_stateHashes[i] = Animator.StringToHash(_stateNames[i]);
			}
			_lastIndex = UnityEngine.Random.Range(0, _stateNames.Length);
			_animator.SetFloat(_speedParam, UnityEngine.Random.Range(_minSpeed, _maxSpeed));
			_animator.Play(_stateHashes[_lastIndex], _layerIndex);
			StartLoop();
		}
	}

	private void OnDisable()
	{
		StopLoop();
	}

	private void StartLoop()
	{
		if (_loopCoroutine != null)
		{
			StopCoroutine(_loopCoroutine);
			_loopCoroutine = null;
		}
		if (!IsPaused && base.isActiveAndEnabled)
		{
			_loopCoroutine = StartCoroutine(IdleLoopCoroutine());
		}
	}

	private void StopLoop()
	{
		if (_loopCoroutine != null)
		{
			StopCoroutine(_loopCoroutine);
			_loopCoroutine = null;
		}
	}

	private bool IsCurrentStateIdle()
	{
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
		for (int i = 0; i < _stateHashes.Length; i++)
		{
			if (currentAnimatorStateInfo.shortNameHash == _stateHashes[i])
			{
				return true;
			}
		}
		return false;
	}

	private void CrossFadeToNext()
	{
		int num = PickNextIndex();
		_lastIndex = num;
		_animator.SetFloat(_speedParam, UnityEngine.Random.Range(_minSpeed, _maxSpeed));
		_animator.CrossFade(_stateHashes[num], _crossFadeDuration, _layerIndex);
	}

	private IEnumerator IdleLoopCoroutine()
	{
		while (true)
		{
			if (!_animator.IsInTransition(_layerIndex) && _animator.GetCurrentAnimatorStateInfo(_layerIndex).normalizedTime >= _transitionPoint)
			{
				CrossFadeToNext();
			}
			if (_animator.IsInTransition(_layerIndex))
			{
				yield return null;
				continue;
			}
			_animator.SetFloat(_speedParam, UnityEngine.Random.Range(_minSpeed, _maxSpeed));
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
			if (!IsCurrentStateIdle())
			{
				yield return null;
				continue;
			}
			float num = _transitionPoint - currentAnimatorStateInfo.normalizedTime;
			if (!(num <= 0f))
			{
				float num2 = Mathf.Max(currentAnimatorStateInfo.speed * currentAnimatorStateInfo.speedMultiplier, 0.01f);
				yield return new WaitForSeconds(currentAnimatorStateInfo.length * num / num2);
			}
		}
	}

	private int PickNextIndex()
	{
		if (_stateHashes.Length == 1)
		{
			return 0;
		}
		int num = UnityEngine.Random.Range(0, _stateHashes.Length - 1);
		if (num >= _lastIndex && _lastIndex >= 0)
		{
			num++;
		}
		return num;
	}
}

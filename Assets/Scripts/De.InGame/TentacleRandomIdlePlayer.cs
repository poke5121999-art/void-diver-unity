using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class TentacleRandomIdlePlayer : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CIdleLoopCoroutine_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TentacleRandomIdlePlayer _003C_003E4__this;

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
		public _003CIdleLoopCoroutine_003Ed__24(int _003C_003E1__state)
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

	[Header("Animator")]
	[Tooltip("제어할 Animator (비어있으면 자신에서 탐색)")]
	[FormerlySerializedAs("animator")]
	[SerializeField]
	private Animator _animator;

	[Tooltip("재생할 Animator Layer 인덱스 (Base=0, Head=1, ...)")]
	[FormerlySerializedAs("layerIndex")]
	[SerializeField]
	private int _layerIndex;

	[Header("Idle Clips")]
	[Tooltip("랜덤 재생할 Animator State 이름 목록")]
	[FormerlySerializedAs("stateNames")]
	[SerializeField]
	private string[] _stateNames;

	[Header("Speed")]
	[Tooltip("최소 재생 속도")]
	[FormerlySerializedAs("minSpeed")]
	[SerializeField]
	private float _minSpeed;

	[Tooltip("최대 재생 속도")]
	[FormerlySerializedAs("maxSpeed")]
	[SerializeField]
	private float _maxSpeed;

	[Tooltip("속도를 적용할 Animator Float 파라미터 이름 (State의 Speed Multiplier Parameter로 설정)")]
	[FormerlySerializedAs("IdleSpeed")]
	[SerializeField]
	private string _speedParam;

	[Header("Transition")]
	[Tooltip("CrossFade 전환 시간 (초)")]
	[FormerlySerializedAs("crossFadeDuration")]
	[SerializeField]
	private float _crossFadeDuration;

	[Tooltip("전환 트리거 시점 (normalizedTime 기준)")]
	[Range(0.5f, 1f)]
	[FormerlySerializedAs("transitionPoint")]
	[SerializeField]
	private float _transitionPoint;

	private int[] _stateHashes;

	private int _lastIndex;

	private Coroutine _loopCoroutine;

	public bool IsPaused { get; private set; }

	public void Pause()
	{
	}

	public void Resume()
	{
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void StartLoop()
	{
	}

	private void StopLoop()
	{
	}

	private bool IsCurrentStateIdle()
	{
		return false;
	}

	private void CrossFadeToNext()
	{
	}

	[IteratorStateMachine(typeof(_003CIdleLoopCoroutine_003Ed__24))]
	private IEnumerator IdleLoopCoroutine()
	{
		return null;
	}

	private int PickNextIndex()
	{
		return 0;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectRandomSeedController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDisableAfter_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delay;

		public ParticleSystem ps;

		public EffectRandomSeedController _003C_003E4__this;

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
		public _003CDisableAfter_003Ed__12(int _003C_003E1__state)
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
	private sealed class _003CRunLoopingPool_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EffectRandomSeedController _003C_003E4__this;

		private float _003CloopInterval_003E5__2;

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
		public _003CRunLoopingPool_003Ed__11(int _003C_003E1__state)
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

	[Header("필수 설정")]
	public ParticleSystem particlePrefab;

	public int[] allowedSeeds;

	public bool isLooping;

	private Queue<ParticleSystem> pool;

	private Coroutine loopCoroutine;

	private bool hasInitializedPool;

	private Dictionary<ParticleSystem, Coroutine> disableCoroutines;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void PlayOnceWithRandomSeed()
	{
	}

	private void InitLoopingPool()
	{
	}

	[IteratorStateMachine(typeof(_003CRunLoopingPool_003Ed__11))]
	private IEnumerator RunLoopingPool()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDisableAfter_003Ed__12))]
	private IEnumerator DisableAfter(ParticleSystem ps, float delay)
	{
		return null;
	}

	private string GetGameObjectPath(Transform transform)
	{
		return null;
	}
}

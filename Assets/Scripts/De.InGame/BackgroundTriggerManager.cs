using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackgroundTriggerManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CFade_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BackgroundTriggerManager _003C_003E4__this;

		public float endFactor;

		private float _003CstartFactor_003E5__2;

		private float _003Ct_003E5__3;

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
		public _003CFade_003Ed__17(int _003C_003E1__state)
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

	public static BackgroundTriggerManager Instance;

	[SerializeField]
	private float _fadeTime;

	private readonly int _globalCoverSeeThroughFactorID;

	private readonly Dictionary<Collider, HashSet<BackgroundTriggerUnit>> _unitSets;

	private bool _isHidden;

	private float _currentFactor;

	private IEnumerator _fadeCo;

	public float FadeTime
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public int EnterCount => 0;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	public void NotifyEnter(Collider col, BackgroundTriggerUnit unit)
	{
	}

	public void NotifyExit(Collider col, BackgroundTriggerUnit unit)
	{
	}

	private void UpdateFade()
	{
	}

	[IteratorStateMachine(typeof(_003CFade_003Ed__17))]
	private IEnumerator Fade(float endFactor)
	{
		return null;
	}
}

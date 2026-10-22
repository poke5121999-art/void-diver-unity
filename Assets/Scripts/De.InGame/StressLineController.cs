using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StressLineController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHitEffectCoroutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StressLineController _003C_003E4__this;

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
		public _003CHitEffectCoroutine_003Ed__16(int _003C_003E1__state)
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

	[FormerlySerializedAs("_sanityLineImages")]
	[Header("UI References")]
	[SerializeField]
	private List<Image> _stressLineImages;

	[SerializeField]
	private AnimationCurve _stressLineCurve;

	private List<Material> _instancedMaterials;

	[SerializeField]
	private float _hitDuration;

	private float _currentHitIntensity;

	private bool _isDead;

	private EStressState _stressState;

	private float _stress;

	private IEnumerator _hitEffectCouroutine;

	private readonly int _intensityProperty;

	private readonly int _hitIntensityProperty;

	private readonly int _blackoutProperty;

	private void Awake()
	{
	}

	public void SetStress(float stress)
	{
	}

	public void SetStressState(EStressState stressState)
	{
	}

	public void PlayHitEffect()
	{
	}

	[IteratorStateMachine(typeof(_003CHitEffectCoroutine_003Ed__16))]
	private IEnumerator HitEffectCoroutine()
	{
		return null;
	}

	public void SetDeathState(bool death)
	{
	}
}

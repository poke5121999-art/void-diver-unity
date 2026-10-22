using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CameraPostProcessingModel))]
[ExecuteInEditMode]
public class CameraPostProcessingController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDamageHpPostProcessing_CO_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CameraPostProcessingController _003C_003E4__this;

		private float _003CcurrentRatio_003E5__2;

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
		public _003CDamageHpPostProcessing_CO_003Ed__51(int _003C_003E1__state)
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
	private sealed class _003CHealingHpPostProcessing_CO_003Ed__55 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CameraPostProcessingController _003C_003E4__this;

		private float _003CcurrentRatio_003E5__2;

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
		public _003CHealingHpPostProcessing_CO_003Ed__55(int _003C_003E1__state)
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

	private CameraPostProcessingModel _cameraPostProcessingModel;

	private static readonly int _screenBlurProgressPropertyID;

	private static readonly int _screenBlurCustomProgressPropertyID;

	private static readonly int _lowHPVignetteIntensityPropertyID;

	private static readonly int _sightVignetteIntensityPropertyID;

	[Header("Ingame Post Processing Setting: Screen Blur Intensity")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _screenBlurIntensity;

	[FormerlySerializedAs("SanityVisualIntensity")]
	[Header("Ingame Post Processing Setting: Stress Intensity")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _stressVisualIntensity;

	[SerializeField]
	[Range(0f, 1f)]
	private float _oneShotStressVisualIntensity;

	[Header("Ingame Post Processing Setting: Low Hp")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _lowHpVisualIntensity;

	[Header("Ingame Post Processing Setting: Sight Enable Hp")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _sightEnableIntensity;

	[Header("Ingame Post Processing Setting: Damage Hp")]
	[SerializeField]
	private float _damageHpDuration;

	[SerializeField]
	private AnimationCurve _damageHpIntensityCurve;

	[Header("Ingame Post Processing Setting: Healing Hp")]
	[SerializeField]
	private float _healingHpDuration;

	[SerializeField]
	private AnimationCurve _healingHpIntensityCurve;

	private IEnumerator _damageHpPostProcessing_CO;

	private IEnumerator _healingHpPostProcessing_CO;

	public float ScreenBlurIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float StressVisualIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float OneShotStressVisualIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float LowHpVisualIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float SightEnableIntensity
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	private void Awake()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public void Clear()
	{
	}

	private void UpdateScreenBlur()
	{
	}

	private void ResetScreenBlur()
	{
	}

	private bool IsStressPostProcessingValid()
	{
		return false;
	}

	private void UpdateStressPostProcessing()
	{
	}

	private void ResetStressPostProcessing()
	{
	}

	private bool IsOneShotStressEffectValid()
	{
		return false;
	}

	private void UpdateOneShotStressPostProcessing()
	{
	}

	private void ResetOneShotStressPostProcessing()
	{
	}

	private bool IsLowHpPostProcessingValid()
	{
		return false;
	}

	private void UpdateLowHpPostProcessing()
	{
	}

	private void ResetLowHpPostProcessing()
	{
	}

	private bool IsSightEnablePostProcessingValid()
	{
		return false;
	}

	private void UpdateSightEnablePostProcessing()
	{
	}

	private void ResetSightEnablePostProcessing()
	{
	}

	private bool IsDamageHpPostProcessingValid()
	{
		return false;
	}

	public void PlayDamageHpPostProcessing()
	{
	}

	[IteratorStateMachine(typeof(_003CDamageHpPostProcessing_CO_003Ed__51))]
	private IEnumerator DamageHpPostProcessing_CO()
	{
		return null;
	}

	private void ResetDamageHpPostProcessing()
	{
	}

	private bool IsHealingHpPostProcessingValid()
	{
		return false;
	}

	public void PlayHealingHpPostProcessing()
	{
	}

	[IteratorStateMachine(typeof(_003CHealingHpPostProcessing_CO_003Ed__55))]
	private IEnumerator HealingHpPostProcessing_CO()
	{
		return null;
	}

	private void ResetHealingHpPostProcessing()
	{
	}

	public void SetGameplayPostProcessingVolumeEnable(bool enable)
	{
	}
}

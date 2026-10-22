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
	private CameraPostProcessingModel _cameraPostProcessingModel;

	private static readonly int _screenBlurProgressPropertyID = Shader.PropertyToID("_ScreenBlurProgress");

	private static readonly int _screenBlurCustomProgressPropertyID = Shader.PropertyToID("_ScreenBlurCustomShaderProgress");

	private static readonly int _lowHPVignetteIntensityPropertyID = Shader.PropertyToID("_LowHPVignetteIntensity");

	private static readonly int _sightVignetteIntensityPropertyID = Shader.PropertyToID("_SightVignetteIntensity");

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
	private float _damageHpDuration = 0.25f;

	[SerializeField]
	private AnimationCurve _damageHpIntensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

	[Header("Ingame Post Processing Setting: Healing Hp")]
	[SerializeField]
	private float _healingHpDuration = 0.25f;

	[SerializeField]
	private AnimationCurve _healingHpIntensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

	private IEnumerator _damageHpPostProcessing_CO;

	private IEnumerator _healingHpPostProcessing_CO;

	public float ScreenBlurIntensity
	{
		get
		{
			return _screenBlurIntensity;
		}
		set
		{
			if (Mathf.Abs(_screenBlurIntensity - value) > 0.0001f)
			{
				_screenBlurIntensity = value;
				UpdateScreenBlur();
			}
		}
	}

	public float StressVisualIntensity
	{
		get
		{
			return _stressVisualIntensity;
		}
		set
		{
			if (Mathf.Abs(_stressVisualIntensity - value) > 0.0001f)
			{
				_stressVisualIntensity = value;
				UpdateStressPostProcessing();
			}
		}
	}

	public float OneShotStressVisualIntensity
	{
		get
		{
			return _oneShotStressVisualIntensity;
		}
		set
		{
			if (Mathf.Abs(_oneShotStressVisualIntensity - value) > 0.0001f)
			{
				_oneShotStressVisualIntensity = value;
				UpdateOneShotStressPostProcessing();
			}
		}
	}

	public float LowHpVisualIntensity
	{
		get
		{
			return _lowHpVisualIntensity;
		}
		set
		{
			if (Mathf.Abs(_lowHpVisualIntensity - value) > 0.0001f)
			{
				_lowHpVisualIntensity = value;
				UpdateLowHpPostProcessing();
			}
		}
	}

	public float SightEnableIntensity
	{
		get
		{
			return _sightEnableIntensity;
		}
		set
		{
			if (Mathf.Abs(_sightEnableIntensity - value) > 0.0001f)
			{
				_sightEnableIntensity = value;
				UpdateSightEnablePostProcessing();
			}
		}
	}

	private void Awake()
	{
		_cameraPostProcessingModel = GetComponent<CameraPostProcessingModel>();
	}

	private void OnDisable()
	{
		Clear();
	}

	private void OnDestroy()
	{
		Clear();
	}

	public void Clear()
	{
		ResetScreenBlur();
		ResetStressPostProcessing();
		ResetOneShotStressPostProcessing();
		ResetLowHpPostProcessing();
		ResetSightEnablePostProcessing();
		ResetDamageHpPostProcessing();
		ResetHealingHpPostProcessing();
		ScreenBlurIntensity = 0f;
		StressVisualIntensity = 0f;
		OneShotStressVisualIntensity = 0f;
		LowHpVisualIntensity = 0f;
		SightEnableIntensity = 1f;
	}

	private void UpdateScreenBlur()
	{
		Shader.SetGlobalFloat(_screenBlurProgressPropertyID, _screenBlurIntensity);
		Shader.SetGlobalFloat(_screenBlurCustomProgressPropertyID, _screenBlurIntensity);
	}

	private void ResetScreenBlur()
	{
		Shader.SetGlobalFloat(_screenBlurProgressPropertyID, 0f);
		Shader.SetGlobalFloat(_screenBlurCustomProgressPropertyID, 0f);
	}

	private bool IsStressPostProcessingValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.StressPostProcessing;
	}

	private void UpdateStressPostProcessing()
	{
		if (IsStressPostProcessingValid())
		{
			_cameraPostProcessingModel.StressPostProcessing.UpdateProgress(_stressVisualIntensity);
		}
	}

	private void ResetStressPostProcessing()
	{
		if (IsStressPostProcessingValid())
		{
			_cameraPostProcessingModel.StressPostProcessing.ResetProgress();
		}
	}

	private bool IsOneShotStressEffectValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.OneShotStressVolume;
	}

	private void UpdateOneShotStressPostProcessing()
	{
		if (IsOneShotStressEffectValid())
		{
			_cameraPostProcessingModel.OneShotStressVolume.weight = _oneShotStressVisualIntensity;
		}
	}

	private void ResetOneShotStressPostProcessing()
	{
		if (IsOneShotStressEffectValid())
		{
			_cameraPostProcessingModel.OneShotStressVolume.weight = 0f;
		}
	}

	private bool IsLowHpPostProcessingValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.LowHpVolume;
	}

	private void UpdateLowHpPostProcessing()
	{
		Shader.SetGlobalFloat(_lowHPVignetteIntensityPropertyID, _lowHpVisualIntensity);
		if (IsLowHpPostProcessingValid())
		{
			_cameraPostProcessingModel.LowHpVolume.weight = _lowHpVisualIntensity;
		}
	}

	private void ResetLowHpPostProcessing()
	{
		Shader.SetGlobalFloat(_lowHPVignetteIntensityPropertyID, 0f);
		if (IsLowHpPostProcessingValid())
		{
			_cameraPostProcessingModel.LowHpVolume.weight = 0f;
		}
	}

	private bool IsSightEnablePostProcessingValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.SightEnableVolume;
	}

	private void UpdateSightEnablePostProcessing()
	{
		Shader.SetGlobalFloat(_sightVignetteIntensityPropertyID, _sightEnableIntensity);
		if (IsSightEnablePostProcessingValid())
		{
			_cameraPostProcessingModel.SightEnableVolume.weight = Mathf.Clamp01(1f - _sightEnableIntensity);
		}
	}

	private void ResetSightEnablePostProcessing()
	{
		Shader.SetGlobalFloat(_sightVignetteIntensityPropertyID, 0f);
		if (IsSightEnablePostProcessingValid())
		{
			_cameraPostProcessingModel.SightEnableVolume.weight = 0f;
		}
	}

	private bool IsDamageHpPostProcessingValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.DamageHpVolume;
	}

	public void PlayDamageHpPostProcessing()
	{
		if (IsDamageHpPostProcessingValid())
		{
			if (_damageHpPostProcessing_CO != null)
			{
				StopCoroutine(_damageHpPostProcessing_CO);
			}
			_damageHpPostProcessing_CO = DamageHpPostProcessing_CO();
			StartCoroutine(_damageHpPostProcessing_CO);
		}
	}

	private IEnumerator DamageHpPostProcessing_CO()
	{
		float currentRatio = 0f;
		while (currentRatio < 1f)
		{
			currentRatio += Time.deltaTime / Mathf.Max(0.001f, _damageHpDuration);
			_cameraPostProcessingModel.DamageHpVolume.weight = _damageHpIntensityCurve.Evaluate(currentRatio);
			yield return null;
		}
		_cameraPostProcessingModel.DamageHpVolume.weight = 0f;
	}

	private void ResetDamageHpPostProcessing()
	{
		if (_damageHpPostProcessing_CO != null)
		{
			StopCoroutine(_damageHpPostProcessing_CO);
		}
	}

	private bool IsHealingHpPostProcessingValid()
	{
		return (bool)_cameraPostProcessingModel && (bool)_cameraPostProcessingModel.HealingHpVolume;
	}

	public void PlayHealingHpPostProcessing()
	{
		if (IsHealingHpPostProcessingValid())
		{
			if (_healingHpPostProcessing_CO != null)
			{
				StopCoroutine(_healingHpPostProcessing_CO);
			}
			_healingHpPostProcessing_CO = HealingHpPostProcessing_CO();
			StartCoroutine(_healingHpPostProcessing_CO);
		}
	}

	private IEnumerator HealingHpPostProcessing_CO()
	{
		float currentRatio = 0f;
		while (currentRatio < 1f)
		{
			currentRatio += Time.deltaTime / Mathf.Max(0.001f, _healingHpDuration);
			_cameraPostProcessingModel.HealingHpVolume.weight = _healingHpIntensityCurve.Evaluate(currentRatio);
			yield return null;
		}
		_cameraPostProcessingModel.HealingHpVolume.weight = 0f;
	}

	private void ResetHealingHpPostProcessing()
	{
		if (_healingHpPostProcessing_CO != null)
		{
			StopCoroutine(_healingHpPostProcessing_CO);
		}
	}

	public void SetGameplayPostProcessingVolumeEnable(bool enable)
	{
		if (_cameraPostProcessingModel == null)
		{
			return;
		}
		if ((bool)_cameraPostProcessingModel.LowHpVolume)
		{
			_cameraPostProcessingModel.LowHpVolume.gameObject.SetActive(enable);
		}
		if ((bool)_cameraPostProcessingModel.SightEnableVolume)
		{
			_cameraPostProcessingModel.SightEnableVolume.gameObject.SetActive(enable);
		}
		if ((bool)_cameraPostProcessingModel.DamageHpVolume)
		{
			_cameraPostProcessingModel.DamageHpVolume.gameObject.SetActive(enable);
		}
		if ((bool)_cameraPostProcessingModel.HealingHpVolume)
		{
			_cameraPostProcessingModel.HealingHpVolume.gameObject.SetActive(enable);
		}
		if ((bool)_cameraPostProcessingModel.OneShotStressVolume)
		{
			_cameraPostProcessingModel.OneShotStressVolume.gameObject.SetActive(enable);
		}
	}
}

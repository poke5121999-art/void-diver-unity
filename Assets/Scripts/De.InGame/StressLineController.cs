using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon.Table;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StressLineController : MonoBehaviour
{
	[FormerlySerializedAs("_sanityLineImages")]
	[Header("UI References")]
	[SerializeField]
	private List<Image> _stressLineImages = new List<Image>();

	[SerializeField]
	private AnimationCurve _stressLineCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	private List<Material> _instancedMaterials = new List<Material>();

	[SerializeField]
	private float _hitDuration = 0.25f;

	private float _currentHitIntensity;

	private bool _isDead;

	private EStressState _stressState;

	private float _stress;

	private IEnumerator _hitEffectCouroutine;

	private readonly int _intensityProperty = Shader.PropertyToID("_Intensity");

	private readonly int _hitIntensityProperty = Shader.PropertyToID("_HitIntensity");

	private readonly int _blackoutProperty = Shader.PropertyToID("_Blackout");

	private void Awake()
	{
		_stressLineImages.ForEach(delegate(Image image)
		{
			Material material = new Material(image.material);
			image.material = material;
			_instancedMaterials.Add(material);
		});
	}

	public void SetStress(float stress)
	{
		_stress = stress;
		int stressMax = TConst.StressMax.IntValue;
		float stressIntensity = _stressLineCurve.Evaluate(stress / (float)stressMax);
		_instancedMaterials.ForEach(delegate(Material material)
		{
			if (!(material == null))
			{
				material.SetFloat(_intensityProperty, stressIntensity);
				material.SetFloat(_blackoutProperty, (stress < (float)stressMax) ? 0f : 1f);
			}
		});
	}

	public void SetStressState(EStressState stressState)
	{
		_stressState = stressState;
	}

	public void PlayHitEffect()
	{
		if (!_isDead)
		{
			if (_hitEffectCouroutine != null)
			{
				StopCoroutine(_hitEffectCouroutine);
			}
			if (base.isActiveAndEnabled)
			{
				_hitEffectCouroutine = HitEffectCoroutine();
				StartCoroutine(_hitEffectCouroutine);
			}
		}
	}

	private IEnumerator HitEffectCoroutine()
	{
		_currentHitIntensity = 1f;
		while (_currentHitIntensity > 0f)
		{
			_instancedMaterials.ForEach(delegate(Material material)
			{
				if (!(material == null))
				{
					material.SetFloat(_hitIntensityProperty, _currentHitIntensity);
				}
			});
			yield return null;
			_currentHitIntensity -= Time.deltaTime / Mathf.Max(_hitDuration, 0.001f);
		}
		_currentHitIntensity = 0f;
		_instancedMaterials.ForEach(delegate(Material material)
		{
			if (!(material == null))
			{
				material.SetFloat(_hitIntensityProperty, _currentHitIntensity);
			}
		});
	}

	public void SetDeathState(bool death)
	{
		_isDead = death;
		if (death)
		{
			_instancedMaterials.ForEach(delegate(Material material)
			{
				if (!(material == null))
				{
					material.SetFloat(_intensityProperty, 0f);
					material.SetFloat(_blackoutProperty, 1f);
				}
			});
		}
		else
		{
			SetStress(_stress);
		}
	}
}

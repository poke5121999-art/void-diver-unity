using DG.Tweening;
using GameCommon.Enum;
using UnityEngine;

public class EnemyVisualEffectView : UnitVisualEffectView
{
	[Header("Light Effect Setting")]
	[SerializeField]
	private bool _lightEffectEnable = true;

	private Tween _lightEffectTween;

	private readonly int _lightIntensityID = Shader.PropertyToID("_LightEffectIntensity");

	[SerializeField]
	private float _lightEffectDuration = 0.25f;

	[SerializeField]
	private AnimationCurve _lightEffectCurve = new AnimationCurve();

	protected override void Initialize()
	{
		base.Initialize();
		if (SkeletonMaterialOverrider == null || SkeletonMaterialOverrider.InstnancedMaterials == null)
		{
			return;
		}
		foreach (Material material in SkeletonMaterialOverrider.InstnancedMaterials)
		{
			if (material != null)
			{
				material.SetFloat(IsEnemyProperty, 1f);
			}
		}
	}

	protected override void OnStatusEffectTagAdded(EStatusEffectTag statusEffectTag)
	{
		base.OnStatusEffectTagAdded(statusEffectTag);
		if (statusEffectTag == EStatusEffectTag.Light)
		{
			PlayLightVisualEffect();
		}
	}

	protected override void OnStatusEffectTagRemoved(EStatusEffectTag statusEffectTag)
	{
		base.OnStatusEffectTagRemoved(statusEffectTag);
	}

	private void ClearLightVisualEffect()
	{
		if (_lightEffectTween != null && _lightEffectTween.IsActive())
		{
			_lightEffectTween.Kill();
		}
		_lightEffectTween = null;
	}

	private void PlayLightVisualEffect()
	{
		ClearLightVisualEffect();
		if (!_lightEffectEnable)
		{
			return;
		}
		_lightEffectTween = DOVirtual.Float(0f, 1f, _lightEffectDuration, delegate(float value)
		{
			if (SkeletonMaterialOverrider != null && SkeletonMaterialOverrider.InstnancedMaterials != null)
			{
				foreach (Material material in SkeletonMaterialOverrider.InstnancedMaterials)
				{
					if (material != null)
					{
						material.SetFloat(_lightIntensityID, value);
					}
				}
			}
		}).SetEase(_lightEffectCurve).OnStart(delegate
		{
			foreach (Material material in SkeletonMaterialOverrider.InstnancedMaterials)
			{
				if (material != null)
				{
					material.SetFloat(_lightIntensityID, 0f);
				}
			}
		}).Play();
	}
}

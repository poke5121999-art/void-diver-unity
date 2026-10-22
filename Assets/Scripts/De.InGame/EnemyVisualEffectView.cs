using DG.Tweening;
using GameCommon.Enum;
using UnityEngine;

public class EnemyVisualEffectView : UnitVisualEffectView
{
	[Header("Light Effect Setting")]
	[SerializeField]
	private bool _lightEffectEnable;

	private Tween _lightEffectTween;

	private readonly int _lightIntensityID;

	[SerializeField]
	private float _lightEffectDuration;

	[SerializeField]
	private AnimationCurve _lightEffectCurve;

	protected override void Initialize()
	{
	}

	protected override void OnStatusEffectTagAdded(EStatusEffectTag statusEffectTag)
	{
	}

	protected override void OnStatusEffectTagRemoved(EStatusEffectTag statusEffectTag)
	{
	}

	private void ClearLightVisualEffect()
	{
	}

	private void PlayLightVisualEffect()
	{
	}
}

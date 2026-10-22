using DG.Tweening;
using UnityEngine;

public abstract class TweenLight : MonoBase
{
	[SerializeField]
	protected Light Light;

	[SerializeField]
	protected float EnableIntensity;

	[SerializeField]
	protected float EnableDuration;

	[SerializeField]
	protected AnimationCurve EnableIntensityCurve;

	[SerializeField]
	protected float DisableIntensity;

	[SerializeField]
	protected float DisableDuration;

	[SerializeField]
	protected AnimationCurve DisableIntensityCurve;

	private Tweener _tweener;

	protected void TurnOn()
	{
	}

	protected void TurnOff()
	{
	}
}

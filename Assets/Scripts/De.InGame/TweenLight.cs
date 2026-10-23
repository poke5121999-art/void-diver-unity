using DG.Tweening;
using DG.Tweening;
using UnityEngine;

public abstract class TweenLight : MonoBase
{
	[SerializeField]
	protected Light Light;

	[SerializeField]
	protected float EnableIntensity = 1f;

	[SerializeField]
	protected float EnableDuration = 0.5f;

	[SerializeField]
	protected AnimationCurve EnableIntensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	protected float DisableIntensity;

	[SerializeField]
	protected float DisableDuration = 0.5f;

	[SerializeField]
	protected AnimationCurve DisableIntensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	private Tweener _tweener;

	protected void TurnOn()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		Light.enabled = true;
		_tweener = Light.DOIntensity(EnableIntensity, EnableDuration).SetEase(EnableIntensityCurve);
	}

	protected void TurnOff()
	{
		if (Light.enabled)
		{
			if (_tweener != null)
			{
				_tweener.Kill();
			}
			_tweener = Light.DOIntensity(DisableIntensity, DisableDuration).SetEase(DisableIntensityCurve).OnComplete(delegate
			{
				Light.enabled = false;
			});
		}
	}
}

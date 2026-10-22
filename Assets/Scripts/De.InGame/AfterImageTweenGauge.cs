using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AfterImageTweenGauge : BaseTweenGauge
{
	[SerializeField]
	protected Image frontBar;

	[SerializeField]
	protected Image backBar;

	[SerializeField]
	private float tweenDuration = 0.25f;

	protected float prevRatio;

	private bool _initialized;

	private void OnEnable()
	{
		if (!_initialized)
		{
			float num = prevRatio;
			if (frontBar != null)
			{
				frontBar.fillAmount = num;
			}
			if (backBar != null)
			{
				backBar.fillAmount = num;
			}
			_initialized = true;
		}
	}

	public override void SetRatio(float ratio, bool immediate = false)
	{
		if (ratio < 0f)
		{
			ratio = 0f;
		}
		else if (1f < ratio)
		{
			ratio = 1f;
		}
		if (frontBar == null || backBar == null)
		{
			prevRatio = ratio;
			return;
		}
		Image image;
		Image other;
		if (ratio - prevRatio < 0f)
		{
			image = frontBar;
			other = backBar;
		}
		else
		{
			image = backBar;
			other = frontBar;
		}
		DOTween.Kill(image);
		DOTween.Kill(other);
		image.fillAmount = ratio;
		if (immediate || tweenDuration <= 0f || ratio <= 0f)
		{
			other.fillAmount = ratio;
			prevRatio = ratio;
			return;
		}
		float startValue = other.fillAmount;
		prevRatio = ratio;
		DOTween.To(() => startValue, delegate(float x)
		{
			other.fillAmount = x;
		}, ratio, tweenDuration).SetTarget(other).SetLink(other.gameObject);
	}

	private void SetFillAmount(float ratio, bool immediate)
	{
		if (frontBar != null)
		{
			frontBar.fillAmount = ratio;
		}
		if (backBar != null)
		{
			backBar.fillAmount = ratio;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;

public class AfterImageTweenGauge : BaseTweenGauge
{
	[SerializeField]
	protected Image frontBar;

	[SerializeField]
	protected Image backBar;

	[SerializeField]
	private float tweenDuration;

	protected float prevRatio;

	private bool _initialized;

	private void OnEnable()
	{
	}

	public override void SetRatio(float ratio, bool immediate = false)
	{
	}

	private void SetFillAmount(float ratio, bool immediate)
	{
	}
}

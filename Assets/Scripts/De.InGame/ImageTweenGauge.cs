using UnityEngine;
using UnityEngine.UI;

public class ImageTweenGauge : BaseTweenGauge
{
	[SerializeField]
	private Image _targetImage;

	[SerializeField]
	private float _tweenDuration;

	private float _prevRatio;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public override void SetRatio(float ratio, bool immediate = false)
	{
	}
}

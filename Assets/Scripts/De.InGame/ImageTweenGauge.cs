using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageTweenGauge : BaseTweenGauge
{
	[SerializeField]
	private Image _targetImage;

	[SerializeField]
	private float _tweenDuration = 0.25f;

	private float _prevRatio;

	private void OnEnable()
	{
		_targetImage.fillAmount = Mathf.Clamp01(_prevRatio);
	}

	private void OnDisable()
	{
		if ((bool)_targetImage)
		{
			DOTween.Kill(_targetImage, complete: true);
		}
	}

	public override void SetRatio(float ratio, bool immediate = false)
	{
		ratio = Mathf.Clamp01(ratio);
		if (_targetImage == null)
		{
			_prevRatio = ratio;
			return;
		}
		DOTween.Kill(_targetImage);
		if (immediate || _tweenDuration <= 0f)
		{
			_targetImage.fillAmount = ratio;
			_prevRatio = ratio;
			return;
		}
		DOTween.To(() => _targetImage.fillAmount, delegate(float v)
		{
			_targetImage.fillAmount = Mathf.Clamp01(v);
		}, ratio, _tweenDuration).SetTarget(_targetImage).SetLink(_targetImage.gameObject).OnComplete(delegate
		{
			_prevRatio = ratio;
		});
	}
}

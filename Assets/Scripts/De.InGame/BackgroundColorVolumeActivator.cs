using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(Volume))]
public class BackgroundColorVolumeActivator : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve _animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private Volume _volume;

	private Tween _currentTween;

	private void OnEnable()
	{
		float lastKeyValue = 0f;
		DOTween.Kill(_volume);
		float duration = ((_animationCurve.keys.Length != 0) ? _animationCurve.keys[_animationCurve.keys.Length - 1].time : 1f);
		if (duration <= 0f)
		{
			duration = 1f;
		}
		if (_animationCurve.keys.Length != 0)
		{
			lastKeyValue = _animationCurve.keys[_animationCurve.keys.Length - 1].value;
		}
		_currentTween = DOTween.To(() => _volume.weight, delegate(float x)
		{
			_volume.weight = x;
		}, 1f, duration).SetEase(_animationCurve).OnComplete(delegate
		{
			_volume.weight = lastKeyValue;
		});
	}

	private void OnDisable()
	{
		DOTween.Kill(_volume);
	}
}

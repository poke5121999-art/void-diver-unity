using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(Volume))]
public class BackgroundColorVolumeActivator : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve _animationCurve;

	[SerializeField]
	private Volume _volume;

	private Tween _currentTween;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}
}

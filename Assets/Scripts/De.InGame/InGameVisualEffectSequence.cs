using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class InGameVisualEffectSequence
{
	[Range(0f, 1f)]
	public float ActiveRatio = 0.5f;

	[Space(10f)]
	[Range(0f, 1f)]
	public float IntensityMin;

	[Range(0f, 1f)]
	public float IntensityMax = 1f;

	[Space(10f)]
	[SerializeField]
	private float _randomDelayMin = 3f;

	[SerializeField]
	private float _randomDelayMax = 5f;

	[Space(10f)]
	public float TransitionDuration = 0.5f;

	public Ease EaseToMax = Ease.InOutSine;

	[Space(10f)]
	[SerializeField]
	private float _randomDurationMin = 3f;

	[SerializeField]
	private float _randomDurationMax = 5f;

	[Space(10f)]
	public float TransitionAfterDuration = 0.5f;

	public Ease EaseToMin = Ease.InOutSine;

	public float Delay => UnityEngine.Random.Range(_randomDelayMin, _randomDelayMax);

	public float Duration => UnityEngine.Random.Range(_randomDurationMin, _randomDurationMax);
}

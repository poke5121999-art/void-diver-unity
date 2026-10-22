using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class InGameVisualEffectSequence
{
	[Range(0f, 1f)]
	public float ActiveRatio;

	[Space(10f)]
	[Range(0f, 1f)]
	public float IntensityMin;

	[Range(0f, 1f)]
	public float IntensityMax;

	[Space(10f)]
	[SerializeField]
	private float _randomDelayMin;

	[SerializeField]
	private float _randomDelayMax;

	[Space(10f)]
	public float TransitionDuration;

	public Ease EaseToMax;

	[Space(10f)]
	[SerializeField]
	private float _randomDurationMin;

	[SerializeField]
	private float _randomDurationMax;

	[Space(10f)]
	public float TransitionAfterDuration;

	public Ease EaseToMin;

	public float Delay => 0f;

	public float Duration => 0f;
}

using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class CameraEffectSequence
{
	[Range(0f, 1f)]
	public float ActiveRatio;

	[Space(10f)]
	[Range(0f, 1f)]
	public float IntensityMin;

	[Range(0f, 1f)]
	public float IntensityMax;

	[Space(10f)]
	public float StartDelay;

	[Space(10f)]
	public Ease EaseToMax;

	public float DurationToMax;

	[Space(10f)]
	public float DelayBetween;

	[Space(10f)]
	public Ease EaseToMin;

	public float DurationToMin;

	[Space(10f)]
	public float DelayAfter;
}

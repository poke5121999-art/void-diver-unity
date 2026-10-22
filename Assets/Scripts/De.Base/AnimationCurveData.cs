using System;
using UnityEngine;

[Serializable]
public class AnimationCurveData
{
	public Keyframe[] Keys;

	public AnimationCurveData(AnimationCurve curve)
	{
	}

	public AnimationCurve ToAnimationCurve()
	{
		return null;
	}
}

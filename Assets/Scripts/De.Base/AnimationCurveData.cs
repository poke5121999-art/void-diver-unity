using System;
using UnityEngine;

[Serializable]
public class AnimationCurveData
{
	public Keyframe[] Keys;

	public AnimationCurveData(AnimationCurve curve)
	{
		Keys = ((curve != null) ? curve.keys : null);
	}

	public AnimationCurve ToAnimationCurve()
	{
		return new AnimationCurve(Keys);
	}
}

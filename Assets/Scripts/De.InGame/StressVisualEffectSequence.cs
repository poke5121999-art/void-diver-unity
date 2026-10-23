using System;
using GameCommon.Enum;

[Serializable]
public class StressVisualEffectSequence
{
	public EStressState State;

	public InGameVisualEffectSequence Sequence;

	public StressVisualEffectSequence(EStressState stressState, float intensityMin = 0f, float intensityMax = 1f)
	{
		Sequence = new InGameVisualEffectSequence();
		State = stressState;
		Sequence.IntensityMin = intensityMin;
		Sequence.IntensityMax = intensityMax;
	}
}

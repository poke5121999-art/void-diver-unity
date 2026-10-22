using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/ArtifactDealSequenceTimeConfig")]
public class ArtifactDealSequenceTimeConfig : ScriptableObject
{
	[Serializable]
	public struct StepDelay
	{
		public EArtifactDealStepDelay Step;

		public float Seconds;
	}

	[SerializeField]
	private StepDelay[] _stepDelays;

	public float GetSeconds(EArtifactDealStepDelay step)
	{
		StepDelay[] stepDelays = _stepDelays;
		for (int i = 0; i < stepDelays.Length; i++)
		{
			StepDelay stepDelay = stepDelays[i];
			if (stepDelay.Step == step)
			{
				return stepDelay.Seconds;
			}
		}
		throw new KeyNotFoundException(string.Format("ArtifactDealSequenceTimeConfig: {0}", step));
	}
}

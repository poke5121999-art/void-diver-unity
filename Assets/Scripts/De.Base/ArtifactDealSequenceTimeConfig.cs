using System;
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
		return 0f;
	}
}

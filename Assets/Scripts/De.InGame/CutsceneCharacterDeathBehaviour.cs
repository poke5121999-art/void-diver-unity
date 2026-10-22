using UnityEngine;
using UnityEngine.Playables;

public class CutsceneCharacterDeathBehaviour : PlayableBehaviour
{
	public Color DeathColor;

	public float DeathProgress;

	private CutsceneCharacterMaterialController _controller;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
	}
}

using UnityEngine;
using UnityEngine.Playables;

public class SfxPlayableBehaviour : PlayableBehaviour
{
	public string Sfx;

	public Transform Transform;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (info.evaluationType == FrameData.EvaluationType.Playback)
		{
			if (!(Transform == null))
			{
				SoundManager.Instance?.PlaySfx3D(Sfx, Transform);
			}
			else
			{
				SoundManager.Instance?.PlaySfx(Sfx);
			}
		}
	}
}

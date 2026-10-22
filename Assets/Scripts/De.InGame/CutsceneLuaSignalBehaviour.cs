using UnityEngine.Playables;

public class CutsceneLuaSignalBehaviour : PlayableBehaviour
{
	public int SignalId;

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (info.evaluationType == FrameData.EvaluationType.Playback && GameCutsceneManager.Instance != null)
		{
			GameCutsceneManager.Instance.EmitLuaSignal(SignalId);
		}
	}
}

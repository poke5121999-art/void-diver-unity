using UnityEngine.Timeline;

[TrackColor(0.94f, 0.5f, 0.7f)]
[TrackBindingType(typeof(CutsceneUnit))]
[TrackClipType(typeof(CutsceneUnitAnimationClip))]
[TrackClipType(typeof(CutsceneUnitAnimationSpeedClip))]
[TrackClipType(typeof(CutsceneUnitSetDirectionClip))]
public class CutsceneUnitAnimationTrack : TrackAsset
{
}

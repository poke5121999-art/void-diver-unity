using UnityEngine.Timeline;

[TrackColor(0.9f, 0.5f, 0.2f)]
[TrackBindingType(typeof(CutsceneCharacterMaterialController))]
[TrackClipType(typeof(CutsceneCharacterEffectClip))]
[TrackClipType(typeof(CutsceneCharacterDeathClip))]
[TrackClipType(typeof(CutsceneCharacterGhostClip))]
public class CutsceneCharacterMaterialTrack : TrackAsset
{
}

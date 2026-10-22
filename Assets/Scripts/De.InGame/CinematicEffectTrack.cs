using UnityEngine.Timeline;

[TrackColor(0.3f, 0.6f, 0.9f)]
[TrackClipType(typeof(CinematicIrisClip))]
[TrackClipType(typeof(CinematicMotionSickClip))]
[TrackClipType(typeof(CinematicGlitchPulseClip))]
[TrackClipType(typeof(CinematicPsycheDiveClip))]
[TrackClipType(typeof(CinematicDissolveClip))]
public class CinematicEffectTrack : TrackAsset
{
}

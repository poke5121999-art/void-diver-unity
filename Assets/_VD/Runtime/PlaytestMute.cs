using System;
using UnityEngine;

namespace VD.Runtime
{
    // Playtests start with no sound (owner's request, 2026-10-26). The mute is AudioListener.volume, Unity's master output
    // gain: the game's option sliders, MasterAudio and the saved settings stay untouched, so turning sound back on gives
    // exactly what the options say. Nothing in the game writes AudioListener.volume, so the mute holds across scenes.
    // First match wins:
    //   - env VD_SOUND=1 / VD_SOUND=0 forces sound on / off;
    //   - command-line flag -vd-sound (a built player) turns it on;
    //   - batchmode (the PlayMode test runner) keeps sound: A8 measures what plays;
    //   - PlayerPrefs "VD.SoundOn" = 1, set from the editor menu VOID DIVER/Sound On;
    //   - otherwise muted.
    public static class PlaytestMute
    {
        public const string PrefKey = "VD.SoundOn";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Apply()
        {
            bool on = SoundOn(out string why);
            AudioListener.volume = on ? 1f : 0f;
            Debug.Log($"VD sound: {(on ? "on" : "off")} ({why}), AudioListener.volume={AudioListener.volume}");
        }

        public static bool SoundOn(out string why)
        {
            string env = Environment.GetEnvironmentVariable("VD_SOUND");
            if (env == "1" || env == "0") { why = $"env VD_SOUND={env}"; return env == "1"; }
            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-vd-sound") >= 0) { why = "-vd-sound"; return true; }
            if (Application.isBatchMode) { why = "batchmode"; return true; }
            if (PlayerPrefs.GetInt(PrefKey, 0) == 1) { why = $"PlayerPrefs {PrefKey}=1"; return true; }
            why = "playtest default";
            return false;
        }

        public static void Set(bool on)
        {
            PlayerPrefs.SetInt(PrefKey, on ? 1 : 0);
            PlayerPrefs.Save();
            AudioListener.volume = on ? 1f : 0f;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A8b: the tutorial's audio, recorded in the real flow. Runs A4_TutorialFlow.NewGameToTutorial unchanged (fresh profile,
    // New Game, lounge prologue with the letter, tutorial stage, boss cutscene, forced escape) while A8's AudioProbe records
    // the mix in segments that change with the scene, the playing playlist, the letter popup and the cutscene
    // (captures/review/audio/tutorial-NN-*.wav) and logs every clip that starts.
    // Expected sounds, from the shipped data: Campaign/1100.lua (PlaySfx Train, Paper, NPC_Knock, DoorKey, DoorOpen,
    // FootStep_Run, ElaraWalk, Radio, BoothEscape_Start_Safe; PlayBgm Scenario_Prologue_Memory, Scenario_Prologue_Meet,
    // Lounge, BossDarkYoung_Battle), TSector 10009/3903/10001/10004 Bgm City_Ambience, the cutscene timeline
    // TL_InGameCutScene_Chapter00_TutorialCampaign_1 (Cutscene Sound Track: SFXPlayableAsset DarkYoung_Appear, SFX_CUTSCENE),
    // and the player's footsteps and sword swings (A8_AudioFlow). Batchmode shims as in A8 (DECODING traps 44, 45).
    public class A8b_TutorialAudio
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Prepare()
        {
            if (!Flow.RunFor("A8b_TutorialAudio")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A8B";
            Flow.FreshProfile();
            Flow.UseLocalPlatform();
            if (Application.isBatchMode) AudioProbe.SwapEndOfFrame();
        }

        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        [TearDown]
        public void Restore()
        {
            s_watch = false;
            AudioProbe.RenderScreen = false;
            typeof(A4_TutorialFlow).GetMethod("StopTutorialEvents", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)?.Invoke(null, null);
            AudioProbe.Shutdown();
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static bool s_watch;
        static readonly List<string> s_lines = new List<string>();

        static readonly string[] Expected =
        {
            "BGM:Scenario_Prologue_Memory", "BGM:Scenario_Prologue_Meet", "BGM:Lounge", "BGM:City_Ambience", "BGM:BossDarkYoung_Battle",
            "SFX:Train", "SFX:Paper", "SFX:NPC_Knock", "SFX:DoorKey", "SFX:DoorOpen", "SFX:FootStep_Run", "SFX:ElaraWalk",
            "SFX:Radio", "SFX:DarkYoung_Appear", "SFX:BoothEscape_Start_Safe", "SFX:Skill_100001_NormalAttack_1",
        };

        [UnityTest, Timeout(2400000)]
        public IEnumerator TutorialAudio()
        {
            LogAssert.ignoreFailingMessages = true;
            s_lines.Clear();
            AudioProbe.Heard.Clear();
            AudioProbe.Install();
            AudioProbe.Mode = "listener";
            s_watch = true;
            Object.FindObjectsByType<AudioProbe>(FindObjectsSortMode.None).First().StartCoroutine(Watch());

            // A4's whole flow, unchanged. Its asserts end it early; keep recording what was reached and report that too.
            Exception failure = null;
            var flow = new A4_TutorialFlow().NewGameToTutorial();
            while (true)
            {
                object current;
                try
                {
                    if (!flow.MoveNext()) break;
                    current = flow.Current;
                }
                catch (Exception e) { if (e.GetType().Name != "SuccessException") failure = e; break; }
                yield return current;
            }
            Flow.Tag = "VD_A8B";
            yield return new WaitForSecondsRealtime(3f);
            s_watch = false;
            yield return null;
            if (s_open) s_lines.Add(AudioProbe.End());

            var missing = Expected.Where(e => !AudioProbe.Heard.Contains(e)).ToList();
            if (!AudioProbe.Heard.Any(h => h.StartsWith("SFX:Walk_") || h.StartsWith("SFX:Run_"))) missing.Add("SFX:Walk_*/Run_* footsteps");
            var loud = s_lines.Where(l => System.Text.RegularExpressions.Regex.IsMatch(l, @"clipped=[1-9]")).ToList();
            Flow.Log($"end a4={(failure == null ? "completed" : failure.Message)}\n{string.Join("\n", s_lines)}\nheard=[{string.Join(",", AudioProbe.Heard.OrderBy(h => h))}]\nmissing=[{string.Join(",", missing)}] clippedSegments={loud.Count}");
            Assert.IsEmpty(missing, "not heard: " + string.Join(", ", missing));
            Assert.IsEmpty(loud, "clipping: " + string.Join("\n", loud));
            if (failure != null) Assert.Fail($"A4 flow stopped: {failure.Message}");
        }

        static bool s_open;

        // One recording segment per (scene, playlist, letter open, cutscene playing), at most 60 s each.
        static IEnumerator Watch()
        {
            string key = null;
            int n = 0;
            float start = 0f, stageSince = -1f;
            Vector3? spawn = null;
            while (s_watch)
            {
                string k = Key();
                if (k != key || Time.realtimeSinceStartup - start > 60f)
                {
                    if (s_open) s_lines.Add(AudioProbe.End());
                    key = k;
                    n++;
                    start = Time.realtimeSinceStartup;
                    AudioProbe.Begin($"tutorial-{n:D2}-{k}");
                    s_open = true;
                }
                // Rendered frames for 40 s once the diver first walks in the stage (run 1 rendered the first 45 s of Playing,
                // which the intro dialog took up: no footstep), so its Spine events fire. Not longer: slow frames shorten
                // A4's game-time F holds.
                bool stage = Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing";
                var me = stage ? Flow.MyCharacter() : null;
                if (me != null && spawn == null) spawn = me.transform.position;
                if (me != null && stageSince < 0f && Flow.Flat(me.transform.position - spawn.Value).magnitude > 0.5f) stageSince = Time.realtimeSinceStartup;
                // Only while walking: run 2 also rendered during the F hold at Elara's terminal 11002 and TalkToEll_1 was missed.
                AudioProbe.RenderScreen = stage && stageSince >= 0f && Time.realtimeSinceStartup - stageSince < 40f
                    && Flow.Get(me, "State")?.ToString() == "Move";
                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        static string Key()
        {
            string scene = Flow.ActiveScene;
            string where = scene.Contains("Lounge") ? "lounge" : scene.Contains("Stage") ? "stage" : scene.StartsWith("InitTestScene") ? "boot" : "title";
            var pc = AudioProbe.ActivePlaylistController();
            var src = Flow.Get(pc, "ActiveAudioSource") as AudioSource;
            string bgm = src && src.isPlaying ? Flow.Get(pc, "PlaylistName") as string ?? "-" : "nobgm";
            bool letter = Flow.Find("NoteSystemPopupView") != null;
            bool cutscene = Object.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None)
                .Any(d => d && d.state == PlayState.Playing && d.name.Contains("CutScene"));
            return $"{where}-{bgm}{(letter ? "-letter" : "")}{(cutscene ? "-cutscene" : "")}";
        }
    }
}

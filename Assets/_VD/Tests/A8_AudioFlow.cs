using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A8: what the player hears. Images cannot review audio, so the run records the final mix to WAV
    // (captures/review/audio/<moment>.wav) and logs every clip that starts playing with its MasterAudio group, bus and
    // volumes (VD_A8 clip ...). Each moment logs one summary line (VD_A8 audio <moment> rms= peak= clipped= clips=[...]).
    //
    // Moments: title BGM, the title's Option button click (and the Options sound sliders: BGM 100 -> 50 -> 100 must scale the
    // playlist), lounge BGM after Continue, a dialog line advance (Elara, lounge quest 80200), the dive BGM of campaign 1101,
    // footsteps and basic-attack swings in the dive stage.
    //
    // Capture: an OnAudioFilterRead tap on the AudioListener; it hears mixer-routed sources too. The editor opens an output
    // device even under batchmode/Xvfb (48 kHz stereo, DSP 1024). AudioRenderer is only a fallback for a run with no DSP:
    // without Time.captureFramerate it renders ~0 samples a frame and stops the DSP clock. "VD_A8 capture" logs which ran.
    // Batchmode needs two shims to sound like a player (docs/DECODING.md traps 44, 45): MasterAudio.EndOfFrameDelay set to
    // null at boot (no end of frame, Addressable clips never finish loading), and rendered frames around the stage
    // moments (Spine skeletons stay at updateWhenInvisible = Nothing and fire no footstep events).
    // Expected groups per moment come from the shipped SoundManager.prefab (tools/review/ma_groups.py), tables and code;
    // verdicts in docs/review/a8/verdicts.tsv.
    //
    // Saves: the post-tutorial fixture (see A3_NpcFlow). The sound settings file stays as it is (Flow.FreshProfile keeps it);
    // the test logs the values it loaded and puts the BGM slider back where it found it.
    public class A8_AudioFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const int CampaignId = 1101;
        static string OutDir => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "captures", "review", "audio");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A8_AudioFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A8";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
            // Before any scene: MasterAudio plays a warming sound when it starts, which JIT-compiles the Addressable loader's
            // MoveNext; Mono then reads the static readonly EndOfFrameDelay as a constant, so a later swap no longer reaches
            // it (run 6: the first PopUpOpen stayed Loading for 1.5 s while the playlist, compiled after the swap, played).
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
            AudioProbe.Shutdown();
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static readonly List<string> s_summary = new List<string>();
        static readonly List<string> s_problems = new List<string>();

        static void Expect(bool ok, string what)
        {
            if (ok) return;
            s_problems.Add(what);
            Flow.Log($"PROBLEM {what}");
        }

        [UnityTest, Timeout(2400000)]
        public IEnumerator HearTheGame()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A8";
            s_summary.Clear();
            s_problems.Clear();
            Directory.CreateDirectory(OutDir);
            Flow.Log($"start platform={Flow.Platform()}");
            AudioProbe.Install();
            yield return AudioProbe.EndOfFrameShim();  // before the title starts its playlist
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);

            // ---- title ----
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            yield return AudioProbe.EnsureCapture();
            Flow.Log($"settings {SoundSettings()}");
            Flow.Log($"playlist internals {PlaylistInternals()}");
            Flow.Log($"mixer {MixerState()}");
            yield return Record("title-bgm", 8f);
            Expect(PlaylistState().Contains("clip=Title ") && PlaylistState().Contains("playing=True"), $"title BGM: {PlaylistState()}");
            Flow.Log($"title playlist internals after {PlaylistInternals()}");
            Flow.Log($"sound manager paused={Flow.Get(Flow.Find("SoundManager"), "_isPaused")} ready={Flow.Get(Flow.Find("SoundManager"), "_isReady")} bgm={Flow.Get(Flow.Find("SoundManager"), "_currentBgmName")}");
            yield return AddressableProbe("a1e80b0ac9cf5b840a67f84797b47f4b");
            Flow.Log($"title playlist internals after own load {PlaylistInternals()}");

            // UI click: the title's Option button (OnClickWithSfx "ButtonClick"), which also opens the Options popup.
            // First play of a group at the title, straight through the game's SoundManager (runs 1-4: the Option button's
            // ButtonClick and the popup's PopUpOpen were never heard; the second ButtonClick was).
            yield return FirstPlay("PopUpOpen");
            AudioProbe.Begin("ui-click");
            yield return new WaitForSecondsRealtime(0.5f);
            Flow.Button(title, "Option_Button").onClick.Invoke();
            // The first ButtonClick loads its Addressable clip first (run 1: heard 3 s after the press).
            yield return new WaitForSecondsRealtime(5f);
            End();

            yield return OptionsSliders();

            // ---- lounge ----
            yield return Flow.ContinueToLounge(loadTitle: false);
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Flow.RouteInputToGame();
            yield return new WaitForSecondsRealtime(2f);
            Flow.Log($"lounge playlist internals {PlaylistInternals()}");
            Flow.Log($"lounge bgm {PlaylistState()} profileBgm={Flow.Get(Flow.Get(Flow.Find("PlayerManager"), "Profile"), "BgmId")}");
            yield return Record("lounge-bgm", 10f);
            Expect(PlaylistState().Contains("clip=Lounge ") && PlaylistState().Contains("playing=True"), $"lounge BGM: {PlaylistState()}");

            // Dialog: Elara's lounge quest 80200 line (DialogPopup; advancing a line plays its sfx).
            AudioProbe.Begin("dialog");
            yield return Flow.Talk(700000, "Elara 80200 start");
            End();
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Flow.Talk(id, $"{who} 80200");
            yield return Flow.Talk(700000, "Elara 80200 finish");

            // Another lounge song: the turntable (NPC 700151, function LoungeBgm) opens BgmPopup; slot 1002 is
            // Lounge_Vinyl_Circuit (TLoungeBgm, no unlock condition). SetBgmId -> GameSoundManager's BgmId watch -> playlist.
            yield return LoungeSong(1002, "Lounge_Vinyl_Circuit");
            yield return LoungeSong(1001, "Lounge");

            // LevelUp: the owner (NPC 700005, function LevelUp) opens InGameScene.OnLevelUpAsync's dialog; Yes sends ReqLevelUp
            // and opens LevelUpPopup (OpenSfx "LevelUp"). Afford it first with the cheat menu's exp and coin grants.
            yield return LevelUp();

            // ---- dive ----
            yield return StartCampaign();
            Assert.AreEqual(CampaignId.ToString(), CampaignNow(), "campaign 1101 did not start");
            yield return Briefing();
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");
            yield return new WaitForSecondsRealtime(3f);
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "stage arrival dialog");
            Flow.Log($"stage playlist internals {PlaylistInternals()}");
            Flow.Log($"stage bgm {PlaylistState()} sector={Flow.Get(Flow.Find("GameMapManager"), "CurrentSectorId")}");
            yield return Record("dive-bgm", 10f);
            Expect(PlaylistState().Contains("clip=Country_") && PlaylistState().Contains("playing=True"), $"dive BGM: {PlaylistState()}");

            // Footsteps: walk a few seconds (open floor first: try the 8 directions, take one NavMesh lets through).
            AudioProbe.RenderScreen = true;
            yield return new WaitForSecondsRealtime(0.5f);
            AudioProbe.Begin("dive-footsteps");
            Flow.Log($"footsteps before {FootstepState()}");
            yield return Walk(6f);
            Flow.Log($"footsteps after {FootstepState()}");
            End();

            // Basic attack: left clicks in front of the diver (TSkill ActionSfx of the basic attack).
            AudioProbe.Begin("dive-attack");
            yield return Swing(6);
            End();
            AudioProbe.RenderScreen = false;

            foreach (var (moment, group) in new[] { ("ui-click", "group=ButtonClick "), ("ui-click", "group=PopupOpen "), ("dialog", "group=DialogNext "),
                         ("dive-footsteps", "group=Walk_"), ("dive-attack", "group=Skill_100001_NormalAttack_1 ") })
            {
                var line = s_summary.FirstOrDefault(l => l.StartsWith($"audio {moment} "));
                Expect(line != null && line.Contains(group), $"{moment}: no {group.Trim()} heard");
            }
            foreach (var line in s_summary)
                Expect(!line.Contains("rms=-180") && !System.Text.RegularExpressions.Regex.IsMatch(line, @"clipped=[1-9]"), $"level: {line.Substring(0, Math.Min(160, line.Length))}");
            Flow.Log($"end summary:\n{string.Join("\n", s_summary)}\nproblems=[{string.Join(" | ", s_problems)}]");
            Assert.IsTrue(AudioProbe.Mode != "none", "no capture ran: neither the listener tap nor AudioRenderer got samples");
            Assert.IsEmpty(s_problems, string.Join("\n", s_problems));
        }

        static IEnumerator Record(string moment, float seconds)
        {
            AudioProbe.Begin(moment);
            yield return new WaitForSecondsRealtime(seconds);
            End();
        }

        static void End() => s_summary.Add(AudioProbe.End());

        // ---- options ----

        static IEnumerator OptionsSliders()
        {
            yield return Flow.WaitUntil(() => Flow.Find("OptionPageView") != null, 10f, "options popup");
            var page = Flow.Find("OptionPageView");
            if (page == null) { Flow.Log($"options: popup did not open, top={Flow.TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(1f);
            var tabs = Flow.Get(page, "Tabs") as GameObject[];
            var soundTab = tabs != null && tabs.Length > 1 ? tabs[1].GetComponent<Button>() : null;
            Flow.Log($"options tabs=[{(tabs == null ? "" : string.Join(",", tabs.Select(t => t.name)))}]");
            if (soundTab != null) soundTab.onClick.Invoke();
            yield return new WaitForSecondsRealtime(1f);
            var panel = Flow.Find("OptionSoundPanelView");
            if (panel == null) { Flow.Log("options: sound panel not active"); yield break; }
            var master = Flow.Get(panel, "TotalVolume_Volume_Slider") as Slider;
            var bgm = Flow.Get(panel, "Bgm_Volume_Slider") as Slider;
            var sfx = Flow.Get(panel, "Sfx_Volume_Slider") as Slider;
            Flow.Log($"options sound sliders master={master?.value} bgm={bgm?.value} sfx={sfx?.value} {SoundSettings()} {PlaylistState()}");
            if (bgm == null) yield break;

            // A player drags BGM to half: the playlist source must follow (SoundSettingService -> PlaylistMasterVolume).
            float was = bgm.value;
            // SoundSettingService saves through StorageService.SaveToLocalAsync, which writes PlayerPrefs (Base64), not SavesDir.
            string prefsBefore = PlayerPrefs.GetString("SoundSettingContext", "");
            float srcBefore = PlaylistSourceVolume();
            yield return Record("title-bgm-slider-100", 4f);
            bgm.value = 50f;
            yield return new WaitForSecondsRealtime(0.5f);
            float srcHalf = PlaylistSourceVolume();
            Flow.Log($"options bgm 50: {SoundSettings()} {PlaylistState()} source {srcBefore:F3} -> {srcHalf:F3}");
            yield return Record("title-bgm-slider-50", 4f);
            bgm.value = was;
            yield return new WaitForSecondsRealtime(0.5f);
            Flow.Log($"options bgm back to {was}: {SoundSettings()} {PlaylistState()} source={PlaylistSourceVolume():F3}");

            // Esc closes the popup (OptionPageModel.OnEscape); OnDisable of the sound panel flushes the settings file.
            // The first Esc leaves the Sound tab (OptionPagePresenter.Close with a panel selected), the second closes the popup.
            Flow.RouteInputToGame();
            for (int i = 0; i < 3 && Flow.Find("OptionPageView") != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find("OptionPageView") == null, 2f, $"options closed after Esc {i + 1}");
            }
            if (Flow.Find("OptionPageView") != null)
            {
                Flow.Log("options: Esc did not close it; PopupManager.CloseTopPopup");
                var pm = Flow.Find("PopupManager");
                pm?.GetType().GetMethod("CloseTopPopup")?.Invoke(pm, null);
                yield return new WaitForSecondsRealtime(1.5f);
            }
            yield return new WaitForSecondsRealtime(1f);  // Flush -> SaveAsync serializes on the thread pool
            string prefsAfter = PlayerPrefs.GetString("SoundSettingContext", "");
            Flow.Log($"options closed top={Flow.TopPopup()} saved settings (PlayerPrefs SoundSettingContext): before={prefsBefore.Length} chars after={prefsAfter.Length} chars rewritten={prefsBefore != prefsAfter} {SoundSettings()}");
        }

        static object SoundService()
        {
            // SoundSettingService is a plain C# singleton in the root container; OptionSoundPanelPresenter and StartScene hold it.
            var start = Flow.Find("StartScene");
            return Flow.Get(start, "_soundSettings") ?? Flow.Get(Flow.Find("OptionSoundPanelPresenter"), "_soundSettings");
        }

        static IEnumerator FirstPlay(string sfx)
        {
            var sm = Flow.Find("SoundManager");
            AudioProbe.Begin("first-play-" + sfx);
            for (int i = 0; i < 2; i++)
            {
                object result = null;
                try { result = sm?.GetType().GetMethod("PlaySfx", new[] { typeof(string) })?.Invoke(sm, new object[] { sfx }); }
                catch (Exception e) { Flow.Log($"first play {sfx}: threw {e.InnerException}"); }
                var variation = Flow.Get(result, "ActingVariation") as Component;
                Flow.Log($"first play {sfx} #{i}: played={Flow.Get(result, "SoundPlayed")} scheduled={Flow.Get(result, "SoundScheduled")} variation={(variation ? variation.name : "-")}");
                for (int k = 0; k < 6; k++)
                {
                    yield return new WaitForSecondsRealtime(0.25f);
                    var src = variation ? variation.GetComponent<AudioSource>() : null;
                    Flow.Log($"first play {sfx} #{i} +{(k + 1) * 0.25f:F2}s: go={(variation ? variation.gameObject.activeSelf : false)} load={Flow.Get(variation, "LoadStatus")} " +
                             $"clip={(src && src.clip ? src.clip.name : "-")} playing={(src && src.isPlaying)} time={(src ? src.time : -1):F2} vol={(src ? src.volume : -1):F2} " +
                             $"param={Flow.Get(Flow.Get(variation, "_playSndParam"), "IsPlaying")} co={(Flow.Get(variation, "_loadAddressableCoroutine") != null)} stop={Flow.Get(variation, "_isStopRequested")}");
                }
            }
            End();
        }

        static string SoundSettings()
        {
            var s = SoundService();
            if (s == null) return "soundSettings=-";
            return $"master={Flow.Get(s, "MasterVolumeOn")}/{Flow.Get(s, "MasterVolumeLevel")} bgm={Flow.Get(s, "BgmVolumeOn")}/{Flow.Get(s, "BgmVolumeLevel")} " +
                   $"sfx={Flow.Get(s, "SfxVolumeOn")}/{Flow.Get(s, "SfxVolumeLevel")} appliedBgm={Flow.Get(s, "AppliedBgmVolume")} appliedSfx={Flow.Get(s, "AppliedSfxVolume")}";
        }

        static float PlaylistSourceVolume()
        {
            var pc = AudioProbe.ActivePlaylistController();
            return (Flow.Get(pc, "ActiveAudioSource") as AudioSource)?.volume ?? -1f;
        }

        static string PlaylistState()
        {
            var ma = AudioProbe.MaType("MasterAudio");
            var pc = AudioProbe.ActivePlaylistController();
            var src = Flow.Get(pc, "ActiveAudioSource") as AudioSource;
            return $"playlistMaster={AudioProbe.Static(ma, "PlaylistMasterVolume")} playlist={Flow.Get(pc, "PlaylistName")} playlistVol={Flow.Get(pc, "_playlistVolume")} " +
                   $"clip={(src && src.clip ? src.clip.name : "-")} srcVol={(src ? src.volume : -1f):F3} playing={(src && src.isPlaying)} buses={AudioProbe.Buses()}";
        }

        static string PlaylistInternals()
        {
            var t = AudioProbe.MaType("PlaylistController");
            if (t == null) return "-";
            var parts = new List<string>();
            foreach (var pc in Object.FindObjectsByType(t, FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<Component>())
            {
                string Src(string f) => Flow.Get(pc, f) is AudioSource a ? $"{a.name}:{(a.clip ? a.clip.name : "-")}/{(a.isPlaying ? "play" : "stop")}/en={a.isActiveAndEnabled}/vol={a.volume:F2}" : "-";
                var cur = Flow.Get(pc, "_currentPlaylist");
                var remaining = Flow.Get(pc, "_clipsRemaining") as ICollection;
                parts.Add($"{pc.name} active={pc.gameObject.activeInHierarchy} ready={Flow.Get(pc, "ControllerIsReady")} state={Flow.Get(pc, "PlaylistState")} playlist={Flow.Get(cur, "playlistName")} " +
                          $"remaining={remaining?.Count} played={Flow.Get(pc, "_songsPlayedFromPlaylist")} song={Flow.Get(Flow.Get(pc, "_currentSong"), "songName")} newSong={Flow.Get(Flow.Get(pc, "_newSongSetting"), "songName")} " +
                          $"requested={Flow.Get(pc, "_nextSongRequested")} addrCo={(Flow.Get(pc, "_addressableCoroutine") != null)} active={Src("_activeAudio")} trans={Src("_transitioningAudio")} audioClip={Src("_audioClip")} " +
                          $"schedStart={Flow.Get(pc, "_currentSchedSongDspStartTime")} dsp={AudioSettings.dspTime:F2}");
            }
            return string.Join(" || ", parts);
        }

        // MasterAudio's loaded-clip map and a load of our own of the same key, to see where an Addressable song stops.
        static IEnumerator AddressableProbe(string key)
        {
            var opt = AudioProbe.MaType("AudioAddressableOptimizer");
            var map = opt?.GetField("AddressableTasksByAddressableId", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(null) as IDictionary;
            Flow.Log($"addressables loaded by MasterAudio: [{(map == null ? "?" : string.Join(",", map.Keys.Cast<object>()))}]");
            var addr = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEngine.AddressableAssets.Addressables")).FirstOrDefault(t => t != null);
            var m = addr?.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == "LoadAssetAsync" && x.IsGenericMethodDefinition && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(object));
            if (m == null) { Flow.Log("addressables: LoadAssetAsync not found"); yield break; }
            object handle = m.MakeGenericMethod(typeof(AudioClip)).Invoke(null, new object[] { key });
            float t0 = Time.realtimeSinceStartup;
            while (!(Flow.Get(handle, "IsDone") is bool d && d) && Time.realtimeSinceStartup - t0 < 10f) yield return null;
            var clip = Flow.Get(handle, "Result") as AudioClip;
            Flow.Log($"addressables own load {key}: done={Flow.Get(handle, "IsDone")} status={Flow.Get(handle, "Status")} after {Time.realtimeSinceStartup - t0:F1}s " +
                     $"error={(Flow.Get(handle, "OperationException") as Exception)?.Message} clip={(clip ? $"{clip.name} {clip.loadType} {clip.loadState} len={clip.length:F1}" : "-")}");
        }

        static string MixerState()
        {
            var ma = AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "Instance") as Component;
            var mixers = Resources.FindObjectsOfTypeAll<UnityEngine.Audio.AudioMixer>();
            var groups = Resources.FindObjectsOfTypeAll<UnityEngine.Audio.AudioMixerGroup>();
            string groupInfo = string.Join(",", groups.Select(g => $"{g.name}@{(g.audioMixer ? g.audioMixer.name : "-")}"));
            string mixerVol = "";
            foreach (var m in mixers)
                foreach (var p in new[] { "MasterVolume", "Master", "BGM", "SFX" })
                    if (m.GetFloat(p, out float v)) mixerVol += $"{m.name}.{p}={v:F1}dB,";
            return $"masterAudio={(ma ? ma.name : "-")} mixers=[{string.Join(",", mixers.Select(m => m.name))}] groups=[{groupInfo}] params=[{mixerVol}] " +
                   $"mixerMuted={AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "MixerMuted")} playlistsMuted={AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "PlaylistsMuted")} " +
                   $"masterVolume={AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "MasterVolumeLevel")} listenerVol={AudioListener.volume} listenerPause={AudioListener.pause}";
        }

        static object MyProfile() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "MyPlayer"), "RxProfile");

        static void Cheat(string type, string member, long value)
        {
            var t = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(type)).FirstOrDefault(x => x != null);
            var req = Activator.CreateInstance(t);
            t.GetProperty(member).SetValue(req, value);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { req });
        }

        static IEnumerator LevelUp()
        {
            Cheat("ReqCheatSetExp", "Exp", 1500);   // TLevel 2 NeedExp 1500, ConsumeGoods Coin 3
            Cheat("ReqCheatSetCoin", "Coin", 100);
            yield return new WaitForSecondsRealtime(1.5f);
            Flow.Log($"level up: level={Flow.Get(MyProfile(), "Level")} exp={Flow.Get(MyProfile(), "Exp")}");
            var owner = Flow.FindNpc(700005);
            if (owner == null) { SpawnNpc(700005); yield return Flow.WaitUntil(() => (owner = Flow.FindNpc(700005)) != null, 10f, "owner spawned"); }
            if (owner == null) { Expect(false, "level up: owner 700005 not in the lounge"); yield break; }
            // Run 11: the owner stands by NPC 700011 (risk level) and F focused that one. Stand on each side of the owner until
            // the interaction focus is on him, then press F.
            bool focused = false;
            foreach (var side in new[] { Vector3.back, Vector3.left, Vector3.right, Vector3.forward })
            {
                var o = owner;
                yield return Flow.WalkTo(() => o ? o.transform.position + side * 1.0f : (Vector3?)null, 0.4f, 10f, "owner side");
                Flow.Aim(owner.transform.position + Vector3.up * 0.8f);
                yield return Flow.WaitUntil(() => Flow.Focused(o), 2f, "focus owner");
                if (focused = Flow.Focused(owner)) break;
                Flow.Log($"level up: focus {Flow.Focus()} from side {side}");
            }
            if (!focused) { Expect(false, $"level up: could not focus the owner (focus {Flow.Focus()})"); yield break; }
            yield return Flow.Keys(0.7f, Key.F);
            yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null || Flow.Find("DialogPopupView") != null, 4f, "owner reaction");
            if (Flow.Find("SelectionPopupView") != null) { yield return new WaitForSecondsRealtime(0.8f); yield return Flow.SelectFunction(owner, "LevelUp"); }
            AudioProbe.Begin("level-up");
            float t0 = Time.realtimeSinceStartup;
            bool yes = false;
            while (Flow.Find("LevelUpPopupView") == null && Time.realtimeSinceStartup - t0 < 40f)
            {
                var dialog = Flow.Find("DialogPopupView");
                var sel = (Flow.Get(dialog, "Selections") as GameObject[])?.FirstOrDefault(g => g && g.activeInHierarchy);
                var b = sel ? sel.GetComponentInChildren<Button>() : null;
                if (b != null && b.IsInteractable() && !yes)
                {
                    Flow.Log($"level up: dialog '{Flow.DialogEntry()}' -> Yes");
                    b.onClick.Invoke();
                    yes = true;
                }
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")) && !Flow.DialogDelaying()) Flow.Button(dialog, "Background").onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.5f);
            }
            yield return new WaitForSecondsRealtime(3f);
            End();
            Flow.Log($"level up: popup={(Flow.Find("LevelUpPopupView") != null)} level={Flow.Get(MyProfile(), "Level")}");
            Expect(Flow.Find("LevelUpPopupView") != null, $"level up: no LevelUpPopup (yes={yes}, top={Flow.TopPopup()})");
            Expect(AudioProbe.Heard.Contains("SFX:LevelUp"), "level up: no LevelUp sound");
            for (int i = 0; i < 4 && Flow.TopPopup() != null; i++)
            {
                var bg = Flow.Button(Flow.Find("LevelUpPopupView"), "Background");
                if (Flow.Ready(bg)) bg.onClick.Invoke(); else yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(1.5f);
                if (Flow.Find("DialogPopupView") != null) yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 30f, "after level up");
            }
        }

        static IEnumerator LoungeSong(int bgmId, string playlist)
        {
            var table = Flow.FindNpc(700151);
            if (table == null)
            {
                // The turntable unlocks later (TNpc 700151 NpcLevel condition); place it as A3 places locked NPCs.
                SpawnNpc(700151);
                yield return Flow.WaitUntil(() => (table = Flow.FindNpc(700151)) != null, 10f, "turntable spawned");
            }
            if (table == null) { Expect(false, $"lounge song {bgmId}: turntable 700151 not in the lounge"); yield break; }
            for (int attempt = 0; attempt < 3 && Flow.Find("BgmPopupPresenter") == null; attempt++)
            {
                yield return Flow.InteractNpc(table, "turntable");
                yield return Flow.WaitUntil(() => Flow.Find("BgmPopupPresenter") != null || Flow.Find("SelectionPopupView") != null, 4f, "turntable reaction");
                if (Flow.Find("SelectionPopupView") != null) { yield return new WaitForSecondsRealtime(0.8f); yield return Flow.SelectFunction(table, "LoungeBgm"); }
                yield return Flow.WaitUntil(() => Flow.Find("BgmPopupPresenter") != null, 5f, "BgmPopup");
            }
            if (Flow.Find("BgmPopupPresenter") == null) { Expect(false, $"lounge song {bgmId}: BgmPopup did not open, top={Flow.TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(1f);
            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "BgmSlotPresenter" && Flow.Get(Flow.Get(m, "_model"), "BgmId") is int id && id == bgmId);
            var button = slot ? slot.GetComponent<Button>() : null;
            if (button == null) { Expect(false, $"lounge song {bgmId}: no slot"); yield break; }
            AudioProbe.Begin($"lounge-song-{bgmId}");
            button.onClick.Invoke();
            yield return new WaitForSecondsRealtime(8f);
            End();
            Flow.Log($"lounge song {bgmId}: {PlaylistState()}");
            Expect(PlaylistState().Contains($"playlist={playlist} ") && PlaylistState().Contains($"clip={playlist} ") && PlaylistState().Contains("playing=True"), $"lounge song {bgmId}: {PlaylistState()}");
            yield return Flow.Keys(0.2f, Key.Escape);
            yield return Flow.WaitUntil(() => Flow.Find("BgmPopupPresenter") == null, 4f, "BgmPopup closed");
            yield return new WaitForSecondsRealtime(1f);
        }

        // Copied from A3_NpcFlow.Spawn: ForceSpawnNpcs when the lounge has spawn data for it, else GameObjectManager.SpawnNpc
        // on a free NavMesh point 2.5 m from the player (the cheat menu's Spawn NPC).
        static void SpawnNpc(int npcId)
        {
            var om = Flow.Find("GameObjectManager");
            var me = Flow.MyCharacter();
            var datas = Flow.Get(om, "NpcSpawnDatas") as IEnumerable;
            if (datas != null && datas.Cast<object>().Any(d => Flow.Get(d, "NpcId") is int id && id == npcId))
            {
                Flow.Log($"spawn {npcId}: ForceSpawnNpcs");
                om.GetType().GetMethod("ForceSpawnNpcs")?.Invoke(om, null);
                return;
            }
            var method = om?.GetType().GetMethods().FirstOrDefault(m => m.Name == "SpawnNpc" && m.GetParameters().Length == 3);
            if (method == null || me == null) { Flow.Log($"spawn {npcId}: no GameObjectManager.SpawnNpc"); return; }
            var p = me.transform.position;
            var spot = new Vector3(p.x - 2f, p.y, p.z + 2f);
            for (int i = 0; i < 16; i++)
            {
                float ang = i * Mathf.PI / 8f;
                var c = p + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * 2.5f;
                if (!UnityEngine.AI.NavMesh.SamplePosition(c, out var hit, 0.3f, UnityEngine.AI.NavMesh.AllAreas)) continue;
                if (Flow.Npcs().Any(n => Flow.Flat(n.transform.position - hit.position).magnitude < 2f)) continue;
                spot = hit.position;
                break;
            }
            var r = method.Invoke(om, new object[] { npcId, spot, true });
            Flow.Log($"spawn {npcId}: SpawnNpc at {spot} -> {r}");
        }

        // ---- lounge to stage (copied from A5_DiveFlow) ----

        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        static bool PopupOpen(string presenter) =>
            Flow.Get(Flow.Find("PopupManager"), "_popups") is IEnumerable list && list.Cast<object>().Any(p => p?.GetType().Name == presenter);

        static string State() => Flow.Get(Flow.MyCharacter(), "State")?.ToString() ?? "-";

        static IEnumerator StartCampaign()
        {
            for (int attempt = 0; attempt < 5 && Flow.Find("CampaignPopupPresenter") == null; attempt++)
            {
                var elara = Flow.FindNpc(700000);
                yield return Flow.InteractNpc(elara, "Elara campaign");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("CampaignPopupPresenter") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara before her menu");
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(elara, "Campaign");
                }
                yield return Flow.WaitUntil(() => Flow.Find("CampaignPopupPresenter") != null, 8f, "CampaignPopupPresenter");
            }
            if (Flow.Find("CampaignPopupPresenter") == null) { Flow.Log($"campaign menu did not open, top={Flow.TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(2f);
            var slots = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "CampaignSlotPresenter").ToList();
            var slot = slots.FirstOrDefault(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId") is int id && id == CampaignId);
            if (slot == null) { Flow.Log($"no slot for campaign {CampaignId}"); yield break; }
            var slotButton = Flow.Get(slot, "Button") as Button;
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, "campaign slot 1101");
            slotButton.onClick.Invoke();
            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == CampaignId, 6f, "campaign 1101 detail");
            if (detail == null) yield break;
            yield return new WaitForSecondsRealtime(1.5f);
            var start = Flow.Get(Flow.Get(detail, "_view"), "Info_StartCampaignButton") as Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == CampaignId.ToString(), 10f, "campaign started");
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        static IEnumerator Briefing()
        {
            yield return new WaitForSecondsRealtime(3.5f);
            for (int attempt = 0; attempt < 3; attempt++)
            {
                if (BoothCheck() == "Success") break;
                var elara = Flow.FindNpc(700000);
                yield return Flow.InteractNpc(elara, "Elara briefing");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, "Elara briefing reaction");
                if (Flow.Find("DialogPopupView") != null)
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 180f, "Elara briefing");
                else if (Flow.Find("SelectionPopupView") != null) yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => BoothCheck() == "Success", 5f, "quest InProgress");
            }
            Flow.Log($"briefing done booth={BoothCheck()}");
        }

        static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            var id = Flow.Get(gm, "LocalClientId");
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { id })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        static IEnumerator EnterBooth()
        {
            var booth = Flow.Find("PhoneBooth");
            if (booth == null) { Flow.Log("no PhoneBooth in the lounge"); yield break; }
            for (int attempt = 0; attempt < 3 && State() != "StageReady" && Flow.ActiveScene == "LoungeScene"; attempt++)
            {
                yield return Flow.InteractObject(booth, 0.8f, "dive booth");
                yield return Flow.WaitUntil(() => State() == "StageReady" || Flow.ActiveScene != "LoungeScene", 4f, "StageReady");
            }
            yield return Flow.WaitUntil(() => PopupOpen("GameLoadingPopupPresenter") || Flow.StageState() == "Playing" && Flow.ActiveScene == "StageScene", 30f, "loading card");
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.Log($"stage {Flow.LoadState()}");
        }

        // ---- stage ----

        static IEnumerator Walk(float seconds)
        {
            var me = Flow.MyCharacter();
            if (me == null) yield break;
            Vector3 from = me.transform.position;
            // The direction with the longest free run on the NavMesh.
            Vector3 best = Vector3.forward;
            float bestLen = -1f;
            for (int i = 0; i < 8; i++)
            {
                var d = Quaternion.Euler(0f, 45f * i, 0f) * Vector3.forward;
                float len = UnityEngine.AI.NavMesh.Raycast(from, from + d * 8f, out var hit, UnityEngine.AI.NavMesh.AllAreas) ? hit.distance : 8f;
                if (len > bestLen) { bestLen = len; best = d; }
            }
            float start = Time.realtimeSinceStartup;
            bool back = false;
            while (Time.realtimeSinceStartup - start < seconds)
            {
                Flow.Hold(Flow.KeysToward(back ? -best : best));
                yield return new WaitForSecondsRealtime(0.25f);
                if (Flow.Flat(Flow.MyCharacter().transform.position - from).magnitude > Mathf.Min(bestLen, 6f) - 0.5f) back = !back;
            }
            Flow.Hold();
            Flow.Log($"walk {Flow.Flat(Flow.MyCharacter().transform.position - from).magnitude:F1} m dir={best} free={bestLen:F1} state={State()}");
        }

        // UnitView.OnSpineEvent counts sfx_run/walk events (_currentFootstepCount) and plays ResolveFootstepSound.
        static string FootstepState()
        {
            var view = Flow.MyView();
            if (view == null) return "view=-";
            string Resolve(bool left)
            {
                try { return view.GetType().GetMethod("ResolveFootstepSound", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.Invoke(view, new object[] { left }) as string ?? "null"; }
                catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}:{e.InnerException?.Message}"; }
            }
            var skels = Flow.Get(view, "SkeletonAnimations") as Array;
            string sk = "";
            if (skels != null)
                foreach (var o in skels)
                    if (o is Behaviour b)
                    {
                        var state = Flow.Get(b, "state") ?? Flow.Get(b, "AnimationState");
                        var cur = state?.GetType().GetMethod("GetCurrent")?.Invoke(state, new object[] { 0 });
                        sk += $"{b.name}:en={b.enabled}/go={b.gameObject.activeInHierarchy}/anim={Flow.Get(Flow.Get(cur, "Animation"), "Name")}" +
                              $"/mode={Flow.Get(b, "UpdateMode")}/whenInvisible={Flow.Get(b, "updateWhenInvisible")}/visible={b.GetComponent<Renderer>()?.isVisible},";
                    }
            return $"count={Flow.Get(view, "_currentFootstepCount")} aimBones={(Flow.Get(view, "_aimBones") as ICollection)?.Count} skeletons=[{sk}] " +
                   $"tryRun={Flow.Get(Flow.Get(view, "CharacterModel"), "TryRun")} left='{Resolve(true)}' right='{Resolve(false)}'";
        }

        static IEnumerator Swing(int count)
        {
            var me = Flow.MyCharacter();
            if (me == null) yield break;
            for (int i = 0; i < count; i++)
            {
                var cam = Camera.main;
                Vector3 aim = me.transform.position + me.transform.forward * 2f + Vector3.up * 0.5f;
                Vector2 screen = cam ? (Vector2)cam.WorldToScreenPoint(aim) : new Vector2(Screen.width / 2f, Screen.height / 2f);
                Flow.Aim(aim);
                yield return Flow.MouseClick(screen);
                yield return new WaitForSecondsRealtime(0.7f);
                Flow.Log($"swing {i} state={State()} using={Flow.Get(me, "UsingSkillId")}");
            }
        }
    }

    // The recorder and the clip log. Lives on a DontDestroyOnLoad object; the listener tap moves with the AudioListener.
    public class AudioProbe : MonoBehaviour
    {
        static AudioProbe s_instance;
        static readonly object s_lock = new object();
        static readonly List<float> s_samples = new List<float>();
        static bool s_recording;
        static int s_channels = 2;
        static long s_tapCallbacks;
        static string s_moment;
        static float s_momentStart;
        static readonly List<string> s_events = new List<string>();
        public static string Mode = "none";
        public static readonly HashSet<string> Heard = new HashSet<string>();  // "BGM:<playlist>" / "SFX:<group>" that started
        static int s_renderChannels => AudioSettings.speakerMode == AudioSpeakerMode.Mono ? 1 : 2;
        const int MaxSamples = 48000 * 2 * 120;

        public static void Install()
        {
            if (s_instance) return;
            var go = new GameObject("VD_A8_AudioProbe");
            DontDestroyOnLoad(go);
            s_instance = go.AddComponent<AudioProbe>();
        }

        public static void Shutdown()
        {
            if (Mode == "renderer") AudioRenderer.Stop();
            if (s_instance) Destroy(s_instance.gameObject);
            s_instance = null;
        }

        // ---- capture ----

        public static IEnumerator EnsureCapture()
        {
            var cfg = AudioSettings.GetConfiguration();
            string editorMute = "?";
#if UNITY_EDITOR
            editorMute = UnityEditor.EditorUtility.audioMasterMute.ToString();
            if (UnityEditor.EditorUtility.audioMasterMute) UnityEditor.EditorUtility.audioMasterMute = false;
#endif
            Flow.Log($"capture config rate={cfg.sampleRate} speaker={cfg.speakerMode} dsp={cfg.dspBufferSize} real={cfg.numRealVoices} virtual={cfg.numVirtualVoices} " +
                     $"outputRate={AudioSettings.outputSampleRate} driverCaps={AudioSettings.driverCapabilities} editorMute={editorMute} listeners={Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length}");
            // Run 1: the listener tap got callbacks (the DSP runs on the editor's device) but read silence while ButtonClick
            // played: MasterAudio routes every source to MainMixer (BGM/SFX groups), and mixer-routed audio does not pass
            // through the listener's filter chain. AudioRenderer pulls the mixer's final output, so it is the capture; the tap
            // stays as a second meter (tapRms) to show the difference.
            long before = s_tapCallbacks;
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 2f) yield return null;
            Flow.Log($"capture listener tap: {s_tapCallbacks - before} callbacks in 2 s, channels={s_channels}");
            // Run 2: AudioRenderer without Time.captureFramerate renders ~0 samples a frame and the DSP clock stops with it
            // (dspTime 53.14 -> 53.18 over 8 s), which stalls MasterAudio's scheduled plays. The tap does hear mixer-routed
            // sources (ButtonClick on the SFX group: tap -31.2 dBFS, renderer -31.2 dBFS), so the tap is the capture.
            Mode = s_tapCallbacks > before ? "listener" : "none";
            if (Mode == "none" && AudioRenderer.Start()) Mode = "renderer";
            Flow.Log($"capture mode={Mode}");
        }

        // MasterAudio waits on MasterAudio.EndOfFrameDelay (a WaitForEndOfFrame) while an Addressable clip loads
        // (AudioAddressableOptimizer, PlaylistController songs). Batchmode has no Game view to finish a frame, so a
        // WaitForEndOfFrame never resumes (Review.cs hits the same): a load that is not done on the first check never
        // finishes and the song never starts. Measured here; when it does not fire, the shared YieldInstruction is
        // replaced by null (resume next frame), which is what a player's end of frame amounts to for these loops.
        public static IEnumerator EndOfFrameShim()
        {
            int eof = 0, frames = 0;
            s_instance.StartCoroutine(Count(new WaitForEndOfFrame(), () => eof++));
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 1.5f) { frames++; yield return null; }
            var f = MaType("MasterAudio")?.GetField("EndOfFrameDelay", BindingFlags.Static | BindingFlags.Public);
            Flow.Log($"capture WaitForEndOfFrame resumed {eof} times in {frames} frames; MasterAudio.EndOfFrameDelay={(f?.GetValue(null) == null ? "null (swapped at boot)" : "WaitForEndOfFrame")} ({s_swap})");
        }

        static string s_swap = "not swapped";

        public static void SwapEndOfFrame()
        {
            var f = MaType("MasterAudio")?.GetField("EndOfFrameDelay", BindingFlags.Static | BindingFlags.Public);
            if (f == null) { s_swap = "field not found"; return; }
            try { f.SetValue(null, null); s_swap = f.GetValue(null) == null ? "swapped at SubsystemRegistration" : "SetValue ignored"; }
            catch (Exception e) { s_swap = $"threw {e.GetType().Name}"; }
            Debug.LogWarning($"VD_A8 EndOfFrameDelay {s_swap}");
        }

        static IEnumerator Count(object wait, Action tick)
        {
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 1.5f) { yield return wait; tick(); }
        }

        // A player's screen: batchmode renders nothing between Review shots, so every Renderer stays invisible. Spine's
        // SkeletonRenderer.Awake sets updateMode = updateWhenInvisible, which the character prefabs set to Nothing, and only
        // OnBecameVisible restores FullUpdate: the diver's skeleton never applies its animation, never fires the
        // sfx_walk/sfx_run events, and UnitView.OnSpineEvent plays no footstep (run 3: _currentFootstepCount 0 after 7.3 m).
        // Render the base cameras into a small target every frame, as the display would.
        static RenderTexture s_screen;
        public static bool RenderScreen;  // only around the stage moments: rendering every frame slows batchmode frames, and
                                          // game-time holds (the dive booth's 1.5 s F) then fall short (run 4)
        void LateUpdate()
        {
            if (!RenderScreen) return;
            if (!s_screen) s_screen = new RenderTexture(320, 180, 24);
            foreach (var cam in Camera.allCameras)
            {
                if (!cam.enabled || cam.targetTexture != null) continue;
                if (cam.TryGetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>(out var d) && d.renderType == UnityEngine.Rendering.Universal.CameraRenderType.Overlay) continue;
                cam.targetTexture = s_screen;
                cam.Render();
                cam.targetTexture = null;
            }
        }

        void Update()
        {
            // Keep a tap on the active listener (the SoundManager's; cutscenes may bring their own).
            foreach (var l in FindObjectsByType<AudioListener>(FindObjectsSortMode.None))
                if (l.isActiveAndEnabled && !l.GetComponent<ListenerTap>()) l.gameObject.AddComponent<ListenerTap>();
            if (Mode == "renderer")
            {
                int n = AudioRenderer.GetSampleCountForCaptureFrame();
                int ch = s_renderChannels;
                if (n > 0)
                {
                    using (var buf = new NativeArray<float>(n * ch, Allocator.Temp))
                    {
                        if (AudioRenderer.Render(buf)) Append(buf.ToArray(), ch);
                    }
                }
            }
            PollSources();
        }

        static double s_tapSum;
        static long s_tapCount;

        public static void Tap(float[] data, int channels)
        {
            s_tapCallbacks++;
            s_channels = channels;
            if (Mode == "listener") Append(data, channels);
            else if (s_recording)
                lock (s_lock) { foreach (var x in data) s_tapSum += x * x; s_tapCount += data.Length; }
        }

        static void Append(float[] data, int channels)
        {
            lock (s_lock)
            {
                if (Mode == "listener") s_channels = channels;
                if (s_recording && s_samples.Count < MaxSamples) s_samples.AddRange(data);
            }
        }

        public static void Begin(string moment)
        {
            lock (s_lock) { s_samples.Clear(); s_recording = true; s_tapSum = 0; s_tapCount = 0; }
            s_moment = moment;
            s_momentStart = Time.realtimeSinceStartup;
            s_events.Clear();
            Flow.Log($"audio begin {moment}");
        }

        public static string End()
        {
            float[] data;
            int ch;
            double tapRms;
            lock (s_lock)
            {
                data = s_samples.ToArray(); ch = Mode == "renderer" ? s_renderChannels : s_channels; s_recording = false; s_samples.Clear();
                tapRms = s_tapCount > 0 ? Math.Sqrt(s_tapSum / s_tapCount) : 0;
            }
            int rate = AudioSettings.outputSampleRate;
            string path = Path.Combine("captures", "review", "audio", s_moment + ".wav");
            WriteWav(Path.Combine(Directory.GetParent(Application.dataPath).FullName, path), data, ch, rate);
            double sum = 0; float peak = 0; int clipped = 0;
            foreach (var x in data) { sum += x * x; float a = Mathf.Abs(x); if (a > peak) peak = a; if (a >= 0.999f) clipped++; }
            double rms = data.Length > 0 ? Math.Sqrt(sum / data.Length) : 0;
            // Loudest 50 ms window: a click on a quiet bed shows here, not in the whole-take RMS.
            int win = Math.Max(1, rate / 20 * ch);
            double best = 0;
            for (int i = 0; i + win <= data.Length; i += win)
            {
                double s = 0;
                for (int j = i; j < i + win; j++) s += data[j] * data[j];
                best = Math.Max(best, s / win);
            }
            string line = $"audio {s_moment} mode={Mode} secs={(data.Length / (double)Math.Max(1, ch * rate)):F1} wall={Time.realtimeSinceStartup - s_momentStart:F1} " +
                          $"rms={Db(rms):F1}dBFS peak={Db(peak):F1}dBFS loudest50ms={Db(Math.Sqrt(best)):F1}dBFS clipped={clipped} tapRms={Db(tapRms):F1}dBFS file={path} clips=[{string.Join(" | ", s_events)}]";
            Flow.Log(line);
            return line;
        }

        static double Db(double v) => v > 1e-9 ? 20 * Math.Log10(v) : -180;

        static void WriteWav(string file, float[] data, int channels, int rate)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (var w = new BinaryWriter(File.Create(file)))
            {
                int bytes = data.Length * 4;
                w.Write(new[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' }); w.Write(36 + bytes);
                w.Write(new[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E', (byte)'f', (byte)'m', (byte)'t', (byte)' ' });
                w.Write(16); w.Write((short)3); w.Write((short)channels); w.Write(rate); w.Write(rate * channels * 4);
                w.Write((short)(channels * 4)); w.Write((short)32);
                w.Write(new[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' }); w.Write(bytes);
                foreach (var x in data) w.Write(x);
            }
        }

        // ---- clip log ----

        struct SrcState { public AudioClip Clip; public bool Playing; public float Time; }
        static readonly Dictionary<int, SrcState> s_sources = new Dictionary<int, SrcState>();

        static void PollSources()
        {
            foreach (var s in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            {
                if (s == null) continue;
                int id = s.GetInstanceID();
                bool playing = s.isPlaying;
                s_sources.TryGetValue(id, out var was);
                bool started = playing && s.clip != null && (!was.Playing || was.Clip != s.clip || (!s.loop && s.time + 0.05f < was.Time));
                s_sources[id] = new SrcState { Clip = s.clip, Playing = playing, Time = playing ? s.time : 0f };
                if (!started) continue;
                string d = Describe(s);
                Flow.Log($"clip {d}");
                var m = System.Text.RegularExpressions.Regex.Match(d, @"^(BGM) playlist=(\S+)|^SFX group=(\S+)");
                if (m.Success) Heard.Add(m.Groups[1].Success ? "BGM:" + m.Groups[2].Value : "SFX:" + m.Groups[3].Value);
                if (s_recording) s_events.Add($"{Time.realtimeSinceStartup - s_momentStart:F1}s {d}");
            }
        }

        static string Describe(AudioSource s)
        {
            var groupT = MaType("MasterAudioGroup");
            var pcT = MaType("PlaylistController");
            var listener = FindObjectsByType<AudioListener>(FindObjectsSortMode.None).FirstOrDefault(l => l.isActiveAndEnabled);
            float dist = listener ? Vector3.Distance(listener.transform.position, s.transform.position) : -1f;
            string common = $"clip={s.clip.name} src={s.volume:F3} mixer={(s.outputAudioMixerGroup ? s.outputAudioMixerGroup.name : "-")} blend={s.spatialBlend:F1} dist={dist:F1} mute={s.mute} virtual={s.isVirtual}";
            var pc = pcT == null ? null : s.GetComponentInParent(pcT);
            if (pc != null)
                return $"BGM playlist={Flow.Get(pc, "PlaylistName")} playlistVol={Flow.Get(pc, "_playlistVolume")} master={Static(MaType("MasterAudio"), "PlaylistMasterVolume")} {common}";
            var g = groupT == null ? null : s.GetComponentInParent(groupT);
            if (g != null)
            {
                var bus = Flow.Get(g, "BusForGroup");
                return $"SFX group={g.name} bus={Flow.Get(bus, "busName") ?? "-"} busVol={Flow.Get(bus, "volume") ?? "-"} groupVol={Flow.Get(g, "groupMasterVolume")} var={s.name} {common}";
            }
            return $"OTHER {PathOf(s.transform)} {common}";
        }

        static string PathOf(Transform t) => t.parent == null ? t.name : PathOf(t.parent) + "/" + t.name;

        // ---- MasterAudio by reflection (the test assembly does not reference it) ----

        static readonly Dictionary<string, Type> s_types = new Dictionary<string, Type>();

        public static Type MaType(string name)
        {
            if (s_types.TryGetValue(name, out var t)) return t;
            t = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("DarkTonic.MasterAudio." + name)).FirstOrDefault(x => x != null);
            s_types[name] = t;
            return t;
        }

        public static object Static(Type t, string member)
        {
            if (t == null) return null;
            const BindingFlags f = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            try
            {
                var p = t.GetProperty(member, f);
                if (p != null) return p.GetValue(null);
                return t.GetField(member, f)?.GetValue(null);
            }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name ?? e.GetType().Name}"; }
        }

        public static Component ActivePlaylistController()
        {
            var t = MaType("PlaylistController");
            if (t == null) return null;
            return FindObjectsByType(t, FindObjectsSortMode.None).OfType<Component>()
                .FirstOrDefault(c => (Flow.Get(c, "ActiveAudioSource") as AudioSource)?.isPlaying == true)
                ?? FindObjectsByType(t, FindObjectsSortMode.None).OfType<Component>().FirstOrDefault();
        }

        public static string Buses()
        {
            var ma = Static(MaType("MasterAudio"), "Instance");
            if (!(Flow.Get(ma, "groupBuses") is IEnumerable buses)) return "-";
            var parts = new List<string>();
            foreach (var b in buses) parts.Add($"{Flow.Get(b, "busName")}={Flow.Get(b, "volume")}{(Flow.Get(b, "isMuted") is bool m && m ? "(muted)" : "")}");
            return string.Join(",", parts);
        }
    }

    // OnAudioFilterRead on the listener's object sees the mix the listener hears.
    public class ListenerTap : MonoBehaviour
    {
        void OnAudioFilterRead(float[] data, int channels) => AudioProbe.Tap(data, channels);
    }
}

namespace VD.PlayTests
{
    // Opt-in audio logging for any flow (A5, A6, ...), off unless the environment says so:
    //   VD_AUDIO_LOG=1  log every clip that starts ("<tag> clip BGM playlist=... / SFX group=... bus=..."), with the batchmode
    //                   end-of-frame shim MasterAudio needs to load Addressable clips at all (DECODING trap 44)
    //   VD_AUDIO_REC=1  also record the mix to captures/review/audio/<test>-NN-<scene>-<playlist>.wav, one segment per
    //                   scene/playlist/popup change, at most 60 s each ("<tag> audio ..." lines with RMS, peak, clipping)
    // With neither set nothing is installed and the flows run exactly as before. Spine footsteps still need rendered frames
    // (trap 45); the opt-in does not render, so it does not change frame timing either.
    public static class AudioOptIn
    {
        static bool Log => Environment.GetEnvironmentVariable("VD_AUDIO_LOG") == "1" || Rec;
        static bool Rec => Environment.GetEnvironmentVariable("VD_AUDIO_REC") == "1";
        static bool s_running;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Boot()
        {
            if (!Log || Flow.RunFor("A8_AudioFlow", "A8b_TutorialAudio")) return;
            if (Application.isBatchMode) AudioProbe.SwapEndOfFrame();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Start()
        {
            if (!Log || s_running || Flow.RunFor("A8_AudioFlow", "A8b_TutorialAudio")) return;
            s_running = true;
            AudioProbe.Install();
            AudioProbe.Mode = "listener";
            Debug.LogWarning($"VD_AUDIO opt-in log={Log} rec={Rec}");
            Object.FindObjectsByType<AudioProbe>(FindObjectsSortMode.None).First().StartCoroutine(Segments());
        }

        static IEnumerator ProbeSfx(string group)
        {
            var ma = AudioProbe.MaType("MasterAudio");
            var inst = AudioProbe.Static(ma, "Instance");
            var map = inst?.GetType().GetField("AudioSourcesBySoundType", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(inst) as IDictionary;
            var info = map != null && map.Contains(group) ? map[group] : null;
            string Vars()
            {
                var parts = new List<string>();
                if (Flow.Get(info, "Sources") is IEnumerable srcs)
                    foreach (var ai in srcs)
                    {
                        var v = Flow.Get(ai, "Variation") as Component;
                        var a = v ? v.GetComponent<AudioSource>() : null;
                        parts.Add($"{(v ? v.name : "-")}:go={(v ? v.gameObject.activeSelf : false)} load={Flow.Get(v, "LoadStatus")} playing={(a && a.isPlaying)} param={Flow.Get(Flow.Get(v, "_playSndParam"), "IsPlaying")} stop={Flow.Get(v, "_isStopRequested")} co={(Flow.Get(v, "_loadAddressableCoroutine") != null)} clip={(a && a.clip ? a.clip.name : "-")}");
                    }
                return string.Join(" ", parts);
            }
            Flow.Log($"probe {group} before: ready={AudioProbe.Static(ma, "SoundsReady")} shutting={AudioProbe.Static(ma, "AppIsShuttingDown")} warming={AudioProbe.Static(ma, "IsWarming")} " +
                     $"inGroup={(info != null)} last={Flow.Get(info, "LastTimePlayed")} now={Time.realtimeSinceStartup:F1} voices={Flow.Get(Flow.Get(info, "Group"), "ActiveVoices")} vars=[{Vars()}]");
            var sm = Flow.Find("SoundManager");
            object r = null;
            try { r = sm?.GetType().GetMethod("PlaySfx", new[] { typeof(string) })?.Invoke(sm, new object[] { group }); }
            catch (Exception e) { Flow.Log($"probe {group}: threw {e.InnerException}"); }
            Flow.Log($"probe {group} after: enable={Flow.Get(sm, "_enable")} result={(r == null ? "null" : $"played={Flow.Get(r, "SoundPlayed")} scheduled={Flow.Get(r, "SoundScheduled")} var={(Flow.Get(r, "ActingVariation") as Component)?.name}")} vars=[{Vars()}]");
            // Is it Addressables or the coroutine? Count frames a coroutine of ours gets on the same variation, and load the
            // same key ourselves.
            var acting = Flow.Get(r, "ActingVariation") as MonoBehaviour;
            int ticks = 0;
            if (acting && acting.isActiveAndEnabled) acting.StartCoroutine(Tick(() => ticks++));
            var key = Flow.Get(Flow.Get(acting, "audioClipAddressable"), "RuntimeKey");
            object handle = null;
            var addr = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEngine.AddressableAssets.Addressables")).FirstOrDefault(t => t != null);
            var m = addr?.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == "LoadAssetAsync" && x.IsGenericMethodDefinition && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(object));
            try { if (key != null) handle = m?.MakeGenericMethod(typeof(AudioClip)).Invoke(null, new[] { key }); }
            catch (Exception e) { Flow.Log($"probe {group}: own load threw {e.InnerException?.Message}"); }
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 3f) yield return null;
            Flow.Log($"probe {group} +3s: ticks={ticks} ownLoad key={key} done={Flow.Get(handle, "IsDone")} status={Flow.Get(handle, "Status")} pct={Flow.Get(handle, "PercentComplete")} " +
                     $"eof={AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "EndOfFrameDelay") ?? "null"} vars=[{Vars()}]");
        }

        static IEnumerator Tick(Action tick)
        {
            for (int i = 0; i < 1000; i++) { tick(); yield return null; }
        }

        static IEnumerator Segments()
        {
            string test = Environment.GetCommandLineArgs().SkipWhile(a => a != "-testFilter").Skip(1).FirstOrDefault() ?? "run";
            string key = null;
            int n = 0;
            float start = 0f;
            bool open = false;
            string lastState = null, lastWhere = null;
            float probeAt = -1f;
            while (true)
            {
                // SoundManager pauses everything on OnApplicationFocus(false) and resumes on focus; log every change, with the
                // buses' mute state (A5 audio run 1: no SFX at all after the death, through Dive Results).
                var sm = Flow.Find("SoundManager");
                string state = $"focused={Application.isFocused} paused={Flow.Get(sm, "_isPaused")} listenerPause={AudioListener.pause} buses={AudioProbe.Buses()} mixerMuted={AudioProbe.Static(AudioProbe.MaType("MasterAudio"), "MixerMuted")}";
                if (state != lastState) { Flow.Log($"audio state {state}"); lastState = state; }
                string scene = Flow.ActiveScene;
                string where = scene.Contains("Lounge") ? "lounge" : scene.Contains("Stage") ? "stage" : scene.StartsWith("InitTestScene") ? "boot" : "title";
                // Back in the lounge from a stage: play ButtonClick through the game's SoundManager and say what MasterAudio did
                // (A5 audio runs 1-2: after a death no SFX started again, Dive Results' EscapeResult included).
                if (where == "lounge" && lastWhere == "stage") probeAt = Time.realtimeSinceStartup + 15f;
                lastWhere = where;
                if (probeAt > 0f && Time.realtimeSinceStartup > probeAt) { probeAt = -1f; yield return ProbeSfx("ButtonClick"); }
                var pc = AudioProbe.ActivePlaylistController();
                var src = Flow.Get(pc, "ActiveAudioSource") as AudioSource;
                string bgm = src && src.isPlaying ? Flow.Get(pc, "PlaylistName") as string ?? "-" : "nobgm";
                string popup = Flow.TopPopup();
                string k = $"{where}-{bgm}{(popup != null && popup.Contains("Result") ? "-" + popup.Replace("PopupView", "") : "")}";
                if (Rec && (k != key || Time.realtimeSinceStartup - start > 60f))
                {
                    if (open) AudioProbe.End();
                    key = k;
                    n++;
                    start = Time.realtimeSinceStartup;
                    AudioProbe.Begin($"{test}-{n:D2}-{k}");
                    open = true;
                }
                yield return new WaitForSecondsRealtime(0.25f);
            }
        }
    }
}

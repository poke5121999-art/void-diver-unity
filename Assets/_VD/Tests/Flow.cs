using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // Shared player-style driving for the flows past the title (A2, A4): find the game's views by type name (the test
    // assembly does not reference the game's assemblies), press their buttons, feed devices, wait on real state.
    public static class Flow
    {
        public static string Tag = "VD_FLOW";

        public static void Log(string message) =>
            Debug.LogWarning($"{Tag} t={Time.realtimeSinceStartup:F1} scene={ActiveScene} {message}");

        public static string ActiveScene => SceneManager.GetActiveScene().name;

        // True when this editor run was started for one of the given test classes (-testFilter on the command line).
        // RuntimeInitializeOnLoadMethod hooks run for every play-mode run, A1's included, so they check this first.
        public static bool RunFor(params string[] tests) =>
            Environment.GetCommandLineArgs().Any(a => tests.Any(t => a.Contains(t)));

        // ---- the game's objects, by type name ----

        public static MonoBehaviour Find(string typeName) =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.GetType().Name == typeName && m.isActiveAndEnabled);

        public static object Get(object target, string member)
        {
            if (target == null) return null;
            for (var t = target.GetType(); t != null; t = t.BaseType)
            {
                const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
                var p = t.GetProperty(member, f);
                if (p != null && p.GetIndexParameters().Length == 0) return p.GetValue(target);
                var fi = t.GetField(member, f);
                if (fi != null) return fi.GetValue(target);
            }
            return null;
        }

        // ReactiveProperty<T>.Value, or the member itself when it is not reactive.
        public static object Value(object target, string member)
        {
            var v = Get(target, member);
            return v != null && v.GetType().GetProperty("Value") != null ? Get(v, "Value") : v;
        }

        public static Button Button(Component view, string member) => Get(view, member) as Button;

        public static bool Ready(Button b) => b != null && b.isActiveAndEnabled && b.IsInteractable();

        public static string StageState()
        {
            var gm = Find("GameManager");
            return gm == null ? "-" : Value(gm, "RxStageState")?.ToString() ?? "-";
        }

        public static Component MyCharacter()
        {
            var gm = Find("GameManager");
            return gm == null ? null : Get(gm, "MyCharacterModel") as Component;
        }

        // ---- waiting and pressing ----

        public static bool LastWaitOk;

        public static IEnumerator WaitUntil(Func<bool> condition, float timeout, string what)
        {
            float start = Time.realtimeSinceStartup, nextLog = start + 10f;
            Log($"wait {what}");
            while (true)
            {
                bool ok;
                try { ok = condition(); }
                catch (Exception e) { ok = false; Log($"wait {what} threw {e.GetType().Name}: {e.Message}"); }
                if (ok) { LastWaitOk = true; Log($"wait {what} ok after {Time.realtimeSinceStartup - start:F1}s"); yield break; }
                if (Time.realtimeSinceStartup - start > timeout) { LastWaitOk = false; Log($"wait {what} TIMEOUT {timeout}s stage={StageState()}"); yield break; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Log($"waiting {what} stage={StageState()}"); }
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        public static IEnumerator WaitView(string typeName, float timeout)
        {
            yield return WaitUntil(() => Find(typeName) != null, timeout, typeName);
        }

        // Click a view's button the way a pointer click ends up: Button.onClick, once the button is shown and interactable.
        public static IEnumerator Click(Component view, string member, float timeout = 20f)
        {
            string what = $"{view?.GetType().Name}.{member}";
            yield return WaitUntil(() => Ready(Button(view, member)), timeout, what + " interactable");
            if (!LastWaitOk) yield break;
            yield return new WaitForSecondsRealtime(0.3f);  // let the open tween settle like a player's reaction time
            Log($"click {what}");
            Button(view, member).onClick.Invoke();
        }

        // TMP_InputField.text, the result of typing into the field (the field reads IMGUI key events, which the batchmode
        // editor does not produce from Input System devices).
        public static void Type(object inputField, string text)
        {
            inputField.GetType().GetProperty("text").SetValue(inputField, text);
            Log($"typed '{text}' into {(inputField as Component)?.name}");
        }

        // ---- devices ----

        static InputSettings.EditorInputBehaviorInPlayMode s_editorBehaviour;
        static InputSettings.BackgroundBehavior s_background;
        static bool s_inputRouted;

        // In the editor, keyboard input goes to the game only while the Game view has focus, and batchmode has no Game
        // view. Route all device input to the game for the test (InputTestFixture does the same) and restore it after.
        public static void RouteInputToGame()
        {
            if (s_inputRouted) return;
            var s = InputSystem.settings;
            s_editorBehaviour = s.editorInputBehaviorInPlayMode;
            s_background = s.backgroundBehavior;
            s.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            s.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            s_inputRouted = true;
            // ResetAndDisableNonBackgroundDevices already disabled the devices while batchmode had no focus, and changing
            // the setting does not turn them back on; a disabled device drops queued state events (A4 run 10: W held,
            // wKey.isPressed=False, the game's Player map enabled and waiting).
            string reenabled = "";
            foreach (var d in InputSystem.devices)
                if (!d.enabled) { InputSystem.EnableDevice(d); reenabled += d.name + ","; }
            Log($"input routed to game (was {s_editorBehaviour}/{s_background}) keyboard={(Keyboard.current != null)} mouse={(Mouse.current != null)} reenabled={reenabled}");
        }

        public static void RestoreInput()
        {
            if (!s_inputRouted) return;
            InputSystem.settings.editorInputBehaviorInPlayMode = s_editorBehaviour;
            InputSystem.settings.backgroundBehavior = s_background;
            s_inputRouted = false;
        }

        public static Keyboard Keyboard => Keyboard.current ?? InputSystem.AddDevice<Keyboard>();
        public static Mouse Mouse => Mouse.current ?? InputSystem.AddDevice<Mouse>();

        public static void Hold(params Key[] keys)
        {
            var kb = Keyboard;
            if (!kb.enabled) InputSystem.EnableDevice(kb);
            InputSystem.QueueStateEvent(kb, new KeyboardState(keys));
            InputSystem.Update();  // apply now, like InputTestFixture; the player loop may run with the device paused
        }

        public static IEnumerator Keys(float seconds, params Key[] keys)
        {
            Hold(keys);
            yield return new WaitForSecondsRealtime(seconds);
            Hold();
            yield return null;
        }

        public static IEnumerator MouseClick(Vector2 position, MouseButton button = MouseButton.Left)
        {
            var mouse = Mouse;
            if (!mouse.enabled) InputSystem.EnableDevice(mouse);
            InputSystem.QueueStateEvent(mouse, new MouseState { position = position }.WithButton(button));
            InputSystem.Update();
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(mouse, new MouseState { position = position });
            InputSystem.Update();
            yield return null;
        }

        // ---- platform and profile ----

        const string EnvironmentAsset = "Assets/AssetBundles/dependencies_assets_scriptable/EnvironmentSettings.asset";
        static object s_oldPlatform;

        public static string Platform()
        {
#if UNITY_EDITOR
            var so = UnityEditor.AssetDatabase.LoadMainAssetAtPath(EnvironmentAsset);
            return so == null ? "?" : Get(so, "Platform")?.ToString();
#else
            return "?";
#endif
        }

        // The shipped EnvironmentSettings say Steam. With no Steam client running, SteamAPI_Init fails, the Steam sockets
        // transport cannot host and Steam saves fail, so New Game cannot create the single-player room. The game's own
        // Local platform makes the same CreateRoom/TryJoinGame/LoadGame calls over UnityTransport and saves to SavesDir.
        // Switched in memory only (the asset is never marked dirty or saved) and switched back when play mode ends.
        public static void UseLocalPlatform()
        {
#if UNITY_EDITOR
            var so = UnityEditor.AssetDatabase.LoadMainAssetAtPath(EnvironmentAsset);
            var f = so == null ? null : so.GetType().GetField("Platform");
            if (f == null) { Debug.LogError($"{Tag} EnvironmentSettings not found at {EnvironmentAsset}"); return; }
            s_oldPlatform = f.GetValue(so);
            f.SetValue(so, Enum.ToObject(f.FieldType, 1));
            UnityEditor.EditorApplication.playModeStateChanged += s =>
            {
                if (s == UnityEditor.PlayModeStateChange.ExitingPlayMode) RestorePlatform();
            };
            Debug.LogWarning($"{Tag} platform {s_oldPlatform} -> {f.GetValue(so)} (in memory)");
#endif
        }

        public static void RestorePlatform()
        {
#if UNITY_EDITOR
            if (s_oldPlatform == null) return;
            var so = UnityEditor.AssetDatabase.LoadMainAssetAtPath(EnvironmentAsset);
            so?.GetType().GetField("Platform").SetValue(so, s_oldPlatform);
            s_oldPlatform = null;
#endif
        }

        // A fresh profile: the Local platform keeps account and player saves in <project>/SavesDir. Saves left by an earlier
        // run move aside (to captures/review/saves-prev-<time>); graphics and sound settings stay, they are not the profile.
        public static void FreshProfile()
        {
            string dir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
            if (!Directory.Exists(dir)) return;
            var old = Directory.GetFiles(dir, "*.sav")
                .Where(p => !Path.GetFileName(p).StartsWith("GraphicSetting") && !Path.GetFileName(p).StartsWith("SoundSetting")).ToList();
            if (old.Count == 0) return;
            string dest = Path.Combine("captures", "review", "saves-prev-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            Directory.CreateDirectory(dest);
            foreach (var p in old) File.Move(p, Path.Combine(dest, Path.GetFileName(p)));
            Debug.LogWarning($"{Tag} fresh profile: moved {old.Count} saves to {dest}");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareFlowRun()
        {
            if (!RunFor("A4_TutorialFlow", "A2_LoungeFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            FreshProfile();
            UseLocalPlatform();
        }

        // Same reason as A1: errors logged while the game boots, before the test body runs, would fail the test.
        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        // ---- diagnostics ----

        // What GameManager waits on before LoadCompleted: players joined, characters spawned, cutscenes preloaded.
        public static string LoadState()
        {
            var gm = Find("GameManager");
            if (gm == null) return "gm=-";
            var ctx = Get(gm, "_gameContext");
            var players = Get(ctx, "Players") as System.Collections.ICollection;
            var chars = Get(gm, "_characterControllers") as System.Collections.ICollection;
            var room = Get(gm, "_roomContext");
            return $"stage={StageState()} players={players?.Count.ToString() ?? "?"} chars={chars?.Count.ToString() ?? "?"} " +
                   $"cutscenePreloaded={Value(ctx, "RxIsCutscenePreloaded")} scene={Get(ctx, "CurrentScene")} campaign={Value(ctx, "RxCampaignId")} " +
                   $"tutorial={Get(ctx, "IsTutorial")} room={Value(room, "RxRoomState")} me={(MyCharacter() ? MyCharacter().transform.position.ToString() : "-")}";
        }

        public static void DumpStage()
        {
            var me = MyCharacter();
            Log($"me={(me ? me.transform.position.ToString() : "-")} stage={StageState()}");
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null) continue;
                string n = m.GetType().Name;
                if (n != "CollisionTrigger" && n != "InteractiveTrigger" && n != "MonsterController") continue;
                var ids = m.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => (f.FieldType == typeof(int) || f.FieldType == typeof(long)) && f.Name.IndexOf("id", StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(f => $"{f.Name}={f.GetValue(m)}");
                Log($"obj {n} {m.name} pos={m.transform.position} active={m.isActiveAndEnabled} {string.Join(" ", ids)}");
            }
        }
    }
}

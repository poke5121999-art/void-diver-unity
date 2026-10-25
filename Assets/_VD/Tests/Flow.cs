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

        // Run `body` with the game's Screen at the shots' 1280x720, then go back to batchmode's 640x480. For screens that lay
        // out or scroll by screen size (Dive Results). Only PlayModeWindow's custom size moves batchmode's Screen, and every
        // frame at 1280x720 runs ~10x slower, so keep the window short (docs/DECODING.md trap 48).
        public static IEnumerator AtShotScreen(IEnumerator body)
        {
            UnityEditor.PlayModeWindow.SetCustomRenderingResolution(Review.Width, Review.Height, "VD shots");
            for (int i = 0; i < 30 && Screen.width != Review.Width; i++) yield return null;
            Log($"screen {Screen.width}x{Screen.height} for the shot");
            try { yield return body; }
            finally
            {
                UnityEditor.PlayModeWindow.SetCustomRenderingResolution(640, 480, "VD batchmode");
                Log("screen back to 640x480");
            }
        }

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
            if (!RunFor("A4_TutorialFlow", "A2_LoungeFlow", "A4c_StageProbe", "A4d_MinimapProbe")) return;
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
        // ==== player-style driving shared by the flows past the lounge (A3, A4, A5) ====
        // Copied from A3_NpcFlow/A4_TutorialFlow (which keep their own copies), so new flows do not grow a third copy.

        // ---- profile from a fixture, title Continue into the lounge ----

        // Moves the profile in SavesDir aside (FreshProfile) and copies the fixture's saves in, so the title's first button
        // is Continue. Call from a SubsystemRegistration hook, before the game boots.
        public static void SeedSaves(string fixture)
        {
            FreshProfile();
            string dir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
            Directory.CreateDirectory(dir);
            foreach (var f in Directory.GetFiles(fixture, "*.sav"))
                File.Copy(f, Path.Combine(dir, Path.GetFileName(f)), true);
            Debug.LogWarning($"{Tag} saves from {fixture}: {string.Join(",", Directory.GetFiles(fixture, "*.sav").Select(Path.GetFileName))}");
        }

        // Title Continue, the Local platform's room popup, the lounge load and its arrival dialog. loadTitle false when the
        // game is already back on the title (Return to Title), as a player presses Continue there.
        public static IEnumerator ContinueToLounge(bool loadTitle = true)
        {
            if (loadTitle) SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return WaitUntil(() => (title = Find("StartSceneView")) != null
                && (Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Ready(Button(title, "Play_Button")), 120f, "title menu");
            if (!LastWaitOk) yield break;
            Log($"title play='{Get(Get(title, "Play_Button_Text"), "text")}' slot='{Get(Get(title, "SelectSlot_Player_Name"), "text")}'");
            yield return Click(title, "Play_Button");
            float start = Time.realtimeSinceStartup;
            while (ActiveScene != "LoungeScene" && Time.realtimeSinceStartup - start < 180f)
            {
                foreach (var (view, button) in new[] { ("LocalRoomCreationPopupView", "BtnConfirm_Button"), ("RoomDifficultyPopupView", "Confirm_Button"), ("TextPopupView", "Confirm_Button") })
                    if (Find(view) != null && Ready(Button(Find(view), button)))
                    {
                        Log($"continue: {view}");
                        yield return Click(Find(view), button);
                    }
                if (Find("InputNamePopupView") != null) { Log("the title started a new game: the fixture saves did not load"); yield break; }
                yield return new WaitForSecondsRealtime(0.5f);
            }
            if (ActiveScene != "LoungeScene") yield break;
            yield return WaitUntil(() => StageState() == "Playing", 240f, "lounge Playing");
            yield return new WaitForSecondsRealtime(2f);
            if (Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Find("DialogPopupView") == null, 120f, "lounge arrival dialog");
            Log($"lounge {LoadState()}");
        }

        // Walk up to an NPC and press F; play the dialog it starts, or leave its selection popup with Esc.
        public static IEnumerator Talk(int npcId, string what)
        {
            var npc = FindNpc(npcId);
            if (npc == null) { Log($"talk {what}: npc {npcId} not in the lounge"); yield break; }
            yield return InteractNpc(npc, what);
            yield return WaitUntil(() => Find("DialogPopupView") != null || Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Find("DialogPopupView") == null, 120f, what);
            else if (Find("SelectionPopupView") != null)
            {
                Log($"talk {what}: selection popup, no quest line; Esc");
                yield return Keys(0.2f, Key.Escape);
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        // ---- popups and dialogs ----

        // The popup on top (any *PopupView active in the scene), by type name.
        public static string TopPopup()
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.isActiveAndEnabled && m.GetType().Name.EndsWith("PopupView") && m.GetType().Name != "RadioToastView") return m.GetType().Name;
            return null;
        }

        // PopupManager's open popups and the screen blur.
        public static string PopupState()
        {
            var list = Get(Find("PopupManager"), "_popups") as IEnumerable;
            var sb = new System.Text.StringBuilder("popups=[");
            if (list != null) foreach (var p in list) sb.Append($"{p?.GetType().Name},");
            sb.Append($"] blur={Shader.GetGlobalFloat("_ScreenBlurProgress"):F2}");
            return sb.ToString();
        }

        // DialogPopupPresenter.DelayDialogAsync is running (a Lua pause between lines). A click then cancels the delay and
        // the Lua await never finishes (docs/PLAN.md, the dialog pause trap), so a player-like click waits it out.
        public static bool DialogDelaying()
        {
            var p = Find("DialogPopupPresenter");
            return p != null && Get(p, "_isDelayActive") is bool b && b;
        }

        public static string DialogEntry() => Get(Get(Find("DialogPopupPresenter"), "_currentEntry"), "Message") as string;

        // Click through DialogPopup lines and NoteSystem letters until done() holds (the first click on a line shows the
        // rest of it, the next moves on). onLine runs before each click on a dialog line, for a shot at a given line.
        public static IEnumerator AdvanceDialogs(Func<bool> done, float timeout, string what, Func<string, IEnumerator> onLine = null)
        {
            float start = Time.realtimeSinceStartup;
            int clicks = 0;
            string lastLine = null;
            while (!done() && Time.realtimeSinceStartup - start < timeout)
            {
                var note = Find("NoteSystemPopupView");
                var dialog = Find("DialogPopupView");
                if (note != null && Ready(Button(note, "BaseBgDimmer")))
                {
                    Button(note, "BaseBgDimmer").onClick.Invoke();
                    clicks++;
                }
                else if (dialog != null && Ready(Button(dialog, "Background")) && !DialogDelaying())
                {
                    string line = DialogEntry();
                    if (line != lastLine) { Log($"dialog {what}: '{line}'"); lastLine = line; }
                    if (onLine != null) yield return onLine(line);
                    if (Find("DialogPopupView") != null) Button(dialog, "Background").onClick.Invoke();
                    clicks++;
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Log($"advance {what} done={done()} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s");
        }

        // ---- walking ----

        public static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

        // The WASD keys toward a world direction: the stage camera is isometric, W moves along world (-x,+z) and D along
        // (+x,+z) (A4 run 11), so a direction splits into a W/S part and an A/D part.
        public static Key[] KeysToward(Vector3 d, bool run = false)
        {
            float a = (d.z - d.x) / 2f, b = (d.x + d.z) / 2f, m = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
            var keys = new System.Collections.Generic.List<Key>();
            if (Mathf.Abs(a) > 0.4f * m) keys.Add(a > 0 ? Key.W : Key.S);
            if (Mathf.Abs(b) > 0.4f * m) keys.Add(b > 0 ? Key.D : Key.A);
            if (run) keys.Add(Key.LeftShift);
            return keys.ToArray();
        }

        // Walk with WASD toward a (possibly moving) world point along the NavMesh path; side-step out of corners the
        // diagonals cannot follow; stop for dialogs. Returns when within arrive, stuck, or out of time.
        public static IEnumerator WalkTo(Func<Vector3?> target, float arrive, float timeout, string what, bool run = false)
        {
            float start = Time.realtimeSinceStartup;
            Vector3 last = Vector3.zero;
            int stuck = 0, unstick = 0;
            bool pathLogged = false;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = MyCharacter();
                var t = target();
                if (me == null || t == null) break;
                Vector3 goal = Flat(t.Value - me.transform.position);
                if (goal.magnitude <= arrive) break;
                Vector3 d = goal;
                var path = new UnityEngine.AI.NavMeshPath();
                // A target inside an obstacle (an NPC behind a counter, a terminal) is off the NavMesh: path to the nearest
                // NavMesh point instead, as a player walks up to it.
                Vector3 dest = t.Value;
                if (UnityEngine.AI.NavMesh.SamplePosition(dest, out var near, 2.5f, UnityEngine.AI.NavMesh.AllAreas)) dest = near.position;
                if (UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, dest, UnityEngine.AI.NavMesh.AllAreas, path) && path.corners.Length > 1)
                {
                    int c = 1;
                    while (c < path.corners.Length - 1 && Vector3.Distance(Flat(path.corners[c]), Flat(me.transform.position)) < 0.4f) c++;
                    d = Flat(path.corners[c] - me.transform.position);
                    if (c == path.corners.Length - 1 && d.magnitude < 0.3f) break;  // as close as the NavMesh gets
                }
                else if (!pathLogged) { pathLogged = true; Log($"walk {what} no NavMesh path from {me.transform.position} status={path.status}"); }
                if (Find("DialogPopupView") != null) { Hold(); yield break; }
                var keys = KeysToward(d, run);
                Hold(keys);
                yield return new WaitForSecondsRealtime(0.15f);
                stuck = (me.transform.position - last).sqrMagnitude < 0.0004f ? stuck + 1 : 0;
                last = me.transform.position;
                if (stuck > 14)
                {
                    if (++unstick > 2) { Log($"walk {what} stuck at {last}"); break; }
                    Log($"walk {what} stuck at {last}, side step {unstick}");
                    bool vertical = keys.Contains(Key.W) || keys.Contains(Key.S);
                    Hold(vertical ? (unstick == 1 ? Key.A : Key.D) : (unstick == 1 ? Key.W : Key.S));
                    yield return new WaitForSecondsRealtime(0.4f);
                    stuck = 0;
                }
            }
            Hold();
            yield return null;
            var end = MyCharacter();
            Log($"walk {what} me={(end ? end.transform.position.ToString() : "-")} target={target()} after {Time.realtimeSinceStartup - start:F1}s");
        }

        // Walk to a world point and open closed doors (Entrance) on the way with F, as A4 does in the tutorial stage.
        public static IEnumerator Go(Vector3 goal, float arrive, float timeout, string what, bool run = false, Func<bool> stop = null)
        {
            float start = Time.realtimeSinceStartup;
            int doors = 0;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = MyCharacter();
                if (me == null || ActiveScene != "StageScene") break;
                if (Flat(goal - me.transform.position).magnitude <= arrive) break;
                if (stop != null && stop()) { Log($"go {what}: stopped"); break; }
                if (Find("DialogPopupView") != null)
                {
                    yield return AdvanceDialogs(() => Find("DialogPopupView") == null, 120f, $"dialog on the way to {what}");
                    continue;
                }
                yield return WalkTo(() => (stop != null && stop()) ? (Vector3?)null : goal, arrive, Mathf.Min(20f, timeout - (Time.realtimeSinceStartup - start)), what, run);
                me = MyCharacter();
                if (me == null || Flat(goal - me.transform.position).magnitude <= arrive) break;
                if (Find("DialogPopupView") != null) continue;
                if (stop != null && stop()) continue;
                var door = NearestClosedDoor(me.transform.position, 12f);
                if (door != null && doors < 4)
                {
                    doors++;
                    yield return OpenDoor(door);
                    continue;
                }
                Log($"go {what}: blocked at {me.transform.position}, no closed door within 12 m");
                break;
            }
            var end = MyCharacter();
            Log($"go {what} end me={(end ? end.transform.position.ToString() : "-")} goal={goal} after {Time.realtimeSinceStartup - start:F1}s");
        }

        public static string DoorState(Component door) => Value(door, "_syncLockState")?.ToString() ?? "?";

        public static Component NearestClosedDoor(Vector3 from, float range)
        {
            Component best = null;
            float bestD = range;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "Entrance" && DoorState(m) != "Opened")
                {
                    float d = Flat(m.transform.position - from).magnitude;
                    if (d < bestD) { bestD = d; best = m; }
                }
            return best;
        }

        // A NavMesh point on either side of the door (its forward axis), the reachable one nearest to the character.
        static Vector3? DoorStand(Component door)
        {
            var me = MyCharacter();
            var f = Value(door, "_syncForward") is Vector3 v && v.sqrMagnitude > 0.01f ? Flat(v).normalized : Vector3.forward;
            Vector3? best = null;
            float bestLen = float.MaxValue;
            foreach (float d in new[] { -1.0f, 1.0f, -1.5f, 1.5f })
            {
                var c = door.transform.position + f * d;
                if (!UnityEngine.AI.NavMesh.SamplePosition(c, out var hit, 0.6f, UnityEngine.AI.NavMesh.AllAreas)) continue;
                var path = new UnityEngine.AI.NavMeshPath();
                bool ok = UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path);
                float len = 0f;
                for (int i = 1; i < path.corners.Length; i++) len += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                if (ok && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete && len < bestLen) { bestLen = len; best = hit.position; }
            }
            return best;
        }

        public static IEnumerator OpenDoor(Component door)
        {
            string before = DoorState(door);
            var stand = DoorStand(door);
            if (stand != null) yield return WalkTo(() => stand, 0.3f, 12f, $"door {door.name} stand");
            yield return InteractObject(door, 0.8f, $"door {door.name}");
            yield return WaitUntil(() => !door || DoorState(door) == "Opened", 4f, $"door {door.name} open");
            Log($"door {door.name} {before} -> {(door ? DoorState(door) : "gone")} focus={Focus()}");
        }

        // ---- interaction ----

        public static Component MyView() => Get(Find("GameManager"), "MyCharacterView") as Component;

        public static string Focus()
        {
            var o = Value(MyView(), "RxInteractiveObject") as Component;
            return o == null ? "-" : $"{o.GetType().Name}:{o.name}";
        }

        public static bool Focused(Component target) => target && ReferenceEquals(Value(MyView(), "RxInteractiveObject"), target);

        // Put the mouse on a world point, as a player aims (the game's aim and the interaction focus follow the cursor).
        public static void Aim(Vector3 world)
        {
            var cam = Camera.main;
            if (!cam) return;
            var mouse = Mouse;
            if (!mouse.enabled) InputSystem.EnableDevice(mouse);
            InputSystem.QueueStateEvent(mouse, new MouseState { position = (Vector2)cam.WorldToScreenPoint(world) });
            InputSystem.Update();
        }

        // Stand at an object until the interaction focus (CharacterView.RxInteractiveObject) is on it, then hold F.
        public static IEnumerator InteractObject(Component target, float hold, string what, float arrive = 1.0f)
        {
            yield return WalkTo(() => target ? target.transform.position : (Vector3?)null, arrive, 12f, what);
            // Hold at least the object's own HoldingTime (the dive booth wants 1.5 s, DungeonEntranceInteractionTime).
            if (target && Get(target, "HoldingTime") is float need && need + 0.3f > hold) hold = need + 0.3f;
            for (int i = 0; i < 3 && target; i++)
            {
                Aim(target.transform.position);
                yield return WaitUntil(() => Focused(target), 2f, $"focus {what}");
                Log($"interact {what} try={i} focus={Focus()}");
                yield return Keys(hold, Key.F);
                yield return new WaitForSecondsRealtime(0.6f);
                if (!Focused(target)) break;
                // In the booth (StageReady) or a looting menu (Interact) another F would cancel it.
                var state = Get(MyCharacter(), "State")?.ToString();
                if (state == "StageReady" || state == "Interact") break;
                if (target.GetType().Name == "Entrance" && DoorState(target) == "Opened") break;
                if (TopPopup() != null || Find("InventoryManagementPagePresenter") != null) break;
            }
        }

        // Walk up to an NPC (they wander), aim at it and hold F (NPC HoldingTime 0.5 s). The character's interaction
        // sensor is small, so keep pushing toward the NPC until the focus is on it.
        public static IEnumerator InteractNpc(Component npc, string what)
        {
            yield return WalkTo(() => npc ? npc.transform.position : (Vector3?)null, 0.5f, 20f, what);
            float push = Time.realtimeSinceStartup;
            while (!Focused(npc) && Time.realtimeSinceStartup - push < 2.5f && npc)
            {
                var me = MyCharacter();
                if (me == null) break;
                var d = Flat(npc.transform.position - me.transform.position);
                if (d.magnitude < 0.6f) break;
                Hold(KeysToward(d));
                yield return new WaitForSecondsRealtime(0.1f);
            }
            Hold();
            if (npc) Aim(npc.transform.position + Vector3.up * 0.8f);
            yield return WaitUntil(() => Focused(npc), 3f, $"focus {what}");
            Log($"interact {what} focus={Focus()}");
            yield return Keys(0.7f, Key.F);
        }

        // ---- NPCs ----

        public static System.Collections.Generic.IEnumerable<Component> Npcs() =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject"));

        public static int NpcId(Component n) => Get(n, "NpcId") is int id ? id : 0;

        // The nearest live NPC of that id (Narcis stands in two places).
        public static Component FindNpc(int id)
        {
            var me = MyCharacter();
            return Npcs().Where(n => NpcId(n) == id)
                .OrderBy(n => me ? Flat(n.transform.position - me.transform.position).sqrMagnitude : 0f).FirstOrDefault();
        }

        public static int[] FunctionIds(Component n) => Get(n, "NpcFunctionIds") as int[] ?? Array.Empty<int>();

        static Type s_tNpcFunction;

        public static string FunctionType(int id)
        {
            s_tNpcFunction ??= AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("GameCommon.Table.TNpcFunction")).FirstOrDefault(t => t != null);
            var table = s_tNpcFunction?.GetProperty("Table")?.GetValue(null);
            var row = table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id });
            return Get(row, "Type")?.ToString() ?? "?";
        }

        // Click the selection popup's slot for an NPC function (slot index = position in the NPC's function list).
        public static IEnumerator SelectFunction(Component npc, string function)
        {
            int functionId = FunctionIds(npc).FirstOrDefault(id => FunctionType(id) == function);
            int index = Array.IndexOf(FunctionIds(npc), functionId);
            var slots = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "SelectionSlotPresenter").ToList();
            Log($"selection {function}: want index {index} slots=[{string.Join(", ", slots.Select(s => $"{Get(Get(s, "_model"), "Index")}:'{Get(Get(Get(s, "_view"), "Text"), "text")}'"))}]");
            var slot = slots.FirstOrDefault(s => Get(Get(s, "_model"), "Index") is int i && i == index);
            if (slot == null) yield break;
            var button = Get(Get(slot, "_view"), "Button") as UnityEngine.UI.Button;
            yield return WaitUntil(() => Ready(button), 5f, $"selection slot {index}");
            yield return new WaitForSecondsRealtime(0.3f);
            button.onClick.Invoke();
        }

        // ---- units ----

        public static int Hp(Component unit)
        {
            if (unit == null) return -1;
            foreach (var c in unit.GetComponents<Component>())
                if (Get(c, "Hp") is int hp) return hp;
            return -1;
        }

        public static bool Alive(Component m) => m != null && m.gameObject.activeInHierarchy && Hp(m) > 0;

        public static System.Collections.Generic.IEnumerable<Component> Monsters() =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Where(m => m != null && m.GetType().Name == "MonsterController" && Alive(m));

        public static Component NearestAliveMonster(float range)
        {
            var me = MyCharacter();
            if (me == null) return null;
            return Monsters().Select(m => (m, d: Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }
    }
}

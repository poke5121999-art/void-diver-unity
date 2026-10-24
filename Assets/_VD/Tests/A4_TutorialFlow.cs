using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A4: a fresh profile presses New Game on the title and plays into the tutorial (campaign 1100), like docs/PLAN.md
    // "Luồng từ title vào game": name, difficulty, room, lounge prologue, tutorial stage. Captures use docs/review/ref keys.
    public class A4_TutorialFlow
    {
        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        [UnityTest, Timeout(2400000)]  // the runner's default is 180 s; boot, prologue and stage take longer
        public IEnumerator NewGameToTutorial()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A4";
            Flow.Log($"start platform={Flow.Platform()}");
            SceneManager.LoadScene(0);

            // Title: StartScene turns the menu on once input is enabled (SetMenuActive).
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            var nm = Flow.Find("NetworkManager");
            var transport = Flow.Get(Flow.Get(nm, "NetworkConfig"), "NetworkTransport");
            Flow.Log($"title play='{Flow.Get(Flow.Get(title, "Play_Button_Text"), "text")}' slot='{Flow.Get(Flow.Get(title, "SelectSlot_Player_Name"), "text")}' transport={transport?.GetType().Name}");
            yield return Flow.Click(title, "Play_Button");

            // Name, then the "use this name?" confirmation.
            yield return Flow.WaitView("InputNamePopupView", 30f);
            Assert.IsTrue(Flow.LastWaitOk, "name popup did not open after New Game");
            var nameView = Flow.Find("InputNamePopupView");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Type(Flow.Get(nameView, "InputField"), "Diver");
            yield return Review.Shot("a4-name");
            yield return Flow.Click(nameView, "Confirm_Button");
            yield return Flow.WaitView("TextPopupView", 20f);
            Assert.IsTrue(Flow.LastWaitOk, "name confirmation did not open");
            yield return Flow.Click(Flow.Find("TextPopupView"), "Confirm_Button");

            // Difficulty (default Junior).
            yield return Flow.WaitView("RoomDifficultyPopupView", 30f);
            Assert.IsTrue(Flow.LastWaitOk, "difficulty popup did not open");
            yield return new WaitForSecondsRealtime(1f);
            yield return Review.Shot("a4-difficulty");
            yield return Flow.Click(Flow.Find("RoomDifficultyPopupView"), "Confirm_Button");

            // Local platform only: the dev room popup with 127.0.0.1:7777 filled in.
            yield return Flow.WaitUntil(() => Flow.Find("LocalRoomCreationPopupView") != null || Flow.ActiveScene == "LoungeScene", 60f, "room popup or lounge");
            var room = Flow.Find("LocalRoomCreationPopupView");
            if (room != null) yield return Flow.Click(room, "BtnConfirm_Button");

            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 180f, "LoungeScene");
            Assert.IsTrue(Flow.LastWaitOk, "lounge did not load");
            Flow.Log($"lounge {Flow.LoadState()}");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing" || Flow.Find("DialogPopupView") != null, 240f, "lounge Playing or prologue dialog");
            Flow.Log($"lounge {Flow.LoadState()}");
            yield return Review.Shot("a4-lounge-load");
            Assert.IsTrue(Flow.LastWaitOk, $"lounge load stuck: {Flow.LoadState()}");

            // Prologue (Lua Campaign/1100 OnLounge): dialog lines and the 7-page letter, advanced by clicking, until the
            // script calls ForceStartStage.
            yield return AdvanceDialogs(() => Flow.ActiveScene == "StageScene", 900f, "prologue", "a4-prologue", "a4-prologue-letter");
            Assert.AreEqual("StageScene", Flow.ActiveScene, "prologue did not lead to the tutorial stage");

            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.DumpStage();

            // Tutorial start: Step_00003 opens a dialog over the scene (ref tutorial-start/01), then the HUD comes back.
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 30f, "tutorial dialog");
            if (Flow.LastWaitOk)
            {
                yield return new WaitForSecondsRealtime(2f);
                yield return Review.Shot("tutorial-start");
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "tutorial intro", null, null);
            }
            else yield return Review.Shot("tutorial-start");
            yield return new WaitForSecondsRealtime(3f);
            yield return Review.Shot("a4-tutorial-hud");
            Flow.DumpStage();

            // Walk: learn how WASD moves the character on this map, and look for Elara's radio hints and monsters.
            Flow.RouteInputToGame();
            bool radioShot = false, combatShot = false;
            // One diagonal step logs how the keys reach the game (device, action map, controller).
            Flow.Hold(Key.W);
            yield return new WaitForSecondsRealtime(0.5f);
            Flow.Log($"input W {InputState()}");
            Flow.Hold();

            // Elara's terminal next to the start point (InteractiveTrigger 11002, Lua TalkToEll_1): walk up, press F, and
            // her radio line comes up (RadioSay -> RadioToast), as the ref tutorial-dialog frame shows.
            var terminal = FindTrigger("InteractiveTrigger", 11002);
            if (terminal != null)
            {
                yield return WalkTo(() => terminal ? terminal.transform.position : (Vector3?)null, 1.0f, 15f, "Elara terminal");
                yield return Flow.Keys(0.6f, Key.F);
                yield return Flow.WaitUntil(() => Flow.Find("RadioToastView") != null || Flow.Find("DialogPopupView") != null, 6f, "radio toast (terminal)");
                if (Flow.Find("RadioToastView") != null)
                {
                    yield return new WaitForSecondsRealtime(1.5f);
                    radioShot = true;
                    yield return Review.Shot("tutorial-dialog");
                }
            }

            // Elara's first radio line: collision trigger 100091 (Lua CallCollision_1), on the way out of the start room.
            var trigger = FindTrigger("CollisionTrigger", 100091);
            if (trigger != null)
            {
                yield return WalkTo(() => trigger ? trigger.transform.position : (Vector3?)null, 0.8f, 25f, "trigger 100091");
                yield return Flow.WaitUntil(() => Flow.Find("RadioToastView") != null || Flow.Find("DialogPopupView") != null, 6f, "radio toast");
                if (!radioShot && Flow.Find("RadioToastView") != null)
                {
                    yield return new WaitForSecondsRealtime(1.5f);
                    radioShot = true;
                    yield return Review.Shot("tutorial-dialog");
                }
                if (Flow.Find("DialogPopupView") != null)
                    yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "stage dialog", null, null);
            }

            // First fight: walk up to the nearest monster and swing at it with the left button, aimed at it.
            yield return WalkTo(() => NearestMonster()?.transform.position, 1.5f, 30f, "monster");
            var monster = NearestMonster();
            if (monster != null && NearMonster(4f))
            {
                Flow.Log($"combat start monster hp={Hp(monster)} at {monster.transform.position}");
                for (int i = 0; i < 6; i++)
                {
                    var cam = Camera.main;
                    Vector2 aim = cam ? (Vector2)cam.WorldToScreenPoint(monster ? monster.transform.position : Flow.MyCharacter().transform.position)
                                      : new Vector2(Screen.width / 2f, Screen.height / 2f);
                    yield return Flow.MouseClick(aim);
                    yield return new WaitForSecondsRealtime(0.15f);
                    if (i == 2) { combatShot = true; yield return Review.Shot("tutorial-first-combat"); }
                }
                Flow.Log($"combat hp={(monster ? Hp(monster) : "dead")} me={Flow.MyCharacter()?.transform.position} monster={(monster ? monster.transform.position.ToString() : "dead")} {Flow.LoadState()}");
            }
            if (Flow.Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "stage dialog", null, null);
            Flow.DumpStage();
            Flow.Log($"end radio={radioShot} combat={combatShot}");
            Assert.Pass($"reached {Flow.ActiveScene} stage={Flow.StageState()}");
        }

        // Where WASD stops: device state, the game's Player action map and its block counts, the controller's _forward.
        static string InputState()
        {
            var pic = Flow.Find("PlayerInputController");
            var im = Flow.Get(pic, "_inputManager");
            var player = Flow.Get(im, "PlayerActions");
            var blocks = Flow.Get(im, "_typeBlockCounts") as System.Collections.IDictionary;
            string b = "";
            if (blocks != null) foreach (System.Collections.DictionaryEntry e in blocks) b += $"{e.Key}:{e.Value},";
            var move = Flow.Get(player, "MoveKeyboard") as InputAction;
            return $"kb={Keyboard.current?.name} kbEnabled={Keyboard.current?.enabled} w={Keyboard.current?.wKey.isPressed} d={Keyboard.current?.dKey.isPressed} pic={(pic != null)} playerEnabled={Flow.Get(player, "enabled")} " +
                   $"move={move?.ReadValue<Vector2>()} moveEnabled={move?.enabled} phase={move?.phase} forward={Flow.Get(pic, "_forward")} canSend={Flow.Get(pic, "CanSendInput")} " +
                   $"blocks={b} hud={Flow.Value(Flow.Find("GameManager"), "RxActiveInGameHud")}";
        }

        static Component FindTrigger(string typeName, int id)
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == typeName && m.isActiveAndEnabled && Flow.Get(m, "_triggerId") is int t && t == id)
                    return m;
            return null;
        }

        static Component NearestMonster()
        {
            var me = Flow.MyCharacter();
            if (me == null) return null;
            Component best = null;
            float bestD = float.MaxValue;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "MonsterController" && m.isActiveAndEnabled)
                {
                    float d = Vector3.Distance(m.transform.position, me.transform.position);
                    if (d < bestD) { bestD = d; best = m; }
                }
            return best;
        }

        // Walk with WASD toward a world point. The camera is isometric: W moves along world (-x,+z), D along (+x,+z)
        // (measured in A4 run 11), so a world direction splits into a W/S part and an A/D part.
        static IEnumerator WalkTo(System.Func<Vector3?> target, float arrive, float timeout, string what)
        {
            float start = Time.realtimeSinceStartup;
            Vector3 last = Vector3.zero;
            int stuck = 0;
            bool pathLogged = false;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = Flow.MyCharacter();
                var t = target();
                if (me == null || t == null) break;
                Vector3 goal = t.Value - me.transform.position;
                goal.y = 0;
                if (goal.magnitude <= arrive) break;
                // Rooms are walled: steer along the stage's NavMesh path (the monsters' own navigation) to the next corner.
                Vector3 d = goal;
                var path = new UnityEngine.AI.NavMeshPath();
                if (UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, t.Value, UnityEngine.AI.NavMesh.AllAreas, path) && path.corners.Length > 1)
                {
                    int c = 1;
                    while (c < path.corners.Length - 1 && Vector3.Distance(Flat(path.corners[c]), Flat(me.transform.position)) < 0.4f) c++;
                    d = path.corners[c] - me.transform.position;
                    d.y = 0;
                }
                else if (!pathLogged) { pathLogged = true; Flow.Log($"walk {what} no NavMesh path from {me.transform.position} status={path.status} corners={path.corners.Length} last={(path.corners.Length > 0 ? path.corners[path.corners.Length - 1].ToString() : "-")}"); }
                if (Flow.Find("DialogPopupView") != null) { Flow.Hold(); yield break; }
                float a = (d.z - d.x) / 2f, b = (d.x + d.z) / 2f, m = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
                var keys = new System.Collections.Generic.List<Key>();
                if (Mathf.Abs(a) > 0.4f * m) keys.Add(a > 0 ? Key.W : Key.S);
                if (Mathf.Abs(b) > 0.4f * m) keys.Add(b > 0 ? Key.D : Key.A);
                Flow.Hold(keys.ToArray());
                yield return new WaitForSecondsRealtime(0.15f);
                stuck = (me.transform.position - last).sqrMagnitude < 0.0004f ? stuck + 1 : 0;
                last = me.transform.position;
                if (stuck > 14) { Flow.Log($"walk {what} stuck at {last}"); break; }
            }
            Flow.Hold();
            yield return null;
            var end = Flow.MyCharacter();
            Flow.Log($"walk {what} me={(end ? end.transform.position.ToString() : "-")} target={target()} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static string Hp(Component unit)
        {
            foreach (var c in unit.GetComponents<Component>())
                if (Flow.Get(c, "Hp") is int hp) return hp.ToString();
            return "?";
        }

        static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

        static bool NearMonster(float range)
        {
            var me = Flow.MyCharacter();
            if (me == null) return false;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "MonsterController" && m.isActiveAndEnabled
                    && Vector3.Distance(m.transform.position, me.transform.position) < range)
                    return true;
            return false;
        }

        // DialogPopupPresenter.DelayDialogAsync is running (a Lua pause between lines). A click then cancels the delay
        // (OnNext), and the cancelled UniTask never runs LuaApi's ContinueWith, so the Lua await would never finish; the
        // original binary does the same (no OperationCanceledException catch in <DelayDialogAsync>d__66). A player clicks
        // to advance the line on screen, so the test waits the pause out.
        static bool DialogDelaying()
        {
            var p = Flow.Find("DialogPopupPresenter");
            return p != null && Flow.Get(p, "_isDelayActive") is bool b && b;
        }

        static string DialogState()
        {
            var p = Flow.Find("DialogPopupPresenter");
            if (p == null) return "presenter=-";
            var entry = Flow.Get(p, "_currentEntry");
            string msg = Flow.Get(entry, "Message") as string;
            if (msg != null && msg.Length > 40) msg = msg.Substring(0, 40);
            var tw = Flow.Get(p, "_typeWriter");
            var ta = Flow.Get(tw, "TextAnimator");
            return $"delay={Flow.Get(p, "_isDelayActive")} writing={Flow.Get(p, "_isWritingText")} state={Flow.Get(p, "_state")} " +
                   $"needSelect={Flow.Get(p, "_needSelect")} canClose={Flow.Get(p, "_canClose")} queue={(Flow.Get(p, "_dialogQueue") as System.Collections.ICollection)?.Count} " +
                   $"entry='{msg}' twShowing={Flow.Get(tw, "isShowingText")} chars={Flow.Get(ta, "CharactersCount")}";
        }

        // Click through DialogPopup lines and NoteSystem letters until done() holds. The first click on a line shows the
        // rest of its text, the next one moves on (DialogPopupPresenter.OnNext), as a player clicking does.
        static IEnumerator AdvanceDialogs(System.Func<bool> done, float timeout, string what, string dialogShot, string noteShot)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            int clicks = 0, dialogSeen = 0;
            Flow.Log($"advance {what}");
            while (!done() && Time.realtimeSinceStartup - start < timeout)
            {
                var note = Flow.Find("NoteSystemPopupView");
                var dialog = Flow.Find("DialogPopupView");
                if (note != null && Flow.Ready(Flow.Button(note, "BaseBgDimmer")))
                {
                    if (noteShot != null) { yield return new WaitForSecondsRealtime(1.5f); yield return Review.Shot(noteShot); noteShot = null; }
                    Flow.Button(note, "BaseBgDimmer").onClick.Invoke();
                    clicks++;
                }
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")) && !DialogDelaying())
                {
                    if (dialogShot != null && ++dialogSeen == 6) { yield return Review.Shot(dialogShot); dialogShot = null; }
                    Flow.Button(dialog, "Background").onClick.Invoke();
                    clicks++;
                }
                if (Time.realtimeSinceStartup > nextLog)
                {
                    nextLog += 10f;
                    Flow.Log($"advance {what} clicks={clicks} note={note != null} dialog={dialog != null} {DialogState()} {Flow.LoadState()}");
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Flow.Log($"advance {what} done={done()} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s");
        }
    }
}

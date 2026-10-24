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
            Key[][] moves = { new[] { Key.W }, new[] { Key.D }, new[] { Key.S }, new[] { Key.A }, new[] { Key.W, Key.D }, new[] { Key.W, Key.A } };
            for (int round = 0; round < 4; round++)
            {
                foreach (var keys in moves)
                {
                    var me = Flow.MyCharacter();
                    var before = me ? me.transform.position : Vector3.zero;
                    yield return Flow.Keys(2f, keys);
                    me = Flow.MyCharacter();
                    Flow.Log($"move {string.Join("+", keys)} {before} -> {(me ? me.transform.position.ToString() : "-")}");
                    if (!radioShot && Flow.Find("RadioToastView") != null)
                    {
                        radioShot = true;
                        yield return Review.Shot("tutorial-dialog");
                    }
                    if (!combatShot && NearMonster(6f))
                    {
                        for (int i = 0; i < 3; i++) yield return Flow.MouseClick(new Vector2(Screen.width / 2f + 60, Screen.height / 2f));
                        yield return new WaitForSecondsRealtime(0.2f);
                        combatShot = true;
                        yield return Review.Shot("tutorial-first-combat");
                    }
                    if (Flow.Find("DialogPopupView") != null)
                        yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "stage dialog", null, null);
                }
            }
            Flow.DumpStage();
            Flow.Log($"end radio={radioShot} combat={combatShot}");
            Assert.Pass($"reached {Flow.ActiveScene} stage={Flow.StageState()}");
        }

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
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")))
                {
                    if (dialogShot != null && ++dialogSeen == 6) { yield return Review.Shot(dialogShot); dialogShot = null; }
                    Flow.Button(dialog, "Background").onClick.Invoke();
                    clicks++;
                }
                if (Time.realtimeSinceStartup > nextLog)
                {
                    nextLog += 10f;
                    Flow.Log($"advance {what} clicks={clicks} note={note != null} dialog={dialog != null} {Flow.LoadState()}");
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Flow.Log($"advance {what} done={done()} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s");
        }
    }
}

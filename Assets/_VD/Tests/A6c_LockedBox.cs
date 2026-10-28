using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A6c: a locked reward box opened without its key lets the diver go again when the key popup is closed.
    // The game (InGameScene.OnKeyInteractionRewardBoxAsync) opens KeyInteractionPopupView on F at a box whose TRewardBox
    // KeyItemIds the bag does not hold, and keeps the diver in state Interact until the popup is closed. Esc sends the cancel
    // and then ReqFinishInteractionWaiting, as in the original (a6-full11: the diver stayed in Interact on box 11306 for the
    // rest of the dive when only the cancel went out). This test is that case on its own, in a real stage:
    // - Start Normal campaign 111 (Town) with the cheat menu's own call, as A6b does (SROptions.StartCampaign): Elara's board
    //   is a random pick, so a given id cannot be chosen like a player. Logged as a cheat.
    // - Dive through the booth. Sector 4002 is the fixed sector at grid cell [0,7] of campaigns 111-118; its reward group
    //   holds group-10300 entries (box 10300, SafeBox, key 9012). Group 10300 is also in sectors 4004 and 4005, 11300 holds
    //   the locked 11306/11309. A box exists only within 10 m of a diver (GameObjectManager.UpdateRewardBoxesSpawns), so the
    //   diver walks to each spawn point of those groups. The points come from the live stage (GameObjectManager.
    //   _rewardBoxSpawnDatas, picked at stage start with the sector offset already applied), not from the sector tables.
    // - The first RewardBox in view whose TableData.KeyItemIds is not empty is the one: hold F for HoldingTime + 0.3 s, wait
    //   for KeyInteractionPopupView, shot (a6c-keypopup), Esc, wait for the popup to close, then the state must leave Interact
    //   within 3 s. No locked box in the stage after every point: Inconclusive (random placement), never a pass.
    // - Leave by the game's own terminal; only when that does not end the dive the cheat menu's escape (ReqForceEscape) does.
    //   Dive Results, Confirm, and the open campaign is given up at Elara so the next case starts from a Ready room.
    // Cheats: StartCampaign (above), the HP/stress refill (A9aDive.KeepAlive). The key is kept out of the bag (stashed first).
    public class A6c_LockedBox
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        static readonly int[] BoxGroups = { 10300, 11300, 10600 };   // the 9012 box, its group 11300 (11306/11309), the 9015 box
        static readonly int[] Keys = { 9012, 9015 };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A6c_LockedBox")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A6C";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
        }

        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        [OneTimeTearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        [UnityTest, Timeout(1800000)]
        public IEnumerator LockedBoxLetsGo()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6C";
            A9aDive.ResetCheats();
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Flow.RouteInputToGame();
            // Lounge quest 80200 first, as A6b does: Elara's quest lines take her F until it is done, and the campaign is
            // given up at her at the end.
            yield return Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Talk(id, $"{who} 80200");
            yield return Talk(700000, "Elara 80200 finish");
            yield return DrainDialogs("session");

            yield return StashKeys();
            Assert.AreEqual(0, KeysCarried(), $"keys {string.Join(",", Keys)} are still in the bag after stashing it");

            Flow.GameLog.Start();
            string fail = null, inconclusive = null;
            bool tested = false;
            try
            {
                yield return StartCampaign(111, s => fail ??= s);
                if (fail == null)
                    yield return A9aDive.Safe(Test(s => fail ??= s, s => inconclusive ??= s, () => tested = true), "locked box");
                yield return LeaveStage();
            }
            finally
            {
                Flow.GameLog.Stop();
            }
            Flow.Log($"RESULT tested={tested} fail={fail ?? "-"} cheats=[{string.Join(", ", A9aDive.Cheats)}]");
            Assert.IsNull(fail, $"locked box: {fail}");
            if (!tested) Assert.Inconclusive(inconclusive ?? "no locked box spawned in sector 4002 (random placement, ~96.5%)");
            Flow.GameLog.AssertClean("locked box");
        }

        // ---- the test in the stage ----

        static IEnumerator Test(Action<string> fail, Action<string> inconclusive, Action done)
        {
            yield return new WaitForSecondsRealtime(3f);
            var points = A9aDive.BoxPoints(BoxGroups);
            Flow.Log($"stage 111: {points.Count} box spawn points of groups [{string.Join(",", BoxGroups)}] at [{string.Join(" ", points.Select(p => p.ToString("F0")))}] {A9aDive.MeState()}");
            yield return Review.Shot("a6c-stage");

            Component box = LockedBox();
            var todo = new List<Vector3>(points);
            while (box == null && todo.Count > 0 && !A9aDive.Over())
            {
                var me = A9aDive.Me();
                if (me == null) break;
                var next = todo.OrderBy(p => Flow.Flat(p - me.transform.position).sqrMagnitude).First();
                todo.Remove(next);
                // Within 10 m the box spawns; 6 m leaves room for the NavMesh not reaching the exact spot.
                Flow.Log($"box point {next:F1}: {Flow.Flat(next - me.transform.position).magnitude:F0} m away, {todo.Count} left");
                yield return A9aDive.GoFar(next, 6f, "box point", 150f);
                yield return A9aDive.Breathe(1f);
                box = LockedBox();
            }
            if (A9aDive.Over()) { fail("the diver was dead or out of the stage before a locked box was reached"); yield break; }
            if (box == null)
            {
                inconclusive($"no locked box spawned in sector 4002 (random placement, ~96.5%); visited {points.Count} spawn points");
                yield break;
            }

            int id = A9aDive.BoxTableId(box);
            string name = box.name;
            Flow.Log($"locked box {id} {name} at {box.transform.position} keys=[{string.Join(",", KeyIds(box))}] carried={KeysCarried()}");
            yield return A9aDive.GoFar(box.transform.position, 1.5f, $"box {id}", 90f);
            if (!box) { fail($"box {id} was gone before the diver reached it"); yield break; }
            yield return Flow.WalkTo(() => box ? box.transform.position : (Vector3?)null, 0.9f, 8f, $"box {id}");
            if (!box) { fail($"box {id} was gone before the diver reached it"); yield break; }
            Flow.Aim(box.transform.position);
            yield return Flow.WaitUntil(() => Flow.Focused(box), 3f, $"focus box {id}");
            if (!Flow.LastWaitOk) { fail($"the interaction focus never reached box {id} ({Flow.Focus()})"); yield break; }

            float hold = Flow.Get(box, "HoldingTime") is float h ? h + 0.3f : 0.8f;
            yield return Flow.Keys(Mathf.Clamp(hold, 0.3f, 3f), Key.F);
            yield return Flow.WaitUntil(() => Flow.Find("KeyInteractionPopupView") != null || Flow.Find("LootingInventoryPanelPresenter") != null, 6f, $"box {id} popup");
            if (Flow.Find("KeyInteractionPopupView") == null)
            {
                fail($"F on the locked box {id} opened {(Flow.Find("LootingInventoryPanelPresenter") != null ? "the looting panel" : "nothing")}, not the key popup (state={A9aDive.State()}, top={Flow.TopPopup()})");
                yield break;
            }
            done();
            Flow.Log($"box {id}: key popup open, state={A9aDive.State()}");
            yield return new WaitForSecondsRealtime(0.8f);
            yield return Review.Shot("a6c-keypopup");

            yield return Flow.Keys(0.2f, Key.Escape);
            yield return Flow.WaitUntil(() => Flow.Find("KeyInteractionPopupView") == null, 4f, $"box {id} key popup closed");
            if (Flow.Find("KeyInteractionPopupView") != null) { fail($"the key popup of box {id} stayed open after Esc"); yield break; }
            // The diver must be free again: the game sends the cancel and then ReqFinishInteractionWaiting.
            yield return Flow.WaitUntil(() => A9aDive.State() != "Interact", 3f, $"box {id} interaction over");
            if (A9aDive.State() == "Interact") { fail($"the diver is still held by the locked box {id} (state Interact) 3 s after its key popup closed"); yield break; }
            Flow.Log($"locked box let go: box {id}, state={A9aDive.State()}");
        }

        // A box in view that has a key (TRewardBox KeyItemIds), nearest by path.
        static Component LockedBox() => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "RewardBox" && KeyIds(m).Length > 0)
            .OrderBy(m => A9aDive.PathLength(m.transform.position)).FirstOrDefault();

        static int[] KeyIds(Component box) =>
            (Flow.Get(Flow.Get(box, "TableData"), "KeyItemIds") as IEnumerable)?.Cast<object>().Select(Convert.ToInt32).ToArray() ?? Array.Empty<int>();

        // ---- the keys in the bag ----

        static Component PM() => Flow.Find("PlayerManager");

        static int KeysCarried()
        {
            int n = 0;
            foreach (var slots in new[] { "InventorySlots", "SafeInventorySlots" })
                foreach (var slot in Flow.Get(Flow.Get(PM(), slots), "Values") as IEnumerable ?? new object[0])
                {
                    var goods = Flow.Get(slot, "Goods");
                    if (goods != null && Flow.Get(goods, "Category")?.ToString() == "Item" && Flow.Get(goods, "Id") is int id && Keys.Contains(id)) n++;
                }
            return n;
        }

        static int BagUsed() => (Flow.Get(Flow.Get(PM(), "InventorySlots"), "Values") as IEnumerable)?.Cast<object>()
            .Count(s => !(Flow.Get(s, "IsEmpty") is bool e && e)) ?? -1;

        // Keys in the bag go to the stash like a player's (lounge: Tab opens the inventory beside the stash, T moves everything
        // that fits; InventoryManagementPagePresenter.OnMoveToStashAll), so the box has to ask for them.
        static IEnumerator StashKeys()
        {
            int carried = KeysCarried(), before = BagUsed();
            Flow.Log($"keys {string.Join(",", Keys)} in the bag: {carried}, bag {before} used slots");
            if (carried == 0 || before <= 0) yield break;
            yield return Flow.Keys(0.15f, Key.Tab);
            yield return Flow.WaitUntil(() => Flow.Find("MenuPopupPresenter") != null, 4f, "inventory");
            Assert.IsNotNull(Flow.Find("MenuPopupPresenter"), $"Tab opened no inventory, top={Flow.TopPopup()}");
            yield return new WaitForSecondsRealtime(1f);
            yield return Flow.Keys(0.15f, Key.T);
            yield return Flow.WaitUntil(() => BagUsed() < before, 5f, "bag to the stash");
            for (int i = 0; i < 2 && Flow.Find("MenuPopupPresenter") != null; i++)
            {
                yield return Flow.Keys(0.2f, i == 0 ? Key.Escape : Key.Tab);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        // ---- campaign, copied from A6b_CampaignSweep (its helpers are private) ----

        static string Room() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_roomContext"), "RxRoomState")?.ToString() ?? "-";
        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        static IEnumerator StartCampaign(int id, Action<string> fail)
        {
            yield return CloseOpenCampaign("before");
            if (Room() != "Ready") { fail($"room {Room()} before start"); yield break; }
            var gm = Flow.Find("GameManager");
            var cm = Flow.Find("GameCampaignManager");
            var anomaly = cm.GetType().GetMethod("GetCampaignAnomalyType").Invoke(cm, new object[] { id });
            var difficulty = Enum.Parse(GameType("GameCommon.Enum.EDifficulty") ?? anomaly.GetType().Assembly.GetTypes().First(t => t.Name == "EDifficulty"), "Normal");
            Flow.Log($"CHEAT start {id}: GameManager.StartCampaign({id}, Normal, {anomaly})");
            gm.GetType().GetMethod("StartCampaign").Invoke(gm, new[] { (object)id, difficulty, anomaly });
            yield return Flow.WaitUntil(() => CampaignNow() == id.ToString(), 10f, $"campaign {id} started");
            if (!Flow.LastWaitOk) { fail("campaign did not start"); yield break; }
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 20f, "start card gone");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Log($"booth check {A9aDive.BoothCheck()}");
            yield return A9aDive.EnterBooth();
            if (Flow.ActiveScene != "StageScene" || Flow.StageState() != "Playing") fail($"stage did not load ({Flow.ActiveScene}/{Flow.StageState()})");
        }

        static IEnumerator Talk(int npcId, string what)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"talk {what}: npc {npcId} not here"); yield break; }
            yield return Flow.InteractNpc(npc, what);
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, what);
            else if (Flow.Find("SelectionPopupView") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        static IEnumerator DrainDialogs(string what)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Flow.Find("DialogPopupView") != null) yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 90f, what);
                else if (Flow.TopPopup() != null) { Flow.Log($"{what}: popup {Flow.TopPopup()}, Esc"); yield return Flow.Keys(0.2f, Key.Escape); }
                else yield break;
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        // ---- leaving ----

        // The game's own terminal first; the cheat menu's escape only when it does not end the dive. Then the banner, the
        // Dive Results (A9aDive.Leave) and the campaign given up at Elara.
        static IEnumerator LeaveStage()
        {
            if (Flow.ActiveScene == "StageScene")
            {
                bool Gone() => A9aDive.Over() || Flow.Find("EscapeSuccessPopupView") != null;
                // A popup left open (the key popup after a failed run) is closed as a player would.
                for (int c = 0; c < 3 && Flow.TopPopup() != null && Flow.Find("DialogPopupView") == null; c++)
                {
                    yield return Flow.Keys(0.2f, Key.Escape);
                    yield return new WaitForSecondsRealtime(0.8f);
                }
                if (!Gone()) yield return A9aDive.Safe(A9aDive.TerminalEscape(), "terminal");
                if (!Gone())
                {
                    Flow.Log($"CHEAT escape: forced escape (no exit ended the dive) {A9aDive.MeState()}");
                    A9aDive.SendCheat("ReqForceEscape");
                    yield return Flow.WaitUntil(Gone, 20f, "forced escape");
                }
            }
            yield return A9aDive.Leave();
            yield return CloseOpenCampaign("after");
        }

        // A campaign still open after its dive keeps the room Playing. The player gives it up at Elara: her Campaign function
        // asks "abandon <campaign>?" (InGameScene.OnCampaignNpcAsync -> TextPopup, Confirm -> GameManager.CloseCampaign(GiveUp))
        // and opens the campaign menu, which Esc closes.
        static IEnumerator CloseOpenCampaign(string when)
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                if (Room() == "Ready" && (CampaignNow() == "0" || CampaignNow() == null)) yield break;
                Flow.Log($"{when}: campaign {CampaignNow()} still open (room {Room()}); abandon at Elara");
                var elara = Flow.FindNpc(700000);
                if (elara == null) break;
                yield return Flow.InteractNpc(elara, "Elara abandon");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("TextPopupView") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara line"); continue; }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(elara, "Campaign");
                }
                yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") != null, 5f, "abandon confirmation");
                var confirm = Flow.Find("TextPopupView");
                if (confirm != null)
                {
                    Flow.Log($"{when}: confirm '{Flow.Get(Flow.Get(confirm, "Message"), "text")}'");
                    yield return Flow.Click(confirm, "Confirm_Button");
                }
                yield return Flow.WaitUntil(() => Room() == "Ready" && CampaignNow() == "0", 10f, "campaign closed");
                yield return new WaitForSecondsRealtime(1f);
                if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
                yield return DrainDialogs($"{when} abandon");
            }
        }
    }
}

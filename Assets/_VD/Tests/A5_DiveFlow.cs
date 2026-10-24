using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A5: a normal campaign run played like a player. From the post-tutorial profile (tools/fixtures/saves-post-tutorial,
    // see A3_NpcFlow), Continue into the lounge, finish Elara's lounge quest 80200 (talk to Narcis, Felix, Lucas), take
    // campaign 1101 from Elara's Campaign menu, hear her briefing (Lua Campaign/1101 TalkToNPC), step into the dive booth
    // (PhoneBooth, F) and dive. In the stage: explore the random 4x4 Country map, fight with the left button and the
    // skills (right button, Q, E, R), take hits, loot corpses and boxes, open the inventory (Tab) and the map (M), step on
    // a trap, then find the exit terminal (WaveExit, hold F), hold out while it activates and stand in its field to escape.
    // Dying instead ends the run with the Extraction Failed banner. Either way the lounge shows the Dive Results; Confirm.
    // Shots use the docs/review/ref/dive-* keys and result-screen; extra shots are prefixed a5-.
    public class A5_DiveFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const int CampaignId = 1101;
        const float ExploreBudget = 300f;   // seconds of exploring before heading for the exit

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A5_DiveFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A5";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
        }

        // Errors logged while the game boots, before the test body runs, would fail the test (same as A1/A3/A4).
        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static readonly string[] Keys =
        {
            "dive-loading", "dive-explore", "dive-combat", "dive-hit-damage-numbers", "dive-skill-vfx", "dive-loot-pickup",
            "dive-inventory", "dive-minimap", "dive-trap", "dive-stress-high", "dive-exit", "dive-death", "result-screen",
        };

        static readonly HashSet<string> s_shots = new HashSet<string>();

        // One shot per key: the first time the state comes up.
        static IEnumerator Shot(string key)
        {
            if (!s_shots.Add(key)) yield break;
            Flow.Log($"shot {key} {MeState()}");
            yield return Review.Shot(key);
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator DiveCampaign1101()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A5";
            s_shots.Clear();
            Flow.Log($"start platform={Flow.Platform()}");
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();
            Flow.Log($"lounge me={Me()} inventory={InventoryIds()}");

            // Lounge quest 80200: Elara sends the player to Narcis, Felix and Lucas, then wants a word before her menu opens.
            yield return Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Talk(id, $"{who} 80200");
            yield return Talk(700000, "Elara 80200 finish");

            yield return StartCampaign();
            Assert.AreEqual(CampaignId.ToString(), CampaignNow(), "campaign 1101 did not start");

            // Lua Campaign/1101 Step_00001 puts the briefing (TalkToNPC) on Elara; Step_00002 sets the quest InProgress.
            yield return Briefing();
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");

            yield return Dive();
            yield return Results();

            var missing = Keys.Where(k => !s_shots.Contains(k)).ToList();
            Flow.Log($"end scene={Flow.ActiveScene} shots=[{string.Join(",", s_shots)}] missing=[{string.Join(",", missing)}]");
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the run did not come back to the lounge");
        }

        // ---- lounge ----

        // Walk up to an NPC and press F; play the dialog it starts, or leave its selection popup with Esc.
        static IEnumerator Talk(int npcId, string what)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"talk {what}: npc {npcId} not in the lounge"); yield break; }
            yield return Flow.InteractNpc(npc, what);
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, what);
            else if (Flow.Find("SelectionPopupView") != null)
            {
                Flow.Log($"talk {what}: selection popup, no quest line; Esc");
                yield return Flow.Keys(0.2f, Key.Escape);
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        // Elara, Campaign, the 1101 slot, Start Campaign.
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
            Flow.Log($"campaign slots=[{string.Join(",", slots.Select(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId")))}]");
            var slot = slots.FirstOrDefault(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId") is int id && id == CampaignId);
            if (slot == null) { Flow.Log($"no slot for campaign {CampaignId}"); yield break; }
            var slotButton = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, "campaign slot 1101");
            slotButton.onClick.Invoke();

            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == CampaignId, 6f, "campaign 1101 detail");
            if (detail == null) yield break;
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Review.Shot("a5-campaign-1101");
            var start = Flow.Get(Flow.Get(detail, "_view"), "Info_StartCampaignButton") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            Flow.Log($"start campaign {CampaignId} difficulty={Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxDifficulty")}");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == CampaignId.ToString(), 10f, "campaign started");
            // The campaign menu closes itself; the CampaignStartPopup card plays and goes.
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            Flow.Log($"after start campaign={CampaignNow()} top={Flow.TopPopup()} {Flow.PopupState()}");
            if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        // Elara's 1101 briefing (a Quest dialog, played before her menu); then the quest is InProgress and the booth opens.
        static IEnumerator Briefing()
        {
            yield return new WaitForSecondsRealtime(3.5f);  // Step_00001 waits 3 s before pinging Elara
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

        // PhoneBooth.CheckCanEnter for the local player: Success once the campaign is running and the diver has a weapon.
        static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            var id = Flow.Get(gm, "LocalClientId");
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { id })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        // F at the dive booth: StageReady, the stage loads behind the LOADING card (booth icon, tip).
        static IEnumerator EnterBooth()
        {
            var booth = Flow.Find("PhoneBooth");
            if (booth == null) { Flow.Log("no PhoneBooth in the lounge"); yield break; }
            yield return Review.Shot("a5-lounge-booth");
            for (int attempt = 0; attempt < 3 && State() != "StageReady" && Flow.ActiveScene == "LoungeScene"; attempt++)
            {
                yield return Flow.InteractObject(booth, 0.8f, "dive booth");
                yield return Flow.WaitUntil(() => State() == "StageReady" || Flow.ActiveScene != "LoungeScene", 4f, "StageReady");
                Flow.Log($"booth attempt {attempt}: state={State()} check={BoothCheck()} focus={Flow.Focus()}");
            }
            // The card opens when the stage scene starts loading (InGameScene.OnSceneLoading) and closes once it plays.
            yield return Flow.WaitUntil(() => PopupOpen("GameLoadingPopupPresenter") || Flow.StageState() == "Playing" && Flow.ActiveScene == "StageScene", 30f, "loading card");
            Flow.Log($"loading: {Flow.PopupState()} view={(Flow.Find("GameLoadingPopupView") != null)} scene={Flow.ActiveScene} stage={Flow.StageState()}");
            if (PopupOpen("GameLoadingPopupPresenter"))
            {
                // The card fades in over the scene that is loading behind it; shoot it once it is opaque.
                yield return Flow.WaitUntil(() => !(Flow.Find("GameLoadingPopupView") is Component v) || !(v.GetComponentInParent<CanvasGroup>() is CanvasGroup g) || g.alpha >= 0.99f, 3f, "loading card opaque");
                Flow.Log($"loading card alpha={(Flow.Find("GameLoadingPopupView") is Component lv && lv.GetComponentInParent<CanvasGroup>() is CanvasGroup lg ? lg.alpha : -1f)}");
                yield return Shot("dive-loading");
            }
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.Log($"stage {Flow.LoadState()}");
        }

        // ---- the dive ----

        static Component Me() => Flow.MyCharacter();
        static string State() => Flow.Get(Me(), "State")?.ToString() ?? "-";
        static float HpMax() => Flow.Get(Me(), "HpMax") is float f ? f : 1f;
        static float HpFrac() => Flow.Hp(Me()) / Mathf.Max(1f, HpMax());
        static float Stress() => Flow.Get(Me(), "Stress") is float f ? f : 0f;
        static bool Over() => State() == "Dead" || State() == "Escaped" || Flow.ActiveScene != "StageScene" || Flow.StageState() == "Ended";
        static bool Down() => Over() || State() == "Dying";

        static bool PopupOpen(string presenter) =>
            Flow.Get(Flow.Find("PopupManager"), "_popups") is IEnumerable list && list.Cast<object>().Any(p => p?.GetType().Name == presenter);

        // At 0 HP the diver lies dying (death's door) until it revives or dies; nothing to press meanwhile.
        static IEnumerator WhileDying()
        {
            if (State() != "Dying") yield break;
            // Game time, not wall time: batchmode runs slow frames, and the game's timers (Observable.Timer) run on it.
            float t0 = Time.time;
            Flow.Log($"dying {MeState()}");
            // ref dive-stress-high/01: the first death in 1101 is a self-revive (UnitDyingState: 3.7 s of the dark
            // death-threshold sight, NtfSelfReviveText at 2.5 s puts "Death's door!" on the HUD). Shoot at 2.8 s.
            while (Time.time - t0 < 0.5f) yield return null;
            yield return Review.Shot("a5-dying");
            while (Time.time - t0 < 2.9f && State() == "Dying") yield return null;
            Flow.Log($"death's door at game {Time.time - t0:F1}s");
            Flow.Log($"death's door: {MeState()}");
            if (State() == "Dying" || State() == "Revive") yield return Shot("dive-stress-high");
            yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 240f, "out of Dying");
            Flow.Log($"after dying {MeState()} stage={Flow.StageState()}");
        }

        static string MeState()
        {
            var me = Me();
            return me == null ? "me=-" : $"hp={Flow.Hp(me)}/{HpMax():F0} stress={Stress():F0}/{Flow.Get(me, "StressState")} state={State()} pos={me.transform.position}";
        }


        static readonly HashSet<int> s_looted = new HashSet<int>();
        static readonly HashSet<int> s_trapsTried = new HashSet<int>();
        static readonly List<Vector3> s_waypoints = new List<Vector3>();
        static int s_kills, s_hitsTaken, s_skillUses, s_boxes;
        static bool s_healed;

        static IEnumerator Dive()
        {
            s_looted.Clear(); s_trapsTried.Clear(); s_waypoints.Clear(); s_ignored.Clear();
            s_kills = s_hitsTaken = s_skillUses = s_boxes = 0;
            s_healed = false;
            s_bubbleShot = false;
            yield return new WaitForSecondsRealtime(3f);
            DumpDive("start");
            yield return Shot("dive-explore");
            // The full map (M), then back to the HUD corner minimap.
            yield return Flow.Keys(0.15f, Key.M);
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Shot("dive-minimap");
            yield return Flow.Keys(0.15f, Key.M);
            yield return new WaitForSecondsRealtime(0.5f);
            BuildWaypoints();

            float start = Time.realtimeSinceStartup, nextLog = start;
            int lastHp = Flow.Hp(Me());
            while (!Over() && Time.realtimeSinceStartup - start < ExploreBudget && !ExploreDone())
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 20f; Flow.Log($"explore t={Time.realtimeSinceStartup - start:F0} {MeState()} kills={s_kills} hits={s_hitsTaken} skills={s_skillUses} boxes={s_boxes} shots=[{string.Join(",", s_shots)}]"); }
                yield return Watch();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the dive"); continue; }
                var m = Enemy(8f);
                if (m != null) { yield return Fight(m); continue; }
                // The first box (loot and inventory shots), then the trap, then the rest.
                var trap = s_shots.Contains("dive-trap") || !s_shots.Contains("dive-inventory") ? null : NearestTrap(30f);
                if (trap != null) { yield return StepOnTrap(trap); continue; }
                var loot = NearestLoot(20f);
                if (loot != null) { yield return Loot(loot); continue; }
                trap = s_shots.Contains("dive-trap") ? null : NearestTrap(30f);
                if (trap != null) { yield return StepOnTrap(trap); continue; }
                var wp = NextWaypoint();
                if (wp == null) { Flow.Log("explore: no waypoint left"); break; }
                yield return Flow.Go(wp.Value, 2f, 30f, "explore", stop: () => Enemy(7f) != null || NearestLoot(6f) != null || Down());
                s_waypoints.Remove(wp.Value);
            }
            Flow.Log($"explore end {MeState()} kills={s_kills} hits={s_hitsTaken} skills={s_skillUses} boxes={s_boxes}");
            DumpDive("before exit");
            if (!Over()) yield return Escape();
            yield return AfterDive();
        }

        static bool ExploreDone() => new[] { "dive-combat", "dive-hit-damage-numbers", "dive-skill-vfx", "dive-loot-pickup", "dive-inventory", "dive-trap" }.All(s_shots.Contains);

        // Between steps: the high-stress / death's-door screen, a heal when low, dialogs.
        static bool s_bubbleShot;

        static IEnumerator Watch()
        {
            if (Me() == null) yield break;
            if (!s_bubbleShot)
            {
                var pet = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).FirstOrDefault(m => m != null && m.GetType().Name == "PetHudPresenter");
                var bubble = Flow.Get(Flow.Get(pet, "_view"), "Bubble") as GameObject;
                if (bubble && bubble.activeInHierarchy)
                {
                    s_bubbleShot = true;
                    var text = Flow.Get(Flow.Get(pet, "_view"), "Bubble_Text");
                    for (int i = 0; i < 3; i++)
                    {
                        Flow.Log($"pet bubble {i}: text='{Flow.Get(text, "text")}' visible={Flow.Get(text, "maxVisibleCharacters")} {BubbleLetters(text)}");
                        yield return Review.Shot($"a5-pet-bubble-{i}");
                        yield return new WaitForSecondsRealtime(0.3f);
                    }
                }
            }
            // ref dive-stress-high/02: the red thorn vignette of high stress; /01 is death's door (WhileDying).
            if (Stress() >= 75f && !s_shots.Contains("dive-stress-high-02")) yield return Shot("dive-stress-high-02");
            if (HpFrac() < 0.35f && !s_shots.Contains("a5-low-hp")) { s_shots.Add("a5-low-hp"); yield return Review.Shot("a5-low-hp"); }
            if (HpFrac() < 0.4f && !s_healed) yield return Heal();
        }

        // A consumable from the quick slots (keys 1-5, PlayerFunc UseItem), as a player at low HP does.
        static IEnumerator Heal()
        {
            s_healed = true;
            int before = Flow.Hp(Me());
            foreach (var key in new[] { Key.Digit1, Key.Digit2, Key.Digit3 })
            {
                yield return Flow.Keys(0.15f, key);
                yield return new WaitForSecondsRealtime(2.5f);
                Flow.Log($"heal {key}: hp {before} -> {Flow.Hp(Me())} {MeState()}");
                if (Flow.Hp(Me()) > before) yield break;
            }
        }

        // Fight like a player: close in, aim the mouse at the monster, left-click; every fourth swing a skill (right button,
        // Q, E, R in turn). Shots: the melee exchange, the first hit number, the first skill's effect.
        static IEnumerator Fight(Component m, float reach = 12f)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            int clicks = 0, myHp = Flow.Hp(Me()), lastTargetHp = Flow.Hp(m);
            float lastProgress = start;
            string name = m.name;
            Flow.Log($"fight {name} hp={Flow.Hp(m)} state={MonsterState(m)} at {m.transform.position} {MeState()}");
            while (Flow.Alive(m) && Time.realtimeSinceStartup - start < 45f && !Down())
            {
                if (Flow.Hp(m) != lastTargetHp) { lastTargetHp = Flow.Hp(m); lastProgress = Time.realtimeSinceStartup; }
                if (Time.realtimeSinceStartup - lastProgress > 10f)
                {
                    // Ten seconds of swings without a hit landing: leave it, as a player walks off from a stuck fight.
                    s_ignored.Add(m.GetInstanceID());
                    Flow.Log($"fight {name}: no hit lands in 10 s, leaving it; target state={MonsterState(m)} hp={Flow.Hp(m)} {MeState()} {InputState()}");
                    break;
                }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in a fight"); continue; }
                if (Flow.TopPopup() != null) { Flow.Log($"fight: popup {Flow.TopPopup()} open, Esc"); yield return Flow.Keys(0.2f, Key.Escape); }
                var me = Me();
                float d = Flow.Flat(m.transform.position - me.transform.position).magnitude;
                if (d > reach) { Flow.Log($"fight {name}: out of reach d={d:F1}"); break; }
                if (d > 1.8f) { yield return Flow.WalkTo(() => Flow.Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, $"fight {name} approach"); continue; }
                Flow.Hold();
                Flow.Aim(m.transform.position + Vector3.up * 0.5f);
                int targetHp = Flow.Hp(m);
                clicks++;
                if (clicks % 4 == 0)
                {
                    var skill = (s_skillUses++) % 4;
                    Flow.Log($"fight {name}: skill {(skill == 0 ? "RMB" : skill == 1 ? "Q" : skill == 2 ? "E" : "R")} state={State()}");
                    if (skill == 0) yield return Flow.MouseClick(Aimed(m), MouseButton.Right);
                    else yield return Flow.Keys(0.12f, skill == 1 ? Key.Q : skill == 2 ? Key.E : Key.R);
                    yield return new WaitForSecondsRealtime(0.25f);
                    if (State() == "Skill") yield return Shot("dive-skill-vfx");
                }
                else yield return Flow.MouseClick(Aimed(m));
                // The hit number pops where the blow lands, for about half a second.
                float hitWait = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - hitWait < 0.6f && Flow.Hp(m) == targetHp && Flow.Alive(m)) yield return null;
                if (Flow.Hp(m) < targetHp && Flow.Hp(m) > 0) yield return Shot("dive-hit-damage-numbers");
                if (clicks >= 3) yield return Shot("dive-combat");
                int hpNow = Flow.Hp(Me());
                if (hpNow < myHp) { s_hitsTaken++; Flow.Log($"took damage {myHp} -> {hpNow} {MeState()}"); }
                myHp = hpNow;
                yield return Watch();
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 5f; Flow.Log($"fight {name} clicks={clicks} target hp={Flow.Hp(m)} state={MonsterState(m)} d={d:F1} {MeState()}"); }
                yield return new WaitForSecondsRealtime(0.2f);
            }
            if (!Flow.Alive(m)) s_kills++;
            Flow.Log($"fight {name} end alive={Flow.Alive(m)} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s {MeState()}");
        }

        static readonly HashSet<int> s_ignored = new HashSet<int>();

        static string MonsterState(Component m) => Flow.Get(m, "UnitModel") is Component model ? Flow.Get(model, "State")?.ToString() : Flow.Get(m.GetComponent("MonsterModel"), "State")?.ToString() ?? "?";

        // A monster to fight: alive, not left behind after a stuck fight.
        static Component Enemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => !s_ignored.Contains(m.GetInstanceID()) && MonsterState(m) != "Dead" && MonsterState(m) != "Dying")
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        // Where a click stops: the PlayerFunc map, its block counts, the skill in use.
        static string InputState()
        {
            var im = Flow.Get(Flow.Find("PlayerInputController"), "_inputManager");
            var blocks = Flow.Get(im, "_typeBlockCounts") as IDictionary;
            string b = "";
            if (blocks != null) foreach (DictionaryEntry e in blocks) b += $"{e.Key}:{e.Value},";
            var attack = Flow.Get(Flow.Get(im, "PlayerFuncActions"), "SkillBasicAttack") as InputAction;
            return $"blocks={b} attack={attack?.enabled}/{attack?.phase} mouse={Mouse.current?.position.ReadValue()} using={Flow.Get(Me(), "UsingSkillId")} top={Flow.TopPopup()}";
        }

        // Where the bubble's letters are drawn: the TMP text's first characters, visibility and position.
        static string BubbleLetters(object text)
        {
            var info = Flow.Get(text, "textInfo");
            var chars = Flow.Get(info, "characterInfo") as Array;
            int count = Flow.Get(info, "characterCount") is int c ? c : 0;
            var sb = new System.Text.StringBuilder($"chars={count}");
            for (int i = 0; i < Math.Min(3, count) && chars != null; i++)
            {
                var ci = chars.GetValue(i);
                sb.Append($" [{Flow.Get(ci, "character")} vis={Flow.Get(ci, "isVisible")} bl={Flow.Get(ci, "bottomLeft")}]");
            }
            return sb.ToString();
        }

        static Vector2 Aimed(Component m)
        {
            var cam = Camera.main;
            return cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        // ---- loot ----

        static bool IsLoot(MonoBehaviour m)
        {
            if (m == null || !m.isActiveAndEnabled) return false;
            string n = m.GetType().Name;
            if (n == "DropGoods") return true;
            if (n == "RewardBox") return true;
            if (n == "MonsterBody")
            {
                var model = Flow.Get(m, "_monsterModel");
                return Flow.Get(model, "State")?.ToString() == "Dead";
            }
            return false;
        }

        static Component NearestLoot(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => IsLoot(m) && !s_looted.Contains(m.GetInstanceID()))
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => (Component)x.m).FirstOrDefault();
        }

        // Walk up, aim, wait for the F prompt (shot), hold F; boxes and corpses open the looting panel, take everything.
        static IEnumerator Loot(Component target)
        {
            s_looted.Add(target.GetInstanceID());
            string kind = target.GetType().Name;
            string name = target.name;
            Flow.Log($"loot {kind} {name} at {target.transform.position}");
            yield return Flow.WalkTo(() => target ? target.transform.position : (Vector3?)null, 0.9f, 15f, $"loot {name}");
            if (!target) yield break;
            Flow.Aim(target.transform.position);
            yield return Flow.WaitUntil(() => Flow.Focused(target), 3f, $"focus {name}");
            if (!Flow.LastWaitOk) { Flow.Log($"loot {name}: no focus ({Flow.Focus()})"); yield break; }
            yield return new WaitForSecondsRealtime(0.4f);
            yield return Shot("dive-loot-pickup");
            if (kind != "DropGoods") yield return Shot("a5-loot-" + kind);
            float hold = Flow.Get(target, "HoldingTime") is float h ? h + 0.3f : 0.8f;
            yield return Flow.Keys(Mathf.Clamp(hold, 0.3f, 3f), Key.F);
            if (kind == "DropGoods")
            {
                yield return new WaitForSecondsRealtime(0.8f);
                Flow.Log($"picked up {name}: inventory {InventoryIds()}");
                yield break;
            }
            yield return Flow.WaitUntil(() => Flow.Find("LootingInventoryPanelPresenter") != null, 6f, $"{name} looting panel");
            var panel = Flow.Find("LootingInventoryPanelPresenter");
            if (panel == null) { Flow.Log($"loot {name}: no looting panel, focus={Flow.Focus()} state={State()}"); yield break; }
            s_boxes++;
            // The search reveals slots one by one (Unrevealed eye -> Revealing animation -> item): a short series.
            if (s_boxes == 1)
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSecondsRealtime(i == 0 ? 0.2f : 0.5f);
                    Flow.Log($"looting series {i}: {LootLayouts(panel)}");
                    yield return Review.Shot($"a5-looting-{i}");
                }
            else yield return new WaitForSecondsRealtime(1.5f);
            Flow.Log($"looting: {LootLayouts(panel)}");
            yield return Review.Shot("a5-looting");
            // The search reveals the box's slots one by one (GoodsSlot.IsRevealed); take each as it shows, as a player
            // clicking the revealed items does, until every filled slot is taken or the search stalls.
            var slots = Flow.Get(panel, "_goodsSlotModels") as IList;
            var takenSlots = new HashSet<object>();
            int taken = 0;
            float searchStart = Time.realtimeSinceStartup;
            while (slots != null && Time.realtimeSinceStartup - searchStart < 15f && Flow.Find("LootingInventoryPanelPresenter") != null && !Down())
            {
                bool pending = false;
                foreach (var slot in slots.Cast<object>().ToList())
                {
                    if (takenSlots.Contains(slot)) continue;
                    var gs = Flow.Value(slot, "RxGoodsSlot");
                    if (gs == null || Flow.Get(gs, "IsEmpty") is bool e && e) continue;
                    if (!(Flow.Get(gs, "IsRevealed") is bool r && r)) { pending = true; continue; }
                    Flow.Log($"loot slot {Flow.Get(gs, "Id")} x{Flow.Get(gs, "Count")} after {Time.realtimeSinceStartup - searchStart:F1}s {CanTake(gs)}");
                    Fire(slot, "OnEnter", slot);
                    yield return null;
                    Fire(slot, "OnLeftClick", slot);
                    takenSlots.Add(slot);
                    taken++;
                    yield return new WaitForSecondsRealtime(0.4f);
                }
                if (!pending) break;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            Flow.Log($"loot {name} took {taken} slots, inventory: {InventoryIds()}");
            yield return CloseMenu("looting");
            // The inventory (Tab) once there is something in it.
            if (!s_shots.Contains("dive-inventory") && taken > 0) yield return Inventory();
        }

        // What the client checks before it asks the host to move a looted slot (InventoryManagementPagePresenter
        // .TransferLootingSlotToInventorySlot): the addable count, CanMoveGoods.
        static string CanTake(object goodsSlot)
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            var goods = Flow.Get(goodsSlot, "Goods");
            try
            {
                var addable = inv?.GetType().GetMethod("GetAddableItemCountWithBags")?.Invoke(inv, new[] { Flow.Get(goodsSlot, "Id") });
                var canMove = inv?.GetType().GetMethods().FirstOrDefault(m => m.Name == "CanMoveGoods" && m.GetParameters().Length == 1)?.Invoke(inv, new[] { goods });
                return $"addable={addable} canMove={canMove} category={Flow.Get(goodsSlot, "Category")} inventory={InventoryIds()}";
            }
            catch (Exception e) { return $"check threw {e.InnerException?.GetType().Name}: {e.InnerException?.Message}"; }
        }

        // Each looting slot's layout (Normal/Unrevealed/Revealing/Locked) and its goods' revealed flag.
        static string LootLayouts(Component panel)
        {
            var slots = Flow.Get(panel, "_goodsSlotModels") as IList;
            if (slots == null) return "-";
            return string.Join(" ", slots.Cast<object>().Select((m, i) =>
            {
                var gs = Flow.Value(m, "RxGoodsSlot");
                return $"{i}:{Flow.Value(m, "RxLayout")}/{Flow.Get(gs, "SlotIndex")}/{(Flow.Get(gs, "IsEmpty") is bool e && e ? "empty" : Flow.Get(gs, "Id"))}/rev={Flow.Get(gs, "IsRevealed")}";
            }));
        }

        static IEnumerator Inventory()
        {
            yield return Flow.Keys(0.15f, Key.Tab);
            yield return Flow.WaitUntil(() => Flow.Find("MenuPopupPresenter") != null, 4f, "inventory menu");
            if (Flow.Find("MenuPopupPresenter") == null) { Flow.Log($"inventory: Tab opened nothing, top={Flow.TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(1.2f);
            yield return Shot("dive-inventory");
            yield return CloseMenu("inventory");
        }

        // Esc (Ui/Escape), then Tab (Ui/CloseMenu), as A4 closes the looting menu.
        static IEnumerator CloseMenu(string what)
        {
            foreach (var key in new[] { Key.Escape, Key.Tab })
            {
                if (!MenuOpen()) break;
                yield return Flow.Keys(0.2f, key);
                yield return Flow.WaitUntil(() => !MenuOpen(), 2f, $"{what} closed by {key}");
            }
            if (MenuOpen())
            {
                Flow.Log($"BUG {what}: Esc and Tab left the menu open top={Flow.TopPopup()} {Flow.PopupState()} state={State()}");
                var menu = Flow.Find("MenuPopupPresenter");
                menu?.GetType().GetMethod("Close", new[] { typeof(object) })?.Invoke(menu, new object[] { null });
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        static bool MenuOpen() => Flow.Find("MenuPopupPresenter") != null || State() == "Interact";

        static void Fire(object owner, string subject, object arg)
        {
            var s = Flow.Get(owner, subject);
            var m = s?.GetType().GetMethod("OnNext");
            if (m == null) { Flow.Log($"fire {subject}: no subject"); return; }
            m.Invoke(s, new[] { arg });
        }

        static string InventoryIds()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            var slots = Flow.Get(inv, "Slots") as IEnumerable;
            var sb = new System.Text.StringBuilder();
            if (slots != null)
                foreach (var kv in slots)
                    if (Flow.Get(kv, "Value") is object gs && !(Flow.Get(gs, "IsEmpty") is bool e && e)) sb.Append($"{Flow.Get(gs, "Id")}x{Flow.Get(gs, "Count")},");
            return sb.Length == 0 ? "(empty)" : sb.ToString();
        }

        // ---- traps ----

        static Component NearestTrap(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().BaseType?.Name == "TrapObject") && !s_trapsTried.Contains(m.GetInstanceID()))
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => (Component)x.m).FirstOrDefault();
        }

        // Walk onto the trap and stand there until it goes off (TriggerTrap fires on a unit inside its trigger collider).
        static IEnumerator StepOnTrap(Component trap)
        {
            s_trapsTried.Add(trap.GetInstanceID());
            string name = trap.name;
            var pos = trap.transform.position;
            int hp = Flow.Hp(Me());
            Flow.Log($"trap {trap.GetType().Name} {name} at {pos} activated={Flow.Value(trap, "_isActivated")}");
            // A one-shot trap goes off as the diver steps in and despawns; a hit or the trap's activation ends the walk.
            bool WentOff() => !trap || Flow.Value(trap, "_isActivated") is bool b && b || Flow.Hp(Me()) < hp;
            yield return Flow.Go(pos, 0.4f, 30f, $"trap {name}", stop: () => WentOff() || Enemy(5f) != null || Down());
            float t = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t < 6f && !WentOff() && !Down()) yield return new WaitForSecondsRealtime(0.1f);
            if (WentOff())
            {
                yield return new WaitForSecondsRealtime(0.2f);
                Flow.Log($"trap {name} went off: hp {hp} -> {Flow.Hp(Me())} trap={(trap ? "still there" : "gone")} {MeState()}");
                yield return Shot("dive-trap");
            }
            else Flow.Log($"trap {name}: nothing happened in 6 s, {MeState()} trap at {pos}");
            // Off the trap again.
            yield return Flow.Keys(0.6f, Key.S);
        }

        // ---- exploring ----

        // NavMesh points 10 m apart across the stage, the far corners a player would walk to.
        static void BuildWaypoints()
        {
            var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
            if (tri.vertices.Length == 0) return;
            var b = new Bounds(tri.vertices[0], Vector3.zero);
            foreach (var v in tri.vertices) b.Encapsulate(v);
            for (float x = b.min.x + 5f; x < b.max.x; x += 10f)
                for (float z = b.min.z + 5f; z < b.max.z; z += 10f)
                    if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, b.center.y, z), out var hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
                        s_waypoints.Add(hit.position);
            Flow.Log($"waypoints {s_waypoints.Count} over {b.min}..{b.max}");
        }

        static Vector3? NextWaypoint()
        {
            var me = Me();
            if (me == null || s_waypoints.Count == 0) return null;
            var p = me.transform.position;
            return s_waypoints.OrderBy(w => Flow.Flat(w - p).sqrMagnitude).First();
        }

        // ---- escape ----

        static Component NearestExit()
        {
            var me = Me();
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "WaveExit" || m.GetType().Name == "SafeExit") && Flow.Get(m, "ExitState")?.ToString() != "Finished")
                .OrderBy(m => me ? Flow.Flat(m.transform.position - me.transform.position).sqrMagnitude : 0f).FirstOrDefault();
        }

        static string ExitState(Component e) => e == null ? "-" : $"{e.GetType().Name} {Flow.Get(e, "ExitState")} remain={Flow.Get(e, "StateRemainTime")}";

        // Find the terminal, hold F to activate it (the booth is summoned, a wave comes), hold out, stand in its field.
        static IEnumerator Escape()
        {
            var exit = NearestExit();
            if (exit == null) { Flow.Log("escape: no exit in the stage"); yield break; }
            Flow.Log($"escape: {exit.name} at {exit.transform.position} {ExitState(exit)}");
            float start = Time.realtimeSinceStartup;
            // Run for the terminal (Shift), only stopping for a monster in the way.
            while (!Over() && Time.realtimeSinceStartup - start < 300f && Flow.Get(exit, "ExitState")?.ToString() == "Deactivated")
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                yield return Watch();
                var m = Enemy(2.5f);
                if (m != null) { yield return Fight(m, 8f); continue; }
                yield return Flow.Go(exit.transform.position, 1.2f, 40f, "exit", run: true, stop: () => Enemy(2.5f) != null || Down());
                if (Me() == null || Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2f) continue;
                Flow.Aim(exit.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus exit");
                yield return Flow.Keys(2.6f, Key.F);  // ExitActivateInteractionTime 2 s
                yield return new WaitForSecondsRealtime(0.5f);
                Flow.Log($"escape: after F {ExitState(exit)} focus={Flow.Focus()} {MeState()}");
            }
            if (Over() || Flow.Get(exit, "ExitState")?.ToString() == "Deactivated") { Flow.Log($"escape: terminal not activated {ExitState(exit)} {MeState()}"); yield break; }
            yield return new WaitForSecondsRealtime(2f);
            yield return Shot("dive-exit");
            // Activating (15 s, a wave), then Activated (30 s): stay inside the 5 m field until the escape buff takes the diver.
            float hold = Time.realtimeSinceStartup, nextLog = hold;
            while (!Over() && Time.realtimeSinceStartup - hold < 150f)
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 5f; Flow.Log($"escape: {ExitState(exit)} {MeState()}"); }
                yield return Watch();
                var m = Enemy(3.5f);
                if (m != null) { yield return Fight(m, 5f); continue; }
                if (exit && Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2.5f)
                    yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 1.5f, 3f, "back to the exit");
                yield return new WaitForSecondsRealtime(0.3f);
            }
            Flow.Log($"escape end {ExitState(exit)} {MeState()} stage={Flow.StageState()}");
        }

        // The banner (escape or Extraction Failed) and the way back to the lounge.
        static IEnumerator AfterDive()
        {
            Flow.Log($"after dive {MeState()} stage={Flow.StageState()} scene={Flow.ActiveScene}");
            // A diver at 0 HP lies dying before death; a fight goes on around it.
            yield return Flow.WaitUntil(() => Flow.Find("EscapeFailPopupView") != null || Flow.Find("CampaignFailPopupView") != null
                || Flow.Find("EscapeSuccessPopupView") != null || Flow.ActiveScene != "StageScene", 180f, "dive end banner");
            Flow.Log($"dive end: top={Flow.TopPopup()} {Flow.PopupState()} {MeState()}");
            if (Flow.Find("EscapeFailPopupView") != null || Flow.Find("CampaignFailPopupView") != null)
            {
                yield return new WaitForSecondsRealtime(1f);
                yield return Shot("dive-death");
            }
            else if (Flow.Find("EscapeSuccessPopupView") != null)
            {
                yield return new WaitForSecondsRealtime(1f);
                yield return Review.Shot("a5-escape-success");
            }
        }

        // Dive Results over the lounge, Confirm.
        static IEnumerator Results()
        {
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 240f, "back in the lounge");
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 120f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            if (view == null) { Flow.Log($"no result popup: top={Flow.TopPopup()} {Flow.LoadState()}"); yield break; }
            // The sections fill in one after another; Skip shows them all.
            yield return new WaitForSecondsRealtime(6f);
            yield return Shot("result-screen");
            for (int i = 0; i < 6 && Flow.Find("StageResultPopupView") != null; i++)
            {
                if (Flow.Ready(Flow.Button(view, "Confirm_Button"))) yield return Flow.Click(view, "Confirm_Button");
                else if (Flow.Ready(Flow.Button(view, "SkipButton"))) yield return Flow.Click(view, "SkipButton");
                yield return new WaitForSecondsRealtime(2f);
            }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") == null, 10f, "result closed");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 60f, "lounge Playing after the dive");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge after the dive");
            yield return new WaitForSecondsRealtime(2f);
            yield return Review.Shot("a5-lounge-after");
            Flow.Log($"lounge after the dive {Flow.LoadState()} top={Flow.TopPopup()} inventory={InventoryIds()}");
        }

        // ---- diagnostics ----

        static void DumpDive(string when)
        {
            var me = Me();
            var counts = new Dictionary<string, int>();
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null) continue;
                string n = m.GetType().Name, b = m.GetType().BaseType?.Name;
                if (n == "MonsterController" || n == "RewardBox" || n == "MonsterBody" || n == "DropGoods" || n == "WaveExit" || n == "SafeExit"
                    || n == "Entrance" || b == "TrapObject" || n == "InteractiveTrigger" || n == "CollisionTrigger")
                {
                    counts[n] = counts.TryGetValue(n, out var c) ? c + 1 : 1;
                    if (n != "MonsterBody")
                        Flow.Log($"dive {when}: {n} {m.name} pos={m.transform.position} d={(me ? Flow.Flat(m.transform.position - me.transform.position).magnitude : -1):F1}" +
                                 (n == "MonsterController" ? $" hp={Flow.Hp(m)}" : "") + (n == "WaveExit" || n == "SafeExit" ? $" {ExitState(m)}" : ""));
                }
            }
            Flow.Log($"dive {when}: {string.Join(" ", counts.Select(kv => $"{kv.Key}={kv.Value}"))} {MeState()} inventory={InventoryIds()} {Flow.LoadState()}");
        }
    }
}

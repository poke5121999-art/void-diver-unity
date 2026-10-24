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
    // A6: the story boss of the demo, reached and fought like a player. Hachishakusama is the boss of story campaign 1102
    // "The Sealed One" (TCampaign 1102: IsBossCampaign, UnlockConditions UserLevel:2|CampaignCleared:1101, 4x4 Country
    // map with fixed sectors 10202 at (0,2) and 10201 at (0,3)). Lua Campaign/1102: Elara briefs (TalkToNPC); in the
    // stage the quest marker points at the magic-circle portal 1101 (ZoneSpawn 1101, sector 10202); F on it teleports to
    // portal 1102 (sector 10201), the first use plays IngameCutScene_BossFeature_Slender at zone 11021 and spawns monster
    // 300014 "Wandering Hachishakusama", whose death brings 300015 "Curious" and then 300016 "Enraged" (TMonster phases of
    // base 300014). The last phase down, the stranded Alex (NPC 700404) is rescued with F and Lua drops dive booth 1007.
    // Ex-Chief Donovan (300004, the Apr refs) is still in the demo, as the boss of the next campaign, 1103.
    //
    // Assumptions (keep the run reproducible and short):
    // - Saves: the post-tutorial profile (tools/fixtures/saves-post-tutorial, see A3/A5). Campaign 1102 needs 1101 cleared
    //   and user level 2. Instead of playing 1101 (A5 does that), the test uses the cheat menu's own calls in the lounge:
    //   PlayerManager.SetClearedCampaign(1101) (what SROptions.ClearAllCampaigns calls for each campaign, here for 1101
    //   only) and ReqCheatLevelUp (SROptions.LevelUp). So 1101's clear rewards and its lounge follow-ups are not there.
    // - Difficulty Hard, as in the Jul clip (one click on the campaign detail's next-difficulty arrow).
    // - The level-1 diver with the starter weapon cannot outlast a Hard boss. While fighting the boss the test refills HP
    //   with the cheat menu's Recover (ReqSetCharacterHp to HpMax) when it falls under 35%, and if a phase still stands
    //   after PhaseBudget seconds of fighting it sets the monsters' HP with the cheat menu's monster HP (ReqCheatApplyMonsterHp)
    //   so the next phase comes. Every use is logged ("cheat ...").
    // Shots: boss-intro (boss bar just after the spawn), boss-fight (phase 1 by the blood circle), boss-fight-02 (phase 2,
    // "Curious", red spikes); extra shots are prefixed a6-.
    public class A6_BossFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const int CampaignId = 1102;
        const int PortalA = 1101;
        const int NpcAlex = 700404;
        static readonly int[] BossPhases = { 300014, 300015, 300016 };
        const float PhaseBudget = 150f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A6_BossFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A6";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
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
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static readonly string[] Keys = { "boss-intro", "boss-fight", "boss-fight-02" };
        static readonly HashSet<string> s_shots = new HashSet<string>();

        static IEnumerator Shot(string key)
        {
            if (!s_shots.Add(key)) yield break;
            Flow.Log($"shot {key} {MeState()} boss={BossState()} hud={BossHud()}");
            yield return Review.Shot(key);
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator BossCampaign1102()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6";
            s_shots.Clear();
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();

            // Lounge quest 80200 first: Elara's menu opens only after it (A5).
            yield return Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Talk(id, $"{who} 80200");
            yield return Talk(700000, "Elara 80200 finish");

            yield return CheatPast1101();
            yield return StartCampaign();
            Assert.AreEqual(CampaignId.ToString(), CampaignNow(), "campaign 1102 did not start");
            yield return Briefing();
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");

            yield return new WaitForSecondsRealtime(3f);
            DumpStageObjects("start");
            yield return Review.Shot("a6-stage-start");
            yield return ToPortal();
            yield return Encounter();
            Assert.IsTrue(s_shots.Contains("boss-intro"), "the boss never showed up");
            yield return FightBoss();
            yield return Rescue();

            var missing = Keys.Where(k => !s_shots.Contains(k)).ToList();
            Flow.Log($"end scene={Flow.ActiveScene} {MeState()} shots=[{string.Join(",", s_shots)}] missing=[{string.Join(",", missing)}]");
            Assert.IsEmpty(missing, "boss shots missing");
        }

        // ---- lounge ----

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

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        // A cheat request to the host, as SROptions sends it (GameChannel.SendToHost).
        static void SendCheat(string type, params (string prop, object value)[] props)
        {
            var request = Activator.CreateInstance(GameType(type));
            foreach (var (p, v) in props) request.GetType().GetProperty(p).SetValue(request, v);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });
            Flow.Log($"cheat {type} {string.Join(" ", props.Select(x => $"{x.prop}={x.value}"))}");
        }

        static object PlayerProfile() => Flow.Get(Flow.Get(Flow.Get(Flow.Find("PlayerManager"), "_playerContext"), "Profile"), "Level");

        // 1101 cleared and user level 2: the conditions of 1102 (see the header).
        static IEnumerator CheatPast1101()
        {
            var pm = Flow.Find("PlayerManager");
            bool cleared = pm != null && pm.GetType().GetMethod("SetClearedCampaign")?.Invoke(pm, new object[] { 1101 }) is bool b && b;
            Flow.Log($"cheat SetClearedCampaign(1101) -> {cleared} isCleared={pm?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pm, new object[] { 1101 })}");
            SendCheat("ReqCheatLevelUp");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Log($"after cheats level={PlayerProfile()} myLevel={Flow.Get(Flow.Get(Flow.Find("GameManager"), "MyPlayer"), "Level")}");
        }

        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        // Elara, Campaign, the 1102 slot, difficulty Hard, Start Campaign.
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
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, "campaign slot 1102");
            slotButton.onClick.Invoke();

            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == CampaignId, 6f, "campaign 1102 detail");
            if (detail == null) yield break;
            yield return new WaitForSecondsRealtime(1.5f);
            var view = Flow.Get(detail, "_view");
            var ctx = Flow.Get(Flow.Find("GameManager"), "_gameContext");
            for (int i = 0; i < 3 && Flow.Value(ctx, "RxDifficulty")?.ToString() != "Hard"; i++)
            {
                var next = Flow.Get(view, "Info_Difficulty_NextButton") as UnityEngine.UI.Button;
                var pre = Flow.Get(view, "Info_Difficulty_PreButton") as UnityEngine.UI.Button;
                bool up = Flow.Value(ctx, "RxDifficulty")?.ToString() is "Easy" or "Normal";
                var b = up ? next : pre;
                yield return Flow.WaitUntil(() => Flow.Ready(b), 3f, $"difficulty {(up ? "next" : "previous")}");
                if (!Flow.LastWaitOk) break;
                b.onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.6f);
            }
            Flow.Log($"difficulty={Flow.Value(ctx, "RxDifficulty")} text='{Flow.Get(Flow.Get(view, "Info_Difficulty_Text"), "text")}'");
            yield return Review.Shot("a6-campaign-1102");
            var start = Flow.Get(view, "Info_StartCampaignButton") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == CampaignId.ToString(), 10f, "campaign started");
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            Flow.Log($"after start campaign={CampaignNow()} difficulty={Flow.Value(ctx, "RxDifficulty")} top={Flow.TopPopup()} {Flow.PopupState()}");
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
                Flow.Log($"booth attempt {attempt}: state={State()} check={BoothCheck()} focus={Flow.Focus()}");
            }
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.Log($"stage {Flow.LoadState()}");
        }

        // ---- the diver ----

        static Component Me() => Flow.MyCharacter();
        static string State() => Flow.Get(Me(), "State")?.ToString() ?? "-";
        static float HpMax() => Flow.Get(Me(), "HpMax") is float f ? f : 1f;
        static float HpFrac() => Flow.Hp(Me()) / Mathf.Max(1f, HpMax());
        static bool Over() => State() == "Dead" || State() == "Escaped" || Flow.ActiveScene != "StageScene" || Flow.StageState() == "Ended";

        static string MeState()
        {
            var me = Me();
            return me == null ? "me=-" : $"hp={Flow.Hp(me)}/{HpMax():F0} stress={Flow.Get(me, "Stress")} state={State()} pos={me.transform.position}";
        }

        // Recover (the cheat menu's HP refill) when the boss has the diver low; see the header.
        static void KeepAlive()
        {
            if (Me() == null || Over() || State() == "Dying") return;
            if (HpFrac() < 0.35f) SendCheat("ReqSetCharacterHp", ("Hp", HpMax()));
        }

        // ---- monsters ----

        static object Model(Component m) => Flow.Get(m, "UnitModel");
        static string MonsterState(Component m) => Flow.Get(Model(m), "State")?.ToString() ?? "?";
        static int MonsterId(Component m) => Flow.Get(Model(m), "TableId") is int id ? id : 0;
        static bool IsBossPhase(Component m) => m != null && BossPhases.Contains(MonsterId(m));

        static Component Boss() => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .FirstOrDefault(m => m != null && m.GetType().Name == "MonsterController" && m.gameObject.activeInHierarchy && IsBossPhase(m));

        static string BossState()
        {
            var b = Boss();
            if (b == null) return "-";
            var me = Me();
            return $"{MonsterId(b)} hp={Flow.Hp(b)} state={MonsterState(b)} skill={Flow.Get(Model(b), "UsingSkillId")} pos={b.transform.position} d={(me ? Flow.Flat(b.transform.position - me.transform.position).magnitude : -1f):F1}";
        }

        // The boss bar: InGameBossStatusPanelPresenter's cards and their name text.
        static string BossHud()
        {
            var panel = Flow.Find("InGameBossStatusPanelPresenter");
            var bosses = Flow.Get(panel, "_bosses") as IList;
            var names = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "InGameBossStatusCardView")
                .Select(v => $"{Flow.Get(Flow.Get(v, "TextBossName"), "text")}");
            return $"panel={(panel ? "on" : "off")} bosses={bosses?.Count.ToString() ?? "-"} cards=[{string.Join(",", names)}]";
        }

        static Component Enemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => MonsterState(m) != "Dead" && MonsterState(m) != "Dying")
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        static Vector2 Aimed(Component m)
        {
            var cam = Camera.main;
            return cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        static int s_skillUses;

        // One exchange: close in, aim, left click; every fourth swing a skill (right button, Q, E, R in turn) (A5).
        static IEnumerator Swing(Component m, int clicks)
        {
            var me = Me();
            float d = Flow.Flat(m.transform.position - me.transform.position).magnitude;
            if (d > 1.8f) { yield return Flow.WalkTo(() => Flow.Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, $"approach {m.name}"); yield break; }
            Flow.Hold();
            Flow.Aim(m.transform.position + Vector3.up * 0.5f);
            if (clicks % 4 == 0)
            {
                var skill = (s_skillUses++) % 4;
                if (skill == 0) yield return Flow.MouseClick(Aimed(m), MouseButton.Right);
                else yield return Flow.Keys(0.12f, skill == 1 ? Key.Q : skill == 2 ? Key.E : Key.R);
            }
            else yield return Flow.MouseClick(Aimed(m));
            yield return new WaitForSecondsRealtime(0.35f);
        }

        // A small monster in the way: fight it for up to 30 s.
        static IEnumerator Fight(Component m)
        {
            float start = Time.realtimeSinceStartup;
            int clicks = 0;
            string name = m.name;
            Flow.Log($"fight {name} {MonsterId(m)} hp={Flow.Hp(m)} {MeState()}");
            while (Flow.Alive(m) && Time.realtimeSinceStartup - start < 30f && !Over() && State() != "Dying")
            {
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in a fight"); continue; }
                if (Flow.Flat(m.transform.position - Me().transform.position).magnitude > 10f) break;
                yield return Swing(m, ++clicks);
            }
            Flow.Log($"fight {name} end alive={Flow.Alive(m)} clicks={clicks} {MeState()}");
        }

        // ---- the stage ----

        static Component Trigger(int id) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .FirstOrDefault(m => m != null && m.GetType().Name == "InteractiveTrigger" && Flow.Get(m, "TriggerId") is int t && t == id);

        static void DumpStageObjects(string when)
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null) continue;
                string n = m.GetType().Name;
                if (n == "InteractiveTrigger") Flow.Log($"{when}: trigger {Flow.Get(m, "TriggerId")} {m.name} pos={m.transform.position} can={Flow.Get(m, "CanTriggered")} active={m.isActiveAndEnabled}");
                else if (n == "MiniMapMarker" || n.Contains("QuestMarker")) Flow.Log($"{when}: {n} {m.name} pos={m.transform.position}");
                else if (n == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject") Flow.Log($"{when}: npc {Flow.NpcId(m)} pos={m.transform.position} state={Flow.Value(m, "RxNpcState") ?? Flow.Get(m, "NpcState")}");
            }
            Flow.Log($"{when}: {MeState()} monsters={Flow.Monsters().Count()} boss={BossState()} {Flow.LoadState()}");
        }

        // The quest marker's goal: portal 1101 in sector 10202. Walk there, fighting what blocks the way, and hold F on it.
        static IEnumerator ToPortal()
        {
            var portal = Trigger(PortalA);
            if (portal == null) { Flow.Log("no portal 1101 in the stage"); yield break; }
            Flow.Log($"portal 1101 at {portal.transform.position} can={Flow.Get(portal, "CanTriggered")} {MeState()}");
            float start = Time.realtimeSinceStartup;
            while (!Over() && Time.realtimeSinceStartup - start < 360f && Boss() == null)
            {
                if (State() == "Dying") { yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 60f, "out of Dying"); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog on the way"); continue; }
                if (HpFrac() < 0.3f) KeepAlive();
                var m = Enemy(3f);
                if (m != null) { yield return Fight(m); continue; }
                yield return Flow.Go(portal.transform.position, 1.2f, 60f, "portal 1101", run: true, stop: () => Enemy(3f) != null || State() == "Dying");
                if (Me() == null || Flow.Flat(portal.transform.position - Me().transform.position).magnitude > 2f) continue;
                yield return Review.Shot("a6-portal");
                yield return Flow.InteractObject(portal, 0.8f, "portal 1101");
                yield return Flow.WaitUntil(() => Flow.Flat(portal.transform.position - Me().transform.position).magnitude > 5f || Boss() != null, 6f, "teleported");
                Flow.Log($"after portal: {MeState()} boss={BossState()}");
                if (Flow.LastWaitOk) break;
            }
        }

        // The first use: radio "Wait, something's coming!", the cutscene, then the boss and its bar.
        static IEnumerator Encounter()
        {
            float t0 = Time.realtimeSinceStartup;
            bool cutsceneShot = false;
            while (Time.realtimeSinceStartup - t0 < 60f && !Over())
            {
                if (!cutsceneShot && Time.realtimeSinceStartup - t0 > 2.5f && Boss() == null)
                {
                    cutsceneShot = true;
                    Flow.Log($"cutscene? {MeState()} top={Flow.TopPopup()}");
                    yield return Review.Shot("a6-cutscene");
                }
                if (Boss() != null && BossHud().Contains("bosses=1")) break;
                yield return new WaitForSecondsRealtime(0.25f);
            }
            Flow.Log($"encounter after {Time.realtimeSinceStartup - t0:F1}s boss={BossState()} hud={BossHud()} {MeState()}");
            if (Boss() == null) { DumpStageObjects("no boss"); yield break; }
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Shot("boss-intro");
        }

        static IEnumerator FightBoss()
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            int phase = -1, clicks = 0;
            float phaseStart = start;
            bool cheated = false;
            while (!Over() && Time.realtimeSinceStartup - start < 3 * PhaseBudget + 120f)
            {
                var boss = Boss();
                if (boss == null)
                {
                    // Between phases (the dead body becomes the next phase) or the end.
                    yield return Flow.WaitUntil(() => Boss() != null && Flow.Hp(Boss()) > 0 || Over(), 10f, "next phase");
                    if (Boss() == null) break;
                    continue;
                }
                int id = MonsterId(boss), p = Array.IndexOf(BossPhases, id);
                if (p != phase)
                {
                    Flow.Log($"boss phase {p}: {BossState()} hud={BossHud()} after {Time.realtimeSinceStartup - start:F0}s {MeState()}");
                    phase = p; phaseStart = Time.realtimeSinceStartup; cheated = false;
                    yield return Review.Shot($"a6-phase{p + 1}-start");
                }
                float inPhase = Time.realtimeSinceStartup - phaseStart;
                if (phase == 0 && inPhase > 8f) yield return Shot("boss-fight");
                if (phase == 1)
                {
                    foreach (var at in new[] { 4f, 10f, 18f })
                        if (inPhase > at && !s_shots.Contains($"a6-phase2-{at:F0}s"))
                        {
                            s_shots.Add($"a6-phase2-{at:F0}s");
                            Flow.Log($"phase 2 at {at}s: {BossState()} {MeState()}");
                            yield return Review.Shot($"a6-phase2-{at:F0}s");
                            if (at >= 10f) yield return Shot("boss-fight-02");
                        }
                }
                if (phase == 2 && inPhase > 6f && !s_shots.Contains("a6-phase3")) { s_shots.Add("a6-phase3"); yield return Review.Shot("a6-phase3"); }
                if (!cheated && inPhase > PhaseBudget && Flow.Hp(boss) > 1)
                {
                    cheated = true;
                    Flow.Log($"phase {p + 1} still up after {PhaseBudget}s: {BossState()}");
                    SendCheat("ReqCheatApplyMonsterHp", ("Hp", 1));
                }
                if (State() == "Dying") { Flow.Log($"diver down {MeState()}"); yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 30f, "out of Dying"); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the boss fight"); continue; }
                KeepAlive();
                // Adds the boss summons stand in the way; the boss comes first unless one is in reach and the boss is not.
                var me = Me();
                var target = boss;
                var add = Enemy(1.8f);
                if (add != null && !IsBossPhase(add) && Flow.Flat(boss.transform.position - me.transform.position).magnitude > 3f) target = add;
                yield return Swing(target, ++clicks);
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"boss fight t={Time.realtimeSinceStartup - start:F0} clicks={clicks} boss={BossState()} {MeState()}"); }
            }
            Flow.Log($"boss fight end after {Time.realtimeSinceStartup - start:F0}s boss={BossState()} hud={BossHud()} {MeState()}");
        }

        // After the last phase: Lua puts Alex in QuestTalkable; F on Alex, then dive booth 1007 drops; take it.
        static IEnumerator Rescue()
        {
            if (Over()) yield break;
            yield return new WaitForSecondsRealtime(4f);
            var alex = Flow.FindNpc(NpcAlex);
            Flow.Log($"rescue: alex={(alex ? alex.transform.position.ToString() : "-")} state={(alex ? Flow.Value(alex, "RxNpcState") : null)} {MeState()}");
            if (alex == null) yield break;
            yield return Flow.InteractNpc(alex, "Alex");
            yield return new WaitForSecondsRealtime(9f);
            yield return Review.Shot("a6-rescued");
            DumpStageObjects("rescued");
            var exit = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "SafeExit" || m.GetType().Name == "WaveExit"));
            Flow.Log($"rescue: exit={(exit ? $"{exit.GetType().Name} {exit.transform.position} {Flow.Get(exit, "ExitState")}" : "-")}");
            if (exit == null) yield break;
            yield return Flow.InteractObject(exit, 2.6f, "dive booth 1007");
            yield return Flow.WaitUntil(() => Over() || Flow.Find("EscapeSuccessPopupView") != null, 60f, "escape");
            Flow.Log($"escape: {MeState()} top={Flow.TopPopup()} stage={Flow.StageState()}");
            if (Flow.Find("EscapeSuccessPopupView") != null) yield return Review.Shot("a6-escape-success");
        }
    }
}

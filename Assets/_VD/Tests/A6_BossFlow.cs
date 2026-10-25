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
    // A6: the story bosses of the demo, reached and fought like a player, one test per boss campaign
    // (docs/review/a6/campaigns.tsv lists every campaign): 1102 "The Sealed One" (Hachishakusama), 1103 "Formless Horizon"
    // (Ex-Chief Donovan) and 1105 "Soul Collector" (Eldris). Each Lua Campaign/<id>.lua works the same way: Elara briefs
    // (TalkToNPC); in the stage the quest marker points at a two-way portal pair (TwoWayPortalForStory); its first use
    // plays the boss cutscene and spawns the boss at the campaign's zone; the phases follow on death (TMonster
    // BasePhaseMonsterId). After the boss: 1102 rescues the stranded Alex (NPC 700404), 1103 picks up the painting
    // (item 300011), 1105 plays the finale cutscene; each then drops dive booth 1007 (SafeExit), and the test escapes,
    // confirms the Dive Results and hears Elara's wrap-up (Step_00010 QuestComplete).
    // 1102: portal 1101 (sector 10202) -> 1102 (sector 10201), IngameCutScene_BossFeature_Slender, 300014 Wandering ->
    // 300015 Curious -> 300016 Enraged Hachishakusama. 1103: portals 1111/1112 (police station), IngameCutScene_BossFeature
    // _Bigman, 300004 Donovan; its refs are the Apr clip (older HUD: compare the boss, not the HUD). 1105: the Stone Child
    // Statue 11052 -> 1033, 300005 -> 300009 Eldris; no ref.
    //
    // Assumptions (keep the run reproducible and short):
    // - Saves: the post-tutorial profile (tools/fixtures/saves-post-tutorial, see A3/A5). Story campaign N needs N-1 cleared
    //   and a user level (2/3/5). Instead of playing the earlier campaigns, the test uses the cheat menu's own calls in
    //   the lounge: PlayerManager.SetClearedCampaign(1101..N-1) (what SROptions.ClearAllCampaigns calls for each campaign)
    //   and ReqCheatLevelUp (SROptions.LevelUp). So their clear rewards and lounge follow-ups are not there.
    // - Difficulty Hard, as in the Jul clip (one click on the campaign detail's next-difficulty arrow).
    // - The level-1 diver with the starter weapon cannot outlast Hard (run 2 died to a 2232-HP 200009 on the way, runs 5
    //   and 8 to the boss). In the stage the test runs past monsters to the portal and refills HP with the cheat menu's
    //   Recover (ReqSetCharacterHp to HpMax) whenever it falls under 60% (a Hard grab, 21005013, takes ~240 HP at once).
    //   If a boss phase still stands after PhaseBudget seconds it sets the monsters' HP with the cheat menu's monster HP
    //   (ReqCheatApplyMonsterHp) so the next phase comes. It turns on the cheat menu's invincibility
    //   (ReqCheatApplyCharacterOptions) so the diver lives to the booth: in 1102 once the three review shots are taken,
    //   from 1103 on as the fight starts (Donovan killed the refilled diver in run 11). Every use is logged ("cheat ...").
    // Shots (keys per campaign in Bosses): 1102 boss-intro (the boss bar once the boss targets the diver, after the
    // cutscene), boss-fight (phase 1 by the blood circle), boss-fight-02 (phase 2 "Curious" at its summon 21005015, the red
    // spikes; the test waits up to 300 s of game time for the AI to pick it and holds off below 30% boss HP meanwhile);
    // 1103 boss-intro-02, boss-fight-03, boss-fight-04 (Donovan under 75%); 1105 a6-1105-*. Extra shots: a6-<id>-*.
    public class A6_BossFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const int NpcAlex = 700404;
        const float PhaseBudget = 120f;

        // One story boss campaign (TCampaign, Lua Campaign/<id>.lua): its portal pair (TwoWayPortalForStory), the boss
        // phases (TMonster BasePhaseMonsterId), the user level it needs, and the review keys of its shots.
        class BossCampaign
        {
            public int Id, Level, PortalA, PortalB;
            public int[] Phases;
            public string Intro, Fight, Fight2;   // review keys; Fight2 is taken in the second phase or later in the fight
        }

        static readonly BossCampaign[] Bosses =
        {
            // The Sealed One: Hachishakusama 300014 -> 300015 Curious -> 300016 Enraged; Jul refs.
            new BossCampaign { Id = 1102, Level = 2, PortalA = 1101, PortalB = 1102, Phases = new[] { 300014, 300015, 300016 },
                Intro = "boss-intro", Fight = "boss-fight", Fight2 = "boss-fight-02" },
            // Formless Horizon: Ex-Chief Donovan 300004 (one phase), police station; Apr refs (older HUD).
            new BossCampaign { Id = 1103, Level = 3, PortalA = 1111, PortalB = 1112, Phases = new[] { 300004 },
                Intro = "boss-intro-02", Fight = "boss-fight-03", Fight2 = "boss-fight-04" },
            // Soul Collector: Eldris 300005 -> 300009 Enraged; portal is the Stone Child Statue 11052; no ref.
            new BossCampaign { Id = 1105, Level = 5, PortalA = 11052, PortalB = 1033, Phases = new[] { 300005, 300009 },
                Intro = "a6-1105-intro", Fight = "a6-1105-fight", Fight2 = "a6-1105-fight-02" },
        };

        static BossCampaign C;
        static int CampaignId => C.Id;
        static int[] BossPhases => C.Phases;

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

        static readonly HashSet<string> s_shots = new HashSet<string>();

        static IEnumerator Shot(string key)
        {
            if (!s_shots.Add(key)) yield break;
            Flow.Log($"shot {key} {MeState()} boss={BossState()} hud={BossHud()}");
            yield return Review.Shot(key);
        }

        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1102() { yield return Play(1102); }
        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1103() { yield return Play(1103); }
        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1105() { yield return Play(1105); }

        static IEnumerator Play(int campaignId)
        {
            C = Bosses.First(b => b.Id == campaignId);
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

            yield return CheatPastEarlierCampaigns();
            yield return StartCampaign();
            Assert.AreEqual(CampaignId.ToString(), CampaignNow(), $"campaign {CampaignId} did not start");
            yield return Briefing();
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");

            yield return new WaitForSecondsRealtime(3f);
            DumpStageObjects("start");
            yield return Review.Shot($"a6-{CampaignId}-stage-start");
            yield return ToPortal();
            yield return Encounter();
            Assert.IsTrue(s_shots.Contains(C.Intro), "the boss never showed up");
            yield return FightBoss();
            yield return AfterBoss();

            var missing = new[] { C.Intro, C.Fight, C.Fight2 }.Where(k => !s_shots.Contains(k)).ToList();
            Flow.Log($"end scene={Flow.ActiveScene} {MeState()} shots=[{string.Join(",", s_shots)}] missing=[{string.Join(",", missing)}]");
            Assert.IsEmpty(missing, "boss shots missing");
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the dive did not end back in the lounge (booth escape)");
            var pmEnd = Flow.Find("PlayerManager");
            Assert.IsTrue(pmEnd?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pmEnd, new object[] { CampaignId }) is bool done && done, $"campaign {CampaignId} not cleared after Elara's wrap-up");
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

        static object PlayerProfile() => Flow.Get(Flow.Find("PlayerManager"), "Level");

        // The story campaigns before this one cleared and the user level it needs (see the header).
        static IEnumerator CheatPastEarlierCampaigns()
        {
            var pm = Flow.Find("PlayerManager");
            for (int id = 1101; id < CampaignId; id++)
            {
                bool cleared = pm != null && pm.GetType().GetMethod("SetClearedCampaign")?.Invoke(pm, new object[] { id }) is bool b && b;
                Flow.Log($"cheat SetClearedCampaign({id}) -> {cleared}");
            }
            for (int i = 0; i < 10 && (PlayerProfile() is int lv ? lv : 99) < C.Level; i++)
            {
                SendCheat("ReqCheatLevelUp");
                yield return new WaitForSecondsRealtime(1f);
            }
            Flow.Log($"after cheats level={PlayerProfile()}");
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
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, $"campaign slot {CampaignId}");
            slotButton.onClick.Invoke();

            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == CampaignId, 6f, $"campaign {CampaignId} detail");
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
            yield return Review.Shot($"a6-campaign-{CampaignId}");
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

        // Recover (the cheat menu's HP refill) when the diver is low, in the whole stage; see the header.
        static void KeepAlive()
        {
            if (Me() == null || Over() || State() == "Dying") return;
            if (HpFrac() < 0.6f) SendCheat("ReqSetCharacterHp", ("Hp", HpMax()));
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
            // A dead boss lies there until it despawns (run 12: swinging at the destroyed Donovan threw).
            if (!m || !Flow.Alive(m) || Me() == null) { yield return new WaitForSecondsRealtime(0.3f); yield break; }
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
                KeepAlive();
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

        static float PathLength(Vector3 to)
        {
            var path = new UnityEngine.AI.NavMeshPath();
            if (Me() == null || !UnityEngine.AI.NavMesh.SamplePosition(to, out var hit, 2.5f, UnityEngine.AI.NavMesh.AllAreas)
                || !UnityEngine.AI.NavMesh.CalculatePath(Me().transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path)
                || path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) return float.MaxValue;
            float len = 0f;
            for (int i = 1; i < path.corners.Length; i++) len += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            return len;
        }

        // The quest marker's goal: the nearer portal of the campaign's pair (SetQuestMarkerAtPortalPair; 1102 marks
        // 1101 in sector 10202). Walk there, fighting only what blocks the way, and hold F on it.
        static IEnumerator ToPortal()
        {
            var pair = new[] { Trigger(C.PortalA), Trigger(C.PortalB) }.Where(t => t != null).ToList();
            var portal = C.Id == 1102 ? Trigger(C.PortalA) : pair.OrderBy(t => PathLength(t.transform.position)).FirstOrDefault();
            Flow.Log($"portals {string.Join(" ", pair.Select(t => $"{Flow.Get(t, "TriggerId")}@{t.transform.position} path={PathLength(t.transform.position):F0} can={Flow.Get(t, "CanTriggered")}"))}");
            if (portal == null) { Flow.Log($"no portal {C.PortalA}/{C.PortalB} in the stage"); yield break; }
            Flow.Log($"portal {Flow.Get(portal, "TriggerId")} at {portal.transform.position} can={Flow.Get(portal, "CanTriggered")} {MeState()}");
            float start = Time.realtimeSinceStartup;
            while (!Over() && Time.realtimeSinceStartup - start < 360f && Boss() == null)
            {
                if (State() == "Dying") { yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 60f, "out of Dying"); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog on the way"); continue; }
                KeepAlive();
                // Run past the sector's monsters (a Hard 200009 has 2232 HP and killed the diver in run 2); fight only
                // what blocks the way, i.e. when the run stalls with a monster next to the diver.
                var before = Me().transform.position;
                yield return Flow.Go(portal.transform.position, 1.2f, 20f, "portal", run: true, stop: () => State() == "Dying" || HpFrac() < 0.35f);
                var m = Enemy(2f);
                if (m != null && Flow.Flat(Me().transform.position - before).magnitude < 1f) { yield return Fight(m); continue; }
                if (Me() == null || Flow.Flat(portal.transform.position - Me().transform.position).magnitude > 2f) continue;
                yield return Review.Shot($"a6-{CampaignId}-portal");
                yield return Flow.InteractObject(portal, 0.8f, "portal");
                yield return Flow.WaitUntil(() => Flow.Flat(portal.transform.position - Me().transform.position).magnitude > 5f || Boss() != null, 6f, "teleported");
                Flow.Log($"after portal: {MeState()} boss={BossState()}");
                if (Flow.LastWaitOk) break;
            }
        }

        // The first use: radio "Wait, something's coming!", the cutscene, then the boss. Its bar shows once it targets the
        // diver (GameCombatSystem.UpdateAggroedBoss), so walk up to it as the Jul player did (ref boss-intro/01).
        static IEnumerator Encounter()
        {
            float t0 = Time.realtimeSinceStartup, nextLog = t0;
            bool cutsceneShot = false;
            while (Time.realtimeSinceStartup - t0 < 90f && !Over())
            {
                if (!cutsceneShot && Time.realtimeSinceStartup - t0 > 2.5f && State() == "Cutscene")
                {
                    cutsceneShot = true;
                    yield return Review.Shot($"a6-{CampaignId}-cutscene");
                }
                // 1105: Alex's line ("Soul Collector Eldris, answer my call…") opens a dialog before the cutscene.
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return Review.Shot($"a6-{CampaignId}-encounter-dialog");
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog before the boss");
                    continue;
                }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 2f; Flow.Log($"encounter t={Time.realtimeSinceStartup - t0:F1} {MeState()} boss={BossState()} hud={BossHud()}"); }
                if (Boss() != null && State() != "Cutscene") break;
                yield return new WaitForSecondsRealtime(0.25f);
            }
            if (Boss() == null) { DumpStageObjects("no boss"); yield break; }
            yield return Review.Shot($"a6-{CampaignId}-after-cutscene");
            KeepAlive();
            var boss = Boss();
            yield return Flow.WalkTo(() => Boss() != null && !BossHud().Contains("bosses=1") ? Boss().transform.position : (Vector3?)null, 2f, 20f, "up to the boss");
            Flow.Hold();
            yield return Flow.WaitUntil(() => BossHud().Contains("bosses=1"), 5f, "boss bar");
            Flow.Log($"encounter after {Time.realtimeSinceStartup - t0:F1}s boss={BossState()} hud={BossHud()} {MeState()}");
            yield return new WaitForSecondsRealtime(1.0f);
            yield return Shot(C.Intro);
        }

        static int s_lastSkill;
        static float s_summonWaitStart;
        static bool s_invincible;

        static IEnumerator FightBoss()
        {
            s_lastSkill = 0; s_summonWaitStart = -1f; s_invincible = false;
            float start = Time.realtimeSinceStartup, nextLog = start;
            int phase = -1, clicks = 0;
            float phaseStart = start;
            bool cheated = false;
            while (!Over() && Time.realtimeSinceStartup - start < 1200f)
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
                // The last phase's body stays (bosses keep their loot), so the fight ends on its Dead state.
                if (p == BossPhases.Length - 1 && (MonsterState(boss) == "Dead" || Flow.Hp(boss) <= 0)) break;
                if (p != phase)
                {
                    Flow.Log($"boss phase {p}: {BossState()} hud={BossHud()} after {Time.realtimeSinceStartup - start:F0}s {MeState()}");
                    phase = p; phaseStart = Time.realtimeSinceStartup; cheated = false;
                    yield return Review.Shot($"a6-{CampaignId}-phase{p + 1}-start");
                }
                float inPhase = Time.realtimeSinceStartup - phaseStart;
                float dBoss = Flow.Flat(boss.transform.position - Me().transform.position).magnitude;
                float bossHp = Flow.Hp(boss) / Mathf.Max(1f, Flow.Get(Model(boss), "HpMax") is float hm ? hm : 1f);
                // ref boss-fight/01, /03: trading blows with the boss, a few swings in.
                if (phase == 0 && clicks >= 8 && dBoss < 3f) yield return Shot(C.Fight);
                int skill = Flow.Get(Model(boss), "UsingSkillId") is int sk ? sk : 0;
                if (skill != 0 && skill != s_lastSkill) Flow.Log($"boss skill {skill} phase {p + 1} hp={bossHp:P0}");
                bool holdOff = false;
                if (C.Id == 1102 && phase == 1 && !s_shots.Contains(C.Fight2))
                {
                    // ref boss-fight/02: the red spikes are the summoning circles of 21005015 (Lost Child's Souls; run 8/9).
                    // The AI picks it at random (weight 200, 40 s cooldown, target within 12 m, at most 3 monsters within
                    // 5 m), so run 10 saw none in a minute. Wait for it in game time; keep the phase alive meanwhile.
                    if (s_summonWaitStart < 0f) s_summonWaitStart = Time.time;
                    holdOff = bossHp < 0.3f;
                    if (skill == 21005015 && s_lastSkill != 21005015)
                    {
                        // The skill draws EnemySummoningMagicCircle_Red for 2 s, then the souls rise in red pillars (the
                        // ref's spikes): run 19 at +0.6 s caught only the circles, run 20 (a circles shot first, ~12 s of
                        // rendering) only the souls. One shot, 1.6 s of game time after the skill shows.
                        float t = Time.time;
                        while (Time.time - t < 1.6f) yield return null;
                        yield return Shot(C.Fight2);
                    }
                    else if (Time.time - s_summonWaitStart > 300f)
                    {
                        Flow.Log($"no summon in 300 s of game time; phase 2 shot without it");
                        yield return Shot(C.Fight2);
                    }
                }
                // Donovan (one phase) mid-fight, ref boss-fight/04; the Enraged Soul Collector a few seconds into its phase.
                if (C.Id == 1103 && bossHp < 0.75f && dBoss < 4f) yield return Shot(C.Fight2);
                if (C.Id == 1105 && phase == 1 && inPhase > 6f && BossHud().Contains("bosses=1")) yield return Shot(C.Fight2);  // run 18 shot it during PhaseUp, bar hidden
                if (C.Id == 1102 && phase == 2 && inPhase > 6f && !s_shots.Contains("a6-phase3")) { s_shots.Add("a6-phase3"); yield return Review.Shot("a6-phase3"); }
                s_lastSkill = skill;
                // The review shots are taken: from here on the cheat menu's invincibility (buff 1000001) holds, as the last
                // phases killed the diver in runs 4 and 8 before the rescue.
                // Donovan (20625 HP on Hard, grenades) killed the refilled diver 79 s in (run 11): from 1103 on the
                // invincibility starts with the fight.
                bool shotsDone = C.Id != 1102 || s_shots.Contains(C.Intro) && s_shots.Contains(C.Fight) && s_shots.Contains(C.Fight2);
                if (shotsDone && !s_invincible) { s_invincible = true; SendCheat("ReqCheatApplyCharacterOptions", ("Invincibility", true)); }
                bool phaseShotsDone = !(C.Id == 1102 && phase == 1 && !s_shots.Contains(C.Fight2)) && !(phase == 0 && !s_shots.Contains(C.Fight));
                if (!cheated && inPhase > PhaseBudget && phaseShotsDone && Flow.Hp(boss) > 1)
                {
                    cheated = true;
                    Flow.Log($"phase {p + 1} still up after {PhaseBudget}s: {BossState()}");
                    SendCheat("ReqCheatApplyMonsterHp", ("Hp", 1));
                }
                if (holdOff)
                {
                    // Stay close (the summon wants the target within 12 m) without finishing the phase.
                    KeepAlive();
                    if (dBoss > 5f) yield return Flow.WalkTo(() => Boss() ? Boss().transform.position : (Vector3?)null, 4f, 2f, "near the boss");
                    else yield return new WaitForSecondsRealtime(0.3f);
                    continue;
                }
                if (State() == "Dying") { Flow.Log($"diver down {MeState()}"); yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 30f, "out of Dying"); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the boss fight"); continue; }
                KeepAlive();
                // Adds the boss summons stand in the way; the boss comes first unless one is in reach and the boss is not.
                if (!boss || Me() == null) continue;
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
        static IEnumerator AfterBoss()
        {
            if (Over()) yield break;
            s_exitsBefore.Clear();
            foreach (var e in SafeExits()) s_exitsBefore.Add(e.GetInstanceID());
            Flow.Log($"after boss: {s_exitsBefore.Count} SafeExit(s) already in the stage");
            if (C.Id == 1102) yield return RescueAlex();
            // 1103: Donovan drops the painting Formless Horizon (item 300011); picking it up (E_TaskAchieved) brings the booth.
            if (C.Id == 1103) yield return PickUpPainting();
            // 1105: OnBossDefeated plays IngameCutScene_Chapter_01_Campaign_5_2, then drops the booth.
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 120f && !Over() && SafeExitNow() == null)
            {
                KeepAlive();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog after the boss"); continue; }
                var add = Enemy(2.5f);
                if (add != null) yield return Fight(add);
                else yield return new WaitForSecondsRealtime(0.5f);
            }
            Flow.Log($"after boss: exit={(SafeExitNow() ? SafeExitNow().transform.position.ToString() : "-")} after {Time.realtimeSinceStartup - t0:F0}s {MeState()}");
            yield return Escape();
        }

        // The booth Lua drops after the boss (1007 at the boss zone): a SafeExit that was not in the stage before the fight
        // ends. 1103's map has its own SafeExit far off (run 13: (50, 102.5)), which is not it.
        static readonly HashSet<int> s_exitsBefore = new HashSet<int>();

        static IEnumerable<MonoBehaviour> SafeExits() => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "SafeExit");

        static Component SafeExitNow() => SafeExits()
            .FirstOrDefault(m => !s_exitsBefore.Contains(m.GetInstanceID()) && Flow.Get(m, "ExitState")?.ToString() == "Activated");

        // DropGoods near the diver (the painting falls where Donovan dies), or the loot in the boss's body.
        static readonly HashSet<int> s_lootTried = new HashSet<int>();

        static IEnumerator PickUpPainting()
        {
            s_lootTried.Clear();
            yield return new WaitForSecondsRealtime(3f);
            for (int round = 0; round < 8 && !Over() && SafeExitNow() == null; round++)
            {
                var me = Me();
                // A boss body despawns at once (MonsterDeadState: bosses go to ReservedDespawn), so the drop group 127000
                // (127001: Item 300011) lands as DropGoods or a box. Run 14 picked a "Body" MonsterBody at the diver's own
                // position: only dead monsters' bodies count (A5 IsLoot).
                var loot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "DropGoods" || m.GetType().Name == "RewardBox"
                        || m.GetType().Name == "MonsterBody" && Flow.Get(Flow.Get(m, "_monsterModel"), "State")?.ToString() == "Dead"))
                    .Where(m => !s_lootTried.Contains(m.GetInstanceID()))
                    .OrderBy(m => Flow.Flat(m.transform.position - me.transform.position).sqrMagnitude).ToList();
                if (round == 0) foreach (var l in loot.Take(8)) Flow.Log($"loot near: {l.GetType().Name} {l.name} at {l.transform.position} d={Flow.Flat(l.transform.position - me.transform.position).magnitude:F1}");
                var drop = loot.FirstOrDefault();
                if (drop) s_lootTried.Add(drop.GetInstanceID());
                Flow.Log($"painting round {round}: {(drop ? $"{drop.GetType().Name} {drop.name} at {drop.transform.position} id={Flow.Get(drop, "Id")}" : "nothing")} {MeState()}");
                if (drop == null) { yield return new WaitForSecondsRealtime(2f); continue; }
                yield return Flow.WalkTo(() => drop ? drop.transform.position : (Vector3?)null, 0.9f, 15f, "painting");
                if (!drop) continue;
                Flow.Aim(drop.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(drop), 3f, "focus painting");
                if (round == 0) yield return Review.Shot("a6-1103-painting");
                float hold = Flow.Get(drop, "HoldingTime") is float h ? h + 0.5f : 1f;
                yield return Flow.Keys(Mathf.Clamp(hold, 0.5f, 4f), Key.F);
                yield return new WaitForSecondsRealtime(1f);
                var panel = Flow.Find("LootingInventoryPanelPresenter");
                if (panel != null)
                {
                    yield return new WaitForSecondsRealtime(3f);
                    var slots = Flow.Get(panel, "_goodsSlotModels") as IList;
                    if (slots != null)
                        foreach (var slot in slots.Cast<object>().ToList())
                        {
                            var gs = Flow.Value(slot, "RxGoodsSlot");
                            if (gs == null || Flow.Get(gs, "IsEmpty") is bool e && e) continue;
                            Flow.Log($"painting: take {Flow.Get(gs, "Id")} x{Flow.Get(gs, "Count")}");
                            Fire(slot, "OnEnter", slot);
                            yield return null;
                            Fire(slot, "OnLeftClick", slot);
                            yield return new WaitForSecondsRealtime(0.4f);
                        }
                    yield return Flow.Keys(0.2f, Key.Escape);
                    yield return new WaitForSecondsRealtime(1f);
                }
                yield return Flow.WaitUntil(() => SafeExitNow() != null, 5f, "booth after the painting");
            }
        }

        static void Fire(object owner, string subject, object arg)
        {
            var sub = Flow.Get(owner, subject);
            sub?.GetType().GetMethod("OnNext")?.Invoke(sub, new[] { arg });
        }

        static IEnumerator RescueAlex()
        {
            yield return new WaitForSecondsRealtime(4f);
            var alex = Flow.FindNpc(NpcAlex);
            Flow.Log($"rescue: alex={(alex ? alex.transform.position.ToString() : "-")} state={(alex ? Flow.Value(alex, "RxNpcState") : null)} {MeState()}");
            if (alex == null) yield break;
            yield return Flow.InteractNpc(alex, "Alex");
            // Lua OnAliceRescued talks for ~6 s, then drops booth 1007 at zone 11021; the boss's adds are still about
            // (run 4: they killed the diver here).
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 9f && !Over())
            {
                KeepAlive();
                var add = Enemy(2.5f);
                if (add != null) yield return Fight(add);
                else yield return new WaitForSecondsRealtime(0.3f);
            }
            yield return Review.Shot($"a6-{CampaignId}-rescued");
            DumpStageObjects("rescued");
        }

        static IEnumerator Escape()
        {
            if (Over()) yield break;
            var me = Me();
            Component exit = SafeExitNow();
            Flow.Log($"escape: exit={(exit ? $"{exit.GetType().Name} {exit.transform.position} {Flow.Get(exit, "ExitState")}" : "-")}");
            if (exit == null) yield break;
            // SafeExit wants F held for SafeExitInteractionTime (3 s of game time). Run 9 held 3.3 s of real time three
            // times and never escaped: hold until the diver leaves (EscapeEvent) or the booth reacts, as a player does.
            for (int attempt = 0; attempt < 3 && !Over() && Flow.Find("EscapeSuccessPopupView") == null; attempt++)
            {
                yield return Flow.Go(exit.transform.position, 0.9f, 90f, "dive booth 1007", run: true);
                Flow.Aim(exit.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus dive booth 1007");
                float gameStart = Time.time, realStart = Time.realtimeSinceStartup;
                Flow.Hold(Key.F);
                while (Time.realtimeSinceStartup - realStart < 12f && !Over() && Flow.Find("EscapeSuccessPopupView") == null
                       && State() != "Escaped" && Flow.Get(exit, "ExitState")?.ToString() == "Activated")
                    yield return null;
                Flow.Hold();
                Flow.Log($"booth 1007 try {attempt}: held {Time.realtimeSinceStartup - realStart:F1}s real / {Time.time - gameStart:F1}s game, state={State()} exit={Flow.Get(exit, "ExitState")} remain={Flow.Get(exit, "StateRemainTime")} focus={Flow.Focus()} top={Flow.TopPopup()}");
                yield return new WaitForSecondsRealtime(1f);
            }
            yield return Flow.WaitUntil(() => Flow.Find("EscapeSuccessPopupView") != null || Flow.ActiveScene != "StageScene", 60f, "escape banner");
            Flow.Log($"escape: {MeState()} top={Flow.TopPopup()} stage={Flow.StageState()}");
            if (Flow.Find("EscapeSuccessPopupView") != null) { yield return new WaitForSecondsRealtime(1f); yield return Review.Shot($"a6-{CampaignId}-escape-success"); }
            if (Flow.Find("EscapeSuccessPopupView") == null && Flow.ActiveScene == "StageScene") yield break;
            yield return Results();
        }

        // Dive Results over the lounge (Confirm), then Elara's 1102 wrap-up (Lua Step_00010 QuestComplete) as in A5.
        static IEnumerator Results()
        {
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 240f, "back in the lounge");
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 120f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            if (view == null) { Flow.Log($"no result popup: top={Flow.TopPopup()} {Flow.LoadState()}"); yield break; }
            yield return new WaitForSecondsRealtime(6f);
            yield return Review.Shot($"a6-{CampaignId}-result");
            for (int i = 0; i < 6 && Flow.Find("StageResultPopupView") != null; i++)
            {
                if (Flow.Ready(Flow.Button(view, "Confirm_Button"))) yield return Flow.Click(view, "Confirm_Button");
                else if (Flow.Ready(Flow.Button(view, "SkipButton"))) yield return Flow.Click(view, "SkipButton");
                yield return new WaitForSecondsRealtime(2f);
            }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") == null && Flow.StageState() == "Playing", 60f, "lounge after the dive");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge after the dive");
            yield return new WaitForSecondsRealtime(3f);
            // Felix stands next to Elara; run 19's F caught him and the wrap-up never played. Try again until it has.
            var pm = Flow.Find("PlayerManager");
            bool Cleared() => pm?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pm, new object[] { CampaignId }) is bool b && b;
            for (int i = 0; i < 3 && !Cleared(); i++)
            {
                yield return Talk(700000, $"Elara {CampaignId} complete");
                yield return Flow.WaitUntil(Cleared, 3f, "campaign cleared");
            }
            Flow.Log($"after {CampaignId}: cleared={Flow.Find("PlayerManager")?.GetType().GetMethod("IsClearedCampaign")?.Invoke(Flow.Find("PlayerManager"), new object[] { CampaignId })} {Flow.LoadState()} top={Flow.TopPopup()}");
            yield return Review.Shot($"a6-{CampaignId}-lounge-after");
        }
    }
}

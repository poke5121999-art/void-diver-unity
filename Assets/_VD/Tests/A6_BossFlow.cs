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
    // The story campaigns without a boss play their objectives in Lua order (BossCampaign.Story), Normal difficulty:
    // 1104 "The Compatible One" (collision 104011 -> encounter cutscene -> wave 11041 -> painting trigger 11041),
    // 1201 "Iron and Fire" (observation devices 12011..12013, each sends a squad; the third starts wave 12011),
    // 1202 "No Survivors" (Glitch 700405, portal pairs 1111/1113, 1112/1114, 1115/1116, military box 12022). Each then
    // drops its own dive booth 1007 (SafeExit) and the test escapes through it. Each test checks the cleared flag and
    // that the next story campaign unlocks; the level part of that unlock is the cheat level-up (a player's grind).
    // No refs exist for these moments; their shots are a6-<id>-*.
    // 1101 "Tutorial: First Request" (StoryCampaign1101, Case1101) proves the first link of the chain with no
    // cleared-campaign cheat: from the post-tutorial fixture it takes 1101 at Elara, dives (again, up to MaxDives1101,
    // as a player does) looting boxes until Lua's tasks hold (11011 TeamTotalWorth >= 7000, 11012 five Ancient Components
    // 301003), escapes through a terminal (WaveExit), confirms Dive Results, hears Elara's QuestComplete, then reads the
    // cleared flag back from SavesDir/PlayerContext_0.sav with the game's SerializeHelper and finds 1102 unlocked in
    // Elara's campaign list. Its cheats: ReqSetCharacterHp to HpMax (Recover) when HP < 35%, and ReqCheatLevelUp only if
    // the diver is under 1102's UserLevel:2 at the end; no stress setter, no invincibility, no SetClearedCampaign.
    // It runs past shadow monsters (9000xx) and leaves for a terminal at stress 90+, as a player with no calming item does
    // (the first try stood 10 min at stress 100 fighting shadows); the next dive starts rested.
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
    // - The refill runs from the game loop (Guardian, every 0.15 s) for the whole stage: a 1201 squad took 75% HP in one
    //   swing. Stress at 70+ goes back to 0 with the cheat menu's stress setter (ReqSetCharacterStress): at 100 shadow
    //   Dark Youngs keep spawning and stun the diver, which a player avoids by resting it off. Story tests never turn on
    //   invincibility.
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
            public int[] Phases = Array.Empty<int>();
            public string Intro, Fight, Fight2;   // review keys; Fight2 is taken in the second phase or later in the fight
            public string Difficulty = "Hard";
            public Func<IEnumerator> Story;       // story campaigns without a boss: their objectives, played in order
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
            // The story campaigns without a boss (Normal difficulty, no ref frames): their key moments are the shots.
            // The Compatible One: collision 104011 -> encounter cutscene -> wave 11041 -> painting trigger 11041 -> booth.
            // First Request: loot until tasks 11011 (TeamTotalWorth 7000) and 11012 (5x Ancient Component 301003) hold,
            // escape through a terminal (WaveExit); its own case (Case1101), default difficulty.
            new BossCampaign { Id = 1101, Level = 1, Difficulty = null,
                Intro = "a6-1101-loot", Fight = "a6-1101-terminal", Fight2 = "a6-1101-campaign-list" },
            new BossCampaign { Id = 1104, Level = 4, Difficulty = "Normal", Story = Story1104,
                Intro = "a6-1104-encounter", Fight = "a6-1104-wave", Fight2 = "a6-1104-painting" },
            // Iron and Fire: observation equipment 12011 -> 12012 -> 12013 (monsters, then wave 12011) -> booth.
            new BossCampaign { Id = 1201, Level = 6, Difficulty = "Normal", Story = Story1201,
                Intro = "a6-1201-equip-1", Fight = "a6-1201-equip-3", Fight2 = "a6-1201-wave" },
            // No Survivors: Glitch (NPC 700405) -> portals 1111/1113, 1112/1114, 1115/1116 -> military box 12022 -> booth.
            new BossCampaign { Id = 1202, Level = 7, Difficulty = "Normal", Story = Story1202,
                Intro = "a6-1202-glitch", Fight = "a6-1202-portals", Fight2 = "a6-1202-milbox" },
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
            // The Local platform stays for the whole session (UseLocalPlatform puts Steam back when play mode exits): the
            // next test boots the title again, and on Steam its splash fails "[Auth] Authentication failed" (run story4).
        }

        static readonly HashSet<string> s_shots = new HashSet<string>();

        static IEnumerator Shot(string key)
        {
            if (!s_shots.Add(key)) yield break;
            KeepAlive();
            Flow.Log($"shot {key} {MeState()} boss={BossState()} hud={BossHud()}");
            yield return Review.Shot(key);
            KeepAlive();
        }

        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1102() { yield return Play(1102); }
        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1103() { yield return Play(1103); }
        [UnityTest, Timeout(3000000)] public IEnumerator BossCampaign1105() { yield return Play(1105); }
        [UnityTest, Timeout(3000000)] public IEnumerator StoryCampaign1101() { yield return Flow.GameLog.Run("StoryCampaign1101", Case1101()); }
        [UnityTest, Timeout(3000000)] public IEnumerator StoryCampaign1104() { yield return Play(1104); }
        [UnityTest, Timeout(3000000)] public IEnumerator StoryCampaign1201() { yield return Play(1201); }
        [UnityTest, Timeout(3000000)] public IEnumerator StoryCampaign1202() { yield return Play(1202); }

        // Errors the game logs during a case fail it (as in A6b): see Flow.GameLog.
        static IEnumerator Play(int campaignId) => Flow.GameLog.Run($"campaign {campaignId}", PlayCase(campaignId));

        static IEnumerator PlayCase(int campaignId)
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
            s_exitsBefore.Clear();
            foreach (var e in SafeExits()) s_exitsBefore.Add(e.GetInstanceID());
            WatchExitDespawns();
            yield return Review.Shot($"a6-{CampaignId}-stage-start");
            // KeepAlive between the test's own steps is too coarse in a wave: run full5 lost 380 HP (75%) in the 1.4 s one
            // swing took. The guardian refills from the game loop itself, every 0.15 s, for the whole stage.
            var guardian = new GameObject("VD_A6_Guardian").AddComponent<Guardian>();
            Object.DontDestroyOnLoad(guardian.gameObject);
            if (C.Story != null) yield return C.Story();
            else
            {
                yield return ToPortal();
                yield return Encounter();
                Assert.IsTrue(s_shots.Contains(C.Intro), "the boss never showed up");
                yield return FightBoss();
            }
            yield return AfterBoss();

            var missing = new[] { C.Intro, C.Fight, C.Fight2 }.Where(k => !s_shots.Contains(k)).ToList();
            Flow.Log($"end scene={Flow.ActiveScene} {MeState()} shots=[{string.Join(",", s_shots)}] missing=[{string.Join(",", missing)}]");
            Assert.IsEmpty(missing, "boss shots missing");
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the dive did not end back in the lounge (booth escape)");
            var pmEnd = Flow.Find("PlayerManager");
            Assert.IsTrue(pmEnd?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pmEnd, new object[] { CampaignId }) is bool done && done, $"campaign {CampaignId} not cleared after Elara's wrap-up");
            // The next story campaign unlocks: its CampaignCleared condition holds now; its UserLevel
            // condition is the grind a player does between stories, so that part is a level cheat.
            var nextStory = StoryOrder.SkipWhile(x => x != CampaignId).Skip(1).FirstOrDefault();
            if (nextStory != 0)
            {
                var row = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("GameCommon.Table.TCampaign")).FirstOrDefault(t => t != null)?.GetProperty("Table")?.GetValue(null);
                var next = row?.GetType().GetProperty("Item")?.GetValue(row, new object[] { nextStory });
                var conditions = Flow.Get(next, "UnlockConditions");
                var check = pmEnd?.GetType().GetMethod("CheckStateConditions");
                bool Met() => check?.Invoke(pmEnd, new[] { conditions }) is bool m && m;
                for (int i = 0; i < 3 && !Met(); i++)
                {
                    SendCheat("ReqCheatLevelUp");
                    yield return new WaitForSecondsRealtime(1f);
                }
                Flow.Log($"next story {nextStory}: unlock {string.Join("|", (conditions as Array)?.Cast<object>() ?? Enumerable.Empty<object>())} met={Met()} level={Flow.Get(pmEnd, "Level")}");
                Assert.IsTrue(Met(), $"next story {nextStory} still locked after {CampaignId} cleared");
            }
        }

        // Story campaigns in unlock order (TCampaign UnlockConditions CampaignCleared chain).
        static readonly int[] StoryOrder = { 1101, 1102, 1103, 1104, 1105, 1201, 1202 };

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
            foreach (var id in StoryOrder.TakeWhile(x => x != CampaignId))
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
            yield return OpenCampaignPopup();
            if (Flow.Find("CampaignPopupPresenter") == null) { Flow.Log($"campaign menu did not open, top={Flow.TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(2f);
            yield return PickCampaign();
        }

        // Elara, her Campaign function: the campaign list (CampaignPopupPresenter).
        static IEnumerator OpenCampaignPopup()
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
        }

        static List<MonoBehaviour> CampaignSlots() => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "CampaignSlotPresenter").ToList();

        static IEnumerator PickCampaign()
        {
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
            var order = new[] { "Easy", "Normal", "Hard", "Insane" };
            for (int i = 0; i < 3 && C.Difficulty != null && Flow.Value(ctx, "RxDifficulty")?.ToString() != C.Difficulty; i++)
            {
                var next = Flow.Get(view, "Info_Difficulty_NextButton") as UnityEngine.UI.Button;
                var pre = Flow.Get(view, "Info_Difficulty_PreButton") as UnityEngine.UI.Button;
                bool up = Array.IndexOf(order, Flow.Value(ctx, "RxDifficulty")?.ToString()) < Array.IndexOf(order, C.Difficulty);
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

        // Recover (the cheat menu's HP refill) when the diver is low, in the whole stage; see the header. 1101 refills
        // under 35% and leaves stress alone.
        static float s_hpFloor = 0.6f;
        static bool s_stressRecover = true;

        static void KeepAlive()
        {
            if (Me() == null || Over() || State() == "Dying") return;
            if (HpFrac() < s_hpFloor) SendCheat("ReqSetCharacterHp", ("Hp", HpMax()));
            if (!s_stressRecover) return;
            // Stress 100 keeps summoning shadow Dark Youngs (ShadowMonsterSpawn) and stuns the diver; a player rests it
            // off between fights, here it is the recover cheat (run story3: the 1104 painting never took the F).
            if (Convert.ToSingle(Flow.Get(Me(), "Stress") ?? 0f) >= 70f) SendCheat("ReqSetCharacterStress", ("Stress", 0f));
        }

        class Guardian : MonoBehaviour
        {
            float _next;
            void Update()
            {
                if (Flow.ActiveScene != "StageScene") { Destroy(gameObject); return; }
                if (Time.realtimeSinceStartup < _next) return;
                _next = Time.realtimeSinceStartup + 0.15f;
                KeepAlive();
            }
        }

        // A wait that still refills HP every frame (run s1201b: a 10-monster squad took 630 HP during a blind 3 s wait).
        static IEnumerator Breathe(float seconds)
        {
            for (float end = Time.realtimeSinceStartup + seconds; Time.realtimeSinceStartup < end && !Over(); )
            {
                KeepAlive();
                yield return null;
            }
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
        // ---- story campaigns without a boss (1104, 1201, 1202) ----

        static Component CollisionTriggerById(int id) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .FirstOrDefault(m => m != null && m.GetType().Name == "CollisionTrigger" && Flow.Get(m, "TriggerId") is int t && t == id);

        static Vector3? ZonePosition(int zoneSpawnId)
        {
            var om = Flow.Find("GameObjectManager");
            try { return om?.GetType().GetMethod("GetZoneSpawnPosition")?.Invoke(om, new object[] { zoneSpawnId }) as Vector3?; }
            catch (Exception e) { Flow.Log($"zone {zoneSpawnId}: {e.InnerException?.Message}"); return null; }
        }

        // Several Flow.Go legs while the distance shrinks (one leg gives up after 20 s; A6b run 6), fighting what blocks.
        static IEnumerator GoFar(Vector3 goal, float arrive, string what, float budget = 180f)
        {
            float start = Time.realtimeSinceStartup, best = float.MaxValue;
            int noProgress = 0;
            while (Time.realtimeSinceStartup - start < budget && Me() != null && !Over())
            {
                KeepAlive();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog on the way to {what}"); continue; }
                float d = Flow.Flat(goal - Me().transform.position).magnitude;
                if (d <= arrive) break;
                var m = Enemy(2f);
                if (m != null && noProgress > 0) { yield return Fight(m); continue; }
                if (d < best - 1f) { best = d; noProgress = 0; }
                else if (++noProgress >= 4) { Flow.Log($"go far {what}: no progress at d={d:F1}"); break; }
                yield return Flow.Go(goal, arrive, 40f, what, run: true);
                if (noProgress > 0) yield return Flow.Keys(0.5f, noProgress % 2 == 1 ? Key.A : Key.D);
            }
        }

        // Fight what is near until done() or the time is up, staying around a point (a wave's spawn).
        static IEnumerator Hold(Vector3 at, Func<bool> done, float budget, string what, Func<IEnumerator> onTick = null)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            while (!done() && !Over() && Time.realtimeSinceStartup - start < budget)
            {
                KeepAlive();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog in {what}"); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"{what}: t={Time.realtimeSinceStartup - start:F0} monsters near={Flow.Monsters().Count(x => Flow.Flat(x.transform.position - Me().transform.position).magnitude < 15f)} {MeState()}"); }
                if (onTick != null) yield return onTick();
                var m = Enemy(12f);
                if (m != null) { yield return Fight(m); continue; }
                if (Flow.Flat(at - Me().transform.position).magnitude > 4f) { yield return Flow.WalkTo(() => at, 3f, 4f, $"back to {what}"); continue; }
                yield return new WaitForSecondsRealtime(0.5f);
            }
            Flow.Log($"{what}: done={done()} after {Time.realtimeSinceStartup - start:F0}s {MeState()}");
        }

        static IEnumerator UseTrigger(int id, string what, string shot = null)
        {
            var t = Trigger(id);
            Flow.Log($"{what}: trigger {id} {(t ? $"at {t.transform.position} can={Flow.Get(t, "CanTriggered")}" : "missing")}");
            if (t == null) yield break;
            yield return GoFar(t.transform.position, 1.2f, what);
            yield return Flow.WaitUntil(() => !t || Flow.Get(t, "CanTriggered") is bool c && c, 10f, $"{what} triggerable");
            for (int i = 0; i < 6 && t && Flow.Get(t, "CanTriggered") is bool can && can && !Over(); i++)
            {
                // A monster on the diver (stun, hit reactions) eats the F; clear it first.
                for (int f = 0; f < 5 && Enemy(6f) is Component m && !Over(); f++) yield return Fight(m);
                yield return GoFar(t.transform.position, 1.2f, what);
                // The interaction sensor is small: run diag6 stood 0.96 m from equipment 2 for six tries with no focus.
                yield return Flow.InteractObject(t, 1.0f, what, i == 0 ? 1.0f : 0.5f);
                yield return Breathe(1f);
            }
            Flow.Log($"{what}: after F can={(t ? Flow.Get(t, "CanTriggered") : "gone")} {MeState()}");
            Assert.IsFalse(t && Flow.Get(t, "CanTriggered") is bool still && still, $"{what}: trigger {id} still takes F after 6 tries");
            if (shot != null) { yield return Breathe(1f); yield return Shot(shot); }
        }

        // 1104 The Compatible One: walk into collision 104011 at the gallery (ZONE 11043), the encounter cutscene
        // (IngameCutScene_Chapter_01_Campaign_4), the Voice of Liberation wave (WaveExecutor 11041); when it is cleared the
        // painting (trigger 11041) can be touched, and Lua drops booth 1007 at zone 11042.
        static IEnumerator Story1104()
        {
            var col = CollisionTriggerById(104011);
            var waveAt = ZonePosition(11043) ?? (col ? col.transform.position : Me().transform.position);
            Flow.Log($"1104: collision 104011 {(col ? col.transform.position.ToString() : "missing")} wave zone {waveAt}");
            yield return GoFar(col ? col.transform.position : waveAt, 0.8f, "gallery");
            yield return Flow.WaitUntil(() => State() == "Cutscene", 8f, "encounter cutscene");
            if (State() == "Cutscene") { yield return Breathe(3f); yield return Shot(C.Intro); }
            yield return Flow.WaitUntil(() => State() != "Cutscene", 90f, "cutscene end");
            var painting = Trigger(11041);
            bool Cleared() => painting && Flow.Get(painting, "CanTriggered") is bool c && c;
            yield return Hold(waveAt, Cleared, 300f, "1104 wave", () => WaveShot(C.Fight));
            yield return UseTrigger(11041, "painting", C.Fight2);
        }

        static IEnumerator WaveShot(string key)
        {
            if (s_shots.Contains(key)) yield break;
            if (Flow.Monsters().Count(x => Flow.Flat(x.transform.position - Me().transform.position).magnitude < 10f) >= 3)
                yield return Shot(key);
        }

        // 1201 Iron and Fire: three observation devices in order (12011 -> 12012 -> 12013). The first two bring firemen
        // and soldiers; the third starts wave 12011, whose clear drops booth 1007 next to it.
        static IEnumerator Story1201()
        {
            for (int i = 1; i <= 3; i++)
            {
                int id = 12010 + i;
                yield return UseTrigger(id, $"equipment {i}", i == 1 ? C.Intro : i == 3 ? C.Fight : $"a6-1201-equip-{i}");
                Assert.IsFalse(Over(), $"diver {State()} at equipment {i}");
                var at = Me().transform.position;
                yield return Breathe(3f);   // Lua spawns the squad 1.5 s after the device
                if (i < 3) yield return Hold(at, () => Enemy(14f) == null, 90f, $"1201 after equipment {i}");
                else yield return Hold(at, () => SafeExitNow() != null, 420f, "1201 wave", () => WaveShot(C.Fight2));
                Assert.IsFalse(Over(), $"diver {State()} after equipment {i}");
            }
        }

        // 1202 No Survivors: Glitch (NPC 700405, QuestTalkable) opens the portals; three portal pairs lead through the
        // shopping tower to the basement, where the military box (trigger 12022) is empty and Lua drops booth 1007.
        static IEnumerator Story1202()
        {
            var glitch = Flow.FindNpc(700405);
            Flow.Log($"1202: Glitch {(glitch ? glitch.transform.position.ToString() : "missing")} state={(glitch ? Flow.Value(glitch, "RxNpcState") : null)}");
            if (glitch == null) yield break;
            yield return GoFar(glitch.transform.position, 1.5f, "Glitch");
            for (int i = 0; i < 3 && Flow.Find("DialogPopupView") == null; i++)
            {
                yield return Flow.InteractNpc(glitch, "Glitch");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 4f, "Glitch dialog");
            }
            if (Flow.Find("DialogPopupView") != null)
            {
                yield return new WaitForSecondsRealtime(1.5f);
                yield return Shot(C.Intro);
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Glitch");
            }
            foreach (var (a, b) in new[] { (1111, 1113), (1112, 1114), (1115, 1116) })
            {
                var pair = new[] { Trigger(a), Trigger(b) }.Where(t => t != null).ToList();
                var portal = pair.OrderBy(t => PathLength(t.transform.position)).FirstOrDefault();
                Flow.Log($"1202 portals {a}/{b}: {string.Join(" ", pair.Select(t => $"{Flow.Get(t, "TriggerId")}@{t.transform.position} path={PathLength(t.transform.position):F0}"))}");
                if (portal == null) yield break;
                var from = portal.transform.position;
                yield return GoFar(from, 1.2f, $"portal {Flow.Get(portal, "TriggerId")}");
                yield return Flow.InteractObject(portal, 0.8f, $"portal {Flow.Get(portal, "TriggerId")}");
                yield return Flow.WaitUntil(() => Flow.Flat(Me().transform.position - from).magnitude > 5f, 6f, "teleported");
                yield return new WaitForSecondsRealtime(1f);
                if (a == 1115) yield return Shot(C.Fight);
            }
            yield return UseTrigger(12022, "military box", C.Fight2);
        }

        static IEnumerator AfterBoss()
        {
            if (Over()) yield break;
            Flow.Log($"after boss: {s_exitsBefore.Count} SafeExit(s) were in the stage at the start");
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

        // Who despawns a booth: GameObjectManager.OnInteractiveObjectDespawned, with the call stack (run 22: booth 1007 in
        // 1104 vanished as the diver walked up to it).
        class DespawnObserver<T> : IObserver<T>
        {
            public void OnNext(T o)
            {
                if (o is Component c && c && c.GetType().Name.EndsWith("Exit"))
                    Flow.Log($"despawned {c.GetType().Name} at {c.transform.position} state={Flow.Get(c, "ExitState")} stack:\n{Environment.StackTrace}");
            }
            public void OnError(Exception error) { }
            public void OnCompleted() { }
        }

        static IDisposable s_despawnSub;

        static void WatchExitDespawns()
        {
            s_despawnSub?.Dispose();
            var subject = Flow.Get(Flow.Find("GameObjectManager"), "OnInteractiveObjectDespawned");
            var observable = subject?.GetType().GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IObservable<>));
            if (observable == null) { Flow.Log("exit despawns: subject not found"); return; }
            var observer = Activator.CreateInstance(typeof(DespawnObserver<>).MakeGenericType(observable.GetGenericArguments()[0]));
            s_despawnSub = observable.GetMethod("Subscribe").Invoke(subject, new[] { observer }) as IDisposable;
        }

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

        // ---- 1101: the first story, from the post-tutorial profile, no cleared-campaign cheat ----

        const int Campaign1101 = 1101, Campaign1102 = 1102;
        const int MaxDives1101 = 4;
        const float LootBudget1101 = 900f;   // seconds of looting per dive before heading for a terminal

        static IEnumerator Case1101()
        {
            C = Bosses.First(b => b.Id == Campaign1101);
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6";
            s_shots.Clear();
            s_hpFloor = 0.35f;
            s_stressRecover = false;
            try
            {
                yield return Flow.ContinueToLounge();
                Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
                Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
                Flow.RouteInputToGame();
                Assert.IsFalse(SavedCleared(Campaign1101, out var before), $"the fixture save already has 1101 cleared ({before})");
                Flow.Log($"1101 start: saved {before}; level={Flow.Get(Flow.Find("PlayerManager"), "Level")}");

                yield return Talk(700000, "Elara 80200 start");
                foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                    yield return Talk(id, $"{who} 80200");
                yield return Talk(700000, "Elara 80200 finish");

                yield return StartCampaign();
                Assert.AreEqual("1101", CampaignNow(), "campaign 1101 did not start");
                yield return Briefing();

                for (int dive = 1; dive <= MaxDives1101 && !TasksDone(); dive++)
                {
                    yield return EnterBooth();
                    Assert.AreEqual("StageScene", Flow.ActiveScene, $"dive {dive}: the dive booth did not lead into the stage");
                    yield return Dive1101(dive);
                    Assert.AreNotEqual("Dead", State(), $"dive {dive}: the diver died");
                    yield return Results(wrapUp: false);
                    Assert.AreEqual("LoungeScene", Flow.ActiveScene, $"dive {dive} did not end back in the lounge");
                    Flow.Log($"1101 after dive {dive}: tasks {Tasks()} done={TasksDone()} campaign={CampaignNow()}");
                }
                Assert.IsTrue(TasksDone(), $"1101 tasks not achieved after {MaxDives1101} dives: {Tasks()}");

                // Step_00010 puts QuestComplete on Elara; her wrap-up calls GameCampaignManager.CompleteQuest.
                var pm = Flow.Find("PlayerManager");
                bool Cleared() => pm?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pm, new object[] { Campaign1101 }) is bool b && b;
                for (int i = 0; i < 3 && !Cleared(); i++)
                {
                    yield return Talk(700000, "Elara 1101 complete");
                    yield return Flow.WaitUntil(Cleared, 3f, "campaign cleared");
                }
                Flow.Log($"after 1101: cleared in memory={Cleared()} {Flow.LoadState()} top={Flow.TopPopup()}");
                yield return Review.Shot("a6-1101-lounge-after");

                // The save the game writes (SavesDir/PlayerContext_0.sav, the lounge autosave) must hold 1101's flag.
                string saved = "-";
                yield return Flow.WaitUntil(() => SavedCleared(Campaign1101, out saved), 120f, "1101 cleared in PlayerContext_0.sav");
                Flow.Log($"1101 saved: {saved}");
                Assert.IsTrue(SavedCleared(Campaign1101, out saved), $"1101 not cleared in the saved profile: {saved}");

                // 1102 in Elara's campaign list: UnlockConditions UserLevel:2|CampaignCleared:1101. The level is the grind.
                for (int i = 0; i < 3 && !(Flow.Get(pm, "Level") is int lv && lv >= 2); i++)
                {
                    SendCheat("ReqCheatLevelUp");
                    yield return new WaitForSecondsRealtime(1f);
                }
                yield return OpenCampaignPopup();
                Assert.IsNotNull(Flow.Find("CampaignPopupPresenter"), "Elara's campaign list did not open");
                yield return new WaitForSecondsRealtime(2f);
                var slots = CampaignSlots();
                string SlotState(MonoBehaviour slot)
                {
                    var view = Flow.Get(slot, "_view");
                    var lockIcon = Flow.Get(view, "LockIcon") as Component;
                    var done = Flow.Get(view, "CompletedMark") as Component;
                    var button = Flow.Get(view, "Button") as Component;
                    var locked = Flow.Get(button ? button.GetComponent("ButtonController") : null, "_isLocked");
                    return $"lockIcon={(lockIcon ? lockIcon.gameObject.activeSelf.ToString() : "-")} locked={locked} completed={(done ? done.gameObject.activeSelf.ToString() : "-")}";
                }
                foreach (var sl in slots) Flow.Log($"campaign slot {Flow.Get(Flow.Get(sl, "_model"), "CampaignId")}: {SlotState(sl)}");
                var s1101 = slots.FirstOrDefault(x => Flow.Get(Flow.Get(x, "_model"), "CampaignId") is int id && id == Campaign1101);
                var s1102 = slots.FirstOrDefault(x => Flow.Get(Flow.Get(x, "_model"), "CampaignId") is int id && id == Campaign1102);
                yield return Shot(C.Fight2);
                Assert.IsNotNull(s1102, "no 1102 slot in Elara's campaign list");
                Assert.AreEqual("lockIcon=False locked=False", string.Join(" ", SlotState(s1102).Split(' ').Take(2)), $"1102 slot is locked: {SlotState(s1102)}");
                if (s1101 != null) Assert.IsTrue(SlotState(s1101).EndsWith("completed=True"), $"1101 slot has no completed mark: {SlotState(s1101)}");
                yield return Flow.Keys(0.2f, Key.Escape);
                Assert.IsEmpty(new[] { C.Intro, C.Fight, C.Fight2 }.Where(k => !s_shots.Contains(k)).ToList(), "1101 shots missing");
            }
            finally
            {
                s_hpFloor = 0.6f;
                s_stressRecover = true;
            }
        }

        // PlayerContext_0.sav read back with the game's own SerializeHelper (what Continue loads): cleared flag of a campaign.
        static bool SavedCleared(int campaignId, out string what)
        {
            what = "-";
            try
            {
                string path = System.IO.Path.Combine(System.IO.Directory.GetParent(Application.dataPath).FullName, "SavesDir", "PlayerContext_0.sav");
                if (!System.IO.File.Exists(path)) { what = "no PlayerContext_0.sav"; return false; }
                var bytes = System.IO.File.ReadAllBytes(path);
                var dataType = GameType("PlayerContextData");
                var deserialize = GameType("SerializeHelper")?.GetMethod("Deserialize")?.MakeGenericMethod(dataType);
                var data = deserialize?.Invoke(null, new object[] { bytes });
                var flags = Flow.Get(data, "ClearedCampaignFlags");
                var table = GameType("GameCommon.Table.TCampaign")?.GetProperty("Table")?.GetValue(null);
                var row = table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { campaignId });
                var bit = Flow.Get(row, "BitFlagIndex");
                var set = flags?.GetType().GetMethod("IsSet")?.Invoke(flags, new[] { bit });
                what = $"{System.IO.File.GetLastWriteTime(path):HH:mm:ss} bit {bit} set={set} flags={string.Join(",", (Flow.Get(flags, "Flags") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>())}";
                return set is bool b && b;
            }
            catch (Exception e) { what = $"read threw {e.GetType().Name}: {e.InnerException?.Message ?? e.Message}"; return false; }
        }

        static object CampaignManager() => Flow.Find("GameCampaignManager");
        static bool TasksDone() => CampaignManager()?.GetType().GetMethod("IsAllTaskAchieved")?.Invoke(CampaignManager(), null) is bool b && b;
        static string Tasks() => string.Join(" ", (Flow.Get(CampaignManager(), "CampaignTasks") as IEnumerable)?.Cast<object>()
            .Select(kv => Flow.Get(kv, "Value")).Select(t => $"{Flow.Get(t, "Id")}:{Flow.Get(t, "Progress")}/{Flow.Get(t, "Goal")}")
            ?? Enumerable.Empty<string>());

        static readonly HashSet<int> s_looted = new HashSet<int>();
        static readonly List<Vector3> s_waypoints = new List<Vector3>();

        // Loot boxes (the Ancient Components come from the chapter-1 quest box drops) and what lies near, until both
        // tasks hold or the budget runs out; then a terminal.
        static IEnumerator Dive1101(int dive)
        {
            yield return new WaitForSecondsRealtime(3f);
            DumpStageObjects($"1101 dive {dive}");
            s_looted.Clear();
            s_waypoints.Clear();
            BuildWaypoints();
            var guardian = new GameObject("VD_A6_Guardian").AddComponent<Guardian>();
            Object.DontDestroyOnLoad(guardian.gameObject);
            yield return Review.Shot($"a6-1101-dive{dive}-start");
            float start = Time.realtimeSinceStartup, nextLog = start;
            while (!Over() && Time.realtimeSinceStartup - start < LootBudget1101 && !TasksDone())
            {
                // At stress 90+ shadows (ShadowMonsterSpawn 900003 Dark Young, 900004) keep spawning and stunning: run a6-1101
                // stood 10 min in one spot fighting them at 100. A player leaves with what they have and dives again rested.
                if (Stress() >= 90f) { Flow.Log($"1101 dive {dive}: stress {Stress():F0}, heading out with tasks {Tasks()}"); break; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 30f; Flow.Log($"1101 dive {dive} t={Time.realtimeSinceStartup - start:F0} tasks {Tasks()} inventory={InventoryIds()} {MeState()}"); }
                if (State() == "Dying") { yield return new WaitForSecondsRealtime(1f); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the dive"); continue; }
                var m = RealEnemy(7f);
                if (m != null) { yield return Fight(m); continue; }
                var loot = NearestLoot();
                if (loot != null) { yield return Loot(loot); continue; }
                var me = Me();
                if (me == null || s_waypoints.Count == 0) { Flow.Log("1101: no waypoint left"); break; }
                var wp = s_waypoints.OrderBy(w => Flow.Flat(w - me.transform.position).sqrMagnitude).First();
                s_waypoints.Remove(wp);
                yield return Flow.Go(wp, 2f, 30f, "explore", run: true, stop: () => RealEnemy(6f) != null || NearestLoot() != null || Over());
            }
            Flow.Log($"1101 dive {dive} loot end after {Time.realtimeSinceStartup - start:F0}s tasks {Tasks()} done={TasksDone()} inventory={InventoryIds()} {MeState()}");
            if (!Over()) yield return TerminalEscape();
            yield return Flow.WaitUntil(() => Flow.Find("EscapeSuccessPopupView") != null || Flow.Find("EscapeFailPopupView") != null
                || Flow.Find("CampaignFailPopupView") != null || Flow.ActiveScene != "StageScene", 180f, "dive end banner");
            Flow.Log($"1101 dive {dive} end: top={Flow.TopPopup()} {MeState()}");
            if (Flow.Find("EscapeSuccessPopupView") != null) { yield return new WaitForSecondsRealtime(1f); yield return Review.Shot($"a6-1101-dive{dive}-escaped"); }
        }

        static float Stress() => Convert.ToSingle(Flow.Get(Me(), "Stress") ?? 0f);

        // Shadow monsters (TMonster MonsterType Shadow, 9000xx) are the stress hallucinations; a looting player runs past them.
        static Component RealEnemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => MonsterState(m) != "Dead" && MonsterState(m) != "Dying" && MonsterId(m) / 100000 != 9)
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        // Boxes anywhere (a player heads for the ones the map shows), loose drops and dead bodies close by (A5 IsLoot).
        static Component NearestLoot()
        {
            var me = Me();
            if (me == null) return null;
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && !s_looted.Contains(m.GetInstanceID()))
                .Select(m => (m, n: m.GetType().Name, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.n == "RewardBox" && x.d < 80f || x.n == "DropGoods" && x.d < 10f
                    || x.n == "MonsterBody" && x.d < 8f && Flow.Get(Flow.Get(x.m, "_monsterModel"), "State")?.ToString() == "Dead")
                .OrderBy(x => x.d).Select(x => (Component)x.m).FirstOrDefault();
        }

        // Walk up, hold F; a box or body opens the looting panel: take each slot as the search reveals it (A5 Loot).
        static IEnumerator Loot(Component target)
        {
            s_looted.Add(target.GetInstanceID());
            string kind = target.GetType().Name, name = target.name;
            Flow.Log($"loot {kind} {name} at {target.transform.position}");
            yield return GoFar(target.transform.position, 1.5f, $"loot {name}", 90f);
            if (!target || Over()) yield break;
            yield return Flow.WalkTo(() => target ? target.transform.position : (Vector3?)null, 0.9f, 8f, $"loot {name}");
            if (!target) yield break;
            Flow.Aim(target.transform.position);
            yield return Flow.WaitUntil(() => Flow.Focused(target), 3f, $"focus {name}");
            if (!Flow.LastWaitOk) { Flow.Log($"loot {name}: no focus ({Flow.Focus()})"); yield break; }
            if (kind == "RewardBox") yield return Shot(C.Intro);
            float hold = Flow.Get(target, "HoldingTime") is float h ? h + 0.3f : 0.8f;
            yield return Flow.Keys(Mathf.Clamp(hold, 0.3f, 3f), Key.F);
            if (kind == "DropGoods") { yield return new WaitForSecondsRealtime(0.8f); Flow.Log($"picked up {name}: inventory {InventoryIds()}"); yield break; }
            yield return Flow.WaitUntil(() => Flow.Find("LootingInventoryPanelPresenter") != null, 6f, $"{name} looting panel");
            var panel = Flow.Find("LootingInventoryPanelPresenter");
            if (panel == null) { Flow.Log($"loot {name}: no looting panel, focus={Flow.Focus()} state={State()}"); yield break; }
            var slots = Flow.Get(panel, "_goodsSlotModels") as IList;
            var taken = new HashSet<object>();
            float searchStart = Time.realtimeSinceStartup;
            while (slots != null && Time.realtimeSinceStartup - searchStart < 60f && Flow.Find("LootingInventoryPanelPresenter") != null && !Over() && State() != "Dying")
            {
                bool pending = false;
                foreach (var slot in slots.Cast<object>().ToList())
                {
                    if (taken.Contains(slot)) continue;
                    var gs = Flow.Value(slot, "RxGoodsSlot");
                    if (gs == null || Flow.Get(gs, "IsEmpty") is bool e && e) continue;
                    if (!(Flow.Get(gs, "IsRevealed") is bool r && r)) { pending = true; continue; }
                    Flow.Log($"loot slot {Flow.Get(gs, "Id")} x{Flow.Get(gs, "Count")}");
                    Fire(slot, "OnEnter", slot);
                    yield return null;
                    Fire(slot, "OnLeftClick", slot);
                    taken.Add(slot);
                    yield return new WaitForSecondsRealtime(0.4f);
                }
                if (!pending) break;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            Flow.Log($"loot {name} took {taken.Count} slots, tasks {Tasks()} inventory: {InventoryIds()}");
            for (int i = 0; i < 2 && (Flow.Find("LootingInventoryPanelPresenter") != null || Flow.Find("MenuPopupPresenter") != null || State() == "Interact"); i++)
            {
                yield return Flow.Keys(0.2f, i == 0 ? Key.Escape : Key.Tab);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        static string InventoryIds()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            var sb = new System.Text.StringBuilder();
            if (Flow.Get(inv, "Slots") is IEnumerable slots)
                foreach (var kv in slots)
                    if (Flow.Get(kv, "Value") is object gs && !(Flow.Get(gs, "IsEmpty") is bool e && e)) sb.Append($"{Flow.Get(gs, "Id")}x{Flow.Get(gs, "Count")},");
            return sb.Length == 0 ? "(empty)" : sb.ToString();
        }

        // NavMesh points 10 m apart across the stage (A5).
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

        // The nearest terminal (WaveExit): hold F to activate it (ExitActivateInteractionTime 2 s), hold out the wave
        // while it charges, stay in its field until the escape takes the diver (A5 Escape).
        static IEnumerator TerminalEscape()
        {
            var me = Me();
            var exit = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "WaveExit" && Flow.Get(m, "ExitState")?.ToString() != "Finished")
                .OrderBy(m => PathLength(m.transform.position)).FirstOrDefault();
            if (exit == null) { Flow.Log("1101: no terminal in the stage"); yield break; }
            string ExitNow() => exit ? $"{Flow.Get(exit, "ExitState")} remain={Flow.Get(exit, "StateRemainTime")}" : "gone";
            Flow.Log($"1101 terminal at {exit.transform.position} {ExitNow()} path={PathLength(exit.transform.position):F0}");
            float start = Time.realtimeSinceStartup;
            while (!Over() && exit && Time.realtimeSinceStartup - start < 300f && Flow.Get(exit, "ExitState")?.ToString() == "Deactivated")
            {
                yield return GoFar(exit.transform.position, 1.2f, "terminal", 120f);
                if (Over() || !exit) break;
                var m = RealEnemy(2.5f);
                if (m != null) { yield return Fight(m); continue; }
                yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 1.0f, 5f, "terminal");
                Flow.Aim(exit.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus terminal");
                yield return Flow.Keys(2.6f, Key.F);
                yield return new WaitForSecondsRealtime(0.5f);
                Flow.Log($"1101 terminal after F {ExitNow()} focus={Flow.Focus()} {MeState()}");
            }
            if (Over() || !exit || Flow.Get(exit, "ExitState")?.ToString() == "Deactivated") { Flow.Log($"1101 terminal not activated {ExitNow()} {MeState()}"); yield break; }
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Shot(C.Fight);
            float hold = Time.realtimeSinceStartup, nextLog = hold;
            while (!Over() && Time.realtimeSinceStartup - hold < 180f)
            {
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"1101 terminal {ExitNow()} {MeState()}"); }
                var m = RealEnemy(3.5f);
                if (m != null) { yield return Fight(m); continue; }
                if (exit && Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2.5f)
                    yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 1.5f, 3f, "back to the terminal");
                yield return new WaitForSecondsRealtime(0.3f);
            }
            Flow.Log($"1101 terminal end {ExitNow()} {MeState()} stage={Flow.StageState()}");
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
                // Walk in short legs and watch the booth: in 1104 (run 22) it vanished while the diver walked up to it.
                var goal = exit.transform.position;
                float walkStart = Time.realtimeSinceStartup;
                string lastState = Flow.Get(exit, "ExitState")?.ToString();
                while (exit && Time.realtimeSinceStartup - walkStart < 90f && Me() != null && Flow.Flat(goal - Me().transform.position).magnitude > 0.9f && !Over())
                {
                    yield return Flow.WalkTo(() => goal, 0.9f, 3f, "dive booth 1007", run: true);
                    if (exit) lastState = $"{Flow.Get(exit, "ExitState")} remain={Flow.Value(exit, "_syncStateRemainTime")} grace={Flow.Get(exit, "_isGraceTimerRunning")}";
                    Flow.Log($"booth walk: d={Flow.Flat(goal - Me().transform.position).magnitude:F1} exit={(exit ? lastState : "DESTROYED")} {MeState()}");
                    if (Flow.Flat(goal - Me().transform.position).magnitude <= 1.6f) break;
                }
                // The booth despawns as Finished once the escape went through (stage Ended): that is the escape, not a loss.
                if (!exit && Over()) { Flow.Log($"booth 1007 gone with the escape; last state {lastState}; stage={Flow.StageState()} {MeState()}"); break; }
                if (!exit)
                {
                    Flow.Log($"BUG? booth 1007 destroyed before the escape; last state {lastState}; stage={Flow.StageState()}");
                    exit = SafeExitNow();
                    if (exit == null) break;
                    attempt = -1;
                    continue;
                }
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
        static IEnumerator Results(bool wrapUp = true)
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
            if (!wrapUp) yield break;
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

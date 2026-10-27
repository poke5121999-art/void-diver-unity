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
    // the diver is under 1102's UserLevel:2 at the end, and the stress setter at 70+ as in the other A6 cases: the stress
    // comes from the hits the Recover lets the bot absorb (StressDamageOnHpDamaged, back attacks), and at 100 the shadow
    // monsters' crowd control pinned the diver for 10 minutes (runs a6-1101 #1, full9). No invincibility, no
    // SetClearedCampaign. It runs past shadow monsters (9000xx) when looting.
    //
    // StoryChain_1101 .. StoryChain_1202, StoryChain_AfterLast (gaps 2, 3, 8 of the whole-demo review): ordered checkpoint cases on
    // ONE profile that only the game writes (no SetClearedCampaign, no quest accept/complete cheat). 1101 loads the post-tutorial
    // fixture (after a field-by-field comparison with the A4 export, FixtureIgnore), each later case loads the saves the case before
    // it exported to captures/review/chain-saves/runs/<RUN_ID>/<case>/, asserts that clear and its main lounge quests from the loaded
    // profile, plays its campaign and the main quests it opens (80100 by selling the owner's relic to Arthur and the owner's own LevelUp),
    // exports. Provenance: LINK = RUN_ID + last case; a case refuses a link from another run or case (VD_CHAIN_RESUME=1 is a loud
    // debugging opt-in); a failed case makes the following ones fail at that check. chain-saves/GREEN names the run whose AfterLast
    // passed (Flow.ChainSaveDir). Before each export the case calls PlayerManager.SaveAsync: SetClearedLoungeQuest does not mark the
    // profile dirty (original behaviour), so the game's autosave alone would leave the last quest clears out of the file.
    // Cheats in the chain (the game's own, logged "cheat ..."): HP Recover (the Guardian refills every 0.15 s; floor 0.9 in 1201's
    // wave), the stress setter at 70+, ReqCheatLevelUp for the level part of the next story's unlock (1102 and later; 1101's level 2 is
    // the owner's LevelUp, paid with the relic's coins), invincibility only after a boss attack took HP off the diver, and
    // ReqCheatApplyMonsterHp Hp=1 only as a safety net after a per-phase budget of HpMax / measured dps x 1.3 (at least PhaseBudget).
    // Gap 3 gates, per boss fight: boss damage on the diver > 0 before invincibility, bot damage > 0 on every phase before the HP
    // cheat and at the end; proofs are per TableId, so a phase change on the same unit (1102, 1105) starts a new proof.
    // A dive that dies is retried (<= 3 attempts, logged, the same place twice fails); a dive counts only with the Escape Success popup.
    // Not pressed on the title: Wishlist (steam://store/3438920) and Quit (Application.Quit) leave the process.
    // Per-boss proof numbers: see the a6-chain run log lines "fight proof".

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
            // The fixture again: the story chain leaves the demo's end profile in SavesDir, and these cases start from the post-tutorial one.
            yield return FromTitle(seedFixture: true);
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();

            // Lounge quest 80200 first: Elara's menu opens only after it (A5).
            yield return Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Talk(id, $"{who} 80200");
            yield return Talk(700000, "Elara 80200 finish");
            yield return CampaignBody(campaignId);
        }

        // One campaign from the lounge to its cleared, saved flag and the next one in Elara's list. StoryChain calls it
        // with s_chain set (no SetClearedCampaign: the profile carries the earlier clears).
        static IEnumerator CampaignBody(int campaignId)
        {
            C = Bosses.First(b => b.Id == campaignId);
            s_shots.Clear();
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
            StartGuardian();
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
            AssertEscaped($"campaign {CampaignId}");
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the dive did not end back in the lounge (booth escape)");
            var pmEnd = Flow.Find("PlayerManager");
            Assert.IsTrue(pmEnd?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pmEnd, new object[] { CampaignId }) is bool done && done, $"campaign {CampaignId} not cleared after Elara's wrap-up");
            // The saved profile holds the clear and Elara's list shows the next story open (its UserLevel part is the
            // grind, a level cheat).
            yield return SavedAndListed(CampaignId, StoryOrder.SkipWhile(x => x != CampaignId).Skip(1).FirstOrDefault());
        }

        // Story campaigns in unlock order (TCampaign UnlockConditions CampaignCleared chain).
        static readonly int[] StoryOrder = { 1101, 1102, 1103, 1104, 1105, 1201, 1202 };

        // ---- lounge ----

        static string s_lastLoadFromDisk = "-";

        // Every case starts at the title and presses Continue. A case that ended inside a room (run full8: 1201's diver lay
        // Dead in the stage when its assert fired) is left the way the system menu's Return to Title leaves
        // (SystemPagePresenter.ReturnToTitleButtonClick: ClearStageResult, RxConnectState BackToTitle, host CloseRoom, then
        // InGameScene.OnGameClosed loads StartScene). The old LoadScene(0) skipped the room shutdown, so the next Continue
        // landed in a lounge still holding campaign 1201's Playing room with the diver stuck. seedFixture: copy the
        // post-tutorial saves back in and drop the cached profile (as PlayerManager.DeletePlayer does), so Continue's
        // LoadPlayerAsync reads PlayerContext_0.sav like a fresh launch.
        static bool InRoom => Flow.ActiveScene == "LoungeScene" || Flow.ActiveScene == "StageScene";

        // The system menu's Return to Title when the game is in a room; nothing when it is already on the title.
        static IEnumerator ReturnToTitle()
        {
            bool inRoom = InRoom;
            if (inRoom)
            {
                var gm = Flow.Find("GameManager");
                Flow.Log($"return to title from {Flow.ActiveScene} {Flow.LoadState()} top={Flow.TopPopup()}");
                gm?.GetType().GetMethod("ClearStageResult")?.Invoke(gm, null);
                var channel = Flow.Get(gm, "_gameChannel");
                var rx = Flow.Get(channel, "RxConnectState");
                var valueProp = rx?.GetType().GetProperty("Value");
                valueProp?.SetValue(rx, Enum.ToObject(valueProp.PropertyType, 5));   // ERoomConnectState.BackToTitle
                var host = Flow.Get(gm, "_gameHost");
                if (Flow.Get(gm, "IsHost") is bool isHost && isHost) host?.GetType().GetMethod("CloseRoom")?.Invoke(host, null);
                else channel?.GetType().GetMethod("LeftRoom")?.Invoke(channel, null);
                yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 60f, "title after Return to Title");
                Assert.IsNotNull(Flow.Find("StartSceneView"), $"Return to Title did not reach the title: {Flow.ActiveScene}");
            }
        }

        static IEnumerator FromTitle(bool seedFixture, string savesDir = null)
        {
            bool inRoom = InRoom;
            yield return ReturnToTitle();
            if (seedFixture)
            {
                if (!inRoom) { UnityEngine.SceneManagement.SceneManager.LoadScene(0); yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 120f, "title"); }
                yield return new WaitForSecondsRealtime(2f);   // a lounge autosave in flight lands before the copy
                Flow.SeedSaves(savesDir ?? Fixture);
                var pm = Flow.Find("PlayerManager");
                (Flow.Get(pm, "_playerDisposable") as IDisposable)?.Dispose();
                pm?.GetType().GetField("_playerDisposable", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pm, null);
                (Flow.Get(pm, "_playerContext") as IDisposable)?.Dispose();
                pm?.GetType().GetField("_playerContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pm, null);
                Flow.SavedCleared(Campaign1101, out s_lastLoadFromDisk);
                Flow.Log($"seeded {savesDir ?? Fixture}; cached profile dropped; PlayerContext_0.sav: {s_lastLoadFromDisk}");
                yield return Flow.ContinueToLounge(loadTitle: false);
            }
            else yield return Flow.ContinueToLounge(loadTitle: !inRoom);
        }

        static IEnumerator Talk(int npcId, string what)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"talk {what}: npc {npcId} not in the lounge"); yield break; }
            // Other NPCs stand next to some givers (the owner beside Oldry 700011): stand where the giver is the focused object.
            var near = Flow.Npcs().Where(n => n && n != npc && (n.transform.position - npc.transform.position).sqrMagnitude < 25f).ToList();
            Flow.Log($"{what}: giver at {npc.transform.position}, near: {string.Join(", ", near.Select(n => Flow.NpcId(n) + "@" + n.transform.position))}");
            bool focused = false;
            foreach (var off in new[] { new Vector3(0, 0, -1.3f), new Vector3(0, 0, 1.3f), new Vector3(1.3f, 0, 0), new Vector3(-1.3f, 0, 0) })
            {
                if (!npc) break;
                yield return Flow.WalkTo(() => npc ? npc.transform.position + off : (Vector3?)null, 0.4f, 12f, $"{what} stand {off}");
                Flow.Aim(npc.transform.position + Vector3.up * 0.8f);
                yield return new WaitForSecondsRealtime(0.5f);
                if (focused = Flow.Focused(npc)) break;
                Flow.Log($"{what}: focus={Flow.Focus()} at offset {off}");
            }
            if (focused) { Flow.Log($"interact {what} focus={Flow.Focus()}"); yield return Flow.Keys(0.7f, Key.F); }
            else yield return Flow.InteractNpc(npc, what);
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
            foreach (var id in s_chain ? Enumerable.Empty<int>() : StoryOrder.TakeWhile(x => x != CampaignId))
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
            // A round of Elara's lines (one per reputation quest 80002..80007 a level-up added) is not a failed attempt:
            // a fixture raised to level 7 has up to six of them before her menu.
            int dialogRounds = 0;
            for (int attempt = 0; attempt < 5 && Flow.Find("CampaignPopupPresenter") == null; attempt++)
            {
                var elara = Flow.FindNpc(700000);
                {
                    var me = Flow.MyCharacter();
                    var path = new UnityEngine.AI.NavMeshPath();
                    bool ok = me && elara && UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, elara.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
                    Flow.Log($"to Elara: attempt {attempt} me={(me ? me.transform.position.ToString() : "-")} {MeState()} top={Flow.TopPopup()} focus={Flow.Focus()} path ok={ok} status={path.status} corners={path.corners.Length} " +
                        $"npcs near me: {string.Join(", ", Flow.Npcs().Where(n => me && (n.transform.position - me.transform.position).sqrMagnitude < 36f).Select(n => Flow.NpcId(n) + "@" + n.transform.position))}");
                    // Esc only over a popup (NewContentPopupView after a level-up takes the F): on a bare lounge it opens the
                    // game menu, which took the next F (a6-full10: 1103/1104/1105/1201/1202 "did not start", MenuPopupView
                    // on every second attempt).
                    for (int i = 0; i < 3 && Flow.TopPopup() != null && Flow.Find("DialogPopupView") == null; i++)
                    {
                        yield return Flow.Keys(0.2f, Key.Escape);
                        yield return new WaitForSecondsRealtime(0.5f);
                    }
                }
                yield return Flow.InteractNpc(elara, "Elara campaign");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("CampaignPopupPresenter") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara before her menu");
                    yield return new WaitForSecondsRealtime(1f);
                    if (++dialogRounds <= 10) attempt--;
                    Flow.Log($"Elara dialog round {dialogRounds} before her menu (attempt {attempt + 1} of 5 next)");
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
        const float HpFloor = 0.7f;
        static float s_hpFloor = HpFloor, s_lastRecover;
        static bool s_stressRecover = true;

        static void KeepAlive()
        {
            if (Me() == null || Over() || State() == "Dying") return;
            // Every frame from the Guardian; one request per 0.1 s while the host applies it. Run full8: ten Monitormen
            // (Atk 200) took 474 HP in 1.5 s past the old 0.6 floor checked every 0.15 s.
            if (HpFrac() < s_hpFloor && Time.realtimeSinceStartup - s_lastRecover > 0.1f)
            {
                s_lastRecover = Time.realtimeSinceStartup;
                SendCheat("ReqSetCharacterHp", ("Hp", HpMax()));
            }
            if (!s_stressRecover) return;
            // Stress 100 keeps summoning shadow Dark Youngs (ShadowMonsterSpawn) and stuns the diver; a player rests it
            // off between fights, here it is the recover cheat (run story3: the 1104 painting never took the F).
            if (Convert.ToSingle(Flow.Get(Me(), "Stress") ?? 0f) >= 70f) SendCheat("ReqSetCharacterStress", ("Stress", 0f));
        }

        // How the dive ended, as the game's popups say (EscapeSuccessPopupView / EscapeFailPopupView): a death also lands in the lounge.
        static bool s_escapeOk, s_escapeFail;
        static int s_bigHits;
        static IDisposable s_hitWatch;
        static float s_nextPopupPoll;

        static void StartGuardian()
        {
            s_escapeOk = s_escapeFail = false; s_bigHits = 0; s_nextPopupPoll = 0;
            s_hitWatch?.Dispose();
            // Any single hit that takes a quarter of the diver's HP is logged with its source (chain12/15: 1201's diver went from
            // >= 70% HP to Dead inside one refill interval).
            s_hitWatch = WatchDamage(Me(), e =>
            {
                int amount = Flow.Get(e, "Amount") is int a ? a : 0;
                if (amount < HpMax() * 0.25f) return;
                var src = Flow.Get(e, "SourceModel") as Component;
                s_bigHits++;
                Flow.Log($"BIG HIT {amount} on the diver (hp now {Flow.Hp(Me())}/{HpMax():F0}) from {(src ? src.name : "?")} id={(src ? MonsterId(src) : 0)} skill={(src ? Flow.Get(Model(src), "UsingSkillId") : null)} event skill={Flow.Get(e, "SkillId")} hitbox={Flow.Get(e, "HitBoxId")} reason={Flow.Get(e, "ReasonType")} sourceTable={Flow.Get(Flow.Get(e, "SourceModel"), "TableId")} state={MeState()}");
            });
            Object.DontDestroyOnLoad(new GameObject("VD_A6_Guardian").AddComponent<Guardian>().gameObject);
        }

        class Guardian : MonoBehaviour
        {
            void Update()
            {
                if (Flow.ActiveScene != "StageScene") { Destroy(gameObject); return; }
                KeepAlive();
                if (Time.realtimeSinceStartup < s_nextPopupPoll) return;
                s_nextPopupPoll = Time.realtimeSinceStartup + 0.15f;
                if (Flow.Find("EscapeSuccessPopupView") != null) s_escapeOk = true;
                if (Flow.Find("EscapeFailPopupView") != null) s_escapeFail = true;
            }
        }

        static void AssertEscaped(string what)
        {
            Flow.Log($"{what}: escape popups success={s_escapeOk} fail={s_escapeFail}, big hits={s_bigHits}");
            Assert.IsFalse(s_escapeFail, $"{what}: the dive ended on the Escape Failed popup (the diver died)");
            Assert.IsTrue(s_escapeOk, $"{what}: no Escape Success popup, the dive did not end in an escape");
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
            int portalTries = 0;
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
                yield return Flow.InteractObject(portal, 0.8f, "portal", portalTries++ == 0 ? 1.0f : 0.4f);   // the sensor is small: closer after a miss (run chain13 stood 0.94 m, no focus)
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
        static int s_hurtAtInvincible;
        static string s_lastBossSeen;

        // ---- proof that the fight works before a grind cheat stands in (whole-demo review gap 3) ----

        // HpDamageEvent as the game's own UnitController event broker publishes it (HandleUnitEvent), per unit.
        class DamageObserver<T> : IObserver<T>
        {
            readonly Action<object> _on;
            public DamageObserver(Action<object> on) { _on = on; }
            public void OnNext(T value) { try { _on(value); } catch (Exception) { } }
            public void OnError(Exception error) { }
            public void OnCompleted() { }
        }

        static IDisposable WatchDamage(Component unit, Action<object> on)
        {
            var controller = unit == null ? null : unit.GetComponents<Component>().FirstOrDefault(c => c != null && Flow.Get(c, "_eventMessageBroker") != null);
            var broker = Flow.Get(controller, "_eventMessageBroker");
            var evType = GameType("HpDamageEvent");
            if (broker == null || evType == null) return null;
            var observable = broker.GetType().GetMethod("Receive")?.MakeGenericMethod(evType).Invoke(broker, null);
            var observer = Activator.CreateInstance(typeof(DamageObserver<>).MakeGenericType(evType), on);
            return typeof(IObservable<>).MakeGenericType(evType).GetMethod("Subscribe").Invoke(observable, new[] { observer }) as IDisposable;
        }

        // Per boss phase: what the bot's own hits took off the boss, and what the boss's attacks took off the diver.
        class FightProof { public int AtCheat, Phase, BossId, HpMax, BotHits, BotDamage, BossHits, BossDamage, DiverHpBefore, DiverHpLow = int.MaxValue; }
        static readonly List<FightProof> s_proofs = new List<FightProof>();
        static FightProof s_proof;
        static IDisposable s_bossWatch, s_diverWatch;
        static Component s_watchedBoss;

        static void WatchPhase(Component boss, int phase)
        {
            // A phase is a new proof when the monster TableId changes, also on the same unit (1102, 1105 change phase in place).
            if (boss == s_watchedBoss && s_proof != null && s_proof.BossId == MonsterId(boss)) return;
            s_bossWatch?.Dispose();
            s_watchedBoss = boss;
            var me = Me();
            s_proof = new FightProof { Phase = phase + 1, BossId = MonsterId(boss), HpMax = Flow.Get(Model(boss), "HpMax") is float hm ? (int)hm : Flow.Hp(boss), DiverHpBefore = Flow.Hp(me) };
            s_proofs.Add(s_proof);
            var proof = s_proof;
            var bossGo = boss.gameObject;
            s_bossWatch = WatchDamage(boss, e =>
            {
                if (Flow.Get(e, "SourceModel") is Component src && src && src.gameObject == (Me() ? Me().gameObject : null))
                { proof.BotHits++; proof.BotDamage += Flow.Get(e, "Amount") is int a ? a : 0; }
            });
            if (s_diverWatch == null)
                s_diverWatch = WatchDamage(me, e =>
                {
                    var src = Flow.Get(e, "SourceModel") as Component;
                    var cur = s_proof;
                    if (cur == null || !src || s_watchedBoss == null || src.gameObject != s_watchedBoss.gameObject) return;
                    cur.BossHits++; cur.BossDamage += Flow.Get(e, "Amount") is int a ? a : 0;
                    cur.DiverHpLow = Mathf.Min(cur.DiverHpLow, Flow.Hp(Me()));
                });
            Flow.Log($"proof: watching phase {phase + 1} boss {s_proof.BossId} hpMax={s_proof.HpMax} boss events={(s_bossWatch != null)} diver events={(s_diverWatch != null)}");
        }

        static string ProofLine(FightProof f) =>
            $"phase {f.Phase} boss {f.BossId} hpMax={f.HpMax}: bot hits={f.BotHits} damage={f.BotDamage}; boss hits on the diver={f.BossHits} damage={f.BossDamage} (diver hp {f.DiverHpBefore} -> low {(f.DiverHpLow == int.MaxValue ? -1 : f.DiverHpLow)})";

        static void EndProofs()
        {
            s_bossWatch?.Dispose(); s_diverWatch?.Dispose();
            s_bossWatch = s_diverWatch = null; s_watchedBoss = null; s_proof = null;
            foreach (var f in s_proofs) Flow.Log($"fight proof {CampaignId}: {ProofLine(f)}");
        }

        static IEnumerator FightBoss()
        {
            s_proofs.Clear();
            try { yield return FightBossLoop(); }
            finally { EndProofs(); }
        }

        static IEnumerator FightBossLoop()
        {
            s_lastSkill = 0; s_summonWaitStart = -1f; s_invincible = false; s_hurtAtInvincible = 0; s_lastBossSeen = null;
            float start = Time.realtimeSinceStartup, nextLog = start;
            int phase = -1, clicks = 0;
            float phaseStart = start;
            int phaseDamage0 = 0;   // the phase's BotDamage when its budget clock (re)started
            bool cheated = false, holdOffPrev = false;
            while (!Over() && Time.realtimeSinceStartup - start < 1200f)
            {
                var boss = Boss();
                if (boss == null)
                {
                    // Between phases (the dead body becomes the next phase) or the end.
                    Flow.Log($"boss gone: last hp={s_proof?.HpMax}; monsters now: {string.Join(", ", Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(m => m != null && m.GetType().Name == "MonsterController").Select(m => $"{MonsterId(m)} active={m.gameObject.activeInHierarchy} hp={Flow.Hp(m)} {MonsterState(m)} @{m.transform.position}"))}; me={MeState()}");
                    yield return Flow.WaitUntil(() => Boss() != null && Flow.Hp(Boss()) > 0 || Over(), 30f, "next phase");
                    if (Boss() == null)
                    {
                        if (Over()) break;
                        // The last phase's body stays in the stage Dead; a boss that is gone with HP left is a failure, not a win.
                        Assert.Fail($"boss vanished: last seen {s_lastBossSeen}");
                    }
                    continue;
                }
                s_lastBossSeen = $"{MonsterId(boss)} hp={Flow.Hp(boss)} state={MonsterState(boss)} skill={Flow.Get(Model(boss), "UsingSkillId")} @{boss.transform.position} t={Time.realtimeSinceStartup - start:F0}s";
                int id = MonsterId(boss), p = Array.IndexOf(BossPhases, id);
                // The last phase's body stays (bosses keep their loot), so the fight ends on its Dead state.
                if (p == BossPhases.Length - 1 && (MonsterState(boss) == "Dead" || Flow.Hp(boss) <= 0)) break;
                WatchPhase(boss, p);
                if (p != phase)
                {
                    Flow.Log($"boss phase {p}: {BossState()} hud={BossHud()} after {Time.realtimeSinceStartup - start:F0}s {MeState()}");
                    phase = p; phaseStart = Time.realtimeSinceStartup; phaseDamage0 = s_proof?.BotDamage ?? 0; cheated = false;
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
                // Invincibility only once a boss attack has taken HP off the diver (gap 3): the fight hurts before the cheat.
                if (shotsDone && !s_invincible && s_proofs.Any(f => f.BossDamage > 0))
                {
                    var hurt = s_proofs.First(f => f.BossDamage > 0);
                    Flow.Log($"proof before invincibility: {ProofLine(hurt)}");
                    s_invincible = true;
                    s_hurtAtInvincible = s_proofs.Sum(f => f.BossDamage);
                    SendCheat("ReqCheatApplyCharacterOptions", ("Invincibility", true));
                }
                bool phaseShotsDone = !(C.Id == 1102 && phase == 1 && !s_shots.Contains(C.Fight2)) && !(phase == 0 && !s_shots.Contains(C.Fight));
                // Budget from the bot's measured damage per second (>= PhaseBudget): HpMax / dps x 1.8. The monster-HP cheat is a safety net;
                // x1.3 cut 1105 off at 19% and 4% of the phase HP left (a6-chain20), while the diver is invincible by then anyway.
                float budget = PhaseBudget;
                if (inPhase > 20f && s_proof.BotDamage > phaseDamage0)
                    budget = Mathf.Max(PhaseBudget, s_proof.HpMax / ((s_proof.BotDamage - phaseDamage0) / inPhase) * 1.8f);
                if (!cheated && inPhase > budget && phaseShotsDone && Flow.Hp(boss) > 1)
                {
                    cheated = true;
                    Flow.Log($"phase {p + 1} still up after {inPhase:F0}s (budget {budget:F0}s): {BossState()}; cheated with {100f * Flow.Hp(boss) / Mathf.Max(1, s_proof.HpMax):F0}% of the phase HP left");
                    // Gap 3: the bot's own hits took HP off this phase before the monster-HP cheat ends it.
                    Flow.Log($"proof before the monster HP cheat: {ProofLine(s_proof)}");
                    // Phase 2 of a boss can be the same unit (HP refilled, max raised), so count the bot's damage since the last cheat.
                    Assert.Greater(s_proof.BotDamage, s_proof.AtCheat, $"phase {p + 1}: the bot's hits did not lower the boss's HP before the monster HP cheat");
                    s_proof.AtCheat = s_proof.BotDamage;
                    SendCheat("ReqCheatApplyMonsterHp", ("Hp", 1));
                }
                // The wait for the summon (holdOff) does not count against the phase budget.
                if (holdOffPrev && !holdOff) { phaseStart = Time.realtimeSinceStartup; phaseDamage0 = s_proof.BotDamage; Flow.Log($"phase {p + 1}: summon wait over, budget restarts"); }
                holdOffPrev = holdOff;
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
            // The 1200 s cap ends the loop with the boss still up: a failure with its state, not a fight that went on.
            if (!Over() && Time.realtimeSinceStartup - start >= 1200f && Boss() != null && Flow.Hp(Boss()) > 0)
                Assert.Fail($"fight budget 1200 s spent: boss={BossState()} {MeState()}");
            // Gap 3 gates, at the end of the fight: the boss hurt the diver before invincibility was switched on, and the
            // bot's hits took HP off every phase it fought.
            Assert.Greater(s_proofs.Sum(f => f.BossDamage), 0, "no boss attack hurt the diver in the whole fight");
            if (s_invincible) Assert.Greater(s_hurtAtInvincible, 0, "invincibility was switched on before any boss attack hurt the diver");
            foreach (var f in s_proofs) Assert.Greater(f.BotDamage, 0, $"phase {f.Phase} (boss {f.BossId}): the bot's hits never lowered the boss's HP");
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
            // Wave 12011 (11 monsters on a level-6 diver in a chain run: 479 -> 0 HP inside one Recover interval) needs the refill earlier.
            s_hpFloor = 0.9f;
            try { yield return Story1201Body(); }
            finally { s_hpFloor = HpFloor; }
        }

        static IEnumerator Story1201Body()
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
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6";
            yield return Start1101();
            yield return Body1101();
        }

        // The post-tutorial profile read from disk at Continue, and Elara's tour (80200).
        static IEnumerator Start1101(string seed = null)
        {
            {
                // Earlier cases cheat 1101 cleared and the lounge autosave writes it: seed the post-tutorial fixture again
                // and make Continue read it from disk (FromTitle drops the cached profile).
                var pmBefore = Flow.Find("PlayerManager");
                var ctxBefore = Flow.Get(pmBefore, "_playerContext");
                bool clearedBefore = ctxBefore != null && pmBefore.GetType().GetMethod("IsClearedCampaign")?.Invoke(pmBefore, new object[] { Campaign1101 }) is bool cb && cb;
                yield return FromTitle(seedFixture: true, savesDir: seed);
                Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
                Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
                Flow.RouteInputToGame();
                Assert.IsFalse(Flow.SavedCleared(Campaign1101, out var before), $"the seeded save ({seed ?? Fixture}) already has 1101 cleared ({before})");
                Assert.IsTrue(!before.StartsWith("read threw") && !before.StartsWith("no "), $"the fixture save could not be read: {before}");
                var pmNow = Flow.Find("PlayerManager");
                var ctxNow = Flow.Get(pmNow, "_playerContext");
                bool clearedNow = pmNow.GetType().GetMethod("IsClearedCampaign")?.Invoke(pmNow, new object[] { Campaign1101 }) is bool cn && cn;
                Flow.Log($"1101 profile: before Continue {(ctxBefore == null ? "none" : $"#{ctxBefore.GetHashCode()} cleared1101={clearedBefore}")}, after #{ctxNow?.GetHashCode()} cleared1101={clearedNow} level={Flow.Get(pmNow, "Level")} (loaded from PlayerContext_0.sav: {s_lastLoadFromDisk})");
                Assert.IsTrue(ctxNow != null && !ReferenceEquals(ctxNow, ctxBefore), "Continue kept the cached profile instead of loading PlayerContext_0.sav");
                Assert.IsFalse(clearedNow, "the loaded profile has 1101 cleared");
                Flow.Log($"1101 start: saved {before}; level={Flow.Get(Flow.Find("PlayerManager"), "Level")}");

                yield return Talk(700000, "Elara 80200 start");
                foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                    yield return Talk(id, $"{who} 80200");
                yield return Talk(700000, "Elara 80200 finish");
            }
        }

        // 1101 from Elara's list to its saved clear and 1102 open in the list (also the chain's first link).
        static IEnumerator Body1101()
        {
            C = Bosses.First(b => b.Id == Campaign1101);
            s_shots.Clear();
            s_hpFloor = 0.35f;
            try
            {
                yield return StartCampaign();
                Assert.AreEqual("1101", CampaignNow(), "campaign 1101 did not start");
                yield return Briefing();

                for (int dive = 1; dive <= MaxDives1101 && !TasksDone(); dive++)
                {
                    yield return EnterBooth();
                    Assert.AreEqual("StageScene", Flow.ActiveScene, $"dive {dive}: the dive booth did not lead into the stage");
                    yield return Dive1101(dive);
                    Assert.AreNotEqual("Dead", State(), $"dive {dive}: the diver died");
                    AssertEscaped($"1101 dive {dive}");
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

                yield return SavedAndListed(Campaign1101, s_chain ? 0 : Campaign1102);
                Assert.IsEmpty(new[] { C.Intro, C.Fight, C.Fight2 }.Where(k => !s_shots.Contains(k)).ToList(), "1101 shots missing");
            }
            finally
            {
                s_hpFloor = HpFloor;
            }
        }

        // ---- after a clear: the saved profile and Elara's list ----

        static bool s_chain, s_relicTalked, s_relicSold;
        static DateTime s_lastQuestClearUtc;

        static string SlotState(MonoBehaviour slot)
        {
            var view = Flow.Get(slot, "_view");
            var lockIcon = Flow.Get(view, "LockIcon") as Component;
            var done = Flow.Get(view, "CompletedMark") as Component;
            var button = Flow.Get(view, "Button") as Component;
            var locked = Flow.Get(button ? button.GetComponent("ButtonController") : null, "_isLocked");
            return $"lockIcon={(lockIcon ? lockIcon.gameObject.activeSelf.ToString() : "-")} locked={locked} completed={(done ? done.gameObject.activeSelf.ToString() : "-")}";
        }

        static MonoBehaviour Slot(List<MonoBehaviour> slots, int id) =>
            slots.FirstOrDefault(x => Flow.Get(Flow.Get(x, "_model"), "CampaignId") is int c && c == id);

        // PlayerContext_0.sav (the lounge autosave) holds `id` cleared; `next` (0: none) unlocks once the diver has its
        // level (ReqCheatLevelUp, the grind) and shows open in Elara's list, `id` with its completed mark.
        static IEnumerator SavedAndListed(int id, int next)
        {
            string saved = "-";
            yield return Flow.WaitUntil(() => Flow.SavedCleared(id, out saved), 120f, $"{id} cleared in PlayerContext_0.sav");
            Flow.Log($"{id} saved: {saved}");
            Assert.IsTrue(Flow.SavedCleared(id, out saved), $"{id} not cleared in the saved profile: {saved}");
            var pm = Flow.Find("PlayerManager");
            if (next != 0)
            {
                var table = GameType("GameCommon.Table.TCampaign")?.GetProperty("Table")?.GetValue(null);
                var row = table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { next });
                var conditions = Flow.Get(row, "UnlockConditions");
                bool Met() => pm?.GetType().GetMethod("CheckStateConditions")?.Invoke(pm, new[] { conditions }) is bool m && m;
                for (int i = 0; i < 6 && !Met(); i++)
                {
                    SendCheat("ReqCheatLevelUp");
                    yield return new WaitForSecondsRealtime(1f);
                }
                Flow.Log($"next story {next}: unlock {string.Join("|", (conditions as Array)?.Cast<object>() ?? Enumerable.Empty<object>())} met={Met()} level={Flow.Get(pm, "Level")}");
                Assert.IsTrue(Met(), $"next story {next} still locked after {id} cleared");
            }
            yield return OpenCampaignPopup();
            Assert.IsNotNull(Flow.Find("CampaignPopupPresenter"), "Elara's campaign list did not open");
            yield return new WaitForSecondsRealtime(2f);
            var slots = CampaignSlots();
            foreach (var sl in slots) Flow.Log($"campaign slot {Flow.Get(Flow.Get(sl, "_model"), "CampaignId")}: {SlotState(sl)}");
            yield return Shot($"a6-{id}-campaign-list");
            var mine = Slot(slots, id);
            if (mine != null) Assert.IsTrue(SlotState(mine).EndsWith("completed=True"), $"{id} slot has no completed mark: {SlotState(mine)}");
            if (next != 0)
            {
                var nextSlot = Slot(slots, next);
                Assert.IsNotNull(nextSlot, $"no {next} slot in Elara's campaign list");
                Assert.AreEqual("lockIcon=False locked=False", string.Join(" ", SlotState(nextSlot).Split(' ').Take(2)), $"{next} slot is locked: {SlotState(nextSlot)}");
            }
            yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        // ---- the story chain: ordered checkpoint cases (1101 -> 1202 -> after the last story) ----

        // One profile, written only by the game: no SetClearedCampaign, no quest cheat. Each case loads the save the case
        // before it left (captures/review/chain-saves/<case>/), asserts that clear and its main quests from it, plays its
        // campaign and the main lounge quests it opens, waits for the game's own autosave and exports the context saves for
        // the next case. A link file (RUN_ID + last finished case) makes a case refuse a save from another run or from a
        // case that did not pass ("previous link not from this run"); the cases after a failed one fail at that check
        // without playing. VD_CHAIN_RESUME=1 (logged loudly, never for the proof run) lets a single case take the link of
        // an earlier run. A dive that dies is retried (a player would): up to 3 attempts per case, the count and the
        // place of each death are logged, a repeated death at the same place fails the case, and a death never counts as
        // a clear or an escape (AssertEscaped).
        static readonly string[] ChainNames = { "1101", "1102", "1103", "1104", "1105", "1201", "1202", "AfterLast" };
        static string s_runId;
        static readonly List<string> s_chainReport = new List<string>();

        static string ProjectPath(params string[] parts) =>
            System.IO.Path.Combine(new[] { System.IO.Directory.GetParent(Application.dataPath).FullName }.Concat(parts).ToArray());

        // runs/<RUN_ID>/ holds this run's links; Flow.ChainSavesPath/ChainSaveDir are the shared API (A9a reads the green run).
        static string RunDir => Flow.ChainSavesPath("runs", s_runId);
        static readonly string[] ContextSaves = { "PlayerContext_0.sav", "AccountContext.sav", "GamePlaySettingContext.sav", "AccountRecordContext.sav" };

        [UnityTest, Order(1001), Timeout(9000000)] public IEnumerator StoryChain_1101() { yield return Chain("1101"); }
        [UnityTest, Order(1002), Timeout(9000000)] public IEnumerator StoryChain_1102() { yield return Chain("1102"); }
        [UnityTest, Order(1003), Timeout(9000000)] public IEnumerator StoryChain_1103() { yield return Chain("1103"); }
        [UnityTest, Order(1004), Timeout(9000000)] public IEnumerator StoryChain_1104() { yield return Chain("1104"); }
        [UnityTest, Order(1005), Timeout(9000000)] public IEnumerator StoryChain_1105() { yield return Chain("1105"); }
        [UnityTest, Order(1006), Timeout(9000000)] public IEnumerator StoryChain_1201() { yield return Chain("1201"); }
        [UnityTest, Order(1007), Timeout(9000000)] public IEnumerator StoryChain_1202() { yield return Chain("1202"); }
        [UnityTest, Order(1008), Timeout(9000000)]
        public IEnumerator StoryChain_AfterLast()
        {
            yield return Chain("AfterLast");
            // Reached only when every check of the case and the game-error check passed: the pointer A9a follows.
            System.IO.File.WriteAllText(Flow.ChainSavesPath("GREEN"), s_runId);
            Flow.Log($"chain run {s_runId} is green: chain-saves/GREEN written; {string.Join("; ", s_chainReport)}");
        }

        // The case's link becomes the next case's input only after the game-error check passed too: a case whose game logged
        // an exception after its export (a6-chain20: 1105 red on a NullReferenceException, 1201 still went on) leaves no LINK.
        static IEnumerator Chain(string name)
        {
            yield return Flow.GameLog.Run($"StoryChain_{name}", ChainCase(name));
            if (name == "AfterLast") yield break;   // the last case exports nothing; its caller writes GREEN
            string pending = System.IO.Path.Combine(RunDir, "LINK.pending"), link = System.IO.Path.Combine(RunDir, "LINK");
            Assert.IsTrue(System.IO.File.Exists(pending), $"chain {name}: passed but wrote no pending link");
            if (System.IO.File.Exists(link)) System.IO.File.Delete(link);
            System.IO.File.Move(pending, link);
            Flow.Log($"chain {name}: link committed after the game-error check");
        }

        static string ReadLink(out string runId, out string lastCase)
        {
            runId = lastCase = null;
            string path = System.IO.Path.Combine(RunDir, "LINK");
            if (!System.IO.File.Exists(path)) return "no LINK file";
            var lines = System.IO.File.ReadAllLines(path);
            if (lines.Length < 2) return "LINK file is malformed";
            runId = lines[0]; lastCase = lines[1];
            return null;
        }

        // Fields of the saves that legitimately differ between the fixture and a fresh A4 export (the 10-26 04:40 STATUS entry:
        // "differ only in creation time, play time, monsters killed and the randomly drawn employees"). Each entry has its reason;
        // the list is logged, it documents how the fixture differs from A4.
        static readonly (string path, string reason)[] FixtureIgnore =
        {
            ("SaveTimeTicks", "time of the last save"),
            ("CreateTimeTicks", "creation time of the profile"),
            ("PlayTimeTicks", "play time"),
            ("MonsterRecords", "monsters killed, differ with each tutorial run"),
            ("RecruitableEmployees", "randomly drawn employees"),
            ("Profile.CreateTime", "creation time of the profile"),
            ("Profile.PlayTime", "play time"),
        };

        // Every public property and field, recursively (arrays and dictionaries element by element), compared; the paths in
        // FixtureIgnore are skipped. A difference is "Path[i].Field: a vs b".
        static void DeepDiff(string path, object a, object b, List<string> diffs, int depth = 0)
        {
            if (diffs.Count >= 40) return;
            if (FixtureIgnore.Any(i => i.path == path)) return;
            if (a == null || b == null) { if (a != b) diffs.Add($"{path}: {Describe(a)} vs {Describe(b)}"); return; }
            var t = a.GetType();
            if (t != b.GetType()) { diffs.Add($"{path}: type {t.Name} vs {b.GetType().Name}"); return; }
            if (t.IsPrimitive || t.IsEnum || a is string || a is decimal || a is DateTime || a is TimeSpan) { if (!a.Equals(b)) diffs.Add($"{path}: {a} vs {b}"); return; }
            if (depth > 14) { diffs.Add($"{path}: deeper than 14 levels"); return; }
            if (a is System.Collections.IDictionary da && b is System.Collections.IDictionary db)
            {
                foreach (var k in da.Keys.Cast<object>().Union(db.Keys.Cast<object>()).OrderBy(k => k.ToString()))
                    DeepDiff($"{path}[{k}]", da.Contains(k) ? da[k] : null, db.Contains(k) ? db[k] : null, diffs, depth + 1);
                return;
            }
            if (a is IEnumerable ea && b is IEnumerable eb)
            {
                var la = ea.Cast<object>().ToList(); var lb = eb.Cast<object>().ToList();
                // Elements with an Id (PlayerStores, LoungeQuests, Characters) pair up by it: the lists come from Dictionary.Values,
                // whose order says nothing (PlayerContext.cs:419).
                if (la.Concat(lb).Any() && la.Concat(lb).All(x => x != null && KeyOf(x) != null))
                {
                    foreach (var k in la.Select(KeyOf).Union(lb.Select(KeyOf)).OrderBy(k => k.ToString()))
                        DeepDiff($"{path}[Id={k}]", la.FirstOrDefault(x => Equals(KeyOf(x), k)), lb.FirstOrDefault(x => Equals(KeyOf(x), k)), diffs, depth + 1);
                    return;
                }
                for (int i = 0; i < Math.Max(la.Count, lb.Count); i++)
                    DeepDiff($"{path}[{i}]", i < la.Count ? la[i] : null, i < lb.Count ? lb[i] : null, diffs, depth + 1);
                return;
            }
            const System.Reflection.BindingFlags F = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            foreach (var f in t.GetFields(F)) DeepDiff(path == "" ? f.Name : $"{path}.{f.Name}", f.GetValue(a), f.GetValue(b), diffs, depth + 1);
            foreach (var pr in t.GetProperties(F).Where(x => x.GetIndexParameters().Length == 0 && x.CanRead))
            {
                object va, vb;
                try { va = pr.GetValue(a); vb = pr.GetValue(b); } catch (Exception) { continue; }
                DeepDiff(path == "" ? pr.Name : $"{path}.{pr.Name}", va, vb, diffs, depth + 1);
            }
        }

        static object KeyOf(object x) => x == null || x.GetType().IsPrimitive || x is string ? null : Flow.Get(x, "Id");

        // An element only one side has, written out: "{Id=80200, Entries=[{Key=Started, Value=true}, ...]}".
        static string Describe(object x, int depth = 0)
        {
            if (x == null) return "-";
            var t = x.GetType();
            if (t.IsPrimitive || t.IsEnum || x is string || x is decimal || x is DateTime) return x.ToString();
            if (depth > 3) return t.Name;
            if (x is IEnumerable e) return "[" + string.Join(", ", e.Cast<object>().Take(12).Select(v => Describe(v, depth + 1))) + "]";
            const System.Reflection.BindingFlags F = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            var parts = t.GetFields(F).Select(f => $"{f.Name}={Describe(f.GetValue(x), depth + 1)}")
                .Concat(t.GetProperties(F).Where(p => p.GetIndexParameters().Length == 0 && p.CanRead).Select(p =>
                {
                    try { return $"{p.Name}={Describe(p.GetValue(x), depth + 1)}"; } catch (Exception) { return null; }
                }).Where(v => v != null));
            return "{" + string.Join(", ", parts) + "}";
        }

        static object ReadSave(string typeName, string path)
        {
            var data = GameType("SerializeHelper")?.GetMethod("Deserialize")?.MakeGenericMethod(GameType(typeName)).Invoke(null, new object[] { System.IO.File.ReadAllBytes(path) });
            Assert.IsNotNull(data, $"{path} could not be deserialized as {typeName}");
            return data;
        }

        // A compact line about a PlayerContext file, for the log.
        static string SaveFacts(string path)
        {
            var data = ReadSave("PlayerContextData", path);
            string FlagsOf(string prop) => string.Join(",", (Flow.Get(Flow.Get(data, prop), "Flags") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>());
            return $"level={Flow.Get(Flow.Get(data, "Profile"), "Level")} exp={Flow.Get(Flow.Get(data, "Profile"), "Exp")} campaigns=[{FlagsOf("ClearedCampaignFlags")}] loungeQuests=[{FlagsOf("CompletedLoungeQuestFlags")}]";
        }

        // C3: the first link's input is the profile the last green A4 run exported (captures/review/a4-saves): the chain is
        // A4 -> 1101 -> ... -> 1202 on today's code. The export must carry A4's provenance and be newer than every game source
        // file, so it was written by the code under test. The post-tutorial fixture the other flow tests start from is
        // compared with it and the drift is LOGGED, not asserted: the final report says where those tests' profile differs.
        const string A4Export = "captures/review/a4-saves";

        static void AssertA4Export()
        {
            string a4dir = ProjectPath(A4Export), prov = System.IO.Path.Combine(a4dir, "PROVENANCE.txt");
            Assert.IsTrue(System.IO.File.Exists(prov), "a4-saves has no provenance (not exported by A4_TutorialFlow)");
            string text = System.IO.File.ReadAllText(prov);
            Assert.IsTrue(text.Contains("test=VD.PlayTests.A4_TutorialFlow.NewGameToTutorial"), $"a4-saves was not exported by A4_TutorialFlow: {text}");
            var exported = System.IO.File.GetLastWriteTimeUtc(prov);
            // The export comes from the game code and from the test code that played it (A4's walk, Flow), and from the runtime mute.
            var sources = System.IO.Directory.GetFiles(ProjectPath("Assets", "Scripts"), "*.cs", System.IO.SearchOption.AllDirectories)
                .Concat(System.IO.Directory.GetFiles(ProjectPath("Assets", "_VD", "Runtime"), "*.cs", System.IO.SearchOption.AllDirectories))
                .Concat(new[] { ProjectPath("Assets", "_VD", "Tests", "A4_TutorialFlow.cs"), ProjectPath("Assets", "_VD", "Tests", "Flow.cs") });
            var newest = sources.Select(f => (f, t: System.IO.File.GetLastWriteTimeUtc(f))).OrderByDescending(x => x.t).First();
            Assert.Greater(exported, newest.t, $"a4-saves ({exported:u}) is older than the source {newest.f} ({newest.t:u}): rerun A4_TutorialFlow");
            Flow.Log($"chain first link: A4 export {exported:u}, newest source {newest.t:u} ({System.IO.Path.GetFileName(newest.f)}); {text.Replace("\n", "; ")}");
        }

        static void ReportFixtureDrift()
        {
            Flow.Log($"fixture drift: fields left out as run-to-run noise: {string.Join("; ", FixtureIgnore.Select(i => $"{i.path} = {i.reason}"))}");
            foreach (var (typeName, file) in new[] { ("PlayerContextData", "PlayerContext_0.sav"), ("AccountContextData", "AccountContext.sav") })
            {
                var diffs = new List<string>();
                DeepDiff("", ReadSave(typeName, ProjectPath(A4Export, file)), ReadSave(typeName, ProjectPath(Fixture, file)), diffs);
                Flow.Log(diffs.Count == 0 ? $"fixture drift: {file} none" : $"fixture drift: {file} (A4 export today vs {Fixture}): {string.Join(" | ", diffs)}");
            }
        }

        static IEnumerator ChainCase(string name)
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6";
            s_chain = true;
            int index = Array.IndexOf(ChainNames, name);
            string prev = index > 0 ? ChainNames[index - 1] : null;
            string input = null;
            try
            {
                // C1/C2: provenance first, before any game is touched.
                if (index == 0)
                {
                    s_runId = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    s_chainReport.Clear();
                    System.IO.Directory.CreateDirectory(RunDir);
                    System.IO.File.WriteAllText(Flow.ChainSavesPath("LATEST"), s_runId);
                    PruneOldRuns();
                    Flow.Log($"chain run {s_runId}: first link {name}; saves in {RunDir}");
                }
                else
                {
                    bool resume = Environment.GetEnvironmentVariable("VD_CHAIN_RESUME") == "1";
                    if (resume)
                    {
                        string latest = System.IO.File.Exists(Flow.ChainSavesPath("LATEST")) ? System.IO.File.ReadAllText(Flow.ChainSavesPath("LATEST")).Trim() : null;
                        Flow.Log($"!!!! VD_CHAIN_RESUME=1: case {name} takes the link of run {latest}; this is a debugging run, NOT the proof run");
                        s_runId = latest;
                    }
                    Assert.IsNotNull(s_runId, "previous link not from this run: the first case did not run in this editor");
                    string problem = ReadLink(out var linkRun, out var linkCase);
                    Assert.IsNull(problem, $"previous link not from this run: {problem}");
                    Assert.AreEqual(s_runId, linkRun, $"previous link not from this run (link run {linkRun}, this run {s_runId})");
                    Assert.AreEqual(prev, linkCase, $"previous link is not case {prev}: the link says {linkCase}");
                    input = System.IO.Path.Combine("captures", "review", "chain-saves", "runs", s_runId, prev);
                    Assert.IsTrue(System.IO.File.Exists(ProjectPath(input, "PlayerContext_0.sav")), $"no saves from case {prev} in {input}");
                }

                int deaths = 0;
                var places = new List<string>();
                for (int attempt = 1; attempt <= 3; attempt++)
                {
                    Exception failure = null;
                    yield return Catching(ChainAttempt(name, index, prev, input, attempt), e => failure = e);
                    if (failure == null)
                    {
                        s_chainReport.Add($"{name}: attempts={attempt} deaths={deaths}");
                        Flow.Log($"chain {name}: passed on attempt {attempt}, deaths={deaths} [{string.Join(" | ", places)}]");
                        break;
                    }
                    bool died = s_escapeFail || State() == "Dead" || failure.Message.Contains("died") || failure.Message.Contains("Dead") || failure.Message.Contains("Escape Failed");
                    string place = DeathPlace();
                    Flow.Log($"chain {name}: attempt {attempt} failed ({(died ? "death" : "not a death")}): {failure.Message.Split('\n')[0]} at {place}");
                    if (!died || attempt == 3) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
                    deaths++;
                    // C4: the same place twice is a game-bug signal, not bad luck.
                    Assert.IsFalse(places.Contains(place), $"chain {name}: the diver died at the same place twice ({place}): not a retry-able accident");
                    places.Add(place);
                    Flow.Log($"chain {name}: retry {deaths} of 2 after a death at {place}");
                }
            }
            finally
            {
                s_chain = false;
            }
        }

        static string DeathPlace()
        {
            var me = Me();
            string at = me ? $"{(int)(me.transform.position.x / 15)},{(int)(me.transform.position.z / 15)}" : "-";
            return $"{Flow.ActiveScene} cell {at} boss {(s_lastBossSeen ?? "-").Split(' ').FirstOrDefault()} hp {(s_lastBossSeen ?? "-").Split(' ').Skip(1).FirstOrDefault()}";
        }

        // Runs `body`, handing an exception to onFail instead of throwing (a yield cannot sit inside try/catch). Nested
        // IEnumerators are flattened with a stack, as the test framework's TestEnumerator does: passed up they would be run by the
        // framework, whose own catch fails the test. On a failure the stack is disposed so the pending finally blocks run.
        static IEnumerator Catching(IEnumerator body, Action<Exception> onFail)
        {
            var stack = new Stack<IEnumerator>();
            stack.Push(body);
            while (stack.Count > 0)
            {
                var top = stack.Peek();
                object current;
                try
                {
                    if (!top.MoveNext()) { stack.Pop(); continue; }
                    current = top.Current;
                }
                catch (Exception e) when (!(e is OperationCanceledException))
                {
                    while (stack.Count > 0) { try { (stack.Pop() as IDisposable)?.Dispose(); } catch (Exception) { } }
                    onFail(e);
                    yield break;
                }
                if (current is IEnumerator nested && !(current is CustomYieldInstruction)) { stack.Push(nested); continue; }
                yield return current;
            }
        }

        static IEnumerator CatchOuter(Action<string> log)
        {
            try { yield return CatchInner(); }
            finally { log("outer finally ran"); }
        }

        static IEnumerator CatchInner()
        {
            yield return null;
            Assert.Fail("inner failed after one frame");
        }

        // The retry machinery's own proof: an assert thrown two levels down reaches onFail, the pending finally runs, the test stays green.
        [UnityTest, Timeout(60000)]
        public IEnumerator CatchingCatchesNested()
        {
            Exception caught = null; var logs = new List<string>();
            yield return Catching(CatchOuter(logs.Add), e => caught = e);
            Assert.IsNotNull(caught, "onFail was not called");
            Assert.IsInstanceOf<AssertionException>(caught);
            StringAssert.Contains("inner failed after one frame", caught.Message);
            CollectionAssert.Contains(logs, "outer finally ran");
            Flow.Tag = "VD_A6";
            Flow.Log($"Catching: nested failure caught ({caught.GetType().Name}), finally ran={logs.Contains("outer finally ran")}");
        }

        static IEnumerator ChainAttempt(string name, int index, string prev, string input, int attempt)
        {
            s_relicTalked = s_relicSold = false; s_lastQuestClearUtc = default;
            if (attempt > 1) Flow.Log($"chain {name}: attempt {attempt}, reloading the input save");
            if (index == 0)
            {
                AssertA4Export();
                ReportFixtureDrift();
                yield return Start1101(A4Export);
                Assert.IsTrue(QuestCleared(80200), $"80200 not cleared after Elara's tour: {QState(80200)}");
                Flow.Log($"quests at the start: 81101={QState(81101)} 80100={QState(80100)}");
                yield return Body1101();
                yield return MainQuests(Campaign1101);
                yield return SavedAndListed(Campaign1101, Campaign1102);
            }
            else
            {
                yield return FromTitle(seedFixture: true, savesDir: input);
                Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
                Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
                Flow.RouteInputToGame();
                // The previous link, read back from the save the game loaded: its clear and its main quests.
                if (int.TryParse(prev, out int prevId))
                {
                    Assert.IsTrue(Flow.SavedCleared(prevId, out var saved), $"{prevId} not cleared in the input save: {saved}");
                    Assert.IsTrue(!saved.StartsWith("read threw") && !saved.StartsWith("no "), $"the input save could not be read: {saved}");
                    var pm = Flow.Find("PlayerManager");
                    Assert.IsTrue(pm?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pm, new object[] { prevId }) is bool c && c, $"{prevId} not cleared in the loaded profile");
                    if (MainAfter.TryGetValue(prevId, out var quests))
                        foreach (var (quest, _) in quests) Assert.IsTrue(QuestCleared(quest), $"main quest {quest} (after {prevId}) is not in the loaded profile's cleared set");
                    Flow.Log($"chain {name}: input save from {prev}: {saved}; level={Flow.Get(pm, "Level")}");
                }
                if (name == "AfterLast")
                {
                    yield return DemoEnd();
                    yield return TitleButtons();
                    yield break;
                }
                int id = int.Parse(name);
                yield return CampaignBody(id);
                yield return MainQuests(id);
            }
            // The game's own autosave (PlayerManager.AutoSave: dirty, in the lounge, Playing) writes the quests; wait for it.
            yield return WaitSaved();
            yield return ExportLink(name);
        }

        static IEnumerator WaitSaved()
        {
            var pm = Flow.Find("PlayerManager");
            float t0 = Time.realtimeSinceStartup;
            yield return new WaitForSecondsRealtime(2f);
            while (Time.realtimeSinceStartup - t0 < 90f)
            {
                var ctx = Flow.Get(pm, "_playerContext");
                bool dirty = Flow.Get(ctx, "HasDirty") is bool d && d, saving = Flow.Get(pm, "_isSaving") is bool sv && sv;
                if (!dirty && !saving) break;
                yield return new WaitForSecondsRealtime(1f);
            }
            yield return new WaitForSecondsRealtime(2f);
            Flow.Log($"autosave settled after {Time.realtimeSinceStartup - t0:F0}s: dirty={Flow.Get(Flow.Get(pm, "_playerContext"), "HasDirty")}");
        }

        static void PruneOldRuns()
        {
            string runs = Flow.ChainSavesPath("runs"), green = System.IO.File.Exists(Flow.ChainSavesPath("GREEN")) ? System.IO.File.ReadAllText(Flow.ChainSavesPath("GREEN")).Trim() : null;
            if (!System.IO.Directory.Exists(runs)) return;
            foreach (var d in System.IO.Directory.GetDirectories(runs).OrderByDescending(x => x).Skip(3))
                if (System.IO.Path.GetFileName(d) != green) System.IO.Directory.Delete(d, true);
        }

        // Lounge quest clears do not set PlayerContext.HasDirty (only inventory, stash, storage and profile changes do), so the
        // lounge autosave does not wait for them. The game saves the loaded profile when the title scene comes up (StartScene.
        // EnsureAccountAsync -> PlayerManager.SaveAsync): the export goes through the system menu's Return to Title, as a player
        // leaves, and reads the saved flags back from the copied file.
        static IEnumerator ExportLink(string name)
        {
            string savesPath = ProjectPath("SavesDir", "PlayerContext_0.sav");
            var before = System.IO.File.GetLastWriteTimeUtc(savesPath);
            var pmSave = Flow.Find("PlayerManager");
            Flow.Log($"checkpoint save: the game would persist the quest clears at its next dirty change (PlayerManager.SetClearedLoungeQuest does not mark the profile dirty); calling PlayerManager.SaveAsync now. last quest cleared {s_lastQuestClearUtc:HH:mm:ss}, file {before:HH:mm:ss} (UTC)");
            pmSave?.GetType().GetMethod("SaveAsync")?.Invoke(pmSave, null);
            yield return Flow.WaitUntil(() => System.IO.File.GetLastWriteTimeUtc(savesPath) > before, 30f, "PlayerManager.SaveAsync wrote the profile");
            yield return new WaitForSecondsRealtime(3f);
            var after = System.IO.File.GetLastWriteTimeUtc(savesPath);
            Flow.Log($"export {name}: PlayerContext_0.sav {before:HH:mm:ss} -> {after:HH:mm:ss} (UTC)");
            Assert.Greater(after, before, "PlayerManager.SaveAsync did not rewrite PlayerContext_0.sav");
            if (s_lastQuestClearUtc != default) Assert.GreaterOrEqual(after, s_lastQuestClearUtc, "the saved profile is older than the last quest clear");
            string savesDir = ProjectPath("SavesDir"), outDir = System.IO.Path.Combine(RunDir, name);
            System.IO.Directory.CreateDirectory(outDir);
            foreach (var f in ContextSaves)
                if (System.IO.File.Exists(System.IO.Path.Combine(savesDir, f))) System.IO.File.Copy(System.IO.Path.Combine(savesDir, f), System.IO.Path.Combine(outDir, f), true);
            string exported = System.IO.Path.Combine(outDir, "PlayerContext_0.sav");
            if (int.TryParse(name, out int id))
            {
                Assert.IsTrue(Flow.SavedCleared(id, out var w), $"{id} not cleared in the exported save: {w}");
                Assert.IsTrue(!w.StartsWith("read threw") && !w.StartsWith("no "), $"the exported save could not be read: {w}");
                if (MainAfter.TryGetValue(id, out var quests))
                    foreach (var (quest, _) in quests)
                        Assert.IsTrue(SavedQuestCleared(exported, quest), $"main quest {quest} is not in the exported save's CompletedLoungeQuestFlags ({SaveFacts(exported)})");
            }
            Flow.Log($"export {name}: {SaveFacts(exported)}");
            // Committed to LINK by Chain() once the game-error check of the case has passed as well.
            System.IO.File.WriteAllLines(System.IO.Path.Combine(RunDir, "LINK.pending"), new[] { s_runId, name });
            Flow.Log($"chain link written: run {s_runId} after {name} -> {outDir}");
        }

        // CompletedLoungeQuestFlags in the file, the field PlayerContext.ClearedLoungeQuestFlags loads from (PlayerContext.cs:198).
        static bool SavedQuestCleared(string path, int questId)
        {
            var data = GameType("SerializeHelper")?.GetMethod("Deserialize")?.MakeGenericMethod(GameType("PlayerContextData")).Invoke(null, new object[] { System.IO.File.ReadAllBytes(path) });
            var row = LoungeQuestRow(questId);
            var flags = Flow.Get(data, "CompletedLoungeQuestFlags");
            return flags?.GetType().GetMethod("IsSet")?.Invoke(flags, new[] { Flow.Get(row, "BitFlagIndex") }) is bool b && b;
        }

        // Gap 8's title half: back on the title after the last story, Options opens and closes; Wishlist (Application.OpenURL
        // steam://store/3438920) and Quit (Application.Quit) leave the process, so they are not pressed here: their handlers
        // are StartScene.InitButtons and the test asserts the buttons are there and ready.
        static IEnumerator TitleButtons()
        {
            yield return ReturnToTitle();
            if (Flow.Find("StartSceneView") == null) { UnityEngine.SceneManagement.SceneManager.LoadScene(0); yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 120f, "title"); }
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true && Flow.Ready(Flow.Button(title, "Option_Button")), 60f, "title menu");
            Assert.IsNotNull(title, "the title did not show after the last story");
            foreach (var b in new[] { "Play_Button", "Option_Button", "Quit_Button", "Wishlist_Button" })
                Flow.Log($"title {b}: {(Flow.Button(title, b) != null ? (Flow.Ready(Flow.Button(title, b)) ? "ready" : "present, not ready") : "missing")}");
            Assert.IsNotNull(Flow.Button(title, "Quit_Button"), "no Quit button on the title");
            Assert.IsNotNull(Flow.Button(title, "Wishlist_Button"), "no Wishlist button on the title");
            yield return Review.Shot("a6-demo-end-title");
            yield return Flow.Click(title, "Option_Button");
            yield return Flow.WaitUntil(() => Flow.TopPopup() != null, 8f, "options popup");
            Flow.Log($"title Options -> {Flow.TopPopup()}");
            Assert.IsNotNull(Flow.TopPopup(), "Options did not open a popup");
            yield return Review.Shot("a6-demo-end-options");
            yield return Flow.Keys(0.2f, Key.Escape);
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 5f, "options closed");
            Assert.IsNull(Flow.TopPopup(), "Options did not close with Esc");
        }

        // Main lounge quests (TLoungeQuest Category Main) by the clear that opens them; the level that comes with each
        // next story (SavedAndListed) auto-starts the reputation ones. Giver NPCs from each Lua START_NPC.
        static readonly Dictionary<int, (int quest, int giver)[]> MainAfter = new Dictionary<int, (int, int)[]>
        {
            { 1101, new[] { (81101, 0), (80100, 700005), (80002, 700000), (80101, 700102) } },
            { 1102, new[] { (81102, 0), (80003, 700000), (80102, 700102) } },
            { 1103, new[] { (81103, 0), (80004, 700000), (80103, 700102) } },
            { 1104, new[] { (81104, 0), (80005, 700005), (80104, 700102) } },
            { 1105, new[] { (81105, 0), (80006, 700102) } },
            { 1201, new[] { (80007, 700005) } },
        };

        static IEnumerator MainQuests(int afterCampaign)
        {
            if (!MainAfter.TryGetValue(afterCampaign, out var quests)) yield break;
            yield return new WaitForSecondsRealtime(1.5f);
            foreach (var (quest, giver) in quests)
            {
                var before = InventoryIds();
                Flow.Log($"main quest {quest}: {QState(quest)} giver={giver} level={Flow.Get(Flow.Find("PlayerManager"), "Level")}");
                for (int i = 0; i < 8 && !QuestCleared(quest); i++)
                {
                    var st = QState(quest);
                    if (quest == 80100 && st == "InProgress" && HasEquipment(108021))
                    {
                        yield return SellRelic(108021);   // owned: StartDialog's Lua GiveEquipment gave it
                        s_relicSold = true;
                    }
                    // The sale pays the coins for the owner's LevelUp (TLevel 2: Exp 1500, Coin 3); the quest's task is UserLevel:2.
                    else if (quest == 80100 && st == "InProgress" && s_relicTalked && Flow.Get(Flow.Find("PlayerManager"), "Level") is int lvl && lvl < 2)
                        yield return OwnerLevelUp();
                    if (giver != 0 && (st == "NotStarted" || st == "InProgress" || st == "NotCompleted"))
                    {
                        yield return TalkQuest(giver, $"main {quest} at {giver}");
                        if (quest == 80100 && !HasEquipment(108021) && st == "InProgress") s_relicTalked = true;
                    }
                    else
                        yield return new WaitForSecondsRealtime(1.5f);
                }
                var rewards = Flow.Get(LoungeQuestRow(quest), "Rewards") as Array;
                Flow.Log($"main quest {quest}: {QState(quest)} cleared={QuestCleared(quest)} table rewards=[{string.Join(",", rewards?.Cast<object>() ?? Enumerable.Empty<object>())}] inventory {before} -> {InventoryIds()}");
                // CompleteQuest sets the cleared bit, pays the rewards, triggers LoungeQuestClearEvent and then RemoveQuest, so
                // a finished quest reads None from GetQuestState: the cleared set is the completion signal (GameLoungeQuestManager.CompleteQuest).
                Assert.IsTrue(QuestCleared(quest), $"main quest {quest} not in the cleared set (state {QState(quest)})");
                s_lastQuestClearUtc = DateTime.UtcNow;
                Assert.AreEqual("None", QState(quest), $"main quest {quest} cleared but still listed as active");
                Assert.IsTrue(rewards == null || rewards.Length == 0, $"main quest {quest} has table rewards to check: {rewards?.Length}");
                if (quest == 80100) Assert.IsTrue(s_relicSold, "80100: the relic 108021 the owner gave was not sold to Arthur");
            }
        }

        static IEnumerator OwnerLevelUp()
        {
            var pm = Flow.Find("PlayerManager");
            Flow.Log($"owner level up: level={Flow.Get(pm, "Level")} exp={Flow.Get(pm, "Exp")} inventory {InventoryIds()}");
            var owner = Flow.FindNpc(700005);
            Assert.IsNotNull(owner, "owner 700005 not in the lounge");
            bool focused = false;
            foreach (var side in new[] { Vector3.back, Vector3.left, Vector3.right, Vector3.forward })
            {
                var o = owner;
                yield return Flow.WalkTo(() => o ? o.transform.position + side * 1.0f : (Vector3?)null, 0.4f, 10f, "owner side");
                Flow.Aim(owner.transform.position + Vector3.up * 0.8f);
                yield return Flow.WaitUntil(() => Flow.Focused(o), 2f, "focus owner");
                if (focused = Flow.Focused(owner)) break;
            }
            Assert.IsTrue(focused, $"could not focus the owner (focus {Flow.Focus()})");
            yield return Flow.Keys(0.7f, Key.F);
            yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null || Flow.Find("DialogPopupView") != null, 4f, "owner reaction");
            float t0 = Time.realtimeSinceStartup;
            bool yes = false;
            Flow.Log($"owner level up: after F top={Flow.TopPopup()} focus={Flow.Focus()}");
            while (Flow.Find("LevelUpPopupView") == null && Time.realtimeSinceStartup - t0 < 40f)
            {
                var dialog = Flow.Find("DialogPopupView");
                if (dialog == null && Flow.Find("SelectionPopupView") != null)
                {
                    Flow.Log($"owner level up: selection popup open, choosing LevelUp (focus {Flow.Focus()})");
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(owner, "LevelUp");
                    yield return new WaitForSecondsRealtime(2f);
                    continue;
                }
                var sel = (Flow.Get(dialog, "Selections") as GameObject[])?.FirstOrDefault(g => g && g.activeInHierarchy);
                var b = sel ? sel.GetComponentInChildren<UnityEngine.UI.Button>() : null;
                if (b != null && b.IsInteractable() && !yes) { Flow.Log($"owner level up: '{Flow.DialogEntry()}' -> Yes"); b.onClick.Invoke(); yes = true; }
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")) && !Flow.DialogDelaying()) Flow.Button(dialog, "Background").onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.5f);
            }
            yield return new WaitForSecondsRealtime(3f);
            Assert.IsNotNull(Flow.Find("LevelUpPopupView"), $"no LevelUpPopup after the owner's LevelUp (yes={yes}, top={Flow.TopPopup()})");
            for (int i = 0; i < 4 && Flow.TopPopup() != null; i++)
            {
                var bg = Flow.Button(Flow.Find("LevelUpPopupView"), "Background");
                if (Flow.Ready(bg)) bg.onClick.Invoke(); else yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(1.5f);
                if (Flow.Find("DialogPopupView") != null) yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 30f, "after level up");
            }
            Flow.Log($"owner level up: level={Flow.Get(pm, "Level")} 80100={QState(80100)} cleared={QuestCleared(80100)}");
        }

        static IEnumerator A3Call(string name, params object[] args)
        {
            var m = typeof(A3_NpcFlow).GetMethod(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(m, $"A3_NpcFlow.{name} not found");
            return (IEnumerator)m.Invoke(null, args);
        }

        // 80100 (Lua): the owner's 'Rosary of Pain' (equipment 108021) is sold to the guest Arthur 600012 (spawned by the quest) at
        // his ArtifactDeal function; E_ArtifactDeal with the item gone moves the quest to Return. A3's negotiation, on that item.
        static IEnumerator SellRelic(int equipmentId)
        {
            const string key = "sell-relic";
            yield return Flow.WaitUntil(() => Flow.FindNpc(600012) != null, 10f, "guest 600012 spawned by 80100");
            var guest = Flow.FindNpc(600012);
            Assert.IsNotNull(guest, "guest 600012 was not spawned by quest 80100");
            int functionId = Flow.FunctionIds(guest).FirstOrDefault(id => Flow.FunctionType(id) == "ArtifactDeal");
            Assert.AreNotEqual(0, functionId, "guest 600012 has no ArtifactDeal function");
            for (int attempt = 0; attempt < 4 && Flow.Find("ArtifactDealPopupPresenter") == null; attempt++)
            {
                yield return A3Call("Interact", guest, key);
                yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null || Flow.Find("DialogPopupView") != null || Flow.Find("ArtifactDealPopupPresenter") != null, 4f, $"{key} reaction");
                if (Flow.Find("DialogPopupView") != null) { yield return new WaitForSecondsRealtime(1f); continue; }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return A3Call("Select", functionId, guest, key);
                }
                yield return Flow.WaitUntil(() => Flow.Find("ArtifactDealPopupPresenter") != null, 8f, $"{key} popup");
            }
            Assert.IsNotNull(Flow.Find("ArtifactDealPopupPresenter"), "the deal popup did not open at guest 600012");
            yield return new WaitForSecondsRealtime(2f);
            var popup = Flow.Find("ArtifactDealPopupPresenter");
            var manager = Flow.Get(popup, "_artifactDealManager");
            string State() => Flow.Value(manager, "RxDealState")?.ToString();
            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "InventoryGoodsSlotPresenter")
                .FirstOrDefault(m =>
                {
                    var gs = Flow.Value(Flow.Get(m, "_model"), "RxGoodsSlot");
                    return gs != null && Flow.Get(gs, "IsEmpty") is false && Flow.Get(gs, "Id") is int gid && gid == equipmentId;
                });
            Assert.IsNotNull(slot, $"item {equipmentId} is not in the deal popup's inventory");
            var button = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, $"{key} item slot");
            button.onClick.Invoke();
            yield return A3Call("Confirm", $"{key} pick");
            yield return Flow.WaitUntil(() => State() == "AwaitAction", 20f, $"{key} hand dealt");
            Assert.AreEqual("AwaitAction", State(), "the hand was not dealt");
            yield return new WaitForSecondsRealtime(1.5f);
            var card = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "ArtifactDealCardPlayPresenter" && Flow.Ready(Flow.Get(m, "NavButton") as UnityEngine.UI.Button))
                .OrderBy(m => m.transform.position.x).FirstOrDefault();
            if (card != null)
            {
                (Flow.Get(card, "NavButton") as UnityEngine.UI.Button).onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.5f);
                yield return Flow.WaitUntil(() => State() is "AwaitAction" or "AwaitActionWithNoCount", 20f, $"{key} card resolved");
                yield return new WaitForSecondsRealtime(1.5f);
            }
            var deal = Flow.Button(Flow.Find("ArtifactDealPopupView"), "DealButtonLayout_Button");
            yield return Flow.WaitUntil(() => Flow.Ready(deal), 5f, $"{key} Deal Sealed");
            deal.onClick.Invoke();
            yield return A3Call("Confirm", $"{key} deal");
            yield return Flow.WaitUntil(() => Flow.Find("ArtifactDealCompletePopupView") != null, 20f, $"{key} complete popup");
            Assert.IsNotNull(Flow.Find("ArtifactDealCompletePopupView"), "the deal did not complete");
            yield return new WaitForSecondsRealtime(2f);
            yield return A3Call("Close", "ArtifactDealCompletePopupPresenter", key + "-complete");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Log($"{key}: sold, state {QState(80100)} has item={HasEquipment(equipmentId)} inventory {InventoryIds()}");
            Assert.IsFalse(HasEquipment(equipmentId), "the relic is still owned after the deal");
        }

        static object LoungeQuestRow(int id)
        {
            var table = GameType("GameCommon.Table.TLoungeQuest")?.GetProperty("Table")?.GetValue(null);
            try { return table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id }); }
            catch (Exception) { return null; }
        }

        static object LQM() => Flow.Find("GameLoungeQuestManager");
        static string QState(int questId) => LQM()?.GetType().GetMethod("GetQuestState")?.Invoke(LQM(), new object[] { questId })?.ToString() ?? "?";
        static bool QuestCleared(int questId)
        {
            var pm = Flow.Find("PlayerManager");
            try { return pm != null && pm.GetType().GetMethod("IsClearedLoungeQuest").Invoke(pm, new object[] { questId }) is bool b && b; }
            catch (Exception) { return false; }
        }

        static bool HasEquipment(int id)
        {
            var pm = Flow.Find("PlayerManager");
            var ctx = Flow.Get(pm, "_playerContext");
            foreach (var name in new[] { "Equipments", "RxEquipments" })
                if (Flow.Get(ctx, name) is IEnumerable list)
                    foreach (var e in list)
                    {
                        var v = Flow.Get(e, "Value") ?? e;
                        if (Flow.Get(v, "TableId") is int t && t == id || Flow.Get(v, "Id") is int i2 && i2 == id) return true;
                    }
            return InventoryIds().Contains($"{id}x");
        }

        // Walk up, press F; a quest dialog plays with the first choice at each choice (A9a Play); a function menu is left.
        static IEnumerator TalkQuest(int npcId, string what)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"{what}: npc {npcId} not in the lounge"); yield return new WaitForSecondsRealtime(1.5f); yield break; }
            // Other NPCs stand next to some givers (the owner beside Oldry 700011): stand where the giver is the focused object.
            var near = Flow.Npcs().Where(n => n && n != npc && (n.transform.position - npc.transform.position).sqrMagnitude < 25f).ToList();
            Flow.Log($"{what}: giver at {npc.transform.position}, near: {string.Join(", ", near.Select(n => Flow.NpcId(n) + "@" + n.transform.position))}");
            bool focused = false;
            foreach (var off in new[] { new Vector3(0, 0, -1.3f), new Vector3(0, 0, 1.3f), new Vector3(1.3f, 0, 0), new Vector3(-1.3f, 0, 0) })
            {
                if (!npc) break;
                yield return Flow.WalkTo(() => npc ? npc.transform.position + off : (Vector3?)null, 0.4f, 12f, $"{what} stand {off}");
                Flow.Aim(npc.transform.position + Vector3.up * 0.8f);
                yield return new WaitForSecondsRealtime(0.5f);
                if (focused = Flow.Focused(npc)) break;
                Flow.Log($"{what}: focus={Flow.Focus()} at offset {off}");
            }
            if (focused) { Flow.Log($"interact {what} focus={Flow.Focus()}"); yield return Flow.Keys(0.7f, Key.F); }
            else yield return Flow.InteractNpc(npc, what);
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Flow.Find("SelectionPopupView") != null)
            {
                Flow.Log($"{what}: selection popup, no quest dialog; Esc");
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(1f);
                yield break;
            }
            float start = Time.realtimeSinceStartup;
            object lastEntry = null;
            string last = null;
            while (Time.realtimeSinceStartup - start < 150f)
            {
                var note = Flow.Find("NoteSystemPopupView");
                var dialog = Flow.Find("DialogPopupView");
                if (dialog == null && note == null) break;
                if (note != null && Flow.Ready(Flow.Button(note, "BaseBgDimmer"))) Flow.Button(note, "BaseBgDimmer").onClick.Invoke();
                else if (dialog != null)
                {
                    var entry = Flow.Get(Flow.Find("DialogPopupPresenter"), "_currentEntry");
                    var choices = (Flow.Get(dialog, "Selections") as GameObject[])?.Where(g => g && g.activeInHierarchy).ToList() ?? new List<GameObject>();
                    if (choices.Count > 0 && entry != null)
                    {
                        var b = choices[0].GetComponentInChildren<UnityEngine.UI.Button>();
                        if (b != null && b.IsInteractable() && !ReferenceEquals(entry, lastEntry))
                        {
                            Flow.Log($"dialog {what}: '{Flow.DialogEntry()}' choices={choices.Count} -> #0");
                            yield return new WaitForSecondsRealtime(0.6f);
                            lastEntry = entry;
                            if (b) b.onClick.Invoke();
                        }
                    }
                    else if (choices.Count == 0 && Flow.Ready(Flow.Button(dialog, "Background")) && !Flow.DialogDelaying())
                    {
                        string line = Flow.DialogEntry();
                        if (line != last) { Flow.Log($"dialog {what}: '{line}'"); last = line; }
                        if (Flow.Find("DialogPopupView") != null) Flow.Button(dialog, "Background").onClick.Invoke();
                    }
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        // ---- after the last story (the demo's end) ----

        // What the original does after 1202 (Campaign/1202.lua QuestComplete, TLoungeQuest, StartScene): Elara's wrap-up,
        // quest state Completed, no further story campaign in TCampaign, sub quest 82205 (Lucas, UnlockConditions
        // CampaignCleared:1202) offered, and no demo-end screen or return to title: the game stays in the lounge.
        static IEnumerator DemoEnd()
        {
            var table = GameType("GameCommon.Table.TCampaign")?.GetProperty("Table")?.GetValue(null);
            var rows = (Flow.Get(table, "Values") ?? Flow.Get(table, "DataList")) as IEnumerable;
            var laterStories = rows?.Cast<object>().Where(r => Flow.Get(r, "Category")?.ToString() == "Story" && Flow.Get(r, "Id") is int i && i > 1202)
                .Select(r => Flow.Get(r, "Id")).ToList();
            Flow.Log($"demo end: story campaigns after 1202 in TCampaign: [{(laterStories == null ? "table not enumerable" : string.Join(",", laterStories))}]");
            Assert.IsNotNull(laterStories, "TCampaign rows could not be enumerated");
            Assert.IsEmpty(laterStories, "TCampaign has story campaigns after 1202");
            yield return new WaitForSecondsRealtime(3f);
            Flow.Log($"demo end: 82205={QState(82205)} Lucas dialogs={NpcDialogs(700001)}");
            Assert.AreEqual("NotStarted", QState(82205), "sub quest 82205 (CampaignCleared:1202) is not offered");
            Assert.IsTrue(NpcDialogs(700001).Contains("82205"), $"Lucas has no 82205 dialog: {NpcDialogs(700001)}");
            float t0 = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t0 < 20f)
            {
                Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the game left the lounge after the last story");
                yield return new WaitForSecondsRealtime(1f);
            }
            Flow.Log($"demo end: scene={Flow.ActiveScene} top={Flow.TopPopup()} {Flow.LoadState()}");
            yield return Review.Shot("a6-demo-end-lounge");
            yield return OpenCampaignPopup();
            yield return new WaitForSecondsRealtime(2f);
            // With no new story campaign Elara's list opens on the Common tab (CampaignPopupPresenter.OnOpenAsync): switch to Story.
            var storyToggle = Flow.Get(Flow.Get(Flow.Find("CampaignPopupPresenter"), "_view"), "CategoryCommon_Toggle_Story") as UnityEngine.UI.Toggle;
            Assert.IsNotNull(storyToggle, "no Story tab in Elara's list");
            storyToggle.isOn = true;
            yield return new WaitForSecondsRealtime(2f);
            var slots = CampaignSlots();
            foreach (var sl in slots) Flow.Log($"demo end slot {Flow.Get(Flow.Get(sl, "_model"), "CampaignId")}: {SlotState(sl)}");
            foreach (var id in StoryOrder)
            {
                var sl = Slot(slots, id);
                Assert.IsNotNull(sl, $"no {id} slot in the final list");
                Assert.IsTrue(SlotState(sl).EndsWith("completed=True"), $"{id} not marked completed in the final list: {SlotState(sl)}");
            }
            yield return Review.Shot("a6-demo-end-list");
            yield return Flow.Keys(0.2f, Key.Escape);
        }

        static string NpcDialogs(int npcId)
        {
            var mgr = Flow.Find("GameNpcDialogManager");
            var list = mgr?.GetType().GetMethod("GetDialogInfos")?.Invoke(mgr, new object[] { npcId }) as IEnumerable;
            return list == null ? "-" : string.Join(",", list.Cast<object>().Select(d => $"{Flow.Get(d, "LuaKey") ?? Flow.Get(d, "Key")}/{Flow.Get(d, "FunctionName")}/{Flow.Get(d, "Purpose")}"));
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
            StartGuardian();
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
            if (panel == null && Flow.Find("KeyInteractionPopupView") != null)
            {
                // A locked box (TRewardBox KeyItemIds) opens the key popup (InGameScene.OnKeyInteractionRewardBoxAsync) and holds the
                // diver in the interaction until it is closed; with no key a player backs out (Esc -> ReqInputCancelInteract).
                Flow.Log($"loot {name}: locked, key popup open; Esc");
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find("KeyInteractionPopupView") == null, 4f, $"{name} key popup closed");
                yield break;
            }
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
                // Exit 1007 (TExit: UseExitCostTable FALSE, no CustomUnlockGoodsData) takes no 10001; the levels' ExitCost applies to the other exits. Logged anyway.
                {
                    var pmx = Flow.Find("PlayerManager");
                    int lvl = Flow.Get(pmx, "Level") is int lv ? lv : 0;
                    var costRow = GameType("GameCommon.Table.TExitCost")?.GetProperty("Table")?.GetValue(null);
                    object cost = null; try { cost = costRow?.GetType().GetProperty("Item")?.GetValue(costRow, new object[] { lvl }); } catch (Exception) { }
                    var inv10001 = InventoryIds().Split(',').FirstOrDefault(x => x.StartsWith("10001x")) ?? "none";
                    Flow.Log($"booth 1007 try {attempt}: level {lvl} ExitCost row=[{Flow.Get(cost, "UnlockGoodsData")}] (not used by 1007), item 10001 in the inventory: {inv10001}, state={State()} focus={Flow.Focus()} {MeState()} monsters within 8 m={Flow.Monsters().Count(x => Flow.Flat(x.transform.position - Me().transform.position).magnitude < 8f)}");
                }
                float gameStart = Time.time, realStart = Time.realtimeSinceStartup, nextHoldLog = realStart + 1f;
                string stateSeq = State();
                Flow.Hold(Key.F);
                while (Time.realtimeSinceStartup - realStart < 12f && !Over() && Flow.Find("EscapeSuccessPopupView") == null
                       && State() != "Escaped" && Flow.Get(exit, "ExitState")?.ToString() == "Activated")
                {
                    if (State() != stateSeq.Split('>').Last()) stateSeq += ">" + State();
                    if (Time.realtimeSinceStartup > nextHoldLog) { nextHoldLog += 1f; Flow.Log($"booth hold {Time.realtimeSinceStartup - realStart:F0}s: state={State()} focused={Flow.Focused(exit)} hp={Flow.Hp(Me())}"); }
                    yield return null;
                }
                Flow.Log($"booth hold state sequence: {stateSeq}");
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

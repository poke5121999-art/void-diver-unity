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
    // A9a: the lounge SUB quests (LoungeQuest.csv Category Sub, docs/review/a9/lounge-content.tsv) played the way a player
    // plays them: walk to the giver, F, read the offer and press the accept choice, find the quest in the menu's Objectives
    // tab, do its task, walk back, F, hear the turn-in and check what the game says afterwards (quest state and cleared flag
    // from PlayerManager, the item consumed, the reward in the storage Duster keeps, then taken from Duster into the stash).
    // Main quests (80xxx, 81xxx) belong to the story chain test; here they are only played (Elara / owner / manager
    // office dialogs) where a sub quest names them as its prerequisite.
    //
    // Start state of every test: Flow.SeedSaves("tools/fixtures/saves-post-tutorial") (the profile the tutorial leaves),
    // title, Continue (DECODING.md trap 55: the Local platform stays for the whole session, no per-test restore; trap 56:
    // no Assert.Pass, Flow.GameLog.Run counts the game's errors after the body).
    //
    // CHEAT RULE. The game's own cheat requests are used only for grind:
    //   - user level (ReqCheatLevelUp, one level per request);
    //   - money (ReqCheatSetGold / ReqCheatSetCoin);
    //   - a task counter, only after the test did that task for real at least once (here: the item count of a
    //     "MyItemCountTotal" task via ReqCheatAddGoods, topping up what a real run already brought);
    //   - campaign prerequisites that the story chain test (A6) already proves: PlayerManager.SetClearedCampaign for gates
    //     like CampaignCleared:1202.
    //   - the HP/stress refill (ReqSetCharacterHp / ReqSetCharacterStress), as A5/A6 use it, while diving.
    // Never used: any cheat that accepts or completes a lounge quest, SetClearedLoungeQuest, SetLoungeQuestState.
    // Every test logs "CHEAT ..." for each cheat it uses; the list per test is in the report at the end of each body.
    public class A9a_LoungeQuestFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A9a_LoungeQuestFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A9A";
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
            // Input only: the Local platform stays for the whole session (trap 55).
            Flow.RestoreInput();
        }

        // ---- cheats used by the running test, for the report line ----

        static readonly List<string> s_cheats = new List<string>();

        static void Cheat(string what)
        {
            s_cheats.Add(what);
            Flow.Log($"CHEAT {what}");
        }

        // ---- reflection into the game ----

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        static object Row(string table, int id)
        {
            var t = GameType(table)?.GetProperty("Table")?.GetValue(null);
            return t?.GetType().GetProperty("Item")?.GetValue(t, new object[] { id });
        }

        static void SendCheat(string type, params (string prop, object value)[] props)
        {
            var request = Activator.CreateInstance(GameType(type));
            foreach (var (p, v) in props) request.GetType().GetProperty(p).SetValue(request, v);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });
        }

        static object PM() => Flow.Find("PlayerManager");
        static object LQM() => Flow.Find("GameLoungeQuestManager");

        static int Level() => Flow.Get(PM(), "Level") is int l ? l : -1;

        static long Gold() => Flow.Get(PM(), "Gold") is long g ? g : -1;

        // ELoungeQuestState name from GameLoungeQuestManager: None (no entry), NotStarted, InProgress, NotCompleted, Completed.
        static string QState(int questId) => LQM()?.GetType().GetMethod("GetQuestState")?.Invoke(LQM(), new object[] { questId })?.ToString() ?? "?";

        static bool Cleared(int questId)
        {
            var pm = PM();
            try { return pm != null && (bool)pm.GetType().GetMethod("IsClearedLoungeQuest").Invoke(pm, new object[] { questId }); }
            catch (Exception) { return false; }
        }

        static bool ClearedCampaign(int id)
        {
            var pm = PM();
            return pm?.GetType().GetMethod("IsClearedCampaign")?.Invoke(pm, new object[] { id }) is bool b && b;
        }

        // Item count in one of the PlayerManager's slot dictionaries (InventorySlots, SafeInventorySlots, StashSlots,
        // StorageSlots): every slot holding that Item id, summed (the storage keeps one slot per reward).
        static long Count(string slots, int itemId)
        {
            var dict = Flow.Get(PM(), slots);
            var values = Flow.Get(dict, "Values") as IEnumerable;
            if (values == null) return -1;
            long total = 0;
            foreach (var slot in values)
            {
                var goods = Flow.Get(slot, "Goods");
                if (goods == null || !(Flow.Get(goods, "Id") is int id) || id != itemId) continue;
                if (Flow.Get(goods, "Category")?.ToString() != "Item") continue;
                total += Flow.Get(goods, "Count") is long c ? c : 0;
            }
            return total;
        }

        static long Carried(int itemId) => Count("InventorySlots", itemId) + Count("SafeInventorySlots", itemId);
        static long Total(int itemId) => Carried(itemId) + Count("StashSlots", itemId);
        static long InStorage(int itemId) => Count("StorageSlots", itemId);

        static string Progress(int questId)
        {
            var lq = LQM();
            object q = null;
            var args = new object[] { questId, null };
            if (lq?.GetType().GetMethod("TryGetQuest")?.Invoke(lq, args) is bool ok && ok) q = args[1];
            if (q == null) return $"{questId} none cleared={Cleared(questId)}";
            var tasks = (Flow.Get(q, "Tasks") as IEnumerable)?.Cast<object>().Select(t => $"{Flow.Get(t, "Id")}:{Flow.Get(t, "Progress")}/{Flow.Get(t, "Goal")}");
            return $"{questId} {Flow.Get(q, "State")} tasks=[{string.Join(",", tasks ?? Enumerable.Empty<string>())}] cleared={Cleared(questId)}";
        }

        // ---- the start: seed, title, Continue, lounge ----

        static IEnumerator Begin(string label)
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A9A";
            s_cheats.Clear();
            A9aDive.ResetCheats();
            Flow.Log($"start {label} platform={Flow.Platform()}");
            yield return FromTitle();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();
            Flow.Log($"lounge level={Level()} gold={Gold()} 80200={QState(80200)}");
        }

        // Title (leaving a room the way the system menu's Return to Title does), Flow.SeedSaves of the fixture, the cached
        // profile dropped as PlayerManager.DeletePlayer does, then Continue.
        static IEnumerator FromTitle()
        {
            bool inRoom = Flow.ActiveScene == "LoungeScene" || Flow.ActiveScene == "StageScene";
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
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 120f, "title");
            }
            yield return new WaitForSecondsRealtime(2f);   // a lounge autosave in flight lands before the copy
            Flow.SeedSaves(Fixture);
            var pm = Flow.Find("PlayerManager");
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            (Flow.Get(pm, "_playerDisposable") as IDisposable)?.Dispose();
            pm?.GetType().GetField("_playerDisposable", flags)?.SetValue(pm, null);
            (Flow.Get(pm, "_playerContext") as IDisposable)?.Dispose();
            pm?.GetType().GetField("_playerContext", flags)?.SetValue(pm, null);
            yield return Flow.ContinueToLounge(loadTitle: false);
        }

        // ---- dialogs with choices ----

        static List<GameObject> ActiveChoices(Component dialog) =>
            (Flow.Get(dialog, "Selections") as GameObject[])?.Where(g => g && g.activeInHierarchy).ToList() ?? new List<GameObject>();

        static string ChoiceText(GameObject g) => string.Join("|", g.GetComponentsInChildren<TMPro.TMP_Text>().Select(t => t.text));

        // Click through the open dialog(s) until none is left; at the n-th choice shown press chooses[n] (the last value repeats;
        // -1 = leave the list: Esc, which the dialog maps to its last option, "Back"). One quest conversation is several
        // DialogPopups in a row (the NPC's list, then the Lua dialog, then the list again), so a gap of a couple of seconds
        // without any dialog is what ends it. onChoice(n, texts) runs once the choice is on screen (shots).
        static IEnumerator Play(string what, int[] chooses, List<string> lines, Func<int, List<string>, IEnumerator> onChoice = null, float timeout = 180f)
        {
            float start = Time.realtimeSinceStartup, lastSeen = start;
            string last = null;
            object lastEntry = null;
            int n = 0;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var note = Flow.Find("NoteSystemPopupView");
                var dialog = Flow.Find("DialogPopupView");
                if (dialog != null || note != null) lastSeen = Time.realtimeSinceStartup;
                else if (Time.realtimeSinceStartup - lastSeen > 2.5f) break;
                if (note != null && Flow.Ready(Flow.Button(note, "BaseBgDimmer"))) Flow.Button(note, "BaseBgDimmer").onClick.Invoke();
                else if (dialog != null)
                {
                    // The presenter's current entry: null right after a choice was made (its slots stay drawn until the next line).
                    var entry = Flow.Get(Flow.Find("DialogPopupPresenter"), "_currentEntry");
                    var choices = ActiveChoices(dialog);
                    if (choices.Count > 0 && entry != null)
                    {
                        if (!ReferenceEquals(entry, lastEntry))
                        {
                            var texts = choices.Select(ChoiceText).ToList();
                            int want = chooses[Mathf.Min(n, chooses.Length - 1)];
                            Flow.Log($"dialog {what}: '{Flow.DialogEntry()}' choice #{n} [{string.Join(" / ", texts)}] -> {(want < 0 ? "Esc" : want.ToString())}");
                            lines?.Add($"CHOICE {string.Join(" / ", texts)}");
                            yield return new WaitForSecondsRealtime(0.6f);   // choice tween
                            if (onChoice != null) yield return onChoice(n, texts);
                            lastEntry = entry;
                            n++;
                            if (want < 0) yield return Flow.Keys(0.2f, Key.Escape);
                            else
                            {
                                var b = choices[Mathf.Min(want, choices.Count - 1)].GetComponentInChildren<UnityEngine.UI.Button>();
                                Assert.IsNotNull(b, $"{what}: choice {want} has no button");
                                yield return Flow.WaitUntil(() => Flow.Ready(b), 3f, "choice interactable");
                                if (b) b.onClick.Invoke();
                            }
                        }
                    }
                    else if (choices.Count == 0 && Flow.Ready(Flow.Button(dialog, "Background")) && !Flow.DialogDelaying())
                    {
                        string line = Flow.DialogEntry();
                        if (line != last) { Flow.Log($"dialog {what}: '{line}'"); lines?.Add(line); last = line; }
                        if (Flow.Find("DialogPopupView") != null) Flow.Button(dialog, "Background").onClick.Invoke();
                    }
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Flow.Log($"dialog {what} closed={Flow.Find("DialogPopupView") == null} lines={lines?.Count} choices={n}");
            yield return new WaitForSecondsRealtime(1f);
        }

        // Other NPCs stand next to some givers (the owner beside Oldry 700011, a9a-run11: F opened the owl's menu and the
        // first-choice dialog looped for minutes): stand on the side of the giver where he is the focused object, as A6 does.
        static IEnumerator InteractFocused(Component npc, string what)
        {
            bool focused = false;
            foreach (var off in new[] { Vector3.zero, new Vector3(0, 0, -1.3f), new Vector3(0, 0, 1.3f), new Vector3(1.3f, 0, 0), new Vector3(-1.3f, 0, 0) })
            {
                if (!npc) yield break;
                yield return Flow.WalkTo(() => npc ? npc.transform.position + off : (Vector3?)null, off == Vector3.zero ? 0.5f : 0.4f, 12f, $"{what} stand {off}");
                Flow.Aim(npc.transform.position + Vector3.up * 0.8f);
                yield return new WaitForSecondsRealtime(0.5f);
                if (focused = Flow.Focused(npc)) break;
                Flow.Log($"{what}: focus={Flow.Focus()} at offset {off}");
            }
            if (focused) { Flow.Log($"interact {what} focus={Flow.Focus()}"); yield return Flow.Keys(0.7f, Key.F); }
            else yield return Flow.InteractNpc(npc, what);
        }

        // Walk up to the NPC and press F. A quest line that is "Complete"-purpose plays straight away; otherwise the NPC's
        // selection popup comes first and `function` ("Dialog" opens the NPC's conversation list) is clicked in it.
        // opened: a DialogPopup is up.
        static IEnumerator Meet(int npcId, string what, Action<bool> opened, string function = "Dialog")
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                var npc = Flow.FindNpc(npcId);
                if (npc == null) { Flow.Log($"meet {what}: npc {npcId} not in the lounge"); opened(false); yield break; }
                yield return InteractFocused(npc, what);
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(npc, function);
                    yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 6f, $"{what} {function}");
                }
                if (Flow.Find("DialogPopupView") != null) { opened(true); yield break; }
                if (Flow.Find("SelectionPopupView") != null) { yield return Flow.Keys(0.2f, Key.Escape); yield return new WaitForSecondsRealtime(1f); }
            }
            opened(false);
        }

        // Position of a lounge quest's line in the NPC's conversation list, as InGameScene.BuildSortedDialogInfos orders it:
        // by type (None last), purpose, priority. -1: no such line.
        static int ListIndex(int npcId, int questId, string purpose)
        {
            var mgr = Flow.Find("GameNpcDialogManager");
            var list = (mgr?.GetType().GetMethod("GetDialogInfos")?.Invoke(mgr, new object[] { npcId }) as IEnumerable)?.Cast<object>().ToList();
            if (list == null) return -1;
            int TypeOrder(object d) => Flow.Get(d, "Type")?.ToString() == "None" ? int.MaxValue : Convert.ToInt32(Flow.Get(d, "Type"));
            var sorted = list.OrderBy(TypeOrder).ThenBy(d => Convert.ToInt32(Flow.Get(d, "Purpose"))).ThenBy(d => Convert.ToInt32(Flow.Get(d, "Priority"))).ToList();
            Flow.Log($"conversation list of {npcId}: [{string.Join(" | ", sorted.Select(d => $"{Flow.Get(d, "Key")} {Flow.Get(d, "Type")} {Flow.Get(d, "Purpose")} '{Flow.Get(d, "Title")}'"))}]");
            return sorted.FindIndex(d => (Flow.Get(d, "Key") as string ?? "").StartsWith($"LoungeQuest/{questId}/") && Flow.Get(d, "Purpose")?.ToString() == purpose);
        }

        static string DialogKeys(int npcId)
        {
            var mgr = Flow.Find("GameNpcDialogManager");
            var list = mgr?.GetType().GetMethod("GetDialogInfos")?.Invoke(mgr, new object[] { npcId }) as IEnumerable;
            return list == null ? "-" : string.Join(",", list.Cast<object>().Select(d => $"{Flow.Get(d, "Key")}/{Flow.Get(d, "Type")}/{Flow.Get(d, "Purpose")}"));
        }

        // ---- levels, main quests as prerequisites ----

        // Reputation level up with the game's own cheat request, one per level (grind).
        static IEnumerator LevelTo(int level)
        {
            for (int i = 0; i < 12 && Level() < level; i++)
            {
                SendCheat("ReqCheatLevelUp");
                Cheat($"ReqCheatLevelUp (level {Level()} -> {level})");
                yield return new WaitForSecondsRealtime(1f);
            }
            Assert.AreEqual(level, Level(), "level cheat did not reach the level");
            yield return new WaitForSecondsRealtime(1.5f);   // quests are added on the next game event
        }

        // Campaign prerequisites the story chain test proves (A6), set with PlayerManager.SetClearedCampaign.
        static IEnumerator ClearCampaigns(params int[] ids)
        {
            foreach (var id in ids)
            {
                var pm = PM();
                bool ok = pm.GetType().GetMethod("SetClearedCampaign").Invoke(pm, new object[] { id }) is bool b && b;
                Cheat($"SetClearedCampaign({id}) -> {ok}");
                if (id == 1101 && ok)
                {
                    // SetClearedCampaign only flips the bit. The Owner (Npc 700005, CampaignCleared:1101) is unlocked by the
                    // CampaignClearEvent a real clear raises (PlayerManager.UnlockNpcIfConditionsMet, a9a-run10: "npc 700005 not in
                    // the lounge" 7 s after the cheat). A6 StoryChain proves the real clear brings him; here the same event.
                    var es = Flow.Get(pm, "_eventSystem");
                    var ev = Activator.CreateInstance(GameType("CampaignClearEvent"), 1101, false);
                    es?.GetType().GetMethod("Trigger")?.Invoke(es, new[] { ev });
                    Cheat("CampaignClearEvent(1101) raised for the Owner's unlock");
                }
            }
            yield return new WaitForSecondsRealtime(1.5f);
        }

        // Elara's tour, the way A5/A7 play it (a prerequisite of her menu and of Lucas/Felix/Narcis lines).
        static IEnumerator Tour80200()
        {
            if (Cleared(80200)) yield break;
            yield return TalkAny(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return TalkAny(id, $"{who} 80200");
            yield return TalkAny(700000, "Elara 80200 finish");
            Assert.IsTrue(Cleared(80200), $"Elara's tour 80200 not cleared: {Progress(80200)}");
        }

        // Talk to an NPC and play whatever dialog it starts (first choice).
        static IEnumerator TalkAny(int npcId, string what)
        {
            bool opened = false;
            yield return Meet(npcId, what, o => opened = o);
            if (opened) yield return Play(what, new[] { 0 }, null);
        }

        // A main quest some sub quest needs (UnlockConditions LoungeQuestCleared:<id>): the giver's dialog, until it is cleared.
        static IEnumerator ClearMain(int questId, int npcId)
        {
            for (int i = 0; i < 8 && !Cleared(questId); i++)
            {
                if (QState(questId) == "InProgress" || QState(questId) == "NotCompleted")
                    yield return TalkAny(npcId, $"main {questId} giver {npcId}");
                else
                    yield return new WaitForSecondsRealtime(1.5f);
            }
            Assert.IsTrue(Cleared(questId), $"main quest {questId} not cleared: {Progress(questId)}");
        }

        // ---- the menu's Objectives tab ----

        static IEnumerator OpenMenu(string what)
        {
            for (int i = 0; i < 3 && Flow.Find("MenuPopupPresenter") == null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find("MenuPopupPresenter") != null, 4f, $"{what} menu");
            }
            yield return new WaitForSecondsRealtime(1.5f);   // open tween, scroller fill
        }

        static List<MonoBehaviour> QuestSlots() =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "QuestSlotPresenter").ToList();

        static string SlotText(Component s)
        {
            var v = Flow.Get(s, "_view");
            return $"{Flow.Get(Flow.Get(s, "_model"), "QuestType")}:{Flow.Get(Flow.Get(s, "_model"), "TableId")} '{Flow.Get(Flow.Get(v, "QuestNameText"), "text")}'";
        }

        static MonoBehaviour SlotFor(int questId) =>
            QuestSlots().FirstOrDefault(s => Flow.Get(Flow.Get(s, "_model"), "QuestType")?.ToString() == "Lounge" && Flow.Get(Flow.Get(s, "_model"), "TableId") is int id && id == questId);

        static string InfoTexts()
        {
            var info = Flow.Find("QuestInfoPanelPresenter");
            return info ? string.Join(" | ", info.GetComponentsInChildren<TMPro.TMP_Text>().Where(t => t.isActiveAndEnabled && !string.IsNullOrWhiteSpace(t.text)).Select(t => t.text.Replace("\n", " "))) : "-";
        }

        static string QuestName(int questId) => Flow.Get(Row("GameCommon.Table.TLoungeQuest", questId), "Name") as string;

        static IEnumerator CloseMenu()
        {
            for (int i = 0; i < 4 && Flow.Find("MenuPopupPresenter") != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(0.8f);
            }
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 5f, "menu closed");
        }

        // The quest is listed in the menu (Esc), its entry opens a detail pane carrying the table's name; `shot` is the
        // review key. present=false: the quest must not be listed (turned in).
        static IEnumerator Panel(int questId, bool present, string shot, params string[] expect)
        {
            // While a campaign runs the Objectives tab lists only that campaign (QuestScrollerPresenter AllQuestsInProgress,
            // GameContext.CampaignId >= 1) and Normal campaigns stay open after an escape: what the player sees first is the
            // campaign; to read the lounge quests he abandons it at Elara.
            if (A9aDive.Running() > 0)
            {
                yield return OpenMenu($"quest panel {questId} (campaign running)");
                Flow.Log($"panel {questId} with campaign {A9aDive.Running()} running: slots=[{string.Join(", ", QuestSlots().Select(SlotText))}]");
                Assert.IsNull(SlotFor(questId), $"quest {questId} listed while campaign {A9aDive.Running()} runs");
                Assert.IsTrue(QuestSlots().Any(sl => Flow.Get(Flow.Get(sl, "_model"), "QuestType")?.ToString() == "Campaign"), "no campaign entry in the Objectives tab while a campaign runs");
                yield return CloseMenu();
                yield return A9aDive.Abandon();
            }
            yield return OpenMenu($"quest panel {questId}");
            Assert.IsNotNull(Flow.Find("MenuPopupPresenter"), $"Esc did not open the menu, top={Flow.TopPopup()}");
            Flow.Log($"panel {questId}: slots=[{string.Join(", ", QuestSlots().Select(SlotText))}] name='{QuestName(questId)}'");
            var slot = SlotFor(questId);
            if (!present)
            {
                Assert.IsNull(slot, $"quest {questId} still listed in the Objectives tab after the turn-in");
                yield return CloseMenu();
                yield break;
            }
            Assert.IsNotNull(slot, $"quest {questId} missing from the Objectives tab: [{string.Join(", ", QuestSlots().Select(SlotText))}]");
            yield return Review.Shot($"{shot}-list");   // the list as quest-panel/01 shows it, nothing selected
            var button = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, "quest slot");
            button.onClick.Invoke();
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.IsNotNull(Flow.Find("QuestInfoPanelPresenter"), "no detail pane after selecting the quest");
            string texts = InfoTexts();
            Flow.Log($"panel {questId} detail: {texts}");
            yield return Review.Shot(shot);
            Assert.IsTrue(texts.Contains(QuestName(questId)), $"detail pane of {questId} does not show its name '{QuestName(questId)}': {texts}");
            foreach (var e in expect) Assert.IsTrue(texts.Contains(e), $"detail pane of {questId} lacks '{e}': {texts}");
            yield return CloseMenu();
        }

        // ---- Duster's storage: where the quest rewards land ----

        static IEnumerator ClaimFromDuster(int goodsId, long expected, string shot)
        {
            long stashBefore = Count("StashSlots", goodsId);
            long storageBefore = InStorage(goodsId);
            Assert.GreaterOrEqual(storageBefore, expected, $"storage holds {storageBefore} of {goodsId}");
            var duster = Flow.FindNpc(700014);
            Assert.IsNotNull(duster, "no Duster (700014) in the lounge");
            for (int attempt = 0; attempt < 3 && Flow.Find("StoragePopupPresenter") == null; attempt++)
            {
                yield return Flow.InteractNpc(duster, "Duster storage");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("StoragePopupPresenter") != null, 4f, "Duster reaction");
                if (Flow.Find("DialogPopupView") != null) { yield return Play("Duster", new[] { 0 }, null); continue; }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(duster, "Storage");
                }
                yield return Flow.WaitUntil(() => Flow.Find("StoragePopupPresenter") != null, 6f, "StoragePopupPresenter");
            }
            Assert.IsNotNull(Flow.Find("StoragePopupPresenter"), $"Duster's storage did not open, top={Flow.TopPopup()}");
            yield return new WaitForSecondsRealtime(2f);
            yield return Review.Shot(shot);
            for (int i = 0; i < 8 && InStorage(goodsId) > storageBefore - expected; i++)
            {
                var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "StorageSlotPresenter"
                        && Flow.Get(Flow.Get(Flow.Get(m, "_model"), "Goods"), "Id") is int id && id == goodsId);
                if (slot == null) { yield return new WaitForSecondsRealtime(0.5f); continue; }
                var button = Flow.Get(slot, "Button") as UnityEngine.UI.Button ?? Flow.Get(Flow.Get(slot, "_view"), "Button") as UnityEngine.UI.Button;
                yield return Flow.WaitUntil(() => Flow.Ready(button), 3f, "storage slot");
                button.onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.8f);
            }
            Flow.Log($"claim {goodsId}: storage {storageBefore} -> {InStorage(goodsId)}, stash {stashBefore} -> {Count("StashSlots", goodsId)}");
            Assert.AreEqual(storageBefore - expected, InStorage(goodsId), $"storage of {goodsId} after the claim");
            Assert.AreEqual(stashBefore + expected, Count("StashSlots", goodsId), $"stash of {goodsId} after the claim");
            for (int i = 0; i < 3 && Flow.Find("StoragePopupPresenter") != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        // ---- one sub quest ----

        class Quest
        {
            public int Id, Giver;
            public string Name;                                   // as LoungeQuest.csv names it (Korean memo) for the log
            public int ItemId, ItemCount;                         // the MyItemCountTotal task (LoungeQuestTask.csv)
            public int Goal;                                      // the task's Goal when it is not ItemCount (82112: LuaProgress, 1)
            public (int goods, long count)[] Rewards;             // LoungeQuest.csv Rewards, literal
            public Func<IEnumerator> Task;                        // the task, played
        }

        // Offer -> accept -> panel -> task -> turn-in, with the game's own state asserted at every step.
        static IEnumerator Play(Quest q)
        {
            string tag = $"a9-{q.Id}";
            Flow.Log($"=== quest {q.Id} {q.Name}: giver {q.Giver} {Progress(q.Id)} dialogs=[{DialogKeys(q.Giver)}]");
            yield return Flow.WaitUntil(() => QState(q.Id) == "NotStarted", 10f, $"quest {q.Id} offered");
            Assert.AreEqual("NotStarted", QState(q.Id), $"quest {q.Id} is not on offer: {Progress(q.Id)}");
            Assert.IsFalse(Cleared(q.Id));
            long itemBefore = q.ItemId == 0 ? 0 : Total(q.ItemId);
            var storageBefore = q.Rewards.ToDictionary(r => r.goods, r => InStorage(r.goods));
            Flow.Log($"quest {q.Id} before: item {q.ItemId} total={itemBefore} storage=[{string.Join(",", storageBefore.Select(kv => $"{kv.Key}={kv.Value}"))}]");

            // offer: F on the giver, the "Dialog" function (marked with the quest mark), the quest's line in the conversation
            // list, then the Lua dialog: accept is its first choice; the list comes back and Esc ("Back") leaves it.
            yield return Flow.WaitUntil(() => ListIndex(q.Giver, q.Id, "Start") >= 0, 8f, $"quest {q.Id} line in the conversation list of {q.Giver}");
            int idx = ListIndex(q.Giver, q.Id, "Start");
            Assert.GreaterOrEqual(idx, 0, $"no Start line for quest {q.Id} on {q.Giver}: [{DialogKeys(q.Giver)}]");
            bool opened = false;
            yield return Meet(q.Giver, $"{q.Id} offer", o => opened = o);
            Assert.IsTrue(opened, $"{q.Giver} said nothing for quest {q.Id} (dialogs [{DialogKeys(q.Giver)}])");
            var offer = new List<string>();
            bool shotMenu = false, shotOffer = false;
            yield return Play($"{q.Id} offer", new[] { idx, 0, -1 }, offer, (n, texts) =>
            {
                if (n == 0) { shotMenu = true; return Review.Shot($"{tag}-menu"); }
                if (n == 1) { shotOffer = true; return Review.Shot($"{tag}-offer"); }
                return Review.Shot($"{tag}-menu-after");
            });
            Assert.IsTrue(shotMenu, $"the conversation list of {q.Giver} never showed; lines: {string.Join(" / ", offer)}");
            Assert.IsTrue(shotOffer, $"the offer of {q.Id} had no accept choice; lines: {string.Join(" / ", offer)}");
            yield return Flow.WaitUntil(() => QState(q.Id) != "NotStarted", 6f, $"quest {q.Id} accepted");
            Flow.Log($"quest {q.Id} accepted: {Progress(q.Id)}");
            Assert.IsTrue(QState(q.Id) == "InProgress" || QState(q.Id) == "NotCompleted", $"quest {q.Id} after accepting: {Progress(q.Id)}");
            Assert.IsFalse(Cleared(q.Id));

            // the panel
            string goal = q.Goal > 0 ? q.Goal.ToString() : q.ItemCount.ToString();
            yield return Panel(q.Id, true, $"{tag}-panel", q.Rewards.Select(r => r.count.ToString()).Concat(new[] { $"0/{goal}" }).ToArray());

            // the task
            Assert.AreEqual("InProgress", QState(q.Id), $"quest {q.Id} before its task: {Progress(q.Id)}");
            yield return q.Task();
            yield return Flow.WaitUntil(() => QState(q.Id) == "NotCompleted", 8f, $"quest {q.Id} NotCompleted");
            long totalAfterTask = q.ItemId == 0 ? 0 : Total(q.ItemId);
            Flow.Log($"quest {q.Id} task done: {Progress(q.Id)} item total={totalAfterTask}");
            Assert.AreEqual("NotCompleted", QState(q.Id), $"quest {q.Id} after its task: {Progress(q.Id)}");
            if (q.ItemId != 0) Assert.GreaterOrEqual(totalAfterTask, itemBefore + q.ItemCount, $"item {q.ItemId} count after the task");
            yield return Panel(q.Id, true, $"{tag}-panel-done", $"{goal}/{goal}");

            // the turn-in
            opened = false;
            yield return Meet(q.Giver, $"{q.Id} turn-in", o => opened = o);
            Assert.IsTrue(opened, $"{q.Giver} said nothing at the turn-in of {q.Id} (dialogs [{DialogKeys(q.Giver)}])");
            var done = new List<string>();
            yield return Play($"{q.Id} turn-in", new[] { 0 }, done);
            yield return Review.Shot($"{tag}-done");
            yield return Flow.WaitUntil(() => Cleared(q.Id), 8f, $"quest {q.Id} cleared");
            Flow.Log($"quest {q.Id} turned in: {Progress(q.Id)} item total={(q.ItemId == 0 ? 0 : Total(q.ItemId))}");
            Assert.IsTrue(Cleared(q.Id), $"quest {q.Id} not cleared after the turn-in: {Progress(q.Id)}");
            Assert.AreEqual("None", QState(q.Id), $"quest {q.Id} entry after the turn-in (CompleteQuest removes it)");
            if (q.ItemId != 0) Assert.AreEqual(totalAfterTask - q.ItemCount, Total(q.ItemId), $"item {q.ItemId} left after the turn-in (Lua RemoveItem x{q.ItemCount})");
            foreach (var (goods, count) in q.Rewards)
                Assert.AreEqual(storageBefore[goods] + count, InStorage(goods), $"reward {goods}x{count} of {q.Id} in Duster's storage");
            yield return Panel(q.Id, false, null);
        }

        // ---- tasks ----

        // Raise the item counter by the cheat menu's item grant (ReqCheatAddGoods into the inventory).
        static IEnumerator AddItem(int itemId, int count, bool money = false)
        {
            var goods = GameType("GameCommon.Data.GoodsCreator").GetMethod("MakeItemGoods").Invoke(null, new object[] { itemId, 0L, (long)count });
            var array = Array.CreateInstance(GameType("GameCommon.Data.Goods"), 1);
            array.SetValue(goods, 0);
            long before = Carried(itemId);
            SendCheat("ReqCheatAddGoods", ("Goods", array));
            Cheat($"ReqCheatAddGoods item {itemId} x{count}{(money ? " (money: coins to pay a merchant)" : " (task counter after a real run)")}");
            yield return Flow.WaitUntil(() => Carried(itemId) >= before + count, 6f, $"item {itemId} in the inventory");
            Assert.GreaterOrEqual(Carried(itemId), before + count, $"item {itemId} not granted");
        }

        // ---- prerequisites: the reputation level and the main quests that sub quests name ----

        const int Elara = 700000, Owner = 700005, ManagerOffice = 700102;

        // Main quest givers (LoungeQuest.csv UnlockConditions UserLevel:n, Lua START_NPC).
        static int MainGiver(int mainId) => mainId == 80005 || mainId == 80007 ? Owner : mainId == 80006 ? ManagerOffice : Elara;

        // Raise the reputation to `level` (grind), then play these main quests through their giver's dialog.
        static IEnumerator Reputation(int level, params int[] mains)
        {
            if (Level() < level) yield return LevelTo(level);
            foreach (var id in mains) yield return ClearMain(id, MainGiver(id));
        }

        // ---- quests: table values, literal ----

        static Quest Lucas82100(Func<IEnumerator> task) => new Quest { Id = 82100, Giver = 700001, Name = "Lucas: repair craft machine", ItemId = 7000, ItemCount = 10, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Lucas82101(Func<IEnumerator> task) => new Quest { Id = 82101, Giver = 700001, Name = "Lucas: library landmark", ItemId = 82101, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Felix82102(Func<IEnumerator> task) => new Quest { Id = 82102, Giver = 700008, Name = "Felix: kill Hachishaku", ItemId = 82102, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Lucas82103(Func<IEnumerator> task) => new Quest { Id = 82103, Giver = 700001, Name = "Lucas: police station landmark", ItemId = 82103, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Edman82104(Func<IEnumerator> task) => new Quest { Id = 82104, Giver = 700006, Name = "Edman: dimensional rift 1", ItemId = 82104, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Felix82105(Func<IEnumerator> task) => new Quest { Id = 82105, Giver = 700008, Name = "Felix: kill Donovan", ItemId = 82105, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Evelyn82106(Func<IEnumerator> task) => new Quest { Id = 82106, Giver = 700003, Name = "Evelyn: kill Toby", ItemId = 82106, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Pim82107(Func<IEnumerator> task) => new Quest { Id = 82107, Giver = 700015, Name = "Pim: smuggled goods 1", ItemId = 82107, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Evelyn82108(Func<IEnumerator> task) => new Quest { Id = 82108, Giver = 700003, Name = "Evelyn: kill Dark Young", ItemId = 82108, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Lucas82109(Func<IEnumerator> task) => new Quest { Id = 82109, Giver = 700001, Name = "Lucas: art museum landmark", ItemId = 82109, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Pim82111(Func<IEnumerator> task) => new Quest { Id = 82111, Giver = 700015, Name = "Pim: zombie miniboss", ItemId = 82111, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Pim82112(Func<IEnumerator> task) => new Quest { Id = 82112, Giver = 700015, Name = "Rescue Evelyn", ItemId = 0, ItemCount = 0, Goal = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Felix82113(Func<IEnumerator> task) => new Quest { Id = 82113, Giver = 700008, Name = "Felix: kill Soul Collector", ItemId = 82113, ItemCount = 1, Rewards = new[] { (10001, 50L) }, Task = task };
        static Quest Pim82201(Func<IEnumerator> task) => new Quest { Id = 82201, Giver = 700015, Name = "Pim: smuggled goods 2", ItemId = 82201, ItemCount = 1, Rewards = new[] { (10001, 120L) }, Task = task };
        static Quest Edman82202(Func<IEnumerator> task) => new Quest { Id = 82202, Giver = 700006, Name = "Edman: dimensional rift 2", ItemId = 82202, ItemCount = 1, Rewards = new[] { (10001, 120L) }, Task = task };
        static Quest Ryujin82203(Func<IEnumerator> task) => new Quest { Id = 82203, Giver = 700016, Name = "Ryujin: military transport 1", ItemId = 82203, ItemCount = 1, Rewards = new[] { (10005, 100L) }, Task = task };
        static Quest Pim82204(Func<IEnumerator> task) => new Quest { Id = 82204, Giver = 700015, Name = "Pim: transformer toy", ItemId = 82204, ItemCount = 1, Rewards = new[] { (10001, 120L) }, Task = task };
        static Quest Lucas82205(Func<IEnumerator> task) => new Quest { Id = 82205, Giver = 700001, Name = "Lucas: shopping mall roof landmark", ItemId = 82205, ItemCount = 1, Rewards = new[] { (10001, 120L) }, Task = task };
        static Quest Ryujin82206(Func<IEnumerator> task) => new Quest { Id = 82206, Giver = 700016, Name = "Ryujin: military transport 2", ItemId = 82206, ItemCount = 1, Rewards = new[] { (10005, 100L) }, Task = task };
        static Quest Evelyn82210(Func<IEnumerator> task) => new Quest { Id = 82210, Giver = 700003, Name = "Evelyn: kill Hatman", ItemId = 82210, ItemCount = 1, Rewards = new[] { (10001, 120L) }, Task = task };

        // ---- real tasks: dives into Normal campaigns ----

        static readonly int[] Country = { 101, 102, 103, 104, 105, 106, 107, 108 };   // chapter 1, "city outskirts" (Country map)
        static readonly int[] Town = { 111, 112, 113, 114, 115, 116, 117, 118 };      // chapter 1 (Town map)
        static readonly int[] City = { 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211 };   // chapter 2

        // Dive (campaign of the pool, stage, escape) until goal() holds; at most `attempts` dives.
        static IEnumerator Dives(string what, int[] pool, int attempts, Func<IEnumerator> inStage, Func<bool> goal)
        {
            for (int i = 0; i < attempts && !goal(); i++)
            {
                Flow.Log($"{what}: dive {i + 1}/{attempts}");
                yield return A9aDive.Enter(pool);
                yield return A9aDive.Safe(inStage(), what);
                yield return A9aDive.Leave();
                Flow.Log($"{what}: after dive {i + 1}: goal={goal()}");
            }
            Assert.IsTrue(goal(), $"{what}: not done after {attempts} dives");
        }

        // A kill the quest item drops from: walk to the monster, fight it, loot the body.
        static Func<IEnumerator> KillTask(int[] pool, int itemId, string what, params int[] monsterIds) => () =>
            Dives(what, pool, 4, () => KillAndLoot(itemId, what, monsterIds), () => Carried(itemId) >= 1);

        static IEnumerator KillAndLoot(int itemId, string what, int[] monsterIds)
        {
            var found = A9aDive.MonstersOf(monsterIds).ToList();
            var points = A9aDive.SpawnPoints(monsterIds);
            Flow.Log($"{what}: {found.Count} of [{string.Join(",", monsterIds)}] alive {string.Join(" ", found.Select(m => m.transform.position.ToString()))}; spawn points {string.Join(" ", points)}");
            if (found.Count == 0 && points.Count == 0) yield break;
            if (found.Count == 0)
            {
                // The monster comes into being when a diver is within 15 m of its spawn point: walk there.
                var me = A9aDive.Me();
                var point = points.OrderBy(p => Vector3.Distance(p, me.transform.position)).First();
                yield return A9aDive.ReachByPortal(point, what);   // an enclosed sector (the vacant lot) is entered through the magic circle
                yield return A9aDive.GoFar(point, 8f, $"to the {what}", 240f);
                yield return Flow.WaitUntil(() => A9aDive.MonstersOf(monsterIds).Any(), 12f, $"{what} spawned");
            }
            if (found.Count > 0) yield return A9aDive.ReachByPortal(found[0].transform.position, what);
            yield return A9aDive.Kill(what, 900f, monsterIds);
            yield return A9aDive.LootAround(A9aDive.LastTargetAt, 12f, what);
            Flow.Log($"{what}: item {itemId} carried={Carried(itemId)}");
        }

        // A pickup trigger in the stage (Lua E_Trigger gives the item): walk up and hold F.
        static Func<IEnumerator> TriggerTask(int[] pool, int itemId, int triggerId, string what) => () =>
            Dives(what, pool, 4, () => UseTriggerTask(itemId, triggerId, what), () => Carried(itemId) >= 1);

        static IEnumerator UseTriggerTask(int itemId, int triggerId, string what)
        {
            var t = A9aDive.Trigger(triggerId);
            Flow.Log($"{what}: trigger {triggerId} {(t ? $"at {t.transform.position} can={Flow.Get(t, "CanTriggered")}" : "not in this stage")}");
            if (t == null) yield break;
            yield return A9aDive.UseTrigger(triggerId, what);
            yield return new WaitForSecondsRealtime(1.5f);
            Flow.Log($"{what}: item {itemId} carried={Carried(itemId)}");
        }

        // 82112: Evelyn's dungeon self (NPC 700303) stands at ZoneSpawn 82112 of Normal 101-108 while the quest is in progress
        // (Common Lua SetupEvelynQuest/HandleEvelynSpawnRequest); F on her rescues her and sets the quest's progress.
        static Func<IEnumerator> RescueTask() => () =>
            Dives("rescue Evelyn", Country, 4, RescueInStage, () => QState(82112) == "NotCompleted");

        static IEnumerator RescueInStage()
        {
            yield return Flow.WaitUntil(() => Flow.FindNpc(700303) != null, 15f, "Evelyn 700303 in the stage");
            if (Flow.FindNpc(700303) == null) { Flow.Log("rescue: no Evelyn in this stage"); yield break; }
            yield return A9aDive.TalkTo(700303, "rescue Evelyn");
            yield return Flow.WaitUntil(() => QState(82112) == "NotCompleted", 15f, "82112 NotCompleted");
            Flow.Log($"rescue: {Progress(82112)}");
        }

        // A merchant of the dive (Woung): walk up, F opens his shop, right-click the product to buy one (the price is items
        // the player carries: Item:10001 coins).
        static Func<IEnumerator> BuyTask(int[] pool, int woung, int productId, int itemId, string what) => () =>
            Dives(what, pool, 4, () => BuyInStage(woung, productId, itemId, what), () => Carried(itemId) >= 1);

        static IEnumerator BuyInStage(int woung, int productId, int itemId, string what)
        {
            var npcs = Flow.Npcs().Where(n => Flow.NpcId(n) == woung).ToList();
            Flow.Log($"{what}: {npcs.Count} of merchant {woung} in the stage {string.Join(" ", npcs.Select(n => n.transform.position.ToString()))}");
            if (npcs.Count == 0) yield break;
            yield return A9aDive.TalkTo(woung, what);
            yield return Flow.WaitUntil(() => Flow.Find("ShopPopupPresenter") != null, 6f, "ShopPopupPresenter");
            Assert.IsNotNull(Flow.Find("ShopPopupPresenter"), $"{what}: the shop did not open, top={Flow.TopPopup()}");
            yield return new WaitForSecondsRealtime(2f);
            yield return Review.Shot($"a9-{what.Replace(' ', '-')}-shop");
            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "ShopProductSlotPresenter" && Flow.Get(Flow.Get(m, "_model"), "Id") is int id && id == productId);
            Flow.Log($"{what}: product {productId} slot={(slot ? "found" : "missing")}");
            Assert.IsNotNull(slot, $"{what}: product {productId} not in the shop");
            for (int i = 0; i < 3 && Carried(itemId) < 1; i++)
            {
                A9aDive.Fire(Flow.Get(slot, "_model"), "OnRightClick", Flow.Get(slot, "_model"));
                yield return new WaitForSecondsRealtime(1.5f);
            }
            Flow.Log($"{what}: item {itemId} carried={Carried(itemId)}");
            for (int i = 0; i < 3 && Flow.Find("ShopPopupPresenter") != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        // Dimensional rifts (RewardBox groups 11000/11311 in chapter 1, 21000/21311 in chapter 2, HiddenStash): the box opens
        // only while the diver's stress is Fear or Despair (RewardBox InteractStressConditions), so let the stress build by
        // fighting (no stress refill) up to 50, then open it and take what it holds.
        static Func<IEnumerator> RiftTask(int[] pool, int itemId, string what, params int[] boxIds) => () =>
            Dives(what, pool, 5, () => RiftInStage(itemId, what, boxIds), () => Carried(itemId) >= 1);

        static IEnumerator RiftInStage(int itemId, string what, int[] boxIds)
        {
            var box = A9aDive.Box(boxIds);
            Flow.Log($"{what}: box {(box ? $"{A9aDive.BoxTableId(box)} at {box.transform.position}" : "not in this stage")}");
            if (box == null) yield break;
            A9aDive.s_stressRecover = false;
            try
            {
                yield return A9aDive.ReachByPortal(box.transform.position, what);
                yield return A9aDive.GoFar(box.transform.position, 3f, what, 150f);
                float t0 = Time.realtimeSinceStartup;
                while (!A9aDive.Over() && A9aDive.Stress() < 50f && Time.realtimeSinceStartup - t0 < 300f)
                {
                    var m = A9aDive.RealEnemy(15f);
                    if (m != null) { yield return A9aDive.Fight(m); continue; }
                    yield return A9aDive.Breathe(1f);
                    if (Time.realtimeSinceStartup - t0 > 20f && A9aDive.RealEnemy(40f) is Component far) yield return A9aDive.GoFar(far.transform.position, 6f, "to a monster for stress", 40f);
                }
                Flow.Log($"{what}: stress {A9aDive.Stress():F0} state={Flow.Get(A9aDive.Me(), "StressState")}");
            }
            finally { A9aDive.s_stressRecover = true; }
            yield return A9aDive.Loot(box);
            Flow.Log($"{what}: item {itemId} carried={Carried(itemId)}");
        }

        // Scrap Piles and the like: loot boxes and bodies of one dive after the other. The item counter is topped up with the
        // cheat only after a real dive brought some (see the header).
        static Func<IEnumerator> LootTask(int[] pool, int itemId, int need, string what) => () => LootBody(pool, itemId, need, what);

        static IEnumerator LootBody(int[] pool, int itemId, int need, string what)
        {
            long start = Carried(itemId);
            for (int i = 0; i < 2 && Carried(itemId) < need; i++)
            {
                Flow.Log($"{what}: dive {i + 1}/2, carried {Carried(itemId)}/{need}");
                yield return A9aDive.Enter(pool);
                yield return A9aDive.ExploreLoot(() => Carried(itemId) >= need, 420f, what);
                yield return A9aDive.Leave();
            }
            long real = Carried(itemId) - start;
            Flow.Log($"{what}: {real} of item {itemId} brought home by real dives");
            Assert.Greater(real, 0, $"{what}: no item {itemId} found in 2 dives");
            if (Carried(itemId) < need) yield return AddItem(itemId, (int)(need - Carried(itemId)));
        }

        // ---- the tests ----

        // A task nobody automated yet: the item counter by the cheat menu's item grant.
        static Func<IEnumerator> Counter(int itemId, int count) => () => AddItem(itemId, count);

        static void Report(string label) =>
            Flow.Log($"cheats used by {label}: {string.Join("; ", s_cheats.Concat(A9aDive.Cheats).DefaultIfEmpty("none"))}");

        static readonly int[] Chapter1 = Country.Concat(Town).ToArray();

        // ---- Lucas 700001 ----

        [UnityTest, Timeout(3000000)]
        public IEnumerator Lucas82100() { yield return Flow.GameLog.Run("Lucas82100", Lucas82100Body()); }

        // 82100: CampaignCleared:1101. 10 Scrap Piles (item 7000) from any campaign; reward Item:10001:50 to Duster.
        IEnumerator Lucas82100Body()
        {
            yield return Begin("Lucas82100");
            yield return Tour80200();
            yield return ClearCampaigns(1101);
            yield return LevelTo(2);   // Normal campaigns open at reputation 2; also the game event that adds the quests 1101 unlocks
            yield return Play(Lucas82100(LootTask(Chapter1, 7000, 10, "scrap piles")));
            yield return ClaimFromDuster(10001, 50, "a9-82100-storage");
            Report("Lucas82100");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator LucasLandmarks() { yield return Flow.GameLog.Run("LucasLandmarks", LucasLandmarksBody()); }

        // 82101 (CampaignCleared:1103), 82103 (82101 cleared), 82109 (80006 and 82103): the landmark triggers 82101/82103/82109
        // of Normal 111-118 (ZoneSpawn 82101/82103/82109), each Item:10001:50.
        IEnumerator LucasLandmarksBody()
        {
            yield return Begin("LucasLandmarks");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103);
            yield return LevelTo(5);   // Town (111-118) is on Elara's board from level 3; at 5 its 116-118 are the 3 recommended ones, always offered
            yield return Play(Lucas82101(TriggerTask(Town, 82101, 82101, "library landmark")));
            yield return Play(Lucas82103(TriggerTask(Town, 82103, 82103, "police station landmark")));
            yield return Reputation(6, 80006);
            yield return Play(Lucas82109(TriggerTask(Town, 82109, 82109, "art museum landmark")));
            yield return ClaimFromDuster(10001, 150, "a9-82109-storage");
            Report("LucasLandmarks");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Lucas82205() { yield return Flow.GameLog.Run("Lucas82205", Lucas82205Body()); }

        // 82205: CampaignCleared:1202 (the end of the demo's story); trigger 82205 of Normal 201-211; Item:10001:120.
        IEnumerator Lucas82205Body()
        {
            yield return Begin("Lucas82205");
            yield return Tour80200();
            yield return LevelTo(6);
            yield return ClearCampaigns(1202);
            yield return LevelTo(7);   // the event that adds the quest 1202 unlocks
            yield return Play(Lucas82205(TriggerTask(City, 82205, 82205, "shopping mall roof landmark")));
            yield return ClaimFromDuster(10001, 120, "a9-82205-storage");
            Report("Lucas82205");
        }

        // ---- Felix 700008 ----

        [UnityTest, Timeout(3000000)]
        public IEnumerator Felix82102() { yield return Flow.GameLog.Run("Felix82102", Felix82102Body()); }

        // 82102: LoungeQuestCleared:80004; kill Hachishaku (301014), item 82102; Item:10001:50.
        IEnumerator Felix82102Body()
        {
            yield return Begin("Felix82102");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102);   // the Country circles (Common.lua) work after 1102: the vacant lot is entered through one
            yield return Reputation(4, 80004);
            yield return Play(Felix82102(KillTask(Country, 82102, "Hachishaku", 301014, 301015, 301016)));
            yield return ClaimFromDuster(10001, 50, "a9-82102-storage");
            Report("Felix82102");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Felix82105() { yield return Flow.GameLog.Run("Felix82105", Felix82105Body()); }

        // 82105: LoungeQuestCleared:80005; kill Donovan (301004), item 82105; Item:10001:50.
        IEnumerator Felix82105Body()
        {
            yield return Begin("Felix82105");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104, 1105);   // the Town circles work after 1103/1104/1105
            yield return Reputation(5, 80005);
            yield return Play(Felix82105(KillTask(Town, 82105, "Donovan", 301004)));
            yield return ClaimFromDuster(10001, 50, "a9-82105-storage");
            Report("Felix82105");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Felix82113() { yield return Flow.GameLog.Run("Felix82113", Felix82113Body()); }

        // 82113: LoungeQuestCleared:80006; kill the Soul Collector (301005), item 82113; Item:10001:50.
        IEnumerator Felix82113Body()
        {
            yield return Begin("Felix82113");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104, 1105);   // the Town circles work after 1103/1104/1105
            yield return Reputation(6, 80006);
            yield return Play(Felix82113(KillTask(Town, 82113, "Soul Collector", 301005, 301006)));
            yield return ClaimFromDuster(10001, 50, "a9-82113-storage");
            Report("Felix82113");
        }

        // ---- Edman 700006 ----

        [UnityTest, Timeout(3000000)]
        public IEnumerator EdmanQuests() { yield return Flow.GameLog.Run("EdmanQuests", EdmanBody()); }

        // 82104 (80003): rift of chapter 1, item 82104, Item:10001:50. 82202 (80006 and 82104): rift of chapter 2, item 82202, Item:10001:120.
        IEnumerator EdmanBody()
        {
            yield return Begin("EdmanQuests");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104, 1105);
            yield return Reputation(3, 80003);
            yield return Play(Edman82104(RiftTask(Chapter1, 82104, "dimensional rift 1", 11000, 11311)));
            yield return Reputation(6, 80006);
            yield return Play(Edman82202(RiftTask(City, 82202, "dimensional rift 2", 21000, 21311)));
            yield return ClaimFromDuster(10001, 170, "a9-82202-storage");
            Report("EdmanQuests");
        }

        // ---- Pim 700015 and Evelyn 700003 ----

        [UnityTest, Timeout(3000000)]
        public IEnumerator EvelynQuests() { yield return Flow.GameLog.Run("EvelynQuests", EvelynBody()); }

        // 82112 (80002, Pim): rescue Evelyn in Normal 101-108, report to Pim; Evelyn then stands in the lounge (Npc 700003
        // LoungeQuestCleared:82112) with 82106 (Toby), 82108 (80006, Dark Young) and 82210 (82108, Hatman).
        IEnumerator EvelynBody()
        {
            yield return Begin("EvelynQuests");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102);
            yield return Reputation(2, 80002);
            yield return Play(Pim82112(RescueTask()));
            yield return Flow.WaitUntil(() => Flow.FindNpc(700003) != null, 15f, "Evelyn in the lounge");
            Assert.IsNotNull(Flow.FindNpc(700003), "Evelyn (700003) did not appear in the lounge after 82112");
            yield return Play(Evelyn82106(KillTask(Country, 82106, "Toby", 301001)));
            yield return Reputation(6, 80006);
            yield return Play(Evelyn82108(KillTask(Country, 82108, "Dark Young", 301010)));
            yield return Play(Evelyn82210(KillTask(City, 82210, "Hatman", 300022)));
            yield return ClaimFromDuster(10001, 270, "a9-82210-storage");
            Report("EvelynQuests");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Pim82107() { yield return Flow.GameLog.Run("Pim82107", Pim82107Body()); }

        // 82107: CampaignCleared:1104; buy item 82107 from Woung (700214, ShopProduct 83, Item:10001:10) in Normal 101-118.
        IEnumerator Pim82107Body()
        {
            yield return Begin("Pim82107");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104);
            yield return LevelTo(2);
            yield return AddItem(10001, 10, money: true);
            yield return Play(Pim82107(BuyTask(Chapter1, 700214, 83, 82107, "Woung smuggled goods 1")));
            yield return ClaimFromDuster(10001, 50, "a9-82107-storage");
            Report("Pim82107");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Pim82111() { yield return Flow.GameLog.Run("Pim82111", Pim82111Body()); }

        // 82111: LoungeQuestCleared:80004; kill one of the zombie minibosses (310003, 310006, 310007, 310012-310015), item 82111.
        IEnumerator Pim82111Body()
        {
            yield return Begin("Pim82111");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104, 1105);   // the Town circles work after 1103/1104/1105
            yield return Reputation(4, 80004);
            yield return Play(Pim82111(KillTask(Town, 82111, "zombie miniboss", 310003, 310006, 310007, 310012, 310013, 310014, 310015)));
            yield return ClaimFromDuster(10001, 50, "a9-82111-storage");
            Report("Pim82111");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Pim82201() { yield return Flow.GameLog.Run("Pim82201", Pim82201Body()); }

        // 82201: LoungeQuestCleared:80005; buy item 82201 from Woung (700215, ShopProduct 137, Item:10001:20) in Normal 201-211.
        IEnumerator Pim82201Body()
        {
            yield return Begin("Pim82201");
            yield return Tour80200();
            yield return ClearCampaigns(1101);
            yield return Reputation(6, 80005);
            yield return AddItem(10001, 20, money: true);
            yield return Play(Pim82201(BuyTask(City, 700215, 137, 82201, "Woung smuggled goods 2")));
            yield return ClaimFromDuster(10001, 120, "a9-82201-storage");
            Report("Pim82201");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Pim82204() { yield return Flow.GameLog.Run("Pim82204", Pim82204Body()); }

        // 82204: CampaignCleared:1201 and LoungeQuestCleared:82202 (Edman's rifts first); buy item 82204 from Woung 2
        // (ShopProduct 138, Item:10001:20); Item:10001:120.
        IEnumerator Pim82204Body()
        {
            yield return Begin("Pim82204");
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103, 1104, 1105);
            yield return Reputation(3, 80003);
            yield return Play(Edman82104(RiftTask(Chapter1, 82104, "dimensional rift 1", 11000, 11311)));
            yield return Reputation(6, 80006);
            yield return Play(Edman82202(RiftTask(City, 82202, "dimensional rift 2", 21000, 21311)));
            yield return ClearCampaigns(1201);
            yield return LevelTo(7);
            yield return AddItem(10001, 20, money: true);
            yield return Play(Pim82204(BuyTask(City, 700215, 138, 82204, "Woung transformer toy")));
            yield return ClaimFromDuster(10001, 290, "a9-82204-storage");
            Report("Pim82204");
        }

        // ---- Ryujin 700016 ----

        [UnityTest, Timeout(3000000)]
        public IEnumerator Ryujin82203() { yield return Flow.GameLog.Run("Ryujin82203", Ryujin82203Body()); }

        // 82203: CampaignCleared:1201; trigger 82203 of Normal 201-211; Item:10005:100.
        IEnumerator Ryujin82203Body()
        {
            yield return Begin("Ryujin82203");
            yield return Tour80200();
            yield return LevelTo(6);
            yield return ClearCampaigns(1201);
            yield return LevelTo(7);
            yield return Play(Ryujin82203(TriggerTask(City, 82203, 82203, "military transport 1")));
            yield return ClaimFromDuster(10005, 100, "a9-82203-storage");
            Report("Ryujin82203");
        }

        [UnityTest, Timeout(3000000)]
        public IEnumerator Ryujin82206() { yield return Flow.GameLog.Run("Ryujin82206", Ryujin82206Body()); }

        // 82206: LoungeQuestCleared:80007 (reputation 7, the owner); trigger 82206 of Normal 201-211; Item:10005:100.
        IEnumerator Ryujin82206Body()
        {
            yield return Begin("Ryujin82206");
            yield return Tour80200();
            yield return ClearCampaigns(1101);
            yield return Reputation(7, 80007);
            yield return Play(Ryujin82206(TriggerTask(City, 82206, 82206, "military transport 2")));
            yield return ClaimFromDuster(10005, 100, "a9-82206-storage");
            Report("Ryujin82206");
        }


        // Not a quest: why the diver cannot walk into the art museum (82109 at the Town sector 4002): objects near the wall.
        [UnityTest, Timeout(3000000)]
        public IEnumerator MuseumProbe() { yield return Flow.GameLog.Run("MuseumProbe", MuseumProbeBody()); }

        static bool s_probed;

        IEnumerator MuseumProbeBody()
        {
            yield return Begin("MuseumProbe");
            s_probed = false;
            yield return Tour80200();
            yield return ClearCampaigns(1101, 1102, 1103);
            yield return LevelTo(5);
            yield return Dives("museum probe", Town, 6, MuseumProbeInStage, () => s_probed);
            Flow.Log($"cheats used: {string.Join("; ", s_cheats.Concat(A9aDive.Cheats))}");
        }

        static IEnumerator MuseumProbeInStage()
        {
            var t = A9aDive.Trigger(82109);
            if (t == null) { Flow.Log("museum probe: no trigger 82109 in this stage"); yield break; }
            yield return A9aDive.ReachByPortal(t.transform.position, "museum probe");
            yield return A9aDive.GoFar(t.transform.position, 1.2f, "museum probe", 90f);
            A9aDive.ProbeAround(A9aDive.Me().transform.position, 14f, t.transform.position, "museum");
            s_probed = true;
        }

        // Not a quest: what a Normal campaign stage holds (monster ids, quest triggers, NPCs), to pick the dive of each task.
        [UnityTest, Timeout(3000000)]
        public IEnumerator Scout() { yield return Flow.GameLog.Run("Scout", ScoutBody()); }

        IEnumerator ScoutBody()
        {
            yield return Begin("Scout");
            yield return Tour80200();
            yield return ClearCampaigns(1101);
            yield return LevelTo(6);
            foreach (var pool in new[] { new[] { 101, 102, 103, 104, 105, 106, 107, 108 }, new[] { 111, 112, 113, 114, 115, 116, 117, 118 }, new[] { 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211 }, new[] { 101, 102, 103, 104, 105, 106, 107, 108 } })
            {
                yield return A9aDive.Enter(pool);
                A9aDive.Scout($"{A9aDive.Campaign}");
                yield return Review.Shot($"a9-scout-{A9aDive.Campaign}");
                yield return A9aDive.Leave();
            }
            Flow.Log($"cheats used: {string.Join("; ", s_cheats.Concat(A9aDive.Cheats))}");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A7: lounge progress as a player sees it. From the post-tutorial profile (tools/fixtures/saves-post-tutorial, see
    // A3_NpcFlow), Continue into the lounge, then:
    // - the dive booth (PhoneBooth) with its Interact prompt (review key lounge-phonebooth), and F on it with no campaign
    //   running: InGameScene's key interaction says CannotEnterStage in a NoticeToast (a2-phonebooth-open);
    // - Elara's lounge quest 80200 (Narcis, Felix, Lucas, back to Elara) and campaign 1101 from her Campaign menu;
    // - the menu (Esc, InGame/Menu) opens on its Objectives tab (MenuPopupModel.PageCategory defaults to Quest): the
    //   campaign entry (quest-panel), then the entry clicked, with its detail pane (a7-quest-detail);
    // - saving: lounge quest progress does not mark PlayerContext dirty (only slots and the profile do, as in the binary),
    //   so the lounge autosave (PlayerManager.FixedUpdate, 60 s apart) does not write it. Return to Title (menu, System tab)
    //   reaches StartScene.EnsureAccountAsync, which saves the loaded player; Continue reads the profile back from
    //   <project>/SavesDir (Local platform) and must still have 80200 cleared. The campaign does not survive: closing the
    //   room ends it (GameCampaignManager.OnRoomClosed), so the Objectives tab is empty after Continue.
    public class A7_LoungeProgressFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const int LoungeQuest = 80200, CampaignId = 1101;

        static readonly List<string> s_errors = new List<string>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A7_LoungeProgressFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
        }

        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        // Exceptions and errors logged after the lounge is up, by step; the test fails on any.
        static string s_step = "boot";
        static void Collect(string message, string stackTrace, LogType type)
        {
            if (type != LogType.Exception && type != LogType.Error && type != LogType.Assert) return;
            lock (s_errors) s_errors.Add($"[{s_step}] {type}: {message.Split('\n')[0]}");
        }

        [TearDown]
        public void Restore()
        {
            Application.logMessageReceived -= Collect;
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static string SavesDir => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
        static string PlayerSave => Path.Combine(SavesDir, "PlayerContext_0.sav");
        static DateTime SaveTime => File.Exists(PlayerSave) ? File.GetLastWriteTimeUtc(PlayerSave) : DateTime.MinValue;

        [UnityTest, Timeout(1800000)]
        public IEnumerator LoungeProgress()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A7";
            s_errors.Clear();
            Flow.Log($"start platform={Flow.Platform()}");
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Application.logMessageReceived += Collect;
            Flow.RouteInputToGame();
            Flow.Log($"lounge {Progress()} save={SaveTime:HH:mm:ss}");

            s_step = "booth";
            yield return Booth();

            s_step = "quest 80200";
            yield return Flow.Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Flow.Talk(id, $"{who} 80200");
            yield return Flow.Talk(700000, "Elara 80200 finish");
            Flow.Log($"after 80200 {Progress()}");
            Assert.IsTrue(Cleared(LoungeQuest), $"lounge quest {LoungeQuest} not cleared: {Progress()}");

            s_step = "campaign";
            yield return StartCampaign();
            Assert.AreEqual(CampaignId.ToString(), CampaignNow(), "campaign 1101 did not start");

            s_step = "quest panel";
            yield return QuestPanel();

            s_step = "save";
            yield return SaveAndReturn();

            Flow.Log($"end errors={s_errors.Count}\n{string.Join("\n", s_errors)}");
            Assert.IsEmpty(s_errors, "errors logged: " + string.Join(" | ", s_errors.Take(5)));
        }

        // ---- the dive booth ----

        static IEnumerator Booth()
        {
            var booth = Flow.Find("PhoneBooth");
            Assert.IsNotNull(booth, "no PhoneBooth in the lounge");
            Flow.Log($"booth at {booth.transform.position} check={BoothCheck()} hold={Flow.Get(booth, "HoldingTime")}");
            yield return Flow.WalkTo(() => booth ? booth.transform.position : (Vector3?)null, 1.0f, 20f, "dive booth");
            for (int i = 0; i < 3 && !Flow.Focused(booth); i++)
            {
                var me = Flow.MyCharacter();
                var d = Flow.Flat(booth.transform.position - me.transform.position);
                if (d.magnitude > 0.8f) yield return Flow.Keys(0.2f, Flow.KeysToward(d));
                Flow.Aim(booth.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(booth), 2f, "focus booth");
            }
            Assert.IsTrue(Flow.Focused(booth), $"no interaction focus on the booth, focus={Flow.Focus()}");
            yield return new WaitForSecondsRealtime(1f);  // prompt tween, InputIcons device switch (0.2 s)
            Flow.Log($"booth prompt {Prompt()} me={Flow.MyCharacter().transform.position}");
            yield return Review.Shot("lounge-phonebooth");

            float hold = Flow.Get(booth, "HoldingTime") is float h ? h + 0.3f : 1.8f;
            Flow.Hold(Key.F);
            yield return Flow.WaitUntil(() => Flow.Find("NoticeToastView") != null || State() == "StageReady", hold + 1f, "booth reaction");
            Flow.Hold();
            yield return new WaitForSecondsRealtime(0.6f);  // toast open tween
            var toast = Flow.Find("NoticeToastView");
            Flow.Log($"booth F: state={State()} check={BoothCheck()} toast={(toast ? string.Join("|", toast.GetComponentsInChildren<TMPro.TMP_Text>().Select(t => t.text)) : "-")} top={Flow.TopPopup()} {Flow.PopupState()}");
            yield return Review.Shot("a2-phonebooth-open");
            Assert.AreNotEqual("StageReady", State(), "the booth took the diver in with no campaign running");
            Assert.IsNotNull(toast, "F on the booth with no campaign showed no notice");
            yield return new WaitForSecondsRealtime(3f);
        }

        static string Prompt()
        {
            var view = Flow.Find("InGameInteractionPromptView");
            if (view == null) return "view=-";
            var pivot = Flow.Get(view, "Pivot") as GameObject;
            return $"pivot={(pivot ? pivot.activeInHierarchy : false)} title='{Flow.Get(Flow.Get(view, "TitleText"), "text")}' desc='{Flow.Get(Flow.Get(view, "DescText"), "text")}' " +
                   $"key='{Flow.Get(Flow.Get(view, "HoldKeyText"), "text")}' keyboard={(Flow.Get(view, "Keyboard") as GameObject)?.activeInHierarchy}";
        }

        static string State() => Flow.Get(Flow.MyCharacter(), "State")?.ToString() ?? "-";

        static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { Flow.Get(gm, "LocalClientId") })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        // ---- campaign 1101 from Elara (as A5) ----

        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

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
            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "CampaignSlotPresenter" && Flow.Get(Flow.Get(m, "_model"), "CampaignId") is int id && id == CampaignId);
            if (slot == null) { Flow.Log($"no slot for campaign {CampaignId}"); yield break; }
            var slotButton = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, "campaign slot 1101");
            slotButton.onClick.Invoke();
            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == CampaignId, 6f, "campaign 1101 detail");
            if (detail == null) yield break;
            yield return new WaitForSecondsRealtime(1f);
            var start = Flow.Get(Flow.Get(detail, "_view"), "Info_StartCampaignButton") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == CampaignId.ToString(), 10f, "campaign started");
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1.5f);
            Flow.Log($"campaign={CampaignNow()} top={Flow.TopPopup()} {Flow.PopupState()}");
        }

        // ---- the quest panel ----

        // Esc in the lounge (InGame/Menu) opens the menu on its first tab, Objectives.
        static IEnumerator OpenMenu(string what)
        {
            for (int i = 0; i < 3 && Flow.Find("MenuPopupPresenter") == null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find("MenuPopupPresenter") != null, 4f, $"{what} menu");
            }
            yield return new WaitForSecondsRealtime(1.5f);  // open tween, scroller fill
        }

        static List<MonoBehaviour> QuestSlots() =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "QuestSlotPresenter").ToList();

        static string SlotText(Component s)
        {
            var v = Flow.Get(s, "_view");
            return $"{Flow.Get(Flow.Get(s, "_model"), "QuestType")}:{Flow.Get(Flow.Get(s, "_model"), "TableId")} '{Flow.Get(Flow.Get(v, "QuestTypeText"), "text")}' '{Flow.Get(Flow.Get(v, "QuestNameText"), "text")}'";
        }

        static string Page()
        {
            var menu = Flow.Find("MenuPopupPresenter");
            var page = Flow.Find("QuestPagePresenter");
            var view = Flow.Get(page, "_view");
            return $"category={Flow.Get(Flow.Get(menu, "_model"), "PageCategory")} questPage={(page != null)} empty={(Flow.Get(view, "Empty") as GameObject)?.activeInHierarchy} " +
                   $"info={(Flow.Get(view, "QuestInfoPanel") as Component)?.gameObject.activeInHierarchy} slots=[{string.Join(", ", QuestSlots().Select(SlotText))}]";
        }

        static IEnumerator QuestPanel()
        {
            yield return OpenMenu("quest panel");
            Assert.IsNotNull(Flow.Find("MenuPopupPresenter"), $"Esc did not open the menu, top={Flow.TopPopup()}");
            Flow.Log($"quest panel {Page()}");
            yield return Review.Shot("quest-panel");
            var slot = QuestSlots().OrderByDescending(s => s.transform.position.y).FirstOrDefault();
            Assert.IsNotNull(slot, "the Objectives tab lists no entry");
            var button = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, "quest slot");
            button.onClick.Invoke();
            yield return new WaitForSecondsRealtime(1.5f);
            var info = Flow.Find("QuestInfoPanelPresenter");
            Flow.Log($"quest detail {Page()} infoModel={Flow.Get(Flow.Get(info, "_model"), "QuestType")}:{Flow.Get(Flow.Get(info, "_model"), "TableId")} " +
                     $"texts=[{(info ? string.Join("|", info.GetComponentsInChildren<TMPro.TMP_Text>().Where(t => t.isActiveAndEnabled && !string.IsNullOrWhiteSpace(t.text)).Select(t => t.text.Replace("\n", " "))) : "-")}]");
            yield return Review.Shot("a7-quest-detail");
            Assert.IsNotNull(info, "no detail pane after selecting the entry");
            // Esc leaves the selection, the next Esc closes the menu.
            for (int i = 0; i < 3 && Flow.Find("MenuPopupPresenter") != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(0.8f);
            }
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 5f, "menu closed");
        }

        // ---- saving ----

        static object PlayerManager() => Flow.Find("PlayerManager");

        static bool Cleared(int questId)
        {
            var pm = PlayerManager();
            try { return pm != null && (bool)pm.GetType().GetMethod("IsClearedLoungeQuest").Invoke(pm, new object[] { questId }); }
            catch (Exception) { return false; }
        }

        static string Progress()
        {
            var pm = PlayerManager();
            var ctx = Flow.Get(pm, "_playerContext");
            return $"cleared{LoungeQuest}={Cleared(LoungeQuest)} campaign={CampaignNow()} dirty={Flow.Get(ctx, "HasDirty")} gold={Flow.Get(Flow.Value(ctx, "RxProfile"), "Gold")} " +
                   $"lastSave={Flow.Get(pm, "_lastSaveTime")}";
        }

        // What the profile file on disk says about the quest (the Local platform also writes PlayerContext_0.json).
        static string OnDisk()
        {
            string json = Path.Combine(SavesDir, "PlayerContext_0.json");
            if (!File.Exists(json)) return "no json";
            string s = File.ReadAllText(json);
            int i = s.IndexOf("\"CompletedLoungeQuestFlags\"", StringComparison.Ordinal);
            int q = s.IndexOf($"\"Id\": {LoungeQuest},", StringComparison.Ordinal);
            if (q < 0) q = s.IndexOf($"\"Id\":{LoungeQuest},", StringComparison.Ordinal);
            return $"{(i >= 0 ? s.Substring(i, Math.Min(60, s.Length - i)) : "no flags")} quest80200={(q >= 0 ? s.Substring(q, Math.Min(200, s.Length - q)).Replace("\n", " ") : "absent")}";
        }

        static IEnumerator SaveAndReturn()
        {
            Flow.Log($"before title file={SaveTime:HH:mm:ss.fff} {Progress()} disk: {OnDisk()}");

            // Menu, System tab, Return to Title, confirm.
            yield return OpenMenu("return to title");
            var tab = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "MenuTabPresenter" && Flow.Get(Flow.Get(m, "_model"), "PageCategory")?.ToString() == "System");
            Assert.IsNotNull(tab, "no System tab in the menu");
            yield return Flow.WaitUntil(() => Flow.Ready(Flow.Get(tab, "Button") as UnityEngine.UI.Button), 5f, "System tab");
            (Flow.Get(tab, "Button") as UnityEngine.UI.Button).onClick.Invoke();
            MonoBehaviour system = null;
            yield return Flow.WaitUntil(() => (system = Flow.Find("SystemPageView")) != null, 5f, "System page");
            Assert.IsNotNull(system, "System page did not open");
            yield return new WaitForSecondsRealtime(1f);
            yield return Flow.Click(system, "ReturnToTitleButton");
            yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") != null && Flow.Ready(Flow.Button(Flow.Find("TextPopupView"), "Confirm_Button")), 5f, "return confirmation");
            DateTime beforeTitle = DateTime.UtcNow.AddSeconds(-1);
            yield return Flow.Click(Flow.Find("TextPopupView"), "Confirm_Button");

            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title after Return to Title");
            Assert.IsTrue(Flow.LastWaitOk, $"Return to Title did not reach the title, scene={Flow.ActiveScene}");
            yield return new WaitForSecondsRealtime(2f);
            Flow.Log($"title play='{Flow.Get(Flow.Get(title, "Play_Button_Text"), "text")}' slot='{Flow.Get(Flow.Get(title, "SelectSlot_Player_Name"), "text")}' " +
                     $"saved={SaveTime > beforeTitle} file={SaveTime:HH:mm:ss.fff} disk: {OnDisk()}");
            Assert.IsTrue(SaveTime > beforeTitle, "the title did not save the profile after Return to Title");

            // Continue from the title; the profile is read back from SavesDir.
            yield return Flow.ContinueToLounge(loadTitle: false);
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue after Return to Title did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();
            Flow.Log($"continued {Progress()} disk: {OnDisk()}");
            Assert.IsTrue(Cleared(LoungeQuest), $"lounge quest {LoungeQuest} lost after Return to Title and Continue: {Progress()} disk: {OnDisk()}");

            // The Objectives tab after Continue (what the player checks).
            yield return OpenMenu("after continue");
            Flow.Log($"quest panel after continue {Page()}");
            yield return Review.Shot("a7-quest-panel-continued");
            yield return Flow.Keys(0.2f, Key.Escape);
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 5f, "menu closed");
        }
    }
}
